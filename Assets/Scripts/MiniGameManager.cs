using System;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
	public enum MiniGameNames
	{
		Rooftop,
		EvilInterior,
		HangGliding,
		RoofDoofenCruiser,
		Balloony
	}

	public enum BossType
	{
		DoofenCruiser,
		Balloony
	}

	[Serializable]
	public class MiniGameMap
	{
		[Serializable]
		public class ToSequence
		{
			public PlatformSequence m_TransitionSequence;

			public Platform m_TransitionPlatform;
		}

		public ToSequence[] m_To;
	}

	public Platform m_TutorialStartPlatform;

	public Platform m_MiniGameStartPlatform;

	public Platform m_BabyHeadStartPlatform;

	[HideInInspector]
	public float[] m_DistanceToNextMiniGameMin;

	[HideInInspector]
	public float[] m_DistanceToNextMiniGameMax;

	private float m_DistanceToNextMiniGame = 10000f;

	public float m_EagleSpawnDisallowDistance = 200f;

	[HideInInspector]
	public MiniGameMap[] m_MiniGameMap;

	[HideInInspector]
	public MiniGameMap m_BabyHeadToRoof;

	[HideInInspector]
	public MiniGameMap m_RoofDuckyToOthers;

	private static MiniGameManager m_The;

	private int m_MiniGamePrev;

	private int m_MiniGameShuffleIndex;

	private int[] m_MiniGameShuffles = new int[5] { 0, 1, 2, 3, 4 };

	private int m_MiniGameCount;

	public BabyAndDuckyRules m_BabyHeadRules = new BabyAndDuckyRules(BabyAndDuckyRules.Type.Baby, 3);

	public BabyAndDuckyRules m_DuckyMomoRules = new BabyAndDuckyRules(BabyAndDuckyRules.Type.Ducky, 10);

	private float m_DistanceAtLastMiniGameChange;

	private bool m_WaitingForNewScenePlatform;

	private Platform m_PlatformWaitingFor;

	public static MiniGameManager The()
	{
		if (m_The == null)
		{
			m_The = GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>();
		}
		return m_The;
	}

	private void Awake()
	{
		m_The = this;
		DebugDoofenCruiser();
		m_MiniGameCount = Enum.GetNames(typeof(MiniGameNames)).Length;
		GameEventManager.GameStart += GameStartListener;
		GameEventManager.BossMoveToNextMiniGameEvents += BossMoveToNextMiniGameListener;
		GameEventManager.MoveToNextPlatformEvents += MoveToNextPlatformListener;
		m_BabyHeadRules.ResetCalculationsAll();
		m_DuckyMomoRules.ResetCalculationsAll();
	}

	private void OnDestroy()
	{
		GameEventManager.GameStart -= GameStartListener;
		GameEventManager.BossMoveToNextMiniGameEvents -= BossMoveToNextMiniGameListener;
		GameEventManager.MoveToNextPlatformEvents -= MoveToNextPlatformListener;
	}

	private void GameStartListener()
	{
		ResetMiniGameManager();
	}

	private void ResetMiniGameManager()
	{
		m_DistanceAtLastMiniGameChange = 0f;
		m_MiniGameShuffleIndex = 0;
		ResetMiniGameShuffles();
		MathOps.Shuffle(m_MiniGameShuffles, 1);
		CalcDistanceToNextMiniGame();
		m_WaitingForNewScenePlatform = false;
	}

	private void DebugDoofenCruiser()
	{
	}

	private void ResetMiniGameShuffles()
	{
		for (int i = 0; i < m_MiniGameShuffles.Length; i++)
		{
			m_MiniGameShuffles[i] = i;
		}
	}

	public void MoveToDoofenCruiser()
	{
		ResetMiniGameShuffles();
		int num = (m_MiniGamePrev = m_MiniGameShuffles[m_MiniGameShuffleIndex]);
		MiniGameMap.ToSequence toSequence = m_MiniGameMap[num].m_To[3];
		m_MiniGameShuffleIndex = 3;
		TriggerNextToSequence(toSequence);
	}

	public void MoveToBalloony()
	{
		ResetMiniGameShuffles();
		int num = (m_MiniGamePrev = m_MiniGameShuffles[m_MiniGameShuffleIndex]);
		MiniGameMap.ToSequence toSequence = m_MiniGameMap[num].m_To[4];
		m_MiniGameShuffleIndex = 4;
		TriggerNextToSequence(toSequence);
	}

	private void DebugShuffles(string skippedBossOrNot)
	{
		for (int i = 0; i < m_MiniGameShuffles.Length; i++)
		{
		}
	}

	private void ShuffleMiniGamesOnSkippedBoss()
	{
		if (m_MiniGameShuffleIndex >= m_MiniGameShuffles.Length)
		{
			m_MiniGameShuffleIndex = 0;
			MathOps.ShuffleSecondToLastCantBeFirst(m_MiniGameShuffles);
		}
	}

	public void MoveToNextMiniGame()
	{
		int num = (m_MiniGamePrev = m_MiniGameShuffles[m_MiniGameShuffleIndex]);
		m_MiniGameShuffleIndex++;
		if (m_MiniGameShuffleIndex >= m_MiniGameShuffles.Length)
		{
			m_MiniGameShuffleIndex = 0;
			MathOps.ShuffleLastCantBeFirst(m_MiniGameShuffles);
		}
		if (m_MiniGamePrev == 3 || m_MiniGamePrev == 4)
		{
			while (RetrieveMiniGameCur() == MiniGameNames.RoofDoofenCruiser || RetrieveMiniGameCur() == MiniGameNames.Balloony)
			{
				m_MiniGameShuffleIndex++;
				if (m_MiniGameShuffleIndex >= m_MiniGameShuffles.Length)
				{
					m_MiniGameShuffleIndex = 0;
				}
			}
		}
		if (RetrieveMiniGameCur() == MiniGameNames.Balloony)
		{
			if (m_MiniGameShuffleIndex != m_MiniGameShuffles.Length - 1)
			{
				m_MiniGameShuffleIndex++;
			}
			else
			{
				m_MiniGameShuffleIndex = 0;
				MathOps.ShuffleLastCantBeFirst(m_MiniGameShuffles);
			}
		}
		if (RetrieveMiniGameCur() == MiniGameNames.RoofDoofenCruiser && PlayerData.RoundBossDefeats > 0 && PlayerData.RoundBossEncounters % 4 == 3)
		{
			while (RetrieveMiniGameCur() != MiniGameNames.Balloony)
			{
				m_MiniGameShuffleIndex++;
				if (m_MiniGameShuffleIndex >= m_MiniGameShuffles.Length)
				{
					m_MiniGameShuffleIndex = 0;
					MathOps.ShuffleLastCantBeFirst(m_MiniGameShuffles);
				}
			}
		}
		int num2 = m_MiniGameShuffles[m_MiniGameShuffleIndex];
		Debug.Log("MiniGameManager.MoveToNextMiniGame() : chosen: " + (MiniGameNames)num2);
		MiniGameMap.ToSequence toSequence = m_MiniGameMap[num].m_To[num2];
		if (num2 == 0)
		{
			if (m_BabyHeadRules.ShouldSpawn())
			{
				toSequence = m_BabyHeadToRoof.m_To[num];
			}
		}
		else if (num == 0 && m_DuckyMomoRules.ShouldSpawn())
		{
			toSequence = m_RoofDuckyToOthers.m_To[num2];
		}
		TriggerNextToSequence(toSequence);
	}

	private void MoveToMiniGame(MiniGameNames miniGameNext)
	{
		int num = (m_MiniGamePrev = m_MiniGameShuffles[m_MiniGameShuffleIndex]);
		MiniGameMap.ToSequence toSequence = m_MiniGameMap[num].m_To[(int)miniGameNext];
		TriggerNextToSequence(toSequence);
	}

	public void MoveToMiniGameDebug(MiniGameNames miniGameCur, MiniGameNames miniGameNext)
	{
		m_MiniGamePrev = (int)miniGameCur;
		MiniGameMap.ToSequence toSequence = m_MiniGameMap[(int)miniGameCur].m_To[(int)miniGameNext];
		TriggerNextToSequence(toSequence);
	}

	private void TriggerNextToSequence(MiniGameMap.ToSequence toSequence)
	{
		if (toSequence.m_TransitionSequence != null)
		{
			GameEventManager.TriggerForcePlatformSequence(toSequence.m_TransitionSequence.m_PlatformSequence);
			if (toSequence.m_TransitionSequence.m_PlatformSequence != null && toSequence.m_TransitionSequence.m_PlatformSequence.Length > 0)
			{
				m_PlatformWaitingFor = toSequence.m_TransitionSequence.m_PlatformSequence[0].m_Platform;
			}
			ResetMoveToMiniGame();
		}
		else if (toSequence.m_TransitionPlatform != null)
		{
			GameEventManager.TriggerForceNextPlatform(toSequence.m_TransitionPlatform);
			m_PlatformWaitingFor = toSequence.m_TransitionPlatform;
			ResetMoveToMiniGame();
		}
		else
		{
			m_WaitingForNewScenePlatform = false;
		}
	}

	private void ResetMoveToMiniGame()
	{
		m_WaitingForNewScenePlatform = true;
		PlayerData.RoundScenesChanged++;
	}

	private void MoveToNextPlatformListener(Platform platform)
	{
		if (m_WaitingForNewScenePlatform)
		{
			string strA = platform.name.Replace("(Clone)", string.Empty);
			if (m_PlatformWaitingFor != null && string.Compare(strA, m_PlatformWaitingFor.name) == 0)
			{
				HitNewPlatformHandler();
				m_WaitingForNewScenePlatform = false;
			}
		}
	}

	public void HitNewPlatformHandler()
	{
		DuckyMomo.TriggerCutOffDucky();
		if (Runner.The() != null)
		{
			Runner.The().TriggerPowerUpFeatherCutOff();
		}
		m_DistanceAtLastMiniGameChange = Runner.The().m_Distance;
		CalcDistanceToNextMiniGame();
		PlayMusicForNextMiniGame();
		if (m_MiniGamePrev == 0)
		{
			m_DuckyMomoRules.m_RoofPlayedCount++;
			m_DuckyMomoRules.ReCalcShouldSpawn();
		}
	}

	private void UpdateMoveToNextMiniGame()
	{
		if (!(Runner.The() != null) || m_WaitingForNewScenePlatform)
		{
			return;
		}
		float num = CalcDistanceThisMiniGame();
		if (num > m_DistanceToNextMiniGame)
		{
			if (RetrieveMiniGameCur() == MiniGameNames.Rooftop)
			{
				m_BabyHeadRules.m_RoofPlayedCount++;
				m_BabyHeadRules.ReCalcShouldSpawn();
			}
			Debug.Log("MiniGameManager.UpdateMoveToNextMiniGame(): selecting next mini game, distanceThisMiniGame: " + num);
			MoveToNextMiniGame();
		}
	}

	public float CalcDistanceThisMiniGame()
	{
		return Runner.The().m_Distance - m_DistanceAtLastMiniGameChange;
	}

	private void Update()
	{
		if ((!(GameManager.The != null) || GameManager.The.IsInGamePlay()) && (!(Runner.The() != null) || !Runner.The().IsInEagleState()) && (!(Runner.The() != null) || !Runner.The().IsInCopterBoostState()) && (!(PlatformManager.The() != null) || !PlatformManager.The().IsForking()))
		{
			MiniGameNames miniGameNames = RetrieveMiniGameCur();
			if (miniGameNames != MiniGameNames.RoofDoofenCruiser && miniGameNames != MiniGameNames.Balloony)
			{
				UpdateMoveToNextMiniGame();
			}
		}
	}

	public MiniGameNames RetrieveMiniGameCur()
	{
		return (MiniGameNames)m_MiniGameShuffles[m_MiniGameShuffleIndex];
	}

	public int RetrieveMiniGameCount()
	{
		return m_MiniGameCount;
	}

	private void CalcDistanceToNextMiniGame()
	{
		int num = (int)RetrieveMiniGameCur();
		m_DistanceToNextMiniGame = UnityEngine.Random.Range(m_DistanceToNextMiniGameMin[num], m_DistanceToNextMiniGameMax[num]);
	}

	public float CalcDistanceToNextMiniGameLeft()
	{
		return m_DistanceToNextMiniGame - CalcDistanceThisMiniGame();
	}

	private void PlayMusicForNextMiniGame()
	{
		if (!(GameManager.The == null))
		{
			switch (RetrieveMiniGameCur())
			{
			case MiniGameNames.Rooftop:
				GameManager.The.PlayMusic(AudioClipFiles.ROOFTOPTHEME);
				break;
			case MiniGameNames.EvilInterior:
				GameManager.The.PlayClip(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.LAIRINTROSTING);
				GameManager.The.PlayMusic(AudioClipFiles.LAIRTHEME);
				break;
			case MiniGameNames.HangGliding:
				GameManager.The.PlayMusic(AudioClipFiles.HANGLIDERTHEME);
				break;
			case MiniGameNames.RoofDoofenCruiser:
				GameManager.The.PlayMusic(AudioClipFiles.ROOFTOPTHEME);
				break;
			case MiniGameNames.Balloony:
				GameManager.The.PlayMusic(AudioClipFiles.ROOFTOPTHEME);
				break;
			}
		}
	}

	private void BossMoveToNextMiniGameListener()
	{
		MoveToNextMiniGame();
	}

	public bool CanSpawnEaglePowerUp()
	{
		if (RetrieveMiniGameCur() != MiniGameNames.Rooftop)
		{
			return false;
		}
		float num = CalcDistanceThisMiniGame();
		if (m_DistanceToNextMiniGame - num < m_EagleSpawnDisallowDistance)
		{
			return false;
		}
		return true;
	}

	public Platform ChooseStartPlatform()
	{
		PlayerData.ShouldNotShowTutorial = true;
		if (!PlayerData.ShouldNotShowTutorial)
		{
			return m_TutorialStartPlatform;
		}
		if (m_BabyHeadRules.ShouldSpawn())
		{
			return m_BabyHeadStartPlatform;
		}
		return m_MiniGameStartPlatform;
	}
}

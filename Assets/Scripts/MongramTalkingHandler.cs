using UnityEngine;

public class MongramTalkingHandler : MonoBehaviour
{
	public enum MonogramTalkingState
	{
		MainMenuMissionsIntro = 0,
		GamePlay_TubeIntro = 1,
		GamePlay_TubeIntroFirstTime = 2,
		GreatJob = 3,
		OnLaunch = 4
	}

	private static MongramTalkingHandler m_the;

	public MonogramTalkingState m_currentState;

	private bool m_isTalking;

	private bool m_isTexting;

	private string m_currentMissionMenuAudioClip = string.Empty;

	public static MongramTalkingHandler The
	{
		get
		{
			if (m_the == null)
			{
				GameObject gameObject = new GameObject("MonogramGUIManager");
				((MonogramGUIManager)gameObject.AddComponent<MonogramGUIManager>()).Init();
			}
			return m_the;
		}
	}

	private void OnDestroy()
	{
		MainMenuEventManager.StartMonogramTalking -= HandleStartMonogramTalking;
		MainMenuEventManager.StopMonogramTalking -= HandleStopMonogramTalking;
		MainMenuEventManager.GoToNextMenu -= HandleGoToNextMenu;
	}

	private void Awake()
	{
		m_the = this;
		MainMenuEventManager.StartMonogramTalking += HandleStartMonogramTalking;
		MainMenuEventManager.StopMonogramTalking += HandleStopMonogramTalking;
		MainMenuEventManager.GoToNextMenu += HandleGoToNextMenu;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void HandleStartMonogramTalking(MonogramTalkingState state)
	{
		m_isTexting = false;
		switch (state)
		{
		case MonogramTalkingState.MainMenuMissionsIntro:
			PlayMainMenuMissionsIntroAudio();
			break;
		case MonogramTalkingState.GamePlay_TubeIntro:
			PlayTubeIntroText();
			break;
		case MonogramTalkingState.GamePlay_TubeIntroFirstTime:
			Invoke("PlayTubeIntroFirstTimeText", 1f);
			Invoke("PlayTubeIntroFirstTimeAudio", 1f);
			break;
		case MonogramTalkingState.OnLaunch:
			PlayOnLaunchStory();
			break;
		case MonogramTalkingState.GreatJob:
			PlayGreatJobAudio();
			break;
		}
	}

	public void HandleStopMonogramTalking(MonogramTalkingState state)
	{
		if (state == MonogramTalkingState.GamePlay_TubeIntroFirstTime || state == MonogramTalkingState.GamePlay_TubeIntro)
		{
			if (MonogramGUIManager.The != null)
			{
				MonogramGUIManager.The.HideAll();
			}
			if (Runner.The() != null)
			{
				Runner.The().StopTubeIntro();
			}
		}
		m_isTalking = false;
		m_isTexting = false;
	}

	private void HandleGoToNextMenu(MainMenuEventManager.MenuState menuState)
	{
		if (menuState != MainMenuEventManager.MenuState.InGame_Missions_Menu && m_isTalking)
		{
			GameManager.The.StopClip(m_currentMissionMenuAudioClip);
			if (IsInvoking("StopPlayAudio"))
			{
				CancelInvoke("StopPlayAudio");
			}
			StopPlayAudio();
		}
	}

	private void PlayMainMenuMissionsIntroAudio()
	{
		m_currentState = MonogramTalkingState.MainMenuMissionsIntro;
		if (!m_isTalking)
		{
			m_currentMissionMenuAudioClip = AudioClipFiles.MONOGRAMMISSIONINTRO + Random.Range(1, AudioClipFiles.NUMBERMONOGRAMMISSIONINTRO + 1);
			float num = GameManager.The.PlayClip(m_currentMissionMenuAudioClip);
			if (num != -1f)
			{
				m_isTalking = true;
				Invoke("StopPlayAudio", num);
			}
		}
	}

	private void PlayTubeIntroAudio()
	{
		m_currentState = MonogramTalkingState.GamePlay_TubeIntro;
		if (!m_isTalking)
		{
			string currentCharacterName = PlayerData.CurrentCharacterName;
			currentCharacterName = "generic";
			string clip = AudioClipFiles.MONOGRAMGAMEINTROCHAR + currentCharacterName + Random.Range(1, AudioClipFiles.NUMBERMONOGRAMGAMEINTRO + 1);
			float num = GameManager.The.PlayClip(clip);
			if (num != -1f)
			{
				m_isTalking = true;
				Invoke("StopPlayAudio", num);
			}
		}
	}

	private void PlayTubeIntroFirstTimeAudio()
	{
		m_currentState = MonogramTalkingState.GamePlay_TubeIntroFirstTime;
		if (!m_isTalking)
		{
			string mONOGRAMGAMEINTROFIRSTTIME = AudioClipFiles.MONOGRAMGAMEINTROFIRSTTIME;
			float num = GameManager.The.PlayClip(mONOGRAMGAMEINTROFIRSTTIME);
			if (num != -1f)
			{
				Invoke("StopPlayAudio", num);
				m_isTalking = true;
			}
		}
	}

	private void StopPlayAudio()
	{
		m_isTalking = false;
		if (!m_isTexting)
		{
			MainMenuEventManager.TriggerStopMonogramTalking(m_currentState);
		}
	}

	private void PlayTubeIntroText()
	{
		m_isTexting = true;
		string text = PlayerData.CurrentCharacterName.ToLower();
		if (text.Contains("perry"))
		{
			text = "perry";
		}
		else if (text.Contains("pinky"))
		{
			text = "pinky";
		}
		else if (text.Contains("peter"))
		{
			text = "perry";
		}
		else if (text.Contains("pinky"))
		{
			text = "pinky";
		}
		else if (text.Contains("terry"))
		{
			text = "terry";
		}
		text = text.ToUpper();
		string key = LocalTextFiles.MONOGRAMTIP + Random.Range(1, LocalTextFiles.NUMBERMONOGRAMTIP + 1) + "_";
		string monogramText = LocalTextManager.GetMonogramText(key);
		MonogramGUIManager.The.StartTalkBoxGameIntroPos(monogramText);
	}

	private void PlayTubeIntroFirstTimeText()
	{
		m_isTexting = true;
	}

	public void PlayOnLaunchStory()
	{
		m_isTexting = true;
		string key = LocalTextFiles.MONOGRAMSTORY + Random.Range(1, LocalTextFiles.NUMBERMONOGRAMSTORY + 1) + "_";
		string monogramText = LocalTextManager.GetMonogramText(key);
		MonogramGUIManager.The.StartTalkBox(monogramText);
		m_currentState = MonogramTalkingState.OnLaunch;
	}

	public void StopPlayTextWithDelay(float delay)
	{
		Invoke("StopPlayText", delay);
	}

	private void StopPlayText()
	{
		m_isTexting = false;
		if (!m_isTalking)
		{
			PlayTubeIntroAudio();
			MainMenuEventManager.TriggerStopMonogramTalking(m_currentState);
		}
	}

	private void OnTubeIntroFirstTime()
	{
		MainMenuEventManager.TriggerStartMonogramTalking(MonogramTalkingState.GamePlay_TubeIntroFirstTime);
	}

	private void PlayGreatJobAudio()
	{
		m_currentState = MonogramTalkingState.GreatJob;
		if (!m_isTalking)
		{
			string text = PlayerData.CurrentCharacterName;
			if (text.Contains("perry"))
			{
				text = "perry";
			}
			else if (text.Contains("pinky"))
			{
				text = "pinky";
			}
			else if (text.Contains("peter"))
			{
				text = "perry";
			}
			else if (text.Contains("pinky"))
			{
				text = "pinky";
			}
			else if (text.Contains("terry"))
			{
				text = "terry";
			}
			int num = Random.Range(0, 1);
			if (num > 3)
			{
				text = "generic";
			}
			string clip = AudioClipFiles.MONOGRAMGREATJOB + text + Random.Range(1, AudioClipFiles.NUMBERMONOGRAMGREATJOB + 1);
			float num2 = GameManager.The.PlayClip(clip);
			if (num2 != -1f)
			{
				m_isTalking = true;
				Invoke("StopPlayAudio", num2);
			}
		}
	}
}

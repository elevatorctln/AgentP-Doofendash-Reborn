using System;
using UnityEngine;

public class Balloony : PathStateMachine
{
	public enum BossState
	{
		Entering = 0,
		PauseAfterEnter = 1,
		MoveTowardsPlayer = 2,
		CanBeTargeted = 3,
		Dying = 4,
		FlyingAwayAlive = 5,
		None = 6
	}

	private const float DefaultHealth = 1f;

	private const float CanBeTargetedTime = 4f;

	private const float StartingHealth = 1f;

	private const float PuaseTimeAfterEnter = 1f;

	public UnityEngine.Object particleSystemFlashPrefab;

	public UnityEngine.Object particleSystemBitsPrefab;

	[HideInInspector]
	public BossState bossState = BossState.None;

	private bool isParticleSpawned;

	private ParticleSystem particleSystemFlash;

	private ParticleSystem particleSystemBits;

	private StopWatch phaseStopWatch;

	private float health = 1f;

	private StopWatch targetActiveStopWatch;

	private string timeAttackBonusString = "Time Attack Bonus";

	private Vector3 originalStartingPosition;

	private GameObject mesh;

	private GameObject targetToShootAtGo;

	private bool isFlyAwaySoundPlayed;

	private bool isBalloonyIntroSoundPlayed;

	private static Balloony instance;

	public static Balloony The
	{
		get
		{
			if (instance == null)
			{
				GameObject gameObject = GameObject.Find("Balloony");
				if (gameObject != null)
				{
					instance = gameObject.GetComponent<Balloony>();
				}
			}
			return instance;
		}
	}

	public float GetAnimationSpeed(string animName)
	{
		return AnimationOps.GetAnimationSpeed(base.GetComponent<Animation>(), animName);
	}

	private void Awake()
	{
		instance = this;
		Init();
		GameEventManager.BossStartEvents += BossStartListener;
		GameEventManager.DamageBoss += DamageBossListener;
		GameEventManager.GameRestartMenu += GameRestartMenuListener;
		bossState = BossState.None;
		originalStartingPosition = base.transform.position;
		mesh = base.transform.Find("BalloonyMesh").gameObject;
		targetToShootAtGo = base.transform.Find("BalloonyMesh/TargetToShootAt").gameObject;
		targetToShootAtGo.GetComponent<Animation>().Stop();
		base.gameObject.SetActive(false);
	}

	private void GameOverListener()
	{
		Debug.Log("Ballony.GameOverListener()");
		if (bossState != BossState.None)
		{
			Reset();
			GameEventManager.TriggerBossEnd(MiniGameManager.BossType.Balloony);
		}
	}

	private void Start()
	{
		AssetGPULoader.The().PreLoadObject(base.gameObject);
		targetActiveStopWatch = new StopWatch();
		phaseStopWatch = new StopWatch();
		phaseStopWatch.Start();
		string uIText = LocalTextManager.GetUIText("_ATTACK_BONUS_");
		if (uIText != null && uIText != "_ATTACK_BONUS_")
		{
			timeAttackBonusString = uIText;
		}
		SpawnParticles();
		DisableParticles();
	}

	private void OnDestroy()
	{
		GameEventManager.BossStartEvents -= BossStartListener;
		GameEventManager.DamageBoss -= DamageBossListener;
		GameEventManager.GameRestartMenu -= GameRestartMenuListener;
	}

	private void Reset()
	{
		health = 1f;
		mesh.SetActive(true);
		bossState = BossState.None;
		if (phaseStopWatch != null)
		{
			phaseStopWatch.Stop();
		}
		targetToShootAtGo.GetComponent<Animation>().Stop();
		targetToShootAtGo.GetComponent<Animation>()["BalloonyShowTarget"].speed = 1f;
		targetToShootAtGo.SetActive(false);
		mesh.GetComponent<Animation>().Stop();
		isFlyAwaySoundPlayed = false;
		isBalloonyIntroSoundPlayed = false;
		SetOffsetToPerry(originalStartingPosition);
		base.transform.position = originalStartingPosition;
		base.gameObject.SetActive(false);
	}

	public bool IsActive()
	{
		return bossState != BossState.None;
	}

	private void BossStartListener(MiniGameManager.BossType bossType)
	{
		if (bossType == MiniGameManager.BossType.Balloony && bossState == BossState.None)
		{
			base.gameObject.SetActive(true);
			SetRotationState(BossRotationState.NoRotation);
			base.transform.rotation = Runner.The().transform.rotation;
			bossState = BossState.Entering;
			StartOnPath("EnterPath");
			PlayerData.RoundBossEncounters++;
			PlayerData.RoundBalloonyEncounterCount++;
			mesh.GetComponent<Animation>()["BalloonyIdle"].wrapMode = WrapMode.Loop;
			mesh.GetComponent<Animation>().Rewind("BalloonyIdle");
			mesh.GetComponent<Animation>().Play("BalloonyIdle");
			Animation animation = mesh.transform.Find("Rope").GetComponent<Animation>();
			animation["RopeSwing"].wrapMode = WrapMode.Loop;
			animation.Rewind("RopeSwing");
			animation.Play("RopeSwing");
		}
	}

	private void GameRestartMenuListener()
	{
		Debug.Log("Balloony.GameRestartMenuListener()");
		if (bossState != BossState.None)
		{
			Reset();
			GameEventManager.TriggerBossEnd(MiniGameManager.BossType.Balloony);
		}
	}

	private void UpdatePhase()
	{
		if (bossState == BossState.None)
		{
			return;
		}
		switch (bossState)
		{
		case BossState.Entering:
			if (!isBalloonyIntroSoundPlayed && phaseStopWatch.RetrieveTimeElapsed() > 2f)
			{
				GameManager.The.PlayRandomClip(AudioClipFiles.BossBalloonyAttackTaunt);
				isBalloonyIntroSoundPlayed = true;
			}
			if (base.CurPath.IsPathCompleted())
			{
				bossState = BossState.PauseAfterEnter;
				phaseStopWatch.Start();
				RecalcOffsetToPerry();
			}
			break;
		case BossState.PauseAfterEnter:
			if (phaseStopWatch.RetrieveTimeElapsed() > 1f)
			{
				bossState = BossState.MoveTowardsPlayer;
				phaseStopWatch.Start();
				StartOnPath("MoveTowardsPlayerPath");
				HUDGUIManager.The.BossAttackStart();
			}
			break;
		case BossState.MoveTowardsPlayer:
			if (base.CurPath.IsPathCompleted())
			{
				bossState = BossState.CanBeTargeted;
				phaseStopWatch.Start();
				RecalcOffsetToPerry();
				targetToShootAtGo.gameObject.SetActive(true);
				targetToShootAtGo.GetComponent<Animation>().Rewind("BalloonyShowTarget");
				targetToShootAtGo.GetComponent<Animation>().Play("BalloonyShowTarget");
				targetActiveStopWatch.Start();
				if (PlayerData.HasPinWeapon)
				{
					GameEventManager.TriggerBossTargetVisible(DoofenCruiser.WeaponType.Pin);
					HUDGUIManager.The.ShowBossGUI();
				}
				Runner.The().PlayFireTutorial(MiniGameManager.BossType.Balloony);
				GameManager.The.PlayRandomClip(AudioClipFiles.BossBalloonyDoesNothing);
			}
			break;
		case BossState.CanBeTargeted:
			if (!targetToShootAtGo.GetComponent<Animation>().isPlaying)
			{
				if (health > 0f && phaseStopWatch.RetrieveTimeElapsed() > 4f)
				{
					bossState = BossState.FlyingAwayAlive;
					phaseStopWatch.Start();
					targetActiveStopWatch.Pause();
					targetToShootAtGo.GetComponent<Animation>()["BalloonyShowTarget"].speed = -1f;
					targetToShootAtGo.GetComponent<Animation>()["BalloonyShowTarget"].normalizedTime = 1f;
					targetToShootAtGo.GetComponent<Animation>().Play("BalloonyShowTarget");
					StartOnPath("ExitPath");
					HUDGUIManager.The.EndBossFightGUI();
					GameEventManager.TriggerBossTargetInvisible(DoofenCruiser.WeaponType.Pin);
				}
				else if (health <= 0f)
				{
					bossState = BossState.Dying;
					phaseStopWatch.Start();
					targetToShootAtGo.SetActive(false);
					targetActiveStopWatch.Pause();
					PlayExplosionEffect();
					mesh.SetActive(false);
					GameManager.The.PlayRandomClip(AudioClipFiles.BossBalloonyExplosion);
					HUDGUIManager.The.EndBossFightGUI();
					GameEventManager.TriggerBossTargetInvisible(DoofenCruiser.WeaponType.Pin);
				}
			}
			break;
		case BossState.FlyingAwayAlive:
			if (!isFlyAwaySoundPlayed && phaseStopWatch.RetrieveTimeElapsed() > 1f)
			{
				GameManager.The.PlayRandomClip(AudioClipFiles.BossBalloonyFliesAwaySafely);
				isFlyAwaySoundPlayed = true;
			}
			if (base.CurPath.IsPathCompleted())
			{
				GameEventManager.TriggerBossEnd(MiniGameManager.BossType.Balloony);
				GameEventManager.TriggerBossMoveToNextMiniGame();
				Reset();
			}
			break;
		case BossState.Dying:
			if ((!particleSystemBits.isPlaying && !particleSystemFlash.isPlaying) || phaseStopWatch.RetrieveTimeElapsed() > 2f)
			{
				DisableParticles();
				DisplayAndUpdateBossScore();
				PlayerData.RoundBossDefeats++;
				PlayerData.AllTimeBossDefeats++;
				GameEventManager.TriggerBossEnd(MiniGameManager.BossType.Balloony);
				GameEventManager.TriggerBossMoveToNextMiniGame();
				Reset();
				GameManager.The.PlayRandomClip(AudioClipFiles.BossBalloonyDestroyed);
			}
			break;
		case BossState.None:
			Debug.LogError("Balloony.UpdatePhase(): Update shouldn't be called when no state is set.");
			Reset();
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		if (Runner.The().GetTutorialState() != Runner.TutorialState.PauseWaitForFire)
		{
			phaseStopWatch.Update();
			targetActiveStopWatch.Update();
		}
	}

	private void Update()
	{
		if (!(GameManager.The == null) && !GameManager.The.IsGamePaused() && GameManager.The.IsInGamePlay())
		{
			PathUpdate();
			UpdatePhase();
		}
	}

	private float CalcHealthNormalized()
	{
		float num = health / 1f;
		return num * 100f;
	}

	private void DamageBossListener(MiniGameManager.BossType bossType, float damage)
	{
		if (bossType == MiniGameManager.BossType.Balloony)
		{
			health -= damage;
			GameEventManager.TriggerBossHealthUpdate(MiniGameManager.BossType.Balloony, CalcHealthNormalized());
		}
	}

	private void DisplayAndUpdateBossScore()
	{
		int num = CalculateBossDefeatedScore();
		PlayerData.RoundScore += num * AllMissionData.TotalScoreMultiplier;
		HUDGUIManager.The.ShowNotification(timeAttackBonusString + "\n" + num);
	}

	private int CalculateBossDefeatedScore()
	{
		float num = PlayerData.BalloonyPointValueInitial - targetActiveStopWatch.RetrieveTimeElapsed() * PlayerData.BalloonyPointDecPerSec;
		if (num < PlayerData.BalloonyPointValueMinimum)
		{
			num = PlayerData.BalloonyPointValueMinimum;
		}
		num = (int)num;
		return Mathf.RoundToInt(num);
	}

	public bool IsInTargetMode()
	{
		return bossState == BossState.CanBeTargeted;
	}

	private void DisableParticles()
	{
		if (isParticleSpawned)
		{
			particleSystemBits.gameObject.SetActive(false);
			particleSystemFlash.gameObject.SetActive(false);
		}
	}

	private void SpawnParticles()
	{
		if (!isParticleSpawned)
		{
			particleSystemBits = CacheManager.The().Spawn(particleSystemBitsPrefab).GetComponent<ParticleSystem>();
			particleSystemBits.transform.parent = base.transform;
			particleSystemBits.transform.localPosition = Vector3.zero;
			particleSystemFlash = CacheManager.The().Spawn(particleSystemFlashPrefab).GetComponent<ParticleSystem>();
			particleSystemFlash.transform.parent = base.transform;
			particleSystemFlash.transform.localPosition = Vector3.zero;
			isParticleSpawned = true;
		}
	}

	private void UnspawnParticles()
	{
		if (isParticleSpawned)
		{
			CacheManager.The().Unspawn(particleSystemBits.gameObject);
			CacheManager.The().Unspawn(particleSystemFlash.gameObject);
			isParticleSpawned = false;
		}
	}

	private void PlayExplosionEffect()
	{
		particleSystemBits.gameObject.SetActive(true);
		particleSystemBits.Clear();
		particleSystemBits.Play();
		particleSystemFlash.gameObject.SetActive(true);
		particleSystemFlash.Clear();
		particleSystemFlash.Play();
	}
}

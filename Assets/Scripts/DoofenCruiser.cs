using UnityEngine;

public class DoofenCruiser : PathStateMachine
{
	public enum BossDeck
	{
		Idle = 0,
		OneArm = 1,
		TwoArm = 2,
		None = 3
	}

	private enum BossPhase
	{
		Intro = 0,
		Roller = 1,
		AttackOneArmA = 2,
		WaitingForTarget = 3,
		Target = 4,
		AttackOneArmB = 5,
		AttackTwoArm = 6,
		End = 7,
		None = 8
	}

	public enum WeaponType
	{
		Fire = 0,
		Ice = 1,
		Water = 2,
		Pin = 3
	}

	public enum Attacks
	{
		AttackL = 0,
		AttackM = 1,
		AttackR = 2,
		AttackLM = 3,
		AttackLR = 4,
		AttackMR = 5,
		None = 6
	}

	protected delegate void ActionState();

	private const float m_DefaultHealth = 0f;

	private Animation m_DoofAnimation;

	public Object m_Doofenschmirtz;

	private Transform m_DoofParent;

	private GameObject m_DoofSpawned;

	private Quaternion m_InitialRotation = Quaternion.identity;

	private int m_SequencePassedCount;

	[HideInInspector]
	public BossDeck m_BossDeck = BossDeck.None;

	private BossPhase m_BossPhase = BossPhase.None;

	private SkinnedMeshRenderer m_SpinnyMeshRenderer;

	private Texture m_RollerDefault;

	private Texture m_RollerFire;

	private Texture m_RollerIce;

	private Texture m_RollerWater;

	private Texture m_RollerSpin;

	private Transform m_FireMeshL;

	private Transform m_FireMeshR;

	private Transform m_IceMeshL;

	private Transform m_IceMeshR;

	private Transform m_WaterMeshL;

	private Transform m_WaterMeshR;

	public Transform m_StreamAttachL;

	public Transform m_StreamAttachR;

	public Object m_ChargeUpEffectsPrefab;

	public Object m_EmitterFireStreamPrefab;

	public Object m_EmitterIceStreamPrefab;

	public Object m_EmitterWaterStreamPrefab;

	private ParticleSystem m_ChargeUpEffectsL;

	private ParticleSystem m_ChargeUpEffectsR;

	private BossWeapon m_EmitterFireStreamL;

	private BossWeapon m_EmitterFireStreamR;

	private BossWeapon m_EmitterIceStreamL;

	private BossWeapon m_EmitterIceStreamR;

	private BossWeapon m_EmitterWaterStreamL;

	private BossWeapon m_EmitterWaterStreamR;

	public ParticleSystem m_CruiserFlashSystem;

	public ParticleSystem m_CruiserPoofSystem;

	private bool m_AreParticlesSpawned;

	public WeaponType m_WeaponChosen;

	private float m_Health = 15f;

	private float m_StartingHealth = 15f;

	private StopWatch m_PointWatch;

	private string m_TimeAttackBonusString = "Time Attack Bonus";

	private static DoofenCruiser m_The;

	private float m_AttackTimerReductionFactor = 1f;

	private float m_CollisionDelayReductionFactor = 1f;

	private Timer m_AttackTimer;

	private Timer m_AttackColliderTimer;

	private Timer m_AttackCoolOffParticlesTimer;

	private bool m_IsAbleToAttack;

	private bool m_HasAttackedSinceTriggerTarget;

	private bool m_HitTargetTrigger;

	private string m_AttackAnimNameCur = string.Empty;

	private Attacks m_AttackChosen = Attacks.None;

	private Timer m_RollerTimer;

	private bool m_IsParticleRotationCalculated;

	private Quaternion m_ParticleRotation;

	private BossWeapon m_WeaponParticlesChosenL;

	private BossWeapon m_WeaponParticlesChosenR;

	private bool m_AreCollidersEnabled;

	private bool m_AreChargeUpEffectsEnabled;

	public Animation DoofAnimation
	{
		get
		{
			return m_DoofAnimation;
		}
	}

	private event ActionState m_AttackState;

	private event ActionState m_TargetState;

	private event ActionState m_TutorialState;

	private event ActionState m_RollerState;

	private event ActionState m_RamState;

	private event ActionState m_ExitState;

	public static DoofenCruiser The()
	{
		if (m_The == null)
		{
			m_The = GameObject.Find("DoofenCruiser").GetComponent<DoofenCruiser>();
		}
		return m_The;
	}

	public void MinusOffMaxWorldBoundary(float x, float z)
	{
		TransformOps.AddPositionXZ(base.transform, 0f - x, 0f - z);
	}

	public void SetAllAnimationSpeeds(float newSpeed)
	{
		if (base.GetComponent<Animation>() != null)
		{
			AnimationOps.SetAnimationSpeed(base.GetComponent<Animation>(), newSpeed);
		}
	}

	public void SetAttackAnimationSpeeds(float newSpeed)
	{
		if (base.GetComponent<Animation>() != null)
		{
			AnimationOps.SetAnimationSpeed(base.GetComponent<Animation>(), "DoofenCruiser_AtkLStart", newSpeed);
			AnimationOps.SetAnimationSpeed(base.GetComponent<Animation>(), "DoofenCruiser_AtkLLoop", newSpeed);
			AnimationOps.SetAnimationSpeed(base.GetComponent<Animation>(), "DoofenCruiser_AtkLEnd", newSpeed);
			AnimationOps.SetAnimationSpeed(base.GetComponent<Animation>(), "DoofenCruiser_AtkCStart", newSpeed);
			AnimationOps.SetAnimationSpeed(base.GetComponent<Animation>(), "DoofenCruiser_AtkCLoop", newSpeed);
			AnimationOps.SetAnimationSpeed(base.GetComponent<Animation>(), "DoofenCruiser_AtkCEnd", newSpeed);
			AnimationOps.SetAnimationSpeed(base.GetComponent<Animation>(), "DoofenCruiser_AtkRStart", newSpeed);
			AnimationOps.SetAnimationSpeed(base.GetComponent<Animation>(), "DoofenCruiser_AtkRLoop", newSpeed);
			AnimationOps.SetAnimationSpeed(base.GetComponent<Animation>(), "DoofenCruiser_AtkREnd", newSpeed);
			AnimationOps.SetAnimationSpeed(base.GetComponent<Animation>(), "DoofenCruiser_AtkLCStart", newSpeed);
			AnimationOps.SetAnimationSpeed(base.GetComponent<Animation>(), "DoofenCruiser_AtkLCLoop", newSpeed);
			AnimationOps.SetAnimationSpeed(base.GetComponent<Animation>(), "DoofenCruiser_AtkLCEnd", newSpeed);
			AnimationOps.SetAnimationSpeed(base.GetComponent<Animation>(), "DoofenCruiser_AtkLRStart", newSpeed);
			AnimationOps.SetAnimationSpeed(base.GetComponent<Animation>(), "DoofenCruiser_AtkLRLoop", newSpeed);
			AnimationOps.SetAnimationSpeed(base.GetComponent<Animation>(), "DoofenCruiser_AtkLREnd", newSpeed);
			AnimationOps.SetAnimationSpeed(base.GetComponent<Animation>(), "DoofenCruiser_AtkCRStart", newSpeed);
			AnimationOps.SetAnimationSpeed(base.GetComponent<Animation>(), "DoofenCruiser_AtkCRLoop", newSpeed);
			AnimationOps.SetAnimationSpeed(base.GetComponent<Animation>(), "DoofenCruiser_AtkCREnd", newSpeed);
		}
	}

	public float GetAnimationSpeed(string animName)
	{
		return AnimationOps.GetAnimationSpeed(base.GetComponent<Animation>(), animName);
	}

	public void SetTimerReductionFactors(float speedUpCount, float speedUpCountMax)
	{
		float num = speedUpCount / speedUpCountMax;
		float num2 = num * 0.5f;
		float num3 = num * 0.25f;
		m_AttackTimerReductionFactor = 1f - num2;
		m_CollisionDelayReductionFactor = 1f - num3;
	}

	private void Awake()
	{
		m_The = this;
		Init();
		m_InitialRotation = base.transform.rotation;
		base.GetComponent<Animation>().Play("IdleNormal");
		GameEventManager.GameStart += GameStartListener;
		GameEventManager.BossStartEvents += BossStartListener;
		GameEventManager.BossEndEvents += BossEndListener;
		GameEventManager.DamageBoss += DamageBossListener;
		GameEventManager.GameRestartMenu += GameRestartMenuListener;
		FindAddOns();
		SetAnimationSettings();
		DeathFlashPoofDisable();
	}

	private void OnDestroy()
	{
		GameEventManager.GameStart -= GameStartListener;
		GameEventManager.BossStartEvents -= BossStartListener;
		GameEventManager.BossEndEvents -= BossEndListener;
		GameEventManager.DamageBoss -= DamageBossListener;
		GameEventManager.GameRestartMenu -= GameRestartMenuListener;
	}

	private void SetAnimationSettings()
	{
		base.GetComponent<Animation>()["React"].layer = 2;
		base.GetComponent<Animation>()["React"].blendMode = AnimationBlendMode.Additive;
	}

	private void ResetHealth()
	{
		m_Health = 15f;
	}

	public bool IsActive()
	{
		return m_BossPhase != BossPhase.None;
	}

	private void Start()
	{
		SpawnDoof();
		AssetGPULoader.The().PreLoadObject(base.gameObject);
		AssetGPULoader.The().PreLoadObject(m_DoofSpawned);
		m_AttackTimer = TimerManager.The().SpawnTimer();
		m_AttackColliderTimer = TimerManager.The().SpawnTimer();
		m_AttackCoolOffParticlesTimer = TimerManager.The().SpawnTimer();
		m_RollerTimer = TimerManager.The().SpawnTimer();
		m_PointWatch = TimerManager.The().SpawnStopWatch();
		SpawnParticles();
		DisableParticles();
		string uIText = LocalTextManager.GetUIText("_ATTACK_BONUS_");
		if (uIText != null && uIText != "_ATTACK_BONUS_")
		{
			m_TimeAttackBonusString = uIText;
		}
	}

	public void SpawnDoof()
	{
		if (m_Doofenschmirtz != null && m_DoofSpawned == null)
		{
			m_DoofSpawned = CacheManager.The().Spawn(m_Doofenschmirtz);
			m_DoofAnimation = m_DoofSpawned.GetComponent<Animation>();
			m_DoofSpawned.transform.position = base.transform.position;
			m_DoofSpawned.transform.parent = m_DoofParent;
			m_DoofSpawned.transform.localRotation = Quaternion.identity;
			m_DoofSpawned.transform.localScale = Vector3.one;
		}
		PlayAnimationDoofIdle();
	}

	public void PlayAnimationDoofIdle()
	{
		m_DoofAnimation.Stop();
		m_DoofAnimation.Rewind("BossIdle");
		m_DoofAnimation.Play("BossIdle");
	}

	public void PlayAnimationDoofPressButton()
	{
		m_DoofAnimation.Stop();
		m_DoofAnimation.Rewind("BossPressButton");
		m_DoofAnimation.Play("BossPressButton");
	}

	private void UnspawnDoof()
	{
		if (m_DoofSpawned != null)
		{
			CacheManager.The().Unspawn(m_DoofSpawned);
		}
		m_DoofSpawned = null;
	}

	private void FindAddOns()
	{
		string text = "R:S:Root_Joint/R:S:Main_Joint/R:S:Upper_Joint/DoofParent";
		m_DoofParent = base.transform.Find(text);
		string text2 = "R:S:Mesh/R:S:spinny";
		Transform transform = base.transform.Find(text2);
		m_SpinnyMeshRenderer = transform.GetComponent<SkinnedMeshRenderer>();
		m_RollerDefault = ResourcesMonitor.Load("RollerDefault") as Texture;
		m_RollerFire = ResourcesMonitor.Load("RollerFire") as Texture;
		m_RollerIce = ResourcesMonitor.Load("RollerIce") as Texture;
		m_RollerSpin = ResourcesMonitor.Load("RollerSpin") as Texture;
		m_RollerWater = ResourcesMonitor.Load("RollerWater") as Texture;
		m_SpinnyMeshRenderer.material.mainTexture = m_RollerDefault;
		m_FireMeshL = base.transform.Find("R:S:Mesh/R:S:FireMeshL");
		m_FireMeshR = base.transform.Find("R:S:Mesh/R:S:FireMeshR");
		m_IceMeshL = base.transform.Find("R:S:Mesh/R:S:IceMeshL");
		m_IceMeshR = base.transform.Find("R:S:Mesh/R:S:IceMeshR");
		m_WaterMeshL = base.transform.Find("R:S:Mesh/R:S:WaterMeshL");
		m_WaterMeshR = base.transform.Find("R:S:Mesh/R:S:WaterMeshR");
	}

	private void EnableMeshes()
	{
		m_FireMeshL.gameObject.SetActive(true);
		m_FireMeshR.gameObject.SetActive(true);
		m_IceMeshL.gameObject.SetActive(true);
		m_IceMeshR.gameObject.SetActive(true);
		m_WaterMeshL.gameObject.SetActive(true);
		m_WaterMeshR.gameObject.SetActive(true);
	}

	private void DisableMeshes()
	{
		m_FireMeshL.gameObject.SetActive(false);
		m_FireMeshR.gameObject.SetActive(false);
		m_IceMeshL.gameObject.SetActive(false);
		m_IceMeshR.gameObject.SetActive(false);
		m_WaterMeshL.gameObject.SetActive(false);
		m_WaterMeshR.gameObject.SetActive(false);
	}

	private void SpawnParticles()
	{
		if (!m_AreParticlesSpawned)
		{
			m_ChargeUpEffectsL = CacheManager.The().Spawn(m_ChargeUpEffectsPrefab).GetComponent<ParticleSystem>();
			m_ChargeUpEffectsR = CacheManager.The().Spawn(m_ChargeUpEffectsPrefab).GetComponent<ParticleSystem>();
			m_ChargeUpEffectsL.transform.parent = base.transform;
			m_ChargeUpEffectsR.transform.parent = base.transform;
			m_EmitterFireStreamL = CacheManager.The().Spawn(m_EmitterFireStreamPrefab).GetComponent<BossWeapon>();
			m_EmitterFireStreamR = CacheManager.The().Spawn(m_EmitterFireStreamPrefab).GetComponent<BossWeapon>();
			m_EmitterIceStreamL = CacheManager.The().Spawn(m_EmitterIceStreamPrefab).GetComponent<BossWeapon>();
			m_EmitterIceStreamR = CacheManager.The().Spawn(m_EmitterIceStreamPrefab).GetComponent<BossWeapon>();
			m_EmitterWaterStreamL = CacheManager.The().Spawn(m_EmitterWaterStreamPrefab).GetComponent<BossWeapon>();
			m_EmitterWaterStreamR = CacheManager.The().Spawn(m_EmitterWaterStreamPrefab).GetComponent<BossWeapon>();
			m_AreParticlesSpawned = true;
		}
	}

	public void DisableParticles()
	{
		if (m_AreParticlesSpawned)
		{
			m_ChargeUpEffectsL.gameObject.SetActive(false);
			m_ChargeUpEffectsR.gameObject.SetActive(false);
			m_EmitterFireStreamL.gameObject.SetActive(false);
			m_EmitterFireStreamR.gameObject.SetActive(false);
			m_EmitterIceStreamL.gameObject.SetActive(false);
			m_EmitterIceStreamR.gameObject.SetActive(false);
			m_EmitterWaterStreamL.gameObject.SetActive(false);
			m_EmitterWaterStreamR.gameObject.SetActive(false);
			m_CruiserFlashSystem.gameObject.SetActive(false);
			m_CruiserPoofSystem.gameObject.SetActive(false);
			m_CruiserFlashSystem.Stop();
			m_CruiserPoofSystem.Stop();
		}
	}

	private void UnspawnParticles()
	{
		if (m_AreParticlesSpawned)
		{
			CacheManager.The().Unspawn(m_ChargeUpEffectsL.gameObject);
			CacheManager.The().Unspawn(m_ChargeUpEffectsR.gameObject);
			CacheManager.The().Unspawn(m_EmitterFireStreamL.gameObject);
			CacheManager.The().Unspawn(m_EmitterFireStreamR.gameObject);
			CacheManager.The().Unspawn(m_EmitterIceStreamL.gameObject);
			CacheManager.The().Unspawn(m_EmitterIceStreamR.gameObject);
			CacheManager.The().Unspawn(m_EmitterWaterStreamL.gameObject);
			CacheManager.The().Unspawn(m_EmitterWaterStreamR.gameObject);
			m_AreParticlesSpawned = false;
		}
	}

	private void GameStartListener()
	{
		if (m_BossPhase == BossPhase.None)
		{
			base.transform.rotation = m_InitialRotation;
			base.gameObject.SetActive(true);
			m_BossPhase = BossPhase.None;
			m_BossDeck = BossDeck.None;
			ResetAllActionStates();
			StartAnmationIdle();
			m_SequencePassedCount = 0;
			AttackStreamsColliderDisableAll();
			m_AreCollidersEnabled = false;
			DisableParticles();
			StartPathStayOnFirstPoint("IntroTauntExit");
			SetRotationState(BossRotationState.LookAtPerryXZBase);
			Invoke("TriggerIntroTaunt", 1.5f);
			GameEventManager.TriggerBossIntroTaunt();
		}
	}

	private void BossStartListener(MiniGameManager.BossType bossType)
	{
		if (bossType == MiniGameManager.BossType.DoofenCruiser && m_BossPhase == BossPhase.None)
		{
			CancelInvoke();
			base.gameObject.SetActive(true);
			base.GetComponent<Animation>().Play("IdleNormal");
			PlayAnimationDoofIdle();
			m_IsParticleRotationCalculated = false;
			m_SequencePassedCount = 0;
			SpawnDoof();
			m_SpinnyMeshRenderer.material.mainTexture = m_RollerDefault;
			StartOnPath("Enter");
			SetRotationState(BossRotationState.LookAtPerryXZBase);
			AttackStreamsColliderDisableAll();
			m_AreCollidersEnabled = false;
			DisableParticles();
			TriggerPhaseIdle();
			m_BossPhase = BossPhase.Intro;
			ResetAllActionStates();
			PlayerData.RoundBossEncounters++;
			PlayerData.RoundDoofenschmirtzEncounterCount++;
		}
	}

	private void BossEndListener(MiniGameManager.BossType bossType)
	{
		if (bossType == MiniGameManager.BossType.DoofenCruiser)
		{
			ResetHealth();
			DeathFlashPoofDisable();
			base.gameObject.SetActive(false);
			DisableParticles();
			CancelInvoke();
			m_BossPhase = BossPhase.None;
			m_BossDeck = BossDeck.None;
			ResetAllActionStates();
			base.GetComponent<Animation>().Stop();
			m_DoofAnimation.Stop();
			m_SequencePassedCount = 0;
			AttackStreamsColliderDisableAll();
			m_AreCollidersEnabled = false;
		}
	}

	private void GameRestartMenuListener()
	{
		if (m_BossPhase != BossPhase.None)
		{
			GameEventManager.TriggerBossEnd(MiniGameManager.BossType.DoofenCruiser);
		}
	}

	private void OnFinishedPath()
	{
		if (!(base.CurPath == null))
		{
			if (base.CurPath.name == "Enter")
			{
				LookAtPerryXZTimedStart(2f);
			}
			else if (base.CurPath.name == "Exit")
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	private void OnFinishedRotation()
	{
		if (base.CurPath != null && base.CurPath.name == "IntroTauntExit")
		{
			SetRotationState(BossRotationState.FollowPathRotation);
			StartOnPath("IntroTauntExit");
		}
	}

	public override void OnPathMessage(PathMessages pathMessage)
	{
		switch (pathMessage)
		{
		case PathMessages.StartedPath:
			break;
		case PathMessages.FinishedPath:
			OnFinishedPath();
			break;
		case PathMessages.StartedRotation:
			break;
		case PathMessages.FinishedRotation:
			OnFinishedRotation();
			break;
		}
	}

	public override void OnPathEvent(EventPoint eventPoint)
	{
		if (string.Compare(eventPoint.name, "EndIntroTaunt") == 0)
		{
			PauseCurrentPath();
			base.GetComponent<Animation>().Stop();
			m_DoofAnimation.Stop();
			base.gameObject.SetActive(false);
		}
		if (string.Compare(eventPoint.name, "EndDoofenCruiser") == 0)
		{
			GameEventManager.TriggerBossEnd(MiniGameManager.BossType.DoofenCruiser);
		}
	}

	private void TriggerIntroTaunt()
	{
		StartRotateTowardPath("IntroTauntExit", 0.5f);
	}

	public void StartAnmationIdle()
	{
		base.GetComponent<Animation>().Rewind("IdleNormal");
		base.GetComponent<Animation>().Play("IdleNormal");
		m_DoofAnimation.Rewind("BossIdle");
		m_DoofAnimation.Play("BossIdle");
	}

	private void ResetAllActionStates()
	{
		this.m_AttackState = null;
		this.m_TargetState = null;
		this.m_RollerState = null;
		this.m_RamState = null;
		this.m_ExitState = null;
	}

	private bool IsInAttackMode()
	{
		if (this.m_AttackState != null && this.m_AttackState != new ActionState(AttackCoolingOffParticles))
		{
			return true;
		}
		return false;
	}

	protected void AttackIdleToAttackModeUpdate()
	{
		if (!base.GetComponent<Animation>().IsPlaying("IdleNormalToAttack"))
		{
			m_IsAbleToAttack = true;
			base.GetComponent<Animation>().Play("IdleAttack", PlayMode.StopSameLayer);
			this.m_AttackState = null;
		}
	}

	private void TriggerChooseAttack()
	{
		switch (m_WeaponChosen)
		{
		case WeaponType.Fire:
			GameEventManager.TriggerBossFireAttackType();
			break;
		case WeaponType.Water:
			GameEventManager.TriggerBossWaterAttackType();
			break;
		case WeaponType.Ice:
			GameEventManager.TriggerBossIceAttackType();
			break;
		}
	}

	private void TriggerAttackStart(string animName)
	{
		m_HasAttackedSinceTriggerTarget = true;
		if (Runner.The().GetTutorialState() != Runner.TutorialState.PauseWaitForFire)
		{
			base.GetComponent<Animation>().Rewind(animName);
			float fadeLength = 0.2f / Runner.The().m_animationSpeed;
			base.GetComponent<Animation>().CrossFade(animName, fadeLength, PlayMode.StopSameLayer);
			ChargeUpEffectsEnable();
			this.m_AttackState = AttackStart;
			GameEventManager.TriggerBossWeaponChargeStart();
		}
	}

	protected void AttackStart()
	{
		string text = m_AttackAnimNameCur + "Start";
		string text2 = m_AttackAnimNameCur + "Loop";
		UpdateChargeUpEffectsTransform();
		if (!base.GetComponent<Animation>().IsPlaying(text))
		{
			GameEventManager.TriggerBossWeaponChargeEnd();
			GameEventManager.TriggerBossWeaponShotStart(m_WeaponChosen);
			this.m_AttackState = AttackLoop;
			ChargeUpEffectsDisable();
			AttackStreamsEnable();
			AttackStreamsPlay();
			base.GetComponent<Animation>().Play(text2, PlayMode.StopSameLayer);
			m_AttackTimer.Start(RetrieveAttackLoopDuration());
			m_AttackColliderTimer.Start(RetrieveCollisionStartDelay());
		}
	}

	protected void AttackLoop()
	{
		UpdateWeaponTransform();
		if (m_AttackColliderTimer.IsFinished())
		{
			AttackStreamsColliderEnable();
			m_AreCollidersEnabled = true;
		}
		if (m_AttackTimer.IsFinished())
		{
			GameEventManager.TriggerBossWeaponShotEnd(m_WeaponChosen);
			string text = m_AttackAnimNameCur + "End";
			float fadeLength = 0.25f / Runner.The().m_animationSpeed;
			base.GetComponent<Animation>().CrossFade(text, fadeLength, PlayMode.StopSameLayer);
			m_AttackCoolOffParticlesTimer.Start(RetrieveParticleCoolOffDuration());
			AttackStreamsStop();
			m_AttackColliderTimer.Start(RetrieveCollisionCoolOffDuration());
			this.m_AttackState = AttackEnd;
		}
	}

	protected void AttackEnd()
	{
		string text = m_AttackAnimNameCur + "End";
		if (base.GetComponent<Animation>().IsPlaying(text) && !base.GetComponent<Animation>().IsPlaying("IdleAttack"))
		{
			float fadeLength = 0.25f / Runner.The().m_animationSpeed;
			base.GetComponent<Animation>().Rewind("IdleAttack");
			base.GetComponent<Animation>().CrossFade("IdleAttack", fadeLength, PlayMode.StopSameLayer);
		}
		if (m_AttackColliderTimer.IsFinished() && m_AreCollidersEnabled)
		{
			AttackStreamsColliderDisable();
			m_AreCollidersEnabled = false;
			this.m_AttackState = AttackCoolingOffParticles;
		}
	}

	protected void AttackCoolingOffParticles()
	{
		if (m_AttackCoolOffParticlesTimer.IsFinished())
		{
			AttackStreamsDisable();
			this.m_AttackState = null;
		}
	}

	private void TriggerPhaseIdle()
	{
		m_BossDeck = BossDeck.Idle;
	}

	private void UpdatePhase()
	{
		if (m_BossPhase != BossPhase.None)
		{
			if (m_SequencePassedCount == 1 && m_BossPhase == BossPhase.Intro)
			{
				m_BossDeck = BossDeck.Idle;
			}
			else if (m_SequencePassedCount == 2 && m_BossPhase == BossPhase.Intro)
			{
				m_BossPhase = BossPhase.Roller;
				TriggerRoller();
			}
			else if (m_SequencePassedCount == 3 && m_BossPhase == BossPhase.Roller)
			{
				m_BossDeck = BossDeck.OneArm;
			}
			else if (m_SequencePassedCount == 4 && m_BossPhase == BossPhase.Roller)
			{
				m_BossPhase = BossPhase.AttackOneArmA;
			}
			else if (m_SequencePassedCount == 5 && m_BossPhase == BossPhase.AttackOneArmA)
			{
				m_BossPhase = BossPhase.WaitingForTarget;
				m_HitTargetTrigger = false;
				m_BossDeck = BossDeck.Idle;
			}
			else if (m_SequencePassedCount == 6 && m_HitTargetTrigger && m_BossPhase == BossPhase.WaitingForTarget)
			{
				m_BossPhase = BossPhase.Target;
				TriggerTargetStart();
				m_BossDeck = BossDeck.OneArm;
			}
			else if (m_SequencePassedCount == 8 && m_BossPhase == BossPhase.Target)
			{
				m_BossPhase = BossPhase.AttackOneArmB;
			}
			else if (m_SequencePassedCount == 9 && m_BossPhase == BossPhase.AttackOneArmB)
			{
				m_BossDeck = BossDeck.TwoArm;
			}
			else if (m_SequencePassedCount == 12 && m_BossPhase == BossPhase.AttackOneArmB)
			{
				m_BossPhase = BossPhase.AttackTwoArm;
			}
			else if (m_SequencePassedCount == 13 && m_BossPhase == BossPhase.AttackTwoArm)
			{
				m_BossPhase = BossPhase.End;
			}
			else if (m_SequencePassedCount == 14 && m_BossPhase == BossPhase.End)
			{
				GameManager.The.PlayClip(AudioClipFiles.BOSSLEAVE);
				TriggerTargetEnd();
				TriggerExit();
			}
		}
	}

	private void Update()
	{
		if (!(GameManager.The == null) && !GameManager.The.IsGamePaused() && GameManager.The.IsInGamePlay())
		{
			PathUpdate();
			if (this.m_AttackState != null)
			{
				this.m_AttackState();
			}
			if (this.m_TargetState != null)
			{
				this.m_TargetState();
			}
			if (this.m_TutorialState != null)
			{
				this.m_TutorialState();
			}
			if (this.m_RollerState != null)
			{
				this.m_RollerState();
			}
			if (this.m_RamState != null)
			{
				this.m_RamState();
			}
			if (this.m_ExitState != null)
			{
				this.m_ExitState();
			}
			UpdatePhase();
		}
	}

	public void TriggerAction(Attacks attack)
	{
		if (IsOnPath())
		{
			return;
		}
		if (attack == Attacks.None)
		{
			if (m_BossPhase == BossPhase.WaitingForTarget)
			{
				m_HitTargetTrigger = true;
			}
			m_SequencePassedCount++;
		}
		else if (!IsInExitState() && m_IsAbleToAttack && (this.m_AttackState == null || !(this.m_AttackState != new ActionState(AttackCoolingOffParticles))))
		{
			m_AttackChosen = attack;
			string animName;
			switch (attack)
			{
			default:
				return;
			case Attacks.AttackL:
				m_AttackAnimNameCur = "DoofenCruiser_AtkL";
				animName = m_AttackAnimNameCur + "Start";
				break;
			case Attacks.AttackM:
				m_AttackAnimNameCur = "DoofenCruiser_AtkC";
				animName = m_AttackAnimNameCur + "Start";
				break;
			case Attacks.AttackR:
				m_AttackAnimNameCur = "DoofenCruiser_AtkR";
				animName = m_AttackAnimNameCur + "Start";
				break;
			case Attacks.AttackLM:
				m_AttackAnimNameCur = "DoofenCruiser_AtkLC";
				animName = m_AttackAnimNameCur + "Start";
				break;
			case Attacks.AttackLR:
				m_AttackAnimNameCur = "DoofenCruiser_AtkLR";
				animName = m_AttackAnimNameCur + "Start";
				break;
			case Attacks.AttackMR:
				m_AttackAnimNameCur = "DoofenCruiser_AtkCR";
				animName = m_AttackAnimNameCur + "Start";
				break;
			}
			TriggerAttackStart(animName);
		}
	}

	public bool IsInTargetMode()
	{
		return this.m_TargetState != null && this.m_TargetState != new ActionState(TargetPutAway);
	}

	public void TriggerTargetStart()
	{
		if (this.m_TargetState == null)
		{
			m_PointWatch.Start();
			this.m_TargetState = TargetStartDoof;
			m_HasAttackedSinceTriggerTarget = false;
		}
	}

	public void TriggerTargetEnd()
	{
		m_PointWatch.Pause();
		this.m_TargetState = TargetEnd;
	}

	private void TargetStartDoof()
	{
		if (!IsInAttackMode() && m_HasAttackedSinceTriggerTarget)
		{
			GameEventManager.TriggerBossButtonPress();
			m_DoofAnimation.Rewind("BossPressButton");
			m_DoofAnimation.Play("BossPressButton");
			this.m_TargetState = TargetStart;
		}
	}

	private void TargetStart()
	{
		if (!m_DoofAnimation.IsPlaying("BossPressButton"))
		{
			m_DoofAnimation.Rewind("BossIdle");
			m_DoofAnimation.Play("BossIdle");
			base.GetComponent<Animation>()["TargetStart"].layer = 1;
			base.GetComponent<Animation>()["TargetLoop"].layer = 1;
			base.GetComponent<Animation>()["TargetEnd"].layer = 1;
			base.GetComponent<Animation>()["TargetStart"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["TargetLoop"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["TargetEnd"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["TargetStart"].wrapMode = WrapMode.ClampForever;
			base.GetComponent<Animation>().Rewind("TargetStart");
			base.GetComponent<Animation>().Play("TargetStart");
			GameEventManager.TriggerBossTargetVisible(m_WeaponChosen);
			this.m_TutorialState = TutorialStart;
			this.m_TargetState = TargetUpdate;
		}
	}

	private void TargetUpdate()
	{
	}

	private void TargetEnd()
	{
		if (!IsInAttackMode())
		{
			base.GetComponent<Animation>().Rewind("TargetEnd");
			base.GetComponent<Animation>().Play("TargetEnd");
			GameEventManager.TriggerBossTargetInvisible(m_WeaponChosen);
			this.m_TargetState = TargetPutAway;
		}
	}

	private void TargetPutAway()
	{
	}

	private void TutorialStart()
	{
		if (!IsInAttackMode())
		{
			Runner.The().PlayFireTutorial(MiniGameManager.BossType.DoofenCruiser);
			this.m_TutorialState = null;
		}
	}

	public void TriggerRoller()
	{
		if (this.m_RollerState == null)
		{
			this.m_RollerState = RollerStart;
			GameEventManager.TriggerBossChooseAttack();
			GameEventManager.TriggerBossButtonPress();
			PlayAnimationDoofPressButton();
		}
	}

	private void RollerStart()
	{
		if (!m_DoofAnimation.IsPlaying("BossPressButton"))
		{
			m_DoofAnimation.Rewind("BossIdle");
			m_DoofAnimation.Play("BossIdle");
			base.GetComponent<Animation>().Rewind("RollerSpin");
			base.GetComponent<Animation>().CrossFade("RollerSpin", 0.25f, PlayMode.StopSameLayer);
			m_SpinnyMeshRenderer.material.mainTexture = m_RollerSpin;
			this.m_RollerState = RollerSpinning;
			m_RollerTimer.Start(1.25f);
			GameEventManager.TriggerBossSlotMachineStart();
		}
	}

	private void RollerSpinning()
	{
		if (m_RollerTimer.IsFinished())
		{
			DisableMeshes();
			float num = Random.Range(0, 99);
			if (num < 33f)
			{
				m_SpinnyMeshRenderer.material.mainTexture = m_RollerFire;
				ChooseWeapon(WeaponType.Fire);
			}
			else if (num < 66f)
			{
				m_SpinnyMeshRenderer.material.mainTexture = m_RollerWater;
				ChooseWeapon(WeaponType.Water);
			}
			else if (num < 99f)
			{
				m_SpinnyMeshRenderer.material.mainTexture = m_RollerIce;
				ChooseWeapon(WeaponType.Ice);
			}
			this.m_RollerState = RollerEndingSpin;
		}
	}

	private void RollerEndingSpin()
	{
		if (!base.GetComponent<Animation>().IsPlaying("RollerSpin"))
		{
			switch (m_WeaponChosen)
			{
			case WeaponType.Fire:
				m_FireMeshL.gameObject.SetActive(true);
				m_FireMeshR.gameObject.SetActive(true);
				break;
			case WeaponType.Water:
				m_WaterMeshL.gameObject.SetActive(true);
				m_WaterMeshR.gameObject.SetActive(true);
				break;
			case WeaponType.Ice:
				m_IceMeshL.gameObject.SetActive(true);
				m_IceMeshR.gameObject.SetActive(true);
				break;
			}
			base.GetComponent<Animation>().Stop("RollerSpin");
			this.m_RollerState = RollerEnd;
		}
	}

	private void RollerEnd()
	{
		base.GetComponent<Animation>().Play("IdleNormalToAttack", PlayMode.StopSameLayer);
		this.m_AttackState = AttackIdleToAttackModeUpdate;
		TriggerChooseAttack();
		this.m_RollerState = RollerChosen;
		GameEventManager.TriggerBossSlothMachineEnd();
	}

	private void RollerChosen()
	{
	}

	private Quaternion CalcParticleRotation()
	{
		if (!m_IsParticleRotationCalculated)
		{
			m_ParticleRotation = Runner.The().transform.rotation * Quaternion.AngleAxis(180f, Vector3.up);
			m_IsParticleRotationCalculated = true;
		}
		return m_ParticleRotation;
	}

	private void ChargeUpEffectsEnable()
	{
		if (!(m_WeaponParticlesChosenL == null) && !(m_WeaponParticlesChosenR == null) && !m_AreChargeUpEffectsEnabled)
		{
			switch (m_AttackChosen)
			{
			case Attacks.AttackL:
				m_ChargeUpEffectsL.gameObject.SetActive(true);
				break;
			case Attacks.AttackM:
				m_ChargeUpEffectsR.gameObject.SetActive(true);
				break;
			case Attacks.AttackR:
				m_ChargeUpEffectsR.gameObject.SetActive(true);
				break;
			case Attacks.AttackLM:
				m_ChargeUpEffectsL.gameObject.SetActive(true);
				m_ChargeUpEffectsR.gameObject.SetActive(true);
				break;
			case Attacks.AttackLR:
				m_ChargeUpEffectsL.gameObject.SetActive(true);
				m_ChargeUpEffectsR.gameObject.SetActive(true);
				break;
			case Attacks.AttackMR:
				m_ChargeUpEffectsL.gameObject.SetActive(true);
				m_ChargeUpEffectsR.gameObject.SetActive(true);
				break;
			}
			m_AreChargeUpEffectsEnabled = true;
		}
	}

	private void ChargeUpEffectsDisable()
	{
		if (!(m_WeaponParticlesChosenL == null) && !(m_WeaponParticlesChosenR == null) && m_AreChargeUpEffectsEnabled)
		{
			m_ChargeUpEffectsL.gameObject.SetActive(false);
			m_ChargeUpEffectsR.gameObject.SetActive(false);
			m_AreChargeUpEffectsEnabled = false;
		}
	}

	private void UpdateChargeUpEffectsTransform()
	{
		if (!(m_WeaponParticlesChosenL == null) && !(m_WeaponParticlesChosenR == null))
		{
			m_ChargeUpEffectsL.transform.position = m_StreamAttachL.transform.position;
			m_ChargeUpEffectsL.transform.rotation = CalcParticleRotation();
			m_ChargeUpEffectsR.transform.position = m_StreamAttachR.transform.position;
			m_ChargeUpEffectsR.transform.rotation = CalcParticleRotation();
		}
	}

	private void AttackStreamsEnable()
	{
		if (!(m_WeaponParticlesChosenL == null) && !(m_WeaponParticlesChosenR == null))
		{
			switch (m_AttackChosen)
			{
			case Attacks.AttackL:
				m_WeaponParticlesChosenL.gameObject.SetActive(true);
				break;
			case Attacks.AttackM:
				m_WeaponParticlesChosenR.gameObject.SetActive(true);
				break;
			case Attacks.AttackR:
				m_WeaponParticlesChosenR.gameObject.SetActive(true);
				break;
			case Attacks.AttackLM:
				m_WeaponParticlesChosenL.gameObject.SetActive(true);
				m_WeaponParticlesChosenR.gameObject.SetActive(true);
				break;
			case Attacks.AttackLR:
				m_WeaponParticlesChosenL.gameObject.SetActive(true);
				m_WeaponParticlesChosenR.gameObject.SetActive(true);
				break;
			case Attacks.AttackMR:
				m_WeaponParticlesChosenL.gameObject.SetActive(true);
				m_WeaponParticlesChosenR.gameObject.SetActive(true);
				break;
			}
		}
	}

	private void AttackStreamsColliderEnable()
	{
		if (!(m_WeaponParticlesChosenL == null) && !(m_WeaponParticlesChosenR == null))
		{
			switch (m_AttackChosen)
			{
			case Attacks.AttackL:
				m_WeaponParticlesChosenL.m_ObstacleHitTrigger.gameObject.SetActive(true);
				break;
			case Attacks.AttackM:
				m_WeaponParticlesChosenR.m_ObstacleHitTrigger.gameObject.SetActive(true);
				break;
			case Attacks.AttackR:
				m_WeaponParticlesChosenR.m_ObstacleHitTrigger.gameObject.SetActive(true);
				break;
			case Attacks.AttackLM:
				m_WeaponParticlesChosenL.m_ObstacleHitTrigger.gameObject.SetActive(true);
				m_WeaponParticlesChosenR.m_ObstacleHitTrigger.gameObject.SetActive(true);
				break;
			case Attacks.AttackLR:
				m_WeaponParticlesChosenL.m_ObstacleHitTrigger.gameObject.SetActive(true);
				m_WeaponParticlesChosenR.m_ObstacleHitTrigger.gameObject.SetActive(true);
				break;
			case Attacks.AttackMR:
				m_WeaponParticlesChosenL.m_ObstacleHitTrigger.gameObject.SetActive(true);
				m_WeaponParticlesChosenR.m_ObstacleHitTrigger.gameObject.SetActive(true);
				break;
			}
		}
	}

	private void AttackStreamsColliderDisable()
	{
		if (!(m_WeaponParticlesChosenL == null) && !(m_WeaponParticlesChosenR == null))
		{
			m_WeaponParticlesChosenL.m_ObstacleHitTrigger.gameObject.SetActive(false);
			m_WeaponParticlesChosenR.m_ObstacleHitTrigger.gameObject.SetActive(false);
		}
	}

	private void AttackStreamsColliderDisableAll()
	{
		m_EmitterFireStreamL.m_ObstacleHitTrigger.gameObject.SetActive(false);
		m_EmitterFireStreamR.m_ObstacleHitTrigger.gameObject.SetActive(false);
		m_EmitterIceStreamL.m_ObstacleHitTrigger.gameObject.SetActive(false);
		m_EmitterIceStreamR.m_ObstacleHitTrigger.gameObject.SetActive(false);
		m_EmitterWaterStreamL.m_ObstacleHitTrigger.gameObject.SetActive(false);
		m_EmitterWaterStreamR.m_ObstacleHitTrigger.gameObject.SetActive(false);
	}

	private float RetrieveAttackLoopDuration()
	{
		if (m_WeaponParticlesChosenL != null)
		{
			float attackLoopDuration = m_WeaponParticlesChosenL.m_AttackLoopDuration;
			return attackLoopDuration * m_AttackTimerReductionFactor;
		}
		return 1f;
	}

	private float RetrieveCollisionStartDelay()
	{
		if (m_WeaponParticlesChosenL != null)
		{
			float collisionStartDelay = m_WeaponParticlesChosenL.m_CollisionStartDelay;
			return collisionStartDelay * m_CollisionDelayReductionFactor;
		}
		return 1f;
	}

	private float RetrieveParticleCoolOffDuration()
	{
		if (m_WeaponParticlesChosenL != null)
		{
			return m_WeaponParticlesChosenL.m_ParticleCoolOffDuration;
		}
		return 1f;
	}

	private float RetrieveCollisionCoolOffDuration()
	{
		if (m_WeaponParticlesChosenL != null)
		{
			return m_WeaponParticlesChosenL.m_ColliderCoolOffDuration;
		}
		return 1f;
	}

	private void AttackStreamsPlay()
	{
		if (!(m_WeaponParticlesChosenL == null) && !(m_WeaponParticlesChosenR == null))
		{
			switch (m_AttackChosen)
			{
			case Attacks.AttackL:
				m_WeaponParticlesChosenL.GetComponent<ParticleSystem>().Play();
				break;
			case Attacks.AttackM:
				m_WeaponParticlesChosenR.GetComponent<ParticleSystem>().Play();
				break;
			case Attacks.AttackR:
				m_WeaponParticlesChosenR.GetComponent<ParticleSystem>().Play();
				break;
			case Attacks.AttackLM:
				m_WeaponParticlesChosenL.GetComponent<ParticleSystem>().Play();
				m_WeaponParticlesChosenR.GetComponent<ParticleSystem>().Play();
				break;
			case Attacks.AttackLR:
				m_WeaponParticlesChosenL.GetComponent<ParticleSystem>().Play();
				m_WeaponParticlesChosenR.GetComponent<ParticleSystem>().Play();
				break;
			case Attacks.AttackMR:
				m_WeaponParticlesChosenL.GetComponent<ParticleSystem>().Play();
				m_WeaponParticlesChosenR.GetComponent<ParticleSystem>().Play();
				break;
			}
		}
	}

	private void AttackStreamsStop()
	{
		if (!(m_WeaponParticlesChosenL == null) && !(m_WeaponParticlesChosenR == null))
		{
			m_WeaponParticlesChosenL.GetComponent<ParticleSystem>().Stop();
			m_WeaponParticlesChosenR.GetComponent<ParticleSystem>().Stop();
		}
	}

	private void AttackStreamsDisable()
	{
		if (!(m_WeaponParticlesChosenL == null) && !(m_WeaponParticlesChosenR == null))
		{
			m_WeaponParticlesChosenL.gameObject.SetActive(false);
			m_WeaponParticlesChosenR.gameObject.SetActive(false);
		}
	}

	private void DeathFlashPoofEnable()
	{
		m_CruiserFlashSystem.gameObject.SetActive(true);
		m_CruiserPoofSystem.gameObject.SetActive(true);
		m_CruiserFlashSystem.Play();
		m_CruiserPoofSystem.Play();
	}

	private void DeathFlashPoofDisable()
	{
		m_CruiserFlashSystem.gameObject.SetActive(false);
		m_CruiserPoofSystem.gameObject.SetActive(false);
		m_CruiserFlashSystem.Stop();
		m_CruiserPoofSystem.Stop();
	}

	private void ChooseWeapon(WeaponType weaponType)
	{
		m_WeaponChosen = weaponType;
		switch (weaponType)
		{
		case WeaponType.Fire:
			m_WeaponParticlesChosenL = m_EmitterFireStreamL;
			m_WeaponParticlesChosenR = m_EmitterFireStreamR;
			m_StartingHealth = 15f - (float)PlayerData.WaterWeaponUpgrades;
			m_Health = m_StartingHealth;
			break;
		case WeaponType.Ice:
			m_WeaponParticlesChosenL = m_EmitterIceStreamL;
			m_WeaponParticlesChosenR = m_EmitterIceStreamR;
			m_StartingHealth = 15f - (float)PlayerData.FireWeaponUpgrades;
			m_Health = m_StartingHealth;
			break;
		case WeaponType.Water:
			m_WeaponParticlesChosenL = m_EmitterWaterStreamL;
			m_WeaponParticlesChosenR = m_EmitterWaterStreamR;
			m_StartingHealth = 15f - (float)PlayerData.ElectricWeaponUpgrades;
			m_Health = m_StartingHealth;
			break;
		}
		m_WeaponParticlesChosenL.transform.parent = base.transform;
		m_WeaponParticlesChosenR.transform.parent = base.transform;
	}

	private void UpdateWeaponTransform()
	{
		if (!(m_WeaponParticlesChosenL == null) && !(m_WeaponParticlesChosenR == null))
		{
			m_WeaponParticlesChosenL.transform.position = m_StreamAttachL.transform.position;
			m_WeaponParticlesChosenL.transform.rotation = CalcParticleRotation();
			m_WeaponParticlesChosenR.transform.position = m_StreamAttachR.transform.position;
			m_WeaponParticlesChosenR.transform.rotation = CalcParticleRotation();
		}
	}

	private float CalcHealthNormalized()
	{
		float num = m_Health / m_StartingHealth;
		return num * 100f;
	}

	private void DamageBossListener(MiniGameManager.BossType bossType, float damage)
	{
		if (bossType == MiniGameManager.BossType.DoofenCruiser)
		{
			m_Health -= damage;
			GameEventManager.TriggerBossHealthUpdate(MiniGameManager.BossType.DoofenCruiser, CalcHealthNormalized());
			base.GetComponent<Animation>().Rewind("React");
			base.GetComponent<Animation>().Play("React");
			m_DoofAnimation.Rewind("BossReact");
			m_DoofAnimation.Play("BossReact");
			m_DoofAnimation.Rewind("BossIdle");
			m_DoofAnimation.PlayQueued("BossIdle");
			if (m_Health <= 0f)
			{
				m_IsAbleToAttack = false;
				DeathFlashPoofEnable();
				GameEventManager.TriggerBossDead(MiniGameManager.BossType.DoofenCruiser);
				PlayerData.RoundBossDefeats++;
				PlayerData.AllTimeBossDefeats++;
				DisplayAndUpdateBossScore();
				TriggerTargetEnd();
				TriggerExit();
			}
		}
	}

	private void DisplayAndUpdateBossScore()
	{
		int num = (PlayerData.RoundBossBonusScore = CalculateBossDefeatedScore());
		HUDGUIManager.The.ShowNotification(m_TimeAttackBonusString + "\n" + num);
	}

	private int CalculateBossDefeatedScore()
	{
		float num = PlayerData.ms_BossPointValueInitial - m_PointWatch.RetrieveTimeElapsed() * PlayerData.ms_BossPointDecPerSec;
		if (num < PlayerData.ms_BossPointValueMinimum)
		{
			num = PlayerData.ms_BossPointValueMinimum;
		}
		num = (int)num;
		return Mathf.RoundToInt(num);
	}

	private bool IsInExitState()
	{
		if (this.m_ExitState != null)
		{
			return true;
		}
		return false;
	}

	public void TriggerExit()
	{
		m_BossPhase = BossPhase.None;
		m_BossDeck = BossDeck.None;
		this.m_ExitState = ExitStart;
	}

	private void ExitStart()
	{
		if (!IsInTargetMode() && !IsInAttackMode())
		{
			GameEventManager.TriggerBossMoveToNextMiniGame();
			m_BossPhase = BossPhase.None;
			this.m_ExitState = ExitUpdate;
		}
	}

	private void ExitUpdate()
	{
		this.m_ExitState = ExitEnd;
	}

	private void ExitEnd()
	{
		m_IsAbleToAttack = false;
		base.GetComponent<Animation>().CrossFade("IdleAttackToNormal", 0.5f);
		this.m_ExitState = AttackToIdleModeUpdate;
	}

	protected void AttackToIdleModeUpdate()
	{
		if (!base.GetComponent<Animation>().IsPlaying("IdleAttackToNormal"))
		{
			base.GetComponent<Animation>().CrossFade("IdleNormal", 0.5f, PlayMode.StopSameLayer);
			m_AttackTimer.Start(2f);
			this.m_ExitState = IdleToExit;
		}
	}

	protected void IdleToExit()
	{
		if (m_AttackTimer.IsFinished())
		{
			StartOnPath("Exit");
			this.m_ExitState = null;
		}
	}
}

using System;
using UnityEngine;

public class Runner : MonoBehaviour
{
	private enum EagleState
	{
		FlyToBeforeDestroyObstacles = 0,
		FlyTo = 1,
		Carrying = 2,
		FlyAway = 3,
		None = 4
	}

	private enum CopterBoostState
	{
		RiseUp = 0,
		Flying = 1,
		Dropped = 2,
		None = 3
	}

	public enum TutorialState
	{
		NormalInputLocked = 0,
		PauseWaitForSwipeLeft = 1,
		PauseWaitForSwipeRight = 2,
		PauseWaitForJump = 3,
		PauseWaitForSlide = 4,
		PauseWaitForFire = 5,
		None = 6
	}

	private enum HangGlideStates
	{
		StartedJumpBeforeLaunch = 0,
		DoingJumpBeforeLaunch = 1,
		FinishedJumpTimeToLaunch = 2,
		BeenLaunchedStart = 3,
		BeenLaunchedLoop = 4,
		FinishedLaunch = 5,
		CruisingToHangGlideArea = 6,
		CurrentlyHangGliding = 7,
		DivingDown = 8,
		SlerpCameraBackToNormal = 9,
		LerpCameraBackToNormal = 10,
		None = 11
	}

	protected delegate void ActionState();

	public static float distanceTraveled;

	public float acceleration;

	private float m_CappedTimeDeltaTime;

	public float m_VelocityMaxInitial = 80f;

	public float m_speedIncreaseMultiple = 1.1f;

	public float m_animSpeedIncreaseMultiple = 1.1f;

	private float m_VelocityMax = 80f;

	public float m_SpeedUpThreshold = 2000f;

	public int m_SpeedUpCountMax = 4;

	private int m_SpeedUpCount;

	private float m_SpeedUpDist;

	public float m_fallDuration = 3f;

	public float m_RiseDuration = 7f;

	public float m_DeathFallVelocity = 15f;

	public float m_DeathRiseVelocity = 80f;

	private bool m_ShouldUseDeathPlacementDistance;

	private float m_DeathPlacementDistance;

	private float m_fallStartTime;

	private float m_RiseStartTime;

	private Vector3 startPosition;

	private Waypoint.Lane m_LaneDesired;

	private Waypoint.Lane m_LaneCur;

	public double m_HorizontalMovePercentage = 0.1;

	public Light m_Light;

	private SimpleCharacterShadow simpleShadow;

	private GameObject m_currentRunnerModel;

	private string m_currentRunner = "Perry";

	public float m_animationSpeed = 1f;

	private float m_startAnimSpeed = 1f;

	private Animation m_anim;

	private CapsuleCollider m_collider;

	private bool m_slideOnUpdate;

	private bool m_jumpOnUpdate;

	private bool m_changeLaneLeftOnUpdate;

	private bool m_changeLaneRightOnUpdate;

	private bool m_fireOnUpdate;

	private bool m_isInIntro;

	private float m_colliderStartHeight;

	private Vector3 m_velocityAtPause;

	private float m_distance;

	private float m_lastDistance;

	private float m_lastStepSoundTaken;

	private float m_swipeSpeedX;

	private float m_swipeSpeedY;

	public float m_SwipeDeadZoneX;

	public float m_SwipeDeadZoneY;

	private bool m_swiping;

	private bool m_reverseControls;

	private bool m_touchDown;

	private int m_touchCount;

	public ParticleSystem m_waterWeaponParticle;

	public ParticleSystem m_fireWeaponParticle;

	public ParticleSystem m_electricWeaponParticle;

	public ParticleSystem m_pinWeaponParticle;

	public GameObject m_GunWater;

	public GameObject m_GunFire;

	public GameObject m_GunZap;

	public GameObject m_GunPin;

	private GameObject m_weapon;

	private ParticleSystem m_weaponParticle;

	private bool m_canAttack;

	private Timer m_ShootingTimer;

	private Timer m_CoolOffTimer;

	private bool m_IsShooting;

	public float m_ShootingCoolOffDuration = 0.8f;

	private float m_weaponDamage = 1f;

	private float m_weaponShotDuration = 0.2f;

	private GameObject m_target;

	private bool m_IsWaitingForHolsteredToFinish;

	public GameObject m_tube;

	private Animation m_tubeAnim;

	private Transform m_introCamPos;

	private Vector3 m_HackedIntroCamPosition = Vector3.zero;

	private Quaternion m_HackedIntroCamRotation = Quaternion.identity;

	public GameObject m_HangGlider;

	public GameObject m_PerryHelicopter;

	private Transform m_HGAttachTransform;

	private Animation m_HangGliderAnimations;

	private bool m_invincibilityOn;

	private bool m_powerupCooldown;

	private static float m_blinkInterval = 0.1f;

	private float m_currentBlinkTime;

	public GameObject m_ForceFieldEffects;

	public GameObject m_MagnetEffects;

	public GameObject m_MultiplierEffects;

	public GameObject m_Eagle;

	private EagleState m_EagleState = EagleState.None;

	public float m_EagleRiseVelocityInitial = 240f;

	private float m_EagleRiseVelocity = 240f;

	public float m_PeakEagleYOffset = 120f;

	private float m_PeakEagleHeight;

	private Timer m_EagleTimer;

	private CopterBoostState m_CopterBoostState = CopterBoostState.None;

	public float m_CopterBoostStateDuration = 10f;

	public float m_CopterBoostRiseVelocity = 20f;

	public float m_PeakCopterBoostYOffset = 60f;

	private Timer m_CopterTimer;

	public GameObject renderMesh;

	private Shader m_defaultShader;

	private Shader m_alphaShader;

	private TutorialState m_TutorialState = TutorialState.None;

	public string m_PerryTextureName = "Perry";

	private static Runner m_The;

	private bool m_init;

	private float m_inputX;

	private float m_inputY;

	public float m_LaneChangeSqrdDistToWaypoint = 400f;

	private Vector3 m_BasePosition;

	private bool m_IsBasePositionCalculated;

	private Waypoint m_WaypointHardTurn;

	private Waypoint m_WaypointCur;

	private Waypoint m_WaypointPrev;

	public float m_RotateToWaypointTime = 0.5f;

	private bool m_IsDoingLaneChange;

	private bool m_IgnoreConstrainLaneChange;

	private float m_LaneChangeDir;

	private double m_LaneChangeDist;

	private double m_LaneChangeDistMoved;

	private float m_PizzaLaneChangePercentage;

	private Vector3 m_LaneChangeRightVec = Vector3.right;

	private bool m_DidReverseLaneChange;

	private float m_SpeedOfLaneChangeDefault = 97.5f;

	private float m_SpeedOfLaneChangeHG = 180f;

	private float m_SpeedOfLaneChange;

	private bool m_HasSwipedForHardTurn;

	private float m_JumpAcceleration = 2f;

	private float m_JumpGravity = -0.2f;

	public float m_JumpBoostFactor = 1.55f;

	private float m_JumpVelocity;

	private float m_JumpPosition;

	private bool m_IsJumping;

	private bool m_IsJumpingHitPeak;

	private bool m_IsInAir;

	private bool m_IsTouchingGround;

	private bool m_IsFalling;

	[HideInInspector]
	public bool m_IsInSuperJumpTrigger;

	private bool m_IsDoingSuperJump;

	private Vector3 m_SlidePositionStart;

	public float m_SlideDistanceDesired = 90f;

	private bool m_IsSliding;

	private bool m_IsDipping;

	private bool m_IsDippingHitPeak;

	private float m_DipAcceleration = -3f;

	private float m_DipGravity = 0.1f;

	private float m_DipVelocity;

	private float m_DipPosition;

	private float m_JumpStartTime;

	public float m_JumpDuration;

	private float m_DipStartTime;

	public float m_DipDuration;

	private Vector3 m_DipStartPos;

	private Vector3 m_JumpStartPos;

	public float m_JumpDist;

	public float m_JumpPeak;

	public float m_DipDist;

	public float m_DipPeak;

	public int m_JumpFrameCount;

	public float m_JumpDistanceDesired = 90f;

	public float m_JumpPeakDesired = 16f;

	public float m_HGJumpDistanceDesired = 90f;

	public float m_HGJumpPeakDesired = 40f;

	public float m_HGDipDistanceDesired = 90f;

	public float m_HGDipPeakDesired = 40f;

	private float m_Speed;

	private Vector3 m_Velocity = Vector3.zero;

	private Animation m_EagleAnimation;

	private bool m_HasEagleHitPeak;

	private HangGlideStates m_HangGlideState = HangGlideStates.None;

	private float m_HangGlideHeight;

	private float m_HangGlideLaunchHeight;

	private bool m_IsDying;

	private bool m_IsInDeathFallingState;

	private bool m_IsBouncing;

	private float m_PrevY;

	private bool m_IsTubeIntroStarted;

	private int m_FrameSkipCount = 1;

	private bool m_IsInTubeIntroState;

	private bool m_IsInTubeIntroEndState;

	private bool m_ShouldQueueEnd;

	private bool m_IsTubeIntroFinished;

	private bool m_IsLoopingTubeLoopA;

	private Vector2 lastMousePos;

	public GameObject CurrentRunnerModel
	{
		get
		{
			return m_currentRunnerModel;
		}
	}

	public string currentRunner
	{
		get
		{
			return m_currentRunner;
		}
		set
		{
			m_currentRunner = value;
		}
	}

	public float m_Distance
	{
		get
		{
			return m_distance;
		}
	}

	public bool CanAttack
	{
		get
		{
			return m_canAttack;
		}
	}

	public Transform introCamPos
	{
		get
		{
			return m_introCamPos;
		}
	}

	public Vector3 HackedIntroCamPosition
	{
		get
		{
			return m_HackedIntroCamPosition;
		}
	}

	public Quaternion HackedIntroCamRotation
	{
		get
		{
			return m_HackedIntroCamRotation;
		}
	}

	public float PeakEagleHeight
	{
		get
		{
			return m_PeakEagleHeight;
		}
	}

	public float VelocityMax
	{
		get
		{
			return m_VelocityMax;
		}
	}

	private event ActionState m_ShootingState;

	public string CalcCurrentRunnerBaseName()
	{
		if (m_currentRunner.Contains("Perry"))
		{
			return "Perry";
		}
		if (m_currentRunner.Contains("Peter"))
		{
			return "Peter";
		}
		if (m_currentRunner.Contains("Pinky"))
		{
			return "Pinky";
		}
		if (m_currentRunner.Contains("Terry"))
		{
			return "Terry";
		}
		return "Perry";
	}

	public void SetTutorialState(TutorialState ts)
	{
		m_TutorialState = ts;
	}

	public TutorialState GetTutorialState()
	{
		return m_TutorialState;
	}

	public bool IsInTutorialState()
	{
		if (m_TutorialState != TutorialState.None)
		{
			return true;
		}
		return false;
	}

	public bool IsInPowerUpMode()
	{
		return m_invincibilityOn || Token.ms_IsInMagneticTokenState || GameManager.The.IsScoreMultiplierOn() || IsInEagleState();
	}

	private bool IsInvincible()
	{
		return m_invincibilityOn;
	}

	public static Runner The()
	{
		if (m_The == null)
		{
			m_The = GameObject.Find("Runner").GetComponent<Runner>();
		}
		return m_The;
	}

	private void OnDestroy()
	{
		GameEventManager.GameIntro -= GameIntro;
		GameEventManager.GameStart -= GameStart;
		GameEventManager.GameOver -= GameOver;
		GameEventManager.GameContinue -= GameContinueEvent;
		GameEventManager.GamePause -= GamePause;
		GameEventManager.GameUnPause -= GameUnPause;
		GameEventManager.GameRestartMenu -= GoToGameRestartMenuListener;
		GameEventManager.BossTargetVisible -= BossTargetVisible;
		GameEventManager.BossTargetInvisible -= BossTargetInvisible;
		GameEventManager.BossDone -= BossDoneEvent;
		GameEventManager.BossDead -= BossDeadEvent;
		GameEventManager.BossBehindAttack -= BossBehindAttackEvent;
		GameEventManager.BossFrontAttack -= BossFrontAttackEvent;
		GameEventManager.CollectInvincibility -= CollectInvincibilityListener;
		GameEventManager.InvincibilityOff -= InvincibilityOffListener;
		GameEventManager.InvincibilityCooldown -= InvincibilityCooldownListener;
		GameEventManager.PowerUpMagnetOn -= PowerUpMagnetOnListener;
		GameEventManager.PowerUpMagnetOff -= PowerUpMagnetOffListener;
		GameEventManager.PowerUpFeatherOnEvents -= PowerUpFeatherOnListener;
		GameEventManager.PowerUpFeatherOffEvents -= PowerUpFeatherOffListener;
		GameEventManager.CollectScoreMultiplier -= CollectScoreMultiplierListener;
		GameEventManager.ScoreMultiplierOff -= ScoreMultiplierOffListener;
		GameEventManager.CopterBoostOnEvents -= CopterBoostOnListener;
		GameEventManager.CopterBoostOffEvents -= CopterBoostOffListener;
		GameEventManager.LaunchPerryEvents -= LaunchPerryListener;
		GameEventManager.HangGlideEndEvents -= HangGlideEndListener;
	}

	private void Awake()
	{
		m_The = this;
		GameEventManager.GameIntro += GameIntro;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GameContinue += GameContinueEvent;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnPause += GameUnPause;
		GameEventManager.GameRestartMenu += GoToGameRestartMenuListener;
		GameEventManager.BossTargetVisible += BossTargetVisible;
		GameEventManager.BossTargetInvisible += BossTargetInvisible;
		GameEventManager.BossDone += BossDoneEvent;
		GameEventManager.BossDead += BossDeadEvent;
		GameEventManager.BossBehindAttack += BossBehindAttackEvent;
		GameEventManager.BossFrontAttack += BossFrontAttackEvent;
		GameEventManager.CollectInvincibility += CollectInvincibilityListener;
		GameEventManager.InvincibilityOff += InvincibilityOffListener;
		GameEventManager.InvincibilityCooldown += InvincibilityCooldownListener;
		GameEventManager.PowerUpMagnetOn += PowerUpMagnetOnListener;
		GameEventManager.PowerUpMagnetOff += PowerUpMagnetOffListener;
		GameEventManager.PowerUpFeatherOnEvents += PowerUpFeatherOnListener;
		GameEventManager.PowerUpFeatherOffEvents += PowerUpFeatherOffListener;
		GameEventManager.CollectScoreMultiplier += CollectScoreMultiplierListener;
		GameEventManager.ScoreMultiplierOff += ScoreMultiplierOffListener;
		GameEventManager.CopterBoostOnEvents += CopterBoostOnListener;
		GameEventManager.CopterBoostOffEvents += CopterBoostOffListener;
		GameEventManager.LaunchPerryEvents += LaunchPerryListener;
		GameEventManager.HangGlideEndEvents += HangGlideEndListener;
	}

	private void Start()
	{
		base.useGUILayout = false;
		m_ShootingTimer = TimerManager.The().SpawnTimer();
		m_CoolOffTimer = TimerManager.The().SpawnTimer();
		m_EagleTimer = TimerManager.The().SpawnTimer();
		m_CopterTimer = TimerManager.The().SpawnTimer();
		simpleShadow = GetComponentInChildren<SimpleCharacterShadow>();
	}

	public void SelectRunnerModel()
	{
		string text = DetermineCurrentCharacterModelName();
		GameObject gameObject = RunnerModelChooser.The().SelectRunnerModelForGamePlay(text);
		gameObject.transform.parent = base.transform;
		m_currentRunner = gameObject.name;
		m_currentRunnerModel = gameObject;
		AttachAttachments(gameObject);
		DisableAllAttachments();
		m_anim = m_currentRunnerModel.GetComponent<Animation>();
		MaterialSwitch[] componentsInChildren = gameObject.GetComponentsInChildren<MaterialSwitch>();
		MaterialSwitch[] array = componentsInChildren;
		foreach (MaterialSwitch materialSwitch in array)
		{
			materialSwitch.SwitchMaterial(0);
		}
		for (int j = 0; j < gameObject.transform.childCount; j++)
		{
			Transform child = gameObject.transform.GetChild(j);
			MaterialSwitch[] componentsInChildren2 = child.GetComponentsInChildren<MaterialSwitch>();
			MaterialSwitch[] array2 = componentsInChildren2;
			foreach (MaterialSwitch materialSwitch2 in array2)
			{
				materialSwitch2.SwitchMaterial(0);
			}
		}
		if (GameManager.The.IsLowEndDevice())
		{
			LowEndMaterial componentInChildren = gameObject.GetComponentInChildren<LowEndMaterial>();
			if (componentInChildren != null)
			{
				componentInChildren.AssignLowEndMaterials();
			}
		}
	}

	private void AttachAttachments(GameObject runnerModel)
	{
		string text = "CHAR:hip_joint/CHAR:spine1_joint/CHAR:spine2_joint/CHAR:R_arm_joint/CHAR:R_shoulder_joint/CHAR:R_elbow_joint/CHAR:R_wrist_joint/CHAR:R_weapon_joint/CHAR:AttachGun";
		string text2 = "CHAR:hip_joint/CHAR:spine1_joint/CHAR:spine2_joint/CHAR:L_arm_joint/CHAR:L_shoulder_joint/CHAR:L_elbow_joint/CHAR:L_wrist_joint/CHAR:L_weapon_joint/CHAR:AttachHangGlider";
		string text3 = "CHAR:hip_joint/CHAR:spine1_joint/CHAR:spine2_joint/CHAR:L_arm_joint/CHAR:L_shoulder_joint/CHAR:L_elbow_joint/CHAR:L_wrist_joint/CHAR:L_weapon_joint/CHAR:AttachHelicopter";
		Transform parent = runnerModel.transform.Find(text);
		Transform transform = runnerModel.transform.Find(text2);
		Transform parent2 = runnerModel.transform.Find(text3);
		m_HGAttachTransform = transform;
		m_GunWater.transform.parent = parent;
		m_GunFire.transform.parent = parent;
		m_GunZap.transform.parent = parent;
		m_GunPin.transform.parent = parent;
		m_GunWater.transform.localPosition = Vector3.zero;
		m_GunFire.transform.localPosition = Vector3.zero;
		m_GunZap.transform.localPosition = Vector3.zero;
		m_GunPin.transform.localPosition = Vector3.zero;
		m_GunWater.transform.localRotation = Quaternion.identity;
		m_GunFire.transform.localRotation = Quaternion.identity;
		m_GunZap.transform.localRotation = Quaternion.identity;
		m_GunPin.transform.localRotation = Quaternion.identity;
		m_HangGlider.transform.parent = transform;
		m_HangGlider.transform.localPosition = Vector3.zero;
		m_HangGlider.transform.localRotation = Quaternion.identity;
		m_HangGliderAnimations = m_HangGlider.GetComponent<Animation>();
		m_PerryHelicopter.transform.parent = parent2;
		m_PerryHelicopter.transform.localPosition = Vector3.zero;
		m_PerryHelicopter.transform.localRotation = Quaternion.identity;
		m_Eagle.transform.parent = base.transform;
		m_Eagle.transform.localPosition = Vector3.zero;
		m_Eagle.transform.localRotation = Quaternion.identity;
		Transform parent3 = runnerModel.transform.Find("CHAR:AUX");
		m_ForceFieldEffects.transform.parent = parent3;
		m_MagnetEffects.transform.parent = parent3;
		m_MultiplierEffects.transform.parent = parent3;
		m_ForceFieldEffects.transform.localPosition = Vector3.zero;
		m_MagnetEffects.transform.localPosition = Vector3.zero;
		m_MultiplierEffects.transform.localPosition = Vector3.zero;
		m_ForceFieldEffects.transform.localRotation = Quaternion.identity;
		m_MagnetEffects.transform.localRotation = Quaternion.identity;
		m_MultiplierEffects.transform.localRotation = Quaternion.identity;
		m_introCamPos = parent3;
		m_HackedIntroCamPosition = new Vector3(0f, 15.64139f, -32.10848f);
		m_HackedIntroCamRotation = Quaternion.Euler(270f, 0f, 0f);
		TransformOps.SetLayerRecursively(m_GunWater, LayerMask.NameToLayer("Default"));
		TransformOps.SetLayerRecursively(m_GunFire, LayerMask.NameToLayer("Default"));
		TransformOps.SetLayerRecursively(m_GunZap, LayerMask.NameToLayer("Default"));
		TransformOps.SetLayerRecursively(m_GunPin, LayerMask.NameToLayer("Default"));
		TransformOps.SetLayerRecursively(m_HangGlider, LayerMask.NameToLayer("Default"));
		TransformOps.SetLayerRecursively(m_PerryHelicopter, LayerMask.NameToLayer("Default"));
		TransformOps.SetLayerRecursively(m_Eagle, LayerMask.NameToLayer("Default"));
		TransformOps.SetLayerRecursively(m_ForceFieldEffects, LayerMask.NameToLayer("Default"));
		TransformOps.SetLayerRecursively(m_MagnetEffects, LayerMask.NameToLayer("Default"));
		TransformOps.SetLayerRecursively(m_MultiplierEffects, LayerMask.NameToLayer("Default"));
	}

	public void DisableAllAttachments()
	{
		m_GunWater.SetActive(false);
		m_GunFire.SetActive(false);
		m_GunZap.SetActive(false);
		m_GunPin.SetActive(false);
		m_HangGlider.SetActive(false);
		m_PerryHelicopter.SetActive(false);
		m_Eagle.SetActive(false);
		m_ForceFieldEffects.SetActive(false);
		m_MagnetEffects.SetActive(false);
		m_MultiplierEffects.SetActive(false);
	}

	public string DetermineCurrentCharacterModelName()
	{
		return PlayerData.CurrentCharacterName;
	}

	public void SetRunnerModel(GameObject runner, string name)
	{
		m_currentRunnerModel = runner;
		m_PerryTextureName = runner.name;
		m_GunWater = runner.transform.Find(name + "Guns/waterGun_Attach").gameObject;
		m_GunFire = runner.transform.Find(name + "Guns/fireGun_Attach").gameObject;
		m_GunZap = runner.transform.Find(name + "Guns/zappyGun_Attach").gameObject;
		m_GunPin = runner.transform.Find(name + "Guns/pinGun_Attach").gameObject;
		m_HangGlider = runner.transform.Find(name + "HangGlider").gameObject;
		m_PerryHelicopter = runner.transform.Find(name + "Helicopter").gameObject;
		ResetHelicopter();
		m_ForceFieldEffects = runner.transform.Find("ForceFieldEffects").gameObject;
		m_MagnetEffects = runner.transform.Find("MagnetEffects").gameObject;
		m_MultiplierEffects = runner.transform.Find("MultiplierEffects").gameObject;
		if (m_ForceFieldEffects != null && m_MultiplierEffects != null && m_MultiplierEffects != null)
		{
			m_ForceFieldEffects.transform.localPosition = Vector3.zero;
			m_MagnetEffects.transform.localPosition = Vector3.zero;
			m_MultiplierEffects.transform.localPosition = Vector3.zero;
			m_ForceFieldEffects.transform.localPosition = new Vector3(0f, 5.5f, 0f);
			m_MagnetEffects.transform.localPosition = new Vector3(0f, 5.5f, 0f);
			m_MultiplierEffects.transform.localPosition = new Vector3(0f, 5.5f, 0f);
			m_ForceFieldEffects.SetActive(false);
			m_MagnetEffects.SetActive(false);
			m_MultiplierEffects.SetActive(false);
		}
		m_Eagle = runner.transform.Find("Eagle_Anims").gameObject;
		m_introCamPos = runner.transform.Find("CHAR:AUX");
		if (m_introCamPos == null)
		{
			Debug.Log("Intro Cam Pos is Null");
			m_introCamPos = runner.transform.Find("intro_cam_pos");
		}
	}

	public void Init()
	{
		if (!m_init)
		{
			m_init = true;
			float num = 100f;
			if (Screen.dpi > 0f)
			{
				num = Screen.dpi;
			}
			m_swipeSpeedX = 1f / num;
			m_swipeSpeedY = 1f / num;
			m_SwipeDeadZoneX = 0.1f;
			m_SwipeDeadZoneY = 0.1f;
			startPosition = base.transform.localPosition;
			m_LaneDesired = Waypoint.Lane.Middle;
			m_tubeAnim = m_tube.GetComponent<Animation>();
			m_collider = GetComponent<CapsuleCollider>();
			m_colliderStartHeight = m_collider.center.y;
		}
	}

	private float SqrdDistToWaypoint(Waypoint waypoint)
	{
		Vector3 vector = waypoint.RetrieveTransform(m_LaneDesired).position - base.transform.position;
		return Vector3.Dot(vector, vector);
	}

	private bool IsAllowedToChangeLanes()
	{
		if (m_HasSwipedForHardTurn)
		{
			return false;
		}
		return true;
	}

	private void ChooseLaneChangeSpeed()
	{
		if (!IsInHangGlideState())
		{
			m_SpeedOfLaneChange = m_SpeedOfLaneChangeDefault;
		}
		else
		{
			m_SpeedOfLaneChange = m_SpeedOfLaneChangeHG;
		}
	}

	private void ForceChangeLaneLeft()
	{
		if (!(m_WaypointCur == null))
		{
			m_LaneChangeDir = -1f;
			m_LaneDesired = Waypoint.IncLaneLeft(m_LaneDesired);
			m_LaneChangeDist = CalcLaneChangeDist();
			m_LaneChangeDistMoved = 0.0;
			m_IsDoingLaneChange = true;
			ChooseLaneChangeSpeed();
		}
	}

	private void ForceChangeLaneRight()
	{
		if (!(m_WaypointCur == null))
		{
			m_LaneChangeDir = 1f;
			m_LaneDesired = Waypoint.IncLaneRight(m_LaneDesired);
			m_LaneChangeDist = CalcLaneChangeDist();
			m_LaneChangeDistMoved = 0.0;
			m_IsDoingLaneChange = true;
			ChooseLaneChangeSpeed();
		}
	}

	private bool IsWaypointToLeftHigher(Waypoint.Lane lane)
	{
		if (m_WaypointCur == null)
		{
			return false;
		}
		Waypoint.Lane lane2 = Waypoint.IncLaneLeft(lane);
		return IsWaypointHigherThanRunner(lane2);
	}

	private bool IsWaypointToRightHigher(Waypoint.Lane lane)
	{
		if (m_WaypointCur == null)
		{
			return false;
		}
		Waypoint.Lane lane2 = Waypoint.IncLaneRight(lane);
		return IsWaypointHigherThanRunner(lane2);
	}

	private bool IsWaypointHigherThanRunner(Waypoint.Lane lane)
	{
		Transform transform = base.transform;
		Transform transform2 = m_WaypointCur.RetrieveTransform(lane);
		if (transform2.position.y > transform.position.y + 2f)
		{
			return true;
		}
		return false;
	}

	private void ChangeLaneLeft()
	{
		if (m_WaypointCur == null || !IsAllowedToChangeLanes())
		{
			GameManager.The.PlayClip(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.NOSWIPE);
			return;
		}
		if (IsWaitingForHardTurnSwipe())
		{
			HandleHardTurnSwipeLeft();
			return;
		}
		if (m_LaneDesired == Waypoint.Lane.Left)
		{
			GameManager.The.PlayClip(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.NOSWIPE);
			if (IsFlyingAllowingLaneChangeAnims())
			{
				if (IsInHangGlideState())
				{
					StartAnimHGBankLeft();
				}
				else
				{
					StartAnimLeftBounce();
				}
			}
			return;
		}
		Waypoint.Lane lane = m_LaneCur;
		if (m_IsDoingLaneChange)
		{
			lane = m_LaneDesired;
		}
		if (!IsInHangGlideState() && IsWaypointToLeftHigher(lane))
		{
			StartAnimLeftBounce();
			return;
		}
		string clip = AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.SWIPELEFT;
		if (IsFlyingAllowingLaneChangeAnims())
		{
			if (IsInHangGlideState())
			{
				clip = AudioClipFiles.HANGGLIDERFOLDER + AudioClipFiles.GLIDERLEFT;
				StartAnimHGBankLeft();
			}
			else if (!IsInJumpState())
			{
				StartAnimLeftHop();
			}
		}
		GameManager.The.PlayClip(clip);
		m_LaneChangeDir = -1f;
		m_LaneDesired = Waypoint.IncLaneLeft(m_LaneDesired);
		m_LaneChangeDist = CalcLaneChangeDist();
		m_LaneChangeDistMoved = 0.0;
		m_PizzaLaneChangePercentage = 0f;
		m_IsDoingLaneChange = true;
		ChooseLaneChangeSpeed();
	}

	private void ChangeLaneRight()
	{
		if (m_WaypointCur == null || !IsAllowedToChangeLanes())
		{
			GameManager.The.PlayClip(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.NOSWIPE);
			return;
		}
		if (IsWaitingForHardTurnSwipe())
		{
			HandleHardTurnSwipeRight();
			return;
		}
		if (m_LaneDesired == Waypoint.Lane.Right)
		{
			GameManager.The.PlayClip(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.NOSWIPE);
			if (IsFlyingAllowingLaneChangeAnims())
			{
				if (IsInHangGlideState())
				{
					StartAnimHGBankRight();
				}
				else
				{
					StartAnimRightBounce();
				}
			}
			return;
		}
		Waypoint.Lane lane = m_LaneCur;
		if (m_IsDoingLaneChange)
		{
			lane = m_LaneDesired;
		}
		if (!IsInHangGlideState() && IsWaypointToRightHigher(lane))
		{
			StartAnimRightBounce();
			return;
		}
		string clip = AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.SWIPERIGHT;
		if (IsFlyingAllowingLaneChangeAnims())
		{
			if (IsInHangGlideState())
			{
				clip = AudioClipFiles.HANGGLIDERFOLDER + AudioClipFiles.GLIDERRIGHT;
				StartAnimHGBankRight();
			}
			else if (!IsInJumpState())
			{
				StartAnimRightHop();
			}
		}
		GameManager.The.PlayClip(clip);
		m_LaneChangeDir = 1f;
		m_LaneDesired = Waypoint.IncLaneRight(m_LaneDesired);
		m_LaneChangeDist = CalcLaneChangeDist();
		m_LaneChangeDistMoved = 0.0;
		m_PizzaLaneChangePercentage = 0f;
		m_IsDoingLaneChange = true;
		ChooseLaneChangeSpeed();
	}

	private void OnFinishedLaneChange()
	{
		if (IsFlyingAllowingLaneChangeAnims() && !IsInEagleStateCarrying())
		{
			if (IsInHangGlideState())
			{
				if (m_HangGlideState != HangGlideStates.DivingDown && !IsInDipState())
				{
					StartAnimHGLoopQueued();
				}
			}
			else if (!IsInAir())
			{
				if (m_canAttack)
				{
					if (!IsInSlideState())
					{
						StartAnimWeaponDrawn();
					}
				}
				else if (!IsInSlideState())
				{
					CrossFadeAnimRun(0.35f);
				}
			}
		}
		m_DidReverseLaneChange = false;
		m_LaneCur = m_LaneDesired;
		m_IsDoingLaneChange = false;
		m_IgnoreConstrainLaneChange = false;
		m_LaneChangeDir = 0f;
	}

	private void UpdateControls()
	{
		// Lane change controls - support multiple key bindings
		// Right lane change: D, Right Arrow, or KeyCode.RightArrow
		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			Debug.Log("[INPUT] Right lane change detected - Key: " + (Input.GetKeyDown(KeyCode.D) ? "D" : "Right Arrow") + " | Reverse Controls: " + m_reverseControls);
			if (m_reverseControls)
			{
				m_changeLaneLeftOnUpdate = true;
			}
			else
			{
				m_changeLaneRightOnUpdate = true;
			}
		}
		// Left lane change: A, Left Arrow, or KeyCode.LeftArrow
		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			Debug.Log("[INPUT] Left lane change detected - Key: " + (Input.GetKeyDown(KeyCode.A) ? "A" : "Left Arrow") + " | Reverse Controls: " + m_reverseControls);
			if (m_reverseControls)
			{
				m_changeLaneRightOnUpdate = true;
			}
			else
			{
				m_changeLaneLeftOnUpdate = true;
			}
		}
		
		// Jump controls - W, Up Arrow, Space, or Jump button
		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
		{
			string inputSource = "Unknown";
			if (Input.GetKeyDown(KeyCode.W)) inputSource = "W";
			else if (Input.GetKeyDown(KeyCode.UpArrow)) inputSource = "Up Arrow";
			else if (Input.GetKeyDown(KeyCode.Space)) inputSource = "Space";
			else if (Input.GetButtonDown("Jump")) inputSource = "Jump Button";
			Debug.Log("[INPUT] Jump detected - Source: " + inputSource);
			m_jumpOnUpdate = true;
		}
		
		// Slide controls - S, Down Arrow, X, or Slide button
		if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Slide"))
		{
			string inputSource = "Unknown";
			if (Input.GetKeyDown(KeyCode.S)) inputSource = "S";
			else if (Input.GetKeyDown(KeyCode.DownArrow)) inputSource = "Down Arrow";
			else if (Input.GetKeyDown(KeyCode.X)) inputSource = "X";
			else if (Input.GetButtonDown("Slide")) inputSource = "Slide Button";
			Debug.Log("[INPUT] Slide detected - Source: " + inputSource);
			m_slideOnUpdate = true;
		}
		
		// Fire/Shoot controls - F, E, Left Ctrl, or Fire buttons
		if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetButtonDown("Shoot") || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3"))
		{
			string inputSource = "Unknown";
			if (Input.GetKeyDown(KeyCode.F)) inputSource = "F";
			else if (Input.GetKeyDown(KeyCode.E)) inputSource = "E";
			else if (Input.GetKeyDown(KeyCode.LeftControl)) inputSource = "Left Ctrl";
			else if (Input.GetButtonDown("Shoot")) inputSource = "Shoot Button";
			else if (Input.GetButtonDown("Fire1")) inputSource = "Fire1 Button";
			else if (Input.GetButtonDown("Fire2")) inputSource = "Fire2 Button";
			else if (Input.GetButtonDown("Fire3")) inputSource = "Fire3 Button";
			Debug.Log("[INPUT] Fire/Shoot detected - Source: " + inputSource + " | Can Attack: " + m_canAttack);
			m_fireOnUpdate = true;
		}
	}

	private bool CanRunnerJump()
	{
		if (m_HangGlideState == HangGlideStates.CurrentlyHangGliding || m_HangGlideState == HangGlideStates.StartedJumpBeforeLaunch)
		{
			return !IsInJumpState();
		}
		if (IsInHangGlideState())
		{
			return false;
		}
		if (IsInCopterBoostState())
		{
			return false;
		}
		return IsTouchingGround();
	}

	private bool CanRunnerSlide()
	{
		if (IsInHangGlidePrepareState())
		{
			return false;
		}
		if (IsInHangGlideSlerpAndLerpOutState())
		{
			return false;
		}
		if (IsInDipState())
		{
			return false;
		}
		if (IsInCopterBoostState())
		{
			return false;
		}
		if (m_EagleState == EagleState.Carrying)
		{
			return false;
		}
		if (!IsInSlideState())
		{
			return true;
		}
		return false;
	}

	private void Jump()
	{
		if (CanRunnerJump())
		{
			if (m_IsInSuperJumpTrigger)
			{
				m_IsDoingSuperJump = true;
				m_JumpVelocity = m_JumpAcceleration * m_JumpBoostFactor;
				m_IsInSuperJumpTrigger = false;
			}
			else
			{
				m_JumpVelocity = m_JumpAcceleration;
			}
			if (IsInDipState())
			{
				CancelDip();
			}
			m_IsJumping = true;
			m_IsSliding = false;
			m_IsJumpingHitPeak = false;
			m_IsInAir = true;
			m_JumpPeak = 0f;
			m_JumpStartPos = base.transform.position;
			m_JumpStartTime = Time.time;
			m_JumpFrameCount = 0;
			StartAnimJump();
			GameManager.The.PlayClip(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.JUMPSOUND);
		}
	}

	private void Slide()
	{
		if (CanRunnerSlide())
		{
			CancelJump();
			if (m_HangGlideState == HangGlideStates.CurrentlyHangGliding)
			{
				m_DipVelocity = m_DipAcceleration;
				m_IsDipping = true;
				m_IsDippingHitPeak = false;
				m_DipPeak = 0f;
				m_DipStartTime = Time.time;
				m_DipStartPos = base.transform.position;
				StartAnimHGDive();
				m_DipPeak = 0f;
			}
			else
			{
				StartAnimSliding();
				GameManager.The.PlayClip(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.ROLL);
				m_IsSliding = true;
				m_SlidePositionStart = base.transform.position;
				PlayerData.RoundPlayerSlides++;
			}
		}
	}

	private void Attack()
	{
		if (!m_IsShooting)
		{
			m_IsShooting = true;
			m_CoolOffTimer.Start(m_ShootingCoolOffDuration);
			PurchasableGadgetItem gadgetItem = AllItemData.GetGadgetItem((int)PlayerData.m_currentGadgetType);
			if (gadgetItem.UpgradeNums >= gadgetItem.m_maxUpgrades)
			{
				PlayerData.RoundMaxPowerGadgetFire++;
				PlayerData.RoundHasFiredMaxedOutWeapon = true;
			}
			GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BLASTERFIRE);
			MakeRunnerShoot();
		}
	}

	public Vector3 CalcRunnerLaneBasePosition(Waypoint.Lane lane)
	{
		if (m_WaypointCur == null)
		{
			return Vector3.zero;
		}
		Transform transform = m_WaypointCur.RetrieveTransform(lane);
		Vector3 position = transform.position;
		Vector3 forward = transform.forward;
		if (IsInCopterBoostState() || IsInEagleState())
		{
			forward.y = 0f;
		}
		Vector3 position2 = base.transform.position;
		return MathOps.NearestPointPointLine(position2, position, forward);
	}

	public Vector3 CalcObjectLaneBasePosition(Vector3 objectPosition, Waypoint.Lane lane)
	{
		if (m_WaypointCur == null)
		{
			return Vector3.zero;
		}
		Transform transform = m_WaypointCur.RetrieveTransform(lane);
		Vector3 position = transform.position;
		Vector3 forward = transform.forward;
		return MathOps.NearestPointPointLine(objectPosition, position, forward);
	}

	public Vector3 CalcHalfwayCamTargetPosition()
	{
		Vector3 vector = CalcRunnerLaneBasePosition(Waypoint.Lane.Middle);
		Vector3 vector2 = CalcBasePosition() - vector;
		float num = 0.8f;
		vector2 *= num;
		return vector + vector2;
	}

	public Vector3 CalcBasePosition()
	{
		if (!m_IsBasePositionCalculated)
		{
			m_BasePosition = base.transform.position;
			if (m_WaypointCur != null && (m_HangGlideState == HangGlideStates.StartedJumpBeforeLaunch || m_HangGlideState == HangGlideStates.DoingJumpBeforeLaunch || m_HangGlideState == HangGlideStates.CurrentlyHangGliding || m_HangGlideState == HangGlideStates.DivingDown || m_HangGlideState == HangGlideStates.SlerpCameraBackToNormal || m_HangGlideState == HangGlideStates.LerpCameraBackToNormal || m_HangGlideState == HangGlideStates.None))
			{
				Transform transform = ((!m_WaypointCur.m_FirstInRamp || !(m_WaypointPrev != null)) ? m_WaypointCur.RetrieveTransform(m_LaneDesired) : m_WaypointPrev.RetrieveTransform(m_LaneDesired));
				Vector3 forward = transform.forward;
				if (IsInCopterBoostState() || IsInEagleState())
				{
					forward.y = 0f;
				}
				Vector3 vector = MathOps.IntersectPointLineLine(transform.position, forward, base.transform.position, Vector3.down);
				m_BasePosition.y = vector.y;
				m_IsBasePositionCalculated = true;
			}
		}
		return m_BasePosition;
	}

	public float CalcBasePositionY()
	{
		float y = CalcBasePosition().y;
		if (IsInCopterBoostState() || IsInEagleState())
		{
			return base.transform.position.y;
		}
		if (IsInDeathState())
		{
			return y;
		}
		if (m_IsDoingSuperJump && m_IsJumping)
		{
			return base.transform.position.y;
		}
		if (IsInHangGlideCameraState())
		{
			float num = base.transform.position.y - y;
			float num2 = 0.5f;
			num *= num2;
			return y + num;
		}
		if (IsInDipState())
		{
			return base.transform.position.y;
		}
		float num3 = base.transform.position.y - m_JumpPosition;
		if (num3 < y)
		{
			return y;
		}
		return num3;
	}

	private bool IsPassedWaypoint(bool ignoreLaneConstrainCondition = false)
	{
		if (m_WaypointCur == null)
		{
			return true;
		}
		if (!m_WaypointCur.GetIsPassed())
		{
			Vector3 position = m_WaypointCur.RetrieveTransform(m_LaneDesired).position;
			Vector3 lhs = position - base.transform.position;
			lhs.y = 0f;
			if (!ShouldMovePastLaneConstrainWaypoint() && !ignoreLaneConstrainCondition)
			{
				return false;
			}
			if (Vector3.Dot(lhs, base.transform.forward) < 0.001f)
			{
				m_WaypointCur.SetIsPassed(true);
				if (m_WaypointCur.m_TriggerType == Waypoint.Triggers.HangGlideDiveDown)
				{
					StartAnimHGDive();
					m_HangGlideState = HangGlideStates.DivingDown;
				}
				else if (m_WaypointCur.m_TriggerType == Waypoint.Triggers.HangGlideEnd)
				{
					GameEventManager.TriggerHangGlideEnd();
				}
				return true;
			}
		}
		return false;
	}

	private double CalcLaneChangeDist(out Vector3 runnerToLaneDiff)
	{
		Transform transform = m_WaypointCur.RetrieveTransform(m_LaneDesired);
		m_LaneChangeRightVec = transform.right;
		Vector3 position = transform.position;
		position.y = 0f;
		Vector3 position2 = base.transform.position;
		position2.y = 0f;
		Vector3 forward = transform.forward;
		forward.y = 0f;
		double d = MathOps.SquaredDistancePointLine(position2, position, forward, out runnerToLaneDiff);
		return Math.Sqrt(d);
	}

	private double CalcLaneChangeDist()
	{
		Vector3 runnerToLaneDiff;
		return CalcLaneChangeDist(out runnerToLaneDiff);
	}

	private void MoveRunnerToNewLaneWidth()
	{
		if (m_LaneCur != Waypoint.Lane.Middle)
		{
			Vector3 runnerToLaneDiff;
			m_LaneChangeDist = CalcLaneChangeDist(out runnerToLaneDiff);
			if (Vector3.Dot(runnerToLaneDiff, base.transform.right) > 0f)
			{
				m_LaneChangeDir = -1f;
			}
			else
			{
				m_LaneChangeDir = 1f;
			}
			m_LaneChangeDistMoved = 0.0;
			m_IsDoingLaneChange = true;
			m_IgnoreConstrainLaneChange = true;
			m_SpeedOfLaneChange = 30f;
		}
	}

	private bool IsWaitingForHardTurnSwipe()
	{
		if (m_WaypointCur != null && m_WaypointCur.m_TurnConstrain && !m_HasSwipedForHardTurn)
		{
			return true;
		}
		return false;
	}

	private bool ShouldMovePastLaneConstrainWaypoint()
	{
		if (m_WaypointCur != null && m_WaypointCur.m_TurnConstrain && !m_HasSwipedForHardTurn)
		{
			return false;
		}
		return true;
	}

	private bool ShouldConstrainLaneTurn()
	{
		if (m_HasSwipedForHardTurn)
		{
			if (m_WaypointCur != null && m_WaypointCur.m_TurnConstrain)
			{
				return true;
			}
			float valA = Vector3.Dot(m_WaypointCur.RetrieveTransform(m_LaneCur).transform.forward, base.transform.forward);
			if (MathOps.AboutEqual(valA, 1f))
			{
				m_HasSwipedForHardTurn = false;
				return false;
			}
			return true;
		}
		m_HasSwipedForHardTurn = false;
		return false;
	}

	private bool IsRunnerPassedNextWaypointLocalZ(Waypoint.Lane lane)
	{
		if (m_WaypointHardTurn == null)
		{
			return false;
		}
		Vector3 lhs = m_WaypointHardTurn.RetrieveTransform(lane).position - base.transform.position;
		if (Vector3.Dot(lhs, base.transform.forward) < 0f)
		{
			return true;
		}
		return false;
	}

	private void HandleHardTurnSwipeLeft()
	{
		if (m_WaypointCur.m_LaneToBeIn == Waypoint.Lane.Right)
		{
			return;
		}
		if (m_IsDoingLaneChange)
		{
			ResetLaneChange();
			OnFinishedLaneChange();
		}
		m_LaneChangeDir = -1f;
		if (m_WaypointCur != null && m_WaypointCur.m_LaneToBeIn == Waypoint.Lane.LeftOrRight)
		{
			m_WaypointHardTurn = PlatformManager.The().RetrieveFollowingWaypoint(Waypoint.Lane.Left);
			if (m_WaypointHardTurn == null)
			{
				Debug.Log("m_WaypointHardTurn == null");
			}
			PlatformManager.The().ChoosePath(Waypoint.Lane.Left);
		}
		else
		{
			m_WaypointHardTurn = PlatformManager.The().RetrieveFollowingWaypoint();
		}
		if (!IsRunnerPassedNextWaypointLocalZ(Waypoint.Lane.Left))
		{
			m_LaneDesired = Waypoint.Lane.Left;
		}
		else if (!IsRunnerPassedNextWaypointLocalZ(Waypoint.Lane.Middle))
		{
			m_LaneDesired = Waypoint.Lane.Middle;
		}
		else
		{
			if (IsRunnerPassedNextWaypointLocalZ(Waypoint.Lane.Right))
			{
				return;
			}
			m_LaneDesired = Waypoint.Lane.Right;
		}
		m_LaneCur = m_LaneDesired;
		m_HasSwipedForHardTurn = true;
	}

	private void HandleHardTurnSwipeRight()
	{
		if (m_WaypointCur.m_LaneToBeIn == Waypoint.Lane.Left)
		{
			return;
		}
		if (m_IsDoingLaneChange)
		{
			ResetLaneChange();
			OnFinishedLaneChange();
		}
		m_LaneChangeDir = 1f;
		if (m_WaypointCur != null && m_WaypointCur.m_LaneToBeIn == Waypoint.Lane.LeftOrRight)
		{
			m_WaypointHardTurn = PlatformManager.The().RetrieveFollowingWaypoint(Waypoint.Lane.Right);
			if (m_WaypointHardTurn == null)
			{
				Debug.Log("m_WaypointHardTurn == null");
			}
			PlatformManager.The().ChoosePath(Waypoint.Lane.Right);
		}
		else
		{
			m_WaypointHardTurn = PlatformManager.The().RetrieveFollowingWaypoint();
		}
		if (!IsRunnerPassedNextWaypointLocalZ(Waypoint.Lane.Right))
		{
			m_LaneDesired = Waypoint.Lane.Right;
		}
		else if (!IsRunnerPassedNextWaypointLocalZ(Waypoint.Lane.Middle))
		{
			m_LaneDesired = Waypoint.Lane.Middle;
		}
		else
		{
			if (IsRunnerPassedNextWaypointLocalZ(Waypoint.Lane.Left))
			{
				return;
			}
			m_LaneDesired = Waypoint.Lane.Left;
		}
		m_LaneCur = m_LaneDesired;
		m_HasSwipedForHardTurn = true;
	}

	private void ConstrainLaneTurn()
	{
		Transform transform = m_WaypointHardTurn.RetrieveTransform(m_LaneDesired);
		Vector3 position = transform.position;
		Vector3 forward = transform.forward;
		position.y = 0f;
		Vector3 position2 = base.transform.position;
		position2.y = 0f;
		Vector3 vector = MathOps.NearestPointPointLine(position2, position, forward);
		Vector3 vector2 = vector - position2;
		vector2.y = 0f;
		Vector3 right = transform.right;
		if (m_LaneChangeDir < 0f)
		{
			right *= -1f;
		}
		float num = Vector3.Dot(right, vector2);
		if (num > 0f)
		{
			base.transform.position += vector2;
			float magnitude = m_Velocity.magnitude;
			Vector3 velocity = transform.forward * magnitude;
			velocity.y = 0f;
			m_Velocity = velocity;
		}
	}

	private void ConstrainLaneChange()
	{
		Transform transform = m_WaypointCur.RetrieveTransform(m_LaneDesired);
		Vector3 position = transform.position;
		Vector3 forward = transform.forward;
		Vector3 position2 = base.transform.position;
		position.y = 0f;
		position2.y = 0f;
		Vector3 vector = MathOps.NearestPointPointLine(position2, position, forward);
		Vector3 vector2 = vector - position2;
		vector2.y = 0f;
		Vector3 right = transform.right;
		right *= m_LaneChangeDir;
		float num = Vector3.Dot(right, vector2);
		if (num < 0f)
		{
			if (m_HangGlideState == HangGlideStates.DivingDown)
			{
				OnFinishedLaneChange();
			}
			else
			{
				base.transform.position += vector2;
			}
		}
	}

	private void UpdateLaneChangeMovement()
	{
		if (m_WaypointCur == null)
		{
			return;
		}
		if (m_IsDoingLaneChange)
		{
			float num = m_CappedTimeDeltaTime * m_SpeedOfLaneChange;
			if ((double)num + m_LaneChangeDistMoved > m_LaneChangeDist)
			{
				num = (float)(m_LaneChangeDist - m_LaneChangeDistMoved);
			}
			Vector3 vector = num * m_LaneChangeRightVec;
			if (m_LaneChangeDistMoved < m_LaneChangeDist - (double)MathOps.ms_Epsilon)
			{
				if (m_LaneChangeDir <= 0f)
				{
					if (!IsInHangGlideState() && IsWaypointHigherThanRunner(m_LaneDesired))
					{
						m_DidReverseLaneChange = true;
						m_LaneCur = m_LaneDesired;
						ForceChangeLaneRight();
					}
					else
					{
						base.transform.position -= vector;
					}
				}
				else if (m_LaneChangeDir > 0f)
				{
					if (!IsInHangGlideState() && IsWaypointHigherThanRunner(m_LaneDesired))
					{
						m_DidReverseLaneChange = true;
						m_LaneCur = m_LaneDesired;
						ForceChangeLaneLeft();
					}
					else
					{
						base.transform.position += vector;
					}
				}
				m_LaneChangeDistMoved += num;
				if (!m_IgnoreConstrainLaneChange)
				{
					ConstrainLaneChange();
				}
			}
			else
			{
				ConstrainLaneChange();
				OnFinishedLaneChange();
			}
		}
		else
		{
			if (!(m_WaypointPrev != null) || m_WaypointPrev.IsPizzaSliceVertical())
			{
				return;
			}
			if (m_HangGlideState == HangGlideStates.CurrentlyHangGliding || m_HangGlideState == HangGlideStates.DivingDown)
			{
				Quaternion rotation = m_WaypointCur.RetrieveTransform(m_LaneDesired).rotation;
				float value = 12f * m_CappedTimeDeltaTime;
				value = Mathf.Clamp(value, 0f, 1f);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, rotation, value);
			}
			else if (m_HangGlideState == HangGlideStates.SlerpCameraBackToNormal)
			{
				Transform transform = m_WaypointCur.RetrieveTransform(m_LaneDesired);
				Quaternion rotation2 = transform.rotation;
				float num2 = 0.1f * (m_VelocityMax / m_VelocityMaxInitial);
				num2 *= 60f * m_CappedTimeDeltaTime;
				num2 = Mathf.Clamp(num2, 0f, 1f);
				Quaternion rotation3 = Quaternion.Slerp(base.transform.rotation, rotation2, num2);
				base.transform.rotation = rotation3;
				float valA = Vector3.Dot(transform.forward, base.transform.forward);
				if (MathOps.AboutEqual(valA, 1f, 0.001f))
				{
					m_HangGlideState = HangGlideStates.LerpCameraBackToNormal;
					m_HangGliderAnimations.Play("End");
					PlayHGEndAnimSequence();
				}
			}
			else
			{
				Transform transform2 = m_WaypointCur.RetrieveTransform(m_LaneDesired);
				float y = transform2.eulerAngles.y;
				float value2 = m_WaypointCur.m_SlerpFactor * 60f * m_CappedTimeDeltaTime;
				value2 = Mathf.Clamp(value2, 0f, 1f);
				Quaternion to = Quaternion.Euler(0f, y, 0f);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, to, value2);
			}
		}
	}

	private void LateUpdate()
	{
		if (!(GameManager.The == null) && !GameManager.The.IsGamePaused() && GameManager.currentGameplayState == GameManager.GameplayState.GamePlay_Intro)
		{
			UpdateTubeIntro();
		}
	}

	private void Update()
	{
		if (GameManager.The == null || GameManager.The.IsGamePaused())
		{
			return;
		}
		if (!GameManager.The.IsInGamePlay())
		{
			simpleShadow.gameObject.SetActive(false);
			return;
		}
		m_CappedTimeDeltaTime = Time.deltaTime;
		if (m_CappedTimeDeltaTime > 0.06f)
		{
			m_CappedTimeDeltaTime = 0.06f;
		}
		m_IsBasePositionCalculated = false;
		UpdateControls();
		distanceTraveled = base.transform.localPosition.z;
		if (IsInSlideState())
		{
			Vector3 vector = m_SlidePositionStart - base.transform.position;
			float num = Vector3.Dot(vector, vector);
			if (num > m_SlideDistanceDesired * m_SlideDistanceDesired)
			{
				m_IsSliding = false;
				EndAnimSliding();
			}
		}
		UpdateBringWorldBackToZero();
		UpdateDeathState();
		if (IsInDeathState())
		{
			return;
		}
		if (this.m_ShootingState != null)
		{
			this.m_ShootingState();
		}
		UpdateMovementFromControls();
		UpdateWaypoints();
		UpdateForwardForce();
		UpdateVelocity();
		UpdateLaneConstrainMovement();
		UpdateLaneChangeMovement();
		UpdateAnimationMovements();
		UpdateDistanceTravelled();
		UpdateExternalMovement();
		UpdateFalling();
		UpdateHangGlidingState();
		UpdateSpeedUpDist();
		UpdateFeatherState();
		UpdateCopterBoostState();
		if (!GameManager.The.IsInGamePlay() || m_isInIntro)
		{
			return;
		}
		if (IsInJumpState())
		{
			m_JumpFrameCount++;
		}
		if (m_powerupCooldown)
		{
			m_currentBlinkTime += m_CappedTimeDeltaTime;
			if (m_currentBlinkTime >= m_blinkInterval)
			{
				if (m_invincibilityOn)
				{
					if (m_ForceFieldEffects != null)
					{
						m_ForceFieldEffects.SetActive(!m_ForceFieldEffects.activeSelf);
					}
				}
				else if (Token.ms_IsInMagneticTokenState)
				{
					if (m_MagnetEffects != null)
					{
						m_MagnetEffects.SetActive(!m_MagnetEffects.activeSelf);
					}
				}
				else if (GameManager.The.IsScoreMultiplierOn() && m_MultiplierEffects != null)
				{
					m_MultiplierEffects.SetActive(!m_MultiplierEffects.activeSelf);
				}
				m_currentBlinkTime = 0f;
			}
		}
		if (IsInDeathState())
		{
			return;
		}
		if (IsWaitingForHolsteredToFinish() && IsFinishedAnimBlankToHolstered())
		{
			m_IsWaitingForHolsteredToFinish = false;
			if (m_weapon != null)
			{
				m_weapon.SetActive(false);
			}
		}
		UpdateShootingCoolOff();
	}

	private void UpdateDeathState()
	{
		if (!IsInDeathState())
		{
			return;
		}
		if (Vector3.Dot(m_Velocity, base.transform.forward) > 0f)
		{
			m_Velocity.x *= 0.525f;
			m_Velocity.z *= 0.525f;
		}
		if (IsInDeathFallingState())
		{
			m_fallStartTime += m_CappedTimeDeltaTime;
			AddRelativeVelocityY((0f - m_DeathFallVelocity) * m_CappedTimeDeltaTime);
			Quaternion to = base.transform.rotation * Quaternion.Euler(0f, 180f, 0f);
			float value = m_fallStartTime / m_fallDuration;
			value = Mathf.Clamp(value, 0f, 1f);
			m_currentRunnerModel.transform.rotation = Quaternion.Slerp(base.transform.rotation, to, value);
			if (m_fallStartTime > m_fallDuration)
			{
				m_IsInDeathFallingState = false;
			}
		}
		else if (IsInDeathRisingState())
		{
			if (!m_PerryHelicopter.activeSelf && base.GetComponent<Rigidbody>().velocity.y >= 0f)
			{
				StartAnimHelicopter();
				GameManager.The.PlayLoopClip(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.COPTERLOOP);
			}
			if (m_RiseStartTime > m_RiseDuration)
			{
				m_currentRunnerModel.transform.rotation = base.transform.rotation;
				GameEventManager.TriggerGameOver();
				GameManager.The.StopClipLoop(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.COPTERLOOP);
			}
			m_RiseStartTime += m_CappedTimeDeltaTime;
			AddRelativeVelocityY(m_DeathRiseVelocity * m_CappedTimeDeltaTime);
		}
		UpdateVelocity();
	}

	private void UpdateShootingCoolOff()
	{
		if (m_IsShooting && m_CoolOffTimer.IsFinished())
		{
			m_IsShooting = false;
		}
	}

	private void UpdateJumpTutorial()
	{
		if (m_TutorialState == TutorialState.PauseWaitForJump)
		{
			m_TutorialState = TutorialState.NormalInputLocked;
			TutorialGUIManager.The.CancelAnimSwipe();
		}
	}

	private void UpdateSlideTutorial()
	{
		if (m_TutorialState == TutorialState.PauseWaitForSlide)
		{
			m_TutorialState = TutorialState.None;
			TutorialGUIManager.The.CancelAnimSwipe();
			TutorialGUIManager.The.ShowInstructionText("_GO_", 3f);
			if (PlayerData.JumpStarts > 0)
			{
				HUDGUIManager.The.StartShowJumpStart();
			}
			PlayerData.ShouldNotShowTutorial = true;
			FlurryFacade.Instance.LogEvent("TutorialComplete");
		}
	}

	private void UpdateChangeLaneLeftTutorial()
	{
		if (m_TutorialState == TutorialState.PauseWaitForSwipeLeft)
		{
			m_TutorialState = TutorialState.NormalInputLocked;
			TutorialGUIManager.The.CancelAnimSwipe();
		}
	}

	private void UpdateChangeLaneRightTutorial()
	{
		if (m_TutorialState == TutorialState.PauseWaitForSwipeRight)
		{
			m_TutorialState = TutorialState.NormalInputLocked;
			TutorialGUIManager.The.CancelAnimSwipe();
		}
	}

	private void UpdateFireTutorial()
	{
		if (m_TutorialState == TutorialState.PauseWaitForFire)
		{
			m_TutorialState = TutorialState.None;
			HUDGUIManager.The.ShowForBossTutorial();
			MonogramGUIManager.The.StopTalkBox();
			MonogramGUIManager.The.HideMonogramMonitor();
			TutorialGUIManager.The.CancelHandTouchAnim();
			if (m_canAttack)
			{
				PlayerData.ShouldNotShowBossTutorial = true;
			}
			else
			{
				PlayerData.ShouldNotShowBossTutorialNoGadget = true;
			}
		}
	}

	private bool IsTutorialStateOkToJump()
	{
		if (m_TutorialState == TutorialState.PauseWaitForJump || m_TutorialState == TutorialState.None)
		{
			return true;
		}
		return false;
	}

	private bool IsTutorialStateOkToSlide()
	{
		if (m_TutorialState == TutorialState.PauseWaitForSlide || m_TutorialState == TutorialState.None)
		{
			return true;
		}
		return false;
	}

	private bool IsTutorialStateOkToSwipeLeft()
	{
		if (m_TutorialState == TutorialState.PauseWaitForSwipeLeft || m_TutorialState == TutorialState.None)
		{
			return true;
		}
		return false;
	}

	private bool IsTutorialStateOkToSwipeRight()
	{
		if (m_TutorialState == TutorialState.PauseWaitForSwipeRight || m_TutorialState == TutorialState.None)
		{
			return true;
		}
		return false;
	}

	private bool IsTutorialStateOkToFire()
	{
		if (m_TutorialState == TutorialState.PauseWaitForFire || m_TutorialState == TutorialState.None)
		{
			return true;
		}
		return false;
	}

	private bool ShouldPauseRunnerForTutorial()
	{
		return m_TutorialState != TutorialState.None && m_TutorialState != TutorialState.NormalInputLocked;
	}

	private void UpdateMovementFromControls()
	{
		if (m_jumpOnUpdate)
		{
			if (IsTutorialStateOkToJump())
			{
				UpdateJumpTutorial();
				Jump();
			}
			m_jumpOnUpdate = false;
		}
		if (m_slideOnUpdate)
		{
			if (IsTutorialStateOkToSlide())
			{
				UpdateSlideTutorial();
				Slide();
			}
			m_slideOnUpdate = false;
		}
		if (m_changeLaneLeftOnUpdate)
		{
			if (IsTutorialStateOkToSwipeLeft())
			{
				UpdateChangeLaneLeftTutorial();
				ChangeLaneLeft();
			}
			m_changeLaneLeftOnUpdate = false;
		}
		if (m_changeLaneRightOnUpdate)
		{
			ChangeLaneRight();
			m_changeLaneRightOnUpdate = false;
		}
		if (!m_fireOnUpdate)
		{
			return;
		}
		if (IsTutorialStateOkToFire())
		{
			if (m_canAttack)
			{
				Attack();
			}
			UpdateFireTutorial();
		}
		m_fireOnUpdate = false;
	}

	private void UpdateForceChangeLanesMode()
	{
	}

	private void ForceChangeLanesInInvincibleMode()
	{
		if (m_WaypointCur == null)
		{
			return;
		}
		if (m_WaypointCur.m_LaneToBeIn == Waypoint.Lane.Left)
		{
			ChangeLaneLeft();
		}
		else if (m_WaypointCur.m_LaneToBeIn == Waypoint.Lane.Right)
		{
			ChangeLaneRight();
		}
		else if (m_WaypointCur.m_LaneToBeIn == Waypoint.Lane.LeftOrRight)
		{
			if (m_LaneChangeDir < 0f)
			{
				ChangeLaneLeft();
			}
			else if (m_LaneChangeDir > 0f)
			{
				ChangeLaneRight();
			}
			else if (UnityEngine.Random.Range(0f, 100f) < 50f)
			{
				ChangeLaneLeft();
			}
			else
			{
				ChangeLaneRight();
			}
		}
	}

	private void UpdateBringWorldBackToZero()
	{
		float num = 10000f;
		float num2 = 10000f;
		float x = 0f;
		float z = 0f;
		bool flag = false;
		if (base.transform.position.x > num)
		{
			TransformOps.AddPositionX(base.transform, 0f - num);
			x = num;
			flag = true;
		}
		else if (base.transform.position.x < 0f - num)
		{
			TransformOps.AddPositionX(base.transform, num);
			x = 0f - num;
			flag = true;
		}
		if (base.transform.position.z > num2)
		{
			TransformOps.AddPositionZ(base.transform, 0f - num2);
			z = num2;
			flag = true;
		}
		else if (base.transform.position.z < 0f - num2)
		{
			TransformOps.AddPositionZ(base.transform, num2);
			z = 0f - num2;
			flag = true;
		}
		if (flag)
		{
			if (PlatformManager.The() != null)
			{
				PlatformManager.The().MinusOffMaxWorldBoundary(x, z);
			}
			DoofenCruiser.The().MinusOffMaxWorldBoundary(x, z);
		}
	}

	private void ResetWaypointsBasedOnPosition()
	{
		PlatformManager.The().MoveCurPlatformToFirstPlatform();
		m_WaypointPrev = null;
		m_WaypointCur = null;
		while (IsPassedWaypoint(true))
		{
			if (m_WaypointPrev != null)
			{
				m_WaypointPrev.SetIsRunnerFinished(true);
			}
			m_WaypointPrev = m_WaypointCur;
			m_WaypointCur = PlatformManager.The().RetrieveNextWaypoint(base.transform, m_LaneDesired);
		}
	}

	private void ChangeWaypointHighlightColors()
	{
	}

	private void UpdateLaneAndWaypoints()
	{
		if (m_WaypointPrev != null && m_WaypointPrev.IsPizzaSlice())
		{
			SetPizzaSlicePositionRotation();
		}
		if (IsPassedWaypoint())
		{
			if (m_WaypointPrev != null)
			{
				m_WaypointPrev.SetIsRunnerFinished(true);
			}
			m_WaypointPrev = m_WaypointCur;
			m_WaypointCur = PlatformManager.The().RetrieveNextWaypoint(base.transform, m_LaneDesired);
			UpdateForceChangeLanesMode();
			if (m_WaypointCur != null && m_WaypointCur.m_IsLaneWidthChange)
			{
				MoveRunnerToNewLaneWidth();
			}
			if (m_WaypointPrev != null && m_WaypointPrev.IsPizzaSlice())
			{
				CalcPizzaSlices();
			}
			ChangeWaypointHighlightColors();
			if ((IsInvincible() || IsInEagleState() || IsInCopterBoostState()) && IsWaitingForHardTurnSwipe())
			{
				ForceChangeLanesInInvincibleMode();
			}
		}
		UpdateLaneChangeMovement();
		if (ShouldConstrainLaneTurn())
		{
			ConstrainLaneTurn();
		}
	}

	private void UpdateLaneConstrainMovement()
	{
		if (ShouldConstrainLaneTurn())
		{
			ConstrainLaneTurn();
		}
	}

	private void UpdateWaypoints()
	{
		if (m_WaypointPrev != null && m_WaypointPrev.IsPizzaSlice())
		{
			SetPizzaSlicePositionRotation();
		}
		if (IsPassedWaypoint())
		{
			if (m_WaypointPrev != null)
			{
				m_WaypointPrev.SetIsRunnerFinished(true);
			}
			m_WaypointPrev = m_WaypointCur;
			m_WaypointCur = PlatformManager.The().RetrieveNextWaypoint(base.transform, m_LaneDesired);
			UpdateForceChangeLanesMode();
			if (m_WaypointCur != null && m_WaypointCur.m_IsLaneWidthChange)
			{
				MoveRunnerToNewLaneWidth();
			}
			if (m_WaypointPrev != null && m_WaypointPrev.IsPizzaSlice())
			{
				CalcPizzaSlices();
			}
			ChangeWaypointHighlightColors();
			if ((IsInvincible() || IsInEagleState() || IsInCopterBoostState()) && IsWaitingForHardTurnSwipe())
			{
				ForceChangeLanesInInvincibleMode();
			}
		}
	}

	private void CalcPizzaSlices()
	{
		CalcPizzaSlice(Waypoint.Lane.Left);
		CalcPizzaSlice(Waypoint.Lane.Middle);
		CalcPizzaSlice(Waypoint.Lane.Right);
		m_WaypointPrev.m_SliceAngle = Vector3.Angle(m_WaypointPrev.GetSliceVec(Waypoint.Lane.Middle), m_WaypointCur.GetSliceVec(Waypoint.Lane.Middle));
	}

	private void CalcPizzaSlice(Waypoint.Lane lane)
	{
		Transform transform = m_WaypointCur.RetrieveTransform(lane);
		Transform transform2 = m_WaypointPrev.RetrieveTransform(lane);
		Vector3 vector = Vector3.zero;
		if (m_WaypointPrev.m_PizzaSlice == Waypoint.PizzaSliceType.Left)
		{
			vector = MathOps.IntersectPointLineLine(transform.position, -transform.right, transform2.position, -transform2.right);
		}
		else if (m_WaypointPrev.m_PizzaSlice == Waypoint.PizzaSliceType.Right)
		{
			vector = MathOps.IntersectPointLineLine(transform.position, transform.right, transform2.position, transform2.right);
		}
		else if (m_WaypointPrev.m_PizzaSlice == Waypoint.PizzaSliceType.Up)
		{
			vector = MathOps.IntersectPointLineLine(transform.position, transform.up, transform2.position, transform2.up);
		}
		else if (m_WaypointPrev.m_PizzaSlice == Waypoint.PizzaSliceType.Down)
		{
			vector = MathOps.IntersectPointLineLine(transform.position, -transform.up, transform2.position, -transform2.up);
		}
		m_WaypointCur.SetPizzaCenter(lane, vector);
		m_WaypointPrev.SetPizzaCenter(lane, vector);
		m_WaypointCur.SetSliceVec(lane, transform.position - vector);
		m_WaypointPrev.SetSliceVec(lane, transform2.position - vector);
	}

	private float CalcPizzaLaneChangeDist()
	{
		m_PizzaLaneChangePercentage += (float)m_HorizontalMovePercentage;
		if (m_PizzaLaneChangePercentage >= 1f - MathOps.ms_Epsilon)
		{
			m_IsDoingLaneChange = false;
			m_LaneCur = m_LaneDesired;
		}
		if (m_LaneChangeDir < 0f)
		{
			if (m_WaypointPrev.m_PizzaSlice == Waypoint.PizzaSliceType.Left)
			{
				return (float)((double)(0f - m_PizzaLaneChangePercentage) * m_LaneChangeDist);
			}
			return (float)((double)m_PizzaLaneChangePercentage * m_LaneChangeDist);
		}
		if (m_LaneChangeDir > 0f)
		{
			if (m_WaypointPrev.m_PizzaSlice == Waypoint.PizzaSliceType.Right)
			{
				return (float)((double)(0f - m_PizzaLaneChangePercentage) * m_LaneChangeDist);
			}
			return (float)((double)m_PizzaLaneChangePercentage * m_LaneChangeDist);
		}
		return 0f;
	}

	private void SetPizzaSlicePositionRotation()
	{
		Transform transform = m_WaypointPrev.RetrieveTransform(m_LaneCur);
		float magnitude = m_WaypointCur.GetSliceVec(m_LaneCur).magnitude;
		float magnitude2 = m_WaypointPrev.GetSliceVec(m_LaneCur).magnitude;
		Vector3 pizzaCenter = m_WaypointPrev.GetPizzaCenter(m_LaneCur);
		Vector3 vector = base.transform.position - pizzaCenter;
		float num = Vector3.Angle(m_WaypointPrev.GetSliceVec(m_LaneCur), vector);
		float num2 = num / m_WaypointPrev.m_SliceAngle;
		float num3 = magnitude - magnitude2;
		float num4 = magnitude2 + num2 * num3;
		if (m_IsDoingLaneChange && m_WaypointPrev.IsPizzaSliceHorizontal())
		{
			num4 += CalcPizzaLaneChangeDist();
		}
		if (IsInJumpState())
		{
			if (m_WaypointPrev.m_PizzaSlice == Waypoint.PizzaSliceType.Up)
			{
				num4 += m_JumpPosition;
			}
			if (m_WaypointPrev.m_PizzaSlice == Waypoint.PizzaSliceType.Down)
			{
				num4 -= m_JumpPosition;
			}
		}
		if (IsInDipState())
		{
			if (m_WaypointPrev.m_PizzaSlice == Waypoint.PizzaSliceType.Up)
			{
				num4 += m_DipPosition;
			}
			if (m_WaypointPrev.m_PizzaSlice == Waypoint.PizzaSliceType.Down)
			{
				num4 -= m_DipPosition;
			}
		}
		vector.Normalize();
		Vector3 vector2 = vector * num4;
		base.transform.position = pizzaCenter + vector2;
		Vector3 zero = Vector3.zero;
		if (m_WaypointPrev.IsPizzaSliceHorizontal())
		{
			if (m_WaypointPrev.m_PizzaSlice == Waypoint.PizzaSliceType.Left)
			{
				vector = -vector;
			}
			zero = Vector3.Cross(transform.up, vector);
			base.transform.rotation = Quaternion.LookRotation(zero);
		}
		else
		{
			zero = ((m_WaypointPrev.m_PizzaSlice != Waypoint.PizzaSliceType.Up) ? Vector3.Cross(-transform.right, vector) : Vector3.Cross(transform.right, vector));
			base.transform.rotation = Quaternion.LookRotation(zero);
		}
		float magnitude3 = m_Velocity.magnitude;
		m_Velocity = base.transform.forward * magnitude3;
	}

	private void CalcJumpParameters()
	{
		CalcJumpAcceleration();
		CalcJumpGravity(m_JumpAcceleration);
	}

	private void CalcJumpAcceleration()
	{
		m_JumpAcceleration = 4f * m_JumpPeakDesired * m_VelocityMax / m_JumpDistanceDesired;
	}

	private void CalcJumpGravity(float jumpAcceleration)
	{
		m_JumpGravity = -0.5f / m_JumpPeakDesired * (jumpAcceleration * jumpAcceleration);
	}

	private void CalcHGJumpParameters()
	{
		m_JumpAcceleration = 4f * m_HGJumpPeakDesired * m_VelocityMax / m_HGJumpDistanceDesired;
		m_JumpGravity = -0.5f / m_HGJumpPeakDesired * (m_JumpAcceleration * m_JumpAcceleration);
	}

	private void CalcHGDipParameters()
	{
		m_DipAcceleration = 4f * m_HGDipPeakDesired * m_VelocityMax / m_HGDipDistanceDesired;
		m_DipGravity = -0.5f / m_HGDipPeakDesired * (m_DipAcceleration * m_DipAcceleration);
	}

	public float GetColliderCenterY()
	{
		return m_collider.center.y;
	}

	private bool IsInAir()
	{
		return m_IsInAir;
	}

	private void FinishInAir()
	{
		if (!m_IsInAir)
		{
			return;
		}
		m_IsInAir = false;
		if (!IsInSlideState())
		{
			if (!IsInHangGlideState())
			{
				GameManager.The.PlayClip(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.GROUNDHIT + UnityEngine.Random.Range(1, AudioClipFiles.NUMBEROFGROUNDHITCLIPS + 1));
			}
			EndAnimJump();
		}
	}

	private void CancelJump()
	{
		m_JumpVelocity = 0f - m_JumpAcceleration;
	}

	private void CancelDip()
	{
		m_DipVelocity = 0f - m_DipAcceleration;
	}

	private void FinishJump()
	{
		if (m_IsJumping)
		{
			m_JumpDist = (base.transform.position - m_JumpStartPos).magnitude;
			m_JumpDuration = Time.time - m_JumpStartTime;
			m_IsJumping = false;
			m_IsJumpingHitPeak = false;
			m_IsDoingSuperJump = false;
			m_JumpPosition = 0f;
			m_JumpVelocity = 0f;
		}
	}

	private void FinishDip()
	{
		if (m_IsDipping)
		{
			m_DipDist = (base.transform.position - m_DipStartPos).magnitude;
			m_DipDuration = Time.time - m_DipStartTime;
			m_DipPosition = 0f;
			m_DipVelocity = 0f;
			m_IsDipping = false;
			m_IsDippingHitPeak = false;
		}
	}

	private void UpdateJumpVars(float deltaJumpY)
	{
		m_JumpPosition += deltaJumpY;
		m_JumpVelocity += m_JumpGravity * m_CappedTimeDeltaTime;
		if (m_JumpPosition < 0f)
		{
			FinishJump();
			m_JumpPosition = 0f;
			m_JumpVelocity = 0f;
		}
	}

	private void UpdateDipVars(float deltaDipY)
	{
		if (m_JumpPeak < m_JumpPosition)
		{
			m_JumpPeak = m_JumpPosition;
		}
		m_DipPosition += deltaDipY;
		m_DipVelocity += m_DipGravity * m_CappedTimeDeltaTime;
		if (m_DipPosition > 0f)
		{
			FinishDip();
		}
	}

	private void UpdateAnimationMovements()
	{
		m_collider.height = GetAnimColliderHeight();
		if (IsInSlideState())
		{
			m_collider.center = new Vector3(m_collider.center.x, GetAnimColliderY(), m_collider.center.z);
		}
		else
		{
			m_collider.center = new Vector3(m_collider.center.x, m_colliderStartHeight, m_collider.center.z);
		}
		if (IsInJumpState())
		{
			if (m_JumpPeak > m_JumpPosition && !m_IsJumpingHitPeak)
			{
				m_IsJumpingHitPeak = true;
				if (!IsInSlideState())
				{
					StartAnimJumpUpToDown();
				}
			}
		}
		else if (IsInDipState() && m_DipPeak < m_DipPosition && !m_IsDippingHitPeak)
		{
			m_IsDippingHitPeak = true;
			if (m_anim.IsPlaying("DiveLoop"))
			{
				EndAnimHGDive();
				StartAnimHGLoopQueued();
			}
		}
	}

	private void UpdateDistanceTravelled()
	{
		float num = Vector3.Dot(base.transform.forward, m_Velocity) * m_CappedTimeDeltaTime;
		num *= 0.15f;
		m_distance += num;
		if (IsTouchingGround() && m_distance - m_lastStepSoundTaken > 5f)
		{
			m_lastStepSoundTaken = m_distance;
		}
		if (m_distance - m_lastDistance > 10f)
		{
			GameEventManager.TriggerScoreUpdate(25);
			m_lastDistance = m_distance;
		}
		PlayerData.RoundMeters = (int)(m_distance + 0.5f);
		PlayerData.AllTimeMeters = (int)(m_distance + 0.5f);
	}

	private Vector3 CalcVelocity()
	{
		return base.transform.forward * m_Speed;
	}

	private void AddRelativeVelocityY(float delta)
	{
		m_Velocity += base.transform.up * delta;
	}

	private void AddRelativeVelocityZ(float delta)
	{
		m_Velocity += base.transform.forward * delta;
	}

	private void UpdateForwardForce()
	{
		if (m_IsBouncing && Vector3.Dot(m_Velocity, base.transform.forward) > 0f)
		{
			m_IsBouncing = false;
		}
		if (ShouldPauseRunnerForTutorial())
		{
			m_Velocity *= 0.5f;
		}
		else
		{
			AddRelativeVelocityZ(acceleration * m_CappedTimeDeltaTime);
		}
		if (IsInCopterBoostState())
		{
			m_Velocity = TransformOps.ConstrainVelocity(m_Velocity, m_VelocityMax * 2f);
		}
		else
		{
			m_Velocity = TransformOps.ConstrainVelocity(m_Velocity, m_VelocityMax);
		}
	}

	private void UpdateVelocity()
	{
		base.transform.position += m_Velocity * m_CappedTimeDeltaTime;
	}

	private void UpdateExternalMovement()
	{
		if (IsInDeathState())
		{
			simpleShadow.gameObject.SetActive(false);
			return;
		}
		m_IsTouchingGround = false;
		if (IsInCopterBoostState())
		{
			simpleShadow.gameObject.SetActive(false);
		}
		else if (m_HangGlideState != HangGlideStates.BeenLaunchedStart && m_HangGlideState != HangGlideStates.BeenLaunchedLoop && m_HangGlideState != HangGlideStates.FinishedLaunch && m_HangGlideState != HangGlideStates.CruisingToHangGlideArea)
		{
			Transform transform = null;
			transform = ((!m_WaypointCur.m_FirstInRamp) ? m_WaypointCur.RetrieveTransform(m_LaneDesired) : m_WaypointPrev.RetrieveTransform(m_LaneDesired));
			Vector3 vector = MathOps.IntersectPointLineLine(transform.position, transform.forward, base.transform.position, Vector3.down);
			if (!m_IsJumping)
			{
				if (!m_IsDipping && m_EagleState != EagleState.Carrying && !IsInCopterBoostState())
				{
					float y = -90f * m_CappedTimeDeltaTime;
					TransformOps.AddPositionY(base.transform, y);
				}
			}
			else
			{
				if (m_JumpPeak < base.transform.position.y - vector.y)
				{
					m_JumpPeak = base.transform.position.y - vector.y;
				}
				float num = m_JumpVelocity * m_CappedTimeDeltaTime;
				UpdateJumpVars(num);
				TransformOps.AddPositionY(base.transform, num);
			}
			if (IsInDipState())
			{
				if (m_DipPeak > base.transform.position.y - vector.y)
				{
					m_DipPeak = base.transform.position.y - vector.y;
				}
				float num2 = m_DipVelocity * m_CappedTimeDeltaTime;
				UpdateDipVars(num2);
				TransformOps.AddPositionY(base.transform, num2);
				if (!IsInJumpState() && base.transform.position.y > vector.y)
				{
					TransformOps.SetPositionY(base.transform, vector.y);
					FinishDip();
				}
			}
			else if ((m_WaypointPrev == null || (m_WaypointPrev != null && m_WaypointPrev.m_PizzaSlice != Waypoint.PizzaSliceType.Down)) && base.transform.position.y < vector.y)
			{
				m_IsTouchingGround = true;
				m_IsFalling = false;
				TransformOps.SetPositionY(base.transform, vector.y);
				FinishInAir();
				FinishJump();
			}
			if (IsInHangGlideState() || IsInEagleState() || IsInDipState() || IsInDeathState() || IsInAir())
			{
				simpleShadow.gameObject.SetActive(false);
				return;
			}
			simpleShadow.gameObject.SetActive(true);
			simpleShadow.SetPosition(new Vector3(base.transform.position.x, vector.y + 0.2f, base.transform.position.z), transform.forward, transform.up);
		}
		else
		{
			simpleShadow.gameObject.SetActive(false);
		}
	}

	public void IncSpeedUp()
	{
		if (m_SpeedUpCount < m_SpeedUpCountMax)
		{
			m_SpeedUpCount++;
			m_SpeedUpDist = 0f;
			m_VelocityMax *= m_speedIncreaseMultiple;
			m_animationSpeed *= m_animSpeedIncreaseMultiple;
			AnimationOps.SetAnimationSpeed(m_anim, m_animationSpeed);
			DoofenCruiser.The().SetAttackAnimationSpeeds(m_animationSpeed);
			DoofenCruiser.The().SetTimerReductionFactors(m_SpeedUpCount, m_SpeedUpCountMax);
			if (m_HangGlideState == HangGlideStates.None)
			{
				CalcJumpParameters();
				return;
			}
			CalcHGJumpParameters();
			CalcHGDipParameters();
		}
	}

	public void DecSpeedUp()
	{
		if (m_SpeedUpCount > 0)
		{
			m_SpeedUpCount--;
			m_SpeedUpDist = 0f;
			m_VelocityMax /= m_speedIncreaseMultiple;
			m_animationSpeed /= m_animSpeedIncreaseMultiple;
			AnimationOps.SetAnimationSpeed(m_anim, m_animationSpeed);
			DoofenCruiser.The().SetAttackAnimationSpeeds(m_animationSpeed);
			DoofenCruiser.The().SetTimerReductionFactors(m_SpeedUpCount, m_SpeedUpCountMax);
			if (m_HangGlideState == HangGlideStates.None)
			{
				CalcJumpParameters();
				return;
			}
			CalcHGJumpParameters();
			CalcHGDipParameters();
		}
	}

	private void UpdateSpeedUpDist()
	{
		m_SpeedUpDist += m_Velocity.magnitude / 100f;
		if (m_SpeedUpDist > m_SpeedUpThreshold)
		{
			IncSpeedUp();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
	}

	private void OnCollisionExit(Collision collision)
	{
	}

	public void MakeRunnerFall()
	{
		FinishJump();
		m_IsSliding = false;
		PowerUpFeatherOffListener();
		GameManager.The.PlayClip(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.FALL);
		StartAnimFalling();
		m_IsDying = true;
		m_IsInDeathFallingState = true;
	}

	private void MakeRunnerCollide(ObstacleHitTrigger obstacle)
	{
		if (IsInvincible())
		{
			if (!m_IsBouncing && !m_DidReverseLaneChange)
			{
				m_Velocity = Vector3.zero;
				float num = 50f;
				m_IsBouncing = true;
				m_HasSwipedForHardTurn = false;
				PlayerData.RoundBouncesWhileInvincible++;
				AddRelativeVelocityZ((0f - num) * 2f);
			}
			return;
		}
		m_IsDying = true;
		FinishJump();
		m_IsSliding = false;
		PowerUpFeatherOffListener();
		if (IsInHangGlideState())
		{
			FinishDip();
			m_anim.CrossFade("Fall", 0.2f);
			AnimationState animationState = m_anim.CrossFadeQueued("HelicopterLoop", m_fallDuration, QueueMode.PlayNow);
			animationState.speed = m_animationSpeed;
			m_HangGlider.transform.parent = null;
			m_IsInDeathFallingState = true;
			m_Velocity = Vector3.zero;
			float num2 = 4f;
			AddRelativeVelocityZ((0f - num2) * 2f);
		}
		else
		{
			StartAnimHitObstacle();
			GameEventManager.TriggerGameOver();
		}
	}

	private bool IsWaitingForHolsteredToFinish()
	{
		return m_IsWaitingForHolsteredToFinish;
	}

	private void MakeRunnerShoot()
	{
		if (!(m_weaponParticle == null))
		{
			this.m_ShootingState = ShootingStateWaitingToStartWeaponParticle;
			PlayerData.RoundGadgetUses++;
			if (IsInAnimDrawnLoop())
			{
				StartAnimWeaponDrawnToAim();
			}
			else
			{
				StartAnimRecoil();
			}
		}
	}

	private void ShootingStateWaitingToStartWeaponParticle()
	{
		if (!m_anim.IsPlaying("WeaponDrawnToAim"))
		{
			if (m_weaponParticle != null && m_target != null)
			{
				GameManager.The.PlayClip(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.BEAMSHOT + UnityEngine.Random.Range(1, AudioClipFiles.NUMBEROFBEAMSHOTS));
				m_weaponParticle.transform.LookAt(m_target.transform);
				m_weaponParticle.gameObject.SetActive(true);
				m_ShootingTimer.Start(m_weaponShotDuration);
			}
			this.m_ShootingState = ShootingStateShowingWeaponParticle;
		}
	}

	private void ShootingStateShowingWeaponParticle()
	{
		if (m_weaponParticle != null)
		{
			if (m_target != null)
			{
				m_weaponParticle.transform.LookAt(m_target.transform);
			}
			if (m_ShootingTimer.IsFinished())
			{
				GameEventManager.TriggerBossDamage(m_weaponDamage);
				m_weaponParticle.gameObject.SetActive(false);
				this.m_ShootingState = ShootingStateWaitingForWeaponRecoilToFinish;
			}
		}
	}

	private void ShootingStateWaitingForWeaponRecoilToFinish()
	{
		if (!m_anim.IsPlaying("WeaponRecoil"))
		{
			this.m_ShootingState = null;
		}
	}

	private void StopShooting()
	{
		if (!(m_weaponParticle == null))
		{
			m_weaponParticle.gameObject.SetActive(false);
		}
	}

	private void OnTriggerEnter(Collider trigger)
	{
		if (IsInCopterBoostState())
		{
			return;
		}
		ObstacleHitTrigger component = trigger.gameObject.GetComponent<ObstacleHitTrigger>();
		if (!(component != null))
		{
			return;
		}
		string obstacletag = component.tag;
		if (component.tag == "Untagged")
		{
			obstacletag = component.transform.parent.tag;
		}
		GameManager.The.DoObstacleSound(obstacletag);
		m_ShouldUseDeathPlacementDistance = component.m_ShouldUseDeathPlacementDistance;
		m_DeathPlacementDistance = component.m_DeathPlacementDistance;
		if (component.m_RunnerResponse == ObstacleHitTrigger.RunnerResponse.Fall)
		{
			if (!IsInDeathState())
			{
				MakeRunnerFall();
			}
		}
		else if (component.m_RunnerResponse == ObstacleHitTrigger.RunnerResponse.Collide && !IsInDeathState())
		{
			MakeRunnerCollide(component);
		}
	}

	private void ResetPlatformData()
	{
		m_WaypointCur = null;
		m_WaypointPrev = null;
	}

	private void ResetLaneChange()
	{
		m_DidReverseLaneChange = false;
		m_IsDoingLaneChange = false;
		m_LaneChangeDist = 0.0;
		m_LaneChangeDistMoved = 0.0;
		m_SpeedOfLaneChange = m_SpeedOfLaneChangeDefault;
	}

	private void ResetMovement()
	{
		ResetLaneChange();
		m_LaneDesired = Waypoint.Lane.Middle;
		m_LaneCur = m_LaneDesired;
		base.transform.localPosition = startPosition;
		base.GetComponent<Rigidbody>().isKinematic = false;
		base.GetComponent<Rigidbody>().velocity = Vector3.zero;
		m_Velocity = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		m_HasSwipedForHardTurn = false;
		m_fallStartTime = 0f;
		m_RiseStartTime = 0f;
		m_distance = 0f;
		m_lastDistance = 0f;
		m_VelocityMax = m_VelocityMaxInitial;
		m_SpeedUpCount = 0;
		m_SpeedUpDist = 0f;
		distanceTraveled = 0f;
		m_velocityAtPause = Vector3.zero;
		m_ShouldUseDeathPlacementDistance = false;
		m_DeathPlacementDistance = 0f;
		m_IsInSuperJumpTrigger = false;
		if (m_currentRunnerModel != null)
		{
			m_currentRunnerModel.transform.localPosition = Vector3.zero;
		}
	}

	private void StartHGAnimLiftStart()
	{
		m_anim.Rewind("LiftStart");
		m_anim.Play("LiftStart");
	}

	private void StartHGAnimLiftLoop()
	{
		m_anim.Rewind("LiftLoop");
		AnimationState animationState = m_anim.PlayQueued("LiftLoop");
		animationState.speed = m_animationSpeed;
	}

	private void StartHGAnimLiftEnd()
	{
		m_anim.Rewind("LiftEnd");
		m_anim.Play("LiftEnd");
	}

	private void CrossFadeHGAnimLiftEnd(float duration)
	{
		m_anim.Rewind("LiftEnd");
		m_anim.CrossFade("LiftEnd", duration);
	}

	private void StartHGAnimRoofToHGStart()
	{
		m_anim.Rewind("RoofToHGStart");
		m_anim.Play("RoofToHGStart");
	}

	private bool IsFinishedHGAnimRoofToHGStart()
	{
		return !m_anim.IsPlaying("RoofToHGStart");
	}

	private void StartHGAnimRoofToHGLoop()
	{
		m_anim.Rewind("RoofToHGLoop");
		AnimationState animationState = m_anim.PlayQueued("RoofToHGLoop");
		animationState.speed = m_animationSpeed;
	}

	private bool IsFinishedHGAnimRoofToHGEnd()
	{
		return !m_anim.IsPlaying("RoofToHGEnd");
	}

	private void StartHGAnimRoofToHGEnd()
	{
		m_anim.Rewind("RoofToHGEnd");
		m_anim.Play("RoofToHGEnd");
		m_anim.Rewind("HGLoop");
		AnimationState animationState = m_anim.PlayQueued("HGLoop");
		animationState.speed = m_animationSpeed;
		m_HangGlider.SetActive(true);
		m_HangGliderAnimations.Rewind("Start");
		m_HangGliderAnimations.Play("Start");
	}

	private void StartHGAnimCruising()
	{
		StartAnimHGLoop();
		m_HangGlider.SetActive(true);
		m_HangGliderAnimations.Rewind("Loop");
		m_HangGliderAnimations.Play("Loop");
	}

	private void StopHangGlideAnimated()
	{
		m_anim.Stop();
		m_HangGlider.SetActive(false);
	}

	private void ResetHGStuff()
	{
		m_HangGlideState = HangGlideStates.None;
		StopHangGlideAnimated();
		m_HangGlider.SetActive(false);
	}

	private void ResetHelicopter()
	{
		m_PerryHelicopter.SetActive(false);
	}

	private void ResetBossStuff()
	{
		m_canAttack = false;
		m_fireWeaponParticle.gameObject.SetActive(false);
		m_waterWeaponParticle.gameObject.SetActive(false);
		m_electricWeaponParticle.gameObject.SetActive(false);
		m_pinWeaponParticle.gameObject.SetActive(false);
		m_GunFire.SetActive(false);
		m_GunWater.SetActive(false);
		m_GunZap.SetActive(false);
		m_GunPin.SetActive(false);
		PlayerData.m_currentGadgetType = PlayerData.GadgetTypes.None;
	}

	private void ResetAnimSpeed()
	{
		m_animationSpeed = m_startAnimSpeed;
		if (m_anim != null)
		{
			AnimationOps.SetAnimationSpeed(m_anim, m_animationSpeed);
		}
		DoofenCruiser.The().SetAllAnimationSpeeds(m_animationSpeed);
	}

	private void ResetAnimStuff()
	{
		m_animationSpeed = m_startAnimSpeed;
		if (m_anim != null)
		{
			m_anim.Stop();
			AnimationOps.SetAnimationSpeed(m_anim, m_animationSpeed);
		}
		DoofenCruiser.The().SetAllAnimationSpeeds(m_animationSpeed);
	}

	private void ResetPowerUpEffects()
	{
		if (m_ForceFieldEffects != null)
		{
			m_ForceFieldEffects.SetActive(false);
		}
		if (m_MagnetEffects != null)
		{
			m_MagnetEffects.SetActive(false);
		}
		if (m_MultiplierEffects != null)
		{
			m_MultiplierEffects.SetActive(false);
		}
	}

	private void ResetPowerups()
	{
		m_invincibilityOn = false;
		m_powerupCooldown = false;
		ResetPowerUpEffects();
	}

	private void ResetControlStuff()
	{
		m_reverseControls = false;
	}

	private void ResetAllVars()
	{
		ResetMovement();
		ResetHGStuff();
		ResetHelicopter();
		ResetBossStuff();
		ResetAnimStuff();
		ResetPowerups();
		ResetControlStuff();
		ResetPlatformData();
		CalcJumpParameters();
	}

	public void GameIntro()
	{
		m_isInIntro = true;
		Init();
		m_IsBasePositionCalculated = false;
		ResetMovement();
		SelectRunnerModel();
		ResetHGStuff();
		ResetBossStuff();
		ResetAnimStuff();
		ResetPowerups();
		ResetControlStuff();
		ResetPlatformData();
		CalcJumpParameters();
		ResetEagleState();
		ResetCopterState();
		if (!PlayerData.ShouldNotShowTutorial)
		{
			TutorialGUIManager.The.CacheInstructionText("_TUTORIAL_WELCOME_");
		}
		GlobalGUIManager.The.ShowHUDOnGameIntro();
		StartTubeIntro();
	}

	private void GameStart()
	{
		m_isInIntro = false;
		m_IsBasePositionCalculated = false;
		ResetMovement();
		ResetHGStuff();
		ResetHelicopter();
		ResetBossStuff();
		ResetAnimStuff();
		ResetPowerups();
		ResetControlStuff();
		ResetPlatformData();
		CalcJumpParameters();
		ResetEagleState();
		ResetCopterState();
		m_IsDying = false;
		m_IsBouncing = false;
		m_IsInDeathFallingState = false;
		GlobalGUIManager.The.ShowHUDonGameStart();
		StartAnimFromInAirToRun();
		m_tube.SetActive(false);
		m_TutorialState = TutorialState.None;
		if (!PlayerData.ShouldNotShowTutorial)
		{
			TutorialGUIManager.The.ShowCachedInstructionText(4f, false);
			m_TutorialState = TutorialState.NormalInputLocked;
		}
	}

	private void GameOver()
	{
		base.GetComponent<Rigidbody>().isKinematic = true;
		GameEventManager.TriggerInvincibilityOff();
		GameEventManager.TriggerPowerUpMagnetOff();
		ResetEagleState();
		DisableAllAttachments();
	}

	private void GoToGameRestartMenuListener()
	{
		ResetEagleState();
		ResetCopterState();
		ResetAnimSpeed();
	}

	private bool CanPlaceRunnerInLaneContinue(int laneLocation, Waypoint waypoint)
	{
		return (waypoint.m_LaneForPlacementOnDeath & laneLocation) == laneLocation;
	}

	private Vector3 PlaceRunnerStartPosition(int laneLocation, Waypoint waypoint)
	{
		Vector3 position = waypoint.transform.position;
		m_LaneCur = Waypoint.Lane.Middle;
		if ((laneLocation & LaneLocations.M) == LaneLocations.M)
		{
			m_LaneCur = Waypoint.Lane.Middle;
		}
		else if ((laneLocation & LaneLocations.L) == LaneLocations.L)
		{
			position = waypoint.m_LeftLane.transform.position;
			m_LaneCur = Waypoint.Lane.Left;
		}
		else if ((laneLocation & LaneLocations.R) == LaneLocations.R)
		{
			position = waypoint.m_RightLane.transform.position;
			m_LaneCur = Waypoint.Lane.Right;
		}
		return CalcRunnerLaneBasePosition(m_LaneCur);
	}

	private void PlaceRunnerAtXZContinuePosition()
	{
		Vector3 position = base.transform.position;
		Vector3 forward = base.transform.forward;
		forward.y = 0f;
		forward.Normalize();
		if (m_ShouldUseDeathPlacementDistance)
		{
			forward *= m_DeathPlacementDistance;
		}
		else
		{
			forward *= -60f;
		}
		position += forward;
		base.transform.position = position;
	}

	private Vector3 CalcStartPositionForContinueRunner(Waypoint waypoint)
	{
		Vector3 result = base.transform.position;
		if (waypoint.m_LaneForPlacementOnDeath != 0)
		{
			if (m_LaneCur == Waypoint.Lane.Left)
			{
				result = ((!CanPlaceRunnerInLaneContinue(LaneLocations.L, waypoint)) ? PlaceRunnerStartPosition(waypoint.m_LaneForPlacementOnDeath, waypoint) : PlaceRunnerStartPosition(LaneLocations.L, waypoint));
			}
			else if (m_LaneCur == Waypoint.Lane.Middle)
			{
				result = ((!CanPlaceRunnerInLaneContinue(LaneLocations.M, waypoint)) ? PlaceRunnerStartPosition(waypoint.m_LaneForPlacementOnDeath, waypoint) : PlaceRunnerStartPosition(LaneLocations.M, waypoint));
			}
			else if (m_LaneCur == Waypoint.Lane.Right)
			{
				result = ((!CanPlaceRunnerInLaneContinue(LaneLocations.R, waypoint)) ? PlaceRunnerStartPosition(waypoint.m_LaneForPlacementOnDeath, waypoint) : PlaceRunnerStartPosition(LaneLocations.R, waypoint));
			}
		}
		else
		{
			result = CalcRunnerLaneBasePosition(m_LaneCur);
		}
		m_ShouldUseDeathPlacementDistance = false;
		m_DeathPlacementDistance = 0f;
		return result;
	}

	private void GameContinueEvent()
	{
		m_HasSwipedForHardTurn = false;
		m_isInIntro = false;
		Vector3 localPosition = base.gameObject.transform.position;
		PlaceRunnerAtXZContinuePosition();
		ResetWaypointsBasedOnPosition();
		if (m_WaypointCur != null)
		{
			localPosition = CalcStartPositionForContinueRunner(m_WaypointCur);
		}
		else
		{
			Debug.LogError("GameContinueEvent() m_WaypointCur == null ---This should NEVER happen");
		}
		base.transform.localPosition = localPosition;
		ResetLaneChange();
		if (IsWaitingForHardTurnSwipe())
		{
			ForceChangeLanesInInvincibleMode();
		}
		m_LaneDesired = m_LaneCur;
		base.GetComponent<Rigidbody>().isKinematic = false;
		base.gameObject.SetActive(false);
		base.gameObject.SetActive(true);
		m_fallStartTime = 0f;
		m_RiseStartTime = 0f;
		m_IsDying = false;
		m_IsBouncing = false;
		m_IsInDeathFallingState = false;
		m_IsSliding = false;
		m_Velocity = Vector3.zero;
		if (m_PerryHelicopter != null)
		{
			m_PerryHelicopter.SetActive(false);
		}
		if (m_canAttack && m_weapon != null)
		{
			m_weapon.SetActive(true);
		}
		m_powerupCooldown = false;
		if (IsInHangGlideState())
		{
			m_HangGlider.transform.parent = m_HGAttachTransform;
			m_HangGlider.transform.localPosition = Vector3.zero;
			m_HangGlider.transform.localRotation = Quaternion.identity;
			StartHGAnimCruising();
		}
		else if (m_canAttack)
		{
			StartAnimWeaponDrawn();
		}
		else
		{
			StartAnimRun();
		}
	}

	private void GamePause()
	{
		m_velocityAtPause = m_Velocity;
		base.GetComponent<Rigidbody>().isKinematic = true;
		GameManager.The.PauseMusic();
	}

	private void GameUnPause()
	{
		base.GetComponent<Rigidbody>().isKinematic = false;
		m_Velocity = m_velocityAtPause;
		GameManager.The.ResumeMusic();
		if (DoofenCruiser.The().IsInTargetMode() || (Balloony.The != null && Balloony.The.IsInTargetMode()))
		{
			MakeRunnerDrawWeapon(DoofenCruiser.The().m_WeaponChosen);
		}
	}

	private void BossTargetVisible(DoofenCruiser.WeaponType weaponType)
	{
		if (weaponType == DoofenCruiser.WeaponType.Pin)
		{
			if (Balloony.The != null)
			{
				m_target = Balloony.The.transform.Find("BalloonyMesh/TargetToShootAt").gameObject;
			}
		}
		else
		{
			m_target = DoofenCruiser.The().transform.Find("R:S:Root_Joint/R:S:Main_Joint/R:S:Target_Joint").gameObject;
		}
		MakeRunnerDrawWeapon(weaponType);
	}

	private void MakeRunnerDrawWeapon(DoofenCruiser.WeaponType weaponType)
	{
		m_canAttack = false;
		m_weapon = null;
		PlayerData.m_currentGadgetType = PlayerData.GadgetTypes.None;
		switch (weaponType)
		{
		case DoofenCruiser.WeaponType.Fire:
			if (PlayerData.hasWaterWeapon)
			{
				PlayerData.m_currentGadgetType = PlayerData.GadgetTypes.Water;
				m_weapon = m_GunWater;
				m_weaponParticle = m_waterWeaponParticle;
				m_weaponShotDuration = 0.2f;
				if (m_weapon != null)
				{
					m_weapon.SetActive(true);
				}
				StartAnimWeaponDraw();
				m_canAttack = true;
			}
			break;
		case DoofenCruiser.WeaponType.Ice:
			if (PlayerData.hasFireWeapon)
			{
				PlayerData.m_currentGadgetType = PlayerData.GadgetTypes.Fire;
				m_weapon = m_GunFire;
				m_weaponParticle = m_fireWeaponParticle;
				m_weaponShotDuration = 0.2f;
				if (m_weapon != null)
				{
					m_weapon.SetActive(true);
				}
				StartAnimWeaponDraw();
				m_canAttack = true;
			}
			break;
		case DoofenCruiser.WeaponType.Water:
			if (PlayerData.hasElectricWeapon)
			{
				PlayerData.m_currentGadgetType = PlayerData.GadgetTypes.Electric;
				m_weapon = m_GunZap;
				m_weaponParticle = m_electricWeaponParticle;
				m_weaponShotDuration = 0.2f;
				if (m_weapon != null)
				{
					m_weapon.SetActive(true);
				}
				StartAnimWeaponDraw();
				m_canAttack = true;
			}
			break;
		case DoofenCruiser.WeaponType.Pin:
			if (PlayerData.HasPinWeapon)
			{
				PlayerData.m_currentGadgetType = PlayerData.GadgetTypes.PinShooter;
				m_weapon = m_GunPin;
				m_weaponParticle = m_pinWeaponParticle;
				m_weaponShotDuration = 0.5f;
				if (m_weapon != null)
				{
					m_weapon.SetActive(true);
				}
				StartAnimWeaponDraw();
				m_canAttack = true;
			}
			break;
		}
	}

	public void PlayFireTutorial(MiniGameManager.BossType bossType)
	{
		if (m_canAttack)
		{
			if (!PlayerData.ShouldNotShowBossTutorial)
			{
				SetTutorialState(TutorialState.PauseWaitForFire);
				HUDGUIManager.The.HideForBossTutorial();
				MonogramGUIManager.The.ShowBossTutorial();
				MonogramGUIManager.The.StartTalkBoxGameIntroPos(LocalTextManager.GetUIText("_TAP_TO_SHOOT_"), true);
				TutorialGUIManager.The.ShowHandTouchAnim(0, true);
			}
		}
		else
		{
			if (PlayerData.ShouldNotShowBossTutorialNoGadget)
			{
				return;
			}
			SetTutorialState(TutorialState.PauseWaitForFire);
			HUDGUIManager.The.HideForBossTutorial();
			MonogramGUIManager.The.ShowBossTutorial();
			switch (bossType)
			{
			case MiniGameManager.BossType.DoofenCruiser:
				MonogramGUIManager.The.StartTalkBoxGameIntroPos(LocalTextManager.GetUIText("_BLASTER_TUTORIAL_"), true, 37);
				if (LocalTextManager.isAsianLanguageActive)
				{
					MonogramGUIManager.The.StartTalkBoxGameIntroPos(LocalTextManager.GetUIText("_BLASTER_TUTORIAL_"), true, 22);
				}
				break;
			case MiniGameManager.BossType.Balloony:
				MonogramGUIManager.The.StartTalkBoxGameIntroPos(LocalTextManager.GetUIText("_BLASTER_TUTORIAL_BALLOONY_"), true, 37);
				if (LocalTextManager.isAsianLanguageActive)
				{
					MonogramGUIManager.The.StartTalkBoxGameIntroPos(LocalTextManager.GetUIText("_BLASTER_TUTORIAL_BALLOONY_"), true, 22);
				}
				break;
			default:
				throw new ArgumentOutOfRangeException("bossType");
			}
		}
	}

	private void BossTargetInvisible(DoofenCruiser.WeaponType weaponType)
	{
		if (IsInAnimAimLoop())
		{
			StartAnimWeaponAimToHolstered();
		}
		else if (IsInAnimDrawnLoop())
		{
			StartAnimWeaponDrawnToHolstered();
		}
		m_IsWaitingForHolsteredToFinish = true;
		m_canAttack = false;
	}

	private void BossDoneEvent()
	{
		if (IsInAnimAimLoop())
		{
			StartAnimWeaponAimToHolstered();
		}
		else if (IsInAnimDrawnLoop())
		{
			StartAnimWeaponDrawnToHolstered();
		}
		m_IsWaitingForHolsteredToFinish = true;
		m_canAttack = false;
	}

	private void BossDeadEvent(MiniGameManager.BossType bossType)
	{
		if (IsInAnimAimLoop())
		{
			StartAnimWeaponAimToHolstered();
		}
		else if (IsInAnimDrawnLoop())
		{
			StartAnimWeaponDrawnToHolstered();
		}
		m_IsWaitingForHolsteredToFinish = true;
		m_canAttack = false;
	}

	private void BossBehindAttackEvent()
	{
		m_reverseControls = true;
	}

	private void BossFrontAttackEvent()
	{
		m_reverseControls = false;
	}

	private void CollectScoreMultiplierListener()
	{
		GameManager.The.PlayLoopClip(AudioClipFiles.POWERUPFOLDER + AudioClipFiles.SCOREMULTIPLIER);
		if (m_MultiplierEffects != null)
		{
			m_MultiplierEffects.SetActive(true);
		}
	}

	private void ScoreMultiplierOffListener()
	{
		GameManager.The.StopClipLoop(AudioClipFiles.POWERUPFOLDER + AudioClipFiles.SCOREMULTIPLIER);
		if (m_MultiplierEffects != null)
		{
			m_MultiplierEffects.SetActive(false);
		}
	}

	private void CollectInvincibilityListener()
	{
		m_invincibilityOn = true;
		m_powerupCooldown = false;
		GameManager.The.PlayLoopClip(AudioClipFiles.POWERUPFOLDER + AudioClipFiles.INVINCIBLE);
		if (m_ForceFieldEffects != null)
		{
			m_ForceFieldEffects.SetActive(true);
		}
	}

	private void InvincibilityCooldownListener()
	{
		m_powerupCooldown = true;
		m_currentBlinkTime = 0f;
	}

	private void InvincibilityOffListener()
	{
		m_invincibilityOn = false;
		m_powerupCooldown = false;
		GameManager.The.StopClipLoop(AudioClipFiles.POWERUPFOLDER + AudioClipFiles.INVINCIBLE);
		if (m_ForceFieldEffects != null)
		{
			m_ForceFieldEffects.SetActive(false);
		}
	}

	private bool IsInEagleStateCarrying()
	{
		return m_EagleState == EagleState.Carrying;
	}

	public bool IsInEagleState()
	{
		return m_EagleState == EagleState.Carrying || m_EagleState == EagleState.FlyAway || m_EagleState == EagleState.FlyTo || m_EagleState == EagleState.FlyToBeforeDestroyObstacles;
	}

	public bool IsInEagleStateDisallowingObstacles()
	{
		return m_EagleState == EagleState.Carrying || m_EagleState == EagleState.FlyTo || m_EagleState == EagleState.FlyToBeforeDestroyObstacles;
	}

	private bool IsFlyingAllowingLaneChangeAnims()
	{
		return IsCopterBoostAllowingLaneChangeAnims() && IsEagleAllowingLaneChangeAnims();
	}

	private bool IsEagleAllowingLaneChangeAnims()
	{
		return m_EagleState != EagleState.Carrying;
	}

	private void UpdateFeatherState()
	{
		if (m_EagleAnimation == null)
		{
			return;
		}
		switch (m_EagleState)
		{
		case EagleState.FlyToBeforeDestroyObstacles:
			if (MathOps.AboutEqual(m_EagleAnimation["EagleStart"].time, 2.7f, 0.05f))
			{
				m_EagleTimer.Start(PlayerData.EagleUpgradeTime);
				int num3 = CalcPlatformCountWithEagle();
				PlatformManager.The().m_EagleNoObstaclesPlatformCount = Math.Max(num3 - PlatformManager.The().m_PlatformCount + 7, 0);
				PlatformManager.The().m_EagleTokenSpawnPlatformCount = num3;
				PlatformManager.The().DestroyObstaclesAndTokensOnAllPlatforms();
				Platform.ResetObstacleSequence();
				float y = CalcBasePosition().y;
				m_PeakEagleHeight = m_PeakEagleYOffset + y;
				m_EagleState = EagleState.FlyTo;
			}
			break;
		case EagleState.FlyTo:
			if (!m_EagleAnimation.IsPlaying("EagleStart"))
			{
				if (PerryCamera.The() != null)
				{
					PerryCamera.The().ChangeCam(PerryCamera.CameraType.BackCamEagle);
				}
				int num = Math.Min(PlatformManager.The().m_EagleTokenSpawnPlatformCount + 4, 12);
				int num2 = num - 4;
				PlatformManager.The().m_EagleTokenSpawnPlatformCount = PlatformManager.The().m_EagleTokenSpawnPlatformCount - num2;
				PlatformManager.The().InstantiateEagleTokens(4, num);
				m_EagleState = EagleState.Carrying;
				m_HasEagleHitPeak = false;
				StartAnimEagleLoop();
			}
			break;
		case EagleState.Carrying:
			if (!m_HasEagleHitPeak)
			{
				if (base.transform.position.y >= m_PeakEagleHeight)
				{
					m_Velocity.y = 0f;
					TransformOps.SetPositionY(base.transform, m_PeakEagleHeight);
					m_HasEagleHitPeak = true;
				}
				else
				{
					AddRelativeVelocityY(m_EagleRiseVelocity * 0.05f);
				}
			}
			if (m_EagleTimer.IsFinished())
			{
				GameEventManager.TriggerTempInvincibilityOn();
				m_EagleState = EagleState.FlyAway;
				if (m_EagleAnimation != null)
				{
					m_EagleAnimation.Play("EagleEnd");
				}
				StartAnimRun();
			}
			break;
		case EagleState.FlyAway:
			if (!m_EagleAnimation.IsPlaying("EagleEnd"))
			{
				GameEventManager.TriggerPowerUpFeatherOff();
			}
			break;
		}
	}

	private int CalcPlatformCountWithEagle()
	{
		float num = m_VelocityMax * m_EagleTimer.RetrieveTimeRemaining();
		int num2 = (int)(num / PlatformManager.The().m_PlatformDepth);
		if (num2 < 0)
		{
			num2 = 0;
		}
		return num2;
	}

	private void PowerUpFeatherOnListener()
	{
		if (m_Eagle != null)
		{
			PlayerData.RoundEaglePowerUpCount++;
			PlayerData.AllTimeEaglePowerUpCount++;
			Platform.SetAllowBreaks(false);
			Platform.SetAllowSequences(false);
			Platform.SetAllowRamps(false);
			Platform.SetAllowTurns(false);
			m_EagleState = EagleState.FlyToBeforeDestroyObstacles;
			m_Eagle.SetActive(true);
			m_EagleRiseVelocity = m_VelocityMax / m_VelocityMaxInitial * m_EagleRiseVelocityInitial;
			if (m_EagleAnimation == null)
			{
				m_EagleAnimation = m_Eagle.GetComponent<Animation>();
			}
			m_EagleAnimation.Play("EagleStart");
			m_EagleAnimation.PlayQueued("EagleLoop");
			GameManager.The.PlayClip(AudioClipFiles.POWERUPFOLDER + AudioClipFiles.EAGLE);
			GameManager.The.PlayLoopClip(AudioClipFiles.POWERUPFOLDER + AudioClipFiles.EAGLELOOP);
		}
	}

	private void PowerUpFeatherOffListener()
	{
		if (m_EagleState != EagleState.None)
		{
			m_EagleState = EagleState.None;
			Platform.SetAllowBreaks(true);
			Platform.SetAllowRamps(true);
			Platform.SetAllowSequences(true);
			Platform.SetAllowTurns(true);
			if (m_Eagle != null)
			{
				m_Eagle.SetActive(false);
			}
			if (PerryCamera.The() != null)
			{
				PerryCamera.The().ChangeCam(PerryCamera.CameraType.BackCam);
			}
			GameManager.The.StopClipLoop(AudioClipFiles.POWERUPFOLDER + AudioClipFiles.EAGLELOOP);
		}
	}

	public void TriggerPowerUpFeatherCutOff()
	{
		if (m_EagleState != EagleState.None)
		{
			GameEventManager.TriggerTempInvincibilityOn();
			m_EagleState = EagleState.FlyAway;
			if (m_EagleAnimation != null)
			{
				m_EagleAnimation.Play("EagleEnd");
			}
			StartAnimRun();
			if (PerryCamera.The() != null)
			{
				PerryCamera.The().ChangeCam(PerryCamera.CameraType.BackCam);
			}
			Platform.SetAllowBreaks(true);
			Platform.SetAllowRamps(true);
			Platform.SetAllowSequences(true);
			Platform.SetAllowTurns(true);
		}
	}

	public void ResetEagleState()
	{
		if (m_EagleState != EagleState.None)
		{
			m_EagleState = EagleState.None;
			if (PerryCamera.The() != null)
			{
				PerryCamera.The().ChangeCam(PerryCamera.CameraType.BackCam);
				PerryCamera.The().ResetCameraPosition();
			}
			Platform.SetAllowBreaks(true);
			Platform.SetAllowRamps(true);
			Platform.SetAllowSequences(true);
			Platform.SetAllowTurns(true);
		}
	}

	public void ResetCopterState()
	{
		Platform.SetAllowBreaks(true);
		m_CopterBoostState = CopterBoostState.None;
	}

	public bool IsInCopterBoostState()
	{
		return m_CopterBoostState == CopterBoostState.RiseUp || m_CopterBoostState == CopterBoostState.Flying || m_CopterBoostState == CopterBoostState.Dropped;
	}

	private bool IsCopterBoostAllowingLaneChangeAnims()
	{
		return m_CopterBoostState == CopterBoostState.None;
	}

	private void UpdateCopterBoostState()
	{
		switch (m_CopterBoostState)
		{
		case CopterBoostState.RiseUp:
		{
			float y = CalcBasePosition().y;
			if (base.transform.position.y >= m_PeakCopterBoostYOffset + y)
			{
				m_Velocity.y = 0f;
				m_CopterBoostState = CopterBoostState.Flying;
			}
			else
			{
				AddRelativeVelocityY(m_CopterBoostRiseVelocity * 0.05f);
			}
			break;
		}
		case CopterBoostState.Flying:
			if (m_CopterTimer.IsFinished())
			{
				m_CopterBoostState = CopterBoostState.Dropped;
			}
			break;
		case CopterBoostState.Dropped:
			GameEventManager.TriggerCopterBoostOff();
			GameEventManager.TriggerTempInvincibilityOn();
			break;
		}
	}

	private void CopterBoostOnListener()
	{
		if (m_PerryHelicopter != null)
		{
			Platform.SetAllowBreaks(false);
			m_CopterBoostState = CopterBoostState.RiseUp;
			m_CopterTimer.Start(m_CopterBoostStateDuration);
			PerryCamera.The().ChangeCam(PerryCamera.CameraType.BackCamEagle);
			GameManager.The.PlayLoopClip(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.COPTERLOOP);
			CrossfadeAnimHelicopter(0.2f);
		}
	}

	private void CopterBoostOffListener()
	{
		Platform.SetAllowBreaks(true);
		StopAnimHelicopter();
		StartAnimRun();
		PerryCamera.The().ChangeCam(PerryCamera.CameraType.BackCam);
		GameManager.The.StopClipLoop(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.COPTERLOOP);
		m_CopterBoostState = CopterBoostState.None;
	}

	private void PowerUpMagnetOnListener()
	{
		GameManager.The.PlayLoopClip(AudioClipFiles.POWERUPFOLDER + AudioClipFiles.MAGNET);
		if (m_MagnetEffects != null)
		{
			m_MagnetEffects.SetActive(true);
		}
	}

	private void PowerUpMagnetOffListener()
	{
		GameManager.The.StopClipLoop(AudioClipFiles.POWERUPFOLDER + AudioClipFiles.MAGNET);
		if (m_MagnetEffects != null)
		{
			m_MagnetEffects.SetActive(false);
		}
		Token.EndMagneticTokenState();
	}

	private void LaunchPerryListener()
	{
		HangGlideStates hangGlideState = m_HangGlideState;
		if (hangGlideState == HangGlideStates.None)
		{
			m_HangGlideLaunchHeight = CalcBasePosition().y;
			float num = 75f;
			m_HangGlideHeight = m_HangGlideLaunchHeight + num;
			m_HangGlideState = HangGlideStates.StartedJumpBeforeLaunch;
			if (!IsInJumpState())
			{
				Jump();
			}
			GameManager.The.PlayClip(AudioClipFiles.HANGGLIDERFOLDER + AudioClipFiles.GLIDERLIFT);
			GameManager.The.PlayLoopClip(AudioClipFiles.HANGGLIDERFOLDER + AudioClipFiles.GLIDERAMBIENT);
		}
	}

	private void HangGlideEndListener()
	{
		if (m_HangGlideState != HangGlideStates.None && m_HangGlideState != HangGlideStates.SlerpCameraBackToNormal && m_HangGlideState != HangGlideStates.LerpCameraBackToNormal)
		{
			EndAnimHGDive();
			GameManager.The.StopClipLoop(AudioClipFiles.HANGGLIDERFOLDER + AudioClipFiles.GLIDERAMBIENT);
			m_HangGlideState = HangGlideStates.SlerpCameraBackToNormal;
		}
	}

	private bool IsInHangGlideCameraState()
	{
		return m_HangGlideState == HangGlideStates.CurrentlyHangGliding || m_HangGlideState == HangGlideStates.SlerpCameraBackToNormal || m_HangGlideState == HangGlideStates.LerpCameraBackToNormal;
	}

	private bool IsInHangGlideState()
	{
		return m_HangGlideState != HangGlideStates.None;
	}

	private bool IsInHangGlidePrepareState()
	{
		if (m_HangGlideState <= HangGlideStates.CruisingToHangGlideArea)
		{
			return true;
		}
		return false;
	}

	private bool IsInHangGlideSlerpAndLerpOutState()
	{
		if (m_HangGlideState == HangGlideStates.DivingDown || m_HangGlideState == HangGlideStates.SlerpCameraBackToNormal || m_HangGlideState == HangGlideStates.LerpCameraBackToNormal)
		{
			return true;
		}
		return false;
	}

	private void UpdateHangGlidingState()
	{
		switch (m_HangGlideState)
		{
		case HangGlideStates.StartedJumpBeforeLaunch:
			if (IsInJumpState())
			{
				m_HangGlideState = HangGlideStates.DoingJumpBeforeLaunch;
			}
			break;
		case HangGlideStates.DoingJumpBeforeLaunch:
			if (IsTouchingGround())
			{
				m_HangGlideState = HangGlideStates.FinishedJumpTimeToLaunch;
			}
			break;
		case HangGlideStates.FinishedJumpTimeToLaunch:
			CalcHGJumpParameters();
			CalcHGDipParameters();
			m_HangGlideState = HangGlideStates.BeenLaunchedStart;
			if (PerryCamera.The() != null)
			{
				PerryCamera.The().ChangeCam(PerryCamera.CameraType.BackCamHangGlide);
			}
			AddRelativeVelocityY(1000f);
			break;
		case HangGlideStates.BeenLaunchedStart:
			if (IsFinishedHGAnimRoofToHGEnd())
			{
				StartHGAnimCruising();
				m_HangGlideState = HangGlideStates.BeenLaunchedLoop;
			}
			break;
		case HangGlideStates.BeenLaunchedLoop:
			if (base.transform.position.y > m_HangGlideHeight)
			{
				m_HangGlideState = HangGlideStates.FinishedLaunch;
			}
			break;
		case HangGlideStates.FinishedLaunch:
			TransformOps.SetPositionY(base.transform, m_HangGlideHeight);
			m_Velocity.y = 0f;
			m_HangGlideState = HangGlideStates.CruisingToHangGlideArea;
			break;
		case HangGlideStates.CruisingToHangGlideArea:
		{
			Transform transform = m_WaypointCur.RetrieveTransform(m_LaneDesired);
			if (MathOps.AboutEqual(MathOps.IntersectPointLineLine(transform.position, transform.forward, base.transform.position, Vector3.down).y, base.transform.position.y, 1f))
			{
				m_HangGlideState = HangGlideStates.CurrentlyHangGliding;
			}
			break;
		}
		case HangGlideStates.LerpCameraBackToNormal:
			if (!IsInJumpState() && !m_HangGliderAnimations.IsPlaying("End"))
			{
				if (PerryCamera.The() != null)
				{
					PerryCamera.The().ChangeCam(PerryCamera.CameraType.BackCam);
				}
				m_HangGlider.SetActive(false);
				m_HangGlideState = HangGlideStates.None;
				CalcJumpParameters();
			}
			break;
		case HangGlideStates.CurrentlyHangGliding:
		case HangGlideStates.DivingDown:
		case HangGlideStates.SlerpCameraBackToNormal:
			break;
		}
	}

	public bool HasRightWeapon(DoofenCruiser.WeaponType weaponType)
	{
		if (weaponType == DoofenCruiser.WeaponType.Fire && PlayerData.hasWaterWeapon)
		{
			return true;
		}
		if (weaponType == DoofenCruiser.WeaponType.Ice && PlayerData.hasFireWeapon)
		{
			return true;
		}
		if (weaponType == DoofenCruiser.WeaponType.Water && PlayerData.hasElectricWeapon)
		{
			return true;
		}
		if (weaponType == DoofenCruiser.WeaponType.Pin && PlayerData.HasPinWeapon)
		{
			return true;
		}
		return false;
	}

	private void ResetWeapon()
	{
		m_canAttack = false;
	}

	private bool IsTouchingGround()
	{
		return m_IsTouchingGround;
	}

	public bool IsInJumpState()
	{
		return m_IsJumping;
	}

	public bool IsInDipState()
	{
		return m_IsDipping;
	}

	public bool IsInSlideState()
	{
		return m_IsSliding;
	}

	private bool IsInDeathState()
	{
		return m_IsDying;
	}

	private bool IsInDeathFallingState()
	{
		return m_IsInDeathFallingState;
	}

	private bool IsInDeathRisingState()
	{
		return !m_IsInDeathFallingState;
	}

	private void TriggerDeathFallingState()
	{
	}

	private void SetAnimColliderHeight(float colHeight)
	{
	}

	private float GetAnimColliderHeight()
	{
		if (IsInSlideState())
		{
			return 3.5f;
		}
		return 8f;
	}

	private float GetAnimColliderY()
	{
		return 3f;
	}

	private void StartAnimWeaponDraw()
	{
		m_anim.Rewind("WeaponDraw");
		m_anim.Play("WeaponDraw");
		m_anim.Rewind("WeaponDrawnLoop");
		AnimationState animationState = m_anim.PlayQueued("WeaponDrawnLoop");
		animationState.speed = m_animationSpeed;
	}

	private bool IsInAnimAimLoop()
	{
		return m_anim.IsPlaying("WeaponAimLoop");
	}

	private bool IsInAnimDrawnLoop()
	{
		return m_anim.IsPlaying("WeaponDrawnLoop");
	}

	private void StartAnimWeaponDrawnToAim()
	{
		m_anim.Rewind("WeaponDrawnToAim");
		m_anim.Play("WeaponDrawnToAim");
		m_anim.Rewind("WeaponRecoil");
		AnimationState animationState = m_anim.PlayQueued("WeaponRecoil");
		animationState.speed = m_animationSpeed;
		m_anim.Rewind("WeaponAimToDrawn");
		animationState = m_anim.PlayQueued("WeaponAimToDrawn");
		animationState.speed = m_animationSpeed;
		m_anim.Rewind("WeaponDrawnLoop");
		m_anim.PlayQueued("WeaponDrawnLoop");
		animationState.speed = m_animationSpeed;
	}

	private void StartAnimWeaponAimToDrawn()
	{
		m_anim.Rewind("WeaponAimToDrawn");
		m_anim.Play("WeaponAimToDrawn");
		m_anim.Rewind("WeaponDrawnLoop");
		AnimationState animationState = m_anim.PlayQueued("WeaponDrawnLoop");
		animationState.speed = m_animationSpeed;
	}

	private void StartAnimWeaponAimToHolstered()
	{
		m_anim.Rewind("WeaponAimToHolstered");
		m_anim.Play("WeaponAimToHolstered");
		m_anim.Rewind("Run");
		AnimationState animationState = m_anim.PlayQueued("Run");
		animationState.speed = m_animationSpeed;
	}

	private void StartAnimWeaponDrawnToHolstered()
	{
		m_anim.Rewind("WeaponDrawnToHolstered");
		m_anim.Play("WeaponDrawnToHolstered");
		m_anim.Rewind("Run");
		AnimationState animationState = m_anim.PlayQueued("Run");
		animationState.speed = m_animationSpeed;
	}

	private bool IsFinishedAnimBlankToHolstered()
	{
		if (!m_anim.IsPlaying("WeaponDrawnToHolstered") && !m_anim.IsPlaying("WeaponAimToHolstered"))
		{
			return true;
		}
		return false;
	}

	private void StartAnimRecoil()
	{
		m_anim.Rewind("WeaponRecoil");
		m_anim.Play("WeaponRecoil");
		m_anim.Rewind("WeaponAimLoop");
		AnimationState animationState = m_anim.PlayQueued("WeaponAimLoop");
		animationState.speed = m_animationSpeed;
	}

	private void StopAnimRecoil()
	{
		m_anim.Stop("WeaponRecoil");
	}

	private void StartAnimWeaponAim()
	{
		m_anim.Rewind("WeaponAimLoop");
		m_anim.Play("WeaponAimLoop");
	}

	private void StartAnimWeaponAimQueued()
	{
		m_anim.Rewind("WeaponAimLoop");
		AnimationState animationState = m_anim.PlayQueued("WeaponAimLoop");
		animationState.speed = m_animationSpeed;
	}

	private void StartAnimWeaponDrawn()
	{
		m_anim.Rewind("WeaponDrawnLoop");
		m_anim.Play("WeaponDrawnLoop");
	}

	private void StartAnimWeaponDrawnQueued()
	{
		m_anim.Rewind("WeaponDrawnLoop");
		AnimationState animationState = m_anim.PlayQueued("WeaponDrawnLoop");
		animationState.speed = m_animationSpeed;
	}

	private void StopAnimWeaponAim()
	{
		m_anim.Stop("WeaponAimLoop");
	}

	private void StartAnimHGLoop()
	{
		m_anim.Rewind("HGLoop");
		m_anim.Play("HGLoop");
	}

	private void CrossFadeAnimHGLoopQueued(float duration)
	{
		m_anim.Rewind("HGLoop");
		AnimationState animationState = m_anim.CrossFadeQueued("HGLoop", duration);
		animationState.speed = m_animationSpeed;
	}

	private void StartAnimHGLoopQueued()
	{
		m_anim.Rewind("HGLoop");
		AnimationState animationState = m_anim.PlayQueued("HGLoop");
		animationState.speed = m_animationSpeed;
	}

	private void StartAnimHGDive()
	{
		m_anim.Rewind("DiveStart");
		m_anim.Play("DiveStart");
		m_anim.Rewind("DiveLoop");
		AnimationState animationState = m_anim.PlayQueued("DiveLoop");
		animationState.speed = m_animationSpeed;
	}

	private void CrossFadeAnimHGDive(float duration)
	{
		m_anim.Rewind("DiveEnd");
		m_anim.CrossFade("DiveEnd", duration);
	}

	private void EndAnimHGDive()
	{
		m_anim.Rewind("DiveEnd");
		m_anim.Play("DiveEnd");
	}

	private void StartAnimHGBankLeft()
	{
		if (m_HangGlideState != HangGlideStates.DivingDown)
		{
			m_anim.Rewind("HGBankL");
			if (!IsInJumpState() && !IsInDipState())
			{
				m_anim.Play("HGBankL");
			}
			else
			{
				m_anim.CrossFade("HGBankL", 0.25f);
			}
		}
	}

	private void StartAnimHGBankRight()
	{
		if (m_HangGlideState != HangGlideStates.DivingDown)
		{
			m_anim.Rewind("HGBankR");
			if (!IsInJumpState() && !IsInDipState())
			{
				m_anim.Play("HGBankR");
			}
			else
			{
				m_anim.CrossFade("HGBankR", 0.25f);
			}
		}
	}

	private void StartAnimSliding()
	{
		m_anim.Rewind("SlideStart");
		m_anim.Play("SlideStart");
		m_anim.Rewind("SlideLoop");
		AnimationState animationState = m_anim.PlayQueued("SlideLoop");
		animationState.speed = m_animationSpeed;
	}

	private void EndAnimSliding()
	{
		if (!IsInJumpState() && !IsInDeathState() && !IsInEagleState() && !IsInCopterBoostState())
		{
			if (m_canAttack)
			{
				StartAnimWeaponDrawn();
				return;
			}
			m_anim.Rewind("Run");
			m_anim.CrossFade("Run", 0.2f);
		}
	}

	private void StartAnimLeftHop()
	{
		if (!IsInSlideState())
		{
			m_anim.Rewind("SideHopL");
			m_anim.Play("SideHopL");
		}
	}

	private void StartAnimRightHop()
	{
		if (!IsInSlideState())
		{
			m_anim.Rewind("SideHopR");
			m_anim.Play("SideHopR");
		}
	}

	private void StartAnimLeftBounce()
	{
		if (!IsInSlideState())
		{
			m_anim.Rewind("SideBounceL");
			m_anim.Play("SideBounceL");
			AnimationState animationState;
			if (m_canAttack)
			{
				m_anim.Rewind("WeaponDrawnLoop");
				animationState = m_anim.PlayQueued("WeaponDrawnLoop");
			}
			else
			{
				m_anim.Rewind("Run");
				animationState = m_anim.PlayQueued("Run");
			}
			animationState.speed = m_animationSpeed;
		}
	}

	private void StartAnimRightBounce()
	{
		if (!IsInSlideState())
		{
			m_anim.Rewind("SideBounceR");
			m_anim.Play("SideBounceR");
			AnimationState animationState;
			if (m_canAttack)
			{
				m_anim.Rewind("WeaponDrawnLoop");
				animationState = m_anim.PlayQueued("WeaponDrawnLoop");
			}
			else
			{
				m_anim.Rewind("Run");
				animationState = m_anim.PlayQueued("Run");
			}
			animationState.speed = m_animationSpeed;
		}
	}

	private void StartAnimFallToLand()
	{
		m_anim.Rewind("JumpUpToDown1");
		m_anim.Play("JumpUpToDown1");
		m_anim.Rewind("JumpDownLoop1");
		AnimationState animationState = m_anim.PlayQueued("JumpDownLoop1");
		animationState.speed = m_animationSpeed;
	}

	private void StartAnimJump()
	{
		if (m_HangGlideState == HangGlideStates.StartedJumpBeforeLaunch)
		{
			StartHGAnimRoofToHGStart();
			StartHGAnimRoofToHGLoop();
			return;
		}
		if (IsInHangGlideState())
		{
			StartHGAnimLiftStart();
			StartHGAnimLiftLoop();
			return;
		}
		m_anim.Rewind("JumpStart1");
		m_anim.Play("JumpStart1");
		m_anim.Rewind("JumpUpLoop1");
		AnimationState animationState = m_anim.PlayQueued("JumpUpLoop1", QueueMode.CompleteOthers);
		animationState.speed = m_animationSpeed;
	}

	private void StartAnimJumpUpToDown()
	{
		if (!IsInEagleStateCarrying() && !IsInDeathState() && !IsInHangGlideState())
		{
			m_anim.Rewind("JumpUpToDown1");
			m_anim.Play("JumpUpToDown1");
			m_anim.Rewind("JumpDownLoop1");
			AnimationState animationState = m_anim.PlayQueued("JumpDownLoop1", QueueMode.CompleteOthers);
			animationState.speed = m_animationSpeed;
		}
	}

	private void EndAnimJump()
	{
		if (!IsInEagleStateCarrying() && !IsInDeathState())
		{
			if (m_HangGlideState == HangGlideStates.DoingJumpBeforeLaunch)
			{
				StartHGAnimRoofToHGEnd();
			}
			else if (IsInHangGlideState())
			{
				CrossFadeHGAnimLiftEnd(0.05f);
				StartAnimHGLoopQueued();
			}
			else if (m_canAttack)
			{
				m_anim.Rewind("JumpEnd1");
				m_anim.Play("JumpEnd1");
				StartAnimWeaponDrawnQueued();
			}
			else
			{
				m_anim.Rewind("JumpEnd1");
				m_anim.Play("JumpEnd1");
				m_anim.Rewind("Run");
				AnimationState animationState = m_anim.PlayQueued("Run");
				animationState.speed = m_animationSpeed;
			}
		}
	}

	private bool IsAnimFallPlaying()
	{
		return m_anim.IsPlaying("Fall");
	}

	private void StartAnimFalling()
	{
		m_anim.Play("Fall");
	}

	private bool IsAnimHelicopterPlaying()
	{
		return m_anim.IsPlaying("HelicopterLoop");
	}

	private void StartAnimHelicopter()
	{
		m_PerryHelicopter.SetActive(true);
		m_anim.Play("HelicopterLoop");
	}

	private void CrossfadeAnimHelicopter(float duration)
	{
		m_PerryHelicopter.SetActive(true);
		m_anim.CrossFade("HelicopterLoop", duration);
	}

	private void StopAnimHelicopter()
	{
		m_PerryHelicopter.SetActive(false);
		m_anim.Stop("HelicopterLoop");
	}

	private void StartAnimHitObstacle()
	{
		m_anim.Play("HitObstacle");
	}

	private void UpdateFalling()
	{
		if (!m_IsFalling && !IsInJumpState() && !IsInHangGlideState() && !IsInSlideState() && m_EagleState != EagleState.Carrying && m_CopterBoostState != CopterBoostState.Flying && m_CopterBoostState != CopterBoostState.RiseUp)
		{
			if (base.transform.position.y < m_PrevY - 0.1f)
			{
				m_IsFalling = true;
				m_IsInAir = true;
			}
			m_PrevY = base.transform.position.y;
			if (m_IsFalling && !m_anim.IsPlaying("JumpDownLoop1") && !IsInHangGlideState())
			{
				m_anim.Rewind("JumpDownLoop1");
				m_anim.Play("JumpDownLoop1");
			}
		}
	}

	private void StartAnimEagleLoop()
	{
		m_anim.Play("EagleRideLoop");
	}

	private void CrossFadeAnimRun(float duration)
	{
		m_anim.Rewind("Run");
		m_anim.CrossFade("Run", duration);
	}

	private void StartAnimRun()
	{
		m_anim.Rewind("Run");
		m_anim.Play("Run");
	}

	private void PlayHGEndAnimSequence()
	{
		m_anim.Rewind("HGToRoofStart");
		m_anim.Play("HGToRoofStart");
		m_anim.Rewind("Run");
		AnimationState animationState = m_anim.PlayQueued("Run");
		animationState.speed = m_animationSpeed;
	}

	private void StartAnimFromInAirToRun()
	{
		m_anim.Rewind("JumpEndToRun");
		m_anim.Play("JumpEndToRun");
		m_anim.Rewind("Run");
		AnimationState animationState = m_anim.PlayQueued("Run");
		animationState.speed = m_animationSpeed;
	}

	public bool IsTubeIntroStartedState()
	{
		return m_IsTubeIntroStarted;
	}

	public bool IsInTubeIntroState()
	{
		return m_IsInTubeIntroState;
	}

	public bool IsTubeIntroFinshed()
	{
		return m_IsTubeIntroFinished;
	}

	private void StartTubeIntro()
	{
		m_tube.SetActive(true);
		m_anim.Stop();
		m_anim.Rewind("RunnerTubeLoop");
		m_anim.Play("RunnerTubeLoop");
		m_tubeAnim.Rewind("TubeLoopA");
		m_tubeAnim.Play("TubeLoopA");
		m_IsLoopingTubeLoopA = true;
		m_IsTubeIntroStarted = true;
		m_IsInTubeIntroEndState = false;
		m_ShouldQueueEnd = false;
		m_IsTubeIntroFinished = false;
		m_FrameSkipCount = 1;
	}

	private void UpdateTubeIntro()
	{
		if (!m_IsInTubeIntroState && m_IsTubeIntroStarted && m_anim.IsPlaying("RunnerTubeLoop") && m_FrameSkipCount-- <= 0)
		{
			m_IsInTubeIntroState = true;
			m_IsTubeIntroStarted = false;
		}
		if (!m_IsInTubeIntroState)
		{
			return;
		}
		if (m_ShouldQueueEnd && !m_IsLoopingTubeLoopA && !m_tubeAnim.IsPlaying("TubeLoopB"))
		{
			m_ShouldQueueEnd = false;
			m_IsInTubeIntroEndState = true;
			m_anim.Rewind("RunnerTubeEnd");
			m_anim.Play("RunnerTubeEnd");
			m_tubeAnim.Rewind("TubeEnd");
			m_tubeAnim.Play("TubeEnd");
			m_tubeAnim.Stop("TubeLoopA");
			m_tubeAnim.Stop("TubeLoopB");
		}
		if (m_IsInTubeIntroEndState && !m_anim.IsPlaying("RunnerTubeEnd"))
		{
			m_IsInTubeIntroState = false;
			m_IsTubeIntroFinished = true;
			m_tube.SetActive(false);
			GameEventManager.TriggerGameIntroEnd();
		}
		if (!m_IsInTubeIntroEndState)
		{
			if (m_IsLoopingTubeLoopA && !m_tubeAnim.IsPlaying("TubeLoopA"))
			{
				m_IsLoopingTubeLoopA = false;
				m_tubeAnim.Rewind("TubeLoopB");
				m_tubeAnim.Play("TubeLoopB");
			}
			if (!m_IsLoopingTubeLoopA && !m_tubeAnim.IsPlaying("TubeLoopB"))
			{
				m_IsLoopingTubeLoopA = true;
				m_tubeAnim.Rewind("TubeLoopA");
				m_tubeAnim.Play("TubeLoopA");
			}
		}
	}

	public void ForceStopTubeIntro()
	{
		m_anim.Stop("RunnerTubeLoop");
		m_anim.Stop("RunnerTubeEnd");
		m_tubeAnim.Stop("TubeLoop");
		m_tubeAnim.Stop("TubeEnd");
		m_tubeAnim.Stop("TubeLoopA");
		m_tubeAnim.Stop("TubeLoopB");
		m_IsTubeIntroStarted = false;
		m_IsInTubeIntroState = false;
	}

	public void StopTubeIntro()
	{
		if (!m_IsInTubeIntroEndState)
		{
			m_ShouldQueueEnd = true;
		}
	}

	private void OnGUI()
	{
		if (Event.current.type == EventType.MouseUp)
		{
			if (m_touchDown)
			{
				if (!m_swiping)
				{
					m_fireOnUpdate = true;
				}
				m_touchDown = false;
			}
		}
		else if (Event.current.type == EventType.MouseDown)
		{
			m_touchDown = true;
			m_swiping = false;
			m_inputX = 0f;
			m_inputY = 0f;
			lastMousePos = Event.current.mousePosition;
		}
		else
		{
			if (Event.current.type != EventType.MouseDrag || m_swiping)
			{
				return;
			}
			Vector2 vector = Event.current.mousePosition - lastMousePos;
			vector.y *= -1f;
			lastMousePos = Event.current.mousePosition;
			m_inputX += vector.x * m_swipeSpeedX;
			m_inputY += vector.y * m_swipeSpeedY;
			float num = Mathf.Abs(m_inputY);
			float num2 = Mathf.Abs(m_inputX);
			if (m_inputY > m_SwipeDeadZoneY && num > num2)
			{
				m_jumpOnUpdate = true;
				m_swiping = true;
			}
			else if (m_inputY < 0f - m_SwipeDeadZoneY && num > num2)
			{
				m_slideOnUpdate = true;
				m_swiping = true;
			}
			else if (m_inputX > m_SwipeDeadZoneX)
			{
				if (m_reverseControls)
				{
					m_changeLaneLeftOnUpdate = true;
				}
				else
				{
					m_changeLaneRightOnUpdate = true;
				}
				m_swiping = true;
			}
			else if (m_inputX < 0f - m_SwipeDeadZoneX)
			{
				if (m_reverseControls)
				{
					m_changeLaneRightOnUpdate = true;
				}
				else
				{
					m_changeLaneLeftOnUpdate = true;
				}
				m_swiping = true;
			}
			m_touchCount++;
		}
	}
}

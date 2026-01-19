using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
	public enum GameplayState
	{
		Loading_GameStart = 0,
		GameStart_Menu = 1,
		GameStart_Menu_CharacterSelect = 2,
		GameStart_Menu_Missions = 3,
		GamePlay_Intro = 4,
		GamePlay_Action = 5,
		GamePlay_Pause = 6,
		GameOver_ContinueMenu = 7,
		GameContinue = 8,
		GameRestart_Menu = 9,
		GameRestart_Menu_Missions_Intro = 10,
		GameRestart_Menu_Leaderboards = 11,
		GameRestart_Menu_Missions = 12,
		Menu_GetTokens = 13,
		Menu_Gadgets = 14,
		Menu_Upgrades = 15,
		Menu_Popup_Settings = 16,
		Menu_Popup_BuyTokens = 17,
		Menu_External_GameCenter = 18,
		Menu_External_MoreGames = 19,
		Menu_External_Gloat = 20
	}

	public SoundReferencer sounds;

	private static GameplayState m_lastGameplayState;

	private static GameplayState m_currentGameplayState;

	public Material[] bannerMats;

	public Texture2D[] bannerImages;

	public Texture2D[] bannerTextFree;

	private StopWatch m_InGameStopWatch;

	private bool m_hasInitPersistenData;

	private Vector2 g_aspectratio = Vector2.zero;

	public float m_scoreMultiplierTime = 10f;

	private bool m_isScoreMultiplierOn;

	public float m_invincibilityTime = 10f;

	private bool m_isInvincibilityOn;

	private bool m_tempContinueInvincibility;

	private float m_tempContInvincibilityTime = 4f;

	private float m_currentScoreMultiplierTime;

	private float m_currentInvincibilityTime;

	private static float m_powerupCooldown = 2f;

	private bool m_inPowerupCooldown;

	public float m_TokenFrequency;

	public int m_TokenGroupCountPerSpawn;

	private int m_TokenSpawnCountCur = 6;

	[HideInInspector]
	public float m_TokenTimeLastSpawned = 5f;

	public float m_TokenDistToNextState = 10f;

	private float m_PlatformDifficultyStateDistStart;

	public float m_PlatformDifficultyDistToNextState = 10f;

	public bool g_MusicEnabled = true;

	public bool g_SoundEnabled = true;

	private SoundHandler g_MusicSource;

	private string g_CurrentMusicTrack = string.Empty;

	private List<GameObject> m_UsedChannels = new List<GameObject>();

	private List<GameObject> m_SoundLoopChannels = new List<GameObject>();

	private static bool m_isPaused;

	private bool m_IsLowEndDevice;

	private static GameManager m_the;

	public static GameplayState currentGameplayState
	{
		get
		{
			return m_currentGameplayState;
		}
	}

	public static GameManager The
	{
		get
		{
			if (m_the == null)
			{
				m_the = GameObject.Find("GameManager").GetComponent<GameManager>();
			}
			return m_the;
		}
	}

	public Vector2 aspectRatio
	{
		get
		{
			if (g_aspectratio == Vector2.zero)
			{
				ObtainAspectRatio();
			}
			return g_aspectratio;
		}
	}

	public bool hasPromptedPlayerforBirthday
	{
		get
		{
			return PlayerPrefs.GetInt("_HAS_PROMPTED_FOR_BIRTHDAY_") > 0;
		}
		set
		{
			if (value)
			{
				PlayerPrefs.SetInt("_HAS_PROMPTED_FOR_BIRTHDAY_", 1);
			}
			else
			{
				PlayerPrefs.SetInt("_HAS_PROMPTED_FOR_BIRTHDAY_", 0);
			}
		}
	}

	public bool playerIsUnderThirteen
	{
		get
		{
			return PlayerPrefs.GetInt("_PLAYER_UNDER_13_") > 0;
		}
		set
		{
			if (value)
			{
				PlayerPrefs.SetInt("_PLAYER_UNDER_13_", 1);
			}
			else
			{
				PlayerPrefs.SetInt("_PLAYER_UNDER_13_", 0);
			}
		}
	}

	public bool m_SoundEnabled
	{
		get
		{
			return g_SoundEnabled;
		}
		set
		{
			g_SoundEnabled = value;
			PerryiCloudManager.The.SetItem("_SoundEnabledkey", value);
		}
	}

	public bool m_MusicEnabled
	{
		get
		{
			return g_MusicEnabled;
		}
		set
		{
			PerryiCloudManager.The.SetItem("_MusicEnabledkey", value);
			g_MusicEnabled = value;
			if (g_MusicSource == null)
			{
				if (g_MusicEnabled && g_CurrentMusicTrack.Length > 1)
				{
					PlayMusic(g_CurrentMusicTrack);
				}
			}
			else if (g_MusicEnabled)
			{
				g_MusicSource.GetComponent<AudioSource>().volume = 1f;
			}
			else
			{
				g_MusicSource.GetComponent<AudioSource>().volume = 0f;
			}
		}
	}

	public bool IsInGamePlay()
	{
		if (m_currentGameplayState == GameplayState.GamePlay_Action)
		{
			return true;
		}
		if (m_currentGameplayState == GameplayState.GamePlay_Pause)
		{
			return true;
		}
		return false;
	}

	public void DoLanguageBillboards()
	{
		for (int i = 0; i < bannerMats.Length; i++)
		{
			if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
			{
				bannerMats[i].mainTexture = bannerTextFree[i];
			}
			else
			{
				bannerMats[i].mainTexture = bannerImages[i];
			}
		}
	}

	private void OnDestroy()
	{
		GameEventManager.GameLoading -= GameLoading;
		GameEventManager.GameMainMenu -= StartMainMenu;
		GameEventManager.GameIntro -= GameIntro;
		GameEventManager.GameStart -= GameStart;
		GameEventManager.GameOver -= GameOver;
		GameEventManager.GameContinue -= GameContinueEvent;
		GameEventManager.GameRestartMenu -= GoToGameRestartMenu;
		GameEventManager.GamePause -= GamePause;
		GameEventManager.GameUnPause -= GameUnPause;
		GameEventManager.CollectScoreMultiplier -= CollectScoreMultiplier;
		GameEventManager.CollectInvincibility -= CollectInvincibility;
		GameEventManager.TempInvincibilityOnEvents -= TempInvincibilityOnListener;
		GameEventManager.PowerUpMagnetOn -= CollectPowerUpMagnet;
		GameEventManager.TokensLastSpawnedEvents -= TokenLastSpawnedHandler;
		GameEventManager.TokenHit -= TokenHit;
		GameEventManager.FedoraHit -= FedoraHit;
	}

	private void Awake()
	{
		if (m_the == null)
		{
			m_the = this;
			new GameObject("ReignServices").AddComponent<ReignServices>();
			if (IsiPhone4thGeneration())
			{
				Application.targetFrameRate = 30;
			}
			else
			{
				Application.targetFrameRate = 60;
			}
			PlayHavenController.ShowPlayHavenPlacements = true;
			GameEventManager.GameLoading += GameLoading;
			GameEventManager.GameMainMenu += StartMainMenu;
			GameEventManager.GameIntro += GameIntro;
			GameEventManager.GameStart += GameStart;
			GameEventManager.GameOver += GameOver;
			GameEventManager.GameContinue += GameContinueEvent;
			GameEventManager.GameRestartMenu += GoToGameRestartMenu;
			GameEventManager.GamePause += GamePause;
			GameEventManager.GameUnPause += GameUnPause;
			GameEventManager.CollectScoreMultiplier += CollectScoreMultiplier;
			GameEventManager.CollectInvincibility += CollectInvincibility;
			GameEventManager.TempInvincibilityOnEvents += TempInvincibilityOnListener;
			GameEventManager.PowerUpMagnetOn += CollectPowerUpMagnet;
			GameEventManager.TokensLastSpawnedEvents += TokenLastSpawnedHandler;
			GameEventManager.TokenHit += TokenHit;
			GameEventManager.FedoraHit += FedoraHit;
			SetPlayerSavedSoundSettings();
			DetermineDeviceType();
		}
		else
		{
			Debug.Log("GameManger is already set and is trying to set again");
		}
	}

	private void Start()
	{
		if (!(this != m_the))
		{
			Object.DontDestroyOnLoad(base.gameObject);
			m_InGameStopWatch = TimerManager.The().SpawnStopWatch();
			Screen.autorotateToLandscapeLeft = false;
			Screen.autorotateToLandscapeRight = false;
			Screen.autorotateToPortrait = true;
			Screen.autorotateToPortraitUpsideDown = true;
			Screen.orientation = ScreenOrientation.AutoRotation;
			InitPersistentData();
			PlayerData.SetNewMissions();
			StartPlayHavenPlacements();
			RenderSettings.fogColor = new Color(0.3764f, 0.7843f, 0.98039f);
			PlayerData.AllTimeAppLaunches++;
			if (PlayerData.AllTimeAppLaunches <= 1)
			{
				PlayerData.SetInitialTokenAndFedoraValues();
			}
		}
	}

	private void OnApplicationPause(bool isPaused)
	{
		if (isPaused && m_currentGameplayState == GameplayState.GamePlay_Action)
		{
			MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Pause_Menu);
			GameEventManager.TriggerGamePause();
		}
	}

	private void DetermineDeviceType()
	{
	}

	public bool IsLowEndDevice()
	{
		return m_IsLowEndDevice;
	}

	public static bool IsiPhone4S()
	{
		return false;
	}

	public static bool IsiPhone4thGeneration()
	{
		return false;
	}

	public static bool IsiPad3()
	{
		return false;
	}

	public static bool IsiPad2orMini()
	{
		return false;
	}

	public void StartPlayHavenPlacements()
	{
	}

	public void InitPersistentData()
	{
		if (!m_hasInitPersistenData)
		{
			AllItemData.Init();
			PlayerData.LoadPersistentData();
			m_hasInitPersistenData = true;
		}
	}

	private void UpdateTimeWarp()
	{
	}

	private void Update()
	{
		if (this != m_the)
		{
			return;
		}
		UpdateTimeWarp();
		if (m_isPaused || !IsInGamePlay())
		{
			return;
		}
		if (m_isScoreMultiplierOn)
		{
			m_currentScoreMultiplierTime += Time.deltaTime;
			if (m_currentScoreMultiplierTime >= PlayerData.ScoreMultUpgradeTime)
			{
				m_isScoreMultiplierOn = false;
				GameEventManager.TriggerScoreMultiplierOff();
			}
		}
		if (m_isInvincibilityOn)
		{
			m_currentInvincibilityTime += Time.deltaTime;
			float num = PlayerData.InvulnerabilityUpgradeTime;
			if (m_tempContinueInvincibility)
			{
				num = m_tempContInvincibilityTime;
			}
			if (m_currentInvincibilityTime >= num)
			{
				GameEventManager.TriggerInvincibilityOff();
				m_isInvincibilityOn = false;
				m_tempContinueInvincibility = false;
			}
			else if (!m_inPowerupCooldown && m_currentInvincibilityTime >= num - m_powerupCooldown)
			{
				GameEventManager.TriggerInvincibilityCooldown();
				m_inPowerupCooldown = true;
			}
		}
		PlatformDifficultyUpdate();
	}

	public bool IsGamePaused()
	{
		return m_isPaused;
	}

	public bool IsScoreMultiplierOn()
	{
		return m_isScoreMultiplierOn;
	}

	private void GameLoading()
	{
		Debug.Log("Game Loading!!");
		m_currentGameplayState = GameplayState.Loading_GameStart;
	}

	private void StartMainMenu()
	{
		Debug.Log("Starting the Main Menu!");
		m_currentGameplayState = GameplayState.GameStart_Menu;
	}

	private void GameIntro()
	{
		m_InGameStopWatch.Start();
		ContinueScreenGUIManager.The.HideAll();
		m_currentGameplayState = GameplayState.GamePlay_Intro;
		m_isPaused = false;
		ResetAllPlayerData();
		ResetGameManager();
		ResetAllVars();
		PlayMusic(AudioClipFiles.ROOFTOPTHEME);
		PauseMusic();
		The.PlayLoopClip(AudioClipFiles.TUBEFOLDER + AudioClipFiles.TUBESWOOSH);
	}

	private void GameStart()
	{
		m_currentGameplayState = GameplayState.GamePlay_Action;
		if (m_InGameStopWatch.IsPaused())
		{
			m_InGameStopWatch.Start();
		}
		m_isPaused = false;
		PlatformDifficultyStart();
		StopClipLoop(AudioClipFiles.TUBEFOLDER + AudioClipFiles.TUBESWOOSH);
		PlayClip(AudioClipFiles.TUBEFOLDER + AudioClipFiles.TUBESWOOSHEND);
		ResumeMusic();
		HUDGUIManager.The.UpdateTokens(0);
		PlayerData.UpdateCheckMissions();
	}

	private void GameOver()
	{
		if (m_currentGameplayState != GameplayState.GameOver_ContinueMenu)
		{
			m_InGameStopWatch.Pause();
			m_lastGameplayState = m_currentGameplayState;
			m_currentGameplayState = GameplayState.GameOver_ContinueMenu;
			PlayerData.SaveRoundPersistentData();
			MainMenuEventManager.TriggerStopRecording();
			HUDGUIManager.The.HidePauseButton();
			ContinueScreenGUIManager.The.ShowContinueScreen();
			PlayerData.SavePersistentData();
		}
	}

	private void GameContinueEvent()
	{
		Debug.Log("GameManager: GameContinueEvent");
		m_currentGameplayState = m_lastGameplayState;
		HUDGUIManager.The.ShowPauseButton();
		ContinueScreenGUIManager.The.HideAll();
		m_isScoreMultiplierOn = false;
		GameEventManager.TriggerScoreMultiplierOff();
		GameEventManager.TriggerInvincibilityOff();
		m_isInvincibilityOn = false;
		m_inPowerupCooldown = false;
		m_isPaused = false;
		m_tempContinueInvincibility = true;
		GameEventManager.TriggerInvincibility();
		m_InGameStopWatch.Unpause();
	}

	private void ResetGameManager()
	{
		m_TokenTimeLastSpawned = Time.time;
		m_TokenSpawnCountCur = 0;
		PlatformDifficultyEnd();
	}

	private void ResetAllGUI()
	{
		GameEventManager.TriggerScoreMultiplierOff();
	}

	private void ResetAllPowerups()
	{
		m_isScoreMultiplierOn = false;
		m_isInvincibilityOn = false;
		m_inPowerupCooldown = false;
		GameEventManager.TriggerInvincibilityOff();
		m_tempContinueInvincibility = false;
	}

	private void ResetAllPlayerData()
	{
		PlayerData.RoundMeters = 0;
		PlayerData.RoundTokens = 0;
		PlayerData.RoundScore = 0;
		PlayerData.RoundGadgetUses = 0;
		PlayerData.RoundWindowsBroken = 0;
		PlayerData.RoundHasFiredMaxedOutWeapon = false;
		PlayerData.RoundDidUseJumpStart = false;
		PlayerData.RoundHasConnectedToFacebook = false;
		PlayerData.RoundHasCollectedItemFromBabyHead = false;
		PlayerData.RoundBabyHeadSeenCount = 0;
		PlayerData.RoundDuckySeenCount = 0;
		PlayerData.RoundHasCollectedFedoraFromBabyHead = false;
		PlayerData.RoundJumpOverBoxesCount = 0;
		PlayerData.RoundJumpOverBotsCount = 0;
		PlayerData.RoundPlayerSlidesUnderObstacles = 0;
		PlayerData.RoundDoofenschmirtzEncounterCount = 0;
		PlayerData.RoundFedoras = 0;
		PlayerData.RoundScoreBonus = 0;
		PlayerData.RoundBossDefeats = 0;
		PlayerData.RoundBossEncounters = 0;
		PlayerData.RoundBossBonusScore = 0;
		PlayerData.RoundPlayerSlides = 0;
		PlayerData.RoundContinues = 0;
		PlayerData.RoundMaxPowerGadgetFire = 0;
		PlayerData.RoundPowerups = 0;
		PlayerData.RoundInvincibility = 0;
		PlayerData.RoundScoreMultipliers = 0;
		PlayerData.RoundMagnetizers = 0;
		PlayerData.RoundEaglePowerUpCount = 0;
		PlayerData.RoundScenesChanged = 0;
		PlayerData.RoundJumps = 0;
		PlayerData.RoundBouncesWhileInvincible = 0;
		PlayerData.ContinueCost = PlayerData.BaseContinueCost;
	}

	private void ResetAllVars()
	{
		ResetAllGUI();
		ResetAllPowerups();
		ResetGameManager();
		ResetAllPlayerData();
	}

	private void GoToGameRestartMenu()
	{
		m_currentGameplayState = GameplayState.GameRestart_Menu;
		PlayerData.SendCompletedMissionsToExternalStorage();
		PlayerData.UpdateMissionProgressAchievements();
		PlayerData.SavePersistentData();
		DuckyMomo.TriggerCutOffDucky();
		m_InGameStopWatch.Stop();
		PlayMusic(AudioClipFiles.MENUTHEME);
	}

	public static int GetGreaterCommonDivisor(int a, int b)
	{
		if (a == 0 || b == 0)
		{
			return Mathf.Abs(Mathf.Max(Mathf.Abs(a), Mathf.Abs(b)));
		}
		int num = a % b;
		return (num == 0) ? Mathf.Abs(b) : GetGreaterCommonDivisor(b, num);
	}

	private void ObtainAspectRatio()
	{
		int greaterCommonDivisor = GetGreaterCommonDivisor(Screen.height, Screen.width);
		g_aspectratio = new Vector2(Screen.width / greaterCommonDivisor, Screen.height / greaterCommonDivisor);
	}

	public float PlayClip(string clip)
	{
		if (!g_SoundEnabled)
		{
			return -1f;
		}
		int num = 16;
		if (m_UsedChannels.Count >= num)
		{
			return 0f;
		}
		SoundHandler soundHandler = CacheManager.The().Spawn<SoundHandler>("SFX");
		m_UsedChannels.Add(soundHandler.gameObject);
		return soundHandler.PlayClip(clip, false);
	}

	public float PlayRandomClip(string[] clips)
	{
		return PlayClip(clips[Random.Range(0, clips.Length)]);
	}

	public void PauseMusic()
	{
		if (g_MusicEnabled && !(g_MusicSource == null))
		{
			g_MusicSource.GetComponent<AudioSource>().Pause();
		}
	}

	public void ResumeMusic()
	{
		if (g_MusicEnabled && !(g_MusicSource == null) && !g_MusicSource.GetComponent<AudioSource>().isPlaying)
		{
			g_MusicSource.GetComponent<AudioSource>().Play();
		}
	}

	public void PauseSoundClips()
	{
		if (m_UsedChannels.Count > 0)
		{
			for (int i = 0; i < m_UsedChannels.Count; i++)
			{
				m_UsedChannels[i].GetComponent<AudioSource>().Pause();
			}
		}
		if (m_SoundLoopChannels.Count > 0)
		{
			for (int j = 0; j < m_SoundLoopChannels.Count; j++)
			{
				m_SoundLoopChannels[j].GetComponent<AudioSource>().Pause();
			}
		}
	}

	public void ResumeSoundClips()
	{
		if (m_UsedChannels.Count > 0)
		{
			for (int i = 0; i < m_UsedChannels.Count; i++)
			{
				m_UsedChannels[i].GetComponent<AudioSource>().Play();
			}
		}
		if (m_SoundLoopChannels.Count > 0)
		{
			for (int j = 0; j < m_SoundLoopChannels.Count; j++)
			{
				m_SoundLoopChannels[j].GetComponent<AudioSource>().Play();
			}
		}
	}

	public void StopClip(SoundHandler channel)
	{
		m_UsedChannels.Remove(channel.gameObject);
		CacheManager.The().Unspawn(channel);
	}

	public void StopClip(string clip)
	{
		for (int i = 0; i < m_UsedChannels.Count; i++)
		{
			if (m_UsedChannels[i] != null)
			{
				SoundHandler component = m_UsedChannels[i].GetComponent<SoundHandler>();
				if (component.clip == clip)
				{
					StopClip(component);
					break;
				}
			}
		}
	}

	public void CullSoundChannels()
	{
		if (m_UsedChannels.Count > 0)
		{
			for (int i = 0; i < m_UsedChannels.Count; i++)
			{
				StopClip(m_UsedChannels[i].GetComponent<SoundHandler>());
			}
			m_UsedChannels.Clear();
		}
		while (m_SoundLoopChannels.Count > 0)
		{
			GameObject gameObject = m_SoundLoopChannels[0];
			StopClipLoop(gameObject.name);
		}
		m_SoundLoopChannels.Clear();
	}

	public void DoObstacleSound(string obstacletag)
	{
		switch (obstacletag)
		{
		case "wooden":
			PlayClip(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.WOODCRASH);
			break;
		case "metal":
			PlayClip(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.STEELCRASH);
			break;
		case "brick":
			PlayClip(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.BRICKCRASH);
			break;
		case "obstaclewithdoof":
			PlayClip(AudioClipFiles.BOSSHIT + Random.Range(1, AudioClipFiles.NUMBERBOSSHIT));
			break;
		default:
			PlayClip(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.ORGANICCRASH);
			break;
		}
	}

	public void DoTokenPickUpSound()
	{
		The.PlayClip(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.COINCOLLECT2);
	}

	public void DoStepSound()
	{
		if (!Runner.The().IsInJumpState() && !Runner.The().IsInSlideState())
		{
			PlayClip(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.FOOTSTEP + Random.Range(1, AudioClipFiles.NUMBEROFFOOTSTEPS));
		}
	}

	public void ResetSoundTrack()
	{
		if (g_MusicSource != null)
		{
			g_MusicSource.ResetTrack();
		}
	}

	public SoundHandler PlayLoopClip(string clip)
	{
		if (!g_SoundEnabled)
		{
			return null;
		}
		GameObject gameObject = new GameObject();
		gameObject.transform.parent = base.transform;
		gameObject.name = clip.Replace('/', '_');
		gameObject.AddComponent<AudioSource>();
		SoundHandler soundHandler = gameObject.AddComponent<SoundHandler>();
		soundHandler.PlayClip(clip, true);
		m_SoundLoopChannels.Add(gameObject);
		return soundHandler;
	}

	public void StopClipLoop(string clip)
	{
		clip = clip.Replace('/', '_');
		if (base.transform.Find(clip) != null)
		{
			GameObject gameObject = base.transform.Find(clip).gameObject;
			m_SoundLoopChannels.Remove(gameObject);
			Object.Destroy(gameObject);
		}
	}

	public float PlayMusic(string track)
	{
		g_CurrentMusicTrack = track;
		if (!g_MusicEnabled)
		{
			return -1f;
		}
		string[] array = track.Split('/');
		if (g_MusicSource != null)
		{
			if (array[array.Length - 1] == g_MusicSource.MusicTrackClip.ToString())
			{
				return -1f;
			}
		}
		else
		{
			g_MusicSource = CacheManager.The().Spawn<SoundHandler>("MusicSource");
		}
		return g_MusicSource.PlayMusic(track);
	}

	public void StopMusic()
	{
		if (g_MusicSource != null)
		{
			g_MusicSource.StopClip();
			CacheManager.The().Unspawn(g_MusicSource.gameObject);
			g_MusicSource = null;
		}
	}

	private void SetPlayerSavedSoundSettings()
	{
		if (PlayerData.AllTimeAppLaunches <= 1)
		{
			m_MusicEnabled = true;
			m_SoundEnabled = true;
		}
		else
		{
			m_MusicEnabled = PerryiCloudManager.The.GetBoolItem("_MusicEnabledkey");
			m_SoundEnabled = PerryiCloudManager.The.GetBoolItem("_SoundEnabledkey");
		}
	}

	private void GamePause()
	{
		PlayerData.SavePersistentData();
		m_isPaused = true;
		PauseSoundClips();
		MainMenuEventManager.TriggerStopRecording();
	}

	private void GameUnPause()
	{
		m_isPaused = false;
		ResumeSoundClips();
	}

	private void CollectScoreMultiplier()
	{
		PlayerData.RoundPowerups++;
		PlayerData.AllTimePowerups++;
		PlayerData.RoundScoreMultipliers++;
		PlayerData.AllTimeScoreMultipliers++;
		m_isScoreMultiplierOn = true;
		m_inPowerupCooldown = false;
		m_currentScoreMultiplierTime = 0f;
	}

	private void TempInvincibilityOnListener()
	{
		m_tempContinueInvincibility = true;
		GameEventManager.TriggerInvincibility();
	}

	private void CollectInvincibility()
	{
		if (!m_tempContinueInvincibility)
		{
			PlayerData.RoundPowerups++;
			PlayerData.AllTimePowerups++;
			PlayerData.RoundInvincibility++;
		}
		m_isInvincibilityOn = true;
		m_inPowerupCooldown = false;
		m_currentInvincibilityTime = 0f;
	}

	private void CollectPowerUpMagnet()
	{
		PlayerData.RoundPowerups++;
		PlayerData.AllTimePowerups++;
		PlayerData.RoundMagnetizers++;
		PlayerData.AllTimeMagnetizers++;
	}

	private void TokensStart()
	{
	}

	private void TokensUpdate()
	{
		if (IsInGamePlay() && !(Runner.The() == null))
		{
			float distance = Runner.The().m_Distance;
			if (PlayerData.DoubleTokensEnabled && distance > (float)PlayerData.DoubleTokenAppearTime && Token.ms_TokenStates == Token.TokenStates.Single)
			{
				Token.ms_TokenStates = Token.TokenStates.Double;
			}
			if (PlayerData.TripleTokensEnabled && distance > (float)PlayerData.TripleTokenAppearTime && Token.ms_TokenStates == Token.TokenStates.Double)
			{
				Token.ms_TokenStates = Token.TokenStates.Triple;
			}
		}
	}

	private void TokensEnd()
	{
		Token.ResetTokenState();
	}

	private void PlatformDifficultyStart()
	{
		m_PlatformDifficultyStateDistStart = 0f;
	}

	private void PlatformDifficultyUpdate()
	{
		if (m_currentGameplayState == GameplayState.GamePlay_Action && !(Runner.The() == null))
		{
			float num = Runner.The().m_Distance / 10f;
			if (m_PlatformDifficultyStateDistStart + m_PlatformDifficultyDistToNextState < num)
			{
				m_PlatformDifficultyStateDistStart = num;
			}
		}
	}

	private void PlatformDifficultyEnd()
	{
		m_PlatformDifficultyStateDistStart = 0f;
	}

	public bool CanSpawnTokens()
	{
		if (m_TokenGroupCountPerSpawn > m_TokenSpawnCountCur)
		{
			return true;
		}
		if (m_TokenTimeLastSpawned + m_TokenFrequency < Time.fixedTime)
		{
			m_TokenSpawnCountCur = 0;
			return true;
		}
		return false;
	}

	public bool CanUseCopterBoostJumpStart()
	{
		if (PlayerData.JumpStarts > 0)
		{
			if (Runner.The().IsInTutorialState())
			{
				return true;
			}
			if (m_InGameStopWatch.IsPaused())
			{
				return true;
			}
			if (HUDGUIManager.The.m_CopterBoostDuration > m_InGameStopWatch.RetrieveTimeElapsed())
			{
				return true;
			}
		}
		return false;
	}

	private void TokenLastSpawnedHandler()
	{
		m_TokenSpawnCountCur++;
		m_TokenTimeLastSpawned = Time.fixedTime;
	}

	private void TokenHit(int val)
	{
		PlayerData.RoundTokens += val;
		PlayerData.playerTokens += val;
		PlayerData.AllTimeTokens += val;
	}

	private void FedoraHit(int val)
	{
		PlayerData.playerFedoras += val;
		PlayerData.RoundFedoras += val;
		PlayerData.AllTimeFedoras += val;
	}

	public void FileLoad(string filePath, LocalTextManager.FileDoneLoad callback)
	{
		StartCoroutine(StreamLoadFile(filePath, callback));
	}

	private IEnumerator StreamLoadFile(string filePath, LocalTextManager.FileDoneLoad callback)
	{
		if (filePath.Contains("://"))
		{
			WWW www = new WWW(filePath);
			yield return www;
			if (www.error != null)
			{
				Debug.LogWarning("[GameManager.StreamLoadFile] WWW error loading '" + filePath + "': " + www.error);
				callback(string.Empty);
			}
			else
			{
				Debug.Log("[GameManager.StreamLoadFile] Successfully loaded (WWW): " + filePath);
				callback(www.text);
			}
		}
		else
		{
			TextAsset fullText = (TextAsset)ResourcesMonitor.Load(filePath);
			if (fullText == null)
			{
				Debug.LogWarning("[GameManager.StreamLoadFile] FAILED to load '" + filePath + "' - Resources.Load returned null! Check path casing.");
				callback(string.Empty); 
			}
			else
			{
				Debug.Log("[GameManager.StreamLoadFile] Successfully loaded: " + filePath);
				callback(fullText.text);
			}
		}
	}
}

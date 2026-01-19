using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public sealed class GlobalGUIManager : MonoBehaviour
{
	public struct CharacterSelectData
	{
		public GameObject camPos;

		public GameObject dotsPos;

		public GameObject scrollLPos;

		public GameObject scrollRPos;

		public GameObject buyBtnPos;

		public GameObject charSelectRect;
	}

	private MainMenuEventManager.MenuState m_currentMenuState;

	private MainMenuEventManager.MenuState m_lastMajorMenuState;

	public UIToolkit m_defaultTextToolkit;

	public UIToolkit m_altTextToolkit;

	public UIToolkit m_hudToolkit;

	public UIToolkit m_menuToolkit;

	private static UIText m_defaultText;

	private static UIText m_defaultTextAlt;

	private CharacterSelectData[] m_mainMenuCharacterSelectData;

	private CharacterSelectData[] m_inGameMenuCharacterSelectData;

	private bool m_checkingCompleteMissions;

	private int m_currentMissionIndex;

	private Action<bool> m_onAllMissionsCompleteAction;

	private static bool m_isInit;

	private static GlobalGUIManager m_the;

	public Camera m_menuCam;

	public Camera m_menuBGCam;

	public Light m_MenuLight;

	public GameObject[] m_mainMenuCharacters;

	public GameObject[] m_inGameMenuCharacters;

	public RawImage m_menuBG;

	public FaceTexture faceTexture;

	private bool m_IsInited;

	private bool m_IsDefaultTextInited;

	public bool m_IsAnimatingInGameCompletedMissionGroup;

	private float spriteScaleFactor = 2.5f;

	public MainMenuEventManager.MenuState CurrentMenuState
	{
		get
		{
			return m_currentMenuState;
		}
		set
		{
			m_currentMenuState = value;
		}
	}

	public UIText defaultText
	{
		get
		{
			if (m_defaultText == null)
			{
				InitMenuText();
			}
			return m_defaultText;
		}
	}

	public UIText defaultTextAlt
	{
		get
		{
			if (m_defaultTextAlt == null)
			{
				InitMenuText();
			}
			return m_defaultTextAlt;
		}
	}

	public static GlobalGUIManager The
	{
		get
		{
			if (m_the == null)
			{
				m_the = GameObject.Find("GlobalGUIManager").GetComponent<GlobalGUIManager>();
				Debug.LogWarning("Trying to access GlobalGUIManager.The before it was set");
			}
			return m_the;
		}
	}

	private void OnDisable()
	{
		GameEventManager.TokenHit -= HandleTokenHit;
		GameEventManager.ScoreUpdate -= HandleScoreUpdate;
		GameEventManager.UpdateBossHealth -= HandleUpdateBossHealth;
		GameEventManager.BossFireAttackStart -= HandleBossAttackStart;
		GameEventManager.BossWaterAttackStart -= HandleBossAttackStart;
		GameEventManager.BossIceAttackStart -= HandleBossAttackStart;
		GameEventManager.BossEndEvents -= HandleGoBackToLevel;
		GameEventManager.CollectScoreMultiplier -= HandleCollectScoreMultiplier;
		GameEventManager.ScoreMultiplierOff -= HandleScoreMultiplierOff;
		MainMenuEventManager.GoToNextMenu -= HandleGoToNextMenu;
		MainMenuEventManager.GoToPreviousMenu -= HandleGoToPreviousMenu;
		MainMenuEventManager.BuyMissionPopUp -= HandleBuyMissionPopUp;
		MainMenuEventManager.CompleteMissionPopUp -= HandleCompleteMissionPopUp;
		MainMenuEventManager.StartAnimation -= HandleStartAnimation;
		MainMenuEventManager.EndAnimation -= HandleEndAnimation;
		MainMenuEventManager.BuyTokenItem -= HandleBuyTokenItem;
		MainMenuEventManager.BuyGadgetItem -= HandleBuyGadgetItem;
		MainMenuEventManager.BuyUpgradeItem -= HandleBuyUpgradeItem;
		MainMenuEventManager.BuyPurchasableItem -= HandleBuyPurchasableItem;
		MainMenuEventManager.StartRecording -= HandleStartRecording;
		MainMenuEventManager.StopRecording -= HandleStopRecording;
		MainMenuEventManager.PlayWithCurrentCharacter -= HandlePlayCurrentCharacter;
		MainMenuEventManager.BuyCharacterItem -= BuyCharacter;
		MainMenuEventManager.UseJumpStart -= HandleUseJumpStart;
		MainMenuEventManager.StartMonogramTalking -= HandleStartMonogramTalking;
		MainMenuEventManager.StopMonogramTalking -= HandleStopMonogramTalking;
		MainMenuEventManager.ShowMeterNotification -= HandleShowMeterNotification;
		MainMenuEventManager.ShowMissionNotification -= HandleShowMissionNotification;
	}

	private void OnEnable()
	{
		GameEventManager.TokenHit += HandleTokenHit;
		GameEventManager.ScoreUpdate += HandleScoreUpdate;
		GameEventManager.UpdateBossHealth += HandleUpdateBossHealth;
		GameEventManager.BossFireAttackStart += HandleBossAttackStart;
		GameEventManager.BossWaterAttackStart += HandleBossAttackStart;
		GameEventManager.BossIceAttackStart += HandleBossAttackStart;
		GameEventManager.BossEndEvents += HandleGoBackToLevel;
		GameEventManager.CollectScoreMultiplier += HandleCollectScoreMultiplier;
		GameEventManager.ScoreMultiplierOff += HandleScoreMultiplierOff;
		MainMenuEventManager.GoToNextMenu += HandleGoToNextMenu;
		MainMenuEventManager.GoToPreviousMenu += HandleGoToPreviousMenu;
		MainMenuEventManager.BuyMissionPopUp += HandleBuyMissionPopUp;
		MainMenuEventManager.CompleteMissionPopUp += HandleCompleteMissionPopUp;
		MainMenuEventManager.StartAnimation += HandleStartAnimation;
		MainMenuEventManager.EndAnimation += HandleEndAnimation;
		MainMenuEventManager.BuyTokenItem += HandleBuyTokenItem;
		MainMenuEventManager.BuyGadgetItem += HandleBuyGadgetItem;
		MainMenuEventManager.BuyUpgradeItem += HandleBuyUpgradeItem;
		MainMenuEventManager.BuyPurchasableItem += HandleBuyPurchasableItem;
		MainMenuEventManager.StartRecording += HandleStartRecording;
		MainMenuEventManager.StopRecording += HandleStopRecording;
		MainMenuEventManager.PlayWithCurrentCharacter += HandlePlayCurrentCharacter;
		MainMenuEventManager.BuyCharacterItem += BuyCharacter;
		MainMenuEventManager.UseJumpStart += HandleUseJumpStart;
		MainMenuEventManager.StartMonogramTalking += HandleStartMonogramTalking;
		MainMenuEventManager.StopMonogramTalking += HandleStopMonogramTalking;
		MainMenuEventManager.ShowMeterNotification += HandleShowMeterNotification;
		MainMenuEventManager.ShowMissionNotification += HandleShowMissionNotification;
	}

	private void Awake()
	{
		m_the = this;
		LocalTextManager.Init();
		// hard code to "prompt already handled", age check doesn't work and it doesn't matter now.
		GameManager.The.playerIsUnderThirteen = false;
		GameManager.The.hasPromptedPlayerforBirthday = true;
	}

	private void Start()
	{
		LoadFontTexturesForCurrentLanguage();
		if (GameManager.The.IsLowEndDevice())
		{
			Shader.globalMaximumLOD = 900;
		}
		else
		{
			Shader.globalMaximumLOD = int.MaxValue;
		}
	}

	private void OnExitPromptCancelled(object o)
	{
	}

	private void OnExitPromptConfirmed(object o)
	{
		PlayHavenController.ContentRequest(PlayHavenController.PlayHavenPlacement.AppExit);
		Invoke("ExitGame", 0.5f);
	}

	private void ExitGame()
	{
		Application.Quit();
	}

	private void Update()
	{
		if (!Input.GetKey(KeyCode.Escape) || !Input.anyKeyDown || GameManager.currentGameplayState == GameManager.GameplayState.GamePlay_Intro)
		{
			return;
		}
		if (PopUpGUIManager.The.isAPopupActive)
		{
			if (m_currentMenuState == MainMenuEventManager.MenuState.Main_Menu_Character_Select)
			{
				if (PopUpGUIManager.The.m_PopupType == PopUpGUIManager.PopUpType.ShowGameExitConfirmation)
				{
					PopUpGUIManager.The.FakeTouchUpOkButton();
				}
				else
				{
					PopUpGUIManager.The.FakeTouchUpCancelButton();
				}
			}
			else if (The.IsInPauseMenu())
			{
				PopUpGUIManager.The.FakeTouchUpOkButton();
			}
			else
			{
				PopUpGUIManager.The.FakeTouchUpCancelButton();
			}
		}
		else if (IsInInGameMenu())
		{
			if (ExternalButtonsGUIManager.HomeButtonInGame != null && !ExternalButtonsGUIManager.HomeButtonInGame.hidden)
			{
				MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Main_Menu_Character_Select);
			}
		}
		else if (GameManager.The.IsInGamePlay())
		{
			if (m_currentMenuState == MainMenuEventManager.MenuState.Pause_Menu)
			{
				PauseGUIManager.The.FakePressHomeButton();
				return;
			}
			MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Pause_Menu);
			GameEventManager.TriggerGamePause();
		}
		else if (m_currentMenuState == MainMenuEventManager.MenuState.Main_Menu_Character_Select)
		{
			PopUpGUIManager.The.ShowGameExitConfirmationPrompt(OnExitPromptCancelled, OnExitPromptConfirmed, delegate
			{
			});
		}
		else if (m_currentMenuState == MainMenuEventManager.MenuState.Game_ContinueScreen)
		{
			ContinueScreenGUIManager.The.FakeTouchUpSpeedupWindow();
		}
		else
		{
			MainMenuEventManager.TriggerGoToPreviousMenu();
		}
	}

	public void OnLocalTextManagerComplete()
	{
		Debug.Log("[GlobalGUIManager] OnLocalTextManagerComplete() called - about to call InitAllUINow()");
		InitAllUINow();
		Debug.Log("[GlobalGUIManager] InitAllUINow() completed - about to call ReloadAllUIText()");
		ReloadAllUIText();
		Debug.Log("[GlobalGUIManager] ReloadAllUIText() completed");
	}

	public void InitAllUINow()
	{
		if (!m_IsInited)
		{
			GameManager.The.InitPersistentData();
			GameObject gameObject = new GameObject("MongramTalkingHandler");
			gameObject.AddComponent<MongramTalkingHandler>();
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			InitBackgrounds();
			InitCharacterSelectData();
			AddAllGUIManagers();
			HideAll();
			m_menuCam.gameObject.SetActive(true);
			if (GameManager.The.hasPromptedPlayerforBirthday)
			{
				Debug.Log("Calling ShowMainMenu. Horray!");
				ShowMainMenu();
			}
			else
			{
				ShowMainMenu();
			}
			m_IsInited = true;
		}
	}

	private void InitBackgrounds()
	{
		// RawImage uses RectTransform for positioning and sizing
		// Set anchors to stretch to fill the screen
		m_menuBG.rectTransform.anchorMin = Vector2.zero;
		m_menuBG.rectTransform.anchorMax = Vector2.one;
		m_menuBG.rectTransform.offsetMin = Vector2.zero;
		m_menuBG.rectTransform.offsetMax = Vector2.zero; // +1 pixel like the original
	}

	private void InitCharacterSelectData()
	{
		if (m_mainMenuCharacters != null && m_mainMenuCharacters.Length > 0)
		{
			m_mainMenuCharacterSelectData = new CharacterSelectData[m_mainMenuCharacters.Length];
			InitMainMenuCharacterModelUIPlacements();
		}
		if (m_inGameMenuCharacters != null && m_inGameMenuCharacters.Length > 0)
		{
			m_inGameMenuCharacterSelectData = new CharacterSelectData[m_inGameMenuCharacters.Length];
			InitInGameMenuCharacterModelUIPlacements();
		}
		ShowMainMenuCharacters(false);
		ShowInGameMenuCharacters(false);
	}

	private void InitMainMenuCharacterModelUIPlacements()
	{
		for (int i = 0; i < m_mainMenuCharacters.Length; i++)
		{
			m_mainMenuCharacterSelectData[i].camPos = new GameObject("mainCamPos");
			m_mainMenuCharacterSelectData[i].camPos.transform.parent = m_mainMenuCharacters[i].transform;
			m_mainMenuCharacterSelectData[i].camPos.transform.localPosition = new Vector3(0f, 3f, 25f);
			m_mainMenuCharacterSelectData[i].dotsPos = new GameObject("posDots");
			m_mainMenuCharacterSelectData[i].dotsPos.transform.parent = m_mainMenuCharacters[i].transform;
			m_mainMenuCharacterSelectData[i].dotsPos.transform.localPosition = new Vector3(0f, -2.5f, 0f);
			m_mainMenuCharacterSelectData[i].scrollLPos = new GameObject("posScrollLeft");
			m_mainMenuCharacterSelectData[i].scrollLPos.transform.parent = m_mainMenuCharacters[i].transform;
			m_mainMenuCharacterSelectData[i].scrollLPos.transform.localPosition = new Vector3(6f, 4f, 0f);
			m_mainMenuCharacterSelectData[i].scrollRPos = new GameObject("posScrollRight");
			m_mainMenuCharacterSelectData[i].scrollRPos.transform.parent = m_mainMenuCharacters[i].transform;
			m_mainMenuCharacterSelectData[i].scrollRPos.transform.localPosition = new Vector3(-6f, 4f, 0f);
			m_mainMenuCharacterSelectData[i].buyBtnPos = new GameObject("buyBtnPos");
			m_mainMenuCharacterSelectData[i].buyBtnPos.transform.parent = m_mainMenuCharacters[i].transform;
			m_mainMenuCharacterSelectData[i].buyBtnPos.transform.localPosition = new Vector3(0f, 13f, 0f);
			m_mainMenuCharacterSelectData[i].charSelectRect = new GameObject("charSelectRect");
			m_mainMenuCharacterSelectData[i].charSelectRect.transform.parent = m_mainMenuCharacters[i].transform;
			m_mainMenuCharacterSelectData[i].charSelectRect.transform.localPosition = new Vector3(0f, 3.5f, 0f);
			m_mainMenuCharacterSelectData[i].charSelectRect.transform.localScale = new Vector3(10f, 20f, 1f);
			m_mainMenuCharacterSelectData[i].charSelectRect.layer = LayerMask.NameToLayer("CharSelectRect");
			BoxCollider boxCollider = (BoxCollider)m_mainMenuCharacterSelectData[i].charSelectRect.AddComponent<BoxCollider>();
			boxCollider.size = new Vector3(1f, 1f, 1f);
		}
	}

	private void InitInGameMenuCharacterModelUIPlacements()
	{
		for (int i = 0; i < m_inGameMenuCharacters.Length; i++)
		{
			m_inGameMenuCharacterSelectData[i].camPos = new GameObject("mainCamPos");
			m_inGameMenuCharacterSelectData[i].camPos.transform.parent = m_inGameMenuCharacters[i].transform;
			m_inGameMenuCharacterSelectData[i].camPos.transform.localPosition = new Vector3(-8f, -8f, 42f);
			m_inGameMenuCharacterSelectData[i].dotsPos = new GameObject("posDots");
			m_inGameMenuCharacterSelectData[i].dotsPos.transform.parent = m_inGameMenuCharacters[i].transform;
			m_inGameMenuCharacterSelectData[i].dotsPos.transform.localPosition = new Vector3(0f, -0.5f, 0f);
			m_inGameMenuCharacterSelectData[i].scrollLPos = new GameObject("posScrollLeft");
			m_inGameMenuCharacterSelectData[i].scrollLPos.transform.parent = m_inGameMenuCharacters[i].transform;
			m_inGameMenuCharacterSelectData[i].scrollLPos.transform.localPosition = new Vector3(4f, 6f, 0f);
			m_inGameMenuCharacterSelectData[i].scrollRPos = new GameObject("posScrollRight");
			m_inGameMenuCharacterSelectData[i].scrollRPos.transform.parent = m_inGameMenuCharacters[i].transform;
			m_inGameMenuCharacterSelectData[i].scrollRPos.transform.localPosition = new Vector3(-4f, 6f, 0f);
			m_inGameMenuCharacterSelectData[i].buyBtnPos = new GameObject("buyBtnPos");
			m_inGameMenuCharacterSelectData[i].buyBtnPos.transform.parent = m_inGameMenuCharacters[i].transform;
			m_inGameMenuCharacterSelectData[i].buyBtnPos.transform.localPosition = new Vector3(0f, 13f, 0f);
			m_inGameMenuCharacterSelectData[i].charSelectRect = null;
		}
	}

	public PlayableCharacterChooser FindPlayableCharacterChooser(int characterIndex)
	{
		if (characterIndex >= m_mainMenuCharacters.Length)
		{
			return null;
		}
		return m_mainMenuCharacters[characterIndex].GetComponent<PlayableCharacterChooser>();
	}

	public PlayableCharacterChooser FindInGamePlayableCharacterChooser(int characterIndex)
	{
		if (characterIndex >= m_inGameMenuCharacters.Length)
		{
			return null;
		}
		return m_inGameMenuCharacters[characterIndex].GetComponent<PlayableCharacterChooser>();
	}

	private void InitMenuText()
	{
		if (!m_IsDefaultTextInited)
		{
			m_IsDefaultTextInited = true;
			m_defaultText = new UIText(m_defaultTextToolkit, "snyder", "snyder.png");
			m_defaultTextAlt = new UIText(m_altTextToolkit, "futura", "futura.png");
		}
	}

	private void AddAllGUIManagers()
	{
		GameObject gameObject = new GameObject("UI Managers");
		GameObject gameObject2 = new GameObject("MainMenuGUIManager");
		MainMenuGUIManager mainMenuGUIManager = (MainMenuGUIManager)gameObject2.AddComponent<MainMenuGUIManager>();
		gameObject2.transform.parent = gameObject.transform;
		mainMenuGUIManager.Init();
		GameObject gameObject3 = new GameObject("ExternalButtonsGUIManager");
		ExternalButtonsGUIManager externalButtonsGUIManager = (ExternalButtonsGUIManager)gameObject3.AddComponent<ExternalButtonsGUIManager>();
		gameObject3.transform.parent = gameObject.transform;
		externalButtonsGUIManager.Init();
		GameObject gameObject4 = new GameObject("CharSelectGUIManager");
		CharSelectGUIManager charSelectGUIManager = (CharSelectGUIManager)gameObject4.AddComponent<CharSelectGUIManager>();
		gameObject4.transform.parent = gameObject.transform;
		charSelectGUIManager.Init();
		MissionsGUIManager the = MissionsGUIManager.The;
		the.transform.parent = gameObject.transform;
		GameObject gameObject5 = new GameObject("StoreGUIManagerPersistentElements");
		StoreGUIManagerPersistentElements storeGUIManagerPersistentElements = (StoreGUIManagerPersistentElements)gameObject5.AddComponent<StoreGUIManagerPersistentElements>();
		gameObject5.transform.parent = gameObject.transform;
		storeGUIManagerPersistentElements.Init();
		GameObject gameObject6 = new GameObject("ShopPlayGUIFrame");
		ShopPlayGUIManager shopPlayGUIManager = (ShopPlayGUIManager)gameObject6.AddComponent<ShopPlayGUIManager>();
		gameObject6.transform.parent = gameObject.transform;
		shopPlayGUIManager.Init();
		GameObject gameObject7 = new GameObject("StoreGUIManagerShop");
		StoreGUIManagerShop storeGUIManagerShop = (StoreGUIManagerShop)gameObject7.AddComponent<StoreGUIManagerShop>();
		gameObject7.transform.parent = gameObject.transform;
		storeGUIManagerShop.Init();
		GameObject gameObject8 = new GameObject("PopUpGUIManager");
		PopUpGUIManager popUpGUIManager = (PopUpGUIManager)gameObject8.AddComponent<PopUpGUIManager>();
		gameObject8.transform.parent = gameObject.transform;
		popUpGUIManager.m_popUpBG = m_menuBG;
		popUpGUIManager.Init();
		GameObject gameObject9 = new GameObject("SettingsGUIManager");
		SettingsGUIManager settingsGUIManager = (SettingsGUIManager)gameObject9.AddComponent<SettingsGUIManager>();
		gameObject9.transform.parent = gameObject.transform;
		settingsGUIManager.m_popUpBG = m_menuBG;
		settingsGUIManager.Init();
		GameObject gameObject10 = new GameObject("HUDGUIManager");
		HUDGUIManager hUDGUIManager = (HUDGUIManager)gameObject10.AddComponent<HUDGUIManager>();
		gameObject10.transform.parent = gameObject.transform;
		hUDGUIManager.Init();
		GameObject gameObject11 = new GameObject("PauseGUIManager");
		PauseGUIManager pauseGUIManager = (PauseGUIManager)gameObject11.AddComponent<PauseGUIManager>();
		gameObject11.transform.parent = gameObject.transform;
		pauseGUIManager.Init();
		GameObject gameObject12 = new GameObject("InGameMenuGUIManager");
		InGameMenuGUIManager inGameMenuGUIManager = (InGameMenuGUIManager)gameObject12.AddComponent<InGameMenuGUIManager>();
		gameObject12.transform.parent = gameObject.transform;
		inGameMenuGUIManager.Init();
		GameObject gameObject13 = new GameObject("ContinueScreenGUIManager");
		ContinueScreenGUIManager continueScreenGUIManager = (ContinueScreenGUIManager)gameObject13.AddComponent<ContinueScreenGUIManager>();
		gameObject13.transform.parent = gameObject.transform;
		continueScreenGUIManager.Init();
		GameObject gameObject14 = new GameObject("HighScoreGUIManager");
		HighScoreGUIManager highScoreGUIManager = (HighScoreGUIManager)gameObject14.AddComponent<HighScoreGUIManager>();
		gameObject14.transform.parent = gameObject.transform;
		highScoreGUIManager.Init();
		GameObject gameObject15 = new GameObject("MonogramGUIManager");
		MonogramGUIManager monogramGUIManager = (MonogramGUIManager)gameObject15.AddComponent<MonogramGUIManager>();
		gameObject15.transform.parent = gameObject.transform;
		Debug.Log("Successfully added all GUI managers.");
		monogramGUIManager.Init();
		string text = "Background_";
		if (UI.isHD)
		{
			text += UI.instance.hdExtension;
		}
		Texture texture = (Texture)ResourcesMonitor.Load(text, typeof(Texture));
		m_menuBG.texture = texture;
	}

	public void HideAll()
	{
		HideMainMenu();
		HideInGameMenu();
		HidePauseMenu();
		HideSettingsMenu();
		HideHUD();
		HideContinueScreen();
		HideStore();
	}

	private void HideAllCamerasButMain()
	{
		m_menuBGCam.gameObject.SetActive(false);
	}

	private void ShowStoryCameras()
	{
		CharSelectGUIManager.The.SetCameraPositionForStory();
		m_menuCam.gameObject.SetActive(true);
		m_menuBGCam.gameObject.SetActive(true);
	}

	private void ShowMainMenuCameras(bool show)
{
    Debug.Log("=== ShowMainMenuCameras called with show=" + show + " ===");
    Debug.Log("m_menuCam is: " + (m_menuCam == null ? "NULL" : "valid"));
    Debug.Log("m_menuBGCam is: " + (m_menuBGCam == null ? "NULL" : "valid"));
    Debug.Log("CharSelectGUIManager.The is: " + (CharSelectGUIManager.The == null ? "NULL" : "valid"));
    
    CharSelectGUIManager.The.ResetCameraNow(false);
    
    m_menuCam.gameObject.SetActive(show);
    Debug.Log("m_menuCam.gameObject.SetActive(" + show + ") called");
    
    m_menuBGCam.gameObject.SetActive(show);
    Debug.Log("m_menuBGCam.gameObject.SetActive(" + show + ") called");
    
    Debug.Log("=== ShowMainMenuCameras END ===");
}

	private void ShowInGameMenuCameras(bool show)
	{
		CharSelectGUIManager.The.ResetCameraNow(true);
		m_menuCam.gameObject.SetActive(show);
		m_menuBGCam.gameObject.SetActive(show);
	}

	private void ShowInGameMenuCamerasSansMask(bool show)
	{
		m_menuCam.gameObject.SetActive(show);
		m_menuBGCam.gameObject.SetActive(show);
	}

	private void HideMainMenu()
	{
		ShowMainMenuCameras(false);
		ShowMainMenuCharacters(false);
		ExternalButtonsGUIManager.The.HideAll();
		CharSelectGUIManager.The.HideAll();
		MissionsGUIManager.The.HideAll();
		StoreGUIManagerPersistentElements.The.HideAll();
		StoreGUIManagerShop.The.HideAll();
		MainMenuGUIManager.The.HideAll();
		MonogramGUIManager.The.HideAll();
	}

	public void HideInGameMenu()
	{
		ShowInGameMenuCameras(false);
		ShowInGameMenuCharacters(false);
		ExternalButtonsGUIManager.The.HideAll();
		CharSelectGUIManager.The.HideAll();
		MissionsGUIManager.The.HideAll();
		StoreGUIManagerPersistentElements.The.HideAll();
		InGameMenuGUIManager.The.HideAll();
		HighScoreGUIManager.The.HideAll();
		MonogramGUIManager.The.HideAll();
	}

	private void HidePauseMenu()
	{
		ShowMainMenuCameras(false);
		ExternalButtonsGUIManager.The.HideAll();
		MissionsGUIManager.The.HideAll();
		StoreGUIManagerPersistentElements.The.HideAll();
		PauseGUIManager.The.HideAll();
	}

	private void HideStore()
	{
		StoreGUIManagerPersistentElements.The.HideAll();
		StoreGUIManagerShop.The.HideAll();
	}

	private void HideSettingsMenu()
	{
		SettingsGUIManager.The.HideAll(true);
	}

	private void HideHUD()
	{
		HUDGUIManager.The.HideAll();
		MonogramGUIManager.The.HideAll();
		TutorialGUIManager.The.HideAll();
	}

	private void HideContinueScreen()
	{
		ContinueScreenGUIManager.The.HideAll();
	}

	private void ShowMainMenuCharacters(bool show)
	{
		if (show)
		{
		}
		GameObject[] mainMenuCharacters = m_mainMenuCharacters;
		foreach (GameObject gameObject in mainMenuCharacters)
		{
			gameObject.SetActive(show);
			if (!show)
			{
				continue;
			}
			gameObject.GetComponent<PlayableCharacterChooser>().ChooseModelFromRunnerModelChooser();
			SwitchCharacterMaterials(gameObject, 0);
			if (!GameManager.The.IsLowEndDevice())
			{
				continue;
			}
			LowEndMaterial[] componentsInChildren = gameObject.GetComponentsInChildren<LowEndMaterial>();
			if (componentsInChildren != null)
			{
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].AssignLowEndMaterials();
				}
			}
		}
	}

	private void ShowInGameMenuCharacters(bool show)
	{
		if (show)
		{
		}
		GameObject[] inGameMenuCharacters = m_inGameMenuCharacters;
		foreach (GameObject gameObject in inGameMenuCharacters)
		{
			gameObject.SetActive(show);
			if (!show)
			{
				continue;
			}
			gameObject.GetComponent<PlayableCharacterChooser>().ChooseModelFromRunnerModelChooser();
			SwitchCharacterMaterials(gameObject, 1);
			if (!GameManager.The.IsLowEndDevice())
			{
				continue;
			}
			LowEndMaterial[] componentsInChildren = gameObject.GetComponentsInChildren<LowEndMaterial>();
			if (componentsInChildren != null)
			{
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].SetDontAssignOnStart(true);
				}
			}
		}
	}

	private void SwitchCharacterMaterials(GameObject g, int index)
	{
		MaterialSwitch[] componentsInChildren = g.GetComponentsInChildren<MaterialSwitch>();
		if (componentsInChildren.Length > 0)
		{
			componentsInChildren[0].SwitchMaterial(index);
		}
		for (int i = 0; i < g.transform.childCount; i++)
		{
			Transform child = g.transform.GetChild(i);
			MaterialSwitch[] componentsInChildren2 = child.GetComponentsInChildren<MaterialSwitch>();
			MaterialSwitch[] array = componentsInChildren2;
			foreach (MaterialSwitch materialSwitch in array)
			{
				materialSwitch.SwitchMaterial(index);
			}
		}
	}

	public void ShowMainMenu()
	{
		Debug.Log("ShowMainMenu called");
		HideAll();
		m_currentMenuState = MainMenuEventManager.MenuState.Main_Menu_Character_Select;
		ShowMainMenuCameras(true);
		Debug.Log("Cameras should be active. m_menuCam is currently: " + m_menuCam.gameObject.activeSelf);
		ShowMainMenuCharacters(true);
		ExternalButtonsGUIManager.The.ShowMainMenuButtons();
		CharSelectGUIManager.The.ShowMainMenuCharSelect(ref m_mainMenuCharacterSelectData);
		StoreGUIManagerPersistentElements.The.ShowMainMenu();
		ShopPlayGUIManager.The.ShowMainMenu();
		MainMenuGUIManager.The.ShowMainMenuCharSelectMode();
		GameManager.The.PlayMusic(AudioClipFiles.MENUTHEME);
	}

	public void ShowStory()
	{
		m_currentMenuState = MainMenuEventManager.MenuState.Story;
		ShowStoryCameras();
		CharSelectGUIManager.The.DisableSelectControls();
		ShowMainMenuCharacters(true);
		MonogramGUIManager.The.ShowStory();
		MongramTalkingHandler.The.PlayOnLaunchStory();
		GameManager.The.CullSoundChannels();
	}

	public void ShowCOPA()
	{
		m_currentMenuState = MainMenuEventManager.MenuState.Copa;
		ShowStoryCameras();
		ShowAgePrompt();
	}

	public void ShowMainMenuMissions()
	{
		HideAll();
		MainMenuEventManager.MenuState currentMenuState = m_currentMenuState;
		m_currentMenuState = MainMenuEventManager.MenuState.Main_Menu_Missions;
		if (currentMenuState == MainMenuEventManager.MenuState.Main_Menu_Character_Select)
		{
			MainMenuEventManager.TriggerStartMonogramTalking(MongramTalkingHandler.MonogramTalkingState.MainMenuMissionsIntro);
		}
		ShowMainMenuCameras(true);
		StoreGUIManagerPersistentElements.The.ShowMissionsMainMenu();
		ShopPlayGUIManager.The.ShowMissionsMainMenu();
		MissionsGUIManager.The.ShowMainMenuMissions();
		MainMenuGUIManager.The.ShowmainMenuMissionsMode();
		MonogramGUIManager.The.ShowMainMenu();
		ExternalButtonsGUIManager.The.HideMainMenuButtons();
	}

	public void StartInGameMenuAnim()
	{
		HideAll();
		m_currentMenuState = MainMenuEventManager.MenuState.InGame_Missions_Menu;
		MainMenuEventManager.TriggerStartMonogramTalking(MongramTalkingHandler.MonogramTalkingState.GreatJob);
		ShowInGameMenuCameras(true);
		MissionsGUIManager.The.ShowInGameMenu();
		MonogramGUIManager.The.ShowInGameMenu();
		MonogramGUIManager.The.DisableMonogramButton();
		Invoke("AnimateInGameMenuStart", 1f);
	}

	private void MoveToMissionsAndHighScores()
	{
		MissionsGUIManager.The.UpdateMissionPositionsForInGameMenu();
		HighScoreGUIManager.The.UpdateHighScoresForInGameMenu();
		MissionsGUIManager.The.MoveMissions((float)(-Screen.width) * 2f, true);
		MissionsGUIManager.The.HideMultiplier();
		HighScoreGUIManager.The.MoveHighScores(Screen.width);
		HighScoreGUIManager.The.ShowHighScoresInGameMenu();
		HighScoreGUIManager.The.MoveHighScores(-Screen.width, true, delegate
		{
			InGameMenuGUIManager.The.ShowScores();
			InGameMenuGUIManager.The.AnimateScoreText(delegate
			{
				ShowInGameMenuElements();
			});
		});
	}

	private void OnInGameCompletedMissionsAnimFinished(bool unused)
	{
		if (m_IsAnimatingInGameCompletedMissionGroup)
		{
			m_IsAnimatingInGameCompletedMissionGroup = false;
			Invoke("MoveToMissionsAndHighScores", 3f);
		}
		else
		{
			MoveToMissionsAndHighScores();
		}
	}

	private void AnimateInGameMenuStart()
	{
		m_IsAnimatingInGameCompletedMissionGroup = false;
		PlayInGameCompletedMissionsAnim(OnInGameCompletedMissionsAnimFinished);
	}

	private void PlayInGameCompletedMissionsAnim(Action<bool> onDone)
	{
		m_checkingCompleteMissions = true;
		m_currentMissionIndex = 0;
		m_onAllMissionsCompleteAction = onDone;
		PlayCompleteMissionAnim(m_currentMissionIndex, onDone);
	}

	private void PlayCompleteMissionAnim(int i, Action<bool> onDone)
	{
		int missionCountPerGroup = PlayerData.GetMissionCountPerGroup();
		if (i < missionCountPerGroup && !AllMissionData.AreAllMissionsCompleted())
		{
			Mission missionForLevel = AllMissionData.GetMissionForLevel(PlayerData.MaxMissionLevelSeenByUserIndex, i);
			if (missionForLevel.Completed)
			{
				MissionsGUIManager.The.CompleteMissionAnim(missionForLevel);
				return;
			}
			m_currentMissionIndex++;
			PlayCompleteMissionAnim(m_currentMissionIndex, onDone);
		}
		else
		{
			m_checkingCompleteMissions = false;
			onDone(true);
		}
	}

	private void AnimateMoveToInGameMissions()
	{
		MissionsGUIManager.The.UpdateMissionPositionsForInGameMenu();
		HighScoreGUIManager.The.UpdateHighScoresForInGameMenu();
		InGameMenuGUIManager.The.UpdateScorePositions();
		MonogramGUIManager.The.ShowInGameMenu();
		MissionsGUIManager.The.ShowInGameMenu();
		MissionsGUIManager.The.MoveMissions(-Screen.width);
		MissionsGUIManager.The.MoveMissions(Screen.width, true);
		InGameMenuGUIManager.The.MoveScores(Screen.width, true);
		HighScoreGUIManager.The.MoveHighScores(Screen.width, true, delegate
		{
			ShowInGameMenuMissions();
		});
	}

	private void AnimateMoveToInGameHighscore()
	{
		MissionsGUIManager.The.UpdateMissionPositionsForInGameMenu();
		HighScoreGUIManager.The.UpdateHighScoresForInGameMenu();
		InGameMenuGUIManager.The.UpdateScorePositions();
		HighScoreGUIManager.The.ShowHighScoresInGameMenu();
		MissionsGUIManager.The.MoveMissions(-Screen.width, true);
		InGameMenuGUIManager.The.MoveScores(Screen.width);
		HighScoreGUIManager.The.MoveHighScores(Screen.width);
		HighScoreGUIManager.The.MoveHighScores(-Screen.width, true, delegate
		{
			ShowInGameMenuScores();
		});
	}

	public void ShowInGameMenuMissions()
	{
		HideAll();
		m_currentMenuState = MainMenuEventManager.MenuState.InGame_Missions_Menu;
		ShowInGameMenuCameras(true);
		ShowInGameMenuCharacters(true);
		ShopPlayGUIManager.The.ShowInGameMenu();
		ExternalButtonsGUIManager.The.ShowInGameMenuButtons();
		CharSelectGUIManager.The.ShowInGameMenuCharSelect(ref m_inGameMenuCharacterSelectData);
		MissionsGUIManager.The.ShowInGameMenu();
		MonogramGUIManager.The.ShowInGameMenu();
	}

	public void ShowInGameMenuScores()
	{
		HideAll();
		m_currentMenuState = MainMenuEventManager.MenuState.InGame_Score_Menu;
		ShowInGameMenuCameras(true);
		ShowInGameMenuCharacters(true);
		StoreGUIManagerPersistentElements.The.ShowInGameMenu();
		StoreGUIManagerPersistentElements.The.DisableBottomButtons(false);
		ShopPlayGUIManager.The.ShowInGameMenu();
		ExternalButtonsGUIManager.The.ShowInGameMenuButtons();
		CharSelectGUIManager.The.ShowInGameMenuCharSelect(ref m_inGameMenuCharacterSelectData);
		HighScoreGUIManager.The.ShowHighScoresInGameMenu();
		InGameMenuGUIManager.The.ShowScores();
	}

	private void ShowInGameMenuElements()
	{
		m_currentMenuState = MainMenuEventManager.MenuState.InGame_Score_Menu;
		ShowInGameMenuCameras(true);
		ShowInGameMenuCharacters(true);
		StoreGUIManagerPersistentElements.The.ShowInGameMenu();
		StoreGUIManagerPersistentElements.The.DisableBottomButtons(false);
		ShopPlayGUIManager.The.ShowInGameMenu();
		ExternalButtonsGUIManager.The.ShowInGameMenuButtons();
		CharSelectGUIManager.The.ShowInGameMenuCharSelect(ref m_inGameMenuCharacterSelectData);
		HighScoreGUIManager.The.ShowHighScoresInGameMenu();
		MonogramGUIManager.The.EnableMonogramButton();
	}

	public void ShowPauseMenu()
	{
		HideAll();
		m_currentMenuState = MainMenuEventManager.MenuState.Pause_Menu;
		ShowMainMenuCameras(true);
		float num = 10f;
		float xExtent = (float)Screen.width - num;
		MissionsGUIManager.The.ShowPauseMenuMissions(xExtent);
		ShopPlayGUIManager.The.ShowPauseMenu();
		PauseGUIManager.The.ShowPauseMenu();
	}

	public void ShowGetTokensStoreMenu()
	{
		HideAll();
		m_currentMenuState = MainMenuEventManager.MenuState.Store_Menu_Tokens;
		ShowMainMenuCameras(true);
		StoreGUIManagerPersistentElements.The.ShowStoreMenu();
		StoreGUIManagerShop.The.GoToGetTokenShop();
		ShopPlayGUIManager.The.HideButtons();
	}

	public void ShowGadgetStoreMenu()
	{
		HideAll();
		m_currentMenuState = MainMenuEventManager.MenuState.Store_Menu_Gadgets;
		ShowMainMenuCameras(true);
		StoreGUIManagerPersistentElements.The.ShowStoreMenu();
		StoreGUIManagerShop.The.GoToGadgetShop();
		ShopPlayGUIManager.The.HideButtons();
	}

	public void ShowUpgradeStoreMenu()
	{
		HideAll();
		m_currentMenuState = MainMenuEventManager.MenuState.Store_Menu_Upgrades;
		ShowMainMenuCameras(true);
		StoreGUIManagerPersistentElements.The.ShowStoreMenu();
		StoreGUIManagerShop.The.GoToUpgradeShop();
		ShopPlayGUIManager.The.HideButtons();
	}

	public void ShowFreeStoreMenu()
	{
		HideAll();
		m_currentMenuState = MainMenuEventManager.MenuState.Store_Menu_FreeTokens;
		ShowMainMenuCameras(true);
		StoreGUIManagerPersistentElements.The.ShowFreeTokensStoreMenu();
		StoreGUIManagerShop.The.GoToFreeTokensShop();
		ShopPlayGUIManager.The.HideButtons();
	}

	public void ShowSettingsMenu()
	{
		HideAll();
		m_currentMenuState = MainMenuEventManager.MenuState.Settings_Menu;
		ShowMainMenuCameras(true);
		SettingsGUIManager.The.ShowSettingsMenu();
		ShopPlayGUIManager.The.HideButtons();
	}

	public void ShowSettingsInfoMenu()
	{
		HideAll();
		m_currentMenuState = MainMenuEventManager.MenuState.Settings_Info_Menu;
		ShowMainMenuCameras(true);
		SettingsGUIManager.The.ShowInfoMenu();
	}

	public void ShowSettingsStorageMenu()
	{
		HideAll();
		m_currentMenuState = MainMenuEventManager.MenuState.Settings_Storage_Menu;
		ShowMainMenuCameras(true);
		SettingsGUIManager.The.ShowStorageMenu();
	}

	public void ShowHUDOnGameIntro()
	{
		HideAll();
		if (!GamePlay.ms_ShouldSkipTubeIntro)
		{
			MonogramGUIManager.The.ShowGameIntro();
			if (!IsInInGameMenu())
			{
				MainMenuEventManager.TriggerStartMonogramTalking(MongramTalkingHandler.MonogramTalkingState.GamePlay_TubeIntro);
			}
			m_currentMenuState = MainMenuEventManager.MenuState.Game_HUD;
		}
	}

	public void ShowHUDonGameStart()
	{
		ShowHUD();
		if (PlayerData.JumpStarts > 0 && PlayerData.ShouldNotShowTutorial)
		{
			HUDGUIManager.The.StartShowJumpStartDelayed(3f);
		}
	}

	public void ShowHUD()
	{
		m_currentMenuState = MainMenuEventManager.MenuState.Game_HUD;
		if (GameManager.currentGameplayState == GameManager.GameplayState.GamePlay_Action && ((DoofenCruiser.The() != null && DoofenCruiser.The().IsActive()) || (Balloony.The != null && Balloony.The.IsActive())))
		{
			HUDGUIManager.The.ShowHUDWithBoss();
		}
		else
		{
			HUDGUIManager.The.ShowHUDNoCameraNoBoss();
		}
		if (HUDGUIManager.The.IsInJumpStartState())
		{
			HUDGUIManager.The.ShowJumpStartButton();
		}
	}

	public void ShowContinueScreen()
	{
		HideAll();
		m_currentMenuState = MainMenuEventManager.MenuState.Game_ContinueScreen;
		HUDGUIManager.The.ShowScoreTokens();
		ContinueScreenGUIManager.The.ShowContinueScreen();
	}

	public void ShowTutorialTap()
	{
		TutorialGUIManager.The.ShowHandTouchAnim(3);
	}

	public void ShowTutorialSwipeLeft()
	{
		TutorialGUIManager.The.ShowSwipeAnim(-1, TutorialGUIManager.TutorialSwipeDirection.LEFT);
	}

	public void ShowTutorialSwipeRight()
	{
		TutorialGUIManager.The.ShowSwipeAnim(-1, TutorialGUIManager.TutorialSwipeDirection.RIGHT);
	}

	public void ShowTutorialSwipeUp()
	{
		TutorialGUIManager.The.ShowSwipeAnim(-1, TutorialGUIManager.TutorialSwipeDirection.UP);
	}

	public void ShowTutorialSwipeDown()
	{
		TutorialGUIManager.The.ShowSwipeAnim(-1, TutorialGUIManager.TutorialSwipeDirection.DOWN);
	}

	public Vector2 ConvertFromWorldToScreenCoords(Vector3 worldPos)
	{
		Camera menuCam = m_menuCam;
		Vector3 vector = menuCam.WorldToScreenPoint(worldPos);
		return new Vector2(vector.x, (float)Screen.height - vector.y);
	}

	public Camera GetCurrentCamera()
	{
		return m_menuCam;
	}

	public void SetCurrentCamPosition(Vector3 newCamPosition)
	{
		m_menuCam.transform.position = newCamPosition;
	}

	public CharacterSelectData getCharacterDataByIndex(int index)
	{
		if (IsInMainMenu())
		{
			if (index < m_mainMenuCharacterSelectData.Length)
			{
				return m_mainMenuCharacterSelectData[index];
			}
		}
		else if (IsInInGameMenu() && index < m_inGameMenuCharacterSelectData.Length)
		{
			return m_inGameMenuCharacterSelectData[index];
		}
		return m_inGameMenuCharacterSelectData[index];
	}

	public Transform getCharacterTransformByIndex(int index)
	{
		return m_mainMenuCharacters[index].transform;
	}

	public Transform getInGameCharacterTransformByIndex(int index)
	{
		return m_inGameMenuCharacters[index].transform;
	}

	public int GetTotalNumberOfCharInSelectScreen()
	{
		if (IsInInGameMenu())
		{
			return m_inGameMenuCharacters.Length;
		}
		if (IsInMainMenu())
		{
			return m_mainMenuCharacters.Length;
		}
		return m_mainMenuCharacters.Length;
	}

	public bool IsInMainMenu()
	{
		return m_currentMenuState == MainMenuEventManager.MenuState.Main_Menu_Character_Select || m_currentMenuState == MainMenuEventManager.MenuState.Main_Menu_Missions;
	}

	public bool IsInInGameMenu()
	{
		return m_currentMenuState == MainMenuEventManager.MenuState.InGame_Missions_Menu || m_currentMenuState == MainMenuEventManager.MenuState.InGame_Score_Menu;
	}

	public bool IsInCharSelectScreen()
	{
		return m_currentMenuState == MainMenuEventManager.MenuState.Main_Menu_Character_Select || m_currentMenuState == MainMenuEventManager.MenuState.InGame_Missions_Menu || m_currentMenuState == MainMenuEventManager.MenuState.InGame_Score_Menu;
	}

	public bool IsInPauseMenu()
	{
		return m_currentMenuState == MainMenuEventManager.MenuState.Pause_Menu;
	}

	public bool IsInStoreMenu()
	{
		return m_currentMenuState == MainMenuEventManager.MenuState.Store_Menu_Tokens || m_currentMenuState == MainMenuEventManager.MenuState.Store_Menu_Gadgets || m_currentMenuState == MainMenuEventManager.MenuState.Store_Menu_Upgrades;
	}

	public bool IsInHUD()
	{
		return m_currentMenuState == MainMenuEventManager.MenuState.Game_HUD;
	}

	private void HandleGoToNextMenu(MainMenuEventManager.MenuState menuState)
	{
		switch (menuState)
		{
		case MainMenuEventManager.MenuState.Main_Menu_Character_Select:
			GameEventManager.TriggerGameRestartMenu();
			ShowMainMenu();
			break;
		case MainMenuEventManager.MenuState.Main_Menu_Missions:
			ShowMainMenuMissions();
			break;
		case MainMenuEventManager.MenuState.Store_Menu_Tokens:
			if (IsInMainMenu() || IsInInGameMenu() || IsInPauseMenu())
			{
				m_lastMajorMenuState = m_currentMenuState;
			}
			ShowGetTokensStoreMenu();
			break;
		case MainMenuEventManager.MenuState.Store_Menu_Gadgets:
			if (IsInMainMenu() || IsInInGameMenu() || IsInPauseMenu())
			{
				m_lastMajorMenuState = m_currentMenuState;
			}
			ShowGadgetStoreMenu();
			break;
		case MainMenuEventManager.MenuState.Store_Menu_Upgrades:
			if (IsInMainMenu() || IsInInGameMenu() || IsInPauseMenu())
			{
				m_lastMajorMenuState = m_currentMenuState;
			}
			ShowUpgradeStoreMenu();
			break;
		case MainMenuEventManager.MenuState.Store_Menu_FreeTokens:
			if (IsInMainMenu() || IsInInGameMenu() || IsInPauseMenu())
			{
				m_lastMajorMenuState = m_currentMenuState;
			}
			ShowFreeStoreMenu();
			break;
		case MainMenuEventManager.MenuState.Settings_Menu:
			if (IsInMainMenu() || IsInInGameMenu() || IsInPauseMenu())
			{
				m_lastMajorMenuState = m_currentMenuState;
			}
			ShowSettingsMenu();
			break;
		case MainMenuEventManager.MenuState.Settings_Info_Menu:
			ShowSettingsInfoMenu();
			break;
		case MainMenuEventManager.MenuState.Settings_Storage_Menu:
			ShowSettingsStorageMenu();
			break;
		case MainMenuEventManager.MenuState.Pause_Menu:
			ShowPauseMenu();
			break;
		case MainMenuEventManager.MenuState.Game_HUD:
			ShowHUD();
			break;
		case MainMenuEventManager.MenuState.InGame_Missions_Menu:
			AnimateMoveToInGameMissions();
			break;
		case MainMenuEventManager.MenuState.InGame_Score_Menu:
			AnimateMoveToInGameHighscore();
			break;
		case MainMenuEventManager.MenuState.InGame_IntroAnim:
			StartInGameMenuAnim();
			break;
		case MainMenuEventManager.MenuState.Game_ContinueScreen:
			ShowContinueScreen();
			break;
		case MainMenuEventManager.MenuState.Game_ContinueGame:
			GameEventManager.TriggerGameContinue();
			ShowHUD();
			break;
		case MainMenuEventManager.MenuState.Pop_Up_Buy_Mission:
			break;
		}
	}

	private void HandleGoToPreviousMenu()
	{
		if (m_currentMenuState == MainMenuEventManager.MenuState.Settings_Info_Menu || m_currentMenuState == MainMenuEventManager.MenuState.Settings_Storage_Menu)
		{
			ShowSettingsMenu();
		}
		else if (m_lastMajorMenuState == MainMenuEventManager.MenuState.InGame_Missions_Menu)
		{
			ShowInGameMenuMissions();
		}
		else if (m_lastMajorMenuState == MainMenuEventManager.MenuState.InGame_Score_Menu)
		{
			ShowInGameMenuScores();
			InGameMenuGUIManager.The.ShowScores();
			MonogramGUIManager.The.ShowInGameMenu();
			InGameMenuGUIManager.The.ShowScoreTokenCountAndMultiplier();
		}
		else
		{
			HandleGoToNextMenu(m_lastMajorMenuState);
		}
	}

	private void HandleBuyMissionPopUp(Mission mission)
	{
		if (IsInMainMenu())
		{
			PopUpGUIManager.The.ShowBuyMissionPopUp(mission, MissionBuyPopUpDoneOK, MissionBuyPopUpDoneCancel, delegate
			{
				OnBuyMissionPopUpDisplay();
			});
		}
		else if (IsInInGameMenu())
		{
			PopUpGUIManager.The.ShowBuyMissionPopUp(mission, MissionBuyPopUpDoneOK, MissionBuyPopUpDoneCancel, delegate
			{
				OnBuyMissionPopUpDisplay();
			});
		}
		else if (IsInPauseMenu())
		{
			PopUpGUIManager.The.ShowBuyMissionPopUp(mission, MissionBuyPopUpDoneOK, MissionBuyPopUpDoneCancel, delegate
			{
				OnBuyMissionPopUpDisplay();
			});
		}
	}

	private void HandleCompleteMissionPopUp(Mission mission, bool getScoreBonus)
	{
		if (IsInMainMenu())
		{
			PopUpGUIManager.The.ShowMissionCompletePopUp(mission, getScoreBonus, MissionCompletePopUpDone, delegate
			{
				OnMissionCompletePopUpDisplay();
			});
		}
		else if (IsInInGameMenu())
		{
			PopUpGUIManager.The.ShowMissionCompletePopUp(mission, getScoreBonus, MissionCompletePopUpDone, delegate
			{
				OnMissionCompletePopUpDisplay();
			});
		}
		else if (IsInPauseMenu())
		{
			PopUpGUIManager.The.ShowMissionCompletePopUp(mission, getScoreBonus, MissionCompletePopUpDone, delegate
			{
				OnMissionCompletePopUpDisplay();
			});
		}
	}

	private void HandleStartAnimation()
	{
		if (IsInMainMenu())
		{
			MainMenuGUIManager.The.HideMainMenuMissions();
			MissionsGUIManager.The.DisableMissionButtons(true);
			StoreGUIManagerPersistentElements.The.DisableBottomButtons(true);
			ExternalButtonsGUIManager.The.HideMainMenuButtons();
		}
		else if (IsInInGameMenu())
		{
			ExternalButtonsGUIManager.The.HideInGameMenuButtons();
			MissionsGUIManager.The.DisableMissionButtons(true);
			StoreGUIManagerPersistentElements.The.DisableBottomButtons(true);
		}
		else if (IsInPauseMenu())
		{
			PauseGUIManager.The.HideUnPauseButton();
			MissionsGUIManager.The.DisableMissionButtons(true);
		}
	}

	private void HandleEndAnimation()
	{
		if (IsInMainMenu())
		{
			if (m_currentMenuState == MainMenuEventManager.MenuState.Main_Menu_Character_Select)
			{
				MainMenuGUIManager.The.ShowMainMenuCharSelectMode();
				ExternalButtonsGUIManager.The.ShowMainMenuButtons();
				MissionsGUIManager.The.DisableMissionButtons(false);
				StoreGUIManagerPersistentElements.The.DisableBottomButtons(false);
			}
			else if (m_currentMenuState == MainMenuEventManager.MenuState.Main_Menu_Missions)
			{
				MainMenuGUIManager.The.ShowmainMenuMissionsMode();
				MissionsGUIManager.The.DisableMissionButtons(false);
				StoreGUIManagerPersistentElements.The.DisableBottomButtons(false);
			}
		}
		else if (IsInInGameMenu())
		{
			ExternalButtonsGUIManager.The.ShowInGameMenuButtons();
			StoreGUIManagerPersistentElements.The.DisableBottomButtons(false);
			MissionsGUIManager.The.DisableMissionButtons(false);
			CharSelectGUIManager.The.ShowInGameMenuCharSelect(ref m_inGameMenuCharacterSelectData);
			if (m_currentMenuState != MainMenuEventManager.MenuState.InGame_Missions_Menu && m_currentMenuState != MainMenuEventManager.MenuState.InGame_Score_Menu)
			{
			}
		}
		else if (IsInPauseMenu())
		{
			ShowPauseMenu();
			MissionsGUIManager.The.DisableMissionButtons(false);
		}
	}

	private void HandleBuyTokenItem(PurchasableItem tokenItem)
	{
		Debug.Log("************* HandleBuyTokenItem DISABLE ALL BUTTONS **************");
		StoreGUIManagerShop.The.DisableAllShopButtons(ref StoreGUIManagerShop.m_getTokensShop, true);
		StoreGUIManagerPersistentElements.The.DisableAllButtons(true);
		AllItemData.BuyRealMoneyItem(true, tokenItem, delegate(bool success)
		{
			StoreGUIManagerShop.The.m_waitingForPurchase = false;
			if (success)
			{
				PopUpGUIManager.The.ShowPurchaseOutcomeConfirmation(LocalTextManager.GetUIText("_PURCHASE_COMPLETE_"), OnPurchaseOutcomePopupDismissed, delegate
				{
				});
			}
			else
			{
				PopUpGUIManager.The.ShowPurchaseOutcomeConfirmation(LocalTextManager.GetUIText("_PURCHASE_FAILED_"), OnPurchaseOutcomePopupDismissed, delegate
				{
				});
			}
		});
	}

	private void OnPurchaseOutcomePopupDismissed(object o)
	{
		StoreGUIManagerPersistentElements.The.UpdateMoney();
		StoreGUIManagerShop.The.DisableAllShopButtons(ref StoreGUIManagerShop.m_getTokensShop, false);
		StoreGUIManagerPersistentElements.The.DisableAllButtons(false);
	}

	private void HandleBuyGadgetItem(PurchasableItem gadgetItem)
	{
		PurchasableGadgetItem gi = (PurchasableGadgetItem)gadgetItem;
		if (!AllItemData.BuyGadgetItem(gi))
		{
			ShowCantBuyMessage(gadgetItem);
		}
		else if (m_currentMenuState == MainMenuEventManager.MenuState.Store_Menu_Gadgets)
		{
			StoreGUIManagerPersistentElements.The.ShowStoreMenu();
			StoreGUIManagerShop.The.ShowGadgetShop();
		}
		StoreGUIManagerShop.The.DisableAllShopButtons(ref StoreGUIManagerShop.m_gadgetShop, false);
	}

	private void HandleBuyUpgradeItem(PurchasableItem upgradeItem)
	{
		UpgradableItem upgradableItem = (UpgradableItem)upgradeItem;
		if (upgradableItem.m_maxUpgradeTimes > upgradableItem.m_numOwned)
		{
			if (!AllItemData.BuyUpgradeItem(upgradableItem))
			{
				ShowCantBuyMessage(upgradableItem);
			}
			else if (m_currentMenuState == MainMenuEventManager.MenuState.Store_Menu_Upgrades)
			{
				StoreGUIManagerPersistentElements.The.ShowStoreMenu();
				StoreGUIManagerShop.The.ShowUpgradeShop();
			}
			StoreGUIManagerShop.The.DisableAllShopButtons(ref StoreGUIManagerShop.m_upgradeShop, false);
		}
	}

	private void HandleBuyPurchasableItem(PurchasableItem pi)
	{
		if (!AllItemData.BuyPurchasableItem(pi))
		{
			if (!PopUpGUIManager.The.isAPopupActive)
			{
				ShowCantBuyMessage(pi);
			}
		}
		else if (m_currentMenuState == MainMenuEventManager.MenuState.Store_Menu_Upgrades)
		{
			StoreGUIManagerPersistentElements.The.ShowStoreMenu();
			StoreGUIManagerShop.The.ShowUpgradeShop();
		}
		else if (m_currentMenuState == MainMenuEventManager.MenuState.Store_Menu_Gadgets)
		{
			StoreGUIManagerPersistentElements.The.ShowStoreMenu();
			StoreGUIManagerShop.The.ShowGadgetShop();
		}
	}

	private void HandleStartRecording()
	{
		return;
	}

	private void HandleStopRecording()
	{
		return;
	}

	private void HandleTokenHit(int tokenNum)
	{
		HUDGUIManager.The.UpdateTokens(tokenNum);
	}

	private void HandleScoreUpdate(int score)
	{
		HUDGUIManager.The.UpdateScore(score);
	}

	private void HandleCollectScoreMultiplier()
	{
		HUDGUIManager.The.UpdateScoreMultiplier();
	}

	private void HandleScoreMultiplierOff()
	{
		HUDGUIManager.The.UpdateScoreMultiplier();
	}

	private void HandleBossAttackStart()
	{
		HUDGUIManager.The.BossAttackStart();
	}

	private void HandleUpdateBossHealth(MiniGameManager.BossType bossType, float health)
	{
		HUDGUIManager.The.UpdateBossHealth(health);
	}

	private void HandleGoBackToLevel(MiniGameManager.BossType bossType)
	{
		HUDGUIManager.The.EndBossFightGUI();
	}

	private void HandlePlayCurrentCharacter(int index)
	{
		PlayerData.CurrentCharacterIndex = index;
	}

	public void BuyCharacter(PurchasableItem pc)
	{
		if (pc == null)
		{
			Debug.Log("Trying to buy a null character!");
		}
		if (pc.m_tokenCost > PlayerData.playerTokens || pc.m_fedoraCost > PlayerData.playerFedoras)
		{
			ShowCantBuyMessage(pc);
			return;
		}
		PlayerData.playerTokens -= pc.m_tokenCost;
		PlayerData.playerFedoras -= pc.m_fedoraCost;
		AllItemData.BuyCharacterItem(pc);
		if (m_currentMenuState == MainMenuEventManager.MenuState.Main_Menu_Character_Select)
		{
			ShowMainMenu();
		}
		else if (m_currentMenuState == MainMenuEventManager.MenuState.InGame_Score_Menu)
		{
			ShowInGameMenuScores();
		}
		else if (m_currentMenuState == MainMenuEventManager.MenuState.InGame_Missions_Menu)
		{
			ShowInGameMenuMissions();
		}
	}

	private void HandleUseJumpStart()
	{
		GameEventManager.TriggerCopterBoostOn();
		PlayerData.TotalUsedJumpStarts++;
		PlayerData.RoundDidUseJumpStart = true;
		PlayerData.JumpStarts--;
	}

	private void HandleStartMonogramTalking(MongramTalkingHandler.MonogramTalkingState state)
	{
		if (m_currentMenuState == MainMenuEventManager.MenuState.Main_Menu_Missions)
		{
			MonogramGUIManager.The.StartTalking();
		}
		else if (m_currentMenuState == MainMenuEventManager.MenuState.InGame_Missions_Menu)
		{
			MonogramGUIManager.The.StartTalking();
		}
		else if (m_currentMenuState == MainMenuEventManager.MenuState.Game_HUD)
		{
			MonogramGUIManager.The.StartTalking();
		}
		else if (m_currentMenuState == MainMenuEventManager.MenuState.Story)
		{
			MonogramGUIManager.The.StartTalking();
		}
	}

	private void HandleStopMonogramTalking(MongramTalkingHandler.MonogramTalkingState state)
	{
		Debug.Log("GlobalGuiManager HandleStopMonogramTalking " + MongramTalkingHandler.The.m_currentState);
		MonogramGUIManager.The.StopTalking();
		MonogramGUIManager.The.StopTalkBox();
		if (state == MongramTalkingHandler.MonogramTalkingState.OnLaunch)
		{
			CharSelectGUIManager.The.EnableSelectControls();
		}
	}

	public void ChangeLanguage(LocalTextManager.PerryLanguages language)
	{
		LocalTextManager.ChangeLanguage(language);
		SettingsGUIManager.The.HideBackButton(true);
		LocalTextManager.LoadAllLanguageFiles(OnNewFilesLoaded);
	}

	private void HandleShowMeterNotification(int meters)
	{
		if (PlayerData.RoundMeters >= 1 && IsInHUD())
		{
			HUDGUIManager.The.ShowNotification(meters + "m");
		}
	}

	private void HandleShowMissionNotification(Mission m)
	{
		string uIText = LocalTextManager.GetUIText("_MISSION_COMPLETE_");
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			HUDGUIManager.The.ShowNotification(uIText);
		}
		else
		{
			HUDGUIManager.The.ShowNotification(uIText + "\n" + m.m_desc);
		}
	}

	private void OnCantBuyItemPopUpDisplay()
	{
		OnBuyMissionPopUpDisplay();
	}

	private void OnMissionCompletePopUpDisplay()
	{
		OnBuyMissionPopUpDisplay();
	}

	private void OnBuyMissionPopUpDisplay()
	{
		if (IsInMainMenu())
		{
			HideMainMenu();
			ShowMainMenuCameras(true);
			MissionsGUIManager.The.DisableMissionButtons(true);
		}
		else if (IsInInGameMenu())
		{
			HideInGameMenu();
			ShowInGameMenuCamerasSansMask(true);
			MissionsGUIManager.The.DisableMissionButtons(true);
		}
		else if (IsInPauseMenu())
		{
			HidePauseMenu();
			ShowMainMenuCameras(true);
			MissionsGUIManager.The.DisableMissionButtons(true);
		}
		else if (IsInStoreMenu())
		{
			HideStore();
		}
	}

	private void MissionBuyPopUpDoneOK(object missionObj)
	{
		Mission mission = (Mission)missionObj;
		PlayerData.BuyCurrentMission(mission);
		if (IsInMainMenu())
		{
			ShowMainMenuMissions();
		}
		else if (!IsInInGameMenu() && IsInPauseMenu())
		{
			ShowPauseMenu();
		}
		MissionsGUIManager.The.BuyMissionDone(mission, true);
		MainMenuEventManager.TriggerEndAnimation();
		MissionsGUIManager.The.UpdateMissionElements();
	}

	private void MissionBuyPopUpDoneCancel(object missionObj)
	{
		Mission mission = (Mission)missionObj;
		if (IsInMainMenu())
		{
			ShowMainMenuMissions();
		}
		else if (!IsInInGameMenu() && IsInPauseMenu())
		{
			ShowPauseMenu();
		}
		MissionsGUIManager.The.BuyMissionDone(mission, false);
		MainMenuEventManager.TriggerEndAnimation();
		MissionsGUIManager.The.UpdateMissionElements();
	}

	public void MissionCompletePopUpDone(object missionObj)
	{
		Debug.Log("mission complete popup done");
		Mission mission = (Mission)missionObj;
		if (IsInMainMenu())
		{
			ShowMainMenuMissions();
		}
		else if (IsInInGameMenu())
		{
			ContinueScreenGUIManager.The.CheckPlayHavenPlacements();
		}
		else if (IsInPauseMenu())
		{
			ShowPauseMenu();
		}
		MissionsGUIManager.The.CompleteMission(mission, delegate
		{
			if (m_checkingCompleteMissions)
			{
				m_currentMissionIndex++;
				PlayCompleteMissionAnim(m_currentMissionIndex, m_onAllMissionsCompleteAction);
			}
		});
	}

	private void CantBuyItemPopUpOK(object nada)
	{
		ExternalButtonsGUIManager.The.EnableButtons();
		ShopPlayGUIManager.The.DisableButtons(false);
		CharSelectGUIManager.The.EnableCostumeButtons();
		CharSelectGUIManager.The.EnableSelectControls();
		StoreGUIManagerPersistentElements.The.DisableAllButtons(false);
		StoreGUIManagerPersistentElements.The.SelectTokenButton();
	}

	private void CantBuyItemPopUpCancel(object nada)
	{
		ExternalButtonsGUIManager.The.EnableButtons();
		ShopPlayGUIManager.The.DisableButtons(false);
		CharSelectGUIManager.The.EnableCostumeButtons();
		CharSelectGUIManager.The.EnableSelectControls();
		StoreGUIManagerPersistentElements.The.DisableAllButtons(false);
		HandleGoToNextMenu(m_currentMenuState);
	}

	private void BuyTokenPackOfferPopUpOK(object tokenPack)
	{
		ExternalButtonsGUIManager.The.EnableButtons();
		ShopPlayGUIManager.The.DisableButtons(false);
		CharSelectGUIManager.The.EnableCostumeButtons();
		CharSelectGUIManager.The.EnableSelectControls();
		StoreGUIManagerPersistentElements.The.DisableAllButtons(false);
		HandleGoToNextMenu(m_currentMenuState);
		PurchasableItem tokenItem = (PurchasableItem)tokenPack;
		PurchasableItem pItem = (PurchasableItem)tokenItem.m_userData;
		Debug.Log("************* BuyTokenPackOfferPopUpOK DISALBE ALL BUTTONS **************");
		StoreGUIManagerShop.The.DisableAllShopButtons(ref StoreGUIManagerShop.m_getTokensShop, true);
		AllItemData.BuyRealMoneyItem(true, tokenItem, delegate(bool success)
		{
			string[] array = new string[1] { LocalTextManager.GetUIText("_OK_") };
			if (success)
			{
				Debug.Log("BuyTokenItem!");
				StoreGUIManagerShop.The.DisableAllShopButtons(ref StoreGUIManagerShop.m_getTokensShop, false);
				if (pItem != null)
				{
					if (pItem.GetType() == typeof(PurchasableGadgetItem))
					{
						HandleBuyGadgetItem(pItem);
					}
					else if (pItem.GetType() == typeof(UpgradableItem))
					{
						HandleBuyUpgradeItem(pItem);
					}
					else
					{
						HandleBuyPurchasableItem(pItem);
					}
				}
				tokenItem.m_userData = null;
				StoreGUIManagerPersistentElements.The.UpdateMoney();
			}
			else
			{
				Debug.Log("Don't Buy Token Item!");
				StoreGUIManagerShop.The.DisableAllShopButtons(ref StoreGUIManagerShop.m_getTokensShop, false);
				tokenItem.m_userData = null;
				StoreGUIManagerPersistentElements.The.UpdateMoney();
			}
		});
	}

	private void BuyTokenPackOfferPopUpCancel(object tokenPack)
	{
		ExternalButtonsGUIManager.The.EnableButtons();
		ShopPlayGUIManager.The.DisableButtons(false);
		CharSelectGUIManager.The.EnableCostumeButtons();
		CharSelectGUIManager.The.EnableSelectControls();
		StoreGUIManagerPersistentElements.The.DisableAllButtons(false);
		HandleGoToNextMenu(m_currentMenuState);
	}

	private void AlertButtonClicked(string btn)
	{
		Debug.Log("Alert Button Clicked: " + btn);
	}

	private void HandleAgeEnteredEvent(string age)
	{
		Debug.Log("AgeEnteredEvent: " + age);
		if (age == string.Empty)
		{
			ShowAgePrompt();
			return;
		}
		int num = -1;
		try
		{
			num = ((age.Length <= 3) ? Convert.ToInt32(age) : Convert.ToInt32(age.Substring(0, 3)));
		}
		catch
		{
		}
		if (num == -1)
		{
			ShowAgePrompt();
			return;
		}
		GameManager.The.playerIsUnderThirteen = num < 13;
		GameManager.The.hasPromptedPlayerforBirthday = true;
		Debug.Log("after turning age into a number: " + num.ToString() + " " + GameManager.The.playerIsUnderThirteen);
		ShowMainMenu();
	}

	public void HandleAgeGateCanceled()
	{
		GameManager.The.hasPromptedPlayerforBirthday = false;
		Application.Quit();
	}

	public void HandleAgeDateEnteredEvent(string age)
	{
		Debug.Log("Age Entered: " + age);
		int num = 0;
		try
		{
			num = Convert.ToInt32(age);
		}
		catch
		{
		}
		GameManager.The.playerIsUnderThirteen = num < 13;
		GameManager.The.hasPromptedPlayerforBirthday = true;
		AgeGateAndroid.ShowAlert(LocalTextManager.GetUIText("_NOTICE_"), LocalTextManager.GetUIText("_IAP_NOTICE_"), LocalTextManager.GetUIText("_OK_"));
	}

	public void HandleAlertButtonClicked()
	{
		Invoke("ShowMainMenu", 1f);
	}

	private void ShowAgePrompt()
	{
		AgeGateAndroid.ShowDatePicker(LocalTextManager.GetUIText("_AGE_VERIFICATION_"), LocalTextManager.GetStoreItemText("_COPPA_"), LocalTextManager.GetUIText("_CANCEL_"));
	}

	public void ResizeUIElementToProperAspectRatio(ref UIProgressBar sprite)
	{
		if (Screen.height < 2048)
		{
			float num = (float)Screen.width / 1536f * (float)(4 / UI.scaleFactor);
			sprite.scale = Vector3.one * num;
		}
	}

	public void ResizeUIElementToProperAspectRatio(ref UISprite sprite)
	{
		if (Screen.height < 2048)
		{
			float num = (float)Screen.width / 1536f * (float)(4 / UI.scaleFactor);
			sprite.scale = Vector3.one * num;
		}
	}

	public void ResizeUIElementToProperAspectRatio(ref UIButton sprite)
	{
		if (Screen.height < 2048)
		{
			float num = (float)Screen.width / 1536f * (float)(4 / UI.scaleFactor);
			sprite.scale = Vector3.one * num;
		}
	}

	public void ResizeUIElementToProperAspectRatio(ref UITouchableSprite sprite)
	{
		if (Screen.height < 2048)
		{
			float num = (float)Screen.width / 1536f * (float)(4 / UI.scaleFactor);
			sprite.scale = Vector3.one * num;
		}
	}

	public void ResizeUIElementToProperAspectRatio(ref UIToggleButton sprite)
	{
		if (Screen.height < 2048)
		{
			float num = (float)Screen.width / 1536f * (float)(4 / UI.scaleFactor);
			sprite.scale = Vector3.one * num;
		}
	}

	private void ShowCantBuyMessage(PurchasableItem item)
	{
		ExternalButtonsGUIManager.The.DisableButtons();
		ShopPlayGUIManager.The.DisableButtons(true);
		CharSelectGUIManager.The.DisableCostumeButtons();
		CharSelectGUIManager.The.DisableSelectControls();
		StoreGUIManagerPersistentElements.The.DisableAllButtons(true);
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			PopUpGUIManager.The.ShowNeedMoreTokensPopUp(item.m_name, item.m_tokenCost, item.m_fedoraCost, CantBuyItemPopUpOK, CantBuyItemPopUpCancel, delegate
			{
				OnCantBuyItemPopUpDisplay();
			});
			return;
		}
		PurchasableItem tokenItem = ((item.m_tokenCost <= 0) ? AllItemData.GetTokenItemClosestToFedoraPrice(item.m_fedoraCost) : AllItemData.GetTokenItemClosestToPrice(item.m_tokenCost));
		PopUpGUIManager.The.ShowOfferToBuyMoreTokensAndBuyItem(ref item, ref tokenItem, BuyTokenPackOfferPopUpOK, BuyTokenPackOfferPopUpCancel, delegate
		{
			OnCantBuyItemPopUpDisplay();
		});
	}

	private void ReloadUITextManager(ref UIText TextManager, string FNTfile, string PNGfile)
	{
		string text = ((!UI.isHD) ? string.Empty : UI.instance.hdExtension);
		if (!(TextManager.manager.texturePackerConfigName == FNTfile + "Tex" + text))
		{
			TextManager.manager.texturePackerConfigName = FNTfile + "Tex";
			TextManager.manager.loadTextureAndPrepareForUse();
			List<UITextInstance> textInstances = TextManager.m_TextInstances;
			UIToolkit manager = TextManager.manager;
			TextManager.ReConfigureUIText(manager, FNTfile, PNGfile, textInstances);
			TextManager.updateAllTextInstances();
		}
	}

	public void LoadFontTexturesForCurrentLanguage()
	{
		LocalTextManager.PerryLanguages currentLanguageType = LocalTextManager.CurrentLanguageType;
		switch (currentLanguageType)
		{
		case LocalTextManager.PerryLanguages.Korean:
		case LocalTextManager.PerryLanguages.Japanese:
		case LocalTextManager.PerryLanguages.Chinese:
		case LocalTextManager.PerryLanguages.Russian:
			ReloadUITextManager(ref m_defaultText, "snyder" + currentLanguageType, "snyder" + currentLanguageType.ToString() + ".png");
			ReloadUITextManager(ref m_defaultTextAlt, "futura" + currentLanguageType, "futura" + currentLanguageType.ToString() + ".png");
			break;
		default:
			ReloadUITextManager(ref m_defaultText, "snyder", "snyder.png");
			ReloadUITextManager(ref m_defaultTextAlt, "futura", "futura.png");
			break;
		}
	}

	private void OnNewFilesLoaded()
	{
		SettingsGUIManager.The.HideBackButton(false);
		LoadFontTexturesForCurrentLanguage();
		ReloadAllUIText();
		ShowSettingsMenu();
	}

	public void ReloadAllUIText()
	{
		CharSelectGUIManager.The.ReloadStaticText();
		ContinueScreenGUIManager.The.ReloadStaticText();
		HighScoreGUIManager.The.ReloadStaticText();
		InGameMenuGUIManager.The.ReloadStaticText();
		MissionsGUIManager.The.ReloadStaticText();
		PauseGUIManager.The.ReloadStaticText();
		PopUpGUIManager.The.ReloadStaticText();
		SettingsGUIManager.The.ReloadStaticText();
		StoreGUIManagerPersistentElements.The.ReloadStaticText();
		StoreGUIManagerShop.The.ReloadStaticText();
		ShopPlayGUIManager.The.ReloadStaticText();
		MainMenuGUIManager.The.ReloadStaticText();
	}

	public static bool IsInitialized()
	{
		return m_isInit;
	}
}

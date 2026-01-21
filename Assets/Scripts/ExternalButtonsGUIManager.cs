using UnityEngine;

public class ExternalButtonsGUIManager : MonoBehaviour
{
	private enum ThirdPartyUtilityActive
	{
		none = 0,
		AppleGameCenter = 1,
		MoreGames = 3
	}

	private static UIButton m_menuSettingsBtn;

	private static UIVerticalLayout m_externalButtonsVerticalLayout;

	private static UIButton m_moreGamesBtn;

	private static UITextInstance m_moreGamesLabel;

	private static UIButton m_cameraButton;

	private ThirdPartyUtilityActive m_activeThirdPartyUtility;

	private static UIButton m_homeButtonInGame;

	private int m_mainMenuButtonsDepthStart = 4;

	private int m_pauseMenuButtonsDepthStart = 4;

	private static ExternalButtonsGUIManager m_the;

	public static UIButton HomeButtonInGame
	{
		get
		{
			return m_homeButtonInGame;
		}
	}

	public static ExternalButtonsGUIManager The
	{
		get
		{
			if (m_the == null)
			{
				GameObject gameObject = new GameObject("ExternalButtonsGUIManager");
				((ExternalButtonsGUIManager)gameObject.AddComponent<ExternalButtonsGUIManager>()).Init();
			}
			return m_the;
		}
	}

	private void Awake()
	{
		MainMenuEventManager.StopRecording += HandleStopRecording;
	}

	public void Init()
	{
		if (!(m_the != null))
		{
			m_the = this;
			Object.DontDestroyOnLoad(base.gameObject);
			InitExternalButtonsMenu();
			InitExternalButtonsInGame();
			HideAll();
		}
	}

	private void InitExternalButtonsMenu()
	{
		m_externalButtonsVerticalLayout = new UIVerticalLayout(0);
		m_menuSettingsBtn = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "SettingsButton.png", "SettingsButtonOver.png", 0, 0, m_mainMenuButtonsDepthStart);
		m_menuSettingsBtn.highlightedTouchOffsets = new UIEdgeOffsets(30);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_menuSettingsBtn);
		m_menuSettingsBtn.onTouchUpInside += onTouchUpInsideSettingsButton;
		m_externalButtonsVerticalLayout.addChild(m_menuSettingsBtn);
		bool flag = GameManager.IsiPhone4thGeneration();
	}

	private void InitExternalButtonsInGame()
	{
		m_homeButtonInGame = UIButton.create(GlobalGUIManager.The.m_hudToolkit, "HomeButton.png", "HomeButtonOver.png", 0, 0, m_pauseMenuButtonsDepthStart);
		m_homeButtonInGame.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_homeButtonInGame.pixelsFromTopLeft(1, 1);
		m_homeButtonInGame.scale = new Vector3(0.8f, 0.8f, 1f);
		m_homeButtonInGame.onTouchUpInside += onTouchUpInsideHomeButton;
	}

	public void HideAll()
	{
		HideMainMenuButtons();
		HideInGameMenuButtons();
	}

	public void HideMainMenuButtons()
	{
		m_externalButtonsVerticalLayout.hidden = true;
		m_menuSettingsBtn.hidden = true;
		if (m_cameraButton != null)
		{
			m_cameraButton.hidden = true;
		}
	}

	public void HideInGameMenuButtons()
	{
		m_homeButtonInGame.hidden = true;
	}

	public void DisableButtons()
	{
		MainMenuGUIManager.The.DisableAllButtons(true);
		m_homeButtonInGame.disabled = true;
		m_menuSettingsBtn.disabled = true;
		if (m_cameraButton != null)
		{
			m_cameraButton.disabled = true;
		}
	}

	public void EnableButtons()
	{
		MainMenuGUIManager.The.DisableAllButtons(false);
		m_homeButtonInGame.disabled = false;
		m_menuSettingsBtn.disabled = false;
		if (m_cameraButton != null)
		{
			m_cameraButton.disabled = false;
		}
	}

	public void ShowMainMenuButtons(UIObject parentObj = null)
	{
		float percentFromTop = 0.1f;
		float percentFromRight = 0.01f;
		UIObject parentUIObject = null;
		if (parentObj != null)
		{
			parentUIObject = m_externalButtonsVerticalLayout.parentUIObject;
			m_externalButtonsVerticalLayout.parentUIObject = parentObj;
		}
		m_externalButtonsVerticalLayout.alignMode = UIAbstractContainer.UIContainerAlignMode.Right;
		m_externalButtonsVerticalLayout.positionFromTopRight(percentFromTop, percentFromRight);
		m_externalButtonsVerticalLayout.hidden = false;
		m_menuSettingsBtn.hidden = false;
		if (m_cameraButton != null)
		{
			m_cameraButton.hidden = false;
		}
		if (parentObj != null)
		{
			m_externalButtonsVerticalLayout.parentUIObject = parentUIObject;
		}
	}

	public void ShowInGameMenuButtons()
	{
		m_homeButtonInGame.hidden = false;
	}

	private void onTouchUpInsideSettingsButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		Debug.Log("Settings Menu");
		sender.disabled = true;
		MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Settings_Menu);
		FlurryFacade.Instance.LogEvent("SettingsClicked");
		sender.disabled = false;
	}

	public void onTouchUpInsideGameCenterButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			DisableButtons();
			PopUpGUIManager.The.ShowNoInternetNotificationPopup(OnNoInternetPopUpDismissed, delegate
			{
			});
		}
		else
		{
			if (m_activeThirdPartyUtility != ThirdPartyUtilityActive.none)
			{
				return;
			}
			DisableButtons();
			m_activeThirdPartyUtility = ThirdPartyUtilityActive.AppleGameCenter;
			if (PerryGameServices.The != null)
			{
				Debug.Log("PerryGameServices.ShowAchievements");
				if (!PerryGameServices.IsSignedIn())
				{
					PerryGameServices.AuthenticateAndShowAchievements();
				}
				else
				{
					PerryGameServices.ShowAchievements();
				}
			}
			FlurryFacade.Instance.LogEvent("LeaderboardClicked");
		}
	}

	public void onTouchUpInsideMoreGamesButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			PopUpGUIManager.The.ShowNoInternetNotificationPopup(OnNoInternetPopUpDismissed, delegate
			{
			});
			DisableButtons();
		}
		else if (m_activeThirdPartyUtility == ThirdPartyUtilityActive.none)
		{
			DisableButtons();
			m_activeThirdPartyUtility = ThirdPartyUtilityActive.MoreGames;
			GameManager.The.PauseMusic();
		}
	}

	public void onTouchUpInsideCameraButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		if (m_activeThirdPartyUtility == ThirdPartyUtilityActive.none)
		{
			return;
		}
	}

	public void onTouchUpInsideRestartButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		sender.disabled = true;
		Debug.Log("Restart Game Button");
		sender.disabled = false;
	}

	public void onTouchUpInsideHomeButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		sender.disabled = true;
		Debug.Log("HomeButton");
		MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Main_Menu_Character_Select);
		sender.disabled = false;
	}

	private void OnNoInternetPopUpDismissed(object o)
	{
		EnableButtons();
	}

	private void OnApplicationPause(bool isPaused)
	{
		if (!isPaused && (m_activeThirdPartyUtility == ThirdPartyUtilityActive.AppleGameCenter))
		{
			m_activeThirdPartyUtility = ThirdPartyUtilityActive.none;
			EnableButtons();
		}
	}

	private void HandleStopRecording()
	{

	}

}

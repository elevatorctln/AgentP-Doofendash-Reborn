using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MainMenuGUIManager : MonoBehaviour
{
	private static UIButton m_missionButton;

	public static UIButton m_backButton;

	public static UIButton m_freeTokenStoreButton;

	public static UITextInstance m_freeTokenStoreText;

	public static UIButton m_bee7Button;

	public static UISprite m_bee7Image;

	public static UITextInstance m_bee7Text;

	public static bool m_alreadyInMainMenu;

	private int m_mainMenuDepth = 4;

	private static MainMenuGUIManager m_the;

	public static MainMenuGUIManager The
	{
		get
		{
			if (m_the == null)
			{
				GameObject gameObject = new GameObject("MainMenuGUIManager");
				((MainMenuGUIManager)gameObject.AddComponent<MainMenuGUIManager>()).Init();
			}
			return m_the;
		}
	}

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
		
	}

	public void Init()
	{
		if (!(m_the != null))
		{
			m_the = this;
			Object.DontDestroyOnLoad(base.gameObject);
			InitMainMenuGUI();
		}
	}

	private void InitMainMenuGUI()
	{
		Debug.Log("Started init main menu");
		m_missionButton = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "MissionsButton.png", "MissionsButtonOver.png", 0, 0, m_mainMenuDepth + 3);
		m_missionButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_missionButton);
		m_missionButton.positionFromTopLeft(0.02f, 0.01f);
		m_missionButton.onTouchUpInside += onTouchUpInsideMissionButton;
		m_backButton = UIButton.create(GlobalGUIManager.The.m_hudToolkit, "BackButton.png", "BackButtonOver.png", 0, 0, m_mainMenuDepth);
		m_backButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_backButton.pixelsFromTopLeft(5, 5);
		m_backButton.onTouchUpInside += onTouchUpInsideBackButton;
	}

	public void HideAll()
	{
		m_alreadyInMainMenu = false;
		HideMissionsButton();
		HideMainMenuMissions();
	}

	public void HideMissionsButton()
	{
		m_missionButton.hidden = true;
	}

	public void HideMainMenuMissions()
	{
		m_backButton.hidden = true;
	}

	public void DisableAllButtons(bool disable)
	{
		m_missionButton.disabled = disable;
		m_backButton.disabled = disable;
	}

	public void DisableMissionButton(bool disable)
	{
		m_missionButton.disabled = disable;
	}

	public void ShowMainMenuCharSelectMode()
	{
		HideMainMenuMissions();
		m_missionButton.hidden = false;
		m_alreadyInMainMenu = true;
	}

	public void ShowmainMenuMissionsMode()
	{
		HideMissionsButton();
		m_backButton.hidden = false;
	}

	public void ShowGetFreeTokensStoreButton(bool show)
	{

	}

	public void onTouchUpInsideMissionButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		sender.disabled = true;
		MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Main_Menu_Missions);
		sender.disabled = false;
		m_alreadyInMainMenu = false;
	}

	public void onTouchUpInsideBackButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		sender.disabled = true;
		MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Main_Menu_Character_Select);
		sender.disabled = false;
		m_alreadyInMainMenu = false;
	}

	public void onTouchUpInsideTrialPayButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Store_Menu_FreeTokens);
		m_alreadyInMainMenu = false;
	}

	public void onTouchUpInsideBee7Button(UIButton sender)
	{
		
	}

	public void ReloadStaticText()
	{
		
	}
}

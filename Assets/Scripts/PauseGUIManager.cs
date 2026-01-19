using UnityEngine;

public class PauseGUIManager : MonoBehaviour
{
	private UITextInstance m_pauseTitle;

	private UIButton m_homeButton;

	private int m_pauseScreenDepthIndex = 1;

	private static PauseGUIManager m_the;

	public bool m_isOn;

	public UITextInstance PauseTitle
	{
		get
		{
			return m_pauseTitle;
		}
	}

	public static PauseGUIManager The
	{
		get
		{
			if (m_the == null)
			{
				GameObject gameObject = new GameObject("PauseGUIManager");
				((PauseGUIManager)gameObject.AddComponent<PauseGUIManager>()).Init();
			}
			return m_the;
		}
	}

	public void Init()
	{
		if (!(m_the != null))
		{
			m_the = this;
			Object.DontDestroyOnLoad(base.gameObject);
			InitPauseScreenElements();
			HideAll();
		}
	}

	private void InitPauseScreenElements()
	{
		Color colorForAllLetters = new Color32(byte.MaxValue, 137, 0, byte.MaxValue);
		m_pauseTitle = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_PAUSE_TITLE_"), 0f, 0f, 1f, m_pauseScreenDepthIndex);
		m_pauseTitle.alignMode = UITextAlignMode.Center;
		m_pauseTitle.textScale = 1.3f;
		m_pauseTitle.positionFromTop(0.1f);
		m_pauseTitle.setColorForAllLetters(colorForAllLetters);
		m_pauseTitle.text = LocalTextManager.GetUIText("_PAUSE_TITLE_");
		m_homeButton = UIButton.create(GlobalGUIManager.The.m_hudToolkit, "HomeButton.png", "HomeButtonOver.png", 0, 0, m_pauseScreenDepthIndex);
		m_homeButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_homeButton.positionFromTopLeft(0.01f, 0.01f);
		m_homeButton.onTouchUpInside += onTouchUpInsideHomeButton;
	}

	public void HideAll()
	{
		StoreGUIManagerPersistentElements.The.HideMoneySprites();
		HideTitle();
		HideUnPauseButton();
	}

	public void HideTitle()
	{
		m_pauseTitle.hidden = true;
		m_isOn = false;
	}

	public void HideUnPauseButton()
	{
		m_homeButton.hidden = true;
	}

	public void ShowPauseMenu()
	{
		StoreGUIManagerPersistentElements.The.ShowMoney();
		m_pauseTitle.hidden = false;
		m_homeButton.hidden = false;
		m_isOn = true;
	}

	public void FakePressHomeButton()
	{
		onTouchUpInsideHomeButton(m_homeButton);
	}

	public void onTouchUpInsideHomeButton(UIButton sender)
	{
		MissionsGUIManager.The.DisableMissionButtons();
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		sender.disabled = true;
		ShopPlayGUIManager.The.DisableButtons(true);
		PopUpGUIManager.The.ShowGameExitConfirmationPrompt(OnExitPromptCancelled, OnExitPromptConfirmed, delegate
		{
		});
	}

	private void OnExitPromptCancelled(object o)
	{
		MissionsGUIManager.The.EnableMissionButtons();
		m_homeButton.disabled = false;
		ShopPlayGUIManager.The.DisableButtons(false);
	}

	private void OnExitPromptConfirmed(object o)
	{
		MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Main_Menu_Character_Select);
		m_homeButton.disabled = false;
		ShopPlayGUIManager.The.DisableButtons(false);
	}

	public void ReloadStaticText()
	{
		m_pauseTitle.text = LocalTextManager.GetUIText("_PAUSE_TITLE_");
	}
}

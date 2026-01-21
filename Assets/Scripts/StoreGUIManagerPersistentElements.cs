using UnityEngine;

public class StoreGUIManagerPersistentElements : MonoBehaviour
{
	private static UIToggleButton m_getTokensBtn;

	private static UIToggleButton m_gadgetsBtn;

	private static UIToggleButton m_upgradesBtn;

	private static UIToggleButton m_freeBtn;

	private static UITextInstance m_getTokensLabel;

	private static UITextInstance m_gadgetsLabel;

	private static UITextInstance m_upgradesLabel;

	private static UITextInstance m_freeLabel;

	private static UIButton m_backButton;

	private static UIButton m_videoAdButton;

	private static UITextInstance m_videoAdLabel;

	private static UIButton m_moreGamesBtn;

	private static UITextInstance m_moreGamesLabel;

	private static UITextInstance m_tokenText;

	private static UITextInstance m_fedoraText;

	private static UIButton m_tokenFrame;

	private static UIButton m_fedoraFrame;

	private static int m_startStoreDepth;

	private static bool m_disableBottomButtons;

	private static StoreGUIManagerPersistentElements m_the;

	public static StoreGUIManagerPersistentElements The
	{
		get
		{
			if (m_the == null)
			{
				GameObject gameObject = new GameObject("StoreGUIManagerPersistentElements");
				((StoreGUIManagerPersistentElements)gameObject.AddComponent<StoreGUIManagerPersistentElements>()).Init();
			}
			return m_the;
		}
	}

	public void Awake()
	{
	}

	public void Init()
	{
		if (!(m_the != null))
		{
			m_the = this;
			Object.DontDestroyOnLoad(base.gameObject);
			InitBottomFrameButtons();
			InitBackButton();
			InitVideoAdButton();
			InitMoneySprites();
			HideAll();
		}
	}

	private void InitBottomFrameButtons()
	{
		m_getTokensBtn = UIToggleButton.create(GlobalGUIManager.The.m_menuToolkit, "StoreTokenButton.png", "StoreTokenButtonOn.png", "StoreTokenButtonOver.png", 0, 0, m_startStoreDepth + 2);
		m_getTokensBtn.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_getTokensBtn.positionFromBottomLeft(0.0265f, 0.05f);
		m_getTokensBtn.onToggle += onToggleGetTokenButton;
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_getTokensBtn);
		m_gadgetsBtn = UIToggleButton.create(GlobalGUIManager.The.m_menuToolkit, "StoreGadgetsButton.png", "StoreGadgetsButtonOn.png", "StoreGadgetsButtonOver.png", 0, 0, m_startStoreDepth + 2);
		m_gadgetsBtn.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_gadgetsBtn.positionFromBottomLeft(0.0265f, 0.28f);
		m_gadgetsBtn.onToggle += onToggleGadgetButton;
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_gadgetsBtn);
		m_upgradesBtn = UIToggleButton.create(GlobalGUIManager.The.m_menuToolkit, "StoreUpgradesButton.png", "StoreUpgradesButtonOn.png", "StoreUpgradesButtonOver.png", 0, 0, m_startStoreDepth + 2);
		m_upgradesBtn.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_upgradesBtn.positionFromBottomRight(0.0265f, 0.28f);
		m_upgradesBtn.onToggle += onToggleUpgradesButton;
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_upgradesBtn);
		m_freeBtn = UIToggleButton.create(GlobalGUIManager.The.m_menuToolkit, "PlayButtonOver.png", "PlayButtonOver.png", "PlayButtonOver.png", 0, 0, m_startStoreDepth + 2);
		m_freeBtn.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_freeBtn.onToggle += onToggleFreeButton;
		m_freeBtn.setSize(m_upgradesBtn.width, m_upgradesBtn.height);
		m_freeBtn.positionFromBottomRight(0.0265f, 0.05f);
		float num = 1f;
		num = ((GameManager.The.aspectRatio.x == 3f || GameManager.The.aspectRatio.y == 4f) ? 0.43f : ((LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.Russian) ? 0.3f : 0.2f));
		if (GameManager.The.aspectRatio.x == 3f && GameManager.The.aspectRatio.y == 4f && LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Russian)
		{
			num = 0.35f;
		}
		if (UI.scaleFactor == 1)
		{
			num /= 2f;
		}
		m_getTokensLabel = GlobalGUIManager.The.defaultText.addTextInstance(LocalTextManager.GetUIText("_GET_TOKENS_BTN_LABEL_"), 0f, 0f, 2f, m_startStoreDepth - 8);
		m_getTokensLabel.alignMode = UITextAlignMode.Center;
		m_getTokensLabel.verticalAlignMode = UITextVerticalAlignMode.Bottom;
		m_getTokensLabel.setColorForAllLetters(Color.black);
		m_getTokensLabel.parentUIObject = m_getTokensBtn;
		m_getTokensLabel.positionFromCenter(0.6f, 0f);
		m_getTokensLabel.text = LocalTextManager.GetUIText("_GET_TOKENS_BTN_LABEL_");
		m_gadgetsLabel = GlobalGUIManager.The.defaultText.addTextInstance(LocalTextManager.GetUIText("_GADGETS_BTN_LABEL_"), 0f, 0f, 2f, m_startStoreDepth - 8);
		m_gadgetsLabel.alignMode = UITextAlignMode.Center;
		m_gadgetsLabel.verticalAlignMode = UITextVerticalAlignMode.Bottom;
		m_gadgetsLabel.setColorForAllLetters(Color.black);
		m_gadgetsLabel.parentUIObject = m_gadgetsBtn;
		m_gadgetsLabel.positionFromCenter(0.6f, 0f);
		m_gadgetsLabel.position = new Vector3(m_gadgetsLabel.position.x - 2f, m_gadgetsLabel.position.y, m_gadgetsLabel.position.z);
		m_gadgetsLabel.text = LocalTextManager.GetUIText("_GADGETS_BTN_LABEL_");
		m_upgradesLabel = GlobalGUIManager.The.defaultText.addTextInstance(LocalTextManager.GetUIText("_UPGRADES_BTN_LABEL_"), 0f, 0f, 2f, m_startStoreDepth - 8);
		m_upgradesLabel.alignMode = UITextAlignMode.Center;
		m_upgradesLabel.verticalAlignMode = UITextVerticalAlignMode.Bottom;
		m_upgradesLabel.setColorForAllLetters(Color.black);
		m_upgradesLabel.parentUIObject = m_upgradesBtn;
		m_upgradesLabel.positionFromCenter(0.6f, 0f);
		m_upgradesLabel.text = LocalTextManager.GetUIText("_UPGRADES_BTN_LABEL_");
		m_freeLabel = GlobalGUIManager.The.defaultText.addTextInstance(LocalTextManager.GetUIText("FREE_BTN_LABELZOR"), 0f, 0f, 4f, m_startStoreDepth - 8);
		m_freeLabel.alignMode = UITextAlignMode.Center;
		m_freeLabel.verticalAlignMode = UITextVerticalAlignMode.Bottom;
		m_freeLabel.setColorForAllLetters(Color.black);
		m_freeLabel.parentUIObject = m_freeBtn;
		m_freeLabel.positionFromCenter(-0.05f, 0f);
		m_freeLabel.text = LocalTextManager.GetUIText("FREE_BTN_LABELZOR");
		m_freeLabel.textScale = 0.6f;
		m_upgradesLabel.textScale = num * UIHelper.CalcFontScale();
		m_gadgetsLabel.textScale = num * UIHelper.CalcFontScale();
		m_getTokensLabel.textScale = num * UIHelper.CalcFontScale();
		m_freeLabel.textScale = num * UIHelper.CalcFontScale();
	}

	private void InitBackButton()
	{
		m_backButton = UIButton.create(GlobalGUIManager.The.m_hudToolkit, "BackButton.png", "BackButtonOver.png", 0, 0, m_startStoreDepth);
		m_backButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_backButton.positionFromTopLeft(0.005f, 0.01f);
		m_backButton.onTouchUpInside += onTouchUpInsideBackButton;
	}

	private void InitVideoAdButton()
	{
		m_videoAdButton = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "PlayButton.png", "PlayButton.png", 0, 0, m_startStoreDepth + 1);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_videoAdButton);
		m_videoAdButton.scale = new Vector3(0.9f, 0.9f, 1f);
		m_videoAdButton.positionFromTopLeft(0.81f, 0.32f);
		m_videoAdButton.onTouchUpInside += onTouchUpInsideVideoAdButton;
		m_videoAdLabel = GlobalGUIManager.The.defaultText.addTextInstance(LocalTextManager.GetUIText("_VIDEO_AD_"), 0f, 0f, 0.6f, m_startStoreDepth);
		m_videoAdLabel.alignMode = UITextAlignMode.Center;
		m_videoAdLabel.verticalAlignMode = UITextVerticalAlignMode.Top;
		m_videoAdLabel.setColorForAllLetters(Color.black);
		m_videoAdLabel.parentUIObject = m_videoAdButton;
		m_videoAdLabel.positionFromCenter(-0.05f, 0f);
		m_videoAdLabel.text = LocalTextManager.GetUIText("_VIDEO_AD_");
		m_videoAdLabel.textScale = 0.4f;
		// Video ad button always disabled
		// Original code had a broken condition that always hid the button, works for me.
		m_videoAdButton.hidden = true;
		m_videoAdLabel.hidden = true;
	}

	private void InitMoneySprites()
	{
		m_tokenFrame = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "BuyTokenButton.png", "BuyTokenButtonOver.png", 0, 0, m_startStoreDepth + 1);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_tokenFrame);
		m_tokenFrame.positionFromTopRight(0.01f, 0f);
		m_tokenFrame.onTouchUpInside += onTouchUpInsideTopTokenandFedoraButton;
		m_fedoraFrame = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "BuyFedoraButton.png", "BuyFedoraButtonOver.png", 0, 0, m_startStoreDepth + 1);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_fedoraFrame);
		m_fedoraFrame.positionFromTopRight(0.01f, 0.4f);
		m_fedoraFrame.onTouchUpInside += onTouchUpInsideTopTokenandFedoraButton;
		m_tokenText = GlobalGUIManager.The.defaultText.addTextInstance("999,999,999", 0f, 0f, 1f, m_startStoreDepth - 8);
		m_tokenText.parentUIObject = m_tokenFrame;
		m_tokenText.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_tokenText.alignMode = UITextAlignMode.Right;
		m_tokenText.positionFromRight(0.22f);
		if (GameManager.The.aspectRatio.x == 3f && GameManager.The.aspectRatio.y == 4f)
		{
			m_tokenText.textScale = 0.35f * UIHelper.CalcFontScale();
		}
		else
		{
			m_tokenText.textScale = 0.22f * UIHelper.CalcFontScale();
		}
		m_fedoraText = GlobalGUIManager.The.defaultText.addTextInstance("999", 0f, 0f, 1f, m_startStoreDepth - 8);
		m_fedoraText.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_fedoraText.alignMode = UITextAlignMode.Right;
		m_fedoraText.parentUIObject = m_fedoraFrame;
		m_fedoraText.positionFromRight(0.35f);
		if (GameManager.The.aspectRatio.x == 3f && GameManager.The.aspectRatio.y == 4f)
		{
			m_fedoraText.textScale = 0.35f * UIHelper.CalcFontScale();
		}
		else
		{
			m_fedoraText.textScale = 0.22f * UIHelper.CalcFontScale();
		}
	}

	public void HideAll()
	{
		HideBottomButtons();
		HideBackButton();
		HideVideoAdButton();
		HideMoneySprites();
	}

	public void HideBottomButtons()
	{
		m_getTokensBtn.hidden = true;
		m_gadgetsBtn.hidden = true;
		m_upgradesBtn.hidden = true;
		m_freeBtn.hidden = true;
		m_getTokensLabel.hidden = true;
		m_gadgetsLabel.hidden = true;
		m_upgradesLabel.hidden = true;
		m_freeLabel.hidden = true;
	}

	public void HideBackButton()
	{
		m_backButton.hidden = true;
	}

	public void HideVideoAdButton()
	{
		m_videoAdButton.hidden = true;
		m_videoAdLabel.hidden = true;
	}

	public void HideMoneySprites()
	{
		m_tokenText.hidden = true;
		m_fedoraText.hidden = true;
		m_tokenFrame.hidden = true;
		m_fedoraFrame.hidden = true;
	}

	public void DisableAllButtons(bool bDisable)
	{
		DisableBottomButtons(bDisable);
		DisableMoneyButtons(bDisable);
	}

	public void DisableBottomButtons(bool bDisable)
	{
		m_disableBottomButtons = bDisable;
		m_getTokensBtn.disabled = bDisable;
		m_gadgetsBtn.disabled = bDisable;
		m_upgradesBtn.disabled = bDisable;
		m_freeBtn.disabled = bDisable;
	}

	public void DisableMoneyButtons(bool bDisable)
	{
		m_tokenFrame.disabled = bDisable;
		m_fedoraFrame.disabled = bDisable;
	}

	public void SelectActiveStoreButton()
	{
		DeselectBottomButtons();
		switch (StoreGUIManagerShop.The.m_currentMenuState)
		{
		case StoreGUIManagerShop.SHOP_MENU_STATE.GADGETS:
			m_gadgetsBtn.selected = true;
			m_gadgetsBtn.disabled = true;
			break;
		case StoreGUIManagerShop.SHOP_MENU_STATE.GET_TOKENS:
			m_getTokensBtn.selected = true;
			m_getTokensBtn.disabled = true;
			break;
		case StoreGUIManagerShop.SHOP_MENU_STATE.UPGRADES:
			m_upgradesBtn.selected = true;
			m_upgradesBtn.disabled = true;
			break;
		default:
			m_gadgetsBtn.selected = false;
			m_getTokensBtn.selected = false;
			m_upgradesBtn.selected = false;
			break;
		}
	}

	public void DeselectBottomButtons()
	{
		m_getTokensBtn.selected = false;
		m_gadgetsBtn.selected = false;
		m_upgradesBtn.selected = false;
	}

	public void ShowMainMenu()
	{
		HideAll();
		ShowMoney();
	}

	public void ShowInGameMenu()
	{
		HideAll();
		ShowMoney();
	}

	public void ShowPauseMenu()
	{
		ShowBottomButtonFrames();
		ShowMoney();
		DisableBottomButtons(false);
		DeselectBottomButtons();
	}

	public void ShowMissionsMainMenu()
	{
		ShowMoney();
	}

	public void ShowStoreMenu()
	{
		ShowBottomButtonFrames();
		ShowBackButton();
		ShowMoney();
	}

	public void ShowFreeTokensStoreMenu()
	{
		ShowBottomButtonFrames();
		ShowBackButton();
		ShowMoney();
	}

	private void ShowBottomButtonFrames()
	{
		m_getTokensBtn.hidden = false;
		m_gadgetsBtn.hidden = false;
		m_upgradesBtn.hidden = false;
		m_freeBtn.hidden = false;
		m_getTokensLabel.hidden = false;
		m_gadgetsLabel.hidden = false;
		m_upgradesLabel.hidden = false;
		m_freeLabel.hidden = false;
	}

	private void ShowBackButton()
	{
		m_backButton.hidden = false;
	}

	private void ShowVideoAdButton()
	{
		m_videoAdButton.hidden = false;
		m_videoAdLabel.hidden = false;
	}

	public void ShowMoney()
	{
		UpdateMoney();
		DisableMoneyButtons(false);
		m_tokenText.hidden = false;
		m_fedoraText.hidden = false;
		m_tokenFrame.hidden = false;
		m_fedoraFrame.hidden = false;
	}

	public void SelectTokenButton()
	{
		m_getTokensBtn.selected = true;
		onToggleGetTokenButton(m_getTokensBtn, true);
	}

	public void UpdateMoney()
	{
		m_tokenText.text = PlayerData.playerTokens.ToString("N0");
		m_fedoraText.text = PlayerData.playerFedoras.ToString("N0");
	}

	public void onToggleGetTokenButton(UIToggleButton sender, bool selected)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		if (!m_disableBottomButtons && !PopUpGUIManager.The.isAPopupActive && selected)
		{
			sender.disabled = true;
			MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Store_Menu_Tokens);
			m_gadgetsBtn.disabled = false;
			m_upgradesBtn.disabled = false;
		}
	}

	public void onToggleGadgetButton(UIToggleButton sender, bool selected)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		if (!m_disableBottomButtons && !PopUpGUIManager.The.isAPopupActive && selected)
		{
			m_gadgetsBtn.disabled = true;
			MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Store_Menu_Gadgets);
			m_getTokensBtn.disabled = false;
			m_upgradesBtn.disabled = false;
		}
	}

	public void onToggleUpgradesButton(UIToggleButton sender, bool selected)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		if (!m_disableBottomButtons && !PopUpGUIManager.The.isAPopupActive && selected)
		{
			m_upgradesBtn.disabled = selected;
			MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Store_Menu_Upgrades);
			m_getTokensBtn.disabled = false;
			m_gadgetsBtn.disabled = false;
		}
	}

	public void onToggleFreeButton(UIToggleButton sender, bool selected)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Store_Menu_FreeTokens);
	}

	public void onTouchUpInsideBackButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		sender.disabled = true;
		m_gadgetsBtn.disabled = false;
		m_upgradesBtn.disabled = false;
		m_getTokensBtn.disabled = false;
		MainMenuEventManager.TriggerGoToPreviousMenu();
		sender.disabled = false;
	}

	public void onTouchUpInsideVideoAdButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		sender.disabled = true;
		sender.disabled = false;
	}

	public void onTouchUpInsideTopTokenandFedoraButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		if (!PopUpGUIManager.The.isAPopupActive)
		{
			MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Store_Menu_Tokens);
		}
	}

	private void DownSizeTextToWidth(ref UITextInstance txt, float width)
	{
		if (txt.width > width)
		{
			ResizeTextToWidth(ref txt, width);
		}
	}

	private void ResizeTextToWidth(ref UITextInstance txt, float width)
	{
		float num = width / txt.width;
		txt.textScale *= num;
	}

	private void DownSizeSpriteToWidth(ref UISprite sprite, float width, bool bResizeHeight = false)
	{
		if (sprite.width > width)
		{
			ResizeSpriteToWidth(ref sprite, width, bResizeHeight);
		}
	}

	private void ResizeSpriteToWidth(ref UISprite sprite, float width, bool bResizeHeight = false)
	{
		float num = width / sprite.width;
		sprite.autoRefreshPositionOnScaling = true;
		if (bResizeHeight)
		{
			sprite.scale *= num;
		}
		else
		{
			sprite.scale = new Vector3(sprite.scale.x * num, sprite.scale.y, sprite.scale.z);
		}
	}

	public UISprite GetBottomFrame()
	{
		return m_upgradesBtn;
	}

	public UIButton GetBackButton()
	{
		return m_backButton;
	}

	public UIButton GetVideoAdButton()
	{
		return m_videoAdButton;
	}

	public void ReloadStaticText()
	{
		m_getTokensLabel.text = LocalTextManager.GetUIText("_GET_TOKENS_BTN_LABEL_");
		m_gadgetsLabel.text = LocalTextManager.GetUIText("_GADGETS_BTN_LABEL_");
		m_upgradesLabel.text = LocalTextManager.GetUIText("_UPGRADES_BTN_LABEL_");
		float num = 1f;
		num = ((GameManager.The.aspectRatio.x == 3f || GameManager.The.aspectRatio.y == 4f) ? 0.43f : ((LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.Russian) ? 0.3f : 0.2f));
		if (GameManager.The.aspectRatio.x == 3f && GameManager.The.aspectRatio.y == 4f && LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Russian)
		{
			num = 0.35f;
		}
		if (UI.scaleFactor == 1)
		{
			num /= 2f;
		}
		m_upgradesLabel.textScale = num * UIHelper.CalcFontScale();
		m_gadgetsLabel.textScale = num * UIHelper.CalcFontScale();
		m_getTokensLabel.textScale = num * UIHelper.CalcFontScale();
	}

	public void onTouchUpInsideMoreGamesButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			GameManager.The.PauseMusic();
		}
	}

	private void OnEnable()
	{
		GameEventManager.OnAdCompleted += HandleOnAdCompleted;
	}

	private void HandleOnAdCompleted(BaseAdProvider provider)
	{
		UpdateMoney();
	}

	private void OnDisable()
	{
		GameEventManager.OnAdCompleted += HandleOnAdCompleted;
	}
}

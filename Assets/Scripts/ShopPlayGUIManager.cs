using UnityEngine;

public class ShopPlayGUIManager : MonoBehaviour
{
	private static UIButton m_shopBtn;

	private static UIButton m_playBtn;

	private static UITextInstance m_shopLabel;

	private static UITextInstance m_playLabel;

	private static int m_startStoreDepth;

	private static ShopPlayGUIManager m_the;

	public static ShopPlayGUIManager The
	{
		get
		{
			if (m_the == null)
			{
				GameObject gameObject = new GameObject("ShopPlayGUIManager");
				((ShopPlayGUIManager)gameObject.AddComponent<ShopPlayGUIManager>()).Init();
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
			InitBottomFrameButtons();
			HideButtons();
		}
	}

	private void InitBottomFrameButtons()
	{
		float num = ((!(Screen.dpi >= 400f)) ? 1f : 1.5f);
		float num2 = ((!(Screen.dpi >= 400f)) ? 0.85f : 1.6f);
		float num3 = ((!(Screen.dpi >= 400f)) ? 0.75f : 1.5f);
		if (Screen.dpi >= 400f)
		{
			num2 = 0.8f;
			num3 = 0.7f;
		}
		else if (Screen.dpi <= 170f)
		{
			num2 = 0.7f;
			num3 = 0.6f;
		}
		if (UI.scaleFactor == 1)
		{
			num2 /= 2f;
			num3 /= 2f;
		}
		m_shopBtn = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "ShopButton.png", "PlayButtonOver.png", 0, 0, m_startStoreDepth + 2);
		m_shopBtn.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_shopBtn.centerize();
		m_shopBtn.positionFromBottom(0.01f, -0.25f);
		m_shopBtn.onTouchUpInside += OnShopBtnTouchUpInside;
		if (Screen.dpi >= 400f)
		{
			m_shopBtn.scale = new Vector3(num, num, 1f);
		}
		else
		{
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_shopBtn);
		}
		m_playBtn = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "PlayButton.png", "PlayButtonOver.png", 0, 0, m_startStoreDepth + 2);
		m_playBtn.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_playBtn.centerize();
		m_playBtn.positionFromBottom(0.01f, 0.25f);
		m_playBtn.onTouchUpInside += OnPlayBtnTouchUpInside;
		if (Screen.dpi >= 400f)
		{
			m_playBtn.scale = new Vector3(num, num, 1f);
		}
		else
		{
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_playBtn);
		}
		m_shopLabel = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_SHOP_"), 0f, 0f, 2f, m_startStoreDepth + 1);
		m_shopLabel.alignMode = UITextAlignMode.Center;
		m_shopLabel.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_shopLabel.textScale = num2 * UIHelper.CalcFontScale();
		m_shopLabel.parentUIObject = m_shopBtn;
		m_shopLabel.positionCenter();
		m_shopLabel.text = LocalTextManager.GetUIText("_SHOP_");
		m_shopLabel.zIndex = -9f;
		if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Russian)
		{
			m_shopLabel.textScale = num3 * UIHelper.CalcFontScale();
		}
		m_playLabel = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_PLAY_AGAIN_"), 0f, 0f, 2f, m_startStoreDepth + 1);
		m_playLabel.alignMode = UITextAlignMode.Center;
		m_playLabel.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_playLabel.textScale = num2 * UIHelper.CalcFontScale();
		m_playLabel.parentUIObject = m_playBtn;
		m_playLabel.positionCenter();
		m_playLabel.position = new Vector3(m_playLabel.position.x - 2f, m_playLabel.position.y, m_playLabel.position.z);
		m_playLabel.text = LocalTextManager.GetUIText("_PLAY_AGAIN_");
		m_playLabel.zIndex = -9f;
		if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Russian)
		{
			m_playLabel.textScale = num3 * UIHelper.CalcFontScale();
		}
	}

	public void HideButtons()
	{
		DisableButtons(true);
		m_shopBtn.hidden = true;
		m_playBtn.hidden = true;
		m_shopLabel.hidden = true;
		m_playLabel.hidden = true;
	}

	public void DisableButtons(bool bDisable)
	{
		m_shopBtn.disabled = bDisable;
		m_playBtn.disabled = bDisable;
	}

	public void ShowMainMenu()
	{
		ShowButtons();
	}

	public void ShowInGameMenu()
	{
		ShowButtons();
	}

	public void ShowPauseMenu()
	{
		ShowButtons();
	}

	public void ShowMissionsMainMenu()
	{
		HideButtons();
	}

	public void ShowStoreMenu()
	{
		ShowButtons();
	}

	private void ShowButtons()
	{
		DisableButtons(false);
		m_shopBtn.hidden = false;
		m_playBtn.hidden = false;
		m_shopLabel.hidden = false;
		m_playLabel.hidden = false;
	}

	public void OnShopBtnTouchUpInside(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		CharSelectGUIManager.The.DisableSelectControls();
		HideButtons();
		MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Store_Menu_Upgrades);
		FlurryFacade.Instance.LogEvent("StoreClicked");
		PlayHavenController.ContentRequest(PlayHavenController.PlayHavenPlacement.StoreFromMain);
		CharSelectGUIManager.The.EnableSelectControls();
	}

	public void OnPlayBtnTouchUpInside(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UISTINGER);
		CharSelectGUIManager.The.DisableSelectControls();
		if (CharSelectGUIManager.The.m_isMainMenu || CharSelectGUIManager.The.m_isInGame)
		{
			if (CharSelectGUIManager.The.m_canPlay)
			{
				CharSelectGUIManager.The.PlayNow();
			}
			else
			{
				CharSelectGUIManager.The.GoToLastCharacterAndPlay();
			}
		}
		else if (PauseGUIManager.The.m_isOn)
		{
			MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Game_HUD);
			GameEventManager.TriggerGameUnPause();
			TutorialGUIManager.The.ShowAllAfterPauseResume();
			PauseGUIManager.The.HideAll();
			GlobalGUIManager.The.HideInGameMenu();
			HideButtons();
		}
		CharSelectGUIManager.The.EnableSelectControls();
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

	public void ReloadStaticText()
	{
		m_shopLabel.text = LocalTextManager.GetUIText("_SHOP_");
		m_playLabel.text = LocalTextManager.GetUIText("_PLAY_AGAIN_");
		if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Russian)
		{
			m_shopLabel.textScale = 0.75f;
			m_playLabel.textScale = 0.75f;
		}
	}
}

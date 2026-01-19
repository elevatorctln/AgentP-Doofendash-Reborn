using UnityEngine;

public class StoreGUIManagerShop : MonoBehaviour
{
	public struct UpgradeGUI
	{
		public UIToggleButton[] upgradeSprites;

		public UISprite m_ProgressBarFrame;

		public UIProgressBar m_ProgressBar;
	}

	public enum SHOP_MENU_STATE
	{
		NONE = 0,
		GET_TOKENS = 1,
		GADGETS = 2,
		UPGRADES = 3,
		FREETOKENS = 4
	}

	public struct ShopSubMenu
	{
		public SHOP_MENU_STATE m_shopName;

		public UISprite m_shopFrame;

		public UISlider m_scrollbar;

		public UISprite m_scrollbarPipe;

		public UIScrollableVerticalLayout m_scroll;

		public UIVerticalLayout m_verticalLayout;

		public UISprite[] m_shopItemFrames;

		public UIButton[] m_shopItemButtons;

		public UITextInstance[] m_itemTitles;

		public UITextInstance[] m_itemDescriptions;

		public string[] m_itemTitlesKeys;

		public string[] m_itemDescriptionsKeys;

		public UITextInstance[] m_itemPrices;

		public UITextInstance[] m_buyItemExtraText;

		public UpgradeGUI[] m_upgradeGUIs;

		public UISprite[] m_icons;

		public bool[] m_isInScrollBar;

		public float m_totalHeight;
	}

	private static UITextInstance m_noStoreItemsTitleText;

	private static UITextInstance m_noStoreItemsBodyText;

	private static UITextInstance m_GadgetStoreDescription;

	private static float TITLE_TEXT_SIZE = 0.4f;

	private static float DESC_TEXT_SIZE = 0.3f;

	private static float PRICE_TEXT_SIZE = 0.35f;

	private static float BUY_BUTTON_EXTRA_TEXT_SIZE = 0.35f;

	private static int m_startStoreDepth = 9;

	private static bool m_disableBottomButtons;

	private static StoreGUIManagerShop m_the;

	public static ShopSubMenu m_gadgetShop;

	public static ShopSubMenu m_upgradeShop;

	public static ShopSubMenu m_getTokensShop;

	public static ShopSubMenu m_freeTokensShop;

	public SHOP_MENU_STATE m_currentMenuState;

	private static bool b_buttonsDisabledbyScrollMovement = false;

	public static readonly string[] TOKEN_SHOP_TITLES = new string[3] { "_COOL_OFFERS_", "_COOL_OFFERS_2", "_COOL_OFFERS_3" };

	public static readonly string[] TOKEN_SHOP_DESC = new string[3] { "_EARN_FREE_TOKENS_", "_EARN_FREE_TOKENS_2", "_EARN_FREE_TOKENS_3" };

	public bool m_waitingForPurchase;

	public static StoreGUIManagerShop The
	{
		get
		{
			if (m_the == null)
			{
				GameObject gameObject = new GameObject("StoreGUIManagerShop");
				((StoreGUIManagerShop)gameObject.AddComponent<StoreGUIManagerShop>()).Init();
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
			InitNoConnectionText();
			InitGadgetStoreElements();
			InitGetTokensStoreElements();
			InitUpgradeStoreElements();
			InitFreeTokenStoreElements();
			HideAll();
		}
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private float m_currentScrollOffset = 0f;

	private void Update()
	{
		// Handle mouse wheel scrolling for desktop/WebGL
		float scrollDelta = Input.mouseScrollDelta.y;
		if (scrollDelta != 0f)
		{
			float scrollAmount = scrollDelta * 80f; 
			m_currentScrollOffset -= scrollAmount;
			
			switch (m_currentMenuState)
			{
				case SHOP_MENU_STATE.UPGRADES:
					if (m_upgradeShop.m_scroll != null && !m_upgradeShop.m_scroll.hidden)
					{
						m_currentScrollOffset = Mathf.Clamp(m_currentScrollOffset, 0, m_upgradeShop.m_totalHeight - m_upgradeShop.m_scroll.height);
						m_upgradeShop.m_scroll.scrollTo(-(int)m_currentScrollOffset, false);
					}
					break;
				case SHOP_MENU_STATE.GADGETS:
					if (m_gadgetShop.m_scroll != null && !m_gadgetShop.m_scroll.hidden)
					{
						m_currentScrollOffset = Mathf.Clamp(m_currentScrollOffset, 0, m_gadgetShop.m_totalHeight - m_gadgetShop.m_scroll.height);
						m_gadgetShop.m_scroll.scrollTo(-(int)m_currentScrollOffset, false);
					}
					break;
				case SHOP_MENU_STATE.GET_TOKENS:
					if (m_getTokensShop.m_scroll != null && !m_getTokensShop.m_scroll.hidden)
					{
						m_currentScrollOffset = Mathf.Clamp(m_currentScrollOffset, 0, m_getTokensShop.m_totalHeight - m_getTokensShop.m_scroll.height);
						m_getTokensShop.m_scroll.scrollTo(-(int)m_currentScrollOffset, false);
					}
					break;
			}
		}
	}

	private void InitNoConnectionText()
	{
		m_noStoreItemsTitleText = GlobalGUIManager.The.defaultTextAlt.addTextInstance(UIHelper.WordWrap(LocalTextManager.GetUIText("_GET_MORE_TOKEN_FED_TITLE_"), 18), 0f, 0f, 1f, m_startStoreDepth + 3);
		m_noStoreItemsTitleText.setColorForAllLetters(Color.black);
		m_noStoreItemsTitleText.alignMode = UITextAlignMode.Center;
		m_noStoreItemsTitleText.textScale = 0.7f * UIHelper.CalcFontScale();
		m_noStoreItemsTitleText.positionFromCenter(-0.2f, 0f);
		m_noStoreItemsTitleText.text = UIHelper.WordWrap(LocalTextManager.GetUIText("_GET_MORE_TOKEN_FED_TITLE_"), 18);
		m_noStoreItemsBodyText = GlobalGUIManager.The.defaultTextAlt.addTextInstance(UIHelper.WordWrap(LocalTextManager.GetUIText("_CONNECT_TO_INTERNET_"), 22), 0f, 0f, 1f, m_startStoreDepth + 3);
		m_noStoreItemsBodyText.setColorForAllLetters(Color.black);
		m_noStoreItemsBodyText.alignMode = UITextAlignMode.Center;
		m_noStoreItemsBodyText.parentUIObject = m_noStoreItemsTitleText;
		m_noStoreItemsBodyText.positionFromTop(1.2f);
		m_noStoreItemsBodyText.text = UIHelper.WordWrap(LocalTextManager.GetUIText("_CONNECT_TO_INTERNET_"), 22);
		float num = 1f;
		if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f)
		{
			num = 0.3f;
			if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.German)
			{
				num = 0.17f;
			}
			if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Japanese || LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Chinese)
			{
				num = 0.13f;
			}
		}
		else
		{
			if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
			{
				num = 0.3f;
			}
			if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Japanese || LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Chinese)
			{
				num = 0.22f;
			}
		}
		if (UI.scaleFactor == 1)
		{
			num /= 2f;
		}
		m_noStoreItemsTitleText.textScale = num * UIHelper.CalcFontScale();
		m_noStoreItemsBodyText.textScale = num * UIHelper.CalcFontScale();
	}

	private void InitGadgetStoreElements()
	{
		int gadgetCount = AllItemData.GetGadgetCount();
		m_gadgetShop = default(ShopSubMenu);
		m_gadgetShop.m_shopName = SHOP_MENU_STATE.GADGETS;
		InitStoreScroll(ref m_gadgetShop.m_scroll, ref m_gadgetShop.m_shopFrame);
		m_gadgetShop.m_scroll.client.name = "upgradeStoreScroll";
		m_gadgetShop.m_scroll.onScrollChange += OnGadgetStoreScrollChange;
		m_gadgetShop.m_scrollbarPipe = GlobalGUIManager.The.m_menuToolkit.addSprite("StoreScrollBar.png", 0, 0, m_startStoreDepth);
		m_gadgetShop.m_scrollbarPipe.parentUIObject = m_gadgetShop.m_shopFrame;
		m_gadgetShop.m_scrollbarPipe.positionFromLeft(0.01f, 0.025f);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_gadgetShop.m_scrollbarPipe);
		m_gadgetShop.m_scrollbar = UISlider.create(GlobalGUIManager.The.m_menuToolkit, "StoreMenuSlider.png", "clear.png", 0, 0, UISliderLayout.Vertical, m_startStoreDepth);
		if (Screen.height >= 2048)
		{
			m_gadgetShop.m_scrollbar.setSize(80f, m_gadgetShop.m_scroll.height * 0.96f);
		}
		else
		{
			m_gadgetShop.m_scrollbar.setSize(40f, m_gadgetShop.m_scroll.height * 0.96f);
		}
		m_gadgetShop.m_scrollbar.parentUIObject = m_gadgetShop.m_shopFrame;
		m_gadgetShop.m_scrollbar.updateTransform();
		m_gadgetShop.m_scrollbar.highlightedTouchOffsets = new UIEdgeOffsets(30, 30, 30, 30);
		m_gadgetShop.m_scrollbar.continuous = true;
		m_gadgetShop.m_scrollbar.onChange += OnGadgetStoreScrollBarChange;
		m_gadgetShop.m_scrollbar.value = 0.01f;
		if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f)
		{
			m_gadgetShop.m_scrollbar.positionFromLeft(0.01f, 0.01f);
		}
		else
		{
			m_gadgetShop.m_scrollbar.positionFromLeft(0.01f, 0.014f);
		}
		m_gadgetShop.m_shopItemFrames = new UISprite[gadgetCount];
		m_gadgetShop.m_shopItemButtons = new UIButton[gadgetCount];
		m_gadgetShop.m_itemPrices = new UITextInstance[gadgetCount];
		m_gadgetShop.m_itemTitles = new UITextInstance[gadgetCount];
		m_gadgetShop.m_itemDescriptions = new UITextInstance[gadgetCount];
		m_gadgetShop.m_buyItemExtraText = new UITextInstance[gadgetCount];
		m_gadgetShop.m_icons = new UISprite[gadgetCount];
		m_gadgetShop.m_upgradeGUIs = new UpgradeGUI[gadgetCount];
		m_gadgetShop.m_isInScrollBar = new bool[gadgetCount];
		m_gadgetShop.m_totalHeight = 0f;
		for (int i = 0; i < gadgetCount; i++)
		{
			PurchasableGadgetItem pg = AllItemData.GetGadgetItem(i);
			m_gadgetShop.m_totalHeight += FillInBasicShopItemData(ref m_gadgetShop, pg, i);
			FillInPriceShopItemData(ref m_gadgetShop, pg, i);
			m_gadgetShop.m_shopItemButtons[i].onTouchUpInside += onTouchUpInsideBuyGadgetItemButton;
			m_gadgetShop.m_shopItemButtons[i].onTouchDown += OnTouchDown;
			m_gadgetShop.m_shopItemButtons[i].onTouchUp += OnTouchUp;
			FillInGadgetUpdgradeStuff(ref m_gadgetShop, ref pg, i);
		}
		m_GadgetStoreDescription = GlobalGUIManager.The.defaultTextAlt.addTextInstance(UIHelper.WordWrap(LocalTextManager.GetUIText("_GADGET_STORE_DESC_"), 18), 0f, 0f, 1f, -9);
		m_GadgetStoreDescription.parentUIObject = m_gadgetShop.m_shopFrame;
		m_GadgetStoreDescription.setColorForAllLetters(Color.white);
		m_GadgetStoreDescription.alignMode = UITextAlignMode.Center;
		m_GadgetStoreDescription.positionFromBottom(0.1f, 0f);
		float num2 = 0.5f;
		if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f)
		{
			num2 = 0.3f;
			if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.German)
			{
				num2 = 0.17f;
			}
			if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Japanese || LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Chinese)
			{
				num2 = 0.13f;
			}
		}
		else
		{
			if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
			{
				num2 = 0.3f;
			}
			if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Japanese || LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Chinese)
			{
				num2 = 0.22f;
			}
		}
		if (UI.scaleFactor == 1)
		{
			num2 /= 2f;
		}
		m_GadgetStoreDescription.textScale = num2 * UIHelper.CalcFontScale();
	}

	private void RefreshGadgetStoreElements()
	{
		int gadgetCount = AllItemData.GetGadgetCount();
		float num = 0f;
		m_GadgetStoreDescription.text = LocalTextManager.GetStoreItemText("_GADGET_STORE_DESC_");
		for (int i = 0; i < gadgetCount; i++)
		{
			PurchasableGadgetItem pg = AllItemData.GetGadgetItem(i);
			num += FillInBasicShopItemData(ref m_gadgetShop, pg, i);
			FillInPriceShopItemData(ref m_gadgetShop, pg, i);
			m_gadgetShop.m_shopItemButtons[i].onTouchUpInside += onTouchUpInsideBuyGadgetItemButton;
			m_getTokensShop.m_shopItemButtons[i].onTouchDown += OnTouchDown;
			m_getTokensShop.m_shopItemButtons[i].onTouchUp += OnTouchUp;
			FillInGadgetUpdgradeStuff(ref m_gadgetShop, ref pg, i);
		}
	}

	private void InitUpgradeStoreElements()
	{
		int upgradeCount = AllItemData.GetUpgradeCount();
		int miscUpgradeItemsCount = AllItemData.GetMiscUpgradeItemsCount();
		m_upgradeShop = default(ShopSubMenu);
		m_upgradeShop.m_shopName = SHOP_MENU_STATE.UPGRADES;
		InitStoreScroll(ref m_upgradeShop.m_scroll, ref m_upgradeShop.m_shopFrame);
		m_upgradeShop.m_scroll.client.name = "upgradeStoreScroll";
		m_upgradeShop.m_scroll.onScrollChange += OnUpgradeStoreScrollChange;
		m_upgradeShop.m_scrollbarPipe = GlobalGUIManager.The.m_menuToolkit.addSprite("StoreScrollBar.png", 0, 0, m_startStoreDepth);
		m_upgradeShop.m_scrollbarPipe.parentUIObject = m_upgradeShop.m_shopFrame;
		m_upgradeShop.m_scrollbarPipe.positionFromLeft(0.01f, 0.025f);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_upgradeShop.m_scrollbarPipe);
		m_upgradeShop.m_scrollbar = UISlider.create(GlobalGUIManager.The.m_menuToolkit, "StoreMenuSlider.png", "clear.png", 0, 0, UISliderLayout.Vertical, m_startStoreDepth);
		if (Screen.height >= 2048)
		{
			m_upgradeShop.m_scrollbar.setSize(80f, m_upgradeShop.m_scroll.height * 0.96f);
		}
		else
		{
			m_upgradeShop.m_scrollbar.setSize(40f, m_upgradeShop.m_scroll.height * 0.96f);
		}
		m_upgradeShop.m_scrollbar.parentUIObject = m_upgradeShop.m_shopFrame;
		m_upgradeShop.m_scrollbar.updateTransform();
		m_upgradeShop.m_scrollbar.highlightedTouchOffsets = new UIEdgeOffsets(30, 30, 30, 30);
		m_upgradeShop.m_scrollbar.continuous = true;
		m_upgradeShop.m_scrollbar.onChange += OnUpgradeStoreScrollBarChage;
		m_upgradeShop.m_scrollbar.value = 0.01f;
		if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f)
		{
			m_upgradeShop.m_scrollbar.positionFromLeft(0.01f, 0.01f);
		}
		else
		{
			m_upgradeShop.m_scrollbar.positionFromLeft(0.01f, 0.014f);
		}
		m_upgradeShop.m_shopItemFrames = new UISprite[upgradeCount + miscUpgradeItemsCount];
		m_upgradeShop.m_shopItemButtons = new UIButton[upgradeCount + miscUpgradeItemsCount];
		m_upgradeShop.m_itemPrices = new UITextInstance[upgradeCount + miscUpgradeItemsCount];
		m_upgradeShop.m_itemTitles = new UITextInstance[upgradeCount + miscUpgradeItemsCount];
		m_upgradeShop.m_itemDescriptions = new UITextInstance[upgradeCount + miscUpgradeItemsCount];
		m_upgradeShop.m_buyItemExtraText = new UITextInstance[upgradeCount + miscUpgradeItemsCount];
		m_upgradeShop.m_icons = new UISprite[upgradeCount + miscUpgradeItemsCount];
		m_upgradeShop.m_upgradeGUIs = new UpgradeGUI[upgradeCount + miscUpgradeItemsCount];
		m_upgradeShop.m_isInScrollBar = new bool[upgradeCount + miscUpgradeItemsCount];
		m_upgradeShop.m_totalHeight = 0f;
		for (int i = 0; i < upgradeCount; i++)
		{
			UpgradableItem ui = AllItemData.GetUpgradableItem(i);
			m_upgradeShop.m_totalHeight += FillInBasicShopItemData(ref m_upgradeShop, ui, i);
			FillInPriceShopItemData(ref m_upgradeShop, ui, i);
			m_upgradeShop.m_shopItemButtons[i].onTouchUpInside += onTouchUpInsideBuyUpgradeItemButton;
			m_upgradeShop.m_shopItemButtons[i].onTouchDown += OnTouchDown;
			m_upgradeShop.m_shopItemButtons[i].onTouchUp += OnTouchUp;
			FillInItemUpdgradeStuff(ref m_upgradeShop, ref ui, i);
		}
		for (int j = 0; j < miscUpgradeItemsCount; j++)
		{
			PurchasableItem miscUpgradeItem = AllItemData.GetMiscUpgradeItem(j);
			m_upgradeShop.m_totalHeight += FillInBasicShopItemData(ref m_upgradeShop, miscUpgradeItem, upgradeCount + j);
			FillInPriceShopItemData(ref m_upgradeShop, miscUpgradeItem, upgradeCount + j);
			m_upgradeShop.m_shopItemButtons[upgradeCount + j].onTouchUpInside += onTouchUpInsideBuyMiscItemButton;
			m_upgradeShop.m_shopItemButtons[upgradeCount + j].onTouchDown += OnTouchDown;
			m_upgradeShop.m_shopItemButtons[upgradeCount + j].onTouchUp += OnTouchUp;
			m_upgradeShop.m_upgradeGUIs[upgradeCount + j].upgradeSprites = null;
		}
	}

	private void OnTouchDown(UIButton obj)
	{
		if (m_upgradeShop.m_scroll != null)
			m_upgradeShop.m_scroll.m_disableTouch = true;
		if (m_gadgetShop.m_scroll != null)
			m_gadgetShop.m_scroll.m_disableTouch = true;
		if (m_getTokensShop.m_scroll != null)
			m_getTokensShop.m_scroll.m_disableTouch = true;
	}

	private void OnTouchUp(UIButton obj)
	{
		if (m_upgradeShop.m_scroll != null)
			m_upgradeShop.m_scroll.m_disableTouch = false;
		if (m_gadgetShop.m_scroll != null)
			m_gadgetShop.m_scroll.m_disableTouch = false;
		if (m_getTokensShop.m_scroll != null)
			m_getTokensShop.m_scroll.m_disableTouch = false;
	}

	private void OnUpgradeStoreScrollBarChage(UISlider sender, float val)
	{
		if (!float.IsInfinity(val) && !float.IsPositiveInfinity(val) && !float.IsNegativeInfinity(val) && !float.IsNaN(val))
		{
			int num = (int)((1f - val) * (m_upgradeShop.m_totalHeight - m_upgradeShop.m_scroll.height));
			m_upgradeShop.m_scroll.scrollTo(-num, false);
		}
	}

	private void OnUpgradeStoreScrollChange(UIScrollableVerticalLayout sender, float val)
	{
		if (!m_upgradeShop.m_scrollbar.highlighted && !float.IsInfinity(val) && !float.IsPositiveInfinity(val) && !float.IsNegativeInfinity(val) && !float.IsNaN(val))
		{
			float value = val / (m_upgradeShop.m_totalHeight - m_upgradeShop.m_scroll.height);
			m_upgradeShop.m_scrollbar.value = 1f - Mathf.Clamp01(value);
		}
	}

	private void OnGadgetStoreScrollBarChange(UISlider sender, float val)
	{
		if (!float.IsInfinity(val) && !float.IsPositiveInfinity(val) && !float.IsNegativeInfinity(val) && !float.IsNaN(val))
		{
			int num = (int)((1f - val) * (m_gadgetShop.m_totalHeight - m_gadgetShop.m_scroll.height));
			m_gadgetShop.m_scroll.scrollTo(-num, false);
		}
	}

	private void OnGadgetStoreScrollChange(UIScrollableVerticalLayout sender, float val)
	{
		if (!m_gadgetShop.m_scrollbar.highlighted && !float.IsInfinity(val) && !float.IsPositiveInfinity(val) && !float.IsNegativeInfinity(val) && !float.IsNaN(val))
		{
			float value = val / (m_gadgetShop.m_totalHeight - m_gadgetShop.m_scroll.height);
			m_gadgetShop.m_scrollbar.value = 1f - Mathf.Clamp01(value);
		}
	}

	private void OnStoreScrollStartedMoveHandler(UIScrollableVerticalLayout sender)
	{
		if (!b_buttonsDisabledbyScrollMovement)
		{
			switch (m_currentMenuState)
			{
			case SHOP_MENU_STATE.UPGRADES:
				DisableAllShopButtons(ref m_upgradeShop, true);
				break;
			case SHOP_MENU_STATE.GET_TOKENS:
				DisableAllShopButtons(ref m_getTokensShop, true);
				break;
			case SHOP_MENU_STATE.GADGETS:
				break;
			}
		}
	}

	private void OnStoreScrollTouchEnded(UIScrollableVerticalLayout sender)
	{
		b_buttonsDisabledbyScrollMovement = false;
		switch (m_currentMenuState)
		{
		case SHOP_MENU_STATE.UPGRADES:
			DisableAllShopButtons(ref m_upgradeShop, false);
			break;
		case SHOP_MENU_STATE.GET_TOKENS:
			DisableAllShopButtons(ref m_getTokensShop, false);
			break;
		case SHOP_MENU_STATE.GADGETS:
			break;
		}
	}

	private void InitGetTokensStoreElements()
	{
		m_getTokensShop = default(ShopSubMenu);
		m_getTokensShop.m_shopName = SHOP_MENU_STATE.GET_TOKENS;
		InitStoreScroll(ref m_getTokensShop.m_scroll, ref m_getTokensShop.m_shopFrame);
		m_getTokensShop.m_scroll.client.name = "tokenStoreScroll";
		m_getTokensShop.m_scroll.onScrollChange += OnTokenShopScrollChange;
		m_getTokensShop.m_scrollbarPipe = GlobalGUIManager.The.m_menuToolkit.addSprite("StoreScrollBar.png", 0, 0, m_startStoreDepth);
		m_getTokensShop.m_scrollbarPipe.parentUIObject = m_getTokensShop.m_shopFrame;
		m_getTokensShop.m_scrollbarPipe.positionFromLeft(0.005f, 0.025f);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_getTokensShop.m_scrollbarPipe);
		m_getTokensShop.m_scrollbar = UISlider.create(GlobalGUIManager.The.m_menuToolkit, "StoreMenuSlider.png", "clear.png", 0, 0, UISliderLayout.Vertical, m_startStoreDepth);
		if (Screen.height >= 2048)
		{
			m_getTokensShop.m_scrollbar.setSize(80f, m_getTokensShop.m_scroll.height * 0.96f);
		}
		else
		{
			m_getTokensShop.m_scrollbar.setSize(40f, m_getTokensShop.m_scroll.height * 0.96f);
		}
		m_getTokensShop.m_scrollbar.parentUIObject = m_getTokensShop.m_scroll;
		m_getTokensShop.m_scrollbar.updateTransform();
		m_getTokensShop.m_scrollbar.highlightedTouchOffsets = new UIEdgeOffsets(30, 30, 30, 30);
		m_getTokensShop.m_scrollbar.continuous = true;
		m_getTokensShop.m_scrollbar.onChange += OnTokenShopScrollBarChage;
		m_getTokensShop.m_scrollbar.value = 0.01f;
		if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f)
		{
			m_getTokensShop.m_scrollbar.positionFromLeft(0.01f, -0.042f);
		}
		else
		{
			m_getTokensShop.m_scrollbar.positionFromLeft(0.01f, -0.038f);
		}
		int tokenItemsCount = AllItemData.GetTokenItemsCount();
		m_getTokensShop.m_shopItemFrames = new UISprite[tokenItemsCount];
		m_getTokensShop.m_shopItemButtons = new UIButton[tokenItemsCount];
		m_getTokensShop.m_itemPrices = new UITextInstance[tokenItemsCount];
		m_getTokensShop.m_itemTitles = new UITextInstance[tokenItemsCount];
		m_getTokensShop.m_itemDescriptions = new UITextInstance[tokenItemsCount];
		m_getTokensShop.m_itemTitlesKeys = new string[tokenItemsCount];
		m_getTokensShop.m_itemDescriptionsKeys = new string[tokenItemsCount];
		m_getTokensShop.m_buyItemExtraText = new UITextInstance[tokenItemsCount];
		m_getTokensShop.m_icons = new UISprite[tokenItemsCount];
		m_getTokensShop.m_isInScrollBar = new bool[tokenItemsCount];
		m_getTokensShop.m_totalHeight = 0f;
		for (int i = 0; i < tokenItemsCount; i++)
		{
			PurchasableItem tokenItem = AllItemData.GetTokenItem(i);
			m_getTokensShop.m_totalHeight += FillInBasicShopItemData(ref m_getTokensShop, tokenItem, i);
			FillInPriceShopItemData(ref m_getTokensShop, tokenItem, i);
			m_getTokensShop.m_shopItemButtons[i].onTouchUpInside += onTouchUpInsideBuyGetTokensItemButton;
			m_getTokensShop.m_shopItemButtons[i].onTouchDown += OnTouchDown;
			m_getTokensShop.m_shopItemButtons[i].onTouchUp += OnTouchUp;
		}
	}

	private void OnTokenShopScrollBarChage(UISlider sender, float val)
	{
		if (!float.IsInfinity(val) && !float.IsPositiveInfinity(val) && !float.IsNegativeInfinity(val) && !float.IsNaN(val))
		{
			int num = (int)((1f - val) * (m_getTokensShop.m_totalHeight - m_getTokensShop.m_scroll.height));
			m_getTokensShop.m_scroll.scrollTo(-num, false);
		}
	}

	private void OnTokenShopScrollChange(UIScrollableVerticalLayout sender, float val)
	{
		if (!m_getTokensShop.m_scrollbar.highlighted && !float.IsInfinity(val) && !float.IsPositiveInfinity(val) && !float.IsNegativeInfinity(val) && !float.IsNaN(val))
		{
			float value = val / (m_getTokensShop.m_totalHeight - m_getTokensShop.m_scroll.height);
			m_getTokensShop.m_scrollbar.value = 1f - Mathf.Clamp01(value);
		}
	}

	private void InitFreeTokenStoreElements()
	{
		int num = 3;
		num = 3;
		m_freeTokensShop = default(ShopSubMenu);
		m_freeTokensShop.m_shopName = SHOP_MENU_STATE.FREETOKENS;
		m_freeTokensShop.m_shopFrame = GlobalGUIManager.The.m_menuToolkit.addSprite("StorePlateBG.png", 0, 0, m_startStoreDepth + 30);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_freeTokensShop.m_shopFrame);
		m_freeTokensShop.m_shopFrame.centerize();
		m_freeTokensShop.m_shopFrame.positionCenter();
		m_freeTokensShop.m_verticalLayout = new UIVerticalLayout(10);
		m_freeTokensShop.m_verticalLayout.parentUIObject = m_gadgetShop.m_shopFrame;
		m_freeTokensShop.m_verticalLayout.positionFromTopLeft(100f, 0.02f);
		m_freeTokensShop.m_shopItemFrames = new UISprite[num];
		m_freeTokensShop.m_shopItemButtons = new UIButton[num];
		m_freeTokensShop.m_icons = new UISprite[num];
		m_freeTokensShop.m_itemPrices = new UITextInstance[num];
		m_freeTokensShop.m_itemTitles = new UITextInstance[num];
		m_freeTokensShop.m_itemDescriptions = new UITextInstance[num];
		m_freeTokensShop.m_isInScrollBar = new bool[num];
		for (int i = 0; i < num; i++)
		{
			m_freeTokensShop.m_shopItemFrames[i] = GlobalGUIManager.The.m_menuToolkit.addSprite("StoreLargePlateA.png", 0, 0, m_startStoreDepth + 6);
			m_freeTokensShop.m_verticalLayout.addChild(m_freeTokensShop.m_shopItemFrames[i]);
			m_freeTokensShop.m_isInScrollBar[i] = false;
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_freeTokensShop.m_shopItemFrames[i]);
			DownSizeSpriteToWidth(ref m_freeTokensShop.m_shopItemFrames[i], m_freeTokensShop.m_shopFrame.width * 0.96f);
			string text = ((i != 0) ? LocalTextManager.GetUIText("_FREE_STUFF_") : LocalTextManager.GetUIText("_FREE_TOKENS_"));
			text = LocalTextManager.GetUIText("_FREE_STUFF_");
			m_freeTokensShop.m_itemTitles[i] = GlobalGUIManager.The.defaultText.addTextInstance(text, 0f, 0f, 1f, m_startStoreDepth + 4);
			m_freeTokensShop.m_itemTitles[i].parentUIObject = m_freeTokensShop.m_shopItemFrames[i];
			m_freeTokensShop.m_itemTitles[i].alignMode = UITextAlignMode.Center;
			m_freeTokensShop.m_itemTitles[i].textScale = TITLE_TEXT_SIZE;
			m_freeTokensShop.m_itemTitles[i].setColorForAllLetters(Color.white);
			m_freeTokensShop.m_itemTitles[i].pixelsFromTop(5);
			DownSizeTextToWidth(ref m_freeTokensShop.m_itemTitles[i], m_freeTokensShop.m_shopItemFrames[i].width * m_freeTokensShop.m_shopItemFrames[i].scale.x - 12f);
			string uIText = LocalTextManager.GetUIText("_EARN_FREE_TOKENS_");
			m_freeTokensShop.m_itemDescriptions[i] = GlobalGUIManager.The.defaultTextAlt.addTextInstance(UIHelper.LineBreakString(uIText, 28), 0f, 0f, 1f, m_startStoreDepth + 4);
			m_freeTokensShop.m_itemDescriptions[i].parentUIObject = m_freeTokensShop.m_shopItemFrames[i];
			m_freeTokensShop.m_itemDescriptions[i].alignMode = UITextAlignMode.Center;
			m_freeTokensShop.m_itemDescriptions[i].textScale = DESC_TEXT_SIZE * UIHelper.CalcFontScale();
			m_freeTokensShop.m_itemDescriptions[i].setColorForAllLetters(Color.black);
			m_freeTokensShop.m_itemDescriptions[i].positionFromTop(0.3f);
			DownSizeTextToWidth(ref m_freeTokensShop.m_itemDescriptions[i], m_freeTokensShop.m_shopItemFrames[i].width * m_freeTokensShop.m_shopItemFrames[i].scale.x - 12f);
			string text2 = "FreeTokensIcon.png";
			m_freeTokensShop.m_icons[i] = GlobalGUIManager.The.m_menuToolkit.addSprite(text2, 0, 0, m_startStoreDepth + 6);
			m_freeTokensShop.m_icons[i].parentUIObject = m_freeTokensShop.m_shopItemFrames[i];
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_freeTokensShop.m_icons[i]);
			m_freeTokensShop.m_icons[i].positionFromBottomLeft(0.1f, 0.01f);
			m_freeTokensShop.m_shopItemButtons[i] = UIButton.create(GlobalGUIManager.The.m_hudToolkit, "CashSaleButton.png", "CashSaleButtonOver.png", 0, 0, m_startStoreDepth + 2);
			m_freeTokensShop.m_shopItemButtons[i].highlightedTouchOffsets = new UIEdgeOffsets(30);
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_freeTokensShop.m_shopItemButtons[i]);
			m_freeTokensShop.m_shopItemButtons[i].parentUIObject = m_freeTokensShop.m_shopItemFrames[i];
			m_freeTokensShop.m_shopItemButtons[i].pixelsFromBottomRight(5, 5);
			m_freeTokensShop.m_itemPrices[i] = GlobalGUIManager.The.defaultTextAlt.addTextInstance("FREE", 0f, 0f, 1f, m_startStoreDepth + 1);
			m_freeTokensShop.m_itemPrices[i].parentUIObject = m_freeTokensShop.m_shopItemButtons[i];
			m_freeTokensShop.m_itemPrices[i].alignMode = UITextAlignMode.Right;
			m_freeTokensShop.m_itemPrices[i].verticalAlignMode = UITextVerticalAlignMode.Middle;
			float num2 = ((UI.scaleFactor != 1) ? PRICE_TEXT_SIZE : (PRICE_TEXT_SIZE / 2f));
			m_freeTokensShop.m_itemPrices[i].textScale = num2 * UIHelper.CalcFontScale();
			m_freeTokensShop.m_itemPrices[i].setColorForAllLetters(Color.white);
			m_freeTokensShop.m_itemPrices[i].zIndex = -5f;
			m_freeTokensShop.m_itemPrices[i].positionFromBottomRight(0.3f, 0.09f);
			m_freeTokensShop.m_itemPrices[i].parentUIObject = m_freeTokensShop.m_shopItemFrames[i];
			DownSizeTextToWidth(ref m_freeTokensShop.m_itemPrices[i], m_freeTokensShop.m_shopItemButtons[i].width * m_freeTokensShop.m_shopItemButtons[i].scale.x - 20f);
		}
		m_freeTokensShop.m_itemPrices[0].text = LocalTextManager.GetUIText("COOL_OFFERS");
		m_freeTokensShop.m_shopItemButtons[0].onTouchUpInside += onTouchUpInsideFreeItemsButton;
		m_freeTokensShop.m_itemPrices[1].text = LocalTextManager.GetUIText("TOKEN_ADS");
		m_freeTokensShop.m_shopItemButtons[1].onTouchUpInside += onTouchUpInsideTokenAd;
	}

	private void InitStoreScroll(ref UIScrollableVerticalLayout storeScroll, ref UISprite storeFrame)
	{
		storeFrame = GlobalGUIManager.The.m_menuToolkit.addSprite("StorePlateBG.png", 0, 0, m_startStoreDepth + 30);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref storeFrame);
		
		float backButtonHeight = StoreGUIManagerPersistentElements.The.GetBackButton().height;
		float bottomFrameHeight = StoreGUIManagerPersistentElements.The.GetBottomFrame().height;
		float availableHeight = Screen.height - backButtonHeight - bottomFrameHeight;
		float availableWidth = Screen.width - 20; 
		
		float targetWidth = availableWidth * 0.95f;
		float targetHeight = availableHeight * 0.85f;
		
		float scaleX = targetWidth / storeFrame.width;
		float scaleY = targetHeight / storeFrame.height;
		float useScale = Mathf.Min(scaleX, scaleY);
		storeFrame.scale = new Vector3(useScale, useScale, 1f);
		
		storeFrame.centerize();
		storeFrame.positionFromCenter(-0.025f, 0f);
		
		storeScroll = new UIScrollableVerticalLayout(10);
		storeScroll.verticalAlignMode = UIAbstractContainer.UIContainerVerticalAlignMode.Top;
		storeScroll.alignMode = UIAbstractContainer.UIContainerAlignMode.Center;
		storeScroll.setSize(storeFrame.width * 0.95f, storeFrame.height * 0.97f);
		storeScroll.parentUIObject = storeFrame;
		storeScroll.positionCenter();
		storeScroll.positionFromLeft(0.05f);
		storeScroll.onScrollStartedMoveEvent += OnStoreScrollStartedMoveHandler;
		storeScroll.onTouchEndedAction += OnStoreScrollTouchEnded;
	}

	private void RefreshGetTokensData()
	{
		int tokenItemsCount = AllItemData.GetTokenItemsCount();
		RefreshShopPrices(m_getTokensShop);
		for (int i = 0; i < tokenItemsCount; i++)
		{
			PurchasableItem tokenItem = AllItemData.GetTokenItem(i);
			if (!tokenItem.m_owned)
			{
				if (tokenItem.m_realMoneyCost <= 0f)
				{
					m_getTokensShop.m_scroll.removeChild(m_getTokensShop.m_shopItemFrames[i], false);
					HideShopItemElement(ref m_getTokensShop, i);
					m_getTokensShop.m_isInScrollBar[i] = false;
				}
				else if (!m_getTokensShop.m_isInScrollBar[i])
				{
					m_getTokensShop.m_scroll.addChild(m_getTokensShop.m_shopItemFrames[i]);
					m_getTokensShop.m_isInScrollBar[i] = true;
				}
			}
		}
	}

	private float FillInBasicShopItemData(ref ShopSubMenu shop, PurchasableItem pi, int i)
	{
		float num = 0f;
		if (shop.m_scroll != null)
		{
			shop.m_shopItemFrames[i] = GlobalGUIManager.The.m_menuToolkit.addSprite("StoreSmallPlateA.png", 0, 0, m_startStoreDepth + 6);
			shop.m_scroll.addChild(shop.m_shopItemFrames[i]);
			shop.m_isInScrollBar[i] = true;
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref shop.m_shopItemFrames[i]);
			DownSizeSpriteToWidth(ref shop.m_shopItemFrames[i], shop.m_scroll.width * 0.94f);
			num += shop.m_shopItemFrames[i].height + (float)shop.m_scroll.spacing + 1f;
		}
		else
		{
			shop.m_shopItemFrames[i] = GlobalGUIManager.The.m_menuToolkit.addSprite("StoreLargePlateA.png", 0, 0, m_startStoreDepth + 6);
			shop.m_verticalLayout.addChild(shop.m_shopItemFrames[i]);
			shop.m_isInScrollBar[i] = false;
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref shop.m_shopItemFrames[i]);
			DownSizeSpriteToWidth(ref shop.m_shopItemFrames[i], shop.m_shopFrame.width * 0.96f);
		}
		shop.m_shopItemFrames[i].userData = pi;
		if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f)
		{
			if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
			{
				TITLE_TEXT_SIZE = 0.25f;
			}
			else
			{
				TITLE_TEXT_SIZE = 0.3f;
			}
		}
		else
		{
			TITLE_TEXT_SIZE = 0.4f;
		}
		if (GameManager.The.aspectRatio.x == 3f && GameManager.The.aspectRatio.y == 4f && LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			TITLE_TEXT_SIZE = 0.35f;
		}
		if (UI.scaleFactor == 1)
		{
			TITLE_TEXT_SIZE /= 1.5f;
			DESC_TEXT_SIZE /= 1.5f;
		}
		if (shop.m_itemTitlesKeys != null)
		{
			shop.m_itemTitlesKeys[i] = pi.m_nameLocKey;
		}
		shop.m_itemTitles[i] = GlobalGUIManager.The.defaultText.addTextInstance(pi.m_name, 0f, 0f, 1f, m_startStoreDepth - 16);
		shop.m_itemTitles[i].parentUIObject = shop.m_shopItemFrames[i];
		shop.m_itemTitles[i].alignMode = UITextAlignMode.Center;
		shop.m_itemTitles[i].textScale = TITLE_TEXT_SIZE * UIHelper.CalcFontScale();
		shop.m_itemTitles[i].setColorForAllLetters(Color.white);
		shop.m_itemTitles[i].text = pi.m_name;
		shop.m_itemTitles[i].positionFromTop(0.08f);
		DownSizeTextToWidth(ref shop.m_itemTitles[i], shop.m_shopItemFrames[i].width * shop.m_shopItemFrames[i].scale.x - 12f);
		if (shop.m_itemDescriptionsKeys != null)
		{
			shop.m_itemDescriptionsKeys[i] = pi.m_descLocKey;
		}
		shop.m_itemDescriptions[i] = GlobalGUIManager.The.defaultTextAlt.addTextInstance(UIHelper.WordWrap(pi.m_desc, 28), 0f, 0f, 1f, m_startStoreDepth - 16);
		shop.m_itemDescriptions[i].parentUIObject = shop.m_shopItemFrames[i];
		shop.m_itemDescriptions[i].alignMode = UITextAlignMode.Center;
		shop.m_itemDescriptions[i].textScale = DESC_TEXT_SIZE * UIHelper.CalcFontScale();
		shop.m_itemDescriptions[i].setColorForAllLetters(Color.black);
		shop.m_itemDescriptions[i].text = UIHelper.WordWrap(pi.m_desc, 28);
		shop.m_itemDescriptions[i].positionFromTop(0.25f);
		DownSizeTextToWidth(ref shop.m_itemDescriptions[i], shop.m_shopItemFrames[i].width * shop.m_shopItemFrames[i].scale.x - 12f);
		string iconFileName = pi.m_iconFileName;
		if (iconFileName != null)
		{
			shop.m_icons[i] = GlobalGUIManager.The.m_menuToolkit.addSprite(iconFileName, 0, 0, m_startStoreDepth + 6);
			shop.m_icons[i].parentUIObject = shop.m_shopItemFrames[i];
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref shop.m_icons[i]);
			shop.m_icons[i].positionFromBottomLeft(0.1f, 0.01f);
		}
		else
		{
			shop.m_icons[i] = null;
		}
		return num;
	}

	private bool FillInPriceShopItemData(ref ShopSubMenu shop, PurchasableItem pi, int i)
	{
		string filename = "CashSaleButton.png";
		string highlightedFilename = "CashSaleButtonOver.png";
		bool result = true;
		string text;
		if (pi.m_tokenCost > 0)
		{
			text = pi.m_tokenCost.ToString("N0");
			filename = "TokenSaleButton.png";
			highlightedFilename = "TokenSaleButtonOver.png";
		}
		else if (pi.m_fedoraCost > 0)
		{
			text = pi.m_fedoraCost.ToString("N0");
			filename = "FedoraSaleButton.png";
			highlightedFilename = "FedoraSaleButtonOver.png";
		}
		else if (pi.m_realMoneyCost > 0f)
		{
			text = pi.m_realMoneyCost.ToString("N2");
		}
		else
		{
			text = "0.00";
			result = false;
		}
		shop.m_shopItemButtons[i] = UIButton.create(GlobalGUIManager.The.m_hudToolkit, filename, highlightedFilename, 0, 0, m_startStoreDepth + 2);
		shop.m_shopItemButtons[i].highlightedTouchOffsets = new UIEdgeOffsets(30);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref shop.m_shopItemButtons[i]);
		shop.m_shopItemButtons[i].parentUIObject = shop.m_shopItemFrames[i];
		shop.m_shopItemButtons[i].pixelsFromBottomRight(5, 5);
		shop.m_itemPrices[i] = GlobalGUIManager.The.defaultTextAlt.addTextInstance(text, 0f, 0f, 1f, m_startStoreDepth + 1);
		shop.m_itemPrices[i].parentUIObject = shop.m_shopItemButtons[i];
		shop.m_itemPrices[i].alignMode = UITextAlignMode.Right;
		shop.m_itemPrices[i].verticalAlignMode = UITextVerticalAlignMode.Middle;
		float num = ((UI.scaleFactor != 1) ? PRICE_TEXT_SIZE : (PRICE_TEXT_SIZE / 2f));
		shop.m_itemPrices[i].textScale = num * UIHelper.CalcFontScale();
		shop.m_itemPrices[i].setColorForAllLetters(Color.white);
		shop.m_itemPrices[i].zIndex = -5f;
		if (pi.m_fedoraCost > 0)
		{
			shop.m_itemPrices[i].positionFromBottomLeft(0.3f, 0.15f);
		}
		else
		{
			shop.m_itemPrices[i].positionFromBottomRight(0.3f, 0.09f);
		}
		shop.m_itemPrices[i].parentUIObject = shop.m_shopItemFrames[i];
		if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f)
		{
			shop.m_itemPrices[i].textScale = 0.28f * UIHelper.CalcFontScale();
		}
		shop.m_shopItemButtons[i].userData = shop.m_itemPrices[i];
		string text2 = string.Empty;
		if (pi.m_buyExtraText != null)
		{
			text2 = pi.m_buyExtraText;
		}
		shop.m_buyItemExtraText[i] = GlobalGUIManager.The.defaultTextAlt.addTextInstance(text2, 0f, 0f, 1f, 4);
		shop.m_buyItemExtraText[i].parentUIObject = shop.m_shopItemFrames[i];
		shop.m_buyItemExtraText[i].alignMode = UITextAlignMode.Right;
		shop.m_buyItemExtraText[i].verticalAlignMode = UITextVerticalAlignMode.Middle;
		float num2 = ((UI.scaleFactor != 1) ? BUY_BUTTON_EXTRA_TEXT_SIZE : (BUY_BUTTON_EXTRA_TEXT_SIZE / 2f));
		shop.m_buyItemExtraText[i].textScale = num2 * UIHelper.CalcFontScale();
		shop.m_buyItemExtraText[i].setColorForAllLetters(Color.black);
		if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f)
		{
			shop.m_buyItemExtraText[i].textScale = 0.25f * UIHelper.CalcFontScale();
		}
		if (pi.m_fedoraCost > 0)
		{
			shop.m_buyItemExtraText[i].positionFromRight(0.01f, 0.02f);
		}
		else
		{
			shop.m_buyItemExtraText[i].positionFromBottomRight(0.14f, 0.3f);
		}
		return result;
	}

	private void FillInGadgetUpdgradeStuff(ref ShopSubMenu shop, ref PurchasableGadgetItem pg, int i)
	{
		shop.m_upgradeGUIs[i] = default(UpgradeGUI);
		shop.m_upgradeGUIs[i].upgradeSprites = null;
		shop.m_upgradeGUIs[i].m_ProgressBarFrame = GlobalGUIManager.The.m_menuToolkit.addSprite("SuperChargeBlasterBarEmpty.png", 0, 0, m_startStoreDepth + 4);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref shop.m_upgradeGUIs[i].m_ProgressBarFrame);
		shop.m_upgradeGUIs[i].m_ProgressBarFrame.parentUIObject = shop.m_shopItemFrames[i];
		shop.m_upgradeGUIs[i].m_ProgressBarFrame.positionFromCenter(0.3f, -0.04f);
		shop.m_upgradeGUIs[i].m_ProgressBar = UIProgressBar.create(GlobalGUIManager.The.m_hudToolkit, "SuperChargeBlasterBarFull.png", 0, 0, false, m_startStoreDepth + 3);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref shop.m_upgradeGUIs[i].m_ProgressBar);
		shop.m_upgradeGUIs[i].m_ProgressBar.parentUIObject = shop.m_upgradeGUIs[i].m_ProgressBarFrame;
		shop.m_upgradeGUIs[i].m_ProgressBar.positionFromLeft(0.005f);
		shop.m_upgradeGUIs[i].m_ProgressBar.resizeTextureOnChange = true;
		shop.m_upgradeGUIs[i].m_ProgressBar.value = 0f;
		shop.m_buyItemExtraText[i].positionFromRight(0.01f, 0.02f);
		if (pg.m_maxUpgrades == 0)
		{
			shop.m_upgradeGUIs[i].m_ProgressBar.value = 1f;
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref shop.m_upgradeGUIs[i].m_ProgressBar);
			return;
		}
		for (int j = 0; j <= pg.m_maxUpgrades && pg.UpgradeNums >= j; j++)
		{
			shop.m_upgradeGUIs[i].m_ProgressBar.value = (float)j / (float)pg.m_maxUpgrades;
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref shop.m_upgradeGUIs[i].m_ProgressBar);
		}
	}

	private void FillInItemUpdgradeStuff(ref ShopSubMenu shop, ref UpgradableItem ui, int i)
	{
		shop.m_upgradeGUIs[i] = default(UpgradeGUI);
		shop.m_upgradeGUIs[i].upgradeSprites = null;
		if (ui.m_maxUpgradeTimes <= 1)
		{
			return;
		}
		shop.m_upgradeGUIs[i].upgradeSprites = new UIToggleButton[ui.m_maxUpgradeTimes];
		float num = 0.08f;
		float num2 = (float)ui.m_maxUpgradeTimes / 2f * num + 0.02f;
		float num3 = 0f - num2 + num / 2f;
		float percentFromTop = 0.55f;
		if (GameManager.The.aspectRatio.x != 3f || GameManager.The.aspectRatio.y != 4f)
		{
			num = 0.08f;
			num2 = (float)ui.m_maxUpgradeTimes / 2f * num + 0.022f;
			num3 = 0f - num2 + num / 2f;
			percentFromTop = 0.55f;
		}
		for (int j = 0; j < ui.m_maxUpgradeTimes; j++)
		{
			if (j == 0)
			{
				shop.m_upgradeGUIs[i].upgradeSprites[j] = UIToggleButton.create(GlobalGUIManager.The.m_menuToolkit, "UpgradeEndCapEmpty.png", "UpgradeEndCapFull.png", "UpgradeEndCapEmpty.png", 0, 0, m_startStoreDepth + 4);
				shop.m_upgradeGUIs[i].upgradeSprites[j].parentUIObject = shop.m_shopItemFrames[i];
				shop.m_upgradeGUIs[i].upgradeSprites[j].positionFromTop(percentFromTop, num3);
				shop.m_upgradeGUIs[i].upgradeSprites[j].disabled = true;
				if (ui.upgradesOwned > j)
				{
					shop.m_upgradeGUIs[i].upgradeSprites[j].selected = true;
				}
			}
			else if (j == ui.m_maxUpgradeTimes - 1)
			{
				shop.m_upgradeGUIs[i].upgradeSprites[j] = UIToggleButton.create(GlobalGUIManager.The.m_menuToolkit, "UpgradeEndCapEmpty.png", "UpgradeEndCapFull.png", "UpgradeEndCapEmpty.png", 0, 0, m_startStoreDepth + 4);
				shop.m_upgradeGUIs[i].upgradeSprites[j].parentUIObject = shop.m_shopItemFrames[i];
				shop.m_upgradeGUIs[i].upgradeSprites[j].positionFromTop(percentFromTop, num3 + num + 0.005f);
				shop.m_upgradeGUIs[i].upgradeSprites[j].eulerAngles = new Vector3(shop.m_upgradeGUIs[i].upgradeSprites[j].eulerAngles.x, 180f, shop.m_upgradeGUIs[i].upgradeSprites[j].eulerAngles.z);
				shop.m_upgradeGUIs[i].upgradeSprites[j].disabled = true;
				if (ui.upgradesOwned > j)
				{
					shop.m_upgradeGUIs[i].upgradeSprites[j].selected = true;
				}
			}
			else
			{
				shop.m_upgradeGUIs[i].upgradeSprites[j] = UIToggleButton.create(GlobalGUIManager.The.m_menuToolkit, "UpgradeMiddleEmpty.png", "UpgradeMiddleFull.png", "UpgradeMiddleEmpty.png", 0, 0, m_startStoreDepth + 4);
				shop.m_upgradeGUIs[i].upgradeSprites[j].parentUIObject = shop.m_shopItemFrames[i];
				shop.m_upgradeGUIs[i].upgradeSprites[j].positionFromTop(percentFromTop, num3);
				shop.m_upgradeGUIs[i].upgradeSprites[j].disabled = true;
				if (ui.upgradesOwned > j)
				{
					shop.m_upgradeGUIs[i].upgradeSprites[j].selected = true;
				}
			}
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref shop.m_upgradeGUIs[i].upgradeSprites[j]);
			num3 += num;
		}
	}

	public void OnConnectToGetTokens(bool success)
	{
		DebugManager.Log("OnConnectToGetTokens! " + success);
		if (m_currentMenuState == SHOP_MENU_STATE.GET_TOKENS)
		{
			if (success)
			{
				DebugManager.Log("True: Get Tokens");
				ShowGetTokenShop();
			}
			else
			{
				DebugManager.Log("Fails: Show NoTokensMessage");
				ShowCantConnectText();
			}
		}
	}

	public void HideAll()
	{
		HideNoConnectionText();
		HideAllShops();
	}

	public void HideNoConnectionText()
	{
		m_noStoreItemsTitleText.hidden = true;
		m_noStoreItemsBodyText.hidden = true;
	}

	public void HideAllShops()
	{
		m_noStoreItemsBodyText.hidden = true;
		m_noStoreItemsTitleText.hidden = true;
		m_currentMenuState = SHOP_MENU_STATE.NONE;
		HideShop(ref m_gadgetShop);
		HideShop(ref m_upgradeShop);
		HideShop(ref m_getTokensShop);
		HideShop(ref m_freeTokensShop);
	}

	private void HideShop(ref ShopSubMenu shop)
	{
		if (shop.m_shopName != SHOP_MENU_STATE.GET_TOKENS && shop.m_shopName == SHOP_MENU_STATE.GADGETS)
		{
			m_GadgetStoreDescription.hidden = true;
		}
		if (shop.m_scroll != null)
		{
			shop.m_scroll.scrollTo(0, false);
			shop.m_scroll.hidden = true;
		}
		if (shop.m_scrollbar != null)
		{
			shop.m_scrollbar.hidden = true;
		}
		if (shop.m_scrollbarPipe != null)
		{
			shop.m_scrollbarPipe.hidden = true;
		}
		shop.m_shopFrame.hidden = true;
		if (shop.m_shopItemFrames != null)
		{
			for (int i = 0; i < shop.m_shopItemFrames.Length; i++)
			{
				HideShopItemElement(ref shop, i);
			}
		}
	}

	private void HideShopItemElement(ref ShopSubMenu shop, int i)
	{
		if (shop.m_shopItemFrames == null || i > shop.m_shopItemFrames.Length)
		{
			return;
		}
		shop.m_shopItemFrames[i].hidden = true;
		shop.m_itemTitles[i].hidden = true;
		shop.m_itemDescriptions[i].hidden = true;
		if (shop.m_icons[i] != null)
		{
			shop.m_icons[i].hidden = true;
		}
		if (shop.m_shopItemButtons[i] != null)
		{
			shop.m_shopItemButtons[i].hidden = true;
			shop.m_itemPrices[i].hidden = true;
			if (shop.m_buyItemExtraText != null)
			{
				shop.m_buyItemExtraText[i].hidden = true;
			}
		}
		if (shop.m_upgradeGUIs != null && shop.m_upgradeGUIs[i].upgradeSprites != null)
		{
			for (int j = 0; j < shop.m_upgradeGUIs[i].upgradeSprites.Length; j++)
			{
				shop.m_upgradeGUIs[i].upgradeSprites[j].hidden = true;
			}
		}
		if (shop.m_upgradeGUIs != null && shop.m_upgradeGUIs[i].m_ProgressBar != null)
		{
			shop.m_upgradeGUIs[i].m_ProgressBar.hidden = true;
			shop.m_upgradeGUIs[i].m_ProgressBarFrame.hidden = true;
		}
	}

	private void ClearAllShops()
	{
		ClearShop(ref m_getTokensShop);
		ClearShop(ref m_gadgetShop);
		ClearShop(ref m_upgradeShop);
	}

	private void ClearShop(ref ShopSubMenu shop)
	{
		if (shop.m_scroll != null)
		{
			shop.m_scroll.Clear();
		}
	}

	public void DisableAllShopButtons(ref ShopSubMenu shop, bool bDisable)
	{
		if (shop.m_shopItemButtons != null)
		{
			for (int i = 0; i < shop.m_shopItemButtons.Length; i++)
			{
				shop.m_shopItemButtons[i].disabled = bDisable;
			}
		}
	}

	public void ShowGadgetShop()
	{
		m_gadgetShop.m_shopFrame.hidden = false;
		if (m_gadgetShop.m_scroll != null)
		{
			m_gadgetShop.m_scroll.hidden = false;
		}
		if (m_gadgetShop.m_scrollbarPipe != null)
		{
			m_gadgetShop.m_scrollbarPipe.hidden = false;
		}
		if (m_gadgetShop.m_scrollbar != null)
		{
			m_gadgetShop.m_scrollbar.hidden = false;
		}
		m_GadgetStoreDescription.hidden = false;
		m_GadgetStoreDescription.text = LocalTextManager.GetStoreItemText("_GADGET_STORE_DESC_");
		if (m_gadgetShop.m_shopItemFrames == null)
		{
			return;
		}
		RefreshShopPrices(m_gadgetShop);
		for (int i = 0; i < m_gadgetShop.m_shopItemFrames.Length; i++)
		{
			ShowShopItemElement(ref m_gadgetShop, i);
			if (m_gadgetShop.m_upgradeGUIs != null && m_gadgetShop.m_upgradeGUIs[i].m_ProgressBar != null)
			{
				UpdateGadgetItemUpgradeSprites(i);
			}
		}
		m_currentScrollOffset = 0f;
		if (m_gadgetShop.m_scroll != null)
		{
			m_gadgetShop.m_scroll.scrollTo(0, false);
		}
	}

	public void ShowUpgradeShop()
	{
		m_upgradeShop.m_scroll.hidden = false;
		if (m_upgradeShop.m_scrollbarPipe != null)
		{
			m_upgradeShop.m_scrollbarPipe.hidden = false;
		}
		if (m_upgradeShop.m_scrollbar != null)
		{
			m_upgradeShop.m_scrollbar.hidden = false;
		}
		m_upgradeShop.m_shopFrame.hidden = false;
		if (m_upgradeShop.m_shopItemFrames == null)
		{
			return;
		}
		RefreshShopPrices(m_upgradeShop);
		for (int i = 0; i < m_upgradeShop.m_shopItemFrames.Length; i++)
		{
			ShowShopItemElement(ref m_upgradeShop, i);
			if (m_upgradeShop.m_upgradeGUIs != null && m_upgradeShop.m_upgradeGUIs[i].upgradeSprites != null)
			{
				UpdateUpgradeItemUpgradeSprites(i);
			}
		}
		m_currentScrollOffset = 0f;
		m_upgradeShop.m_scroll.scrollTo(0, false);
	}

	public void ShowGetTokenShop()
	{
		m_getTokensShop.m_scroll.hidden = false;
		if (m_getTokensShop.m_scrollbarPipe != null)
		{
			m_getTokensShop.m_scrollbarPipe.hidden = false;
		}
		m_getTokensShop.m_shopFrame.hidden = false;
		if (m_getTokensShop.m_shopItemFrames != null)
		{
			RefreshShopPrices(m_getTokensShop);
			RefreshGetTokenShopNames();
			if (StoreManager.The.isWaitingForPurchase)
			{
				DisableAllShopButtons(ref m_getTokensShop, true);
			}
			else
			{
				DisableAllShopButtons(ref m_getTokensShop, false);
			}
			DebugManager.Log("showing gettokensshopelements");
			for (int i = 0; i < m_getTokensShop.m_shopItemFrames.Length; i++)
			{
				ShowShopItemElement(ref m_getTokensShop, i);
			}
		}
		m_currentScrollOffset = 0f;
		m_getTokensShop.m_scroll.scrollTo(0, false);
	}

	public void ShowFreeTokensShop()
	{
		m_freeTokensShop.m_shopFrame.hidden = false;
		if (m_freeTokensShop.m_shopItemFrames == null || m_freeTokensShop.m_shopItemFrames.Length <= 0)
		{
			return;
		}
		for (int i = 0; i < m_freeTokensShop.m_shopItemFrames.Length; i++)
		{
			if (m_freeTokensShop.m_itemTitles[i].text != null)
			{
				string uIText = LocalTextManager.GetUIText(TOKEN_SHOP_TITLES[i]);
				m_freeTokensShop.m_itemTitles[i].text = uIText;
				DownSizeTextToWidth(ref m_freeTokensShop.m_itemTitles[i], m_freeTokensShop.m_shopItemFrames[i].width * m_freeTokensShop.m_shopItemFrames[i].scale.x - 12f);
				m_freeTokensShop.m_itemTitles[i].hidden = false;
			}
			if (m_freeTokensShop.m_itemDescriptions[i].text != null)
			{
				string uIText2 = LocalTextManager.GetUIText(TOKEN_SHOP_DESC[i]);
				m_freeTokensShop.m_itemDescriptions[i].text = uIText2;
				m_freeTokensShop.m_itemDescriptions[i].hidden = false;
			}
			if (m_freeTokensShop.m_itemPrices[i].text != null)
			{
				string uIText3 = LocalTextManager.GetUIText("_FREE_");
				m_freeTokensShop.m_itemPrices[i].text = uIText3;
				DownSizeTextToWidth(ref m_freeTokensShop.m_itemPrices[i], m_freeTokensShop.m_shopItemButtons[i].width * m_freeTokensShop.m_shopItemButtons[i].scale.x - 20f);
			}
			m_freeTokensShop.m_shopItemFrames[i].hidden = false;
			if (m_freeTokensShop.m_icons[i] != null)
			{
				m_freeTokensShop.m_icons[i].hidden = false;
			}
			if (m_freeTokensShop.m_shopItemButtons[i] != null)
			{
				m_freeTokensShop.m_shopItemButtons[i].hidden = false;
				m_freeTokensShop.m_itemPrices[i].hidden = false;
			}
		}
	}

	public void ShowShopItemElement(ref ShopSubMenu shop, int i)
	{
		shop.m_shopItemFrames[i].hidden = false;
		shop.m_itemTitles[i].hidden = false;
		shop.m_itemDescriptions[i].hidden = false;
		if (shop.m_icons[i] != null)
		{
			shop.m_icons[i].hidden = false;
		}
		if (shop.m_shopItemButtons[i] != null)
		{
			PurchasableItem purchasableItem = (PurchasableItem)shop.m_shopItemFrames[i].userData;
			if (purchasableItem.m_owned)
			{
				shop.m_itemPrices[i].hidden = true;
				shop.m_shopItemButtons[i].hidden = true;
				shop.m_buyItemExtraText[i].hidden = true;
			}
			else
			{
				shop.m_shopItemButtons[i].hidden = false;
				shop.m_itemPrices[i].hidden = false;
				shop.m_buyItemExtraText[i].hidden = false;
			}
		}
	}

	public void GoToGetTokenShop()
	{
		m_currentMenuState = SHOP_MENU_STATE.GET_TOKENS;
		StoreGUIManagerPersistentElements.The.SelectActiveStoreButton();
		if (!StoreManager.The.isWaitingForProducts)
		{
			StoreManager.The.GetProducts(OnConnectToGetTokens);
		}
		m_getTokensShop.m_scroll.scrollTo(0, false);
		HideShop(ref m_upgradeShop);
		HideShop(ref m_gadgetShop);
		HideShop(ref m_freeTokensShop);
	}

	public void GoToGadgetShop()
	{
		m_currentMenuState = SHOP_MENU_STATE.GADGETS;
		StoreGUIManagerPersistentElements.The.SelectActiveStoreButton();
		ShowGadgetShop();
		HideShop(ref m_upgradeShop);
		HideShop(ref m_getTokensShop);
		HideShop(ref m_freeTokensShop);
	}

	public void GoToUpgradeShop()
	{
		m_currentMenuState = SHOP_MENU_STATE.UPGRADES;
		StoreGUIManagerPersistentElements.The.SelectActiveStoreButton();
		ShowUpgradeShop();
		m_upgradeShop.m_scroll.scrollTo(0, false);
		HideShop(ref m_gadgetShop);
		HideShop(ref m_getTokensShop);
		HideShop(ref m_freeTokensShop);
	}

	public void GoToFreeTokensShop()
	{
		m_currentMenuState = SHOP_MENU_STATE.FREETOKENS;
		StoreGUIManagerPersistentElements.The.SelectActiveStoreButton();
		ShowFreeTokensShop();
		HideShop(ref m_gadgetShop);
		HideShop(ref m_getTokensShop);
		HideShop(ref m_upgradeShop);
	}

	public void ShowCantConnectText()
	{
		m_noStoreItemsTitleText.hidden = false;
		m_noStoreItemsBodyText.hidden = false;
		HideShop(ref m_gadgetShop);
		HideShop(ref m_getTokensShop);
		HideShop(ref m_upgradeShop);
	}

	public void RefreshGetTokenShopNames()
	{
		if (m_getTokensShop.m_shopItemFrames == null || m_getTokensShop.m_shopItemFrames.Length <= 0)
		{
			return;
		}
		for (int i = 0; i < m_getTokensShop.m_shopItemFrames.Length; i++)
		{
			if (m_getTokensShop.m_itemTitles[i].text != null)
			{
				m_getTokensShop.m_itemTitles[i].text = LocalTextManager.GetStoreItemText(m_getTokensShop.m_itemTitlesKeys[i]);
				DownSizeTextToWidth(ref m_getTokensShop.m_itemTitles[i], m_getTokensShop.m_shopItemFrames[i].width * m_getTokensShop.m_shopItemFrames[i].scale.x - 12f);
			}
			if (m_getTokensShop.m_itemDescriptions[i].text != null)
			{
				m_getTokensShop.m_itemDescriptions[i].text = LocalTextManager.GetStoreItemText(m_getTokensShop.m_itemDescriptionsKeys[i]);
			}
		}
	}

	public void RefreshShopPrices(ShopSubMenu shop)
	{
		if (shop.m_shopItemFrames == null || shop.m_shopItemFrames.Length <= 0)
		{
			return;
		}
		for (int i = 0; i < shop.m_shopItemFrames.Length; i++)
		{
			PurchasableItem purchasableItem = (PurchasableItem)shop.m_shopItemFrames[i].userData;
			float num = 1f;
			if (shop.m_itemTitles.Length > i && shop.m_itemDescriptions.Length > i)
			{
				if (shop.m_itemTitles[i] != null && shop.m_itemDescriptions != null)
				{
					shop.m_itemTitles[i].text = purchasableItem.m_name;
					shop.m_itemDescriptions[i].text = purchasableItem.m_desc;
					DownSizeTextToWidth(ref shop.m_itemTitles[i], shop.m_shopItemFrames[i].width * shop.m_shopItemFrames[i].scale.x - 12f);
					DownSizeTextToWidth(ref shop.m_itemDescriptions[i], shop.m_shopItemFrames[i].width * shop.m_shopItemFrames[i].scale.x - 12f);
					num = ((GameManager.The.aspectRatio.x == 3f || GameManager.The.aspectRatio.y == 4f) ? 0.3f : ((LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.English) ? 0.25f : 0.2f));
					if (GameManager.The.aspectRatio.x == 3f && GameManager.The.aspectRatio.y == 4f && LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
					{
						num = 0.25f;
					}
				}
				if (UI.scaleFactor == 1)
				{
					num /= 1.5f;
				}
				shop.m_itemDescriptions[i].textScale = num * UIHelper.CalcFontScale();
			}
			if (shop.m_shopItemButtons.Length > i && shop.m_itemPrices.Length > i && shop.m_shopItemButtons[i] != null && shop.m_itemPrices[i] != null && !purchasableItem.m_owned)
			{
				if (purchasableItem.m_tokenCost > 0)
				{
					shop.m_itemPrices[i].text = purchasableItem.m_tokenCost.ToString("N0");
				}
				else if (purchasableItem.m_fedoraCost > 0)
				{
					shop.m_itemPrices[i].text = purchasableItem.m_fedoraCost.ToString("N0");
				}
				else
				{
					shop.m_itemPrices[i].text = purchasableItem.m_realMoneyCost.ToString("N2");
				}
				DownSizeTextToWidth(ref shop.m_itemPrices[i], shop.m_shopItemButtons[i].width * shop.m_shopItemButtons[i].scale.x - 20f);
				if (purchasableItem.m_buyExtraText != null)
				{
					shop.m_buyItemExtraText[i].text = purchasableItem.m_buyExtraText;
				}
				else
				{
					shop.m_buyItemExtraText[i].text = string.Empty;
				}
			}
		}
	}

	private void RefreshStoreScroll(ref UIScrollableVerticalLayoutNoClip storeScroll, float height)
	{
		float width = storeScroll.width;
		height = Screen.height;
		storeScroll.setSize(width, height);
		storeScroll.positionFromTop(0f);
	}

	private void UpdateGadgetItemUpgradeSprites(int i)
	{
		if (m_gadgetShop.m_upgradeGUIs == null || m_gadgetShop.m_upgradeGUIs[i].m_ProgressBar == null)
		{
			return;
		}
		PurchasableGadgetItem purchasableGadgetItem = (PurchasableGadgetItem)m_gadgetShop.m_shopItemFrames[i].userData;
		string spriteImage = "CashSaleButton.png";
		string highlightedImage = "CashSaleButtonOver.png";
		string text;
		if (purchasableGadgetItem.m_tokenCost > 0)
		{
			text = purchasableGadgetItem.m_tokenCost.ToString("N0");
			spriteImage = "TokenSaleButton.png";
			highlightedImage = "TokenSaleButtonOver.png";
		}
		else if (purchasableGadgetItem.m_fedoraCost <= 0)
		{
			text = ((!(purchasableGadgetItem.m_realMoneyCost > 0f)) ? "0.00" : purchasableGadgetItem.m_realMoneyCost.ToString("N2"));
		}
		else
		{
			text = purchasableGadgetItem.m_fedoraCost.ToString("N0");
			spriteImage = "FedoraSaleButton.png";
			highlightedImage = "FedoraSaleButtonOver.png";
		}
		m_gadgetShop.m_shopItemButtons[i].setSpriteImage(spriteImage);
		m_gadgetShop.m_shopItemButtons[i].SetHighlightedImage(highlightedImage);
		m_gadgetShop.m_shopItemButtons[i].highlighted = false;
		m_gadgetShop.m_itemPrices[i].text = text;
		m_gadgetShop.m_itemPrices[i].parentUIObject = m_gadgetShop.m_shopItemButtons[i];
		if (purchasableGadgetItem.m_fedoraCost > 0)
		{
			m_gadgetShop.m_itemPrices[i].positionFromBottomLeft(0.3f, 0.15f);
		}
		else
		{
			m_gadgetShop.m_itemPrices[i].positionFromBottomRight(0.3f, 0.09f);
		}
		m_gadgetShop.m_itemPrices[i].parentUIObject = m_gadgetShop.m_shopItemFrames[i];
		if (!purchasableGadgetItem.hasBoughtGadget)
		{
			m_gadgetShop.m_upgradeGUIs[i].m_ProgressBar.hidden = true;
			m_gadgetShop.m_upgradeGUIs[i].m_ProgressBarFrame.hidden = true;
			return;
		}
		m_GadgetStoreDescription.text = LocalTextManager.GetStoreItemText("_GADGET_SUPERCHARGE_");
		m_gadgetShop.m_upgradeGUIs[i].m_ProgressBar.hidden = false;
		m_gadgetShop.m_upgradeGUIs[i].m_ProgressBarFrame.hidden = false;
		m_gadgetShop.m_buyItemExtraText[i].positionFromRight(0.01f, 0.03f);
		if (purchasableGadgetItem.m_maxUpgrades == 0)
		{
			m_gadgetShop.m_upgradeGUIs[i].m_ProgressBar.value = 1f;
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_gadgetShop.m_upgradeGUIs[i].m_ProgressBar);
			return;
		}
		for (int j = 0; j <= purchasableGadgetItem.m_maxUpgrades; j++)
		{
			if (purchasableGadgetItem.UpgradeNums >= j)
			{
				m_gadgetShop.m_upgradeGUIs[i].m_ProgressBar.value = (float)j / (float)purchasableGadgetItem.m_maxUpgrades;
				GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_gadgetShop.m_upgradeGUIs[i].m_ProgressBar);
			}
		}
	}

	private void UpdateUpgradeItemUpgradeSprites(int i)
	{
		if (m_upgradeShop.m_upgradeGUIs == null || m_upgradeShop.m_upgradeGUIs[i].upgradeSprites == null)
		{
			return;
		}
		PurchasableItem purchasableItem = (PurchasableItem)m_upgradeShop.m_shopItemFrames[i].userData;
		string spriteImage = "CashSaleButton.png";
		string highlightedImage = "CashSaleButtonOver.png";
		string text;
		if (purchasableItem.m_tokenCost > 0)
		{
			text = purchasableItem.m_tokenCost.ToString("N0");
			spriteImage = "TokenSaleButton.png";
			highlightedImage = "TokenSaleButtonOver.png";
		}
		else if (purchasableItem.m_fedoraCost <= 0)
		{
			text = ((!(purchasableItem.m_realMoneyCost > 0f)) ? "0.00" : purchasableItem.m_realMoneyCost.ToString("N2"));
		}
		else
		{
			text = purchasableItem.m_fedoraCost.ToString("N0");
			spriteImage = "FedoraSaleButton.png";
			highlightedImage = "FedoraSaleButtonOver.png";
		}
		m_upgradeShop.m_shopItemButtons[i].setSpriteImage(spriteImage);
		m_upgradeShop.m_shopItemButtons[i].SetHighlightedImage(highlightedImage);
		m_upgradeShop.m_shopItemButtons[i].highlighted = false;
		m_upgradeShop.m_itemPrices[i].text = text;
		m_upgradeShop.m_itemPrices[i].parentUIObject = m_upgradeShop.m_shopItemButtons[i];
		if (purchasableItem.m_fedoraCost > 0)
		{
			m_upgradeShop.m_itemPrices[i].positionFromBottomLeft(0.3f, 0.15f);
		}
		else
		{
			m_upgradeShop.m_itemPrices[i].positionFromBottomRight(0.3f, 0.09f);
		}
		m_upgradeShop.m_itemPrices[i].parentUIObject = m_upgradeShop.m_shopItemFrames[i];
		for (int j = 0; j < m_upgradeShop.m_upgradeGUIs[i].upgradeSprites.Length; j++)
		{
			m_upgradeShop.m_upgradeGUIs[i].upgradeSprites[j].hidden = false;
			UpgradableItem upgradableItem = (UpgradableItem)m_upgradeShop.m_shopItemFrames[i].userData;
			if (upgradableItem.upgradesOwned > j)
			{
				m_upgradeShop.m_upgradeGUIs[i].upgradeSprites[j].selected = true;
				if (j >= m_upgradeShop.m_upgradeGUIs[i].upgradeSprites.Length - 1)
				{
					m_upgradeShop.m_shopItemButtons[i].hidden = true;
				}
			}
		}
	}

	private void DownSizeTextToWidth(ref UITextInstance txt, float width)
	{
	}

	private void ResizeTextToWidth(ref UITextInstance txt, float width)
	{
		float num = width / txt.width;
		txt.textScale *= num * UIHelper.CalcFontScale();
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

	public void onTouchUpInsideBuyGadgetItemButton(UIButton sender)
	{
		if (!b_buttonsDisabledbyScrollMovement && !PopUpGUIManager.The.isAPopupActive)
		{
			GameManager.The.PlayClip(AudioClipFiles.UIPURCHASE);
			DisableAllShopButtons(ref m_gadgetShop, true);
			DebugManager.Log("Attempting to buy item of Gadget Item Type!");
			UISprite uISprite = (UISprite)sender.parentUIObject;
			PurchasableGadgetItem purchasableGadgetItem = (PurchasableGadgetItem)uISprite.userData;
			if (purchasableGadgetItem == null)
			{
				DebugManager.Log("SOMETHING IS WRONG! ITEM PURCHASED IS NULL!");
			}
			else
			{
				MainMenuEventManager.TriggerBuyGadgetItem(purchasableGadgetItem);
			}
		}
	}

	public void onTouchUpInsideBuyUpgradeItemButton(UIButton sender)
	{
		if (!b_buttonsDisabledbyScrollMovement && !PopUpGUIManager.The.isAPopupActive)
		{
			GameManager.The.PlayClip(AudioClipFiles.UIPURCHASE);
			DisableAllShopButtons(ref m_upgradeShop, true);
			DebugManager.Log("Attempting to buy item of Upgrade Item Type!");
			UISprite uISprite = (UISprite)sender.parentUIObject;
			UpgradableItem upgradableItem = (UpgradableItem)uISprite.userData;
			if (upgradableItem == null)
			{
				DebugManager.Log("SOMETHING IS WRONG! ITEM PURCHASED IS NULL!");
			}
			else
			{
				MainMenuEventManager.TriggerBuyUpgradeItem(upgradableItem);
			}
			if (m_upgradeShop.m_scroll != null && m_getTokensShop.m_scroll != null)
			{
				m_upgradeShop.m_scroll.refreshClip();
				m_getTokensShop.m_scroll.refreshClip();
			}
			sender.disabled = false;
		}
	}

	public void onTouchUpInsideBuyMiscItemButton(UIButton sender)
	{
		if (!b_buttonsDisabledbyScrollMovement && !PopUpGUIManager.The.isAPopupActive)
		{
			GameManager.The.PlayClip(AudioClipFiles.UIPURCHASE);
			sender.disabled = true;
			DebugManager.Log("Attempting to buy item of Misc Item Type!");
			UISprite uISprite = (UISprite)sender.parentUIObject;
			PurchasableItem purchasableItem = (PurchasableItem)uISprite.userData;
			if (purchasableItem == null)
			{
				DebugManager.Log("SOMETHING IS WRONG! ITEM PURCHASED IS NULL!");
			}
			else
			{
				MainMenuEventManager.TriggerBuyPurchasableItem(purchasableItem);
			}
			if (m_upgradeShop.m_scroll != null && m_getTokensShop.m_scroll != null)
			{
				m_upgradeShop.m_scroll.refreshClip();
				m_getTokensShop.m_scroll.refreshClip();
			}
			sender.disabled = false;
		}
	}

	public void onTouchUpInsideBuyGetTokensItemButton(UIButton sender)
	{
		if (!b_buttonsDisabledbyScrollMovement && !PopUpGUIManager.The.isAPopupActive && !m_waitingForPurchase)
		{
			GameManager.The.PlayClip(AudioClipFiles.UIPURCHASE);
			m_waitingForPurchase = true;
			DisableAllShopButtons(ref m_getTokensShop, true);
			DebugManager.Log("Attempting to buy item of Token Item Type!");
			UISprite uISprite = (UISprite)sender.parentUIObject;
			PurchasableItem purchasableItem = (PurchasableItem)uISprite.userData;
			if (purchasableItem == null)
			{
				DebugManager.Log("SOMETHING IS WRONG! ITEM PURCHASED IS NULL!");
			}
			if (purchasableItem.m_realMoneyCost > 0f)
			{
				sender.disabled = false;
				MainMenuEventManager.TriggerBuyTokenItem(purchasableItem);
			}
			else
			{
				DisableAllShopButtons(ref m_getTokensShop, false);
				m_waitingForPurchase = false;
			}
			if (m_upgradeShop.m_scroll != null && m_getTokensShop.m_scroll != null)
			{
				m_upgradeShop.m_scroll.refreshClip();
				m_getTokensShop.m_scroll.refreshClip();
			}
		}
	}

	public void onTouchUpInsideFreeItemsButton(UIButton sender)
	{
		if (!b_buttonsDisabledbyScrollMovement && !PopUpGUIManager.The.isAPopupActive)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				DisableAllShopButtons(ref m_freeTokensShop, true);
				PopUpGUIManager.The.ShowNoInternetNotificationPopup(OnNoInternetPopUpDismissed, delegate
				{
				});
				return;
			}
			DisableAllShopButtons(ref m_freeTokensShop, true);
			GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		}
	}

	public void onTouchUpInsideTokenAd(UIButton sender)
	{
		if (!b_buttonsDisabledbyScrollMovement && !PopUpGUIManager.The.isAPopupActive)
		{
			TokenAdsManager tokenAdsManager = Object.FindObjectOfType(typeof(TokenAdsManager)) as TokenAdsManager;
			if (tokenAdsManager != null)
			{
				tokenAdsManager.ShowOffers();
			}
		}
	}
	public void onTouchUpInsideFreeTokensButton(UIButton sender)
	{
		if (!b_buttonsDisabledbyScrollMovement && !PopUpGUIManager.The.isAPopupActive)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				DisableAllShopButtons(ref m_freeTokensShop, true);
				PopUpGUIManager.The.ShowNoInternetNotificationPopup(OnNoInternetPopUpDismissed, delegate
				{
				});
			}
			else
			{
				DisableAllShopButtons(ref m_freeTokensShop, true);
				GameManager.The.PlayClip(AudioClipFiles.UICLICK);
				#if UNITY_ANDROID
				SuperSonicAndroid.showBrandConnect();
				#endif
			}
		}
	}

	private void OnNoInternetPopUpDismissed(object o)
	{
		DisableAllShopButtons(ref m_freeTokensShop, false);
	}

	private void OnPopUpDisplay()
	{
		if (m_currentMenuState == SHOP_MENU_STATE.GADGETS)
		{
			HideShop(ref m_gadgetShop);
		}
		else if (m_currentMenuState == SHOP_MENU_STATE.UPGRADES)
		{
			HideShop(ref m_upgradeShop);
		}
		else if (m_currentMenuState == SHOP_MENU_STATE.GET_TOKENS)
		{
			HideShop(ref m_getTokensShop);
		}
	}

	public void CantBuyPopUpCancel()
	{
		DebugManager.Log("CantBuyPopUpCancel");
		if (m_currentMenuState == SHOP_MENU_STATE.GADGETS)
		{
			ShowGadgetShop();
		}
		else if (m_currentMenuState == SHOP_MENU_STATE.UPGRADES)
		{
			ShowUpgradeShop();
		}
		else if (m_currentMenuState == SHOP_MENU_STATE.GET_TOKENS)
		{
			ShowGetTokenShop();
			m_waitingForPurchase = false;
		}
	}

	public void CantBuyPopUpDone()
	{
		DebugManager.Log("CantBuyPopUpDone");
		if (m_currentMenuState == SHOP_MENU_STATE.GADGETS)
		{
			ShowGadgetShop();
		}
		else if (m_currentMenuState == SHOP_MENU_STATE.UPGRADES)
		{
			ShowUpgradeShop();
		}
		else if (m_currentMenuState == SHOP_MENU_STATE.GET_TOKENS)
		{
			ShowGetTokenShop();
			m_waitingForPurchase = false;
		}
	}

	private void PurchaseCancelled(string error)
	{
		DebugManager.Log("Purchase Cancelled");
		DisableAllShopButtons(ref m_freeTokensShop, false);
		DisableAllShopButtons(ref m_gadgetShop, false);
		DisableAllShopButtons(ref m_getTokensShop, false);
		DisableAllShopButtons(ref m_upgradeShop, false);
		StoreGUIManagerPersistentElements.The.DisableAllButtons(false);
		m_waitingForPurchase = false;
	}

	public void ReloadStaticText()
	{
		m_noStoreItemsTitleText.text = UIHelper.WordWrap(LocalTextManager.GetUIText("_GET_MORE_TOKEN_FED_TITLE_"), 18);
		m_noStoreItemsBodyText.text = UIHelper.WordWrap(LocalTextManager.GetUIText("_CONNECT_TO_INTERNET_"), 22);
		float num = 1f;
		if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f)
		{
			num = 0.3f;
			if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.German)
			{
				num = 0.17f;
			}
			if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Japanese || LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Chinese)
			{
				num = 0.13f;
			}
		}
		else
		{
			if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
			{
				num = 0.3f;
			}
			if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Japanese || LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Chinese)
			{
				num = 0.22f;
			}
		}
		if (UI.scaleFactor == 1)
		{
			num /= 2f;
		}
		m_noStoreItemsTitleText.textScale = num * UIHelper.CalcFontScale();
		m_noStoreItemsBodyText.textScale = num * UIHelper.CalcFontScale();
		float num2 = 0.5f;
		if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f)
		{
			num2 = 0.3f;
			if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.German)
			{
				num2 = 0.17f;
			}
			if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Japanese || LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Chinese)
			{
				num2 = 0.13f;
			}
		}
		else
		{
			if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
			{
				num2 = 0.3f;
			}
			if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Japanese || LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Chinese)
			{
				num2 = 0.22f;
			}
		}
		if (UI.scaleFactor == 1)
		{
			num2 /= 2f;
		}
		m_GadgetStoreDescription.textScale = num2 * UIHelper.CalcFontScale();
	}
}

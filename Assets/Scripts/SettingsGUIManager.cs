using UnityEngine;
using UnityEngine.UI;

public class SettingsGUIManager : MonoBehaviour
{
	private UISprite m_popUpFrame;

	private float m_popUpScale = 1f;

	private UITextInstance m_topBarTitle;

	private UIButton m_backButton;

	private UIVerticalLayout m_mainButtonLayout;

	private UIButton m_languageButton;

	private UIButton m_fbLogoutButton;

	private UITextInstance m_fbLogoutText;

	private UITextInstance m_languageText;

	private UIButton m_infoButton;

	private UITextInstance m_infoButtonText;

	private UIToggleButton m_soundToggleButton;

	private UIToggleButton m_musicToggleButton;

	private UISprite m_redSlash1;

	private UISprite m_redSlash2;

	private UITextInstance m_publishedByText;

	private UITextInstance m_publisherText;

	private UITextInstance m_developedByText;

	private UITextInstance m_developerText;

	private UITextInstance m_versionText;

	private UIButton m_publisherLogo;

	private UIButton m_developerLogo;

	private UIButton m_developerTextButton;

	private UIButton m_publisherTextButton;

	private UITextInstance m_DisneyText;

	private UITextInstance m_fontByText;

	private UITextInstance m_legalTitle;

	private UITextInstance m_TOSText;

	private UIButton m_TOSButton;

	private UITextInstance m_privacyPolicyText;

	private UIButton m_privacyButton;

	private UIButton m_faqButton;

	private UITextInstance m_faqText;

	private UIToggleButton m_iCloudButton;

	private UITextInstance m_iCloudText;

	private static int m_settingsDepth;

	private static SettingsGUIManager m_the;

	public RawImage m_popUpBG;

	public static SettingsGUIManager The
	{
		get
		{
			if (m_the == null)
			{
				GameObject gameObject = new GameObject("SettingsGUIManager");
				((SettingsGUIManager)gameObject.AddComponent<SettingsGUIManager>()).Init();
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
			InitPopUpFrame();
			InitTopBarTitle();
			InitBackButton();
			InitSettingsTopLevelMenu();
			InitInfoMenu();
			HideAll(true);
		}
	}

	private void InitPopUpFrame()
	{
		m_popUpFrame = GlobalGUIManager.The.m_menuToolkit.addSprite("SettingsMenu.png", 0, 0, m_settingsDepth + 4, true);
		m_popUpFrame.pixelsFromTopLeft(1, 100);
		m_popUpFrame.positionCenter();
		int num = 40;
		float num2 = 1f;
		if (m_popUpFrame.width > (float)(Screen.width - num))
		{
			int num3 = Screen.width - num;
			num2 = (float)num3 / m_popUpFrame.width;
		}
		m_popUpFrame.scale = new Vector3(num2, num2, 1f);
		m_popUpScale = num2;
	}

	private void InitTopBarTitle()
	{
		m_topBarTitle = GlobalGUIManager.The.defaultTextAlt.addTextInstance("DEFAULT TITLE", 0f, 0f, 1f, m_settingsDepth + 2);
		m_topBarTitle.setColorForAllLetters(Color.white);
		m_topBarTitle.alignMode = UITextAlignMode.Center;
		m_topBarTitle.textScale = 0.8f * m_popUpScale;
		m_topBarTitle.parentUIObject = m_popUpFrame;
		m_topBarTitle.positionFromTop(0.05f);
		m_topBarTitle.text = "DEFAULT TITLE";
	}

	private void InitBackButton()
	{
		m_backButton = UIButton.create(GlobalGUIManager.The.m_hudToolkit, "BackButton.png", "BackButtonOver.png", 0, 0, m_settingsDepth + 2);
		m_backButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_backButton.scale = new Vector3(m_popUpScale, m_popUpScale, 1f);
		m_backButton.parentUIObject = m_popUpFrame;
		m_backButton.positionFromTopLeft(0.04f, 0.04f);
		m_backButton.onTouchUpInside += onTouchUpInsideBackButton;
	}

	private void InitSettingsTopLevelMenu()
	{
		int spacing = -10;
		if (UI.scaleFactor == 2)
		{
			spacing = -20;
		}
		if (UI.scaleFactor == 4)
		{
			spacing = -40;
		}
		m_mainButtonLayout = new UIVerticalLayout(spacing);
		m_mainButtonLayout.parentUIObject = m_popUpFrame;
		m_mainButtonLayout.alignMode = UIAbstractContainer.UIContainerAlignMode.Center;
		m_mainButtonLayout.positionFromTop(0.2f);
		m_mainButtonLayout.parentUIObject = null;
		m_languageButton = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "SettingsScreenButton.png", "SettingsScreenButtonOver.png", 0, 0, m_settingsDepth + 2);
		m_languageButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_languageButton.scale = new Vector3(m_popUpScale, m_popUpScale, 1f);
		m_languageButton.onTouchUpInside += onTouchUpInsideLanguageButton;
		m_mainButtonLayout.addChild(m_languageButton);
		m_languageText = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_LANGUAGE_"), 0f, 0f, 1f, m_settingsDepth + 1);
		m_languageText.setColorForAllLetters(Color.black);
		m_languageText.alignMode = UITextAlignMode.Center;
		m_languageText.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_languageText.parentUIObject = m_languageButton;
		m_languageText.textScale = 0.7f * m_popUpScale;
		m_languageText.positionFromCenter(0f, 0f);
		m_languageText.text = LocalTextManager.GetUIText("_LANGUAGE_");
		m_infoButton = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "SettingsScreenButton.png", "SettingsScreenButtonOver.png", 0, 0, m_settingsDepth + 2);
		m_infoButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_infoButton.scale = new Vector3(m_popUpScale, m_popUpScale, 1f);
		m_infoButton.onTouchUpInside += onTouchUpInsideInfoButton;
		m_mainButtonLayout.addChild(m_infoButton);
		m_infoButtonText = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_INFO_"), 0f, 0f, 1f, m_settingsDepth + 1);
		m_infoButtonText.setColorForAllLetters(Color.black);
		m_infoButtonText.alignMode = UITextAlignMode.Center;
		m_infoButtonText.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_infoButtonText.parentUIObject = m_infoButton;
		m_infoButtonText.textScale = 0.7f * m_popUpScale;
		m_infoButtonText.positionFromCenter(0f, 0f);
		m_infoButtonText.text = LocalTextManager.GetUIText("_INFO_");
		m_soundToggleButton = UIToggleButton.create(GlobalGUIManager.The.m_menuToolkit, "MusicButton.png", "MusicButton.png", "MusicButtonOver.png", 0, 0, m_settingsDepth + 2);
		m_soundToggleButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_soundToggleButton.parentUIObject = m_popUpFrame;
		m_soundToggleButton.scale = new Vector3(m_popUpScale, m_popUpScale, 1f);
		m_soundToggleButton.positionFromBottom(0.02f, -0.15f);
		m_soundToggleButton.onToggle += onToggleSoundButton;
		m_musicToggleButton = UIToggleButton.create(GlobalGUIManager.The.m_menuToolkit, "SoundButton.png", "SoundButton.png", "SoundButtonOver.png", 0, 0, m_settingsDepth + 2);
		m_musicToggleButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_musicToggleButton.parentUIObject = m_popUpFrame;
		m_musicToggleButton.scale = new Vector3(m_popUpScale, m_popUpScale, 1f);
		m_musicToggleButton.positionFromBottom(0.02f, 0.15f);
		m_musicToggleButton.onToggle += onToggleMusicButton;
		m_redSlash1 = GlobalGUIManager.The.m_menuToolkit.addSprite("RedSlash.png", 0, 0, m_settingsDepth + 1);
		m_redSlash1.parentUIObject = m_soundToggleButton;
		m_redSlash1.scale = new Vector3(m_popUpScale, m_popUpScale, 1f);
		m_redSlash1.positionFromCenter(0f, 0f);
		m_redSlash1.hidden = GameManager.The.g_SoundEnabled;
		m_redSlash2 = GlobalGUIManager.The.m_menuToolkit.addSprite("RedSlash.png", 0, 0, m_settingsDepth + 1);
		m_redSlash2.parentUIObject = m_musicToggleButton;
		m_redSlash2.scale = new Vector3(m_popUpScale, m_popUpScale, 1f);
		m_redSlash2.positionFromCenter(0f, 0f);
		m_redSlash2.hidden = GameManager.The.m_MusicEnabled;
	}

	public void InitInfoMenu()
	{
		m_publishedByText = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_PUBLISHED_BY_"), 0f, 0f, 1f, m_settingsDepth + 2);
		m_publishedByText.setColorForAllLetters(Color.white);
		m_publishedByText.alignMode = UITextAlignMode.Center;
		m_publishedByText.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_publishedByText.parentUIObject = m_popUpFrame;
		m_publishedByText.textScale = 0.6f * m_popUpScale;
		m_publishedByText.positionFromTop(0.25f, -0.25f);
		m_publishedByText.text = LocalTextManager.GetUIText("_PUBLISHED_BY_");
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_publishedByText.textScale = 0.4f * m_popUpScale;
		}
		m_publisherText = GlobalGUIManager.The.defaultTextAlt.addTextInstance("@SidekickLTD", 0f, 0f, 1f, m_settingsDepth + 2);
		m_publisherText.setColorForAllLetters(Color.black);
		m_publisherText.alignMode = UITextAlignMode.Center;
		m_publisherText.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_publisherText.parentUIObject = m_publishedByText;
		m_publisherText.textScale = 0.48f * m_popUpScale;
		m_publisherText.positionFromTop(1.3f, 0f);
		m_publisherText.text = "@SidekickLTD";
		float num = 0f;
		float num2 = 0f;
		foreach (UISprite textSprite in m_publisherText.textSprites)
		{
			if (num2 < textSprite.height)
			{
				num2 = textSprite.height;
			}
			num += textSprite.width;
		}
		m_publisherTextButton = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "SettingsScreenButton.png", "SettingsScreenButtonOver.png", 0, 0, m_settingsDepth + 3);
		m_publisherTextButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		float x = num / m_publisherTextButton.width;
		float y = num2 / m_publisherTextButton.height;
		m_publisherTextButton.scale = new Vector3(x, y, 1f);
		m_publisherTextButton.parentUIObject = m_publisherText;
		m_publisherTextButton.positionFromTopLeft(0f, 0f);
		m_publisherTextButton.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
		m_publisherTextButton.onTouchUpInside += onTouchUpInsidePublisherTextButton;
		m_developedByText = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_DEVELOPED_BY_"), 0f, 0f, 1f, m_settingsDepth + 2);
		m_developedByText.setColorForAllLetters(Color.white);
		m_developedByText.alignMode = UITextAlignMode.Center;
		m_developedByText.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_developedByText.parentUIObject = m_popUpFrame;
		m_developedByText.textScale = 0.6f * m_popUpScale;
		m_developedByText.positionFromTop(0.25f, 0.25f);
		m_developedByText.text = LocalTextManager.GetUIText("_DEVELOPED_BY_");
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_publishedByText.textScale = 0.4f * m_popUpScale;
		}
		m_developerText = GlobalGUIManager.The.defaultTextAlt.addTextInstance("@theoddgentlemen", 0f, 0f, 1f, m_settingsDepth + 2);
		m_developerText.setColorForAllLetters(Color.black);
		m_developerText.alignMode = UITextAlignMode.Center;
		m_developerText.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_developerText.parentUIObject = m_developedByText;
		m_developerText.textScale = 0.48f * m_popUpScale;
		m_developerText.positionFromTop(1.3f, -0.02f);
		m_developerText.text = "@theoddgentlemen";
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_developerText.textScale = 0.35f * m_popUpScale;
		}
		m_versionText = GlobalGUIManager.The.defaultTextAlt.addTextInstance("1.0.12", 0f, 0f, 1f, m_settingsDepth + 2);
		m_versionText.setColorForAllLetters(Color.white);
		m_versionText.alignMode = UITextAlignMode.Center;
		m_versionText.verticalAlignMode = UITextVerticalAlignMode.Bottom;
		m_versionText.parentUIObject = m_developedByText;
		m_versionText.textScale = 0.48f * m_popUpScale;
		m_versionText.text = "1.0.12";
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_versionText.textScale = 0.35f * m_popUpScale;
		}
		num = 0f;
		num2 = 0f;
		foreach (UISprite textSprite2 in m_developerText.textSprites)
		{
			if (num2 < textSprite2.height)
			{
				num2 = textSprite2.height;
			}
			num += textSprite2.width;
		}
		m_developerTextButton = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "SettingsScreenButton.png", "SettingsScreenButtonOver.png", 0, 0, m_settingsDepth + 3);
		m_developerTextButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		x = num / m_developerTextButton.width;
		y = num2 / m_developerTextButton.height;
		m_developerTextButton.scale = new Vector3(x, y, 1f);
		m_developerTextButton.parentUIObject = m_developerText;
		m_developerTextButton.positionFromTopLeft(0f, 0f);
		m_developerTextButton.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
		m_developerTextButton.onTouchUpInside += onTouchUpInsideDeveloperTextButton;
		m_developerLogo = UIButton.create(GlobalGUIManager.The.m_hudToolkit, "OddGentsLogoSmall.png", "OddGentsLogoSmall.png", 0, 0, m_settingsDepth + 2);
		m_developerLogo.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_developerLogo.scale = new Vector3(m_popUpScale, m_popUpScale, 1f);
		m_developerLogo.parentUIObject = m_developerText;
		m_developerLogo.positionFromTop(1.2f, 0f);
		m_developerLogo.onTouchUpInside += onTouchUpInsideOddGentlemenButton;
		m_publisherLogo = UIButton.create(GlobalGUIManager.The.m_hudToolkit, "MajescoLogoSmall.png", "MajescoLogoSmall.png", 0, 0, m_settingsDepth + 2);
		m_publisherLogo.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_publisherLogo.parentUIObject = m_developerLogo;
		m_publisherLogo.scale = new Vector3(m_popUpScale, m_popUpScale, 1f);
		m_publisherLogo.positionFromCenter(0f, -1.9f);
		m_publisherLogo.onTouchUpInside += onTouchUpInsideMajescoButton;
		m_DisneyText = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_LEGAL_COPYRIGHT_"), 0f, 0f, 1f, m_settingsDepth + 3);
		m_DisneyText.setColorForAllLetters(Color.black);
		m_DisneyText.alignMode = UITextAlignMode.Center;
		m_DisneyText.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_DisneyText.parentUIObject = m_popUpFrame;
		m_DisneyText.textScale = 0.35f * m_popUpScale;
		m_DisneyText.positionFromTop(0.9f);
		m_fontByText = GlobalGUIManager.The.defaultText.addTextInstance(LocalTextManager.GetUIText("_FONT_BY_"), 0f, 0f, 1f, m_settingsDepth + 3);
		m_fontByText.setColorForAllLetters(Color.black);
		m_fontByText.alignMode = UITextAlignMode.Center;
		m_fontByText.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_fontByText.parentUIObject = m_DisneyText;
		m_fontByText.textScale = 0.2f * m_popUpScale;
		m_fontByText.positionFromTop(1f, 0f);
		m_legalTitle = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_LEGAL_"), 0f, 0f, 1f, m_settingsDepth + 2);
		m_legalTitle.setColorForAllLetters(Color.white);
		m_legalTitle.alignMode = UITextAlignMode.Center;
		m_legalTitle.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_legalTitle.parentUIObject = m_popUpFrame;
		m_legalTitle.textScale = 0.5f * m_popUpScale;
		m_legalTitle.positionFromTop(0.66f);
		m_legalTitle.text = LocalTextManager.GetUIText("_LEGAL_");
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_legalTitle.textScale = 0.4f * m_popUpScale;
		}
		if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Russian)
		{
			m_legalTitle.textScale = 0.3f * m_popUpScale;
		}
		Color colorForAllLetters = new Color32(8, 129, 246, byte.MaxValue);
		m_TOSText = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_TERMS_OF_SERVICE_"), 0f, 0f, 1f, m_settingsDepth + 2);
		m_TOSText.setColorForAllLetters(colorForAllLetters);
		m_TOSText.alignMode = UITextAlignMode.Center;
		m_TOSText.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_TOSText.parentUIObject = m_legalTitle;
		m_TOSText.textScale = 0.5f * m_popUpScale;
		m_TOSText.positionFromTop(1.3f, 0f);
		m_TOSText.text = LocalTextManager.GetUIText("_TERMS_OF_SERVICE_");
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_TOSText.textScale = 0.35f * m_popUpScale;
		}
		if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Russian)
		{
			m_TOSText.textScale = 0.3f * m_popUpScale;
		}
		num = 0f;
		num2 = 0f;
		foreach (UISprite textSprite3 in m_TOSText.textSprites)
		{
			if (num2 < textSprite3.height)
			{
				num2 = textSprite3.height;
			}
			num += textSprite3.width;
		}
		m_TOSButton = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "SettingsScreenButton.png", "SettingsScreenButtonOver.png", 0, 0, m_settingsDepth + 3);
		m_TOSButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		x = num / m_TOSButton.width;
		y = num2 / m_TOSButton.height;
		m_TOSButton.scale = new Vector3(x, y, 1f);
		m_TOSButton.parentUIObject = m_TOSText;
		m_TOSButton.positionFromTopLeft(0f, 0f);
		m_TOSButton.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
		m_TOSButton.onTouchUpInside += onTouchUpInsideTOSButton;
		m_privacyPolicyText = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_PRIVACY_POLICY_"), 0f, 0f, 1f, m_settingsDepth + 2);
		m_privacyPolicyText.setColorForAllLetters(colorForAllLetters);
		m_privacyPolicyText.alignMode = UITextAlignMode.Center;
		m_privacyPolicyText.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_privacyPolicyText.parentUIObject = m_legalTitle;
		m_privacyPolicyText.textScale = 0.5f * m_popUpScale;
		m_privacyPolicyText.positionFromTop(2.5f, 0f);
		m_privacyPolicyText.text = LocalTextManager.GetUIText("_PRIVACY_POLICY_");
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_privacyPolicyText.textScale = 0.35f * m_popUpScale;
		}
		if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Russian)
		{
			m_privacyPolicyText.textScale = 0.25f * m_popUpScale;
		}
		num = 0f;
		num2 = 0f;
		foreach (UISprite textSprite4 in m_privacyPolicyText.textSprites)
		{
			if (num2 < textSprite4.height)
			{
				num2 = textSprite4.height;
			}
			num += textSprite4.width;
		}
		m_privacyButton = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "SettingsScreenButton.png", "SettingsScreenButtonOver.png", 0, 0, m_settingsDepth + 3);
		m_privacyButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		x = num / m_privacyButton.width;
		y = num2 / m_privacyButton.height;
		m_privacyButton.scale = new Vector3(x, y, 1f);
		m_privacyButton.parentUIObject = m_privacyPolicyText;
		m_privacyButton.positionFromTopLeft(0f, 0f);
		m_privacyButton.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
		m_privacyButton.onTouchUpInside += onTouchUpInsidePrivacyPolicyButton;
		m_faqText = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_FAQ_"), 0f, 0f, 1f, m_settingsDepth + 2);
		m_faqText.setColorForAllLetters(colorForAllLetters);
		m_faqText.alignMode = UITextAlignMode.Center;
		m_faqText.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_faqText.parentUIObject = m_legalTitle;
		m_faqText.textScale = 0.5f * m_popUpScale;
		m_faqText.positionFromTop(4f, 0f);
		m_faqText.text = LocalTextManager.GetUIText("_FAQ_");
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_faqText.textScale = 0.35f * m_popUpScale;
		}
		if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Russian)
		{
			m_faqText.textScale = 0.3f * m_popUpScale;
		}
		num = 0f;
		num2 = 0f;
		foreach (UISprite textSprite5 in m_faqText.textSprites)
		{
			if (num2 < textSprite5.height)
			{
				num2 = textSprite5.height;
			}
			num += textSprite5.width;
		}
		m_faqButton = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "SettingsScreenButton.png", "SettingsScreenButtonOver.png", 0, 0, m_settingsDepth + 3);
		m_faqButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		x = num / m_faqButton.width;
		y = num2 / m_faqButton.height;
		m_faqButton.scale = new Vector3(x, y, 1f);
		m_faqButton.parentUIObject = m_faqText;
		m_faqButton.positionFromTopLeft(0f, 0f);
		m_faqButton.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
		m_faqButton.onTouchUpInside += onTouchUpInsidefaqButton;
	}

	public void ShowSettingsMenu()
	{
		m_topBarTitle.text = LocalTextManager.GetUIText("_SETTINGS_TITLE_");
		HideInfoElements(true);
		HidePopUpFrame(false);
		HideTopBarTitle(false);
		HideBackButton(false);
		HideSettingsButtons(false);
	}

	public void ShowStorageMenu()
	{
		m_topBarTitle.text = LocalTextManager.GetUIText("_STORAGE_TITLE_");
		HideSettingsButtons(true);
		HidePopUpFrame(false);
		HideTopBarTitle(false);
		HideBackButton(false);
	}

	public void ShowInfoMenu()
	{
		m_topBarTitle.text = LocalTextManager.GetUIText("_INFO_TITLE_");
		HideSettingsButtons(true);
		HidePopUpFrame(false);
		HideTopBarTitle(false);
		HideBackButton(false);
		HideInfoElements(false);
	}

	public void HideAll(bool bHide)
	{
		HidePopUpFrame(bHide);
		HideTopBarTitle(bHide);
		HideBackButton(bHide);
		HideSettingsButtons(bHide);
		HideInfoElements(bHide);
	}

	public void HidePopUpFrame(bool bHide)
	{
		m_popUpFrame.hidden = bHide;
	}

	public void HideTopBarTitle(bool bHide)
	{
		m_topBarTitle.hidden = bHide;
	}

	public void HideBackButton(bool bHide)
	{
		m_backButton.hidden = bHide;
	}

	public void HideSettingsButtons(bool bHide)
	{
		m_mainButtonLayout.hidden = bHide;
		m_languageButton.hidden = bHide;
		m_languageText.hidden = bHide;
		m_infoButton.hidden = bHide;
		m_infoButtonText.hidden = bHide;
		m_soundToggleButton.hidden = bHide;
		m_musicToggleButton.hidden = bHide;
		if (!bHide && !IsSoundFXOn())
		{
			m_soundToggleButton.selected = false;
			m_redSlash1.hidden = false;
		}
		else
		{
			m_soundToggleButton.selected = true;
			m_redSlash1.hidden = true;
		}
		if (!bHide && !IsMusicOn())
		{
			m_musicToggleButton.selected = false;
			m_redSlash2.hidden = false;
		}
		else
		{
			m_musicToggleButton.selected = true;
			m_redSlash2.hidden = true;
		}
	}

	public void HideInfoElements(bool bHide)
	{
		m_publishedByText.hidden = bHide;
		m_publisherText.hidden = bHide;
		m_developedByText.hidden = bHide;
		m_developerText.hidden = bHide;
		m_publisherLogo.hidden = bHide;
		m_developerLogo.hidden = bHide;
		m_developerTextButton.hidden = bHide;
		m_publisherTextButton.hidden = bHide;
		m_fontByText.hidden = bHide;
		m_DisneyText.hidden = bHide;
		m_versionText.hidden = bHide;
		m_legalTitle.hidden = bHide;
		m_TOSText.hidden = bHide;
		m_TOSButton.hidden = bHide;
		m_privacyPolicyText.hidden = bHide;
		m_privacyButton.hidden = bHide;
		m_faqButton.hidden = bHide;
		m_faqText.hidden = bHide;
	}

	public void onTouchUpInsideBackButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		m_backButton.disabled = true;
		MainMenuEventManager.TriggerGoToPreviousMenu();
		m_backButton.disabled = false;
	}

	public void onTouchUpInsideLanguageButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		sender.disabled = true;
		ChangeLanguage();
		sender.disabled = false;
	}

	public void onTouchUpInsideInfoButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		Debug.Log("Info Button");
		m_infoButton.disabled = true;
		MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Settings_Info_Menu);
		m_infoButton.disabled = false;
	}

	public void onTouchUpInsidefbLogoutButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
	}

	public void onTouchUpInsideMajescoButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		Debug.Log("TOS Button");
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			DisableButtons();
			PopUpGUIManager.The.ShowNoInternetNotificationPopup(OnNoInternetPopUpDismissed, delegate
			{
			});
			HideAll(true);
		}
		else
		{
			m_TOSButton.disabled = true;
			EtceteraAndroid.showCustomWebView("http://www.sidekick.co.il/", true, false);
			m_TOSButton.disabled = false;
		}
	}

	public void onTouchUpInsideOddGentlemenButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		Debug.Log("TOS Button");
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			DisableButtons();
			PopUpGUIManager.The.ShowNoInternetNotificationPopup(OnNoInternetPopUpDismissed, delegate
			{
			});
			HideAll(true);
		}
		else
		{
			m_TOSButton.disabled = true;
			EtceteraAndroid.showWebView("http://theoddgentlemen.com/");
			m_TOSButton.disabled = false;
		}
	}

	public void onTouchUpInsidePublisherTextButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			DisableButtons();
			PopUpGUIManager.The.ShowNoInternetNotificationPopup(OnNoInternetPopUpDismissed, delegate
			{
			});
			HideAll(true);
		}
		else
		{
			m_publisherTextButton.disabled = true;
			EtceteraAndroid.showWebView("https://twitter.com/MajescoMobile");
			m_publisherTextButton.disabled = false;
		}
	}

	public void onTouchUpInsideDeveloperTextButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			DisableButtons();
			PopUpGUIManager.The.ShowNoInternetNotificationPopup(OnNoInternetPopUpDismissed, delegate
			{
			});
			HideAll(true);
		}
		else
		{
			m_developerTextButton.disabled = true;
			EtceteraAndroid.showWebView("https://twitter.com/theoddgentlemen");
			m_developerTextButton.disabled = false;
		}
	}

	public void onTouchUpInsideTOSButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		Debug.Log("TOS Button");
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			DisableButtons();
			PopUpGUIManager.The.ShowNoInternetNotificationPopup(OnNoInternetPopUpDismissed, delegate
			{
			});
			HideAll(true);
		}
		else
		{
			m_TOSButton.disabled = true;
			EtceteraAndroid.showWebView("http://www.majesco.net/Majesco_Mobile_App_TOS.htm");
			m_TOSButton.disabled = false;
		}
	}

	public void onTouchUpInsidePrivacyPolicyButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		Debug.Log("Privacy Policy Button");
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			DisableButtons();
			PopUpGUIManager.The.ShowNoInternetNotificationPopup(OnNoInternetPopUpDismissed, delegate
			{
			});
			HideAll(true);
		}
		else
		{
			m_privacyButton.disabled = true;
			EtceteraAndroid.showWebView("http://majesco.net/majesco-mobile-privacy-policy.html");
			m_privacyButton.disabled = false;
		}
	}

	public void onTouchUpInsidefaqButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		Debug.Log("faq Button");
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			DisableButtons();
			PopUpGUIManager.The.ShowNoInternetNotificationPopup(OnNoInternetPopUpDismissed, delegate
			{
			});
			HideAll(true);
		}
		else
		{
			m_faqButton.disabled = true;
			EtceteraAndroid.showWebView("http://www.majesco.net/DoofenDASH/FAQs/");
			m_faqButton.disabled = false;
		}
	}

	public void onToggleMusicButton(UIToggleButton sender, bool selected)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		if (selected)
		{
			Debug.Log("Music On");
			m_redSlash2.hidden = true;
			GameManager.The.m_MusicEnabled = true;
		}
		else
		{
			Debug.Log("Music Off");
			m_redSlash2.hidden = false;
			GameManager.The.m_MusicEnabled = false;
		}
	}

	public void onToggleSoundButton(UIToggleButton sender, bool selected)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		if (selected)
		{
			Debug.Log("Sound On");
			m_redSlash1.hidden = true;
			GameManager.The.m_SoundEnabled = true;
		}
		else
		{
			Debug.Log("Sound Off");
			m_redSlash1.hidden = false;
			GameManager.The.m_SoundEnabled = false;
		}
	}

	public void onToggleCloudButton(UIToggleButton sender, bool selected)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		Debug.Log("iCloud Button");
	}

	public void DisableButtons()
	{
		if (m_backButton != null)
		{
			m_backButton.disabled = true;
		}
		if (m_languageButton != null)
		{
			m_languageButton.disabled = true;
		}
		if (m_fbLogoutButton != null)
		{
			m_fbLogoutButton.disabled = true;
		}
		if (m_infoButton != null)
		{
			m_infoButton.disabled = true;
		}
		if (m_publisherLogo != null)
		{
			m_publisherLogo.disabled = true;
		}
		if (m_developerLogo != null)
		{
			m_developerLogo.disabled = true;
		}
		if (m_developerTextButton != null)
		{
			m_developerTextButton.disabled = true;
		}
		if (m_publisherTextButton != null)
		{
			m_publisherTextButton.disabled = true;
		}
		if (m_TOSButton != null)
		{
			m_TOSButton.disabled = true;
		}
		if (m_privacyButton != null)
		{
			m_privacyButton.disabled = true;
		}
		if (m_faqButton != null)
		{
			m_faqButton.disabled = true;
		}
	}

	public void EnableButtons()
	{
		if (m_backButton != null)
		{
			m_backButton.disabled = false;
		}
		if (m_languageButton != null)
		{
			m_languageButton.disabled = false;
		}
		if (m_fbLogoutButton != null)
		{
			m_fbLogoutButton.disabled = false;
		}
		if (m_infoButton != null)
		{
			m_infoButton.disabled = false;
		}
		if (m_publisherLogo != null)
		{
			m_publisherLogo.disabled = false;
		}
		if (m_developerLogo != null)
		{
			m_developerLogo.disabled = false;
		}
		if (m_developerTextButton != null)
		{
			m_developerTextButton.disabled = false;
		}
		if (m_publisherTextButton != null)
		{
			m_publisherTextButton.disabled = false;
		}
		if (m_TOSButton != null)
		{
			m_TOSButton.disabled = false;
		}
		if (m_privacyButton != null)
		{
			m_privacyButton.disabled = false;
		}
		if (m_faqButton != null)
		{
			m_faqButton.disabled = false;
		}
	}

	private void OnNoInternetPopUpDismissed(object o)
	{
		EnableButtons();
		ShowInfoMenu();
	}

	public void ChangeLanguage()
	{
		LocalTextManager.PerryLanguages language = LocalTextManager.CurrentLanguageType + 1;
		GlobalGUIManager.The.ChangeLanguage(language);
	}

	private bool IsSoundFXOn()
	{
		return GameManager.The.m_SoundEnabled;
	}

	private bool IsMusicOn()
	{
		return GameManager.The.m_MusicEnabled;
	}

	private bool IsDataSharingOn()
	{
		return true;
	}

	private bool IsCloudStorageOn()
	{
		return !PlayerData.IsiCloudDisabledByUser;
	}

	public void ReloadStaticText()
	{
		m_languageText.text = LocalTextManager.GetUIText("_LANGUAGE_");
		m_infoButtonText.text = LocalTextManager.GetUIText("_INFO_");
		m_publishedByText.text = LocalTextManager.GetUIText("_PUBLISHED_BY_");
		m_developedByText.text = LocalTextManager.GetUIText("_DEVELOPED_BY_");
		m_fontByText.text = LocalTextManager.GetUIText("_FONT_BY_");
		m_DisneyText.text = LocalTextManager.GetUIText("_LEGAL_COPYRIGHT_");
		m_legalTitle.text = LocalTextManager.GetUIText("_LEGAL_");
		m_TOSText.text = LocalTextManager.GetUIText("_TERMS_OF_SERVICE_");
		m_privacyPolicyText.text = LocalTextManager.GetUIText("_PRIVACY_POLICY_");
		m_faqText.text = LocalTextManager.GetUIText("_FAQ_");
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_publishedByText.textScale = 0.4f * m_popUpScale;
			m_developedByText.textScale = 0.4f * m_popUpScale;
			m_legalTitle.textScale = 0.35f * m_popUpScale;
			m_TOSText.textScale = 0.35f * m_popUpScale;
			m_privacyPolicyText.textScale = 0.35f * m_popUpScale;
			m_faqText.textScale = 0.35f * m_popUpScale;
		}
		else
		{
			m_publishedByText.textScale = 0.48f * m_popUpScale;
			m_developedByText.textScale = 0.48f * m_popUpScale;
			m_legalTitle.textScale = 0.5f * m_popUpScale;
			m_TOSText.textScale = 0.5f * m_popUpScale;
			m_privacyPolicyText.textScale = 0.5f * m_popUpScale;
			m_faqText.textScale = 0.5f * m_popUpScale;
		}
		if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Russian)
		{
			m_publishedByText.textScale = 0.4f * m_popUpScale;
			m_developedByText.textScale = 0.4f * m_popUpScale;
			m_legalTitle.textScale = 0.3f * m_popUpScale;
			m_TOSText.textScale = 0.3f * m_popUpScale;
			m_privacyPolicyText.textScale = 0.25f * m_popUpScale;
			m_faqText.textScale = 0.3f * m_popUpScale;
		}
	}
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PopUpGUIManager : MonoBehaviour
{
	public enum PopUpType
	{
		ShowGameExitConfirmation = 0,
		Other = 1,
		None = 2
	}

	public delegate void PopUpCallback(object o);

	private UISprite m_greyOutBG;

	private UISprite m_popUpWindow;

	private UIButton m_OKButton;

	private UIButton m_CancelButton;

	private UIButton m_CheckMarkButton;

	private ArrayList m_badges;

	private UIHorizontalLayout m_badgeLayout;

	private UISprite m_multSprite;

	private UIHorizontalLayout m_multiplierLayout;

	private UITextInstance m_missionTitle;

	private UITextInstance m_missionSubTitle;

	private UITextInstance m_badgeEarnedText;

	private UITextInstance m_multiplierEarnedTxt;

	private UITextInstance m_scoreEarnedTxt;

	private UITextInstance m_DefaultTitle;

	private UITextInstance m_DefaultSubTitle;

	private UITextInstance m_DefaultBodyText;

	private UITextInstance m_DefaultCostText;

	private UITextInstance m_notEnoughTokensTitle;

	private UITextInstance m_TokenpurchaseSubText;

	private UITextInstance m_tokenPackTitle;

	private UITextInstance m_tokenPackSubTitle;

	private UITextInstance m_tokenPackPrice;

	private UITextInstance m_alternateCantConnectTxt;

	private UITextInstance m_alternateCantConnectTitle;

	private UISprite m_tokenPackIcon;

	private static UIButton m_videoAdButton;

	private static UITextInstance m_videoAdLabel;

	private PopUpCallback m_okCallback;

	private PopUpCallback m_cancelCallback;

	public RawImage m_popUpBG;

	public PopUpType m_PopupType = PopUpType.None;

	private static int m_popUpDepth = -9;

	private static int MAX_MISSION_LEVEL = 6;

	private static PopUpGUIManager m_the;

	public bool isAPopupActive
	{
		get
		{
			return !m_popUpWindow.hidden;
		}
	}

	public static PopUpGUIManager The
	{
		get
		{
			if (m_the == null)
			{
				GameObject gameObject = new GameObject("PopUpGUIManager");
				((PopUpGUIManager)gameObject.AddComponent<PopUpGUIManager>()).Init();
			}
			return m_the;
		}
	}

	public void Init()
	{
		if (!(m_the != null))
		{
			m_the = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			InitPopUpScreenDefaults();
			InitPopUPButtons();
			InitMissionCompletePopUp();
			InitBuyMissionPopUp();
			InitNotEnoughTokensPopUp();
			InitVideoAdButton();
			HideAll();
		}
	}

	public void HideVideoAdButton()
	{
		m_videoAdButton.hidden = true;
		m_videoAdLabel.hidden = true;
	}

	private void ShowVideoAdButton()
	{
		m_videoAdButton.hidden = false;
		m_videoAdLabel.hidden = false;
	}

	private void InitVideoAdButton()
	{
		m_videoAdButton = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "PlayButton.png", "PlayButton.png", 0, 0, m_popUpDepth);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_videoAdButton);
		m_videoAdButton.positionFromTopLeft(0.65f, 0.32f);
		m_videoAdButton.onTouchUpInside += onTouchUpInsideVideoAdButton;
		m_videoAdLabel = GlobalGUIManager.The.defaultText.addTextInstance(LocalTextManager.GetUIText("_VIDEO_AD_"), 0f, 0f, 0.6f, m_popUpDepth);
		m_videoAdLabel.alignMode = UITextAlignMode.Center;
		m_videoAdLabel.verticalAlignMode = UITextVerticalAlignMode.Top;
		m_videoAdLabel.setColorForAllLetters(Color.black);
		m_videoAdLabel.parentUIObject = m_videoAdButton;
		m_videoAdLabel.positionFromCenter(0f, 0f);
		m_videoAdLabel.text = LocalTextManager.GetUIText("_VIDEO_AD_");
		if (Screen.height >= 2048)
		{
			m_videoAdLabel.textScale = 0.5f;
		}
	}

	private void InitPopUpScreenDefaults()
	{
		m_greyOutBG = GlobalGUIManager.The.m_menuToolkit.addSprite("SellScreenPopUp.png", 0, 0, m_popUpDepth + 2, true);
		m_greyOutBG.positionCenter();
		m_greyOutBG.color = Color.black;
		m_greyOutBG.scale = new Vector3(Screen.width, Screen.height, 1f);
		m_popUpWindow = GlobalGUIManager.The.m_menuToolkit.addSprite("SellScreenPopUp.png", 0, 0, m_popUpDepth + 1, true);
		if (GameManager.The.aspectRatio.x == 3f && GameManager.The.aspectRatio.x == 4f)
		{
			m_popUpWindow.positionCenter();
		}
		else
		{
			m_popUpWindow.positionFromCenter(0f, 0.02f);
		}
		int num = 40;
		float num2 = 1f;
		if (m_popUpWindow.width > (float)(Screen.width - num))
		{
			int num3 = Screen.width - num;
			num2 = (float)num3 / m_popUpWindow.width;
		}
		m_popUpWindow.scale = new Vector3(num2, num2, 1f);
	}

	private void InitPopUPButtons()
	{
		int num = 40;
		float num2 = 1f;
		if (m_popUpWindow.width > (float)(Screen.width - num))
		{
			int num3 = Screen.width - num;
			num2 = (float)num3 / m_popUpWindow.width;
		}
		m_OKButton = UIButton.create(GlobalGUIManager.The.m_hudToolkit, "TokenSaleButton.png", "TokenSaleButtonOver.png", 0, 0, m_popUpDepth);
		m_OKButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_OKButton.scale = new Vector3(num2, num2, 1f);
		m_OKButton.parentUIObject = m_popUpWindow;
		m_OKButton.positionFromBottomRight(0.5f, 0.5f);
		m_OKButton.onTouchUpInside += onTouchUpInsideDefaultClickOkButton;
		m_CancelButton = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "ExitButton.png", "ExitButtonOver.png", 0, 0, m_popUpDepth);
		m_CancelButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_CancelButton.scale = new Vector3(num2, num2, 1f);
		m_CancelButton.parentUIObject = m_popUpWindow;
		m_CancelButton.positionFromTopLeft(-0.05f, -0.05f);
		m_CancelButton.onTouchUpInside += onTouchUpInsideDefaultClickCancelButton;
		m_CheckMarkButton = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "CheckMarkButton.png", "CheckMarkButtonOver.png", 0, 0, m_popUpDepth);
		m_CheckMarkButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_CheckMarkButton.scale = new Vector3(num2, num2, 1f);
		m_CheckMarkButton.parentUIObject = m_popUpWindow;
		m_CheckMarkButton.positionFromBottomRight(0.5f, 0.5f);
		m_CheckMarkButton.onTouchUpInside += onTouchUpInsideCheckMarkButton;
	}

	public void InitMissionCompletePopUp()
	{
		m_missionTitle = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_MISSION_COMPLETE_"), 0f, 0f, 1f, m_popUpDepth);
		m_missionTitle.alignMode = UITextAlignMode.Center;
		m_missionTitle.parentUIObject = m_popUpWindow;
		m_missionTitle.textScale = 0.8f * m_popUpWindow.scale.x;
		m_missionTitle.positionFromTop(0.02f);
		m_missionTitle.text = LocalTextManager.GetUIText("_MISSION_COMPLETE_");
		m_missionSubTitle = GlobalGUIManager.The.defaultTextAlt.addTextInstance("Default Mission Desc", 0f, 0f, 1f, m_popUpDepth);
		m_missionSubTitle.alignMode = UITextAlignMode.Center;
		m_missionSubTitle.parentUIObject = m_missionTitle;
		m_missionSubTitle.setColorForAllLetters(Color.white);
		m_missionSubTitle.textScale = 0.65f * m_popUpWindow.scale.x;
		m_missionSubTitle.positionFromTop(1.5f);
		m_missionSubTitle.text = "Default Mission Desc";
		m_multiplierEarnedTxt = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_MULTIPLIER_PLUS_"), 0f, 0f, 1f, m_popUpDepth);
		m_multiplierEarnedTxt.parentUIObject = m_popUpWindow;
		m_multiplierEarnedTxt.alignMode = UITextAlignMode.Center;
		m_multiplierEarnedTxt.textScale = 0.55f * m_popUpWindow.scale.x;
		m_multiplierEarnedTxt.setColorForAllLetters(Color.black);
		m_multiplierEarnedTxt.positionFromTop(0.32f, -0.1f);
		m_multiplierEarnedTxt.text = LocalTextManager.GetUIText("_MULTIPLIER_PLUS_");
		m_multSprite = GlobalGUIManager.The.m_menuToolkit.addSprite("+1Bonus.png", 0, 0, m_popUpDepth, true);
		m_multSprite.parentUIObject = m_multiplierEarnedTxt;
		m_multSprite.scale = new Vector3(m_popUpWindow.scale.x, m_popUpWindow.scale.y, 1f);
		m_multSprite.positionFromLeft(0.2f, 1.2f);
		m_badgeEarnedText = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_LEVEL_PLUS_"), 0f, 0f, 1f, m_popUpDepth);
		m_badgeEarnedText.parentUIObject = m_popUpWindow;
		m_badgeEarnedText.alignMode = UITextAlignMode.Left;
		m_badgeEarnedText.textScale = 0.55f * m_popUpWindow.scale.x;
		m_badgeEarnedText.setColorForAllLetters(Color.black);
		m_badgeEarnedText.positionFromTop(0.42f, -0.1f);
		m_badgeEarnedText.text = LocalTextManager.GetUIText("_LEVEL_PLUS_");
		m_badgeLayout = new UIHorizontalLayout(0);
		m_badgeLayout.alignMode = UIAbstractContainer.UIContainerAlignMode.Left;
		m_badges = new ArrayList();
		for (int i = 0; i < MAX_MISSION_LEVEL; i++)
		{
			UISprite uISprite = GlobalGUIManager.The.m_menuToolkit.addSprite("Badge.png", 0, 0, m_popUpDepth);
			uISprite.scale = new Vector3(m_popUpWindow.scale.x, m_popUpWindow.scale.y, 1f);
			m_badges.Add(uISprite);
			m_badgeLayout.addChild(uISprite);
		}
		m_badgeLayout.parentUIObject = m_badgeEarnedText;
		m_badgeLayout.positionFromLeft(-0.3f, 1.5f);
		m_scoreEarnedTxt = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_SCORE_PLUS_") + " 10000", 0f, 0f, 1f, m_popUpDepth);
		m_scoreEarnedTxt.parentUIObject = m_badgeEarnedText;
		m_scoreEarnedTxt.alignMode = UITextAlignMode.Left;
		m_scoreEarnedTxt.textScale = m_multiplierEarnedTxt.textScale;
		m_scoreEarnedTxt.setColorForAllLetters(Color.black);
		m_scoreEarnedTxt.positionFromTopLeft(2f, 0f);
		m_scoreEarnedTxt.text = LocalTextManager.GetUIText("_SCORE_PLUS_") + " 10000";
		m_scoreEarnedTxt.parentUIObject = m_popUpWindow;
	}

	public void InitBuyMissionPopUp()
	{
		m_DefaultTitle = GlobalGUIManager.The.defaultText.addTextInstance(LocalTextManager.GetUIText("_BUY_MISSION_"), 0f, 0f, 1f, m_popUpDepth);
		m_DefaultTitle.alignMode = UITextAlignMode.Center;
		m_DefaultTitle.parentUIObject = m_popUpWindow;
		m_DefaultTitle.textScale = 0.45f * m_popUpWindow.scale.x;
		m_DefaultTitle.positionFromCenter(0f, 0f);
		m_DefaultTitle.text = LocalTextManager.GetUIText("_BUY_MISSION_");
		m_DefaultSubTitle = GlobalGUIManager.The.defaultText.addTextInstance("Default Mission Desc", 0f, 0f, 1f, m_popUpDepth);
		m_DefaultSubTitle.alignMode = UITextAlignMode.Center;
		m_DefaultSubTitle.parentUIObject = m_popUpWindow;
		m_DefaultSubTitle.setColorForAllLetters(Color.white);
		m_DefaultSubTitle.textScale = 0.3f * m_popUpWindow.scale.x;
		m_DefaultSubTitle.positionFromCenter(0.1f, 0f);
		m_DefaultSubTitle.text = "Default Mission Desc";
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_DefaultSubTitle.textScale = 0.25f * m_popUpWindow.scale.x;
		}
		m_DefaultBodyText = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_BUY_MISSION_CONFIRMATION_"), 0f, 0f, 1f, m_popUpDepth);
		m_DefaultBodyText.alignMode = UITextAlignMode.Center;
		m_DefaultBodyText.parentUIObject = m_popUpWindow;
		m_DefaultBodyText.setColorForAllLetters(Color.black);
		m_DefaultBodyText.textScale = 0.45f * m_popUpWindow.scale.x;
		m_DefaultBodyText.positionFromTop(0.1f);
		m_DefaultBodyText.text = LocalTextManager.GetUIText("_BUY_MISSION_CONFIRMATION_");
		m_DefaultCostText = GlobalGUIManager.The.defaultText.addTextInstance("999,999,999", 0f, 0f, 1f, m_popUpDepth);
		m_DefaultCostText.setColorForAllLetters(Color.white);
		m_DefaultCostText.textScale = 0.2f * m_popUpWindow.scale.x;
		m_DefaultCostText.alignMode = UITextAlignMode.Left;
		m_DefaultCostText.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_DefaultCostText.parentUIObject = m_popUpWindow;
		m_DefaultCostText.positionFromBottomRight(0.155f, 0.06f);
		m_DefaultCostText.text = "999,999,999";
	}

	public void InitNotEnoughTokensPopUp()
	{
		m_notEnoughTokensTitle = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_NEED_MORE_TOKENS_"), 0f, 0f, 1f, m_popUpDepth);
		m_notEnoughTokensTitle.alignMode = UITextAlignMode.Center;
		m_notEnoughTokensTitle.parentUIObject = m_popUpWindow;
		m_notEnoughTokensTitle.setColorForAllLetters(Color.black);
		m_notEnoughTokensTitle.textScale = 0.45f * m_popUpWindow.scale.x;
		m_notEnoughTokensTitle.positionFromTop(0.06f);
		m_notEnoughTokensTitle.text = LocalTextManager.GetUIText("_NEED_MORE_TOKENS_");
		m_TokenpurchaseSubText = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_PURCHASE_TOKEN_PACK_"), 0f, 0f, 1f, m_popUpDepth);
		m_TokenpurchaseSubText.alignMode = UITextAlignMode.Center;
		m_TokenpurchaseSubText.parentUIObject = m_popUpWindow;
		m_TokenpurchaseSubText.setColorForAllLetters(Color.white);
		m_TokenpurchaseSubText.textScale = 0.32f * m_popUpWindow.scale.x;
		m_TokenpurchaseSubText.positionFromTop(0.2f);
		m_TokenpurchaseSubText.text = LocalTextManager.GetUIText("_PURCHASE_TOKEN_PACK_");
		m_alternateCantConnectTxt = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_CONNECT_TO_INTERNET_"), 0f, 0f, 1f, m_popUpDepth);
		m_alternateCantConnectTxt.alignMode = UITextAlignMode.Center;
		m_alternateCantConnectTxt.setColorForAllLetters(Color.white);
		m_alternateCantConnectTxt.textScale = 0.45f * m_popUpWindow.scale.x;
		m_alternateCantConnectTxt.parentUIObject = m_popUpWindow;
		m_alternateCantConnectTxt.positionFromTop(0.65f);
		m_alternateCantConnectTxt.text = LocalTextManager.GetUIText("_CONNECT_TO_INTERNET_POPUP_");
		UIHelper.LineBreakText(ref m_alternateCantConnectTxt, m_popUpWindow.width - (float)Screen.width * 0.065f);
		m_alternateCantConnectTitle = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_CANNOT_ACCESS_"), 0f, 0f, 1f, m_popUpDepth);
		m_alternateCantConnectTitle.alignMode = UITextAlignMode.Center;
		m_alternateCantConnectTitle.setColorForAllLetters(Color.black);
		m_alternateCantConnectTitle.textScale = 0.45f * m_popUpWindow.scale.x;
		m_alternateCantConnectTitle.parentUIObject = m_popUpWindow;
		m_alternateCantConnectTitle.positionFromTop(0.15f);
		m_alternateCantConnectTitle.text = LocalTextManager.GetUIText("_CANNOT_ACCESS_");
		UIHelper.LineBreakText(ref m_alternateCantConnectTitle, m_popUpWindow.width - (float)Screen.width * 0.065f);
		m_tokenPackTitle = GlobalGUIManager.The.defaultText.addTextInstance("Default Token Pack Title", 0f, 0f, 1f, m_popUpDepth);
		m_tokenPackTitle.alignMode = UITextAlignMode.Center;
		m_tokenPackTitle.parentUIObject = m_popUpWindow;
		m_tokenPackTitle.setColorForAllLetters(Color.white);
		m_tokenPackTitle.textScale = 0.35f * m_popUpWindow.scale.x;
		m_tokenPackTitle.positionFromCenter(0.012f, 0f);
		m_tokenPackTitle.text = "Default Token Pack Title";
		m_tokenPackSubTitle = GlobalGUIManager.The.defaultTextAlt.addTextInstance("TokenPackSubtitle", 0f, 0f, 1f, m_popUpDepth);
		m_tokenPackSubTitle.alignMode = UITextAlignMode.Center;
		m_tokenPackSubTitle.parentUIObject = m_popUpWindow;
		m_tokenPackSubTitle.setColorForAllLetters(Color.black);
		m_tokenPackSubTitle.textScale = 0.25f * m_popUpWindow.scale.x;
		m_tokenPackSubTitle.positionFromTop(0.55f);
		m_tokenPackSubTitle.text = "TokenPackSubtitle";
		m_tokenPackPrice = GlobalGUIManager.The.defaultTextAlt.addTextInstance("???", 0f, 0f, 1f, m_popUpDepth);
		m_tokenPackPrice.parentUIObject = m_OKButton;
		m_tokenPackPrice.setColorForAllLetters(Color.white);
		m_tokenPackPrice.textScale = 0.55f * m_popUpWindow.scale.x;
		m_tokenPackPrice.alignMode = UITextAlignMode.Right;
		m_tokenPackPrice.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_tokenPackPrice.positionFromCenter(0f, 0f);
	}

	public void HideAll()
	{
		HidePopUpDefault();
		HideMissionCompletePopUp();
		HideBuyMissionPopUP();
		HideNotEnoughTokensPopUp();
	}

	private void HidePopUpDefault()
	{
		m_greyOutBG.hidden = true;
		m_popUpWindow.hidden = true;
		m_OKButton.hidden = true;
		m_CancelButton.hidden = true;
		m_CheckMarkButton.hidden = true;
	}

	private void HideMissionCompletePopUp()
	{
		m_missionTitle.hidden = true;
		m_missionSubTitle.hidden = true;
		m_multiplierEarnedTxt.hidden = true;
		m_scoreEarnedTxt.hidden = true;
		m_badgeEarnedText.hidden = true;
		m_multSprite.hidden = true;
		for (int i = 0; i < m_badges.Count; i++)
		{
			((UISprite)m_badges[i]).hidden = true;
		}
	}

	private void HideBuyMissionPopUP()
	{
		m_DefaultTitle.hidden = true;
		m_DefaultSubTitle.hidden = true;
		m_DefaultBodyText.hidden = true;
		m_DefaultCostText.hidden = true;
	}

	private void HideNotEnoughTokensPopUp()
	{
		m_notEnoughTokensTitle.hidden = true;
		m_TokenpurchaseSubText.hidden = true;
		m_tokenPackTitle.hidden = true;
		m_tokenPackSubTitle.hidden = true;
		if (m_tokenPackIcon != null)
		{
			m_tokenPackIcon.hidden = true;
			m_tokenPackIcon.destroy();
			m_tokenPackIcon = null;
		}
		m_tokenPackPrice.hidden = true;
		m_alternateCantConnectTxt.hidden = true;
		m_alternateCantConnectTitle.hidden = true;
		HideVideoAdButton();
	}

	public void ShowMissionCompletePopUp(Mission mission, bool getScoreBonus, PopUpCallback okButtonCallback, Action<bool> onGreyOutCallback)
	{
		GreyOutBG();
		m_okCallback = okButtonCallback;
		m_OKButton.userData = mission;
		UIAnimation ani = m_popUpWindow.alphaFromTo(0.35f, 0f, 1f, Easing.Linear.easeInOut);
		m_popUpWindow.hidden = false;
		m_missionTitle.hidden = false;
		ani = m_missionTitle.alphaFromTo(0.4f, 0f, 1f, Easing.Linear.easeInOut);
		m_missionSubTitle.text = mission.m_desc;
		m_missionSubTitle.hidden = false;
		ani = m_missionSubTitle.alphaFromTo(0.4f, 0f, 1f, Easing.Linear.easeInOut);
		ani.onComplete = delegate
		{
			m_multiplierEarnedTxt.hidden = false;
			Vector3 start = new Vector3(m_multiplierEarnedTxt.localPosition.x - (float)Screen.width, m_multiplierEarnedTxt.localPosition.y, 0f);
			ani = m_multiplierEarnedTxt.positionFrom(0.5f, start, Easing.Sinusoidal.easeInOut);
			start = new Vector3(m_multSprite.localPosition.x - (float)Screen.width, m_multSprite.localPosition.y, 0f);
			m_multSprite.hidden = false;
			ani = m_multSprite.positionFrom(0.25f, start, Easing.Sinusoidal.easeInOut);
			ani.onComplete = delegate
			{
				m_badgeEarnedText.hidden = false;
				start = new Vector3(m_badgeEarnedText.localPosition.x - (float)Screen.width, m_badgeEarnedText.localPosition.y, 0f);
				ani = m_badgeEarnedText.positionFrom(0.5f, start, Easing.Sinusoidal.easeInOut);
				ani.onComplete = delegate
				{
					int levelVal = mission.m_levelVal;
					for (int i = 0; i < levelVal; i++)
					{
						((UISprite)m_badges[i]).hidden = false;
						ani = ((UISprite)m_badges[i]).scaleFrom(0.25f, new Vector3(3f, 3f, 1f), Easing.Sinusoidal.easeInOut);
					}
					ani.onComplete = delegate
					{
						if (getScoreBonus)
						{
							m_scoreEarnedTxt.text = "Score + " + mission.m_scoreBonus;
							m_scoreEarnedTxt.hidden = false;
							start = new Vector3(m_scoreEarnedTxt.localPosition.x - (float)Screen.width, m_scoreEarnedTxt.localPosition.y, 0f);
							ani = m_scoreEarnedTxt.positionFrom(0.5f, start, Easing.Sinusoidal.easeInOut);
						}
						else
						{
							JustDisplayOKButton();
						}
						ani.onComplete = delegate
						{
							JustDisplayOKButton();
						};
					};
				};
			};
		};
	}

	public void ShowBuyMissionPopUp(Mission mission, PopUpCallback okCallback, PopUpCallback cancelCallback, Action<bool> onGreyOutCallback)
	{
		GreyOutBG();
		m_okCallback = okCallback;
		m_cancelCallback = cancelCallback;
		m_OKButton.userData = mission;
		m_CancelButton.userData = mission;
		UIAnimation uIAnimation = m_popUpWindow.alphaFromTo(0.35f, 0f, 1f, Easing.Linear.easeInOut);
		m_popUpWindow.hidden = false;
		m_DefaultSubTitle.text = mission.m_name;
		m_DefaultTitle.hidden = false;
		uIAnimation = m_DefaultTitle.alphaFromTo(0.4f, 0f, 1f, Easing.Linear.easeInOut);
		m_DefaultSubTitle.text = mission.m_desc;
		m_DefaultSubTitle.hidden = false;
		uIAnimation = m_DefaultSubTitle.alphaFromTo(0.4f, 0f, 1f, Easing.Linear.easeInOut);
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_DefaultSubTitle.textScale = 0.25f * m_popUpWindow.scale.x;
		}
		m_DefaultBodyText.positionFromTop(0.1f);
		m_DefaultBodyText.text = LocalTextManager.GetUIText("_BUY_MISSION_CONFIRMATION_");
		m_DefaultBodyText.hidden = false;
		uIAnimation = m_DefaultBodyText.alphaFromTo(0.4f, 0f, 1f, Easing.Linear.easeInOut);
		m_DefaultCostText.text = mission.m_skipTokenCost.ToString("N0");
		if (m_DefaultCostText.textSprites.Count < 5)
		{
			m_DefaultCostText.textScale = 0.45f * m_popUpWindow.scale.x;
			m_DefaultCostText.text = mission.m_skipTokenCost.ToString("N0");
		}
		else if (m_DefaultCostText.textSprites.Count < 6)
		{
			m_DefaultCostText.textScale = 0.45f * m_popUpWindow.scale.x * 0.92f;
			m_DefaultCostText.text = mission.m_skipTokenCost.ToString("N0");
		}
		else if (m_DefaultCostText.textSprites.Count < 7)
		{
			m_DefaultCostText.textScale = 0.45f * m_popUpWindow.scale.x * 0.84f;
			m_DefaultCostText.text = mission.m_skipTokenCost.ToString("N0");
		}
		else if (m_DefaultCostText.textSprites.Count < 8)
		{
			m_DefaultCostText.textScale = 0.45f * m_popUpWindow.scale.x * 0.77f;
			m_DefaultCostText.text = mission.m_skipTokenCost.ToString("N0");
		}
		m_DefaultCostText.text = mission.m_skipTokenCost.ToString("N0");
		m_OKButton.setSpriteImage("TokenSaleButton.png");
		m_OKButton.SetHighlightedImage("TokenSaleButtonOver.png");
		m_DefaultCostText.hidden = false;
		uIAnimation = m_DefaultCostText.alphaFromTo(0.5f, 0f, 1f, Easing.Linear.easeInOut);
		uIAnimation.onComplete = delegate
		{
			DisplayOKandCancel();
		};
	}

	public void ShowGameExitConfirmationPrompt(PopUpCallback CancelButtonCallback, PopUpCallback CheckMarkButtonCallback, Action<bool> onDisplay)
	{
		m_PopupType = PopUpType.ShowGameExitConfirmation;
		m_OKButton.hidden = true;
		GreyOutBG();
		m_cancelCallback = CancelButtonCallback;
		m_okCallback = CheckMarkButtonCallback;
		UIAnimation uIAnimation = m_popUpWindow.alphaFromTo(0.5f, 0f, 1f, Easing.Linear.easeInOut);
		m_popUpWindow.hidden = false;
		m_DefaultBodyText.text = LocalTextManager.GetUIText("_EXIT_CONFIRMATION_PROMPT_");
		UIHelper.LineBreakText(ref m_DefaultBodyText, m_popUpWindow.width - (float)Screen.width * 0.05f);
		m_DefaultBodyText.hidden = false;
		uIAnimation = m_TokenpurchaseSubText.alphaFromTo(0.6f, 0f, 1f, Easing.Linear.easeInOut);
		uIAnimation.onComplete = delegate
		{
			DisplayCheckMarkandCancel();
		};
	}

	public void ShowPurchaseOutcomeConfirmation(string PopupMessage, PopUpCallback CheckMarkButtonCallback, Action<bool> onDisplay)
	{
		m_CancelButton.hidden = true;
		m_OKButton.hidden = true;
		GreyOutBG();
		m_okCallback = CheckMarkButtonCallback;
		UIAnimation uIAnimation = m_popUpWindow.alphaFromTo(0.5f, 0f, 1f, Easing.Linear.easeInOut);
		m_popUpWindow.hidden = false;
		m_DefaultBodyText.text = PopupMessage;
		UIHelper.LineBreakText(ref m_DefaultBodyText, m_popUpWindow.width - (float)Screen.width * 0.05f);
		m_DefaultBodyText.hidden = false;
		uIAnimation = m_TokenpurchaseSubText.alphaFromTo(0.6f, 0f, 1f, Easing.Linear.easeInOut);
		uIAnimation.onComplete = delegate
		{
			DisplayCheckMarkButton();
		};
	}

	public void ShowNoInternetNotificationPopup(PopUpCallback cancelCallback, Action<bool> onDisplay)
	{
		m_OKButton.hidden = true;
		m_CheckMarkButton.hidden = true;
		GreyOutBG();
		m_cancelCallback = cancelCallback;
		UIAnimation uIAnimation = m_popUpWindow.alphaFromTo(0.5f, 0f, 1f, Easing.Linear.easeInOut);
		m_popUpWindow.hidden = false;
		m_alternateCantConnectTxt.hidden = false;
		m_alternateCantConnectTitle.hidden = false;
		uIAnimation = m_alternateCantConnectTxt.alphaFromTo(0.6f, 0f, 1f, Easing.Linear.easeInOut);
		uIAnimation = m_alternateCantConnectTitle.alphaFromTo(0.6f, 0f, 1f, Easing.Linear.easeInOut);
		uIAnimation.onComplete = delegate
		{
			DisplayCancelButton();
		};
	}

	public void ShowNeedMoreTokensPopUp(string itemName, int itemTokenCost, int itemFedoraCost, PopUpCallback okCallback, PopUpCallback cancelCallback, Action<bool> onDisplay)
	{
		m_OKButton.hidden = true;
		m_CheckMarkButton.hidden = true;
		GreyOutBG();
		m_okCallback = okCallback;
		m_cancelCallback = cancelCallback;
		UIAnimation uIAnimation = m_popUpWindow.alphaFromTo(0.5f, 0f, 1f, Easing.Linear.easeInOut);
		m_popUpWindow.hidden = false;
		if (itemTokenCost > 0)
		{
			m_notEnoughTokensTitle.text = LocalTextManager.GetUIText("_NEED_MORE_TOKENS_");
			m_OKButton.setSpriteImage("TokenSaleButton.png");
			m_OKButton.SetHighlightedImage("TokenSaleButtonOver.png");
		}
		else if (itemFedoraCost > 0)
		{
			m_notEnoughTokensTitle.text = LocalTextManager.GetUIText("_NEED_MORE_FEDORAS_");
			m_OKButton.setSpriteImage("FedoraSaleButton.png");
			m_OKButton.SetHighlightedImage("FedoraSaleButtonOver.png");
		}
		m_notEnoughTokensTitle.hidden = false;
		uIAnimation = m_notEnoughTokensTitle.alphaFromTo(0.6f, 0f, 1f, Easing.Linear.easeInOut);
		m_alternateCantConnectTxt.hidden = false;
		uIAnimation = m_alternateCantConnectTxt.alphaFromTo(0.6f, 0f, 1f, Easing.Linear.easeInOut);
		m_alternateCantConnectTitle.hidden = false;
		uIAnimation = m_alternateCantConnectTitle.alphaFromTo(0.6f, 0f, 1f, Easing.Linear.easeInOut);
		uIAnimation.onComplete = delegate
		{
			DisplayCancelButton();
		};
	}

	public void ShowOfferToBuyMoreFedorasForContinue(ref PurchasableItem fedoraItem, PopUpCallback okCallback, PopUpCallback cancelCallback, Action<bool> onDisplay)
	{
		m_OKButton.hidden = true;
		m_CheckMarkButton.hidden = true;
		if (!StoreManager.The.isWaitingForProducts)
		{
			StoreManager.The.GetProducts(OnConnectedToStore);
		}
		GreyOutBG();
		m_okCallback = okCallback;
		m_cancelCallback = cancelCallback;
		UIAnimation uIAnimation = m_popUpWindow.alphaFromTo(0.5f, 0f, 1f, Easing.Linear.easeInOut);
		m_popUpWindow.hidden = false;
		m_notEnoughTokensTitle.text = LocalTextManager.GetUIText("_NEED_MORE_FEDORAS_");
		string text = LocalTextManager.GetUIText("_PURCHASE_FEDORA_PACK_") + " " + LocalTextManager.GetUIText("_CONTINUE_");
		text.Replace(LocalTextManager.GetUIText("_UNLOCK_"), " ");
		m_notEnoughTokensTitle.hidden = false;
		uIAnimation = m_notEnoughTokensTitle.alphaFromTo(0.6f, 0f, 1f, Easing.Linear.easeInOut);
		m_TokenpurchaseSubText.text = text;
		UIHelper.LineBreakText(ref m_TokenpurchaseSubText, m_popUpWindow.width - (float)Screen.width * 0.05f);
		m_TokenpurchaseSubText.hidden = false;
		uIAnimation = m_TokenpurchaseSubText.alphaFromTo(0.6f, 0f, 1f, Easing.Linear.easeInOut);
		m_tokenPackTitle.text = LocalTextManager.GetStoreItemText(fedoraItem.m_nameLocKey);
		m_tokenPackTitle.hidden = false;
		uIAnimation = m_tokenPackTitle.alphaFromTo(0.6f, 0f, 1f, Easing.Linear.easeInOut);
		m_tokenPackSubTitle.text = LocalTextManager.GetStoreItemText(fedoraItem.m_descLocKey);
		m_tokenPackSubTitle.hidden = false;
		uIAnimation = m_tokenPackSubTitle.alphaFromTo(0.6f, 0f, 1f, Easing.Linear.easeInOut);
		m_tokenPackPrice.text = LocalTextManager.GetUIText("_MONEY_SYMBOL_") + " " + fedoraItem.m_realMoneyCost.ToString("N2");
		m_tokenPackPrice.hidden = true;
		m_tokenPackIcon = GlobalGUIManager.The.m_menuToolkit.addSprite(fedoraItem.m_iconFileName, 0, 0, m_popUpDepth);
		m_tokenPackIcon.parentUIObject = m_popUpWindow;
		m_tokenPackIcon.positionFromTopLeft(0.54f, 0.05f);
		m_tokenPackIcon.hidden = false;
		uIAnimation = m_tokenPackIcon.alphaFromTo(0.6f, 0f, 1f, Easing.Linear.easeInOut);
		m_OKButton.setSpriteImage("CashSaleButton.png");
		m_OKButton.SetHighlightedImage("CashSaleButtonOver.png");
		m_OKButton.userData = fedoraItem;
		uIAnimation.onComplete = delegate
		{
			DisplayCancelButton();
		};
		ShowVideoAdButton();
	}

	public void ShowOfferToBuyMoreTokensAndBuyItem(ref PurchasableItem item, ref PurchasableItem tokenItem, PopUpCallback okCallback, PopUpCallback cancelCallback, Action<bool> onDisplay)
	{
		m_OKButton.hidden = true;
		m_CheckMarkButton.hidden = true;
		if (!StoreManager.The.isWaitingForProducts)
		{
			StoreManager.The.GetProducts(OnConnectedToStore);
		}
		GreyOutBG();
		m_okCallback = okCallback;
		m_cancelCallback = cancelCallback;
		UIAnimation uIAnimation = m_popUpWindow.alphaFromTo(0.5f, 0f, 1f, Easing.Linear.easeInOut);
		m_popUpWindow.hidden = false;
		string text = LocalTextManager.GetUIText("_PURCHASE_TOKEN_PACK_") + " " + item.m_name + LocalTextManager.GetUIText("_QUESTION_MARK_END_");
		if (item.m_tokenCost > 0)
		{
			m_notEnoughTokensTitle.text = LocalTextManager.GetUIText("_NEED_MORE_TOKENS_");
		}
		else if (item.m_fedoraCost > 0)
		{
			m_notEnoughTokensTitle.text = LocalTextManager.GetUIText("_NEED_MORE_FEDORAS_");
			text = LocalTextManager.GetUIText("_PURCHASE_FEDORA_PACK_") + " " + item.m_name + LocalTextManager.GetUIText("_QUESTION_MARK_END_");
		}
		m_OKButton.setSpriteImage("CashSaleButton.png");
		m_OKButton.SetHighlightedImage("CashSaleButtonOver.png");
		m_notEnoughTokensTitle.hidden = false;
		uIAnimation = m_notEnoughTokensTitle.alphaFromTo(0.6f, 0f, 1f, Easing.Linear.easeInOut);
		m_TokenpurchaseSubText.text = text;
		UIHelper.LineBreakText(ref m_TokenpurchaseSubText, m_popUpWindow.width - (float)Screen.width * 0.05f);
		m_TokenpurchaseSubText.hidden = false;
		uIAnimation = m_TokenpurchaseSubText.alphaFromTo(0.6f, 0f, 1f, Easing.Linear.easeInOut);
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_TokenpurchaseSubText.textScale = 0.27f * m_popUpWindow.scale.x;
		}
		m_tokenPackTitle.text = LocalTextManager.GetStoreItemText(tokenItem.m_nameLocKey);
		m_tokenPackTitle.hidden = false;
		uIAnimation = m_tokenPackTitle.alphaFromTo(0.6f, 0f, 1f, Easing.Linear.easeInOut);
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_tokenPackTitle.textScale = 0.27f * m_popUpWindow.scale.x;
		}
		m_tokenPackSubTitle.text = LocalTextManager.GetStoreItemText(tokenItem.m_descLocKey);
		m_tokenPackSubTitle.hidden = false;
		uIAnimation = m_tokenPackSubTitle.alphaFromTo(0.6f, 0f, 1f, Easing.Linear.easeInOut);
		m_tokenPackPrice.text = LocalTextManager.GetUIText("_MONEY_SYMBOL_") + " " + tokenItem.m_realMoneyCost.ToString("N2");
		m_tokenPackPrice.hidden = true;
		m_tokenPackIcon = GlobalGUIManager.The.m_menuToolkit.addSprite(tokenItem.m_iconFileName, 0, 0, m_popUpDepth);
		m_tokenPackIcon.parentUIObject = m_popUpWindow;
		m_tokenPackIcon.positionFromTopLeft(0.54f, 0.05f);
		m_tokenPackIcon.hidden = false;
		uIAnimation = m_tokenPackIcon.alphaFromTo(0.6f, 0f, 1f, Easing.Linear.easeInOut);
		tokenItem.m_userData = item;
		m_OKButton.userData = tokenItem;
		uIAnimation.onComplete = delegate
		{
			DisplayCancelButton();
		};
	}

	private void GreyOutBG()
	{
		m_greyOutBG.alphaFromTo(0.5f, 0f, 0.5f, Easing.Linear.easeInOut);
		m_greyOutBG.hidden = false;
	}

	public void OnConnectedToStore(bool success)
	{
		Debug.Log("We connected to the store " + success + " " + m_popUpWindow.hidden);
		if (success && !m_popUpWindow.hidden)
		{
			if (m_OKButton.userData != null)
			{
				PurchasableItem purchasableItem = (PurchasableItem)m_OKButton.userData;
				m_tokenPackPrice.text = LocalTextManager.GetUIText("_MONEY_SYMBOL_") + " " + purchasableItem.m_realMoneyCost.ToString("N2");
			}
			m_OKButton.positionFromBottomRight(0.1f, 0.025f);
			m_tokenPackPrice.hidden = false;
			m_OKButton.hidden = false;
		}
	}

	public void JustDisplayOKButton()
	{
		m_OKButton.pixelsFromBottom(10);
		m_OKButton.hidden = false;
		m_CancelButton.hidden = true;
		m_CheckMarkButton.hidden = true;
	}

	private void DisplayCheckMarkButton()
	{
		m_CheckMarkButton.positionFromBottomRight(0.1f, 0.025f);
		m_CheckMarkButton.hidden = false;
	}

	public void DisplayOKandCancel()
	{
		m_OKButton.positionFromBottomRight(0.1f, 0.025f);
		m_OKButton.hidden = false;
		m_CancelButton.hidden = false;
		m_CheckMarkButton.hidden = true;
	}

	private void DisplayCancelButton()
	{
		m_CancelButton.hidden = false;
	}

	public void DisplayCheckMarkandCancel()
	{
		m_CheckMarkButton.positionFromBottomRight(0.1f, 0.025f);
		m_CheckMarkButton.hidden = false;
		m_CancelButton.hidden = false;
	}

	public void FakeTouchUpOkButton()
	{
		onTouchUpInsideDefaultClickOkButton(m_OKButton);
	}

	public void FakeTouchUpCancelButton()
	{
		onTouchUpInsideDefaultClickCancelButton(m_CancelButton);
	}

	public void onTouchUpInsideDefaultClickOkButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		m_OKButton.disabled = true;
		if (m_okCallback != null)
		{
			HideAll();
			PopUpCallback okCallback = m_okCallback;
			m_okCallback = null;
			okCallback(m_OKButton.userData);
		}
		else
		{
			Debug.Log("Default CLick OK -- you forgot to overload this");
		}
		m_OKButton.disabled = false;
		m_PopupType = PopUpType.None;
	}

	public void onTouchUpInsideDefaultClickCancelButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		HideAll();
		if (m_cancelCallback != null)
		{
			Debug.Log("calling this callback: " + m_cancelCallback);
			m_cancelCallback(m_CancelButton.userData);
		}
		else
		{
			Debug.Log("Default CLick Cancel -- you forgot to overload this");
		}
		m_PopupType = PopUpType.None;
	}

	public void onTouchUpInsideCheckMarkButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		m_CheckMarkButton.disabled = true;
		if (m_okCallback != null)
		{
			HideAll();
			PopUpCallback okCallback = m_okCallback;
			m_okCallback = null;
			okCallback(m_CheckMarkButton.userData);
		}
		else
		{
			Debug.Log("Default CLick OK -- you forgot to overload this");
		}
		m_CheckMarkButton.disabled = false;
		m_PopupType = PopUpType.None;
	}

	public void ReloadStaticText()
	{
		m_TokenpurchaseSubText.text = LocalTextManager.GetUIText("_PURCHASE_TOKEN_PACK_");
		m_missionTitle.text = LocalTextManager.GetUIText("_MISSION_COMPLETE_");
		m_badgeEarnedText.text = LocalTextManager.GetUIText("_LEVEL_PLUS_");
		m_multiplierEarnedTxt.text = LocalTextManager.GetUIText("_MULTIPLIER_PLUS_");
		m_scoreEarnedTxt.text = LocalTextManager.GetUIText("_SCORE_PLUS_");
		m_DefaultTitle.text = LocalTextManager.GetUIText("_BUY_MISSION_");
		m_DefaultBodyText.text = LocalTextManager.GetUIText("_BUY_MISSION_CONFIRMATION_");
		m_notEnoughTokensTitle.text = LocalTextManager.GetUIText("_NEED_MORE_TOKENS_");
		m_alternateCantConnectTxt.text = LocalTextManager.GetUIText("_CONNECT_TO_INTERNET_POPUP_");
		m_alternateCantConnectTitle.text = LocalTextManager.GetUIText("_CANNOT_ACCESS_");
	}

	public void onTouchUpInsideVideoAdButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		sender.disabled = true;
		sender.disabled = false;
	}
}

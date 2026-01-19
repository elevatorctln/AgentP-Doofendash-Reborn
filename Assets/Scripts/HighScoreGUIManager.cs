using System;
using System.Collections.Generic;
using PlayHaven;
using UnityEngine;

public class HighScoreGUIManager : MonoBehaviour
{
	public float m_downClip;

	public float m_upClip;

	private static UIScrollableVerticalLayout m_highScoreScroll;

	private UISlider m_scrollbar;

	private float m_totalHeight;

	private static UISprite m_highScoreScrollFrame;

	private static UIButton m_highScoreShareBtn;

	private static UITextInstance m_highScoreShareBtnLabel;

	private static UIButton m_highScoreFbBtn;

	private static UITextInstance m_highScoreFbBtnLabel;

	private static UITextInstance m_totalMeters;

	private static UITextInstance m_highScoreLabel;

	private static List<HighScoreElement> m_highScoreElements;

	private static UIButton m_moreGamesBtn;

	private static UITextInstance m_moreGamesLabel;

	public static UIButton m_freeTokenStoreButton;

	public static UITextInstance m_freeTokenStoreText;

	public static UIButton m_bee7Button;

	public static UISprite m_bee7Image;

	public static UITextInstance m_bee7Text;

	private static int m_highScoreDepth;

	private static HighScoreGUIManager m_the;

	private Dictionary<string, FaceTexture> m_faceTextures;

	public static HighScoreGUIManager The
	{
		get
		{
			if (m_the == null)
			{
				GameObject gameObject = new GameObject("HighScoreGUIManager");
				((HighScoreGUIManager)gameObject.AddComponent<HighScoreGUIManager>()).Init();
			}
			return m_the;
		}
	}

	private void OnEnable()
	{
		GameEventManager.GameRestartMenu += GameRestartMenuListener;
	}

	private void OnDisable()
	{
		GameEventManager.GameRestartMenu -= GameRestartMenuListener;
	}

	public void Init()
	{
		if (!(m_the != null))
		{
			m_the = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			m_highScoreElements = new List<HighScoreElement>();
			m_faceTextures = new Dictionary<string, FaceTexture>();
			InitHighScores();
			m_moreGamesBtn = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "MajescoButton.png", "MajescoButtonOver.png", 0, 0, m_highScoreDepth);
			m_moreGamesBtn.highlightedTouchOffsets = new UIEdgeOffsets(30);
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_moreGamesBtn);
			m_moreGamesBtn.positionFromTopLeft(0.005f, 0.15f);
			m_moreGamesBtn.onTouchUpInside += onTouchUpInsideMoreGamesButton;
			m_moreGamesLabel = GlobalGUIManager.The.defaultText.addTextInstance(LocalTextManager.GetUIText("_MORE_GAMES_"), 0f, 0f, 0.6f, m_highScoreDepth);
			m_moreGamesLabel.alignMode = UITextAlignMode.Center;
			m_moreGamesLabel.verticalAlignMode = UITextVerticalAlignMode.Top;
			m_moreGamesLabel.setColorForAllLetters(Color.black);
			m_moreGamesLabel.parentUIObject = m_moreGamesBtn;
			m_moreGamesLabel.positionFromCenter(0.65f, -0.05f);
			m_moreGamesLabel.text = LocalTextManager.GetUIText("_MORE_GAMES_");
			m_moreGamesLabel.textScale = 0.3f;
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				m_moreGamesLabel.textScale = 0f;
			}
			m_freeTokenStoreButton = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "PlayButtonOver.png", "PlayButtonOver.png", 0, 0, m_highScoreDepth + 1);
			m_freeTokenStoreButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
			m_freeTokenStoreButton.positionFromTopLeft(0.25f, 0.01f);
			m_freeTokenStoreButton.setSize(137f, 49f);
			m_freeTokenStoreButton.onTouchUpInside += onTouchUpInsideTrialPayButton;
			if (Screen.dpi >= 400f)
			{
				m_freeTokenStoreButton.scale = new Vector3(2f, 2f, 1f);
			}
			if (Screen.height >= 2048)
			{
				m_freeTokenStoreButton.scale = new Vector3(2.5f, 2.5f, 1f);
				m_freeTokenStoreButton.positionFromTopLeft(0.24f, 0.02f);
			}
			m_freeTokenStoreText = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_FREE_STUFF_"), 0f, 0f, 1f, m_highScoreDepth);
			m_freeTokenStoreText.alignMode = UITextAlignMode.Center;
			m_freeTokenStoreText.verticalAlignMode = UITextVerticalAlignMode.Middle;
			m_freeTokenStoreText.setColorForAllLetters(Color.white);
			m_freeTokenStoreText.parentUIObject = m_freeTokenStoreButton;
			m_freeTokenStoreText.positionFromCenter(0f, 0f);
			if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
			{
				m_freeTokenStoreText.textScale = ((!(Screen.dpi >= 400f)) ? 0.4f : 1.4f);
			}
			else
			{
				m_freeTokenStoreText.textScale = ((!(Screen.dpi >= 400f)) ? 0.5f : 1.4f);
			}
			m_bee7Button = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "PlayButton.png", "PlayButton.png", 0, 0, m_highScoreDepth + 1);
			m_bee7Button.highlightedTouchOffsets = new UIEdgeOffsets(30);
			m_bee7Button.positionFromTopLeft(0.45f, 0.01f);
			m_bee7Button.setSize(137f, 49f);
			m_bee7Button.onTouchUpInside += onTouchUpInsideBee7Button;
			if (Screen.dpi >= 400f)
			{
				m_bee7Button.scale = new Vector3(2f, 2f, 1f);
			}
			if (Screen.height >= 2048)
			{
				m_bee7Button.scale = new Vector3(3f, 3f, 1f);
				m_bee7Button.positionFromTopLeft(0.315f, 0.02f);
			}
			m_bee7Image = GlobalGUIManager.The.m_menuToolkit.addSprite("StarterPackIcon.png", 0, 0, m_highScoreDepth);
			m_bee7Image.parentUIObject = m_bee7Button;
			m_bee7Image.positionFromRight(-0f, 0.02f);
			m_bee7Image.scale = new Vector3(0.65f, 0.65f, 1f);
			m_bee7Text = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_FREE_GAMES_"), 0f, 0f, 1f, m_highScoreDepth);
			m_bee7Text.alignMode = UITextAlignMode.Left;
			m_bee7Text.verticalAlignMode = UITextVerticalAlignMode.Middle;
			m_bee7Text.setColorForAllLetters(Color.white);
			m_bee7Text.parentUIObject = m_bee7Button;
			m_bee7Text.positionFromLeft(0f, 0.05f);
			if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
			{
				m_bee7Text.textScale = ((!(Screen.dpi >= 400f)) ? 0.5f : 1.5f);
			}
			else
			{
				m_bee7Text.textScale = ((!(Screen.dpi >= 400f)) ? 0.5f : 1.5f);
			}
			HideAll();
		}
	}

	private void InitHighScores()
	{
		m_highScoreScrollFrame = GlobalGUIManager.The.m_menuToolkit.addSprite("HighScoresBGPlate.png", 0, 0, m_highScoreDepth + 30);
		m_highScoreScrollFrame.positionFromBottomRight(0.2f, 0.2f);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_highScoreScrollFrame);
		AddFacebookButton();
		m_highScoreScroll = new UIScrollableVerticalLayout(0);
		m_highScoreScroll.verticalAlignMode = UIAbstractContainer.UIContainerVerticalAlignMode.Top;
		m_highScoreScroll.alignMode = UIAbstractContainer.UIContainerAlignMode.Center;
		m_highScoreScroll.parentUIObject = m_highScoreScrollFrame;
		m_highScoreScroll.pixelsFromTopLeft(12, 4);
		m_highScoreScroll.setSize(m_highScoreScrollFrame.width, m_highScoreScrollFrame.height - m_highScoreScrollFrame.height / 7f);
		m_highScoreScroll.client.name = "highScoreScroll";
		m_highScoreScroll.onScrollChange += OnScrollChange;
		m_upClip = m_highScoreScroll.client.transform.position.y;
		m_downClip = m_highScoreScroll.client.transform.position.y - m_highScoreScroll.height;
		m_totalHeight = 0f;
		int num = 10;
		for (int i = 0; i < num; i++)
		{
			string text = "HighScorePlateB.png";
			if (i % 2 == 0)
			{
				text = "HighScorePlateA.png";
			}
			HighScoreElement highScoreElement = new HighScoreElement();
			highScoreElement.m_btn = UIButton.create(GlobalGUIManager.The.m_menuToolkit, text, text, 0, 0, m_highScoreDepth + 10);
			highScoreElement.m_btn.client.name = "highscore_" + i;
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref highScoreElement.m_btn);
			if (i == 0)
			{
				highScoreElement.isSet = true;
			}
			Color blue = Color.blue;
			int num2 = i + 1;
			string uIText = LocalTextManager.GetUIText("_YOU_");
			string text2 = PlayerData.HighestAllTimeScore.ToString("N0");
			highScoreElement.m_number = GlobalGUIManager.The.defaultTextAlt.addTextInstance(num2.ToString("N0"), 0f, 0f, 1f, m_highScoreDepth + 9);
			highScoreElement.m_number.alignMode = UITextAlignMode.Left;
			highScoreElement.m_number.verticalAlignMode = UITextVerticalAlignMode.Middle;
			highScoreElement.m_number.textScale = 0.5f * UIHelper.CalcFontScale();
			highScoreElement.m_number.setColorForAllLetters(blue);
			highScoreElement.m_number.parentUIObject = highScoreElement.m_btn;
			highScoreElement.m_number.pixelsFromLeft(5);
			float num3 = 1f;
			num3 = ((GameManager.The.aspectRatio.x == 3f || GameManager.The.aspectRatio.y == 4f) ? 0.45f : 0.3f);
			if (UI.scaleFactor == 1)
			{
				num3 /= 2f;
			}
			highScoreElement.m_name = GlobalGUIManager.The.defaultTextAlt.addTextInstance(uIText, 0f, 0f, 1f, m_highScoreDepth + 8);
			highScoreElement.m_name.alignMode = UITextAlignMode.Left;
			highScoreElement.m_name.verticalAlignMode = UITextVerticalAlignMode.Middle;
			highScoreElement.m_name.setColorForAllLetters(blue);
			highScoreElement.m_name.parentUIObject = highScoreElement.m_btn;
			highScoreElement.m_name.pixelsFromLeft(65);
			highScoreElement.m_score = GlobalGUIManager.The.defaultTextAlt.addTextInstance(text2, 0f, 0f, 1f, m_highScoreDepth + 7);
			highScoreElement.m_score.alignMode = UITextAlignMode.Right;
			highScoreElement.m_score.verticalAlignMode = UITextVerticalAlignMode.Middle;
			highScoreElement.m_score.textScale = 0.45f * UIHelper.CalcFontScale();
			highScoreElement.m_score.setColorForAllLetters(blue);
			highScoreElement.m_score.parentUIObject = highScoreElement.m_btn;
			highScoreElement.m_score.pixelsFromRight(14);
			highScoreElement.m_number.textScale = num3 * UIHelper.CalcFontScale();
			highScoreElement.m_name.textScale = num3 * UIHelper.CalcFontScale();
			highScoreElement.m_score.textScale = num3 * UIHelper.CalcFontScale();
			m_highScoreScroll.addChild(highScoreElement.m_btn);
			m_totalHeight += highScoreElement.m_btn.height;
			m_highScoreElements.Add(highScoreElement);
			if (!highScoreElement.isSet)
			{
				HideHighScoreElement(i);
			}
		}
		m_scrollbar = UISlider.create(GlobalGUIManager.The.m_menuToolkit, "HighScoreMenuSlider.png", "clear.png", 0, 0, UISliderLayout.Vertical, m_highScoreDepth);
		m_scrollbar.setSize(30f, m_highScoreScroll.height);
		m_scrollbar.parentUIObject = m_highScoreScrollFrame;
		m_scrollbar.positionFromLeft(0.0145f);
		m_scrollbar.updateTransform();
		m_scrollbar.highlightedTouchOffsets = new UIEdgeOffsets(30, 30, 30, 30);
		m_scrollbar.continuous = true;
		m_scrollbar.onChange += OnScrollBarChage;
		m_scrollbar.value = 0.01f;
		if (GameManager.The.aspectRatio.x == 3f && GameManager.The.aspectRatio.y == 4f)
		{
			m_scrollbar.positionFromLeft(0.024f);
		}
		else
		{
			m_scrollbar.positionFromLeft(0.019f);
		}
	}

	public void SortHighScores(bool show = false)
	{
		PerryLeaderboard.SortCombinedOrderedScores();
		if (PlayerData.PerryLeaderboards == null || PlayerData.PerryLeaderboards.Count == 0 || PlayerData.m_menuHighScoreIndex >= PlayerData.PerryLeaderboards.Count)
		{
			m_highScoreElements[0].m_name.text = LocalTextManager.GetUIText("_YOU_");
			m_highScoreElements[0].m_score.text = PlayerData.HighestAllTimeScore.ToString("N0");
			m_highScoreElements[0].isSet = true;
			for (int i = 1; i < m_highScoreElements.Count; i++)
			{
				m_highScoreElements[i].m_name.text = " ";
				m_highScoreElements[i].m_number.text = " ";
				m_highScoreElements[i].m_score.text = " ";
			}
			return;
		}
		int totalScores = PlayerData.PerryLeaderboards[PlayerData.m_menuHighScoreIndex].TotalScores;
		if (totalScores == 0)
		{
			return;
		}
		int num = 1;
		int num2 = 0;
		foreach (HighScoreElement highScoreElement in m_highScoreElements)
		{
			if (num2 < totalScores)
			{
				if (!highScoreElement.isFbButton)
				{
					if (PlayerData.PerryLeaderboards != null && PlayerData.m_menuHighScoreIndex < PlayerData.PerryLeaderboards.Count)
					{
						PerryHighScore combinedOrderedScoreAtIndex = PlayerData.PerryLeaderboards[PlayerData.m_menuHighScoreIndex].GetCombinedOrderedScoreAtIndex(num2);
						highScoreElement.m_name.text = combinedOrderedScoreAtIndex.Name;
						highScoreElement.m_number.text = num.ToString();
						if (combinedOrderedScoreAtIndex.IsMe)
						{
							highScoreElement.m_number.setColorForAllLetters(Color.blue);
							highScoreElement.m_name.setColorForAllLetters(Color.blue);
							highScoreElement.m_score.setColorForAllLetters(Color.blue);
							if (combinedOrderedScoreAtIndex.ScoreVal > PlayerData.HighestAllTimeScore)
							{
								PlayerData.HighestAllTimeScore = (int)combinedOrderedScoreAtIndex.ScoreVal;
							}
							if (PlayerData.GetTotalRoundScore() > PlayerData.HighestAllTimeScore)
							{
								highScoreElement.m_score.text = PlayerData.GetTotalRoundScore().ToString("N0");
							}
							else
							{
								highScoreElement.m_score.text = PlayerData.HighestAllTimeScore.ToString("N0");
							}
						}
						else
						{
							highScoreElement.m_number.setColorForAllLetters(Color.black);
							highScoreElement.m_name.setColorForAllLetters(Color.black);
							highScoreElement.m_score.setColorForAllLetters(Color.black);
							highScoreElement.m_score.text = combinedOrderedScoreAtIndex.ScoreVal.ToString("N0");
						}
						if (combinedOrderedScoreAtIndex.FaceUrl != string.Empty && combinedOrderedScoreAtIndex.FaceUrl.Length > 1)
						{
							if (m_faceTextures.ContainsKey(combinedOrderedScoreAtIndex.PlayerID))
							{
								if (highScoreElement.m_faceTexture != null)
								{
									highScoreElement.m_faceTexture.GetComponent<Renderer>().enabled = false;
									highScoreElement.m_faceTexture.parent = null;
									highScoreElement.m_faceTexture = null;
								}
								m_faceTextures[combinedOrderedScoreAtIndex.PlayerID].parent = highScoreElement.m_btn;
								highScoreElement.m_faceTexture = m_faceTextures[combinedOrderedScoreAtIndex.PlayerID];
								if (show)
								{
									highScoreElement.m_faceTexture.GetComponent<Renderer>().enabled = true;
									highScoreElement.m_faceTexture.ReloadTexture();
								}
							}
							else
							{
								highScoreElement.AddFaceTexture(combinedOrderedScoreAtIndex.FaceUrl);
								highScoreElement.m_faceTexture.downClip = m_downClip;
								highScoreElement.m_faceTexture.upClip = m_upClip;
								highScoreElement.m_faceTexture.SetClipping();
								m_faceTextures.Add(combinedOrderedScoreAtIndex.PlayerID, highScoreElement.m_faceTexture);
								if (!show)
								{
									highScoreElement.m_faceTexture.gameObject.GetComponent<Renderer>().enabled = false;
								}
							}
						}
						highScoreElement.isSet = true;
						num++;
					}
					else
					{
						highScoreElement.isSet = false;
					}
				}
				if (show)
				{
					ShowHighScoreElement(num2);
				}
			}
			else
			{
				highScoreElement.isSet = false;
				HideHighScoreElement(num2);
			}
			num2++;
		}
		m_highScoreScroll.scrollTo(0, false);
	}

	private void AddFacebookButton()
	{
		string filename = "ShareScreen.png";
		string highlightedFilename = "ShareScreenOver.png";
		m_highScoreShareBtn = UIButton.create(GlobalGUIManager.The.m_menuToolkit, filename, highlightedFilename, 0, 0, m_highScoreDepth + 3);
		m_highScoreShareBtn.client.name = "m_highScoreShareBtn";
		m_highScoreShareBtn.parentUIObject = m_highScoreScrollFrame;
		m_highScoreShareBtn.positionFromTopLeft(-0.22f, -0.02f);
		m_highScoreShareBtn.onTouchUpInside += onTouchUpInsideShareBtn;
		m_highScoreShareBtnLabel = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_SHARE_BUTTON_"), 0f, 0f, 1f, m_highScoreDepth + 2);
		m_highScoreShareBtn.client.name = "m_highScoreShareBtnLabel";
		m_highScoreShareBtnLabel.parentUIObject = m_highScoreShareBtn;
		m_highScoreShareBtnLabel.positionCenter();
		m_highScoreShareBtnLabel.alignMode = UITextAlignMode.Center;
		m_highScoreShareBtnLabel.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_highScoreShareBtnLabel.textScale = 0.34f;
		m_highScoreShareBtnLabel.setColorForAllLetters(Color.white);
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_highScoreShareBtnLabel.textScale = 0.27f;
		}
		else
		{
			m_highScoreShareBtnLabel.textScale = 0.34f;
		}
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{

		}
		else
		{

		}
	}

	public void onTouchUpInsideFacebookBtn(UIButton btn)
	{
		return;
	}

	public void onTouchUpInsideShareBtn(UIButton btn)
	{
		if (!GameManager.The.playerIsUnderThirteen)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				DisableSocialButtons();
				PopUpGUIManager.The.ShowNoInternetNotificationPopup(OnNoInternetPopUpDismissed, delegate
				{
				});
			}
			else
			{
				string[] items = new string[1] { LocalTextManager.GetUIText("_SHARING_MESSAGE_").Replace("SCORE", PlayerData.GetTotalRoundScore().ToString()) };
				Social.Instance.ShareItems(items);
			}
		}
	}

	private void GameRestartMenuListener()
	{

	}

	private void OnNoInternetPopUpDismissed(object o)
	{
		EnableSocialButtons();
	}

	public void DisableSocialButtons()
	{
		
	}

	public void EnableSocialButtons()
	{
		
	}

	public void HideAll()
	{
		HideHighScores();
		m_moreGamesBtn.hidden = true;
		m_moreGamesLabel.hidden = true;
		m_freeTokenStoreButton.hidden = true;
		m_freeTokenStoreText.hidden = true;
		m_bee7Button.hidden = true;
		m_bee7Image.hidden = true;
		m_bee7Text.hidden = true;
	}

	public void HideHighScores()
	{
		m_highScoreScrollFrame.hidden = true;
		m_scrollbar.hidden = true;
		if (m_highScoreShareBtn != null)
		{
			m_highScoreShareBtn.hidden = true;
		}
		if (m_highScoreShareBtnLabel != null)
		{
			m_highScoreShareBtnLabel.hidden = true;
		}
		m_highScoreScroll.hidden = true;
		for (int i = 0; i < m_highScoreElements.Count; i++)
		{
			HideHighScoreElement(i);
		}
		m_highScoreScroll.hidden = true;
	}

	public void HideHighScoreElement(int i)
	{
		if (m_highScoreElements[i].m_btn != null)
		{
			m_highScoreElements[i].m_btn.hidden = true;
		}
		if (m_highScoreElements[i].m_face != null)
		{
			m_highScoreElements[i].m_face.hidden = true;
		}
		if (m_highScoreElements[i].m_name != null)
		{
			m_highScoreElements[i].m_name.hidden = true;
		}
		if (m_highScoreElements[i].m_number != null)
		{
			m_highScoreElements[i].m_number.hidden = true;
		}
		if (m_highScoreElements[i].m_score != null)
		{
			m_highScoreElements[i].m_score.hidden = true;
		}
		if (m_highScoreElements[i].m_faceTexture != null)
		{
			m_highScoreElements[i].m_faceTexture.GetComponent<Renderer>().enabled = false;
		}
	}

	public static void HideFbButton()
	{

	}

	public void ShowHighScoresInGameMenu()
	{
		m_moreGamesBtn.hidden = false;
		m_moreGamesLabel.hidden = false;
		m_freeTokenStoreButton.hidden = false;
		m_freeTokenStoreText.hidden = false;
		m_bee7Button.hidden = false;
		m_bee7Image.hidden = false;
		m_bee7Text.hidden = false;
		if (PlayerData.PerryLeaderboards != null && PlayerData.PerryLeaderboards.Count > 0)
		{
			SortHighScores(true);
		}
		else
		{
			m_highScoreElements[0].m_name.text = LocalTextManager.GetUIText("_YOU_");
		}
		m_highScoreScrollFrame.hidden = false;
		m_scrollbar.hidden = false;
		m_highScoreScroll.hidden = false;
		for (int i = 0; i < m_highScoreElements.Count; i++)
		{
			ShowHighScoreElement(i);
		}
		m_highScoreScroll.scrollTo(0, false);
	}

	private void ShowHighScoreElement(int i)
	{
		if (!m_highScoreElements[i].isSet)
		{
			HideHighScoreElement(i);
			return;
		}
		if (m_highScoreElements[i].m_btn != null)
		{
			m_highScoreElements[i].m_btn.hidden = false;
		}
		if (m_highScoreElements[i].m_face != null)
		{
			m_highScoreElements[i].m_face.hidden = false;
		}
		if (m_highScoreElements[i].m_name != null)
		{
			m_highScoreElements[i].m_name.hidden = false;
		}
		if (m_highScoreElements[i].m_number != null)
		{
			m_highScoreElements[i].m_number.hidden = false;
		}
		if (m_highScoreElements[i].m_score != null)
		{
			m_highScoreElements[i].m_score.hidden = false;
		}
		if (m_highScoreElements[i].m_faceTexture != null)
		{
			m_highScoreElements[i].m_faceTexture.GetComponent<Renderer>().enabled = true;
			m_upClip = m_highScoreScroll.client.transform.position.y;
			m_downClip = m_highScoreScroll.client.transform.position.y - m_highScoreScroll.height;
			m_highScoreElements[i].m_faceTexture.upClip = m_upClip;
			m_highScoreElements[i].m_faceTexture.downClip = m_downClip;
			m_highScoreElements[i].m_faceTexture.SetClipping();
		}
	}

	public void UpdateHighScoresForInGameMenu()
	{
		m_highScoreScrollFrame.positionFromBottomRight(0.12f, 0.115f);
		m_highScoreScroll.scrollTo(0, false);
	}

	public void MoveHighScores(float offset, bool bAnimate = false, Action<bool> onDone = null)
	{
		Vector3 position = m_highScoreScrollFrame.position;
		Vector3 target = m_highScoreScrollFrame.position + new Vector3(offset, 0f, 0f);
		if (bAnimate)
		{
			UIAnimation uIAnimation = m_highScoreScrollFrame.positionFromTo(1f, position, target, Easing.Sinusoidal.easeInOut);
			uIAnimation.onComplete = delegate
			{
				ReloadStaticText();
				if (onDone != null)
				{
					onDone(true);
				}
			};
		}
		else
		{
			m_highScoreScrollFrame.position = new Vector3(target.x, target.y, target.z);
		}
	}

	private void OnScrollBarChage(UISlider sender, float val)
	{
		int num = (int)((1f - val) * (m_totalHeight - m_highScoreScroll.height));
		m_highScoreScroll.scrollTo(-num, false);
	}

	private void OnScrollChange(UIScrollableVerticalLayout sender, float val)
	{
		if (m_scrollbar != null && !m_scrollbar.highlighted)
		{
			float value = val / (m_totalHeight - m_highScoreScroll.height);
			m_scrollbar.value = 1f - Mathf.Clamp01(value);
		}
	}

	public void ReloadStaticText()
	{
		m_highScoreShareBtnLabel.text = LocalTextManager.GetUIText("_SHARE_BUTTON_");
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_highScoreShareBtnLabel.textScale = 0.2f;
		}
		else
		{
			m_highScoreShareBtnLabel.textScale = 0.34f;
		}
		if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Russian)
		{
			m_highScoreShareBtnLabel.textScale = 0.15f;
		}
	}

	public void onTouchUpInsideMoreGamesButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			GameManager.The.PauseMusic();
			#if UNITY_ANDROID
			PlayHavenManager.instance.ContentRequest("more_games");
			#endif
			FlurryFacade.Instance.LogEvent("MoreGamesClicked");
		}
	}

	public void PlayHavenManagerinstanceOnDismissContentListener(int requestId, DismissType dismissType)
	{
		GameManager.The.ResumeMusic();
	}

	public void onTouchUpInsideTrialPayButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Store_Menu_FreeTokens);
	}

	public void onTouchUpInsideBee7Button(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
	}
}

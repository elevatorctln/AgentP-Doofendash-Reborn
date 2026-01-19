using System;
using UnityEngine;

public class MonogramGUIManager : MonoBehaviour
{
	public delegate void MonogramCallback();

	private UISprite m_monogramHead;

	private UISprite[] m_monogramMouths;

	private int[] m_monogramMouthOpen;

	private int[] m_monogramMouthClose;

	private UISprite m_tvMonitor;

	private UIButton m_monogramButton;

	private UISprite m_storyBox;

	private UITextInstance m_dialogueText;

	private int m_monogramDepth = 3;

	private static bool m_isMouthMoving;

	private static bool m_isMouthOpen;

	private static int m_lastMouthIndex;

	private static float m_mouthStateDuration;

	private static float m_mouthChangeSpeedMin = 0.05f;

	private static float m_mouthChangeSpeedMax = 0.2f;

	private static float m_mouthChangeTime;

	private static float m_timer;

	private static string m_currentText = string.Empty;

	private static string m_fullText = string.Empty;

	private static int m_currentTextLineFeedCount;

	private bool m_TalkBoxWaitForCancel;

	private static MonogramGUIManager m_the;

	public static MonogramGUIManager The
	{
		get
		{
			if (m_the == null)
			{
				GameObject gameObject = new GameObject("MonogramGUIManager");
				((MonogramGUIManager)gameObject.AddComponent<MonogramGUIManager>()).Init();
			}
			return m_the;
		}
	}

	private void Update()
	{
		if (m_isMouthMoving)
		{
			m_mouthChangeTime += Time.deltaTime;
			if (m_mouthChangeTime > m_mouthStateDuration)
			{
				RefreshMonogramMouthSprite();
			}
		}
		if (m_currentText.Length >= m_fullText.Length || m_dialogueText == null)
		{
			return;
		}
		m_timer -= Time.deltaTime;
		if (m_timer < 0f)
		{
			if (m_currentTextLineFeedCount == 4)
			{
				m_fullText = m_fullText.Substring(m_currentText.Length);
				m_currentText = string.Empty;
				m_currentTextLineFeedCount = 0;
				m_timer = 1f;
				return;
			}
			m_timer = 0.035f;
			char c = m_fullText[m_currentText.Length];
			if (c == '\n')
			{
				m_currentTextLineFeedCount++;
			}
			m_currentText += m_fullText[m_currentText.Length];
			m_dialogueText.text = m_currentText;
		}
		if (m_currentText.Length == m_fullText.Length && m_currentText.Length > 0 && !m_TalkBoxWaitForCancel)
		{
			MongramTalkingHandler.The.StopPlayTextWithDelay(1f);
		}
	}

	public void Init()
	{
		if (!(m_the != null))
		{
			m_the = this;
			InitMonogramMonitor();
			InitDialogueText();
			HideAll();
		}
	}

	public void InitMonogramMonitor()
	{
		m_tvMonitor = GlobalGUIManager.The.m_menuToolkit.addSprite("MonogramTV.png", 0, 0, m_monogramDepth + 4);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_tvMonitor);
		m_monogramButton = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "MissionsButton.png", "MissionsButtonOver.png", 0, 0, m_monogramDepth + 4);
		m_monogramButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_monogramButton.onTouchUpInside += onTouchUpInsideMonogramButton;
		m_monogramButton.hidden = true;
		m_monogramHead = GlobalGUIManager.The.m_menuToolkit.addSprite("Monogram.png", 0, 0, m_monogramDepth + 3);
		m_monogramHead.parentUIObject = m_tvMonitor;
		m_monogramHead.positionFromCenter(0.03f, 0f);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_monogramHead);
		m_monogramMouths = new UISprite[5];
		m_monogramMouths[0] = GlobalGUIManager.The.m_menuToolkit.addSprite("MonogramMouth3.png", 0, 0, m_monogramDepth + 2, true);
		m_monogramMouths[1] = GlobalGUIManager.The.m_menuToolkit.addSprite("MonogramMouth5.png", 0, 0, m_monogramDepth + 2, true);
		m_monogramMouths[2] = GlobalGUIManager.The.m_menuToolkit.addSprite("MonogramMouth2.png", 0, 0, m_monogramDepth + 2, true);
		m_monogramMouths[3] = GlobalGUIManager.The.m_menuToolkit.addSprite("MonogramMouth4.png", 0, 0, m_monogramDepth + 2, true);
		m_monogramMouths[4] = GlobalGUIManager.The.m_menuToolkit.addSprite("MonogramMouth1.png", 0, 0, m_monogramDepth + 2, true);
	}

	public void InitDialogueText()
	{
		m_dialogueText = GlobalGUIManager.The.defaultTextAlt.addTextInstance("Default Dialouge Text", 0f, 0f, 1f, m_monogramDepth + 1);
		m_dialogueText.alignMode = UITextAlignMode.Center;
		m_dialogueText.textScale = 0.9f;
		m_dialogueText.localScale = Vector3.one;
		m_storyBox = GlobalGUIManager.The.m_hudToolkit.addSprite("StoryTextBox.png", 0, 0, m_monogramDepth + 10, true);
		m_storyBox.positionFromTop(0f);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_storyBox);
	}

	public void EnableMonogramButton()
	{
		m_monogramButton.disabled = false;
	}

	public void DisableMonogramButton()
	{
		m_monogramButton.disabled = true;
	}

	public void HideAll()
	{
		HideMonogramMonitor();
		HideDialogueText();
	}

	public void HideMonogramMonitor()
	{
		m_tvMonitor.hidden = true;
		m_monogramButton.hidden = true;
		m_monogramHead.hidden = true;
		m_storyBox.hidden = true;
		HideMouths();
	}

	private void HideMouths()
	{
		m_isMouthMoving = false;
		for (int i = 0; i < m_monogramMouths.Length; i++)
		{
			m_monogramMouths[i].hidden = true;
		}
	}

	public void HideDialogueText()
	{
		m_dialogueText.hidden = true;
	}

	public void ShowMainMenu()
	{
		UpdateMonogramPosForMissionMenu();
		UpdateDialoguePosForMainMenu();
		ShowMonogramMonitor();
	}

	public void ShowInGameMenu()
	{
		UpdateMonogramPosForInGameMenu();
		UpdateDialoguePosForInGameMenu();
		ShowMonogramMonitorButton();
	}

	public void ShowStory()
	{
		UpdateMonogramPosForStory();
		UpdateDialoguePosForStory();
		ShowMonogramMonitor();
		ShowDialogueText();
	}

	public void ShowGameIntro()
	{
		UpdateMonogramPosForGameIntro();
		UpdateDialoguePosForGameIntro();
		ShowMonogramMonitor();
	}

	public void ShowBossTutorial()
	{
		UpdateMonogramPosForGameIntro();
		UpdateDialoguePosForGameIntro();
		ShowMonogramMonitor();
	}

	public void ShowMonogramMonitor()
	{
		m_tvMonitor.hidden = false;
		m_monogramHead.hidden = false;
		m_monogramMouths[m_lastMouthIndex].hidden = true;
		m_lastMouthIndex = ChooseNextRandomCloseMouthIndex();
		m_monogramMouths[m_lastMouthIndex].hidden = false;
		m_isMouthOpen = false;
	}

	public void ShowMonogramMonitorButton()
	{
		m_monogramButton.hidden = false;
		m_monogramMouths[m_lastMouthIndex].hidden = true;
		m_lastMouthIndex = ChooseNextRandomCloseMouthIndex();
		m_monogramMouths[m_lastMouthIndex].hidden = false;
		m_isMouthOpen = false;
	}

	private void ShowDialogueText()
	{
		m_dialogueText.hidden = false;
		m_storyBox.hidden = false;
	}

	public void StartTalking()
	{
		m_mouthChangeTime = 0f;
		m_isMouthMoving = true;
	}

	public void StopTalking()
	{
		if (m_isMouthMoving)
		{
			m_isMouthMoving = false;
			m_monogramMouths[m_lastMouthIndex].hidden = true;
			m_lastMouthIndex = ChooseNextRandomCloseMouthIndex();
			m_monogramMouths[m_lastMouthIndex].hidden = false;
			m_isMouthOpen = false;
		}
	}

	public void StartTalkBoxGameIntroPos(string dialogue, bool waitForCancel = false, int charsPerLine = 35)
	{
		UpdateDialoguePosForGameIntro();
		m_storyBox.positionFromTop(0f);
		if (LocalTextManager.isAsianLanguageActive)
		{
			charsPerLine = 17;
		}
		StartTalkBox(dialogue, waitForCancel, charsPerLine);
	}

	public void StartTalkBox(string dialogue, bool waitForCancel = false, int charsPerLine = 35)
	{
		m_storyBox.hidden = false;
		m_dialogueText.hidden = true;
		m_dialogueText.text = string.Empty;
		m_dialogueText.hidden = false;
		m_currentText = string.Empty;
		m_currentTextLineFeedCount = 0;
		m_fullText = UIHelper.WordWrap(dialogue, charsPerLine);
		if (LocalTextManager.isAsianLanguageActive)
		{
			charsPerLine = 17;
		}
		m_TalkBoxWaitForCancel = waitForCancel;
	}

	public void CancelTalkBox()
	{
		m_TalkBoxWaitForCancel = false;
	}

	public void StopTalkBox()
	{
		m_storyBox.hidden = true;
		HideDialogueText();
	}

	private void UpdateMonogramPosForMissionMenu()
	{
		float num = 1f;
		m_tvMonitor.parentUIObject = null;
		m_tvMonitor.positionFromTop(0.1f);
		m_tvMonitor.scale = new Vector3(num, num, 1f);
		m_monogramHead.parentUIObject = m_tvMonitor;
		m_monogramHead.positionFromBottom(0.1f);
		m_monogramHead.scale = new Vector3(num, num, 1f);
		for (int i = 0; i < m_monogramMouths.Length; i++)
		{
			m_monogramMouths[i].parentUIObject = m_monogramHead;
			m_monogramMouths[i].positionFromBottom(0.22f, 0.12f);
			m_monogramMouths[i].scale = new Vector3(num, num, 1f);
			m_monogramMouths[i].parentUIObject = m_tvMonitor;
		}
	}

	private void UpdateDialoguePosForMainMenu()
	{
		m_dialogueText.alignMode = UITextAlignMode.Center;
		m_dialogueText.parentUIObject = m_tvMonitor;
		m_dialogueText.textScale = 0.9f;
		m_dialogueText.localScale = Vector3.one;
	}

	private void UpdateMonogramPosForInGameMenu()
	{
		float num = 0.44f;
		m_monogramButton.hidden = false;
		m_monogramButton.positionFromTopLeft(0.1f, 0.045f);
		for (int i = 0; i < m_monogramMouths.Length; i++)
		{
			m_monogramMouths[i].parentUIObject = m_monogramButton;
			m_monogramMouths[i].positionFromBottom(0.3122f, 0.041f);
			m_monogramMouths[i].scale = new Vector3(num, num, 1f);
		}
		m_monogramHead.parentUIObject = m_monogramButton;
		m_monogramHead.positionFromBottom(0.1f);
		m_monogramHead.scale = new Vector3(num, num, 1f);
	}

	private void UpdateDialoguePosForInGameMenu()
	{
	}

	private void UpdateMonogramPosForGameIntro()
	{
		float num = 0.48f;
		m_tvMonitor.parentUIObject = null;
		m_tvMonitor.positionFromTopLeft(0.007f, 0.01f);
		m_tvMonitor.scale = new Vector3(num, num, 1f);
		if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f)
		{
			m_tvMonitor.positionFromTopLeft(0.003f, 0.01f);
			m_tvMonitor.scale = new Vector3(0.43f, 0.43f, 1f);
		}
		if (Screen.dpi >= 240f)
		{
			if (Screen.dpi >= 320f)
			{
				m_tvMonitor.scale = new Vector3(0.53f, 0.53f, 1f);
			}
			else
			{
				m_tvMonitor.scale = new Vector3(0.35f, 0.35f, 1f);
			}
		}
		else
		{
			m_tvMonitor.scale = new Vector3(0.43f, 0.43f, 1f);
		}
		m_monogramHead.parentUIObject = m_tvMonitor;
		m_monogramHead.positionFromBottom(0.1f);
		m_monogramHead.scale = new Vector3(num, num, 1f);
		if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f)
		{
			m_monogramHead.scale = new Vector3(0.43f, 0.43f, 1f);
		}
		if (Screen.dpi >= 240f)
		{
			if (Screen.dpi >= 320f)
			{
				m_monogramHead.scale = new Vector3(0.53f, 0.53f, 1f);
			}
			else
			{
				m_monogramHead.scale = new Vector3(0.35f, 0.35f, 1f);
			}
		}
		else
		{
			m_monogramHead.scale = new Vector3(0.43f, 0.43f, 1f);
		}
		for (int i = 0; i < m_monogramMouths.Length; i++)
		{
			m_monogramMouths[i].parentUIObject = m_monogramHead;
			m_monogramMouths[i].positionFromBottom(0.22f, 0.12f);
			m_monogramMouths[i].scale = new Vector3(num, num, 1f);
			m_monogramMouths[i].parentUIObject = m_tvMonitor;
			if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f)
			{
				m_monogramMouths[i].scale = new Vector3(0.43f, 0.43f, 1f);
			}
			if (Screen.dpi >= 240f)
			{
				if (Screen.dpi >= 320f)
				{
					m_monogramMouths[i].scale = new Vector3(0.53f, 0.53f, 1f);
				}
				else
				{
					m_monogramMouths[i].scale = new Vector3(0.35f, 0.35f, 1f);
				}
			}
			else
			{
				m_monogramMouths[i].scale = new Vector3(0.43f, 0.43f, 1f);
			}
		}
	}

	private void UpdateDialoguePosForGameIntro()
	{
		m_dialogueText.alignMode = UITextAlignMode.Left;
		m_dialogueText.parentUIObject = m_tvMonitor;
		m_dialogueText.positionFromTopLeft(0.001f, 1.1f);
		m_dialogueText.parentUIObject = null;
		if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f)
		{
			m_dialogueText.textScale = 0.3f * UIHelper.CalcFontScale();
		}
		else
		{
			m_dialogueText.textScale = 0.45f * UIHelper.CalcFontScale();
		}
		float num = ((Screen.height > 1280 || !(Screen.dpi <= 320f)) ? 0.3f : 0.45f);
		if (Screen.height >= 1920)
		{
			if (Screen.dpi >= 400f)
			{
				num = 0.2f;
			}
			else if (Screen.dpi > 320f)
			{
				num = 0.25f;
			}
			else if (Screen.dpi > 250f)
			{
				num = 0.3f;
			}
		}
		else if (Screen.height >= 1820 && Screen.dpi > 320f)
		{
			num = 0.27f;
		}
		num *= 0.8f;
		m_dialogueText.textScale = num * UIHelper.CalcFontScale();
	}

	private void UpdateMonogramPosForStory()
	{
		float num = 0.55f;
		m_tvMonitor.parentUIObject = null;
		m_tvMonitor.scale = new Vector3(num, num, 1f);
		m_tvMonitor.positionFromTopLeft(0.2f, 0.01f);
		m_monogramHead.parentUIObject = m_tvMonitor;
		m_monogramHead.positionFromBottom(0.06f);
		m_monogramHead.scale = new Vector3(num, num, 1f);
		for (int i = 0; i < m_monogramMouths.Length; i++)
		{
			m_monogramMouths[i].parentUIObject = m_monogramHead;
			m_monogramMouths[i].positionFromBottom(0.22f, 0.12f);
			m_monogramMouths[i].scale = new Vector3(num, num, 1f);
			m_monogramMouths[i].parentUIObject = m_tvMonitor;
		}
	}

	private void UpdateDialoguePosForStory()
	{
		m_storyBox.positionFromBottom(0f);
		UIHelper.ResizeSpriteToWidth(ref m_storyBox, Screen.width);
		m_dialogueText.alignMode = UITextAlignMode.Left;
		m_dialogueText.parentUIObject = m_storyBox;
		m_dialogueText.positionFromTopLeft(0.01f, 0.01f);
		m_dialogueText.textScale = 0.45f;
		m_dialogueText.localScale = Vector3.one;
	}

	private void RefreshMonogramMouthSprite()
	{
		m_mouthChangeTime = 0f;
		m_mouthStateDuration = UnityEngine.Random.Range(m_mouthChangeSpeedMin, m_mouthChangeSpeedMax);
		if (m_monogramMouths != null)
		{
			m_monogramMouths[m_lastMouthIndex].hidden = true;
			m_lastMouthIndex = ChooseNextRandomLipIndex();
			m_monogramMouths[m_lastMouthIndex].hidden = false;
		}
	}

	private int ChooseNextRandomLipIndex()
	{
		if (m_isMouthOpen)
		{
			m_isMouthOpen = false;
			return ChooseNextRandomCloseMouthIndex();
		}
		m_isMouthOpen = true;
		return ChooseNextRandomOpenMouthIndex();
	}

	private int ChooseNextRandomCloseMouthIndex()
	{
		int min = 2;
		int max = 4;
		return UnityEngine.Random.Range(min, max);
	}

	private int ChooseNextRandomOpenMouthIndex()
	{
		int min = 0;
		int max = 2;
		return UnityEngine.Random.Range(min, max);
	}

	public void MoveMonogram(float xOffset, float yOffset, bool bAnimate = false, Action<bool> onDone = null)
	{
		Vector3 position = m_monogramButton.position;
		Vector3 target = m_monogramButton.position + new Vector3(xOffset, yOffset, 0f);
		if (!bAnimate)
		{
			return;
		}
		UIAnimation uIAnimation = m_monogramButton.positionFromTo(1f, position, target, Easing.Sinusoidal.easeInOut);
		uIAnimation.onComplete = delegate
		{
			if (onDone != null)
			{
				onDone(true);
			}
		};
	}

	public void onTouchUpInsideMonogramButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		sender.disabled = true;
		MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Main_Menu_Missions);
		sender.disabled = false;
		m_monogramButton.hidden = true;
	}
}

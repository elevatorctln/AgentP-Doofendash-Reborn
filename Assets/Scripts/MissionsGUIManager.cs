using System;
using System.Collections;
using UnityEngine;

public class MissionsGUIManager : MonoBehaviour
{
	public struct MissionElement
	{
		public UIButton m_frameBtn;

		public UITextInstance m_desc;

		public UITextInstance m_equalsign;

		public UISprite m_badge;

		public UISprite m_flashBtn;

		public UISprite m_doneCheckMark;
	}

	private static UISprite m_levelFrame;

	private static UISprite m_multiplierIcon;

	private static UITextInstance m_levelTitle;

	private static UISprite[] m_badges;

	private static UISprite[] m_emptyBadges;

	private static UITextInstance m_equalScoreMultText;

	private static UIHorizontalLayout m_levelBadgeLayout;

	private static UIVerticalLayout m_missionLayout;

	private static UISprite m_missionsFrame;

	private static UITextInstance m_missionsLabel;

	private static UITextInstance m_noMissionText;

	private static MissionElement[] m_missionElements;

	private static UITextInstance m_tapInstructions;

	private static int m_missionStartDepth = 4;

	private static bool m_IsShowingAll;

	private int MAX_LEVEL_BADGES = 3;

	private static MissionsGUIManager m_the;

	private bool m_IsInited;

	public static MissionsGUIManager The
	{
		get
		{
			if (m_the == null)
			{
				GameObject gameObject = new GameObject("MissionsGUIManager");
				m_the = (MissionsGUIManager)gameObject.AddComponent<MissionsGUIManager>();
				m_the.Init();
			}
			return m_the;
		}
	}

	public void Init()
	{
		if (!m_IsInited)
		{
			m_IsInited = true;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			InitMissions();
			HideMissions();
			HideLevel();
		}
	}

	private void InitMissions()
	{
		CreateMissionsGUI();
		CreateMissionLevelGUI();
	}

	public void CreateMissionsGUI()
	{
		m_missionsFrame = GlobalGUIManager.The.m_menuToolkit.addSprite("MissionsPlate.png", 0, 0, m_missionStartDepth + 4);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_missionsFrame);
		m_missionsLabel = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_MISSIONS_CONTAINER_TITLE_"), 0f, 0f, 1f, m_missionStartDepth + 2);
		m_missionsLabel.alignMode = UITextAlignMode.Left;
		m_missionsLabel.verticalAlignMode = UITextVerticalAlignMode.Top;
		m_missionsLabel.parentUIObject = m_missionsFrame;
		m_missionsLabel.setColorForAllLetters(Color.white);
		m_missionsLabel.positionFromTopLeft(0.02f, 0.2f);
		m_missionsLabel.text = LocalTextManager.GetUIText("_MISSIONS_CONTAINER_TITLE_");
		if (GameManager.The.aspectRatio.x == 3f && GameManager.The.aspectRatio.y == 4f)
		{
			m_missionsLabel.textScale = 0.64f * UIHelper.CalcFontScale();
		}
		else
		{
			m_missionsLabel.textScale = 0.45f * UIHelper.CalcFontScale();
		}
		m_missionsLabel.textScale = 0.35f * UIHelper.CalcFontScale();
		m_noMissionText = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_MISSIONS_NO_NEW_DEFAULT_"), 0f, 0f, 1f, m_missionStartDepth);
		m_noMissionText.alignMode = UITextAlignMode.Center;
		m_noMissionText.parentUIObject = m_missionsFrame;
		m_noMissionText.textScale = 0.7f * UIHelper.CalcFontScale();
		m_noMissionText.setColorForAllLetters(Color.white);
		m_noMissionText.positionFromCenter(0f, 0f);
		m_noMissionText.text = LocalTextManager.GetUIText("_MISSIONS_NO_NEW_DEFAULT_");
		m_missionLayout = new UIVerticalLayout(-5);
		m_missionLayout.parentUIObject = m_missionsFrame;
		m_missionLayout.positionFromTopLeft(0.1f, 0.01f);
		if (AllMissionData.AreAllMissionsCompleted())
		{
			m_noMissionText.hidden = false;
		}
		CreateMissionElements();
		m_tapInstructions = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_MISSIONS_SKIP_INSTRUCTIONS_"), 0f, 0f, m_missionStartDepth);
		m_tapInstructions.alignMode = UITextAlignMode.Center;
		m_tapInstructions.verticalAlignMode = UITextVerticalAlignMode.Bottom;
		m_tapInstructions.setColorForAllLetters(Color.black);
		m_tapInstructions.parentUIObject = m_missionsFrame;
		m_tapInstructions.positionFromTop(1f);
		m_tapInstructions.text = LocalTextManager.GetUIText("_MISSIONS_SKIP_INSTRUCTIONS_");
		if (GameManager.The.aspectRatio.x == 3f && GameManager.The.aspectRatio.y == 4f)
		{
			m_tapInstructions.textScale = 0.55f * UIHelper.CalcFontScale();
		}
		else
		{
			m_tapInstructions.textScale = 0.35f * UIHelper.CalcFontScale();
		}
	}

	private void CreateMissionElements()
	{
		int missionCountPerGroup = PlayerData.GetMissionCountPerGroup();
		m_missionElements = new MissionElement[missionCountPerGroup];
		ArrayList groupofMissionsForLevel = AllMissionData.GetGroupofMissionsForLevel(PlayerData.MaxMissionLevelSeenByUserIndex);
		for (int i = 0; i < m_missionElements.Length; i++)
		{
			Mission mission = (Mission)groupofMissionsForLevel[i];
			if (mission == null)
			{
				m_missionElements[i].m_desc = null;
				m_missionElements[i].m_equalsign = null;
				continue;
			}
			m_missionElements[i].m_frameBtn = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "MissionButton.png", "MissionButtonOver.png", 0, 0, m_missionStartDepth + 3);
			m_missionElements[i].m_flashBtn = GlobalGUIManager.The.m_menuToolkit.addSprite("MissionButtonOver.png", 0, 0, m_missionStartDepth + 3);
			float num = 1f;
			m_missionElements[i].m_frameBtn.scale = new Vector3(num, num, 1f);
			m_missionElements[i].m_frameBtn.userData = i;
			m_missionElements[i].m_frameBtn.onTouchUpInside += onTouchUpInsideBuyMissionsButton;
			m_missionElements[i].m_flashBtn.hidden = true;
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_missionElements[i].m_frameBtn);
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_missionElements[i].m_flashBtn);
			string desc = mission.m_desc;
			float num2 = 1f;
			m_missionElements[i].m_desc = GlobalGUIManager.The.defaultTextAlt.addTextInstance(desc, 0f, 0f, 1f, -7);
			m_missionElements[i].m_desc.alignMode = UITextAlignMode.Left;
			m_missionElements[i].m_desc.verticalAlignMode = UITextVerticalAlignMode.Middle;
			m_missionElements[i].m_desc.setColorForAllLetters(Color.black);
			m_missionElements[i].m_desc.parentUIObject = m_missionElements[i].m_frameBtn;
			m_missionElements[i].m_desc.positionFromLeft(0.015f);
			m_missionElements[i].m_desc.text = desc;
			if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
			{
				num2 = 0.27f;
			}
			if (GameManager.The.aspectRatio.x == 3f && GameManager.The.aspectRatio.y == 4f && LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.English)
			{
				num2 = 0.35f;
			}
			if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f && LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
			{
				num2 = 0.21f;
			}
			if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f && LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.English)
			{
				num2 = 0.24f;
			}
			if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f && LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Italian)
			{
				num2 = 0.16f;
			}
			if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f && LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Spanish)
			{
				num2 = 0.19f;
			}
			if (UI.scaleFactor == 1)
			{
				num2 /= 1.5f;
			}
			m_missionElements[i].m_desc.textScale = num2 * UIHelper.CalcFontScale();
			m_missionElements[i].m_equalsign = GlobalGUIManager.The.defaultTextAlt.addTextInstance("=", 0f, 0f, 1f, m_missionStartDepth + 2);
			m_missionElements[i].m_equalsign.alignMode = UITextAlignMode.Left;
			m_missionElements[i].m_equalsign.verticalAlignMode = UITextVerticalAlignMode.Middle;
			m_missionElements[i].m_equalsign.textScale = num2 * UIHelper.CalcFontScale();
			m_missionElements[i].m_equalsign.setColorForAllLetters(Color.black);
			m_missionElements[i].m_equalsign.parentUIObject = m_missionElements[i].m_frameBtn;
			m_missionElements[i].m_equalsign.positionFromRight(15f);
			m_missionElements[i].m_doneCheckMark = GlobalGUIManager.The.m_menuToolkit.addSprite("MissionCheck.png", 0, 0, m_missionStartDepth + 1);
			m_missionElements[i].m_doneCheckMark.scale *= num;
			m_missionElements[i].m_doneCheckMark.parentUIObject = null;
			m_missionElements[i].m_badge = GlobalGUIManager.The.m_menuToolkit.addSprite("Badge.png", 0, 0, m_missionStartDepth + 1);
			m_missionElements[i].m_badge.parentUIObject = null;
			if (GameManager.The.aspectRatio.x == 3f && GameManager.The.aspectRatio.y == 4f)
			{
				m_missionElements[i].m_badge.scale *= 0.75f;
			}
			else
			{
				m_missionElements[i].m_badge.scale *= ((!(Screen.dpi >= 250f) || Screen.height <= 1280) ? 0.55f : 0.95f);
			}
			if (UI.scaleFactor == 1)
			{
				m_missionElements[i].m_badge.scale *= 1.5f;
			}
			m_missionLayout.addChild(m_missionElements[i].m_frameBtn);
		}
	}

	public void CreateMissionLevelGUI()
	{
		m_levelFrame = GlobalGUIManager.The.m_menuToolkit.addSprite("LevelPlate.png", 0, 0, m_missionStartDepth + 4);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_levelFrame);
		Level currentLevel = PlayerData.CurrentLevel;
		if (currentLevel == null)
		{
			m_levelTitle = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_NO_MORE_LEVELS_DEFAULT_"), 0f, 0f, 1f, m_missionStartDepth + 3);
			m_levelTitle.alignMode = UITextAlignMode.Center;
			m_levelTitle.verticalAlignMode = UITextVerticalAlignMode.Top;
			m_levelTitle.textScale = 0.5f * UIHelper.CalcFontScale();
			m_levelTitle.setColorForAllLetters(Color.black);
			m_levelTitle.parentUIObject = m_levelFrame;
			m_levelTitle.text = LocalTextManager.GetUIText("_NO_MORE_LEVELS_DEFAULT_");
			m_badges = new UISprite[0];
			m_emptyBadges = new UISprite[0];
			m_multiplierIcon = GlobalGUIManager.The.m_menuToolkit.addSprite("+1Bonus.png", 0, 0, m_missionStartDepth + 2);
			m_multiplierIcon.parentUIObject = m_levelFrame;
			m_multiplierIcon.positionFromLeft(0f);
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_multiplierIcon);
			m_equalScoreMultText = GlobalGUIManager.The.defaultTextAlt.addTextInstance("1x", 0f, 0f, 1f, m_missionStartDepth + 3);
			m_equalScoreMultText.alignMode = UITextAlignMode.Left;
			m_equalScoreMultText.verticalAlignMode = UITextVerticalAlignMode.Bottom;
			m_equalScoreMultText.textScale = 0.45f * UIHelper.CalcFontScale();
			m_equalScoreMultText.setColorForAllLetters(Color.white);
			m_equalScoreMultText.parentUIObject = m_multiplierIcon;
			m_equalScoreMultText.positionFromCenter(0f, 0f);
			m_equalScoreMultText.text = "1x";
			return;
		}
		m_levelTitle = GlobalGUIManager.The.defaultTextAlt.addTextInstance(currentLevel.m_title, 0f, 0f, 1f, m_missionStartDepth + 3);
		m_levelTitle.alignMode = UITextAlignMode.Center;
		m_levelTitle.verticalAlignMode = UITextVerticalAlignMode.Top;
		m_levelTitle.textScale = 0.6f * UIHelper.CalcFontScale();
		m_levelTitle.setColorForAllLetters(Color.black);
		m_levelTitle.parentUIObject = m_levelFrame;
		m_levelTitle.positionFromTop(0.02f);
		if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f)
		{
			if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
			{
				m_levelTitle.textScale = 0.3f * UIHelper.CalcFontScale();
				if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Russian)
				{
					m_levelTitle.textScale = 0.2f * UIHelper.CalcFontScale();
				}
			}
			else
			{
				m_levelTitle.textScale = 0.4f * UIHelper.CalcFontScale();
			}
		}
		else
		{
			m_levelTitle.textScale = 0.6f * UIHelper.CalcFontScale();
		}
		if (GameManager.The.aspectRatio.x == 3f && GameManager.The.aspectRatio.y == 4f && LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_levelTitle.textScale = 0.45f * UIHelper.CalcFontScale();
		}
		if (GameManager.The.aspectRatio.x == 3f && GameManager.The.aspectRatio.y == 4f && LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Russian)
		{
			m_levelTitle.textScale = 0.35f * UIHelper.CalcFontScale();
		}
		m_levelBadgeLayout = new UIHorizontalLayout(-7);
		m_levelBadgeLayout.alignMode = UIAbstractContainer.UIContainerAlignMode.Center;
		m_levelBadgeLayout.parentUIObject = m_levelFrame;
		m_levelBadgeLayout.spacing = 10;
		m_badges = new UISprite[MAX_LEVEL_BADGES];
		m_emptyBadges = new UISprite[MAX_LEVEL_BADGES];
		for (int i = 0; i < m_badges.Length; i++)
		{
			m_badges[i] = GlobalGUIManager.The.m_menuToolkit.addSprite("Badge.png", 0, 0, m_missionStartDepth + 2);
			m_levelBadgeLayout.addChild(m_badges[i]);
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_badges[i]);
			m_emptyBadges[i] = GlobalGUIManager.The.m_menuToolkit.addSprite("BadgeEmpty.png", 0, 0, m_missionStartDepth + 3);
			m_emptyBadges[i].parentUIObject = m_badges[i];
			m_emptyBadges[i].pixelsFromCenter(0, 0);
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_emptyBadges[i]);
		}
		ShowTheRightBadgesForLevel(ref m_badges, ref m_emptyBadges);
		m_levelBadgeLayout.matchSizeToContentSize();
		m_levelBadgeLayout.parentUIObject = m_levelFrame;
		m_levelBadgeLayout.positionFromTop(0.4f, -0.1f);
		m_multiplierIcon = GlobalGUIManager.The.m_menuToolkit.addSprite("+1Bonus.png", 0, 0, m_missionStartDepth + 3);
		m_multiplierIcon.parentUIObject = m_levelFrame;
		m_multiplierIcon.positionFromLeft(0f);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_multiplierIcon);
		m_equalScoreMultText = GlobalGUIManager.The.defaultTextAlt.addTextInstance("x1", 0f, 0f, 1f, m_missionStartDepth + 2);
		m_equalScoreMultText.alignMode = UITextAlignMode.Left;
		m_equalScoreMultText.verticalAlignMode = UITextVerticalAlignMode.Bottom;
		m_equalScoreMultText.setColorForAllLetters(Color.black);
		m_equalScoreMultText.parentUIObject = m_levelFrame;
		m_equalScoreMultText.positionFromCenter(0.11f, 0.13f);
		m_equalScoreMultText.text = "=";
		if (GameManager.The.aspectRatio.x == 3f && GameManager.The.aspectRatio.y == 4f)
		{
			m_equalScoreMultText.textScale = 0.95f * UIHelper.CalcFontScale();
		}
		else
		{
			m_equalScoreMultText.textScale = 0.7f * UIHelper.CalcFontScale();
		}
	}

	public void HideAll()
	{
		if (m_IsShowingAll)
		{
			HideMissions();
			HideLevel();
			m_IsShowingAll = false;
		}
	}

	private void HideMissions()
	{
		m_missionLayout.hidden = true;
		m_missionsFrame.hidden = true;
		m_missionsLabel.hidden = true;
		m_noMissionText.hidden = true;
		if (m_missionElements != null)
		{
			MissionElement[] missionElements = m_missionElements;
			foreach (MissionElement m in missionElements)
			{
				HideMissionElement(m);
			}
		}
	}

	private void HideMissionElement(MissionElement m)
	{
		m.m_frameBtn.hidden = true;
		m.m_flashBtn.hidden = true;
		m.m_desc.hidden = true;
		m.m_equalsign.hidden = true;
		m.m_doneCheckMark.hidden = true;
		m.m_badge.hidden = true;
	}

	private void HideMissionsElementsText()
	{
		if (m_missionElements != null)
		{
			for (int i = 0; i < m_missionElements.Length; i++)
			{
				m_missionElements[i].m_frameBtn.hidden = true;
				m_missionElements[i].m_flashBtn.hidden = true;
				m_missionElements[i].m_doneCheckMark.hidden = true;
				m_missionElements[i].m_desc.hidden = true;
				m_missionElements[i].m_equalsign.hidden = true;
			}
		}
	}

	public void HideMultiplier()
	{
		m_multiplierIcon.hidden = true;
		m_equalScoreMultText.hidden = true;
	}

	private void HideLevel()
	{
		m_levelFrame.hidden = true;
		m_levelTitle.hidden = true;
		m_multiplierIcon.hidden = true;
		for (int i = 0; i < m_badges.Length; i++)
		{
			m_emptyBadges[i].hidden = true;
			m_badges[i].hidden = true;
		}
		m_equalScoreMultText.hidden = true;
		m_tapInstructions.hidden = true;
	}

	public void ShowMainMenuMissions()
	{
		UpdateMissionPositionsForMainMenu();
		UpdateMissions();
		UpdateLevelPositionsForMainMenu();
		UpdateLevel();
		m_IsShowingAll = true;
	}

	public void ShowPauseMenuMissions(float xExtent)
	{
		UpdateMissionPositionsForPauseMenu();
		UpdateMissions();
		UpdateLevelPositionsForPauseMenu();
		UpdateLevel();
		m_IsShowingAll = true;
	}

	public void ShowInGameMenu()
	{
		UpdateMissionPositionsForInGameMenu();
		UpdateMissions();
		UpdateLevelPositionsForInGameMenu();
		UpdateLevel();
		m_IsShowingAll = true;
	}

	private void UpdateMissions()
	{
		m_missionLayout.hidden = false;
		m_missionsFrame.hidden = false;
		m_missionsLabel.hidden = false;
		m_tapInstructions.hidden = false;
		if (AllMissionData.AreAllMissionsCompleted())
		{
			HideMissionsElementsText();
			m_tapInstructions.hidden = true;
			m_noMissionText.hidden = false;
			return;
		}
		UpdateMissionElements();
		if (!DoesUserHaveEnoughTokensToBuyOneMission() || GlobalGUIManager.The.IsInInGameMenu())
		{
			DisableMissionButtons();
			m_tapInstructions.hidden = true;
		}
	}

	private bool DoesUserHaveEnoughTokensToBuyOneMission()
	{
		ArrayList groupofMissionsForLevel = AllMissionData.GetGroupofMissionsForLevel(PlayerData.MaxMissionLevelSeenByUserIndex);
		if (groupofMissionsForLevel == null)
		{
			return false;
		}
		for (int i = 0; i < groupofMissionsForLevel.Count; i++)
		{
			Mission mission = (Mission)groupofMissionsForLevel[i];
			if (PlayerData.playerTokens >= mission.m_skipTokenCost)
			{
				return true;
			}
		}
		return false;
	}

	private void UpdateLevel()
	{
		Level currentLevel = PlayerData.CurrentLevel;
		m_levelFrame.hidden = false;
		if (currentLevel == null)
		{
			m_levelTitle.text = LocalTextManager.GetUIText("_NO_MORE_LEVELS_DEFAULT_");
		}
		else
		{
			m_levelTitle.text = LocalTextManager.GetUIText("_LEVEL_") + " " + (currentLevel.m_numericLevel + 1) + ": " + currentLevel.m_title;
		}
		m_levelTitle.hidden = false;
		ShowTheRightBadgesForLevel(ref m_badges, ref m_emptyBadges);
		if (currentLevel == null || currentLevel.m_multiplier < 1)
		{
			m_equalScoreMultText.text = "null";
		}
		m_equalScoreMultText.hidden = false;
		m_multiplierIcon.hidden = false;
	}

	public void UpdateMissionElements()
	{
		ArrayList groupofMissionsForLevel = AllMissionData.GetGroupofMissionsForLevel(PlayerData.MaxMissionLevelSeenByUserIndex);
		if (groupofMissionsForLevel == null)
		{
			HideMissions();
		}
		else
		{
			if (m_missionElements == null)
			{
				return;
			}
			int missionCountPerGroup = PlayerData.GetMissionCountPerGroup();
			for (int i = 0; i < missionCountPerGroup; i++)
			{
				Mission mission = (Mission)groupofMissionsForLevel[i];
				if (mission == null)
				{
					HideMissionElement(m_missionElements[i]);
					break;
				}
				m_missionElements[i].m_frameBtn.hidden = false;
				m_missionElements[i].m_frameBtn.userData = i;
				float num = 1f;
				if (UI.scaleFactor == 1)
				{
					num = 0.32f;
				}
				m_missionElements[i].m_desc.text = mission.m_desc;
				m_missionElements[i].m_desc.hidden = false;
				m_missionElements[i].m_equalsign.hidden = false;
				if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
				{
					num = 0.27f;
				}
				if (GameManager.The.aspectRatio.x == 3f && GameManager.The.aspectRatio.y == 4f && LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.English)
				{
					num = 0.35f;
				}
				if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f && LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
				{
					num = 0.21f;
				}
				if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f && LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.English)
				{
					num = 0.24f;
				}
				if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f && LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Italian)
				{
					num = 0.16f;
				}
				if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f && LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Spanish)
				{
					num = 0.19f;
				}
				if (UI.scaleFactor == 1)
				{
					num /= 1.5f;
				}
				m_missionElements[i].m_desc.textScale = num * UIHelper.CalcFontScale();
				m_missionElements[i].m_equalsign.textScale = num * UIHelper.CalcFontScale();
				if (mission.Completed)
				{
					m_missionElements[i].m_frameBtn.alphaFromTo(0.01f, 1f, 0.45f, Easing.Linear.easeInOut);
					m_missionElements[i].m_desc.alphaFromTo(0.01f, 1f, 0.25f, Easing.Linear.easeInOut);
					m_missionElements[i].m_doneCheckMark.parentUIObject = m_missionElements[i].m_frameBtn;
					m_missionElements[i].m_doneCheckMark.positionFromBottomRight(0.25f, 0.02f);
					m_missionElements[i].m_doneCheckMark.hidden = false;
					m_missionElements[i].m_frameBtn.disabled = true;
				}
				else
				{
					m_missionElements[i].m_frameBtn.alphaTo(0.01f, 1f, Easing.Linear.easeInOut);
					m_missionElements[i].m_desc.alphaTo(0.01f, 1f, Easing.Linear.easeInOut);
					m_missionElements[i].m_frameBtn.disabled = false;
					m_missionElements[i].m_doneCheckMark.hidden = true;
					m_missionElements[i].m_badge.parentUIObject = m_missionElements[i].m_frameBtn;
					m_missionElements[i].m_badge.positionFromBottomRight(0.26f, 0.04f);
					m_missionElements[i].m_badge.hidden = false;
				}
			}
		}
	}

	public void UpdateMissionPositionsForInGameMenu()
	{
		UpdateMissionPositionsForMainMenu();
	}

	public void UpdateMissionPositionsForPauseMenu()
	{
		UpdateMissionPositionsForMainMenu();
		m_missionsFrame.positionFromBottom(0.25f);
	}

	public void UpdateMissionPositionsForMainMenu()
	{
		m_missionsFrame.positionFromBottom(0.05f);
		m_missionsLabel.parentUIObject = m_missionsFrame;
		m_missionsLabel.alignMode = UITextAlignMode.Left;
		m_missionsLabel.positionFromTopLeft(0.025f, 0.07f);
		m_noMissionText.alignMode = UITextAlignMode.Center;
		m_noMissionText.parentUIObject = m_missionsFrame;
		m_noMissionText.positionFromCenter(0f, 0f);
		m_missionLayout.parentUIObject = m_missionsFrame;
		m_missionLayout.positionFromTopLeft(0.17f, 0.04f);
		if (m_missionElements != null)
		{
			for (int i = 0; i < m_missionElements.Length; i++)
			{
				m_missionElements[i].m_flashBtn.parentUIObject = m_missionElements[i].m_frameBtn;
				m_missionElements[i].m_desc.alignMode = UITextAlignMode.Left;
				m_missionElements[i].m_desc.verticalAlignMode = UITextVerticalAlignMode.Middle;
				m_missionElements[i].m_desc.parentUIObject = m_missionElements[i].m_frameBtn;
				m_missionElements[i].m_desc.positionFromLeft(0.05f);
				m_missionElements[i].m_equalsign.alignMode = UITextAlignMode.Left;
				m_missionElements[i].m_equalsign.verticalAlignMode = UITextVerticalAlignMode.Middle;
				m_missionElements[i].m_equalsign.parentUIObject = m_missionElements[i].m_frameBtn;
				m_missionElements[i].m_equalsign.positionFromRight(0.15f);
			}
		}
		m_tapInstructions.alignMode = UITextAlignMode.Center;
		m_tapInstructions.verticalAlignMode = UITextVerticalAlignMode.Top;
		m_tapInstructions.parentUIObject = m_missionsFrame;
		m_tapInstructions.positionFromTop(1f);
	}

	public void UpdateLevelPositionsForInGameMenu()
	{
		UpdateLevelPositionsForMainMenu();
	}

	public void UpdateLevelPositionsForPauseMenu()
	{
		UpdateLevelPositionsForMainMenu();
	}

	public void UpdateLevelPositionsForMainMenu()
	{
		Debug.Log("UpdateLevelPositionsForMainMenu");
		m_levelFrame.parentUIObject = m_missionsFrame;
		m_levelFrame.positionFromTop(-0.5f);
		m_levelTitle.parentUIObject = m_levelFrame;
		m_levelTitle.positionFromTop(0.05f);
		m_multiplierIcon.positionFromCenter(0.12f, 0.25f);
		m_multiplierIcon.parentUIObject = m_levelFrame;
		m_levelBadgeLayout.alignMode = UIAbstractContainer.UIContainerAlignMode.Center;
		m_levelBadgeLayout.parentUIObject = m_levelFrame;
		m_levelBadgeLayout.matchSizeToContentSize();
		m_levelBadgeLayout.positionFromTop(0.4f, -0.1f);
	}

	public void ShowTheRightBadgesForLevel(ref UISprite[] badges, ref UISprite[] emptyBadges)
	{
		ArrayList groupofMissionsForLevel = AllMissionData.GetGroupofMissionsForLevel(PlayerData.MaxMissionLevelSeenByUserIndex);
		if (groupofMissionsForLevel == null)
		{
			return;
		}
		int num = 0;
		foreach (Mission item in groupofMissionsForLevel)
		{
			m_emptyBadges[num].hidden = false;
			if (item.Completed)
			{
				m_badges[num].hidden = false;
			}
			else
			{
				m_badges[num].hidden = true;
			}
			num++;
		}
	}

	public void EnableMissionButtons()
	{
		if (m_missionElements != null)
		{
			for (int i = 0; i < m_missionElements.Length; i++)
			{
				m_missionElements[i].m_frameBtn.disabled = false;
			}
		}
	}

	public void DisableMissionButtons(bool bDisable)
	{
		if (m_missionElements == null)
		{
			return;
		}
		for (int i = 0; i < m_missionElements.Length; i++)
		{
			if (m_missionElements[i].m_frameBtn != null)
			{
				m_missionElements[i].m_frameBtn.disabled = bDisable;
			}
		}
	}

	public void DisableMissionButtons()
	{
		if (m_missionElements != null)
		{
			for (int i = 0; i < m_missionElements.Length; i++)
			{
				m_missionElements[i].m_frameBtn.disabled = true;
			}
		}
	}

	private int FindMissionElementIndex(Mission mission)
	{
		if (m_missionElements == null)
		{
			return -1;
		}
		for (int i = 0; i < m_missionElements.Length; i++)
		{
			if (m_missionElements[i].m_desc.text == mission.m_desc)
			{
				return i;
			}
		}
		return -1;
	}

	public void ReshuffleMissions()
	{
		if (m_missionElements == null)
		{
			return;
		}
		ArrayList arrayList = new ArrayList();
		bool flag = false;
		Vector3 vector = Vector3.zero;
		for (int i = 0; i < m_missionElements.Length; i++)
		{
			if (!m_missionElements[i].m_desc.hidden)
			{
				arrayList.Add(m_missionElements[i]);
				if (flag)
				{
					Vector3 localPosition = m_missionElements[i].m_frameBtn.localPosition;
					Vector3 target = new Vector3(m_missionElements[i].m_frameBtn.localPosition.x, vector.y, m_missionElements[i].m_frameBtn.localPosition.z);
					m_missionElements[i].m_frameBtn.positionTo(0.5f, target, Easing.Sinusoidal.easeInOut);
					if (i + 1 < m_missionElements.Length)
					{
						m_missionElements[i + 1].m_frameBtn.positionTo(0.5f, localPosition, Easing.Sinusoidal.easeInOut);
					}
					flag = false;
				}
			}
			else if (!flag)
			{
				flag = true;
				vector = m_missionElements[i].m_frameBtn.localPosition;
			}
		}
		for (int j = 0; j < m_missionElements.Length; j++)
		{
			if (m_missionElements[j].m_desc.hidden)
			{
				HideMissionElement(m_missionElements[j]);
				m_missionLayout.removeChild(m_missionElements[j].m_frameBtn, false);
			}
		}
		m_missionElements = (MissionElement[])arrayList.ToArray(typeof(MissionElement));
	}

	public void MoveMissions(float offset, bool bAnimate = false, Action<bool> onDone = null)
	{
		Vector3 vector = new Vector3(offset, 0f, 0f);
		Vector3 position = m_missionsFrame.position;
		Vector3 target = m_missionsFrame.position + vector;
		Vector3 vector2 = m_levelFrame.position + vector;
		if (bAnimate)
		{
			UIAnimation uIAnimation = m_missionsFrame.positionFromTo(0.5f, position, target, Easing.Sinusoidal.easeInOut);
			m_levelFrame.position = new Vector3(vector2.x, vector2.y, vector2.z);
			uIAnimation.onComplete = delegate
			{
				if (onDone != null)
				{
					onDone(true);
				}
			};
		}
		else
		{
			m_missionsFrame.position = new Vector3(target.x, target.y, target.z);
			m_levelFrame.position = new Vector3(vector2.x, vector2.y, vector2.z);
		}
	}

	public void CompleteMissionAnim(Mission mission)
	{
		int i = FindMissionElementIndex(mission);
		StartCoroutine(FlashMissionButton(i, delegate
		{
			MainMenuEventManager.TriggerCompleteMissionPopUp(mission, true);
		}));
	}

	private IEnumerator FlashMissionButton(int i, Action<bool> onDone)
	{
		if (m_missionElements != null)
		{
			for (int j = 0; j < 6; j++)
			{
				m_missionElements[i].m_flashBtn.position = m_missionElements[i].m_frameBtn.position;
				m_missionElements[i].m_flashBtn.hidden = !m_missionElements[i].m_flashBtn.hidden;
				m_missionElements[i].m_frameBtn.hidden = !m_missionElements[i].m_frameBtn.hidden;
				yield return new WaitForSeconds(0.1f);
			}
		}
		onDone(true);
	}

	public void onTouchUpInsideBuyMissionsButton(UIButton sender)
	{
		MainMenuEventManager.TriggerStartAnimation();
		GameManager.The.PlayClip(AudioClipFiles.UIPURCHASE);
		Debug.Log("BuyMissionsButton");
		int index = (int)sender.userData;
		Mission missionForLevel = AllMissionData.GetMissionForLevel(PlayerData.MaxMissionLevelSeenByUserIndex, index);
		if (missionForLevel.m_skipTokenCost <= PlayerData.playerTokens)
		{
			Debug.Log("Buy Mission " + missionForLevel);
			MainMenuEventManager.TriggerBuyMissionPopUp(missionForLevel);
		}
		else
		{
			MainMenuEventManager.TriggerEndAnimation();
		}
	}

	public void BuyMissionDone(Mission mission, bool success)
	{
		if (success)
		{
			MainMenuEventManager.TriggerCompleteMissionPopUp(mission, false);
		}
		else
		{
			MainMenuEventManager.TriggerEndAnimation();
		}
		PlayerData.SavePersistentData();
	}

	public void CompleteMission(Mission mission, Action<bool> onDone)
	{
		int elementIndex = FindMissionElementIndex(mission);
		CompleteMission(elementIndex, onDone);
	}

	public void CompleteMission(int elementIndex, Action<bool> onDone)
	{
		if (GlobalGUIManager.The.IsInInGameMenu() && elementIndex != 2)
		{
			onDone(true);
		}
		else if (AllMissionData.IsMissionGroupCompleted(PlayerData.MaxMissionLevelSeenByUserIndex))
		{
			GlobalGUIManager.The.m_IsAnimatingInGameCompletedMissionGroup = true;
			if (AllMissionData.AreAllMissionsCompleted())
			{
				onDone(true);
				return;
			}
			CompleteLevel(0, onDone);
			PlayerData.MaxMissionLevelSeenByUserIndex++;
			PlayerData.SetNewMissions();
			CompleteAllMissions(0, onDone);
			CompleteAllMissions(1, onDone);
			CompleteAllMissions(2, onDone);
		}
		else
		{
			onDone(true);
		}
	}

	private void CompleteAllMissions(int elementIndex, Action<bool> onDone)
	{
		if (m_missionElements == null)
		{
			if (onDone != null)
			{
				onDone(true);
			}
			return;
		}
		Vector3 localPosition = m_missionElements[elementIndex].m_frameBtn.localPosition;
		UIAnimation uIAnimation = UIObjectAnimationExtensions.positionTo(target: new Vector3(localPosition.x + (float)Screen.width * 1.5f, localPosition.y, 0f), sprite: m_missionElements[elementIndex].m_frameBtn, duration: 0.5f, ease: Easing.Sinusoidal.easeInOut);
		bool flag = false;
		UpdateMissions();
		if (!flag && m_missionElements.Length > 0)
		{
			uIAnimation = UIObjectAnimationExtensions.positionFromTo(start: new Vector3(localPosition.x - (float)Screen.width, localPosition.y, 0f), sprite: m_missionElements[elementIndex].m_frameBtn, duration: 0.5f, target: localPosition, ease: Easing.Sinusoidal.easeInOut);
			uIAnimation.onComplete = delegate
			{
				onDone(true);
			};
		}
		else
		{
			onDone(true);
		}
	}

	private void CompleteLevel(int badgesLeftToReward, Action<bool> onDone)
	{
		if (AllMissionData.IsMissionGroupCompleted(PlayerData.MaxMissionLevelSeenByUserIndex))
		{
			Vector3 originalPos = m_levelFrame.localPosition;
			Vector3 target = new Vector3(m_levelFrame.localPosition.x + (float)Screen.width, m_levelFrame.localPosition.y, m_levelFrame.localPosition.z);
			UIAnimation ani = m_levelFrame.positionTo(0.5f, target, Easing.Sinusoidal.easeInOut);
			ani.onComplete = delegate
			{
				UpdateLevel();
				ani = UIObjectAnimationExtensions.positionFromTo(start: new Vector3(originalPos.x - (float)Screen.width, originalPos.y, originalPos.z), sprite: m_levelFrame, duration: 0.5f, target: originalPos, ease: Easing.Sinusoidal.easeInOut);
				ani.onComplete = delegate
				{
				};
			};
		}
		else
		{
			onDone(true);
		}
	}

	public void ReloadStaticText()
	{
		m_missionsLabel.text = LocalTextManager.GetUIText("_MISSIONS_CONTAINER_TITLE_");
		m_noMissionText.text = LocalTextManager.GetUIText("_MISSIONS_NO_NEW_DEFAULT_");
		m_tapInstructions.text = LocalTextManager.GetUIText("_MISSIONS_SKIP_INSTRUCTIONS_");
		if (GameManager.The.aspectRatio.x == 3f && GameManager.The.aspectRatio.y == 4f)
		{
			m_tapInstructions.textScale = 0.55f * UIHelper.CalcFontScale();
			m_missionsLabel.textScale = 0.64f * UIHelper.CalcFontScale();
		}
		else
		{
			m_tapInstructions.textScale = 0.35f * UIHelper.CalcFontScale();
			m_missionsLabel.textScale = 0.45f * UIHelper.CalcFontScale();
		}
		m_missionsLabel.textScale = 0.35f * UIHelper.CalcFontScale();
		float num = 1f;
		if (UI.scaleFactor == 1)
		{
			num /= 1.5f;
		}
		num = ((GameManager.The.aspectRatio.x == 3f || GameManager.The.aspectRatio.y == 4f) ? 0.6f : ((LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.English) ? 0.4f : 0.3f));
		if (GameManager.The.aspectRatio.x == 3f && GameManager.The.aspectRatio.y == 4f && LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			num = 0.45f;
		}
		if (GameManager.The.aspectRatio.x == 3f && GameManager.The.aspectRatio.y == 4f && LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Russian)
		{
			num = 0.35f;
		}
		if (UI.scaleFactor == 1)
		{
			num /= 1.5f;
		}
		m_levelTitle.textScale = num * UIHelper.CalcFontScale();
	}
}

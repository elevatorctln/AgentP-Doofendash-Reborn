using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDGUIManager : MonoBehaviour
{
	private UIButton m_pauseButton;

	private UIButton m_pauseButtonTexture;

	private UIButton m_gadgetButton;

	private UISprite m_gadgetFillIcon1;

	private UISprite m_gadgetFillIcon2;

	private UISprite m_gadgetFillIcon3;

	private UISprite m_tokenIcon;

	private UISprite m_scoreFrame;

	private UISprite m_bossHealthFrame;

	private UISprite m_bossHealthFaceDoof;

	private UISprite m_bossHealthFaceBalloony;

	private UISprite m_bossHealtShowForBossTutorialhFace;

	private UIProgressBar m_bossHealthBar;

	private UITextInstance m_scoreLabel;

	private UITextInstance m_scoreText;

	private UITextInstance m_multiplierLabel;

	private UITextInstance m_tokenText;

	private UIButton m_cameraButton;

	private UISprite m_highScoreCrown;

	private FaceTexture m_FBFaceTexture;

	private UISprite m_FBPhotoFrame;

	private UIButton m_FBFaceTextureHolder;

	private UISprite m_FBScoreFrame;

	private UITextInstance m_FBScoreText;

	private UITextInstance m_FBScoreName;

	private PerryHighScore m_NextOpponentHighScore;

	private bool m_hasOpponentScoreReady;

	private UIAnimation m_notificationAnim;

	private UISprite m_notificationFrame;

	private UITextInstance m_notificationText;

	private UISprite m_fireWeaponIcon;

	private UISprite m_electricWeaponIcon;

	private UISprite m_waterWeaponIcon;

	private UISprite m_pinWeaponIcon;

	private UISprite m_lockIcon;

	private Vector3 m_WeaponHidingPlace;

	private Vector3 m_WeaponShowingPlace;

	private UIButton m_jumpStartButton;

	private Queue<string> m_queuedNotifications;

	public float m_CopterBoostDuration = 7f;

	private Timer m_CopterBoostTimer;

	private bool m_IsInJumpStartState;

	private bool m_IsShowingBossTutorial;

	private int m_HudDepth = 4;

	private static HUDGUIManager m_the;

	private bool m_isJumpStartFlashOff;

	public float m_jumpStartFlashDuration = 2f;

	private bool b_playerIstheHighestScore;

	public static HUDGUIManager The
	{
		get
		{
			if (m_the == null)
			{
				GameObject gameObject = new GameObject("HUDGUIManager");
				((HUDGUIManager)gameObject.AddComponent<HUDGUIManager>()).Init();
			}
			return m_the;
		}
	}

	public bool PlayerIsTheHighestScore
	{
		get
		{
			return b_playerIstheHighestScore;
		}
		set
		{
			PlayerIsTheHighestScore = value;
		}
	}

	public bool IsInJumpStartState()
	{
		return m_IsInJumpStartState;
	}

	public void Init()
	{
		if (!(m_the != null))
		{
			m_the = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			InitHUD();
			InitWeaponHUD();
			InitJumpStart();
			InitOpponentHighScore();
			HideAll();
			m_queuedNotifications = new Queue<string>();
		}
	}

	private void Start()
	{
		m_CopterBoostTimer = TimerManager.The().SpawnTimer();
	}

	private void OnEnable()
	{
		GameEventManager.GameIntro += GameIntroListener;
	}

	private void OnDisable()
	{
		GameEventManager.GameIntro -= GameIntroListener;
	}

	private void GameIntroListener()
	{
		b_playerIstheHighestScore = false;
		UpdateNextOpponentHighScore();
	}

	private void InitHUD()
	{
		// Calculate safe area offsets for notched devices
		float topSafeOffset = GetSafeAreaTopOffset();
		float rightSafeOffset = GetSafeAreaRightOffset();
		
		// Adaptive positioning based on device type
		float pauseTopOffset = 0.01f + topSafeOffset;
		float pauseLeftOffset = 0.017f;
		
		m_pauseButtonTexture = UIButton.create(GlobalGUIManager.The.m_hudToolkit, "PauseButton.png", "PauseButtonOver.png", 0, 0, m_HudDepth + 2);
		m_pauseButtonTexture.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_pauseButtonTexture.onTouchUpInside += onTouchUpInsidePauseButton;
		m_pauseButtonTexture.positionFromTopLeft(pauseTopOffset, pauseLeftOffset);
		m_pauseButton = UIButton.create(GlobalGUIManager.The.m_hudToolkit, "PauseButton.png", "PauseButtonOver.png", 0, 0, m_HudDepth + 2);
		m_pauseButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_pauseButton.positionFromTopLeft(pauseTopOffset, pauseLeftOffset);
		m_pauseButton.onTouchUpInside += onTouchUpInsidePauseButton;
		m_pauseButton.color = Color.clear;
		
		// Scale pause button based on screen density
		float buttonScale = GetAdaptiveButtonScale();
		m_pauseButton.scale = Vector3.one * buttonScale;
		
		// Score frame positioning with safe area consideration
		float scoreTopOffset = topSafeOffset;
		float scoreRightOffset = 0.01f + rightSafeOffset;
		
		m_scoreFrame = GlobalGUIManager.The.m_hudToolkit.addSprite("ScorePlate.png", 0, 0, m_HudDepth + 2);
		m_scoreFrame.positionFromTopRight(scoreTopOffset, scoreRightOffset);
		m_scoreText = GlobalGUIManager.The.defaultText.addTextInstance(PlayerData.RoundScore.ToString(), 0f, 0f, 1f, m_HudDepth + 1);
		m_scoreText.textScale = 0.4f * GetAdaptiveFontScale();
		
		// Adaptive score text positioning
		float scoreTextTopOffset = GetAdaptiveScoreTextPosition();
		m_scoreText.positionFromTopRight(scoreTextTopOffset + topSafeOffset, 0.045f + rightSafeOffset);
		
		m_multiplierLabel = GlobalGUIManager.The.defaultTextAlt.addTextInstance(string.Empty, 0f, 0f, 1f, m_HudDepth + 1);
		m_multiplierLabel.parentUIObject = m_scoreFrame;
		m_multiplierLabel.textScale = 0.45f * GetAdaptiveFontScale();
		
		// Token icon and text with adaptive positioning
		float tokenTopOffset = 0.075f + topSafeOffset;
		m_tokenIcon = GlobalGUIManager.The.m_hudToolkit.addSprite("TokenPlate.png", 0, 0, m_HudDepth + 2);
		m_tokenIcon.positionFromTopRight(tokenTopOffset, scoreRightOffset);
		m_tokenText = GlobalGUIManager.The.defaultText.addTextInstance(PlayerData.RoundTokens.ToString(), 0f, 0f, 1f, m_HudDepth + 1);
		m_tokenText.textScale = 0.4f * GetAdaptiveFontScale();
		
		// Adaptive token text position based on device type
		float tokenTextTop = GetAdaptiveTokenTextPosition();
		float tokenTextRight = UIHelper.IsTablet() ? 0.09f : 0.105f;
		m_tokenText.positionFromTopRight(tokenTextTop + topSafeOffset, tokenTextRight + rightSafeOffset);
		
		// Boss health frame with adaptive positioning
		m_bossHealthFrame = GlobalGUIManager.The.m_hudToolkit.addSprite("DoofMeterEmpty.png", 0, 0, m_HudDepth + 3);
		float bossHealthLeft = UIHelper.IsTablet() ? 0.1f : 0.12f;
		m_bossHealthFrame.positionFromTopLeft(0.12f + topSafeOffset, bossHealthLeft);
		m_bossHealthBar = UIProgressBar.create(GlobalGUIManager.The.m_hudToolkit, "DoofMeterFull.png", 0, 0, false, m_HudDepth + 2);
		m_bossHealthBar.parentUIObject = m_bossHealthFrame;
		m_bossHealthBar.positionFromCenter(0f, -0.47f);
		m_bossHealthBar.resizeTextureOnChange = true;
		m_bossHealthBar.value = 1f;
		m_bossHealthFaceDoof = GlobalGUIManager.The.m_hudToolkit.addSprite("DoofMeterHead.png", 0, 0, m_HudDepth + 1);
		m_bossHealthFaceDoof.parentUIObject = m_bossHealthFrame;
		m_bossHealthFaceDoof.positionFromLeft(0f, -0.33f);
		m_bossHealthFaceBalloony = GlobalGUIManager.The.m_hudToolkit.addSprite("BalloonyMeterHead.png", 0, 0, m_HudDepth + 1);
		m_bossHealthFaceBalloony.parentUIObject = m_bossHealthFrame;
		m_bossHealthFaceBalloony.positionFromLeft(0f, -0.33f);
		m_highScoreCrown = GlobalGUIManager.The.m_hudToolkit.addSprite("Crown.png", 0, 0, m_HudDepth + 2);
		m_highScoreCrown.parentUIObject = m_scoreFrame;
		m_highScoreCrown.positionFromTopLeft(0.35f, -0.19f);
		m_notificationFrame = GlobalGUIManager.The.m_hudToolkit.addSprite("ScorePopUp.png", 0, 0, 1);
		m_notificationFrame.positionFromTop(0f);
		m_notificationText = GlobalGUIManager.The.defaultText.addTextInstance("999999999m", 0f, 0f, 1f, 0);
		m_notificationText.alignMode = UITextAlignMode.Center;
		m_notificationText.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_notificationText.setColorForAllLetters(Color.black);
		m_notificationText.textScale = 0.25f;
		m_notificationText.parentUIObject = m_notificationFrame;
		m_notificationText.positionFromCenter(0f, 0f);
		m_notificationFrame.hidden = true;
		m_notificationText.hidden = true;
	}

	private void InitOpponentHighScore()
	{
		string text = "Johnny";
		string text2 = "00000";
		m_FBPhotoFrame = GlobalGUIManager.The.m_hudToolkit.addSprite("FaceBookFrame.png", 0, 0, m_HudDepth + 2);
		m_FBPhotoFrame.positionFromTopLeft(0.1f, 0.01f);
		m_FBPhotoFrame.hidden = true;
		m_FBFaceTextureHolder = UIButton.create(GlobalGUIManager.The.m_hudToolkit, "FaceBookFrame.png", "FaceBookFrame.png", 0, 0, m_HudDepth + 2);
		m_FBFaceTextureHolder.parentUIObject = m_FBPhotoFrame;
		m_FBFaceTextureHolder.positionCenter();
		m_FBFaceTextureHolder.hidden = true;
		m_FBScoreFrame = GlobalGUIManager.The.m_hudToolkit.addSprite("FaceBookNamePlate.png", 0, 0, m_HudDepth + 2);
		m_FBScoreFrame.parentUIObject = m_FBPhotoFrame;
		m_FBScoreFrame.positionFromBottomRight(-0.8f, -0.5f);
		m_FBScoreText = GlobalGUIManager.The.defaultTextAlt.addTextInstance(text2, 0f, 0f, 1f, m_HudDepth + 2);
		m_FBScoreText.textScale = 0.2f;
		m_FBScoreText.alignMode = UITextAlignMode.Center;
		m_FBScoreText.parentUIObject = m_FBScoreFrame;
		m_FBScoreText.setColorForAllLetters(Color.black);
		m_FBScoreText.positionFromCenter(0.2f, 0f);
		m_FBScoreName = GlobalGUIManager.The.defaultTextAlt.addTextInstance(text, 0f, 0f, 1f, m_HudDepth + 2);
		m_FBScoreName.textScale = 0.2f;
		m_FBScoreName.alignMode = UITextAlignMode.Center;
		m_FBScoreName.parentUIObject = m_FBScoreFrame;
		m_FBScoreName.setColorForAllLetters(Color.white);
		m_FBScoreName.positionFromCenter(-0.2f, 0f);
	}

	private void InitWeaponHUD()
	{
		m_fireWeaponIcon = GlobalGUIManager.The.m_menuToolkit.addSprite("FireGunIcon.png", 0, 0, m_HudDepth + 2);
		m_fireWeaponIcon.positionFromBottomLeft(0.002f, 0f);
		m_fireWeaponIcon.hidden = true;
		m_waterWeaponIcon = GlobalGUIManager.The.m_menuToolkit.addSprite("WaterGunIcon.png", 0, 0, m_HudDepth + 2);
		m_waterWeaponIcon.positionFromBottomLeft(0.002f, 0f);
		m_waterWeaponIcon.hidden = true;
		m_electricWeaponIcon = GlobalGUIManager.The.m_menuToolkit.addSprite("ElectricGunIcon.png", 0, 0, m_HudDepth + 2);
		m_electricWeaponIcon.positionFromBottomLeft(0.002f, 0f);
		m_electricWeaponIcon.hidden = true;
		m_pinWeaponIcon = GlobalGUIManager.The.m_menuToolkit.addSprite("NeedleGunIcon.png", 0, 0, m_HudDepth + 2);
		m_pinWeaponIcon.positionFromBottomLeft(0.002f, 0f);
		m_pinWeaponIcon.hidden = true;
		m_WeaponShowingPlace = m_electricWeaponIcon.position;
		m_WeaponHidingPlace = new Vector3(m_WeaponShowingPlace.x, m_WeaponShowingPlace.y - m_electricWeaponIcon.height * 2f, m_WeaponShowingPlace.z);
		m_lockIcon = GlobalGUIManager.The.m_menuToolkit.addSprite("Lock.png", 0, 0, m_HudDepth + 1, true);
		m_lockIcon.parentUIObject = m_fireWeaponIcon;
		m_lockIcon.positionFromCenter(-0.1f, 0.1f);
		m_lockIcon.hidden = true;
	}

	private void InitJumpStart()
	{
		m_jumpStartButton = UIButton.create(GlobalGUIManager.The.m_hudToolkit, "HeadStartButton.png", "HeadStartButtonOver.png", 0, 0, m_HudDepth + 2);
		m_jumpStartButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_jumpStartButton.positionFromBottomLeft(0.01f, 0.01f);
		m_jumpStartButton.onTouchUpInside += onTouchUpInsideJumpStartButton;
	}

	public void HideAll()
	{
		HideScoreTokens();
		HidePauseButton();
		HideBossHUD();
		HideWeaponHUD();
		HideJumpStart();
		HideNotification();
		HideOpponentHighScore();
	}

	public void ShowForBossTutorial()
	{
		ShowScoreTokens();
		ShowPauseButton();
		ShowBossHUD(MiniGameManager.BossType.DoofenCruiser);
		m_IsShowingBossTutorial = false;
	}

	public void HideForBossTutorial()
	{
		HideScoreTokens();
		HidePauseButton();
		HideBossHUD();
		m_highScoreCrown.hidden = true;
		m_IsShowingBossTutorial = true;
		m_multiplierLabel.hidden = true;
	}

	public void HideScoreTokens()
	{
		m_scoreFrame.hidden = true;
		m_scoreText.hidden = true;
		m_multiplierLabel.hidden = true;
		m_tokenIcon.hidden = true;
		m_tokenText.hidden = true;
		m_highScoreCrown.hidden = true;
	}

	public void ShowPauseButton()
	{
		m_pauseButton.hidden = false;
		m_pauseButtonTexture.hidden = false;
	}

	public void HidePauseButton()
	{
		m_pauseButton.hidden = true;
		m_pauseButtonTexture.hidden = true;
	}

	public void ShowBossHUD(MiniGameManager.BossType bossType)
	{
		m_bossHealthFrame.hidden = false;
		m_bossHealthBar.hidden = false;
		switch (bossType)
		{
		case MiniGameManager.BossType.DoofenCruiser:
			m_bossHealthFaceDoof.hidden = false;
			break;
		case MiniGameManager.BossType.Balloony:
			m_bossHealthFaceBalloony.hidden = false;
			break;
		default:
			throw new ArgumentOutOfRangeException("bossType " + bossType);
		}
	}

	public void HideBossHUD()
	{
		m_bossHealthFrame.hidden = true;
		m_bossHealthBar.hidden = true;
		m_bossHealthFaceDoof.hidden = true;
		m_bossHealthFaceBalloony.hidden = true;
	}

	public void HideWeaponHUD()
	{
		m_fireWeaponIcon.hidden = true;
		m_waterWeaponIcon.hidden = true;
		m_electricWeaponIcon.hidden = true;
		m_pinWeaponIcon.hidden = true;
		m_lockIcon.hidden = true;
	}

	public void HideJumpStart()
	{
		m_jumpStartButton.hidden = true;
	}

	public void HideNotification()
	{
		if (m_notificationAnim != null)
		{
			m_notificationAnim.stop();
		}
		m_notificationFrame.hidden = true;
		m_notificationText.hidden = true;
	}

	private void HideOpponentHighScore()
	{
		if (m_FBPhotoFrame != null)
		{
			m_FBPhotoFrame.hidden = true;
		}
		if (m_FBScoreFrame != null)
		{
			m_FBScoreFrame.hidden = true;
		}
		if (m_FBScoreText != null)
		{
			m_FBScoreText.hidden = true;
		}
		if (m_FBScoreName != null)
		{
			m_FBScoreName.hidden = true;
		}
		if (m_FBFaceTextureHolder != null)
		{
			m_FBFaceTextureHolder.hidden = true;
		}
		if (m_FBFaceTexture != null)
		{
			m_FBFaceTexture.GetComponent<Renderer>().enabled = false;
		}
	}

	private void ShowOpponentHighScore()
	{
		if (m_hasOpponentScoreReady)
		{
			if (m_FBPhotoFrame != null)
			{
				m_FBPhotoFrame.hidden = false;
			}
			if (m_FBScoreFrame != null)
			{
				m_FBScoreFrame.hidden = false;
			}
			if (m_FBScoreText != null)
			{
				m_FBScoreText.hidden = false;
			}
			if (m_FBScoreName != null)
			{
				m_FBScoreName.hidden = false;
			}
			if (m_FBFaceTextureHolder != null)
			{
				m_FBFaceTextureHolder.hidden = false;
			}
			if (m_FBFaceTexture != null)
			{
				m_FBFaceTexture.GetComponent<Renderer>().enabled = true;
			}
		}
	}

	public void ShowHUDNoCameraNoBoss()
	{
		ShowPauseButton();
		ShowScoreTokens();
		ShowOpponentHighScore();
	}

	public void ShowHUDWithBoss()
	{
		ShowPauseButton();
		ShowScoreTokens();
		ShowBossGUI();
		ShowWeaponHUD();
	}

	public void ShowScoreTokens()
	{
		m_tokenText.hidden = false;
		m_tokenIcon.hidden = false;
		m_scoreText.hidden = false;
		m_scoreFrame.hidden = false;
		if (PlayerData.RoundScore >= PlayerData.HighestAllTimeScore)
		{
			m_highScoreCrown.hidden = false;
		}
		else
		{
			m_highScoreCrown.hidden = true;
		}
		RefreshScoreMultiplier();
	}

	public void ShowNotification(string notificationText)
	{
		if (GameManager.The.IsLowEndDevice() || GameManager.IsiPhone4S())
		{
			return;
		}
		if (!m_notificationText.hidden)
		{
			m_queuedNotifications.Enqueue(notificationText);
			return;
		}
		m_notificationText.text = notificationText;
		m_notificationFrame.positionFromTop(0f);
		Vector3 target = new Vector3(m_notificationFrame.position.x, m_notificationFrame.position.y + m_notificationFrame.height * 1.2f, m_notificationFrame.position.z);
		m_notificationAnim = m_notificationFrame.positionFrom(0.5f, target, Easing.Linear.easeInOut);
		m_notificationAnim.onComplete = delegate
		{
			Invoke("RetractNotification", 2f);
		};
		m_notificationFrame.hidden = false;
		m_notificationText.hidden = false;
	}

	private void RetractNotification()
	{
		if (m_notificationFrame.hidden)
		{
			return;
		}
		m_notificationFrame.positionFromTop(0f);
		Vector3 target = new Vector3(m_notificationFrame.position.x, m_notificationFrame.position.y + m_notificationFrame.height * 1.2f, m_notificationFrame.position.z);
		m_notificationAnim = m_notificationFrame.positionTo(0.5f, target, Easing.Linear.easeInOut);
		m_notificationAnim.onComplete = delegate
		{
			HideNotification();
			if (m_queuedNotifications.Count > 0)
			{
				string notificationText = m_queuedNotifications.Dequeue();
				ShowNotification(notificationText);
			}
		};
	}

	public void ShowBossGUI()
	{
		if (GameManager.currentGameplayState == GameManager.GameplayState.GamePlay_Action && (DoofenCruiser.The().IsActive() || (Balloony.The != null && Balloony.The.IsActive())))
		{
			m_bossHealthBar.hidden = false;
			m_bossHealthFrame.hidden = false;
			if (DoofenCruiser.The().IsActive())
			{
				m_bossHealthFaceDoof.hidden = false;
			}
			else if (Balloony.The != null && Balloony.The.IsActive())
			{
				m_bossHealthFaceBalloony.hidden = false;
			}
		}
	}

	public void BossAttackStart()
	{
		m_bossHealthBar.value = 0f;
		ShowBossGUI();
		HideOpponentHighScore();
		StartCoroutine(growHealthBar(m_bossHealthBar));
		ShowWeaponHUD();
		ShowWeaponHUDFirstTime();
	}

	public void ShowWeaponHUD()
	{
		if (GameManager.currentGameplayState == GameManager.GameplayState.GamePlay_Action && DoofenCruiser.The().IsActive())
		{
			if (DoofenCruiser.The().m_WeaponChosen == DoofenCruiser.WeaponType.Ice)
			{
				m_fireWeaponIcon.hidden = false;
				m_fireWeaponIcon.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				if (!PlayerData.hasFireWeapon)
				{
					m_fireWeaponIcon.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 85);
					m_lockIcon.parentUIObject = m_fireWeaponIcon;
					m_lockIcon.positionCenter();
					m_lockIcon.hidden = false;
				}
			}
			else if (DoofenCruiser.The().m_WeaponChosen == DoofenCruiser.WeaponType.Fire)
			{
				m_waterWeaponIcon.hidden = false;
				m_waterWeaponIcon.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				if (!PlayerData.hasWaterWeapon)
				{
					m_waterWeaponIcon.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 85);
					m_lockIcon.parentUIObject = m_waterWeaponIcon;
					m_lockIcon.positionCenter();
					m_lockIcon.hidden = false;
				}
			}
			else if (DoofenCruiser.The().m_WeaponChosen == DoofenCruiser.WeaponType.Water)
			{
				m_electricWeaponIcon.hidden = false;
				m_electricWeaponIcon.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				if (!PlayerData.hasElectricWeapon)
				{
					m_electricWeaponIcon.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 85);
					m_lockIcon.parentUIObject = m_electricWeaponIcon;
					m_lockIcon.positionCenter();
					m_lockIcon.hidden = false;
				}
			}
		}
		else if (Balloony.The != null && Balloony.The.IsActive())
		{
			m_pinWeaponIcon.hidden = false;
			m_pinWeaponIcon.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			if (!PlayerData.HasPinWeapon)
			{
				m_pinWeaponIcon.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 85);
				m_lockIcon.parentUIObject = m_pinWeaponIcon;
				m_lockIcon.positionCenter();
				m_lockIcon.hidden = false;
			}
		}
	}

	private void ShowWeaponHUDFirstTime()
	{
		UISprite weapon = null;
		if (!m_fireWeaponIcon.hidden)
		{
			weapon = m_fireWeaponIcon;
		}
		else if (!m_waterWeaponIcon.hidden)
		{
			weapon = m_waterWeaponIcon;
		}
		else if (!m_electricWeaponIcon.hidden)
		{
			weapon = m_electricWeaponIcon;
		}
		else if (!m_pinWeaponIcon.hidden)
		{
			weapon = m_pinWeaponIcon;
		}
		if (weapon == null)
		{
			Debug.Log("Odd...The weapon for the SHowWeaponHUDFirstTime is null.");
			return;
		}
		MainMenuEventManager.TriggerStartAnimation();
		UIAnimation ani = weapon.positionFromTo(3f, m_WeaponHidingPlace, m_WeaponShowingPlace, Easing.Sinusoidal.easeInOut);
		if (!m_lockIcon.hidden)
		{
		}
		ani.onComplete = delegate
		{
			Color clr = weapon.color;
			ani = weapon.colorTo(0.5f, Color.black, Easing.Linear.easeInOut);
			ani.onComplete = delegate
			{
				ani = weapon.colorTo(0.5f, Color.white, Easing.Linear.easeInOut);
				ani.onComplete = delegate
				{
					ani = weapon.colorTo(0.5f, clr, Easing.Linear.easeInOut);
					ani.onComplete = delegate
					{
						MainMenuEventManager.TriggerEndAnimation();
					};
				};
			};
		};
	}

	private void UnShowWeaponHUD()
	{
		UISprite uISprite = null;
		if (!m_fireWeaponIcon.hidden)
		{
			uISprite = m_fireWeaponIcon;
		}
		else if (!m_waterWeaponIcon.hidden)
		{
			uISprite = m_waterWeaponIcon;
		}
		else if (!m_electricWeaponIcon.hidden)
		{
			uISprite = m_electricWeaponIcon;
		}
		else if (!m_pinWeaponIcon.hidden)
		{
			uISprite = m_pinWeaponIcon;
		}
		if (uISprite != null)
		{
			MainMenuEventManager.TriggerStartAnimation();
			UIAnimation uIAnimation = uISprite.positionFromTo(3f, m_WeaponShowingPlace, m_WeaponHidingPlace, Easing.Sinusoidal.easeInOut);
			if (!m_lockIcon.hidden)
			{
			}
			uIAnimation.onComplete = delegate
			{
				HideWeaponHUD();
				MainMenuEventManager.TriggerEndAnimation();
			};
		}
	}

	public void StartShowJumpStartDelayed(float delay)
	{
		Invoke("StartShowJumpStart", delay);
	}

	public void StartShowJumpStart()
	{
		CancelInvoke("UpdateJumpStartButton");
		CancelInvoke("StartShowJumpStart");
		m_jumpStartButton.hidden = false;
		m_CopterBoostTimer.Start(m_CopterBoostDuration);
		m_IsInJumpStartState = true;
		InvokeRepeating("UpdateJumpStartButton", 0.25f, 0.25f);
	}

	public void ShowJumpStartButton()
	{
		m_jumpStartButton.hidden = false;
	}

	private void UpdateJumpStartButton()
	{
		if (m_CopterBoostTimer.IsFinished())
		{
			CancelInvoke("UpdateJumpStartButton");
			m_jumpStartButton.hidden = true;
			m_IsInJumpStartState = false;
		}
		else if (m_CopterBoostTimer.RetrieveTimeElapsed() <= m_jumpStartFlashDuration)
		{
			if (m_isJumpStartFlashOff)
			{
				Color32 color = m_jumpStartButton.color;
				m_jumpStartButton.color = new Color32(color.r, color.g, color.b, byte.MaxValue);
				m_isJumpStartFlashOff = false;
			}
			else
			{
				Color32 color2 = m_jumpStartButton.color;
				m_jumpStartButton.color = new Color32(color2.r, color2.g, color2.b, 16);
				m_isJumpStartFlashOff = true;
			}
		}
		else
		{
			Color32 color3 = m_jumpStartButton.color;
			m_jumpStartButton.color = new Color32(color3.r, color3.g, color3.b, byte.MaxValue);
		}
	}

	public void UpdateBossHealth(float health)
	{
		m_bossHealthBar.value = health / 100f;
	}

	public void UpdateNextOpponentHighScore()
	{
		if (b_playerIstheHighestScore)
		{
			Debug.Log("player is the highest score! dont show");
			m_hasOpponentScoreReady = false;
			HideOpponentHighScore();
			return;
		}
		PerryLeaderboard.SortCombinedOrderedScores();
		if (PlayerData.PerryLeaderboards == null || PlayerData.PerryLeaderboards.Count == 0 || PlayerData.m_menuHighScoreIndex >= PlayerData.PerryLeaderboards.Count)
		{
			return;
		}
		int totalScores = PlayerData.PerryLeaderboards[PlayerData.m_menuHighScoreIndex].TotalScores;
		if (totalScores == 0)
		{
			return;
		}
		int index = 0;
		for (int i = 0; i < totalScores; i++)
		{
			if (PlayerData.PerryLeaderboards == null || PlayerData.m_menuHighScoreIndex >= PlayerData.PerryLeaderboards.Count)
			{
				continue;
			}
			PerryHighScore combinedOrderedScoreAtIndex = PlayerData.PerryLeaderboards[PlayerData.m_menuHighScoreIndex].GetCombinedOrderedScoreAtIndex(i);
			Debug.Log(i + " is the current score, with a value of " + combinedOrderedScoreAtIndex.Name);
			if (combinedOrderedScoreAtIndex.ScoreVal <= PlayerData.RoundScore)
			{
				if (i == 0)
				{
					b_playerIstheHighestScore = true;
				}
				break;
			}
			index = i;
		}
		if (b_playerIstheHighestScore)
		{
			Debug.Log("player has become the highest score");
			PlayerData.NextScoreToBeat = long.MaxValue;
			HideOpponentHighScore();
			m_hasOpponentScoreReady = false;
			return;
		}
		PerryHighScore combinedOrderedScoreAtIndex2 = PlayerData.PerryLeaderboards[PlayerData.m_menuHighScoreIndex].GetCombinedOrderedScoreAtIndex(index);
		m_FBScoreName.text = combinedOrderedScoreAtIndex2.Name;
		m_FBScoreText.text = combinedOrderedScoreAtIndex2.ScoreVal.ToString("N0");
		PlayerData.NextScoreToBeat = combinedOrderedScoreAtIndex2.ScoreVal;
		if (combinedOrderedScoreAtIndex2.FaceUrl != string.Empty && combinedOrderedScoreAtIndex2.FaceUrl.Length > 1)
		{
			if (m_FBFaceTexture != null)
			{
				m_FBFaceTexture.GetComponent<Renderer>().enabled = false;
				UnityEngine.Object.Destroy(m_FBFaceTexture.gameObject);
			}
			m_FBFaceTexture = UnityEngine.Object.Instantiate(GlobalGUIManager.The.faceTexture, Vector3.zero, Quaternion.Euler(new Vector3(90f, 180f, 0f))) as FaceTexture;
			m_FBFaceTexture.HUDparent = m_FBFaceTextureHolder;
			m_FBFaceTexture.LoadTexture(combinedOrderedScoreAtIndex2.FaceUrl);
			float num = m_FBFaceTextureHolder.height / 12f;
			m_FBFaceTexture.transform.localScale = Vector3.one * num;
			m_FBFaceTexture.upClip = Screen.height;
			m_FBFaceTexture.downClip = -Screen.height;
			m_FBFaceTexture.SetClipping();
			m_FBFaceTexture.GetComponent<Renderer>().enabled = !m_FBPhotoFrame.hidden;
		}
		m_hasOpponentScoreReady = true;
	}

	private IEnumerator growHealthBar(UIProgressBar healthBar)
	{
		healthBar.value = 0f;
		while (healthBar.value < 1f)
		{
			healthBar.value += 0.01f;
			yield return 0;
		}
	}

	public void EndBossFightGUI()
	{
		HideBossHUD();
		UnShowWeaponHUD();
		if (GameManager.currentGameplayState != GameManager.GameplayState.GameRestart_Menu)
		{
			ShowOpponentHighScore();
		}
	}

	public void UpdateTokens(int tokenVal)
	{
		m_tokenText.text = PlayerData.RoundTokens.ToString("N0");
		UpdateScore(tokenVal);
	}

	public void UpdateScore(int scoreVal)
	{
		PlayerData.RoundScore += scoreVal * AllMissionData.TotalScoreMultiplier;
		PlayerData.AllTimeScore += scoreVal * AllMissionData.TotalScoreMultiplier;
		m_scoreText.text = PlayerData.RoundScore.ToString("N0");
		if (PlayerData.RoundScore > PlayerData.HighestAllTimeScore)
		{
			if (m_highScoreCrown.hidden && !m_IsShowingBossTutorial)
			{
				m_highScoreCrown.scaleFrom(0.3f, new Vector3(3f, 3f, 1f), Easing.Linear.easeInOut);
				m_highScoreCrown.hidden = false;
			}
		}
		else
		{
			m_highScoreCrown.hidden = true;
		}
	}

	public void UpdateScoreMultiplier()
	{
		RefreshScoreMultiplier();
	}

	public void RefreshScoreMultiplier()
	{
		m_multiplierLabel.hidden = false;
		m_multiplierLabel.text = "x" + AllMissionData.TotalScoreMultiplier.ToString("N0");
		if (GameManager.The.IsScoreMultiplierOn())
		{
			m_multiplierLabel.color = Color.red;
		}
		else
		{
			m_multiplierLabel.color = Color.white;
		}
		m_multiplierLabel.positionFromTopLeft(0.25f, 0.05f);
		if (AllMissionData.TotalScoreMultiplier < 10)
		{
			m_multiplierLabel.positionFromTopLeft(0.25f, 0.08f);
		}
		m_multiplierLabel.hidden = false;
	}

	public void onTouchUpInsidePauseButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		sender.disabled = true;
		Debug.Log("UI PAUSED!");
		MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Pause_Menu);
		GameEventManager.TriggerGamePause();
		sender.disabled = false;
	}

	public void onTouchUpInsideCameraButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		sender.disabled = true;
		Debug.Log("Start Recording");
		MainMenuEventManager.TriggerStartRecording();
	}

	public void onTouchUpInsideJumpStartButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		sender.disabled = true;
		Debug.Log("Jump Start Button");
		CancelInvoke("UpdateJumpStartButton");
		m_jumpStartButton.hidden = true;
		m_IsInJumpStartState = false;
		MainMenuEventManager.TriggerUseJumpStart();
		sender.disabled = false;
	}
	
	#region Adaptive Scaling Helper Methods
	
	/// Most of this code is new
	private float GetSafeAreaTopOffset()
	{
		float safeOffset = Screen.height - (Screen.safeArea.y + Screen.safeArea.height);
		// Convert to percentage of screen height
		return safeOffset / Screen.height;
	}
	
	private float GetSafeAreaRightOffset()
	{
		float safeOffset = Screen.width - (Screen.safeArea.x + Screen.safeArea.width);
		return safeOffset / Screen.width;
	}

	private float GetSafeAreaBottomOffset()
	{
		float safeOffset = Screen.safeArea.y;
		return safeOffset / Screen.height;
	}

	private float GetSafeAreaLeftOffset()
	{
		float safeOffset = Screen.safeArea.x;
		return safeOffset / Screen.width;
	}

	private float GetAdaptiveButtonScale()
	{
		float scale = 1.5f;
		
		if (UI.scaleFactor >= 4)
		{
			scale = 1.0f; 
		}
		else if (UI.scaleFactor >= 2)
		{
			scale = 1.25f;
		}
		
		if (UIHelper.IsModernPhone())
		{
			scale *= 1.1f; 
		}
		
		return scale;
	}

	private float GetAdaptiveFontScale()
	{
		float baseScale = 1.0f;
		
		float heightRatio = Screen.height / 2048f; 
		baseScale = Mathf.Clamp(heightRatio, 0.7f, 1.5f);
		
		if (UIHelper.IsModernPhone())
		{
			baseScale *= 0.9f; 
		}
		
		return baseScale;
	}
	
	private float GetAdaptiveScoreTextPosition()
	{
		float baseOffset = 0.025f;
		
		float heightRatio = Screen.height / 2048f;
		
		if (heightRatio >= 1.2f)
		{
			baseOffset = 0.014f; 
		}
		else if (heightRatio >= 0.9f)
		{
			baseOffset = 0.018f;
		}
		else if (heightRatio >= 0.6f)
		{
			baseOffset = 0.025f; 
		}
		else
		{
			baseOffset = 0.016f; 
		}
		
		return baseOffset;
	}
	
	private float GetAdaptiveTokenTextPosition()
	{
		float baseOffset = 0.09f;
		
		float heightRatio = Screen.height / 2048f;
		
		if (heightRatio >= 1.2f)
		{
			baseOffset = 0.085f; 
		}
		else if (heightRatio >= 0.9f)
		{
			baseOffset = 0.087f; 
		}
		else if (heightRatio >= 0.6f)
		{
			baseOffset = 0.09f; 
		}
		else
		{
			baseOffset = 0.085f; 
		}
		
		return baseOffset;
	}
	
	#endregion
}

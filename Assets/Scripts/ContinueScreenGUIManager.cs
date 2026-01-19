using System.Collections.Generic;
using UnityEngine;

public class ContinueScreenGUIManager : MonoBehaviour
{
	private static UIButton m_popUpWindow;

	private static UITextInstance m_continueMessage;

	private static UIHorizontalLayout m_costLayout;

	private static UITextInstance m_continueCostText;

	private static UIButton m_speedupButton;

	private static UISprite m_timerBg;

	private static UIProgressBar m_timer;

	private static UISprite m_fedoraSpriteInvisible;

	private bool b_isFedoraPurchasePopupActive;

	private static int m_popUpStartDepth;

	private int m_numDoubles = 1;

	private float m_startTime;

	private bool m_countdown;

	private static float COUNTDOWN_TICK = 0.1f;

	private static float updateInterval = 1f;

	private bool m_shouldTriggerPlacement = true;

	private static ContinueScreenGUIManager m_the;

	public static ContinueScreenGUIManager The
	{
		get
		{
			if (m_the == null)
			{
				GameObject gameObject = new GameObject("ContinueScreenGUIManager");
				((ContinueScreenGUIManager)gameObject.AddComponent<ContinueScreenGUIManager>()).Init();
			}
			return m_the;
		}
	}

	private void Awake()
	{
		GameEventManager.GameStart += GameStartListener;
	}

	private void OnDestroy()
	{
		GameEventManager.GameStart -= GameStartListener;
	}

	private void GameStartListener()
	{
		m_numDoubles = 1;
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	public void Init()
	{
		if (!(m_the != null))
		{
			m_the = this;
			Object.DontDestroyOnLoad(base.gameObject);
			InitPopupWindow();
			HideAll();
		}
	}

	private void InitPopupWindow()
	{
		m_costLayout = new UIHorizontalLayout(10);
		m_costLayout.alignMode = UIAbstractContainer.UIContainerAlignMode.Center;
		m_costLayout.verticalAlignMode = UIAbstractContainer.UIContainerVerticalAlignMode.Middle;
		m_popUpWindow = UIButton.create(GlobalGUIManager.The.m_hudToolkit, "ContinuePopUp.png", "ContinuePopUpOver.png", 0, 0, m_popUpStartDepth + 3);
		m_popUpWindow.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_popUpWindow.centerize();
		m_popUpWindow.positionFromCenter(0f, 0f);
		m_popUpWindow.onTouchUpInside += onTouchUpInsideContinueButton;
		float num = 1f;
		if (m_popUpWindow.width > (float)Screen.width)
		{
			float num2 = 30f;
			float num3 = (float)Screen.width - num2;
			num = num3 / m_popUpWindow.width;
			m_popUpWindow.scale = new Vector3(num, num, 1f);
		}
		m_continueMessage = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_CONTINUE_"), 0f, 0f, 1f, m_popUpStartDepth + 2);
		m_continueMessage.alignMode = UITextAlignMode.Center;
		m_continueMessage.textScale = 0.8f;
		if (num < 1f)
		{
			m_continueMessage.textScale *= num * 0.98f;
		}
		m_continueMessage.setColorForAllLetters(Color.black);
		m_continueMessage.parentUIObject = m_popUpWindow;
		m_continueMessage.positionFromTop(0.13f);
		m_continueMessage.text = LocalTextManager.GetUIText("_CONTINUE_");
		m_fedoraSpriteInvisible = GlobalGUIManager.The.m_menuToolkit.addSprite("clear.png", 0, 0, m_popUpStartDepth + 2);
		m_fedoraSpriteInvisible.centerize();
		m_fedoraSpriteInvisible.scale = new Vector3(num, num, 1f);
		m_fedoraSpriteInvisible.hidden = true;
		m_costLayout.addChild(m_fedoraSpriteInvisible);
		UpdateCostInt();
		m_continueCostText = GlobalGUIManager.The.defaultTextAlt.addTextInstance("x" + PlayerData.ContinueCost.ToString("N0"), 0f, 0f, 1f, m_popUpStartDepth + 1);
		m_continueCostText.alignMode = UITextAlignMode.Left;
		m_continueCostText.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_continueCostText.textScale = 0.8f;
		if (num < 1f)
		{
			m_continueCostText.textScale *= num * 0.98f;
		}
		m_continueCostText.setColorForAllLetters(Color.white);
		m_continueCostText.parentUIObject = m_popUpWindow;
		m_continueCostText.positionFromBottom(0.25f, 0.07f);
		m_continueCostText.text = "x" + PlayerData.ContinueCost.ToString("N0");
		m_costLayout.matchSizeToContentSize();
		m_costLayout.parentUIObject = m_popUpWindow;
		m_costLayout.positionFromTop(0.15f);
		m_speedupButton = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "clear.png", "clear.png", 0, 0, m_popUpStartDepth + 12);
		m_speedupButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		m_speedupButton.centerize();
		m_speedupButton.scale = new Vector3(Screen.width, Screen.height, 1f);
		m_speedupButton.onTouchUpInside += onTouchUpInsideSpeedupWindow;
		m_timer = UIProgressBar.create(GlobalGUIManager.The.m_hudToolkit, "ContinueCounterFull.png", 0, 0, false, m_popUpStartDepth + 3);
		m_timer.parentUIObject = m_popUpWindow;
		m_timer.positionFromBottom(-0.5f, -0.25f);
		m_timer.resizeTextureOnChange = true;
		m_timer.value = 1f;
		m_timerBg = GlobalGUIManager.The.m_hudToolkit.addSprite("ContinueCounterEmpty.png", 0, 0, m_popUpStartDepth + 4);
		m_timerBg.parentUIObject = m_timer;
		m_timerBg.positionCenter();
	}

	public void HideAll()
	{
		m_popUpWindow.hidden = true;
		m_continueMessage.hidden = true;
		m_continueCostText.hidden = true;
		m_speedupButton.hidden = true;
		m_timer.hidden = true;
		m_timerBg.hidden = true;
		StopCountdown();
	}

	public void ShowContinueScreen()
	{
		GlobalGUIManager.The.CurrentMenuState = MainMenuEventManager.MenuState.Game_ContinueScreen;
		ShowAllGUIElements();
		UpdateCostText();
		StartCountdown();
	}

	private void ShowAllGUIElements()
	{
		m_popUpWindow.hidden = false;
		m_continueMessage.hidden = false;
		m_continueCostText.hidden = false;
		m_speedupButton.hidden = false;
		m_timer.hidden = false;
		m_timerBg.hidden = false;
	}

	private void UpdateCostInt()
	{
		int baseContinueCost = PlayerData.BaseContinueCost;
		baseContinueCost *= m_numDoubles;
		PlayerData.ContinueCost = baseContinueCost;
	}

	private void UpdateCostText()
	{
		m_continueCostText.text = "x" + PlayerData.ContinueCost;
	}

	private void StopCountdown()
	{
		m_countdown = false;
		CancelInvoke("UpdateCountdown");
	}

	private void StartCountdown()
	{
		m_startTime = Time.time;
		m_countdown = true;
		InvokeRepeating("UpdateCountdown", COUNTDOWN_TICK, COUNTDOWN_TICK);
	}

	private void UpdateCountdown()
	{
		if (m_countdown)
		{
			float num = Time.time - m_startTime;
			float num2 = num / 5f;
			m_timer.value = 1f - num2;
			if (num2 >= 1f)
			{
				Debug.Log("TriggerGameRestartMenu");
				GameManager.The.CullSoundChannels();
				MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.InGame_IntroAnim);
				m_shouldTriggerPlacement = true;
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("Tokens", PlayerData.RoundTokens.ToString());
				dictionary.Add("Fedoras", PlayerData.RoundFedoras.ToString());
				FlurryFacade.Instance.LogEvent("RoundCurrency", dictionary, false);
				GameEventManager.TriggerGameRestartMenu();
			}
		}
	}

	public void onTouchUpInsideContinueButton(UIButton sender)
	{
		if (PopUpGUIManager.The.isAPopupActive)
		{
			return;
		}
		StopCountdown();
		HUDGUIManager.The.HideAll();
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		if (PlayerData.playerFedoras >= PlayerData.ContinueCost)
		{
			DeductFedorasAndContinue();
			return;
		}
		HideAll();
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			PopUpGUIManager.The.ShowNeedMoreTokensPopUp(LocalTextManager.GetUIText("_CONTINUE_"), 0, PlayerData.ContinueCost, BuyFedoraPackPopUpCancel, BuyFedoraPackPopUpCancel, delegate
			{
				OnCantBuyContinuePopUpDisplay();
			});
		}
		else
		{
			DisplayBuyFedoraForContinuePopUp();
		}
	}

	private void DeductFedorasAndContinue()
	{
		HUDGUIManager.The.ShowScoreTokens();
		PlayerData.playerFedoras -= PlayerData.ContinueCost;
		m_numDoubles *= 2;
		UpdateCostInt();
		UpdateCostText();
		PlayerData.RoundContinues++;
		StopCountdown();
		MainMenuEventManager.TriggerGoToNextMenu(MainMenuEventManager.MenuState.Game_ContinueGame);
		m_popUpWindow.disabled = false;
	}

	private void DisplayBuyFedoraForContinuePopUp()
	{
		PurchasableItem fedoraItem = AllItemData.GetTokenItemClosestToFedoraPrice(Mathf.Max(0, PlayerData.ContinueCost - PlayerData.playerFedoras));
		if (fedoraItem == null)
		{
			ShowAllGUIElements();
			StartCountdown();
		}
		else
		{
			PopUpGUIManager.The.ShowOfferToBuyMoreFedorasForContinue(ref fedoraItem, BuyFedoraPackPopUpOK, BuyFedoraPackPopUpCancel, delegate
			{
				OnCantBuyContinuePopUpDisplay();
			});
		}
	}

	public void FakeTouchUpSpeedupWindow()
	{
		onTouchUpInsideSpeedupWindow(m_speedupButton);
	}

	public void onTouchUpInsideSpeedupWindow(UIButton sender)
	{
		if (!b_isFedoraPurchasePopupActive)
		{
			GameManager.The.PlayClip(AudioClipFiles.UICLICK);
			sender.disabled = true;
			m_startTime -= updateInterval;
			UpdateCountdown();
			sender.disabled = false;
		}
	}

	private void BuyFedoraPackPopUpOK(object fedoraPack)
	{
		PurchasableItem pi = (PurchasableItem)fedoraPack;
		AllItemData.BuyRealMoneyItem(true, pi, delegate(bool success)
		{
			string[] array = new string[1] { LocalTextManager.GetUIText("_OK_") };
			if (success)
			{
				PopUpGUIManager.The.ShowPurchaseOutcomeConfirmation(LocalTextManager.GetUIText("_PURCHASE_COMPLETE_"), OnPurchaseOutcomePositive, delegate
				{
				});
			}
			else
			{
				PopUpGUIManager.The.ShowPurchaseOutcomeConfirmation(LocalTextManager.GetUIText("_PURCHASE_FAILED_"), OnPurchaseOutcomeNegative, delegate
				{
				});
			}
		});
		b_isFedoraPurchasePopupActive = false;
	}

	private void BuyFedoraPackPopUpCancel(object tokenPack)
	{
		m_popUpWindow.hidden = false;
		m_continueMessage.hidden = false;
		m_continueCostText.hidden = false;
		m_speedupButton.hidden = false;
		m_timer.hidden = false;
		m_timerBg.hidden = false;
		b_isFedoraPurchasePopupActive = false;
		m_popUpWindow.disabled = false;
		m_timer.value = 1f;
		StartCountdown();
		HUDGUIManager.The.ShowScoreTokens();
	}

	private void OnCantBuyContinuePopUpDisplay()
	{
		m_popUpWindow.hidden = true;
		m_continueMessage.hidden = true;
		m_continueCostText.hidden = true;
		m_speedupButton.hidden = true;
		m_timer.hidden = true;
		m_timerBg.hidden = true;
		b_isFedoraPurchasePopupActive = true;
		m_popUpWindow.disabled = true;
		m_timer.value = 1f;
		StopCountdown();
	}

	private void OnPurchaseOutcomePositive(object o)
	{
		StoreGUIManagerPersistentElements.The.UpdateMoney();
		DeductFedorasAndContinue();
	}

	private void OnPurchaseOutcomeNegative(object o)
	{
		StoreGUIManagerPersistentElements.The.UpdateMoney();
		BuyFedoraPackPopUpCancel(null);
	}

	public void ReloadStaticText()
	{
		m_continueMessage.text = LocalTextManager.GetUIText("_CONTINUE_");
	}

	private void PurchaseCancelled(string error)
	{
		if (GameManager.currentGameplayState == GameManager.GameplayState.GameOver_ContinueMenu)
		{
			BuyFedoraPackPopUpCancel(null);
		}
	}

	public void BossRoundDefeatPlacements()
	{
		if (m_shouldTriggerPlacement)
		{
			if (PlayerData.RoundBossDefeats == 1)
			{
				PlayHavenController.ContentRequest(PlayHavenController.PlayHavenPlacement.LevelEndWin1);
				m_shouldTriggerPlacement = false;
			}
			if (PlayerData.RoundBossDefeats == 2)
			{
				PlayHavenController.ContentRequest(PlayHavenController.PlayHavenPlacement.LevelEndWin2);
				m_shouldTriggerPlacement = false;
			}
			if (PlayerData.RoundBossDefeats == 3)
			{
				PlayHavenController.ContentRequest(PlayHavenController.PlayHavenPlacement.LevelEndWin3);
				m_shouldTriggerPlacement = false;
			}
		}
	}

	public void BossAllTimeDefeatPlacements()
	{
		if (m_shouldTriggerPlacement)
		{
			if (PlayerData.AllTimeBossDefeats == 5)
			{
				PlayHavenController.ContentRequest(PlayHavenController.PlayHavenPlacement.CumulativeWins5);
				m_shouldTriggerPlacement = false;
			}
			if (PlayerData.AllTimeBossDefeats == 10)
			{
				PlayHavenController.ContentRequest(PlayHavenController.PlayHavenPlacement.CumulativeWins10);
				m_shouldTriggerPlacement = false;
			}
			if (PlayerData.AllTimeBossDefeats == 15)
			{
				PlayHavenController.ContentRequest(PlayHavenController.PlayHavenPlacement.CumulativeWins15);
				m_shouldTriggerPlacement = false;
			}
		}
	}

	public void MultiplierPlacements()
	{
		if (m_shouldTriggerPlacement)
		{
			switch (AllMissionData.TotalScoreMultiplier)
			{
			case 3:
				PlayHavenController.ContentRequest(PlayHavenController.PlayHavenPlacement.UserLevelUp3);
				m_shouldTriggerPlacement = false;
				break;
			case 6:
				PlayHavenController.ContentRequest(PlayHavenController.PlayHavenPlacement.UserLevelUp6);
				m_shouldTriggerPlacement = false;
				break;
			case 9:
				PlayHavenController.ContentRequest(PlayHavenController.PlayHavenPlacement.UserLevelUp9);
				m_shouldTriggerPlacement = false;
				break;
			case 12:
				PlayHavenController.ContentRequest(PlayHavenController.PlayHavenPlacement.UserLevelUp12);
				m_shouldTriggerPlacement = false;
				break;
			case 15:
				PlayHavenController.ContentRequest(PlayHavenController.PlayHavenPlacement.UserLevelUp15);
				m_shouldTriggerPlacement = false;
				break;
			}
		}
	}

	public void LevelEndPlacements()
	{
		if (m_shouldTriggerPlacement && PlayerData.RoundPowerups == 0)
		{
			PlayHavenController.ContentRequest(PlayHavenController.PlayHavenPlacement.LevelEndNoUpgrade);
		}
		PlayHavenController.ContentRequest(PlayHavenController.PlayHavenPlacement.LevelEnd);
	}

	public void CheckPlayHavenPlacements()
	{
		BossRoundDefeatPlacements();
		BossAllTimeDefeatPlacements();
		MultiplierPlacements();
		LevelEndPlacements();
	}
}

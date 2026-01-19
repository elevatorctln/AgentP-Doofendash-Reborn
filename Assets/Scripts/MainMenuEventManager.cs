public static class MainMenuEventManager
{
	public enum MenuState
	{
		Main_Menu_Character_Select = 0,
		Main_Menu_Missions = 1,
		InGame_IntroAnim = 2,
		InGame_Score_Menu = 3,
		InGame_Missions_Menu = 4,
		Pause_Menu = 5,
		Settings_Menu = 6,
		Settings_Info_Menu = 7,
		Settings_Storage_Menu = 8,
		Pop_Up_Buy_Mission = 9,
		Store_Menu_Tokens = 10,
		Store_Menu_Gadgets = 11,
		Store_Menu_Upgrades = 12,
		Store_Menu_FreeTokens = 13,
		Game_HUD = 14,
		Game_ContinueScreen = 15,
		Game_ContinueGame = 16,
		Story = 17,
		Copa = 18
	}

	public delegate void MainMenuEvent();

	public delegate void MainMenuChangeStateEvent(MenuState menuState);

	public delegate void MissionBuyPopUpEvent(Mission mission);

	public delegate void MissionCompletePopUpEvent(Mission mission, bool getScoreBonus);

	public delegate void BuyStoreItem(PurchasableItem pi);

	public delegate void MainMenuIntEvent(int val);

	public delegate void MonogramTalkEvent(MongramTalkingHandler.MonogramTalkingState state);

	public delegate void ChangeLanguageEvent(LocalTextManager.PerryLanguages language);

	public static event MainMenuEvent SelectNextChar;

	public static event MainMenuEvent SelectPrevChar;

	public static event MainMenuEvent GoToPreviousMenu;

	public static event MainMenuEvent BuyCurrentCharacter;

	public static event MainMenuChangeStateEvent GoToNextMenu;

	public static event MissionBuyPopUpEvent BuyMissionPopUp;

	public static event MissionCompletePopUpEvent CompleteMissionPopUp;

	public static event MainMenuEvent StartAnimation;

	public static event MainMenuEvent EndAnimation;

	public static event BuyStoreItem BuyTokenItem;

	public static event BuyStoreItem BuyGadgetItem;

	public static event BuyStoreItem BuyUpgradeItem;

	public static event BuyStoreItem BuyCharacterItem;

	public static event BuyStoreItem BuyPurchasableItem;

	public static event MainMenuIntEvent PlayWithCurrentCharacter;

	public static event MainMenuEvent StartRecording;

	public static event MainMenuEvent StopRecording;

	public static event MainMenuEvent UseJumpStart;

	public static event MainMenuEvent FinishedCapsuleEndAnimsEvents;

	public static event MonogramTalkEvent StartMonogramTalking;

	public static event MonogramTalkEvent StopMonogramTalking;

	public static event MainMenuIntEvent ShowMeterNotification;

	public static event MissionBuyPopUpEvent ShowMissionNotification;

	public static void TriggerSelectNextChar()
	{
		if (MainMenuEventManager.SelectNextChar != null)
		{
			MainMenuEventManager.SelectNextChar();
		}
	}

	public static void TriggerSelectPrevChar()
	{
		if (MainMenuEventManager.SelectPrevChar != null)
		{
			MainMenuEventManager.SelectPrevChar();
		}
	}

	public static void TriggerBuyCurrentCharacter()
	{
		if (MainMenuEventManager.BuyCurrentCharacter != null)
		{
			MainMenuEventManager.BuyCurrentCharacter();
		}
	}

	public static void TriggerBuyCharacter(PurchasableItem characterItem)
	{
		if (MainMenuEventManager.BuyCharacterItem != null)
		{
			MainMenuEventManager.BuyCharacterItem(characterItem);
		}
	}

	public static void TriggerPlayWithCurrentCharacter(int characterIndex)
	{
		if (MainMenuEventManager.PlayWithCurrentCharacter != null)
		{
			MainMenuEventManager.PlayWithCurrentCharacter(characterIndex);
		}
	}

	public static void TriggerGoToNextMenu(MenuState menuState)
	{
		if (MainMenuEventManager.GoToNextMenu != null)
		{
			MainMenuEventManager.GoToNextMenu(menuState);
		}
	}

	public static void TriggerGoToPreviousMenu()
	{
		if (MainMenuEventManager.GoToPreviousMenu != null)
		{
			MainMenuEventManager.GoToPreviousMenu();
		}
	}

	public static void TriggerBuyMissionPopUp(Mission mission)
	{
		if (MainMenuEventManager.BuyMissionPopUp != null)
		{
			MainMenuEventManager.BuyMissionPopUp(mission);
		}
	}

	public static void TriggerCompleteMissionPopUp(Mission mission, bool getScoreBonus)
	{
		GlobalGUIManager.The.MissionCompletePopUpDone(mission);
	}

	public static void TriggerStartAnimation()
	{
		if (MainMenuEventManager.StartAnimation != null)
		{
			MainMenuEventManager.StartAnimation();
		}
	}

	public static void TriggerEndAnimation()
	{
		if (MainMenuEventManager.EndAnimation != null)
		{
			MainMenuEventManager.EndAnimation();
		}
	}

	public static void TriggerBuyTokenItem(PurchasableItem token)
	{
		if (MainMenuEventManager.BuyTokenItem != null)
		{
			MainMenuEventManager.BuyTokenItem(token);
		}
	}

	public static void TriggerBuyGadgetItem(PurchasableItem gadget)
	{
		if (MainMenuEventManager.BuyGadgetItem != null)
		{
			MainMenuEventManager.BuyGadgetItem(gadget);
		}
	}

	public static void TriggerBuyUpgradeItem(PurchasableItem upgrade)
	{
		if (MainMenuEventManager.BuyUpgradeItem != null)
		{
			MainMenuEventManager.BuyUpgradeItem(upgrade);
		}
	}

	public static void TriggerBuyPurchasableItem(PurchasableItem item)
	{
		if (MainMenuEventManager.BuyPurchasableItem != null)
		{
			MainMenuEventManager.BuyPurchasableItem(item);
		}
	}

	public static void TriggerStartRecording()
	{
		if (MainMenuEventManager.StartRecording != null)
		{
			MainMenuEventManager.StartRecording();
		}
	}

	public static void TriggerStopRecording()
	{
		if (MainMenuEventManager.StopRecording != null)
		{
			MainMenuEventManager.StopRecording();
		}
	}

	public static void TriggerUseJumpStart()
	{
		if (MainMenuEventManager.UseJumpStart != null)
		{
			MainMenuEventManager.UseJumpStart();
		}
	}

	public static void TriggerFinishedCapsuleEndAnims()
	{
		if (MainMenuEventManager.FinishedCapsuleEndAnimsEvents != null)
		{
			MainMenuEventManager.FinishedCapsuleEndAnimsEvents();
		}
	}

	public static void TriggerStartMonogramTalking(MongramTalkingHandler.MonogramTalkingState state)
	{
		if (MainMenuEventManager.StartMonogramTalking != null)
		{
			MainMenuEventManager.StartMonogramTalking(state);
		}
	}

	public static void TriggerStopMonogramTalking(MongramTalkingHandler.MonogramTalkingState state)
	{
		if (MainMenuEventManager.StopMonogramTalking != null)
		{
			MainMenuEventManager.StopMonogramTalking(state);
		}
	}

	public static void TriggerShowMeterNotification(int meters)
	{
		if (MainMenuEventManager.ShowMeterNotification != null)
		{
			MainMenuEventManager.ShowMeterNotification(meters);
		}
	}

	public static void TriggerMissionNotification(Mission mission)
	{
		if (MainMenuEventManager.ShowMissionNotification != null)
		{
			MainMenuEventManager.ShowMissionNotification(mission);
		}
	}
}

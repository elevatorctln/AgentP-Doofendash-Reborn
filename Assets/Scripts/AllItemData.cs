using System;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;

public static class AllItemData
{
	private static ArrayList m_characterItems;

	private static ArrayList m_gadgetItems;

	private static ArrayList m_upgradeItems;

	private static PurchasableItem m_jumpStart;

	private static UpgradableItem m_babyHead;

	private static ArrayList m_miscUpgrades;

	private static ArrayList m_getTokensItems;

	private static bool m_gotTokenItemsFromStore;

	private static ArrayList m_getHiddenTokensItems;

	private static ArrayList m_realMoneyItems;

	public static string ms_ContinueCostReductionUID = "50318";

	public static void Init()
	{
		InitCharacterItems();
		InitGadgetItems();
		InitAbilityUpgrades();
		InitMiscItems();
		InitGetTokensItems();
		m_realMoneyItems = new ArrayList();
	}

	public static void SetAllStoreItems()
	{
		DebugManager.Log("Entering AllItemData.SetAllStoreItems()");
		SetTokensStoreItems();
		DebugManager.Log("Exiting AllItemData.SetAllStoreItems()");
	}

	private static void InitCharacterItems()
	{
		m_characterItems = new ArrayList();
		m_characterItems.Add(new PurchasableItem(LocalTextManager.GetStoreItemText("_AGENT_P_"), "na", true, 0, 0, 0f, true, 0, 0, null, "10123"));
		m_characterItems.Add(new PurchasableItem(LocalTextManager.GetStoreItemText("_AGENT_PINKY_"), "na", false, 25000, 0, 0f, true, 0, 0, null, "10124"));
		m_characterItems.Add(new PurchasableItem(LocalTextManager.GetStoreItemText("_AGENT_PETER_"), "na", false, 50000, 0, 0f, true, 0, 0, null, "10125"));
		m_characterItems.Add(new PurchasableItem(LocalTextManager.GetStoreItemText("_AGENT_T_"), "na", false, 100000, 0, 0f, true, 0, 0, null, "10126"));
		m_characterItems.Add(new PurchasableItem(LocalTextManager.GetStoreItemText("_COCONUT_PERRY_"), "na", false, 200000, 0, 0f, true, 0, 0, null, "10127"));
		m_characterItems.Add(new PurchasableItem(LocalTextManager.GetStoreItemText("_PRINCESS_PINKY_"), "na", false, 50000, 0, 0f, true, 0, 0, null, "10128"));
		m_characterItems.Add(new PurchasableItem(LocalTextManager.GetStoreItemText("_GENTLEMAN_PANDA_"), "na", false, 100000, 0, 0f, true, 0, 0, null, "10129"));
		m_characterItems.Add(new PurchasableItem(LocalTextManager.GetStoreItemText("_VIKING_TERRY_"), "na", false, 200000, 0, 0f, true, 0, 0, null, "10130"));
		m_characterItems.Add(new PurchasableItem(LocalTextManager.GetStoreItemText("_SUPER_AGENT_P_"), "na", false, 0, 400, 0f, true, 0, 0, null, "10131"));
		m_characterItems.Add(new PurchasableItem(LocalTextManager.GetStoreItemText("_FUNKY_PERRY_"), "na", false, 100000, 0, 0f, true, 0, 0, null, "10132"));
		for (int i = 0; i < m_characterItems.Count; i++)
		{
			PurchasableItem pi = (PurchasableItem)m_characterItems[i];
			PerryiCloudManager.The.UpdatePurchasableItemDataFromPrefs(pi);
		}
	}

	public static void SaveAllCharacterItems()
	{
		if (m_characterItems == null)
		{
			return;
		}
		foreach (PurchasableItem characterItem in m_characterItems)
		{
			PerryiCloudManager.The.SetItem(characterItem.UID, characterItem);
		}
	}

	public static void SaveAllGadgetItems()
	{
		if (m_gadgetItems == null)
		{
			return;
		}
		foreach (PurchasableGadgetItem gadgetItem in m_gadgetItems)
		{
			PerryiCloudManager.The.SetGadgetItem(gadgetItem.UID, gadgetItem);
		}
	}

	public static void SaveAllUpgradableItems()
	{
		if (m_upgradeItems == null)
		{
			return;
		}
		foreach (UpgradableItem upgradeItem in m_upgradeItems)
		{
			PerryiCloudManager.The.SetUpgradableItem(upgradeItem.UID, upgradeItem);
		}
	}

	public static void SaveAll()
	{
		SaveAllCharacterItems();
		SaveAllGadgetItems();
		SaveAllUpgradableItems();
	}

	public static void SaveAllCharacterItemsInitialValues()
	{
		if (m_characterItems == null)
		{
			return;
		}
		foreach (PurchasableItem characterItem in m_characterItems)
		{
			PerryiCloudManager.The.SetItemInitialValues(characterItem.UID, characterItem);
		}
	}

	public static void SaveAllGadgetItemsInitialValues()
	{
		if (m_gadgetItems == null)
		{
			return;
		}
		foreach (PurchasableGadgetItem gadgetItem in m_gadgetItems)
		{
			PerryiCloudManager.The.SetGadgetItemInitialValues(gadgetItem.UID, gadgetItem);
		}
	}

	public static void SaveAllUpgradableItemsInitialValues()
	{
		if (m_upgradeItems == null)
		{
			return;
		}
		foreach (UpgradableItem upgradeItem in m_upgradeItems)
		{
			PerryiCloudManager.The.SetUpgradableItemInitialValues(upgradeItem.UID, upgradeItem);
		}
	}

	public static void SaveAllInitialValues()
	{
		SaveAllCharacterItemsInitialValues();
		SaveAllGadgetItemsInitialValues();
		SaveAllUpgradableItemsInitialValues();
	}

	public static void RemoveAllCharacterItemsInitialValues()
	{
		if (m_characterItems == null)
		{
			return;
		}
		foreach (PurchasableItem characterItem in m_characterItems)
		{
			PerryiCloudManager.The.RemoveItemInitialValues(characterItem.UID, characterItem);
		}
	}

	public static void RemoveAllGadgetItemsInitialValues()
	{
		if (m_gadgetItems == null)
		{
			return;
		}
		foreach (PurchasableGadgetItem gadgetItem in m_gadgetItems)
		{
			PerryiCloudManager.The.RemoveItemInitialValues(gadgetItem.UID, gadgetItem);
		}
	}

	public static void RemoveAllUpgradableItemsInitialValues()
	{
		if (m_upgradeItems == null)
		{
			return;
		}
		foreach (UpgradableItem upgradeItem in m_upgradeItems)
		{
			PerryiCloudManager.The.RemoveItemInitialValues(upgradeItem.UID, upgradeItem);
		}
	}

	public static void RemoveAllInitialValues()
	{
		RemoveAllCharacterItemsInitialValues();
		RemoveAllGadgetItemsInitialValues();
		RemoveAllUpgradableItemsInitialValues();
	}

	public static PurchasableItem[] GetCharacterItems()
	{
		return (PurchasableItem[])m_characterItems.ToArray(typeof(PurchasableItem));
	}

	public static PurchasableItem GetCharacterItemByUniqueID(string uid)
	{
		foreach (PurchasableItem characterItem in m_characterItems)
		{
			if (characterItem.UID == uid)
			{
				return characterItem;
			}
		}
		DebugManager.Log("Couldn't find character item by unique id: " + uid);
		return null;
	}

	public static PurchasableItem GetCharacterItem(PurchasableItem charItem)
	{
		foreach (PurchasableItem characterItem in m_characterItems)
		{
			if (characterItem.UID == charItem.UID)
			{
				return characterItem;
			}
		}
		DebugManager.Log("Couldn't find character item " + charItem.m_name);
		return null;
	}

	public static void BuyCharacterItem(PurchasableItem pc)
	{
		bool flag = false;
		for (int i = 0; i < m_characterItems.Count; i++)
		{
			PurchasableItem purchasableItem = (PurchasableItem)m_characterItems[i];
			if (purchasableItem.UID == pc.UID)
			{
				purchasableItem.m_owned = true;
				purchasableItem.m_numOwned = 1;
				PerryiCloudManager.The.SetItem(purchasableItem.UID, purchasableItem);
				DebugManager.Log("Updated " + purchasableItem.m_name + " item to owned");
				if (purchasableItem.UID == "10124")
				{
					PlayerData.OwnPinky = true;
				}
				if (purchasableItem.UID == "10125")
				{
					PlayerData.OwnPeter = true;
				}
				if (purchasableItem.UID == "10126")
				{
					PlayerData.OwnTerry = true;
				}
				switch (purchasableItem.UID)
				{
				case "10127":
				case "10128":
				case "10129":
				case "10130":
				case "10131":
					PlayerData.CostumesOwned++;
					break;
				}
				flag = true;
			}
		}
		if (!flag)
		{
			DebugManager.Log("Couldn't update buy data for this item!");
		}
	}

	private static void InitGadgetItems()
	{
		m_gadgetItems = new ArrayList();
		string storeItemText = LocalTextManager.GetStoreItemText("_POWER_UP_");
		PurchasableGadgetItem purchasableGadgetItem = new PurchasableGadgetItem(LocalTextManager.GetStoreItemText("_WATER_CANNON_NAME_"), UIHelper.WordWrap(LocalTextManager.GetStoreItemText("_WATER_CANNON_DESC_"), 28), false, 10000, 0, 0f, 0, 0.2f, 1, storeItemText, "WaterGunIcon.png", "20124");
		purchasableGadgetItem.m_buyExtraText = string.Empty;
		purchasableGadgetItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtWaterGadget;
		m_gadgetItems.Add(purchasableGadgetItem);
		purchasableGadgetItem = new PurchasableGadgetItem(LocalTextManager.GetStoreItemText("_TORCH_CANNON_NAME_"), UIHelper.WordWrap(LocalTextManager.GetStoreItemText("_TORCH_CANNON_DESC_"), 28), false, 10000, 0, 0f, 0, 0.2f, 1, storeItemText, "FireGunIcon.png", "20125");
		purchasableGadgetItem.m_buyExtraText = string.Empty;
		purchasableGadgetItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtFireGadget;
		m_gadgetItems.Add(purchasableGadgetItem);
		purchasableGadgetItem = new PurchasableGadgetItem(LocalTextManager.GetStoreItemText("_ELECTRIC_CANNON_NAME_"), UIHelper.WordWrap(LocalTextManager.GetStoreItemText("_ELECTRIC_CANNON_DESC_"), 28), false, 10000, 0, 0f, 0, 0.2f, 1, storeItemText, "ElectricGunIcon.png", "20126");
		purchasableGadgetItem.m_buyExtraText = string.Empty;
		purchasableGadgetItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtElectricWeapon;
		m_gadgetItems.Add(purchasableGadgetItem);
		for (int i = 0; i < m_gadgetItems.Count; i++)
		{
			PurchasableGadgetItem pgi = (PurchasableGadgetItem)m_gadgetItems[i];
			PerryiCloudManager.The.UpdatePurchasableGadgetItemDataFromPrefs(pgi);
		}
	}

	public static int GetGadgetCount()
	{
		return m_gadgetItems.Count;
	}

	public static PurchasableGadgetItem GetGadgetItem(int index)
	{
		return (PurchasableGadgetItem)m_gadgetItems[index];
	}

	public static PurchasableGadgetItem GetGadgetItem(PurchasableGadgetItem p)
	{
		if (m_gadgetItems == null)
		{
			return null;
		}
		for (int i = 0; i < m_gadgetItems.Count; i++)
		{
			if (p.UID == ((PurchasableGadgetItem)m_gadgetItems[i]).UID)
			{
				return (PurchasableGadgetItem)m_gadgetItems[i];
			}
		}
		return null;
	}

	public static bool BuyGadgetItem(PurchasableGadgetItem gi)
	{
		if (!BuyVirtualItem(gi))
		{
			DebugManager.Log("didn't buy virtual gadget item");
			return false;
		}
		if (!gi.hasBoughtGadget)
		{
			gi.hasBoughtGadget = true;
		}
		else
		{
			gi.UpgradeNums++;
		}
		PerryiCloudManager.The.SetGadgetItem(gi.UID, gi);
		return true;
	}

	private static void InitAbilityUpgrades()
	{
		m_upgradeItems = new ArrayList();
		UpgradableItem upgradableItem = new UpgradableItem(LocalTextManager.GetStoreItemText("_BABY_HEAD_NAME_1_"), LocalTextManager.GetStoreItemText("_BABY_HEAD_DESC_1_"), false, 0, 50, 0f, 4, 2, 0, 0, "BabyHeadIcon.png", "50320");
		string[] upgradeTitles = new string[1] { LocalTextManager.GetStoreItemText("_BABY_HEAD_NAME_1_") };
		string[] upgradeDescriptions = new string[3]
		{
			LocalTextManager.GetStoreItemText("_BABY_HEAD_DESC_2_"),
			LocalTextManager.GetStoreItemText("_BABY_HEAD_DESC_3_"),
			LocalTextManager.GetStoreItemText("_BABY_HEAD_DESC_4_")
		};
		int[] fedoraPrices = new int[4] { 50, 25, 25, 25 };
		upgradableItem.SetUpgradeTitles(upgradeTitles);
		upgradableItem.SetUpgradeDescriptions(upgradeDescriptions);
		upgradableItem.SetUpgradeCosts(null, fedoraPrices, null);
		upgradableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtBabyHead;
		m_upgradeItems.Add(upgradableItem);
		upgradableItem = new UpgradableItem(LocalTextManager.GetStoreItemText("_DUCKY_MOMO_NAME_1_"), LocalTextManager.GetStoreItemText("_DUCKY_MOMO_DESC_1_"), false, 0, 50, 0f, 4, 2, 0, 0, "DuckyMomoIcon.png", "50319");
		string[] upgradeTitles2 = new string[1] { LocalTextManager.GetStoreItemText("_DUCKY_MOMO_NAME_1_") };
		string[] upgradeDescriptions2 = new string[3]
		{
			LocalTextManager.GetStoreItemText("_DUCKY_MOMO_DESC_2_"),
			LocalTextManager.GetStoreItemText("_DUCKY_MOMO_DESC_3_"),
			LocalTextManager.GetStoreItemText("_DUCKY_MOMO_DESC_4_")
		};
		int[] fedoraPrices2 = new int[4] { 50, 25, 25, 25 };
		upgradableItem.SetUpgradeTitles(upgradeTitles2);
		upgradableItem.SetUpgradeDescriptions(upgradeDescriptions2);
		upgradableItem.SetUpgradeCosts(null, fedoraPrices2, null);
		upgradableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtDuckyMomo;
		m_upgradeItems.Add(upgradableItem);
		upgradableItem = new UpgradableItem(LocalTextManager.GetStoreItemText("_EAGLE_UPGRADE_NAME_1_"), LocalTextManager.GetStoreItemText("_EAGLE_UPGRADE_DESC_1_"), false, 1250, 0, 0f, 6, 2, 0, 0, "FeatherIcon.png", "50313");
		string[] upgradeTitles3 = new string[1] { LocalTextManager.GetStoreItemText("_EAGLE_UPGRADE_NAME_1_") };
		string[] upgradeDescriptions3 = new string[6]
		{
			LocalTextManager.GetStoreItemText("_EAGLE_UPGRADE_DESC_2_"),
			LocalTextManager.GetStoreItemText("_EAGLE_UPGRADE_DESC_3_"),
			LocalTextManager.GetStoreItemText("_EAGLE_UPGRADE_DESC_4_"),
			LocalTextManager.GetStoreItemText("_EAGLE_UPGRADE_DESC_5_"),
			LocalTextManager.GetStoreItemText("_EAGLE_UPGRADE_DESC_6_"),
			LocalTextManager.GetStoreItemText("_EAGLE_UPGRADE_DESC_7_")
		};
		int[] tokenPrices = new int[6] { 500, 500, 1000, 5000, 15000, 30000 };
		upgradableItem.SetUpgradeTitles(upgradeTitles3);
		upgradableItem.SetUpgradeDescriptions(upgradeDescriptions3);
		upgradableItem.SetUpgradeCosts(tokenPrices, null, null);
		upgradableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtEagleUpgrade;
		m_upgradeItems.Add(upgradableItem);
		upgradableItem = new UpgradableItem(LocalTextManager.GetStoreItemText("_INVULNERABILITY_UPGRADE_NAME_1_"), LocalTextManager.GetStoreItemText("_INVULNERABILITY_UPGRADE_DESC_1_"), false, 1250, 0, 0f, 6, 2, 0, 0, "ShieldIcon.png", "50314");
		string[] upgradeTitles4 = new string[1] { LocalTextManager.GetStoreItemText("_INVULNERABILITY_UPGRADE_NAME_1_") };
		string[] upgradeDescriptions4 = new string[6]
		{
			LocalTextManager.GetStoreItemText("_INVULNERABILITY_UPGRADE_DESC_2_"),
			LocalTextManager.GetStoreItemText("_INVULNERABILITY_UPGRADE_DESC_3_"),
			LocalTextManager.GetStoreItemText("_INVULNERABILITY_UPGRADE_DESC_4_"),
			LocalTextManager.GetStoreItemText("_INVULNERABILITY_UPGRADE_DESC_5_"),
			LocalTextManager.GetStoreItemText("_INVULNERABILITY_UPGRADE_DESC_6_"),
			LocalTextManager.GetStoreItemText("_INVULNERABILITY_UPGRADE_DESC_7_")
		};
		int[] tokenPrices2 = new int[6] { 500, 500, 1000, 5000, 15000, 30000 };
		upgradableItem.SetUpgradeTitles(upgradeTitles4);
		upgradableItem.SetUpgradeDescriptions(upgradeDescriptions4);
		upgradableItem.SetUpgradeCosts(tokenPrices2, null, null);
		upgradableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtInvulnerabilityUpgrade;
		m_upgradeItems.Add(upgradableItem);
		upgradableItem = new UpgradableItem(LocalTextManager.GetStoreItemText("_MAGNETIZER_UPGRADE_NAME_1_"), LocalTextManager.GetStoreItemText("_MAGNETIZER_UPGRADE_DESC_1_"), false, 1250, 0, 0f, 6, 2, 0, 0, "MagnetizerUpgrade.png", "50315");
		string[] upgradeTitles5 = new string[1] { LocalTextManager.GetStoreItemText("_MAGNETIZER_UPGRADE_NAME_1_") };
		string[] upgradeDescriptions5 = new string[6]
		{
			LocalTextManager.GetStoreItemText("_MAGNETIZER_UPGRADE_DESC_2_"),
			LocalTextManager.GetStoreItemText("_MAGNETIZER_UPGRADE_DESC_3_"),
			LocalTextManager.GetStoreItemText("_MAGNETIZER_UPGRADE_DESC_4_"),
			LocalTextManager.GetStoreItemText("_MAGNETIZER_UPGRADE_DESC_5_"),
			LocalTextManager.GetStoreItemText("_MAGNETIZER_UPGRADE_DESC_6_"),
			LocalTextManager.GetStoreItemText("_MAGNETIZER_UPGRADE_DESC_7_")
		};
		int[] tokenPrices3 = new int[6] { 500, 500, 1000, 5000, 15000, 30000 };
		upgradableItem.SetUpgradeTitles(upgradeTitles5);
		upgradableItem.SetUpgradeDescriptions(upgradeDescriptions5);
		upgradableItem.SetUpgradeCosts(tokenPrices3, null, null);
		upgradableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtMagnetUpgrade;
		m_upgradeItems.Add(upgradableItem);
		upgradableItem = new UpgradableItem(LocalTextManager.GetStoreItemText("_SCORE_MULT_UPGRADE_NAME_1_"), LocalTextManager.GetStoreItemText("_SCORE_MULT_UPGRADE_DESC_1_"), false, 1250, 0, 0f, 6, 2, 0, 0, "ScoreMultiplier.png", "50316");
		string[] upgradeTitles6 = new string[1] { LocalTextManager.GetStoreItemText("_SCORE_MULT_UPGRADE_NAME_1_") };
		string[] upgradeDescriptions6 = new string[6]
		{
			LocalTextManager.GetStoreItemText("_SCORE_MULT_UPGRADE_DESC_2_"),
			LocalTextManager.GetStoreItemText("_SCORE_MULT_UPGRADE_DESC_3_"),
			LocalTextManager.GetStoreItemText("_SCORE_MULT_UPGRADE_DESC_4_"),
			LocalTextManager.GetStoreItemText("_SCORE_MULT_UPGRADE_DESC_5_"),
			LocalTextManager.GetStoreItemText("_SCORE_MULT_UPGRADE_DESC_6_"),
			LocalTextManager.GetStoreItemText("_SCORE_MULT_UPGRADE_DESC_7_")
		};
		int[] tokenPrices4 = new int[6] { 1000, 1000, 2500, 10000, 20000, 50000 };
		upgradableItem.SetUpgradeTitles(upgradeTitles6);
		upgradableItem.SetUpgradeDescriptions(upgradeDescriptions6);
		upgradableItem.SetUpgradeCosts(tokenPrices4, null, null);
		upgradableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtScoreMultiplierUpgrade;
		m_upgradeItems.Add(upgradableItem);
		upgradableItem = new UpgradableItem(LocalTextManager.GetStoreItemText("_CONTINUE_NAME_"), LocalTextManager.GetStoreItemText("_CONTINUE_DESC_1_"), false, 1250, 0, 0f, 6, 2, 0, 0, "OWCADropdownIcon.png", ms_ContinueCostReductionUID);
		string[] upgradeTitles7 = new string[1] { LocalTextManager.GetStoreItemText("_CONTINUE_NAME_") };
		string[] upgradeDescriptions7 = new string[6]
		{
			LocalTextManager.GetStoreItemText("_CONTINUE_DESC_2_"),
			LocalTextManager.GetStoreItemText("_CONTINUE_DESC_3_"),
			LocalTextManager.GetStoreItemText("_CONTINUE_DESC_4_"),
			LocalTextManager.GetStoreItemText("_CONTINUE_DESC_5_"),
			LocalTextManager.GetStoreItemText("_CONTINUE_DESC_6_"),
			LocalTextManager.GetStoreItemText("_CONTINUE_DESC_6_")
		};
		int[] tokenPrices5 = new int[6] { 1000, 1000, 2500, 10000, 20000, 50000 };
		upgradableItem.SetUpgradeTitles(upgradeTitles7);
		upgradableItem.SetUpgradeDescriptions(upgradeDescriptions7);
		upgradableItem.SetUpgradeCosts(tokenPrices5, null, null);
		upgradableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtContinueCost;
		m_upgradeItems.Add(upgradableItem);
		InitUpgradeStoreItems();
		for (int i = 0; i < m_upgradeItems.Count; i++)
		{
			upgradableItem = (UpgradableItem)m_upgradeItems[i];
			PerryiCloudManager.The.UpdateUpgradableItemDataFromPrefs(upgradableItem);
		}
	}

	private static void InitUpgradeStoreItems()
	{
		if (m_upgradeItems == null)
		{
			m_upgradeItems = new ArrayList();
		}
	}

	public static int GetUpgradeCount()
	{
		return m_upgradeItems.Count;
	}

	public static UpgradableItem GetUpgradableItem(int index)
	{
		return (UpgradableItem)m_upgradeItems[index];
	}

	public static UpgradableItem GetUpgradableItem(UpgradableItem item)
	{
		if (m_upgradeItems == null)
		{
			return null;
		}
		for (int i = 0; i < m_upgradeItems.Count; i++)
		{
			if (item.UID == ((UpgradableItem)m_upgradeItems[i]).UID)
			{
				return (UpgradableItem)m_upgradeItems[i];
			}
		}
		return null;
	}

	public static UpgradableItem FindUpgradableItem(string UID)
	{
		if (m_upgradeItems == null)
		{
			return null;
		}
		for (int i = 0; i < m_upgradeItems.Count; i++)
		{
			if (UID == ((UpgradableItem)m_upgradeItems[i]).UID)
			{
				return (UpgradableItem)m_upgradeItems[i];
			}
		}
		return null;
	}

	public static bool BuyUpgradeItem(UpgradableItem ui)
	{
		UpgradableItem upgradableItem = GetUpgradableItem(ui);
		if (!BuyVirtualItem(upgradableItem))
		{
			return false;
		}
		DebugManager.Log("Upgrades Owned: " + upgradableItem.upgradesOwned);
		upgradableItem.upgradesOwned++;
		PerryiCloudManager.The.SetUpgradableItem(upgradableItem.UID, upgradableItem);
		DebugManager.Log("Bought Upgrade Item: " + upgradableItem);
		return true;
	}

	private static void InitMiscItems()
	{
		m_jumpStart = new PurchasableItem(LocalTextManager.GetStoreItemText("_JUMP_START_NAME_"), LocalTextManager.GetStoreItemText("_JUMP_START_DESC_"), false, 2500, 0, 0f, false, 0, 0, "JumpstartIcon.png", "60566");
		m_jumpStart.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtJumpStart;
		PerryiCloudManager.The.UpdatePurchasableItemDataFromPrefs(m_jumpStart);
		SetJumpStartSpecialText(PlayerData.JumpStarts.ToString());
		m_miscUpgrades = new ArrayList();
		m_miscUpgrades.Add(m_jumpStart);
	}

	public static int GetMiscUpgradeItemsCount()
	{
		return m_miscUpgrades.Count;
	}

	public static PurchasableItem GetMiscUpgradeItem(int i)
	{
		if (i < m_miscUpgrades.Count)
		{
			return (PurchasableItem)m_miscUpgrades[i];
		}
		return null;
	}

	public static PurchasableItem GetMiscUpgradeItem(PurchasableItem item)
	{
		if (m_miscUpgrades == null)
		{
			return null;
		}
		for (int i = 0; i < m_miscUpgrades.Count; i++)
		{
			if (item.UID == ((PurchasableItem)m_miscUpgrades[i]).UID)
			{
				return (PurchasableItem)m_miscUpgrades[i];
			}
		}
		return null;
	}

	public static void SetJumpStartSpecialText(string text)
	{
		m_jumpStart.m_buyExtraText = LocalTextManager.GetStoreItemText("_OWNED_") + ": " + text;
	}

	public static bool BuyPurchasableItem(PurchasableItem pi)
	{
		PurchasableItem purchasableItem = GetMiscUpgradeItem(pi);
		bool flag = false;
		if (purchasableItem == null)
		{
			purchasableItem = GetTokenItem(pi);
			if (purchasableItem == null)
			{
				purchasableItem = GetCharacterItem(pi);
				if (purchasableItem == null)
				{
					DebugManager.Log("Can't find purchasable item");
					return false;
				}
				flag = true;
			}
		}
		if (!BuyVirtualItem(purchasableItem))
		{
			return false;
		}
		if (flag && GlobalGUIManager.The.CurrentMenuState == MainMenuEventManager.MenuState.Main_Menu_Character_Select)
		{
			GlobalGUIManager.The.ShowMainMenu();
		}
		PerryiCloudManager.The.SetItem(purchasableItem.UID, purchasableItem);
		DebugManager.Log("Bought Purchasable Item: " + purchasableItem);
		return true;
	}

	private static void InitGetTokensItems()
	{
		m_getTokensItems = new ArrayList();
		m_getHiddenTokensItems = new ArrayList();
		PurchasableItem purchasableItem = new PurchasableItem("_DUPLICATORINATOR_NAME_1_", "_DUPLICATORINATOR_DESC_1_", false, 0, 0, 0f, true, 0, 0, "CoinDuplicatorinator.png", "majesco.doofendash.upgrade_coin_duplicatorinator");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtCoinDuplicatorInator;
		m_getTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_STARTER_PACK_NAME_1_", "_STARTER_PACK_DESC_1_", false, 0, 0, 0f, false, 5000, 5, "StarterPackIcon.png", "majesco.doofendash.currency_starter_pack");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtCurrencyStarterPack;
		m_getTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_BUCKET_TOKENS_NAME_1_", "_BUCKET_TOKENS_DESC_1_", false, 0, 0, 0f, false, 5000, 0, "BucketTokenIcons.png", "majesco.doofendash.currency_bucket_of_tokens");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtBucketOfTokens;
		m_getTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_FISHBOWL_TOKENS_NAME_1_", "_FISHBOWL_TOKENS_DESC_1_", false, 0, 0, 0f, false, 30000, 0, "FishBowlTokenIcon.png", "majesco.doofendash.currency_fishbowl_full_of_tokens");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtFishbowlOfTokens;
		m_getTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_LAUNDRY_TOKENS_NAME_1_", "_LAUNDRY_TOKENS_DESC_1_", false, 0, 0, 0f, false, 100000, 0, "BasketTokenIcons.png", "majesco.doofendash.currency_laundry_bag_of_tokens");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtLaundryBagOfTokens;
		m_getTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_WHEELBARROW_TOKENS_NAME_1_", "_WHEELBARROW_TOKENS_DESC_1_", false, 0, 0, 0f, false, 250000, 0, "WheelbarrowTokenIcon.png", "majesco.doofendash.currency_wheelbarrow_of_tokens1");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtWheelbarrowOfTokens;
		m_getTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_WHALE-BELLY_TOKENS_NAME_1_", "_WHALE-BELLY_TOKENS_DESC_1_", false, 0, 0, 0f, false, 500000, 0, "WhaleTokenIcon.png", "majesco.doofendash.currency_whalebelly_full_of_tokens");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtWhaleBellyOfTokens;
		m_getTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_BUCKET_FEDORAS_NAME_1_", "_BUCKET_FEDORAS_DESC_1_", false, 0, 0, 0f, false, 0, 25, "BucketFedoraIcons.png", "majesco.doofendash.currency_bucket_of_fedoras");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtBucketOfFedoras;
		m_getTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_FISHBOWL_FEDORAS_NAME_1_", "_FISHBOWL_FEDORAS_DESC_1_", false, 0, 0, 0f, false, 0, 60, "FishBowlFedoraIcon.png", "majesco.doofendash.currency_fishbowl_full_of_fedoras");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtFishbowlOfFedoras;
		m_getTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_LAUNDRY_FEDORAS_NAME_1_", "_LAUNDRY_FEDORAS_DESC_1_", false, 0, 0, 0f, false, 0, 150, "BasketFedoraIcons.png", "majesco.doofendash.currency_laundry_bag_of_fedoras");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtLaundryBagOfFedoras;
		m_getTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_WHEELBARROW_FEDORAS_NAME_1_", "_WHEELBARROW_FEDORAS_DESC_1_", false, 0, 0, 0f, false, 0, 250, "WheelbarrowFedoraIcon.png", "majesco.doofendash.currency_wheelbarrow_of_fedoras");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtWheelbarrowOfFedoras;
		m_getTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_WHALE-BELLY_FEDORAS_NAME_1_", "_WHALE-BELLY_FEDORAS_DESC_1_", false, 0, 0, 0f, false, 0, 500, "WhaleFedoraIcon.png", "majesco.doofendash.currency_whalebelly_full_of_fedoras");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtWhaleBellyOfFedoras;
		m_getTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_FISHBOWL_TOKENS_NAME_1_", "_FISHBOWL_TOKENS_DESC_1_", false, 0, 0, 0f, false, 30000, 0, "FishBowlTokenIcon.png", "majesco.doofendash.currency_fishbowl_full_of_tokensx40");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtFishbowlOfTokens;
		m_getHiddenTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_LAUNDRY_TOKENS_NAME_1_", "_LAUNDRY_TOKENS_DESC_1_", false, 0, 0, 0f, false, 100000, 0, "BasketTokenIcons.png", "majesco.doofendash.currency_laundry_bag_of_tokensx40");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtLaundryBagOfTokens;
		m_getHiddenTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_WHEELBARROW_TOKENS_NAME_1_", "_WHEELBARROW_TOKENS_DESC_1_", false, 0, 0, 0f, false, 250000, 0, "WheelbarrowTokenIcon.png", "majesco.doofendash.currency_wheelbarrow_of_tokens1x40");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtWheelbarrowOfTokens;
		m_getHiddenTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_WHALE-BELLY_TOKENS_NAME_1_", "_WHALE-BELLY_TOKENS_DESC_1_", false, 0, 0, 0f, false, 500000, 0, "WhaleTokenIcon.png", "majesco.doofendash.currency_whalebelly_full_of_tokensx40");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtWhaleBellyOfTokens;
		m_getHiddenTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_WHEELBARROW_TOKENS_NAME_1_", "_WHEELBARROW_TOKENS_DESC_1_", false, 0, 0, 0f, false, 250000, 0, "WheelbarrowTokenIcon.png", "majesco.doofendash.currency_wheelbarrow_of_tokens1x70");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtWheelbarrowOfTokens;
		m_getHiddenTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_WHALE-BELLY_TOKENS_NAME_1_", "_WHALE-BELLY_TOKENS_DESC_1_", false, 0, 0, 0f, false, 500000, 0, "WhaleTokenIcon.png", "majesco.doofendash.currency_whalebelly_full_of_tokensx70");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtWhaleBellyOfTokens;
		m_getHiddenTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_BUCKET_FEDORAS_NAME_1_", "_BUCKET_FEDORAS_DESC_1_", false, 0, 0, 0f, false, 0, 25, "BucketFedoraIcons.png", "majesco.doofendash.currency_bucket_of_fedorasx40");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtBucketOfFedoras;
		m_getHiddenTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_FISHBOWL_FEDORAS_NAME_1_", "_FISHBOWL_FEDORAS_DESC_1_", false, 0, 0, 0f, false, 0, 60, "FishBowlFedoraIcon.png", "majesco.doofendash.currency_fishbowl_full_of_fedorasx40");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtFishbowlOfFedoras;
		m_getHiddenTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_LAUNDRY_FEDORAS_NAME_1_", "_LAUNDRY_FEDORAS_DESC_1_", false, 0, 0, 0f, false, 0, 150, "BasketFedoraIcons.png", "majesco.doofendash.currency_laundry_bag_of_fedorasx40");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtLaundryBagOfFedoras;
		m_getHiddenTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_WHEELBARROW_FEDORAS_NAME_1_", "_WHEELBARROW_FEDORAS_DESC_1_", false, 0, 0, 0f, false, 0, 250, "WheelbarrowFedoraIcon.png", "majesco.doofendash.currency_wheelbarrow_of_fedorasx40");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtWheelbarrowOfFedoras;
		m_getHiddenTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_WHALE-BELLY_FEDORAS_NAME_1_", "_WHALE-BELLY_FEDORAS_DESC_1_", false, 0, 0, 0f, false, 0, 500, "WhaleFedoraIcon.png", "majesco.doofendash.currency_whalebelly_full_of_fedorasx40");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtWhaleBellyOfFedoras;
		m_getHiddenTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_BUCKET_FEDORAS_NAME_1_", "_BUCKET_FEDORAS_DESC_1_", false, 0, 0, 0f, false, 0, 25, "BucketFedoraIcons.png", "majesco.doofendash.currency_bucket_of_fedorasx70");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtBucketOfFedoras;
		m_getHiddenTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_FISHBOWL_FEDORAS_NAME_1_", "_FISHBOWL_FEDORAS_DESC_1_", false, 0, 0, 0f, false, 0, 60, "FishBowlFedoraIcon.png", "majesco.doofendash.currency_fishbowl_full_of_fedorasx70");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtFishbowlOfFedoras;
		m_getHiddenTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_LAUNDRY_FEDORAS_NAME_1_", "_LAUNDRY_FEDORAS_DESC_1_", false, 0, 0, 0f, false, 0, 150, "BasketFedoraIcons.png", "majesco.doofendash.currency_laundry_bag_of_fedorasx70");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtLaundryBagOfFedoras;
		m_getHiddenTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_WHEELBARROW_FEDORAS_NAME_1_", "_WHEELBARROW_FEDORAS_DESC_1_", false, 0, 0, 0f, false, 0, 250, "WheelbarrowFedoraIcon.png", "majesco.doofendash.currency_wheelbarrow_of_fedorasx70");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtWheelbarrowOfFedoras;
		m_getHiddenTokensItems.Add(purchasableItem);
		purchasableItem = new PurchasableItem("_WHALE-BELLY_FEDORAS_NAME_1_", "_WHALE-BELLY_FEDORAS_DESC_1_", false, 0, 0, 0f, false, 0, 500, "WhaleFedoraIcon.png", "majesco.doofendash.currency_whalebelly_full_of_fedorasx70");
		purchasableItem.ImmediatePurchaseEffectCallback = AllItemEffects.BoughtWhaleBellyOfFedoras;
		m_getHiddenTokensItems.Add(purchasableItem);
		for (int i = 0; i < m_getTokensItems.Count; i++)
		{
			PurchasableItem purchasableItem2 = (PurchasableItem)m_getTokensItems[i];
			purchasableItem2.m_nameLocKey = purchasableItem2.m_name;
			purchasableItem2.m_descLocKey = purchasableItem2.m_desc;
			purchasableItem2.m_name = LocalTextManager.GetStoreItemText(purchasableItem2.m_nameLocKey);
			purchasableItem2.m_desc = LocalTextManager.GetStoreItemText(purchasableItem2.m_descLocKey);
			PerryiCloudManager.The.UpdatePurchasableItemDataFromPrefs(purchasableItem2);
		}
	}

	private static void SetTokensStoreItems()
	{
		DebugManager.Log("Entering AllItemData.SetTokensStoreItems()");
		DebugManager.Log("GetTokensStoreItems: m_getTokensItems count: " + m_getTokensItems.Count);
		for (int i = 0; i < m_getTokensItems.Count; i++)
		{
			PurchasableItem purchasableItem = (PurchasableItem)m_getTokensItems[i];
			IAPProduct product = StoreManager.The.GetProduct(purchasableItem.UID);
			if (product != null)
			{
				string text = Regex.Replace(product.price, "[^.,0-9]", string.Empty);
				float num = 0f;
				try
				{
					num = float.Parse(text, CultureInfo.InvariantCulture);
				}
				catch (Exception)
				{
					num = GetIntFromString(text);
				}
				purchasableItem.m_name = product.title;
				purchasableItem.m_desc = product.description;
				DebugManager.Log("Item Price = " + num);
				purchasableItem.m_realMoneyCost = num;
				purchasableItem.m_gotFromStore = true;
				m_gotTokenItemsFromStore = true;
			}
		}
		DebugManager.Log("Exiting AllItemData.SetTokensStoreItems()");
	}

	public static int GetIntFromString(string input)
	{
		try
		{
			string text = string.Empty;
			foreach (char c in input)
			{
				if (c - 48 < 10 && c - 48 >= 0)
				{
					text += c;
				}
			}
			return Convert.ToInt32(text);
		}
		catch (FormatException)
		{
			return 0;
		}
	}

	public static bool AreTokenItemsFromStore()
	{
		return m_gotTokenItemsFromStore;
	}

	public static int GetTokenItemsCount()
	{
		return m_getTokensItems.Count;
	}

	public static PurchasableItem GetTokenItemClosestToPrice(int tokenCost)
	{
		PurchasableItem purchasableItem = null;
		int num = tokenCost - PlayerData.playerTokens;
		for (int i = 0; i < m_getTokensItems.Count; i++)
		{
			if (((PurchasableItem)m_getTokensItems[i]).m_tokenReward <= num)
			{
				continue;
			}
			if (purchasableItem != null)
			{
				if (purchasableItem.m_realMoneyCost > ((PurchasableItem)m_getTokensItems[i]).m_realMoneyCost)
				{
					purchasableItem = (PurchasableItem)m_getTokensItems[i];
				}
			}
			else
			{
				purchasableItem = (PurchasableItem)m_getTokensItems[i];
			}
		}
		return purchasableItem;
	}

	public static PurchasableItem GetTokenItemByProductId(string productIdentifier)
	{
		PurchasableItem purchasableItem = null;
		for (int i = 0; i < m_getHiddenTokensItems.Count; i++)
		{
			if (((PurchasableItem)m_getHiddenTokensItems[i]).UID == productIdentifier)
			{
				purchasableItem = (PurchasableItem)m_getHiddenTokensItems[i];
				break;
			}
		}
		if (purchasableItem == null)
		{
			DebugManager.Log("Did not find that product ID, search the second list");
			for (int j = 0; j < m_getTokensItems.Count; j++)
			{
				DebugManager.Log("Finding prodID " + productIdentifier + " against " + ((PurchasableItem)m_getTokensItems[j]).UID);
				if (((PurchasableItem)m_getTokensItems[j]).UID == productIdentifier)
				{
					DebugManager.Log("Found it");
					purchasableItem = (PurchasableItem)m_getTokensItems[j];
					break;
				}
			}
		}
		if (purchasableItem == null)
		{
			purchasableItem = (PurchasableItem)m_getHiddenTokensItems[0];
		}
		return purchasableItem;
	}

	public static PurchasableItem GetTokenItemClosestToFedoraPrice(int m_fedoraCost)
	{
		PurchasableItem purchasableItem = null;
		for (int i = 0; i < m_getTokensItems.Count; i++)
		{
			if (((PurchasableItem)m_getTokensItems[i]).m_fedoraReward <= m_fedoraCost)
			{
				continue;
			}
			if (purchasableItem != null)
			{
				if (purchasableItem.m_realMoneyCost > ((PurchasableItem)m_getTokensItems[i]).m_realMoneyCost)
				{
					purchasableItem = (PurchasableItem)m_getTokensItems[i];
				}
			}
			else
			{
				purchasableItem = (PurchasableItem)m_getTokensItems[i];
			}
		}
		return purchasableItem;
	}

	public static PurchasableItem GetTokenItem(int index)
	{
		return (PurchasableItem)m_getTokensItems[index];
	}

	public static PurchasableItem GetTokenItem(PurchasableItem item)
	{
		if (m_getTokensItems == null)
		{
			return null;
		}
		for (int i = 0; i < m_getTokensItems.Count; i++)
		{
			if (item.UID == ((PurchasableItem)m_getTokensItems[i]).UID)
			{
				return (PurchasableItem)m_getTokensItems[i];
			}
		}
		return null;
	}

	public static bool BuyTokenItem(PurchasableItem pi)
	{
		if (!BuyVirtualItem(pi))
		{
			return false;
		}
		return true;
	}

	private static void SetPurchasableStoreItem(string itemID, ref ArrayList items)
	{
	}

	public static void BuyRealMoneyItem(bool dummy, PurchasableItem pi, Action<bool> buyItemCallback)
	{
		m_realMoneyItems.Add(pi);
		DebugManager.Log("Calling StoreManager to Buy: " + pi.UID + " and isConsumable: " + !pi.m_isBuyOnce);
		StoreManager.The.BuyProduct(pi.UID, !pi.m_isBuyOnce, buyItemCallback, dummy);
	}

	public static void BuyRealMoneyItem(PurchasableItem pi, StoreManager.BuyItemCallback callback = null)
	{
		m_realMoneyItems.Add(pi);
		DebugManager.Log("Calling StoreManager to Buy: " + pi.UID + " and isConsumable: " + !pi.m_isBuyOnce);
		StoreManager.The.BuyProduct(pi.UID, !pi.m_isBuyOnce, callback);
	}

	private static bool BuyVirtualItem(PurchasableItem pi)
	{
		if (PlayerData.playerTokens < pi.m_tokenCost)
		{
			return false;
		}
		if (PlayerData.playerFedoras < pi.m_fedoraCost)
		{
			return false;
		}
		PlayerData.playerTokens -= pi.m_tokenCost;
		PlayerData.playerFedoras -= pi.m_fedoraCost;
		SetBuyItemData(pi);
		return true;
	}

	private static void SetBuyItemData(PurchasableItem pi)
	{
		pi.Purchase(1);
		DebugManager.Log("Updated " + pi.m_name + " item to owned");
	}

	public static void FinishBuyingStoreProduct(string productID)
	{
		PurchasableItem purchasableItem = null;
		object obj = null;
		DebugManager.Log("m_realMoneyItems == null?" + (m_realMoneyItems == null));
		for (int i = 0; i < m_realMoneyItems.Count; i++)
		{
			obj = m_realMoneyItems[i];
			purchasableItem = (PurchasableItem)obj;
			if (purchasableItem.UID == productID)
			{
				m_realMoneyItems.RemoveAt(i);
				i = m_realMoneyItems.Count;
			}
			else
			{
				purchasableItem = null;
			}
		}
		if (purchasableItem == null)
		{
			DebugManager.Log("Can't buy store product: " + productID);
		}
		if (obj.GetType() == typeof(PurchasableItem))
		{
			BuyTokenItem((PurchasableItem)obj);
		}
		else if (obj.GetType() == typeof(PurchasableGadgetItem))
		{
			BuyGadgetItem((PurchasableGadgetItem)obj);
		}
		else if (obj.GetType() == typeof(UpgradableItem))
		{
			BuyUpgradeItem((UpgradableItem)obj);
		}
	}

	public static void ReloadAllText()
	{
		ReloadAllGadgetText();
		ReloadAllUpgradeText();
		ReloadAllMiscText();
		ReloadAllCharacterItems();
	}

	public static void ReloadAllGadgetText()
	{
		if (m_gadgetItems != null)
		{
			((PurchasableGadgetItem)m_gadgetItems[0]).m_name = LocalTextManager.GetStoreItemText("_WATER_CANNON_NAME_");
			((PurchasableGadgetItem)m_gadgetItems[0]).m_desc = UIHelper.WordWrap(LocalTextManager.GetStoreItemText("_WATER_CANNON_DESC_"), 28);
			((PurchasableGadgetItem)m_gadgetItems[0]).m_upgradeExtraText = LocalTextManager.GetStoreItemText("_POWER_UP_");
			((PurchasableGadgetItem)m_gadgetItems[0]).m_buyExtraText = string.Empty;
			((PurchasableGadgetItem)m_gadgetItems[1]).m_name = LocalTextManager.GetStoreItemText("_TORCH_CANNON_NAME_");
			((PurchasableGadgetItem)m_gadgetItems[1]).m_desc = UIHelper.WordWrap(LocalTextManager.GetStoreItemText("_TORCH_CANNON_DESC_"), 28);
			((PurchasableGadgetItem)m_gadgetItems[1]).m_upgradeExtraText = LocalTextManager.GetStoreItemText("_POWER_UP_");
			((PurchasableGadgetItem)m_gadgetItems[1]).m_buyExtraText = string.Empty;
			((PurchasableGadgetItem)m_gadgetItems[2]).m_name = LocalTextManager.GetStoreItemText("_ELECTRIC_CANNON_NAME_");
			((PurchasableGadgetItem)m_gadgetItems[2]).m_desc = UIHelper.WordWrap(LocalTextManager.GetStoreItemText("_ELECTRIC_CANNON_DESC_"), 28);
			((PurchasableGadgetItem)m_gadgetItems[2]).m_upgradeExtraText = LocalTextManager.GetStoreItemText("_POWER_UP_");
			((PurchasableGadgetItem)m_gadgetItems[2]).m_buyExtraText = string.Empty;
		}
	}

	public static void ReloadAllUpgradeText()
	{
		if (m_upgradeItems != null)
		{
			((UpgradableItem)m_upgradeItems[0]).m_name = LocalTextManager.GetStoreItemText("_BABY_HEAD_NAME_1_");
			((UpgradableItem)m_upgradeItems[0]).m_desc = LocalTextManager.GetStoreItemText("_BABY_HEAD_DESC_1_");
			string[] upgradeTitles = new string[1] { LocalTextManager.GetStoreItemText("_BABY_HEAD_NAME_1_") };
			string[] upgradeDescriptions = new string[3]
			{
				LocalTextManager.GetStoreItemText("_BABY_HEAD_DESC_2_"),
				LocalTextManager.GetStoreItemText("_BABY_HEAD_DESC_3_"),
				LocalTextManager.GetStoreItemText("_BABY_HEAD_DESC_4_")
			};
			((UpgradableItem)m_upgradeItems[0]).SetUpgradeTitles(upgradeTitles);
			((UpgradableItem)m_upgradeItems[0]).SetUpgradeDescriptions(upgradeDescriptions);
			((UpgradableItem)m_upgradeItems[1]).m_name = LocalTextManager.GetStoreItemText("_DUCKY_MOMO_NAME_1_");
			((UpgradableItem)m_upgradeItems[1]).m_desc = LocalTextManager.GetStoreItemText("_DUCKY_MOMO_DESC_1_");
			string[] upgradeTitles2 = new string[1] { LocalTextManager.GetStoreItemText("_DUCKY_MOMO_NAME_1_") };
			string[] upgradeDescriptions2 = new string[3]
			{
				LocalTextManager.GetStoreItemText("_DUCKY_MOMO_DESC_2_"),
				LocalTextManager.GetStoreItemText("_DUCKY_MOMO_DESC_3_"),
				LocalTextManager.GetStoreItemText("_DUCKY_MOMO_DESC_4_")
			};
			((UpgradableItem)m_upgradeItems[1]).SetUpgradeTitles(upgradeTitles2);
			((UpgradableItem)m_upgradeItems[1]).SetUpgradeDescriptions(upgradeDescriptions2);
			((UpgradableItem)m_upgradeItems[2]).m_name = LocalTextManager.GetStoreItemText("_EAGLE_UPGRADE_NAME_1_");
			((UpgradableItem)m_upgradeItems[2]).m_desc = LocalTextManager.GetStoreItemText("_EAGLE_UPGRADE_DESC_1_");
			string[] upgradeTitles3 = new string[1] { LocalTextManager.GetStoreItemText("_EAGLE_UPGRADE_NAME_1_") };
			string[] upgradeDescriptions3 = new string[6]
			{
				LocalTextManager.GetStoreItemText("_EAGLE_UPGRADE_DESC_2_"),
				LocalTextManager.GetStoreItemText("_EAGLE_UPGRADE_DESC_3_"),
				LocalTextManager.GetStoreItemText("_EAGLE_UPGRADE_DESC_4_"),
				LocalTextManager.GetStoreItemText("_EAGLE_UPGRADE_DESC_5_"),
				LocalTextManager.GetStoreItemText("_EAGLE_UPGRADE_DESC_6_"),
				LocalTextManager.GetStoreItemText("_EAGLE_UPGRADE_DESC_7_")
			};
			((UpgradableItem)m_upgradeItems[2]).SetUpgradeTitles(upgradeTitles3);
			((UpgradableItem)m_upgradeItems[2]).SetUpgradeDescriptions(upgradeDescriptions3);
			((UpgradableItem)m_upgradeItems[3]).m_name = LocalTextManager.GetStoreItemText("_INVULNERABILITY_UPGRADE_NAME_1_");
			((UpgradableItem)m_upgradeItems[3]).m_desc = LocalTextManager.GetStoreItemText("_INVULNERABILITY_UPGRADE_DESC_1_");
			string[] upgradeTitles4 = new string[1] { LocalTextManager.GetStoreItemText("_INVULNERABILITY_UPGRADE_NAME_1_") };
			string[] upgradeDescriptions4 = new string[6]
			{
				LocalTextManager.GetStoreItemText("_INVULNERABILITY_UPGRADE_DESC_2_"),
				LocalTextManager.GetStoreItemText("_INVULNERABILITY_UPGRADE_DESC_3_"),
				LocalTextManager.GetStoreItemText("_INVULNERABILITY_UPGRADE_DESC_4_"),
				LocalTextManager.GetStoreItemText("_INVULNERABILITY_UPGRADE_DESC_5_"),
				LocalTextManager.GetStoreItemText("_INVULNERABILITY_UPGRADE_DESC_6_"),
				LocalTextManager.GetStoreItemText("_INVULNERABILITY_UPGRADE_DESC_7_")
			};
			((UpgradableItem)m_upgradeItems[3]).SetUpgradeTitles(upgradeTitles4);
			((UpgradableItem)m_upgradeItems[3]).SetUpgradeDescriptions(upgradeDescriptions4);
			((UpgradableItem)m_upgradeItems[4]).m_name = LocalTextManager.GetStoreItemText("_MAGNETIZER_UPGRADE_NAME_1_");
			((UpgradableItem)m_upgradeItems[4]).m_desc = LocalTextManager.GetStoreItemText("_MAGNETIZER_UPGRADE_DESC_1_");
			string[] upgradeTitles5 = new string[1] { LocalTextManager.GetStoreItemText("_MAGNETIZER_UPGRADE_NAME_1_") };
			string[] upgradeDescriptions5 = new string[6]
			{
				LocalTextManager.GetStoreItemText("_MAGNETIZER_UPGRADE_DESC_2_"),
				LocalTextManager.GetStoreItemText("_MAGNETIZER_UPGRADE_DESC_3_"),
				LocalTextManager.GetStoreItemText("_MAGNETIZER_UPGRADE_DESC_4_"),
				LocalTextManager.GetStoreItemText("_MAGNETIZER_UPGRADE_DESC_5_"),
				LocalTextManager.GetStoreItemText("_MAGNETIZER_UPGRADE_DESC_6_"),
				LocalTextManager.GetStoreItemText("_MAGNETIZER_UPGRADE_DESC_7_")
			};
			((UpgradableItem)m_upgradeItems[4]).SetUpgradeTitles(upgradeTitles5);
			((UpgradableItem)m_upgradeItems[4]).SetUpgradeDescriptions(upgradeDescriptions5);
			((UpgradableItem)m_upgradeItems[5]).m_name = LocalTextManager.GetStoreItemText("_SCORE_MULT_UPGRADE_NAME_1_");
			((UpgradableItem)m_upgradeItems[5]).m_desc = LocalTextManager.GetStoreItemText("_SCORE_MULT_UPGRADE_DESC_1_");
			string[] upgradeTitles6 = new string[1] { LocalTextManager.GetStoreItemText("_SCORE_MULT_UPGRADE_NAME_1_") };
			string[] upgradeDescriptions6 = new string[6]
			{
				LocalTextManager.GetStoreItemText("_SCORE_MULT_UPGRADE_DESC_2_"),
				LocalTextManager.GetStoreItemText("_SCORE_MULT_UPGRADE_DESC_3_"),
				LocalTextManager.GetStoreItemText("_SCORE_MULT_UPGRADE_DESC_4_"),
				LocalTextManager.GetStoreItemText("_SCORE_MULT_UPGRADE_DESC_5_"),
				LocalTextManager.GetStoreItemText("_SCORE_MULT_UPGRADE_DESC_6_"),
				LocalTextManager.GetStoreItemText("_SCORE_MULT_UPGRADE_DESC_7_")
			};
			((UpgradableItem)m_upgradeItems[5]).SetUpgradeTitles(upgradeTitles6);
			((UpgradableItem)m_upgradeItems[5]).SetUpgradeDescriptions(upgradeDescriptions6);
			((UpgradableItem)m_upgradeItems[6]).m_name = LocalTextManager.GetStoreItemText("_CONTINUE_NAME_");
			((UpgradableItem)m_upgradeItems[6]).m_desc = LocalTextManager.GetStoreItemText("_CONTINUE_DESC_1_");
			string[] upgradeTitles7 = new string[1] { LocalTextManager.GetStoreItemText("_CONTINUE_NAME_") };
			string[] upgradeDescriptions7 = new string[6]
			{
				LocalTextManager.GetStoreItemText("_CONTINUE_DESC_2_"),
				LocalTextManager.GetStoreItemText("_CONTINUE_DESC_3_"),
				LocalTextManager.GetStoreItemText("_CONTINUE_DESC_4_"),
				LocalTextManager.GetStoreItemText("_CONTINUE_DESC_5_"),
				LocalTextManager.GetStoreItemText("_CONTINUE_DESC_6_"),
				LocalTextManager.GetStoreItemText("_CONTINUE_DESC_7_")
			};
			((UpgradableItem)m_upgradeItems[6]).SetUpgradeTitles(upgradeTitles7);
			((UpgradableItem)m_upgradeItems[6]).SetUpgradeDescriptions(upgradeDescriptions7);
		}
	}

	public static void ReloadAllMiscText()
	{
		if (m_miscUpgrades != null)
		{
			((PurchasableItem)m_miscUpgrades[0]).m_name = LocalTextManager.GetStoreItemText("_JUMP_START_NAME_");
			((PurchasableItem)m_miscUpgrades[0]).m_desc = LocalTextManager.GetStoreItemText("_JUMP_START_DESC_");
			SetJumpStartSpecialText(PlayerData.JumpStarts.ToString());
		}
	}

	public static void ReloadAllCharacterItems()
	{
		if (m_characterItems != null)
		{
			((PurchasableItem)m_characterItems[0]).m_name = LocalTextManager.GetStoreItemText("_AGENT_P_");
			((PurchasableItem)m_characterItems[1]).m_name = LocalTextManager.GetStoreItemText("_AGENT_PINKY_");
			((PurchasableItem)m_characterItems[2]).m_name = LocalTextManager.GetStoreItemText("_AGENT_PETER_");
			((PurchasableItem)m_characterItems[3]).m_name = LocalTextManager.GetStoreItemText("_AGENT_T_");
			((PurchasableItem)m_characterItems[4]).m_name = LocalTextManager.GetStoreItemText("_COCONUT_PERRY_");
			((PurchasableItem)m_characterItems[5]).m_name = LocalTextManager.GetStoreItemText("_PRINCESS_PINKY_");
			((PurchasableItem)m_characterItems[6]).m_name = LocalTextManager.GetStoreItemText("_GENTLEMAN_PANDA_");
			((PurchasableItem)m_characterItems[7]).m_name = LocalTextManager.GetStoreItemText("_VIKING_TERRY_");
			((PurchasableItem)m_characterItems[8]).m_name = LocalTextManager.GetStoreItemText("_SUPER_AGENT_P_");
			((PurchasableItem)m_characterItems[9]).m_name = LocalTextManager.GetStoreItemText("_FUNKY_PERRY_");
		}
	}
}

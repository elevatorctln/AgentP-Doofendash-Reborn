using System.Collections.Generic;
using UnityEngine;

public static class AllItemEffects
{
	public static void BoughtCoinDuplicatorInator(object item = null)
	{
		PlayerData.coinDuplicatorInator = true;
		Debug.Log("BoughtCoinDuplicatorInator - PlayerData.coinDuplicatorInator = " + PlayerData.coinDuplicatorInator);
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Coin Duplicatorinator");
		FlurryFacade.Instance.LogEvent("HardPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtCurrencyStarterPack(object item = null)
	{
		Debug.Log("Adding 5000 tokens and 5 fedoras");
		PlayerData.playerTokens += 5000;
		PlayerData.playerFedoras += 5;
		PlayerData.AllTimeTokens += 5000;
		PlayerData.AllTimeFedoras += 5;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Starter Pack");
		dictionary.Add("Fedoras", "5");
		dictionary.Add("Tokens", "5000");
		FlurryFacade.Instance.LogEvent("HardPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtCurrencyValuePack(object item = null)
	{
		Debug.Log("Adding 20,000 tokens and 15 fedoras");
		PlayerData.playerTokens += 20000;
		PlayerData.playerFedoras += 15;
		PlayerData.AllTimeTokens += 20000;
		PlayerData.AllTimeFedoras += 15;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Value Pack");
		dictionary.Add("Fedoras", "15");
		dictionary.Add("Tokens", "20000");
		FlurryFacade.Instance.LogEvent("HardPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtBucketOfTokens(object item = null)
	{
		Debug.Log("Adding 5000 tokens");
		PlayerData.playerTokens += 5000;
		PlayerData.AllTimeTokens += 5000;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Bucket of Tokens");
		dictionary.Add("Fedoras", "5000");
		FlurryFacade.Instance.LogEvent("HardPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtBucketOfFedoras(object item = null)
	{
		Debug.Log("Adding 25 fedoras");
		PlayerData.playerFedoras += 25;
		PlayerData.AllTimeFedoras += 25;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Bucket of Fedoras");
		dictionary.Add("Fedoras", "25");
		FlurryFacade.Instance.LogEvent("HardPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtFishbowlOfTokens(object item = null)
	{
		Debug.Log("Adding 30,000 Tokens");
		PlayerData.playerTokens += 30000;
		PlayerData.AllTimeTokens += 30000;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Fishbowl of Tokens");
		dictionary.Add("Tokens", "30000");
		FlurryFacade.Instance.LogEvent("HardPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtFishbowlOfFedoras(object item = null)
	{
		Debug.Log("Adding 60 Fedoras");
		PlayerData.playerFedoras += 60;
		PlayerData.AllTimeFedoras += 60;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Fishbowl of Fedoras");
		dictionary.Add("Fedoras", "60");
		FlurryFacade.Instance.LogEvent("HardPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtLaundryBagOfTokens(object item = null)
	{
		Debug.Log("Adding 100,000 Tokens");
		PlayerData.playerTokens += 100000;
		PlayerData.AllTimeTokens += 100000;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Laundry Bag of Tokens");
		dictionary.Add("Tokens", "100000");
		FlurryFacade.Instance.LogEvent("HardPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtLaundryBagOfFedoras(object item = null)
	{
		Debug.Log("Adding 150 Fedoras");
		PlayerData.playerFedoras += 150;
		PlayerData.AllTimeFedoras += 150;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Laundry Bag of Fedoras");
		dictionary.Add("Fedoras", "150");
		FlurryFacade.Instance.LogEvent("HardPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtWheelbarrowOfTokens(object item = null)
	{
		Debug.Log("Adding 250,000 Tokens");
		PlayerData.playerTokens += 250000;
		PlayerData.AllTimeTokens += 250000;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Wheelbarrow");
		dictionary.Add("Tokens", "250000");
		FlurryFacade.Instance.LogEvent("HardPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtWheelbarrowOfFedoras(object item = null)
	{
		Debug.Log("Adding 250 Fedoras");
		PlayerData.playerFedoras += 250;
		PlayerData.AllTimeFedoras += 250;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Wheelbarrow of Fedoras");
		dictionary.Add("Fedoras", "250");
		FlurryFacade.Instance.LogEvent("HardPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtWhaleBellyOfTokens(object item = null)
	{
		Debug.Log("Adding 500,000 Tokens");
		PlayerData.playerTokens += 500000;
		PlayerData.AllTimeTokens += 500000;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Whale-belly of Tokens");
		dictionary.Add("Tokens", "500000");
		FlurryFacade.Instance.LogEvent("HardPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtWhaleBellyOfFedoras(object item = null)
	{
		Debug.Log("Adding 500 Fedoras");
		PlayerData.playerFedoras += 500;
		PlayerData.AllTimeFedoras += 500;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Whale-belly of Fedoras");
		dictionary.Add("Fedoras", "500");
		FlurryFacade.Instance.LogEvent("HardPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtVaultOfTokens(object item = null)
	{
		Debug.Log("Adding 500,000 Tokens");
		PlayerData.playerTokens += 500000;
		PlayerData.AllTimeTokens += 500000;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Vault of Tokens");
		dictionary.Add("Tokens", "500000");
		FlurryFacade.Instance.LogEvent("HardPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtVaultOfFedoras(object item = null)
	{
		Debug.Log("Adding 1000 Fedoras");
		PlayerData.playerFedoras += 1000;
		PlayerData.AllTimeFedoras += 1000;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Vault of Fedoras");
		dictionary.Add("Fedoras", "1000");
		FlurryFacade.Instance.LogEvent("HardPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtWaterGadget(object item = null)
	{
		Debug.Log("Adding Water Gadget");
		if (!PlayerData.hasWaterWeapon)
		{
			PlayerData.hasWaterWeapon = true;
		}
		else
		{
			PlayerData.WaterWeaponUpgrades++;
		}
		PurchasableGadgetItem purchasableGadgetItem = (PurchasableGadgetItem)item;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Water Cannon");
		dictionary.Add("TokenCost", purchasableGadgetItem.m_tokenCost.ToString());
		dictionary.Add("FedoraCost", purchasableGadgetItem.m_fedoraCost.ToString());
		dictionary.Add("NumberOwned", purchasableGadgetItem.m_numOwned.ToString());
		FlurryFacade.Instance.LogEvent("SoftPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtFireGadget(object item = null)
	{
		Debug.Log("Adding Fire Gadget");
		if (!PlayerData.hasFireWeapon)
		{
			PlayerData.hasFireWeapon = true;
		}
		else
		{
			PlayerData.FireWeaponUpgrades++;
		}
		PurchasableGadgetItem purchasableGadgetItem = (PurchasableGadgetItem)item;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Torch Cannon");
		dictionary.Add("TokenCost", purchasableGadgetItem.m_tokenCost.ToString());
		dictionary.Add("FedoraCost", purchasableGadgetItem.m_fedoraCost.ToString());
		dictionary.Add("NumberOwned", purchasableGadgetItem.m_numOwned.ToString());
		FlurryFacade.Instance.LogEvent("SoftPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtElectricWeapon(object item = null)
	{
		if (!PlayerData.hasElectricWeapon)
		{
			PlayerData.hasElectricWeapon = true;
		}
		else
		{
			PlayerData.ElectricWeaponUpgrades++;
		}
		PurchasableGadgetItem purchasableGadgetItem = (PurchasableGadgetItem)item;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Electric Cannon");
		dictionary.Add("TokenCost", purchasableGadgetItem.m_tokenCost.ToString());
		dictionary.Add("FedoraCost", purchasableGadgetItem.m_fedoraCost.ToString());
		dictionary.Add("NumberOwned", purchasableGadgetItem.m_numOwned.ToString());
		FlurryFacade.Instance.LogEvent("SoftPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtPinShooter(object item = null)
	{
		if (!PlayerData.HasPinWeapon)
		{
			PlayerData.HasPinWeapon = true;
		}
		PurchasableGadgetItem purchasableGadgetItem = (PurchasableGadgetItem)item;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Pin Shooter");
		dictionary.Add("TokenCost", purchasableGadgetItem.m_tokenCost.ToString());
		dictionary.Add("FedoraCost", purchasableGadgetItem.m_fedoraCost.ToString());
		dictionary.Add("NumberOwned", purchasableGadgetItem.m_numOwned.ToString());
		FlurryFacade.Instance.LogEvent("SoftPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtEagleUpgrade(object item = null)
	{
		UpgradableItem upgradableItem = (UpgradableItem)item;
		if (upgradableItem.m_numOwned == 1)
		{
			PlayerData.EagleUpgradeTime = PlayerData.EAGLE_UPGRADE_DURATIONS[0];
		}
		else if (upgradableItem.m_numOwned == 2)
		{
			PlayerData.EagleUpgradeTime = PlayerData.EAGLE_UPGRADE_DURATIONS[1];
		}
		else if (upgradableItem.m_numOwned == 3)
		{
			PlayerData.EagleUpgradeTime = PlayerData.EAGLE_UPGRADE_DURATIONS[2];
		}
		else if (upgradableItem.m_numOwned == 4)
		{
			PlayerData.EagleUpgradeTime = PlayerData.EAGLE_UPGRADE_DURATIONS[3];
		}
		else if (upgradableItem.m_numOwned == 5)
		{
			PlayerData.EagleUpgradeTime = PlayerData.EAGLE_UPGRADE_DURATIONS[4];
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Eagle Upgrade");
		dictionary.Add("TokenCost", upgradableItem.m_tokenCost.ToString());
		dictionary.Add("FedoraCost", upgradableItem.m_fedoraCost.ToString());
		dictionary.Add("NumberOwned", upgradableItem.m_numOwned.ToString());
		FlurryFacade.Instance.LogEvent("SoftPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtInvulnerabilityUpgrade(object item = null)
	{
		UpgradableItem upgradableItem = (UpgradableItem)item;
		if (upgradableItem.m_numOwned == 1)
		{
			PlayerData.InvulnerabilityUpgradeTime = PlayerData.INVINCIBILITY_UPGRADE_DURATIONS[0];
		}
		else if (upgradableItem.m_numOwned == 2)
		{
			PlayerData.InvulnerabilityUpgradeTime = PlayerData.INVINCIBILITY_UPGRADE_DURATIONS[1];
		}
		else if (upgradableItem.m_numOwned == 3)
		{
			PlayerData.InvulnerabilityUpgradeTime = PlayerData.INVINCIBILITY_UPGRADE_DURATIONS[2];
		}
		else if (upgradableItem.m_numOwned == 4)
		{
			PlayerData.InvulnerabilityUpgradeTime = PlayerData.INVINCIBILITY_UPGRADE_DURATIONS[3];
		}
		else if (upgradableItem.m_numOwned == 5)
		{
			PlayerData.InvulnerabilityUpgradeTime = PlayerData.INVINCIBILITY_UPGRADE_DURATIONS[4];
		}
		else if (upgradableItem.m_numOwned == 6)
		{
			PlayerData.InvulnerabilityUpgradeTime = PlayerData.INVINCIBILITY_UPGRADE_DURATIONS[5];
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Invulnerability Upgrade");
		dictionary.Add("TokenCost", upgradableItem.m_tokenCost.ToString());
		dictionary.Add("FedoraCost", upgradableItem.m_fedoraCost.ToString());
		dictionary.Add("NumberOwned", upgradableItem.m_numOwned.ToString());
		FlurryFacade.Instance.LogEvent("SoftPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtMagnetUpgrade(object item = null)
	{
		UpgradableItem upgradableItem = (UpgradableItem)item;
		if (upgradableItem.m_numOwned == 1)
		{
			PlayerData.MagnetUpgradeTime = PlayerData.MAGNETIZER_UPGRADE_DURATIONS[0];
		}
		else if (upgradableItem.m_numOwned == 2)
		{
			PlayerData.MagnetUpgradeTime = PlayerData.MAGNETIZER_UPGRADE_DURATIONS[1];
		}
		else if (upgradableItem.m_numOwned == 3)
		{
			PlayerData.MagnetUpgradeTime = PlayerData.MAGNETIZER_UPGRADE_DURATIONS[2];
		}
		else if (upgradableItem.m_numOwned == 4)
		{
			PlayerData.MagnetUpgradeTime = PlayerData.MAGNETIZER_UPGRADE_DURATIONS[3];
		}
		else if (upgradableItem.m_numOwned == 5)
		{
			PlayerData.MagnetUpgradeTime = PlayerData.MAGNETIZER_UPGRADE_DURATIONS[4];
		}
		else if (upgradableItem.m_numOwned == 6)
		{
			PlayerData.MagnetUpgradeTime = PlayerData.MAGNETIZER_UPGRADE_DURATIONS[5];
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Magnet Upgrade");
		dictionary.Add("TokenCost", upgradableItem.m_tokenCost.ToString());
		dictionary.Add("FedoraCost", upgradableItem.m_fedoraCost.ToString());
		dictionary.Add("NumberOwned", upgradableItem.m_numOwned.ToString());
		FlurryFacade.Instance.LogEvent("SoftPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtScoreMultiplierUpgrade(object item = null)
	{
		UpgradableItem upgradableItem = (UpgradableItem)item;
		if (upgradableItem.m_numOwned == 1)
		{
			PlayerData.ScoreMultUpgradeTime = PlayerData.SCOREMULTIPLIER_UPGRADE_DURATIONS[0];
		}
		else if (upgradableItem.m_numOwned == 2)
		{
			PlayerData.ScoreMultUpgradeTime = PlayerData.SCOREMULTIPLIER_UPGRADE_DURATIONS[1];
		}
		else if (upgradableItem.m_numOwned == 3)
		{
			PlayerData.ScoreMultUpgradeTime = PlayerData.SCOREMULTIPLIER_UPGRADE_DURATIONS[2];
		}
		else if (upgradableItem.m_numOwned == 4)
		{
			PlayerData.ScoreMultUpgradeTime = PlayerData.SCOREMULTIPLIER_UPGRADE_DURATIONS[3];
		}
		else if (upgradableItem.m_numOwned == 5)
		{
			PlayerData.ScoreMultUpgradeTime = PlayerData.SCOREMULTIPLIER_UPGRADE_DURATIONS[4];
		}
		else if (upgradableItem.m_numOwned == 6)
		{
			PlayerData.ScoreMultUpgradeTime = PlayerData.SCOREMULTIPLIER_UPGRADE_DURATIONS[5];
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Score Multiplier");
		dictionary.Add("TokenCost", upgradableItem.m_tokenCost.ToString());
		dictionary.Add("FedoraCost", upgradableItem.m_fedoraCost.ToString());
		dictionary.Add("NumberOwned", upgradableItem.m_numOwned.ToString());
		FlurryFacade.Instance.LogEvent("SoftPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtPowerUpFreq(object item = null)
	{
		UpgradableItem upgradableItem = (UpgradableItem)item;
		if (upgradableItem.m_numOwned == 1)
		{
			PlayerData.PowerUpFreqChance = 20f;
		}
		else if (upgradableItem.m_numOwned == 2)
		{
			PlayerData.PowerUpFreqChance = 30f;
		}
		else if (upgradableItem.m_numOwned == 3)
		{
			PlayerData.PowerUpFreqChance = 40f;
		}
		else if (upgradableItem.m_numOwned == 4)
		{
			PlayerData.PowerUpFreqChance = 50f;
		}
		else if (upgradableItem.m_numOwned == 5)
		{
			PlayerData.PowerUpFreqChance = 60f;
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Power Up Frequency");
		dictionary.Add("TokenCost", upgradableItem.m_tokenCost.ToString());
		dictionary.Add("FedoraCost", upgradableItem.m_fedoraCost.ToString());
		dictionary.Add("NumberOwned", upgradableItem.m_numOwned.ToString());
		FlurryFacade.Instance.LogEvent("SoftPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtContinueCost(object item = null)
	{
		UpgradableItem upgradableItem = (UpgradableItem)item;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Continue Cost");
		dictionary.Add("TokenCost", upgradableItem.m_tokenCost.ToString());
		dictionary.Add("FedoraCost", upgradableItem.m_fedoraCost.ToString());
		dictionary.Add("NumberOwned", upgradableItem.m_numOwned.ToString());
		FlurryFacade.Instance.LogEvent("SoftPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtJumpStart(object item = null)
	{
		PlayerData.JumpStarts++;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Jump Start");
		dictionary.Add("NumberOwned", PlayerData.JumpStarts.ToString());
		FlurryFacade.Instance.LogEvent("SoftPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtDuckyMomo(object item = null)
	{
		UpgradableItem upgradableItem = (UpgradableItem)item;
		PlayerData.HasMomoBeenSeen = false;
		if (upgradableItem.m_numOwned == 1)
		{
			PlayerData.MomoUnlocked = true;
			PlayerData.MomoUpgradeLevel = 0;
		}
		else if (upgradableItem.m_numOwned == 2)
		{
			PlayerData.MomoUpgradeLevel = 0;
		}
		else if (upgradableItem.m_numOwned == 3)
		{
			PlayerData.MomoUpgradeLevel = 1;
		}
		else if (upgradableItem.m_numOwned == 4)
		{
			PlayerData.MomoUpgradeLevel = 2;
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Ducky Momo");
		dictionary.Add("TokenCost", upgradableItem.m_tokenCost.ToString());
		dictionary.Add("FedoraCost", upgradableItem.m_fedoraCost.ToString());
		dictionary.Add("NumberOwned", upgradableItem.m_numOwned.ToString());
		FlurryFacade.Instance.LogEvent("SoftPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}

	public static void BoughtBabyHead(object item = null)
	{
		UpgradableItem upgradableItem = (UpgradableItem)item;
		PlayerData.HasBabyBeenSeen = false;
		if (upgradableItem.m_numOwned == 1)
		{
			PlayerData.BabyUnlocked = true;
		}
		else if (upgradableItem.m_numOwned == 2)
		{
			PlayerData.BabyFedoraPercent = 10f;
		}
		else if (upgradableItem.m_numOwned == 3)
		{
			PlayerData.BabyFedoraPercent = 15f;
		}
		else if (upgradableItem.m_numOwned == 4)
		{
			PlayerData.BabyFedoraPercent = 20f;
		}
		else if (upgradableItem.m_numOwned == 5)
		{
			PlayerData.BabyFedoraPercent = 25f;
		}
		else if (upgradableItem.m_numOwned == 6)
		{
			PlayerData.BabyFedoraPercent = 30f;
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", "Baby Head");
		dictionary.Add("TokenCost", upgradableItem.m_tokenCost.ToString());
		dictionary.Add("FedoraCost", upgradableItem.m_fedoraCost.ToString());
		dictionary.Add("NumberOwned", upgradableItem.m_numOwned.ToString());
		FlurryFacade.Instance.LogEvent("SoftPurchase", dictionary, false);
		PlayerData.SavePersistentData();
	}
}

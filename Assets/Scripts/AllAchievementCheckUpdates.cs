using UnityEngine;

public class AllAchievementCheckUpdates
{
	public static float BigMoney()
	{
		if (PlayerData.AllTimeTokens >= 50000)
		{
			return 100f;
		}
		return (float)PlayerData.AllTimeTokens / 50000f * 100f;
	}

	public static float PowerTrip()
	{
		if (PlayerData.AllTimePowerups >= 100)
		{
			return 100f;
		}
		return (float)PlayerData.AllTimePowerups / 100f * 100f;
	}

	public static float HatCollector()
	{
		if (PlayerData.AllTimeFedoras >= 50)
		{
			return 100f;
		}
		return (float)PlayerData.AllTimeFedoras / 50f * 100f;
	}

	public static float BigScore()
	{
		if (PlayerData.AllTimeScore >= 400000)
		{
			return 100f;
		}
		return (float)PlayerData.AllTimeScore / 400000f * 100f;
	}

	public static float AgencyDarling()
	{
		if (AllMissionData.CalcCompletedMissionsCount() >= 20)
		{
			return 100f;
		}
		return (float)AllMissionData.CalcCompletedMissionsCount() / 20f * 100f;
	}

	public static float MagneticPersonality()
	{
		if (PlayerData.AllTimeMagnetizers >= 5)
		{
			return 100f;
		}
		return (float)PlayerData.AllTimeMagnetizers / 5f * 100f;
	}

	public static float FlyingHigh()
	{
		if (PlayerData.AllTimeEaglePowerUpCount >= 5)
		{
			return 100f;
		}
		return (float)PlayerData.AllTimeEaglePowerUpCount / 5f * 100f;
	}

	public static float DoubleDown()
	{
		if (PlayerData.AllTimeScoreMultipliers >= 5)
		{
			return 100f;
		}
		return (float)PlayerData.AllTimeScoreMultipliers / 5f * 100f;
	}

	public static float Doofensmash()
	{
		if (PlayerData.AllTimeBossDefeats >= 50)
		{
			return 100f;
		}
		return (float)PlayerData.AllTimeBossDefeats / 50f * 100f;
	}

	public static float GoesToEleven()
	{
		PurchasableGadgetItem gadgetItem = AllItemData.GetGadgetItem(1);
		PurchasableGadgetItem gadgetItem2 = AllItemData.GetGadgetItem(0);
		PurchasableGadgetItem gadgetItem3 = AllItemData.GetGadgetItem(2);
		int upgradeNums = gadgetItem.UpgradeNums;
		if (gadgetItem2.UpgradeNums > upgradeNums)
		{
			upgradeNums = gadgetItem2.UpgradeNums;
		}
		if (gadgetItem3.UpgradeNums > upgradeNums)
		{
			upgradeNums = gadgetItem3.UpgradeNums;
		}
		if (upgradeNums >= 11)
		{
			return 100f;
		}
		return (float)upgradeNums / 12f * 100f;
	}

	public static float JumpStartEngines()
	{
		if (PlayerData.TotalUsedJumpStarts >= 10)
		{
			return 100f;
		}
		return (float)PlayerData.TotalUsedJumpStarts / 10f * 100f;
	}

	public static float PauperForTokens()
	{
		if (PlayerData.RoundScore > 35000 && PlayerData.RoundTokens == 0)
		{
			return 100f;
		}
		float num = (float)PlayerData.RoundScore / 35000f;
		if (num > 1f)
		{
			num = 1f;
		}
		num *= 0.5f;
		return num * 100f;
	}

	public static float GoingTheDistance()
	{
		if (PlayerData.RoundMeters >= 6000)
		{
			return 100f;
		}
		return (float)PlayerData.RoundMeters / 6000f * 100f;
	}

	public static float BiggerScore()
	{
		if (PlayerData.RoundScore >= 2000000)
		{
			return 100f;
		}
		return (float)PlayerData.RoundScore / 2000000f * 100f;
	}

	public static float PowerStarved()
	{
		if (PlayerData.RoundScore > 250000 && PlayerData.RoundPowerups == 0)
		{
			return 100f;
		}
		float num = (float)PlayerData.RoundScore / 250000f;
		if (num > 1f)
		{
			num = 1f;
		}
		num *= 0.5f;
		return num * 100f;
	}

	public static float ChihuahuaChasing()
	{
		if (PlayerData.OwnPinky)
		{
			return 100f;
		}
		return 0f;
	}

	public static float Pandamonium()
	{
		if (PlayerData.OwnPeter)
		{
			return 100f;
		}
		return 0f;
	}

	public static float TerrapinTime()
	{
		if (PlayerData.OwnTerry)
		{
			return 100f;
		}
		return 0f;
	}

	public static float DressedToThrill()
	{
		if (PlayerData.CostumesOwned >= 3)
		{
			return 100f;
		}
		return (float)PlayerData.CostumesOwned / 3f * 100f;
	}

	public static float MissionUnlikely()
	{
		if (AllMissionData.CalcCompletedMissionsCount() >= 45)
		{
			return 100f;
		}
		return (float)AllMissionData.CalcCompletedMissionsCount() / 45f * 100f;
	}

	public static float BossBeGone()
	{
		if (PlayerData.RoundBossDefeats >= 2)
		{
			return 100f;
		}
		return (float)PlayerData.RoundBossDefeats / 2f * 100f;
	}

	public static float SceneJumper()
	{
		if (PlayerData.RoundScenesChanged >= 6)
		{
			return 100f;
		}
		return (float)PlayerData.RoundScenesChanged / 6f * 100f;
	}

	public static float UtilityBelt()
	{
		if (PlayerData.GadgetsUnlocked >= 3)
		{
			return 100f;
		}
		return (float)PlayerData.GadgetsUnlocked / 3f * 100f;
	}

	public static float ToTheNines()
	{
		PurchasableGadgetItem gadgetItem = AllItemData.GetGadgetItem(1);
		PurchasableGadgetItem gadgetItem2 = AllItemData.GetGadgetItem(0);
		PurchasableGadgetItem gadgetItem3 = AllItemData.GetGadgetItem(2);
		int num = ((gadgetItem.UpgradeNums != gadgetItem.m_maxUpgrades - 1) ? gadgetItem.UpgradeNums : gadgetItem.m_maxUpgrades);
		int num2 = ((gadgetItem2.UpgradeNums != gadgetItem2.m_maxUpgrades - 1) ? gadgetItem2.UpgradeNums : gadgetItem2.m_maxUpgrades);
		int num3 = ((gadgetItem3.UpgradeNums != gadgetItem3.m_maxUpgrades - 1) ? gadgetItem3.UpgradeNums : gadgetItem3.m_maxUpgrades);
		if (num >= gadgetItem.m_maxUpgrades && num2 >= gadgetItem2.m_maxUpgrades && num3 >= gadgetItem3.m_maxUpgrades)
		{
			return 100f;
		}
		float num4 = num + num2 + num3;
		return num4 / (float)(gadgetItem.m_maxUpgrades * 3) * 100f;
	}

	public static void GadgetUnlocked()
	{
		float num = UtilityBelt();
		Debug.Log("UTILITY BELT " + num);
		if (PerryGameServices.m_perryAchievements != null && PerryGameServices.m_perryAchievements.ContainsKey("Utility Belt"))
		{
			PerryGameServices.m_perryAchievements["Utility Belt"].UpdateProgress(num);
		}
	}
}

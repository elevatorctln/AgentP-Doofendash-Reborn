public static class AllMissionCheckUpdates
{
	public static bool Mission1()
	{
		if (PlayerData.RoundMeters >= 1000)
		{
			return true;
		}
		return false;
	}

	public static bool Mission2()
	{
		if (PlayerData.RoundTokens >= 25)
		{
			return true;
		}
		return false;
	}

	public static bool Mission3()
	{
		if (PlayerData.RoundScore >= 5000)
		{
			return true;
		}
		return false;
	}

	public static bool Mission4()
	{
		if (PlayerData.GadgetsUnlocked >= 1)
		{
			return true;
		}
		return false;
	}

	public static bool Mission5()
	{
		if (PlayerData.RoundWindowsBroken >= 1)
		{
			return true;
		}
		return false;
	}

	public static bool Mission6()
	{
		if (PlayerData.RoundMeters >= 2000)
		{
			return true;
		}
		return false;
	}

	public static bool Mission7()
	{
		if (PlayerData.RoundTokens >= 100)
		{
			return true;
		}
		return false;
	}

	public static bool Mission8()
	{
		if (PlayerData.EagleUpgradeTime >= PlayerData.EAGLE_UPGRADE_DURATIONS[0] || PlayerData.MagnetUpgradeTime >= PlayerData.MAGNETIZER_UPGRADE_DURATIONS[0] || PlayerData.InvulnerabilityUpgradeTime >= PlayerData.INVINCIBILITY_UPGRADE_DURATIONS[0] || PlayerData.ScoreMultUpgradeTime >= PlayerData.SCOREMULTIPLIER_UPGRADE_DURATIONS[0])
		{
			return true;
		}
		return false;
	}

	public static bool Mission9()
	{
		if (PlayerData.RoundScore >= 15000)
		{
			return true;
		}
		return false;
	}

	public static bool Mission10()
	{
		if (PlayerData.RoundGadgetUses >= 1)
		{
			return true;
		}
		return false;
	}

	public static bool Mission11()
	{
		return PlayerData.RoundDidUseJumpStart;
	}

	public static bool Mission12()
	{
		if (PlayerData.HasFetchedFaceBookHighScores)
		{
			return true;
		}
		return false;
	}

	public static bool Mission13()
	{
		if (PlayerData.RoundWindowsBroken >= 3)
		{
			return true;
		}
		return false;
	}

	public static bool Mission14()
	{
		if (PlayerData.GadgetsUnlocked >= 3)
		{
			return true;
		}
		return false;
	}

	public static bool Mission15()
	{
		if (PlayerData.RoundContinues >= 1)
		{
			return true;
		}
		return false;
	}

	public static bool Mission16()
	{
		if (PlayerData.RoundHasCollectedItemFromBabyHead)
		{
			return true;
		}
		return false;
	}

	public static bool Mission17()
	{
		if (PlayerData.RoundMeters >= 3500)
		{
			return true;
		}
		return false;
	}

	public static bool Mission18()
	{
		if (PlayerData.RoundBossDefeats >= 1)
		{
			return true;
		}
		return false;
	}

	public static bool Mission19()
	{
		if (PlayerData.RoundIsDuckyTokenCollected)
		{
			return true;
		}
		return false;
	}

	public static bool Mission20()
	{
		if (PlayerData.RoundPlayerSlides >= 20)
		{
			return true;
		}
		return false;
	}

	public static bool Mission21()
	{
		if (PlayerData.RoundScore >= 300000)
		{
			return true;
		}
		return false;
	}

	public static bool Mission22()
	{
		if (PlayerData.RoundPlayerSlidesUnderObstacles >= 30)
		{
			return true;
		}
		return false;
	}

	public static bool Mission23()
	{
		if (PlayerData.RoundScore + PlayerData.RoundScoreBonus >= PlayerData.HighestAllTimeScore)
		{
			return true;
		}
		return false;
	}

	public static bool Mission24()
	{
		if (!Runner.The().currentRunner.Contains("Perry"))
		{
			return true;
		}
		return false;
	}

	public static bool Mission25()
	{
		if (PlayerData.RoundContinues >= 2)
		{
			return true;
		}
		return false;
	}

	public static bool Mission26()
	{
		if (PlayerData.RoundTokens >= 300)
		{
			return true;
		}
		return false;
	}

	public static bool Mission27()
	{
		if (PlayerData.RoundMagnetizers >= 2 || PlayerData.RoundInvincibility >= 2 || PlayerData.RoundEaglePowerUpCount >= 2 || PlayerData.RoundScoreMultipliers >= 2)
		{
			return true;
		}
		return false;
	}

	public static bool Mission28()
	{
		if (PlayerData.RoundJumpOverBoxesCount >= 15)
		{
			return true;
		}
		return false;
	}

	public static bool Mission29()
	{
		if (PlayerData.RoundBouncesWhileInvincible >= 10)
		{
			return true;
		}
		return false;
	}

	public static bool Mission30()
	{
		if (PlayerData.RoundScore >= 700000)
		{
			return true;
		}
		return false;
	}

	public static bool Mission31()
	{
		if (PlayerData.RoundHasFiredMaxedOutWeapon)
		{
			return true;
		}
		return false;
	}

	public static bool Mission32()
	{
		if (PlayerData.RoundWindowsBroken >= 3)
		{
			return true;
		}
		return false;
	}

	public static bool Mission33()
	{
		if (PlayerData.RoundTokens >= 1232)
		{
			return true;
		}
		return false;
	}

	public static bool Mission34()
	{
		if (PlayerData.RoundBossDefeats >= 2)
		{
			return true;
		}
		return false;
	}

	public static bool Mission35()
	{
		if (PlayerData.RoundMeters >= 7000 && PlayerData.RoundContinues <= 0)
		{
			return true;
		}
		return false;
	}

	public static bool Mission36()
	{
		if (PlayerData.EagleUpgradeTime >= PlayerData.EAGLE_UPGRADE_DURATIONS[4] || PlayerData.MagnetUpgradeTime >= PlayerData.MAGNETIZER_UPGRADE_DURATIONS[5] || PlayerData.InvulnerabilityUpgradeTime >= PlayerData.INVINCIBILITY_UPGRADE_DURATIONS[5] || PlayerData.ScoreMultUpgradeTime >= PlayerData.SCOREMULTIPLIER_UPGRADE_DURATIONS[5])
		{
			return true;
		}
		return false;
	}

	public static bool Mission37()
	{
		if (PlayerData.RoundBabyHeadSeenCount >= 2)
		{
			return true;
		}
		return false;
	}

	public static bool Mission38()
	{
		if (Runner.The().currentRunner == "PerryTutu" || Runner.The().currentRunner == "PeterDapper" || Runner.The().currentRunner == "PerrySuper" || Runner.The().currentRunner == "PerryFunky" || Runner.The().currentRunner == "PinkyTutu" || Runner.The().currentRunner == "TerryViking")
		{
			return true;
		}
		return false;
	}

	public static bool Mission39()
	{
		if (PlayerData.RoundDuckySeenCount >= 2)
		{
			return true;
		}
		return false;
	}

	public static bool Mission40()
	{
		if (PlayerData.RoundHasCollectedFedoraFromBabyHead)
		{
			return true;
		}
		return false;
	}

	public static bool Mission41()
	{
		if (PlayerData.RoundScore >= 1500000)
		{
			return true;
		}
		return false;
	}

	public static bool Mission42()
	{
		if (PlayerData.RoundJumpOverBotsCount >= 3)
		{
			return true;
		}
		return false;
	}

	public static bool Mission43()
	{
		if (PlayerData.RoundBossDefeats >= 3)
		{
			return true;
		}
		return false;
	}

	public static bool Mission44()
	{
		if (PlayerData.EagleUpgradeTime >= PlayerData.EAGLE_UPGRADE_DURATIONS[0] && PlayerData.MagnetUpgradeTime >= PlayerData.MAGNETIZER_UPGRADE_DURATIONS[0] && PlayerData.InvulnerabilityUpgradeTime >= PlayerData.INVINCIBILITY_UPGRADE_DURATIONS[0] && PlayerData.ScoreMultUpgradeTime >= PlayerData.SCOREMULTIPLIER_UPGRADE_DURATIONS[0])
		{
			return true;
		}
		return false;
	}

	public static bool Mission45()
	{
		if (PlayerData.RoundScore >= 2000000)
		{
			return true;
		}
		return false;
	}
}

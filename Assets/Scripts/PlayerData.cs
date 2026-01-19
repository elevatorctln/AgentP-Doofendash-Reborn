using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
	public enum MomoUpgradeLevels
	{
		None = -1,
		Low = 0,
		Medium = 1,
		High = 2
	}

	public enum GadgetTypes
	{
		None = -1,
		Water = 0,
		Fire = 1,
		Electric = 2,
		PinShooter = 3
	}

	public static string m_playerName = "You";

	private static string m_ageGroupKey = "AgeGroupChild";

	private static string m_ageVerifiedKey = "AgeVerified";

	private static string m_IsiCloudDisableByUserKey = "IsiCloudDisabledByUser";

	private static bool m_IsiCloudDisabledByUser = true;

	private static bool m_iCloudSettinghasBeenInitialized = true;

	private static string m_tokenKey = "PlayerTokens";

	private static int m_playerTokens = 250;

	private static int MAX_ALL_TIME_TOKENS = 999999999;

	private static string m_fedoraKey = "PlayerFedoras";

	private static int m_playerFedoras = 0;

	private static int MAX_ALL_TIME_FEDORAS = 999999999;

	public static float[] EAGLE_UPGRADE_DURATIONS = new float[5] { 9f, 10f, 11f, 12f, 13f };

	public static float[] MAGNETIZER_UPGRADE_DURATIONS = new float[6] { 5f, 6f, 7f, 8f, 9f, 10f };

	public static float[] INVINCIBILITY_UPGRADE_DURATIONS = new float[6] { 5f, 6f, 7f, 8f, 9f, 10f };

	public static float[] SCOREMULTIPLIER_UPGRADE_DURATIONS = new float[6] { 5f, 6f, 7f, 8f, 9f, 10f };

	public static float ms_BossPointValueInitial = 15000f;

	public static float ms_BossPointDecPerSec = 250f;

	public static float ms_BossPointValueMinimum = 250f;

	public static float BalloonyPointValueInitial = 7500f;

	public static float BalloonyPointDecPerSec = 2500f;

	public static float BalloonyPointValueMinimum = 125f;

	private static int m_nextScoreAchievementCheckThreshold = 500;

	private static int m_roundScore = 0;

	private static int m_roundScoreBonus = 0;

	private static int m_roundTokens = 0;

	private static int m_roundFedoras = 0;

	private static int m_NextMissionCheckThreshold = 0;

	private static int m_nextMeterThreshold;

	private static int m_roundMeters = 0;

	private static int m_roundPowerups = 0;

	private static int m_roundMagnetizers = 0;

	private static int m_roundInvincibility = 0;

	private static int m_roundEaglePowerUpCount = 0;

	private static int m_roundPlayerSlidesUnderObstacles = 0;

	private static int m_roundPlayerSlides = 0;

	private static int m_roundScoreMultipliers = 0;

	private static int m_roundBossBonusScore = 0;

	private static string m_RoundDoofenschmirtzEncounterCountStr = "_allTimeBossEncounters";

	private static int m_RoundDoofenschmirtzEncounterCount = 0;

	private static string m_RoundBalloonyEncounterCountStr = "_allTimeBalloonyEncounters";

	private static int m_RoundBalloonyEncounterCount = 0;

	private static int m_roundBossEncounters = 0;

	private static string m_allTimeBossDefeatsStr = "_allTimeBossDefeats";

	private static int m_allTimeBossDefeats = 0;

	private static int m_roundBossDefeats = 0;

	private static string m_HasFetchedFaceBookHighScoresString = "hasFetchedFaceBookHighScores";

	private static bool m_HasFetchedFaceBookHighScores = false;

	private static bool m_RoundHasConnectedToFacebook = false;

	private static int m_roundBouncesWhileInvincible = 0;

	private static int m_RoundJumpOverBotsCount = 0;

	private static int m_RoundJumpOverBoxesCount = 0;

	private static string m_allTimeWindowsBrokenStr = "_allTimeWindowsBroken";

	private static int m_allTimeWindowsBroken = 0;

	private static int m_roundWindowsBroken = 0;

	private static int m_roundContinues = 0;

	private static int m_baseContinueCost = 1;

	private static int MAX_CONTINUE_COST = 65536;

	private static int m_continueCost = 0;

	private static int m_roundGadgetUses = 0;

	private static bool m_RoundHasFiredMaxedOutWeapon = false;

	private static int m_roundMaxPowerGadgetFire = 0;

	private static int m_roundJumps = 0;

	private static int m_roundScenesChanged = 0;

	private static string m_highestAllTimeScoreStr = "HigestAllTimeScore";

	private static int m_highestAllTimeScore = 0;

	private static string m_allTimeTokensStr = "_allTimeTokens";

	private static int m_allTimeTokens = 0;

	private static int MAX_ALL_TIME_POWERUPS = 999999999;

	private static string m_allTimePowerupsStr = "_allTimePowerups";

	private static int m_allTimePowerups;

	private static string m_allTimeFedorasStr = "_allTimeFedoras";

	private static int m_allTimeFedoras;

	private static string m_allTimeAppLaunchesKey = "_mAllTimeAppLaunches";

	private static string m_allTimeMagnetizersStr = "_allTimeMagnetizers";

	private static int m_allTimeMagnetizers;

	private static string m_allTimeEaglePowerUpCountStr = "_allTimeEagles";

	private static int m_allTimeEaglePowerUpCount;

	private static string m_allTimeScoreMultipliersStr = "_allTimeScoreMultipliers";

	private static int m_allTimeScoreMultipliers;

	private static string m_AllTimeMetersStr = "_allTimeMeters";

	private static int m_AllTimeMeters;

	private static string m_AllTimeScoreStr = "_allTimeScore";

	private static int m_AllTimeScore;

	public static int m_menuHighScoreIndex = 0;

	private static List<PerryLeaderboard> m_perryLeaderboards;

	private static long m_NextScoreToBeat = 0L;

	private static int m_currentCharacterNameIndex = 0;

	private static int m_costumesOwned = 0;

	private static bool m_ownSuperPery;

	private static bool m_ownCoconutPerry;

	private static bool m_ownPinky;

	private static bool m_ownPinkyPrincess;

	private static bool m_ownPeter;

	private static bool m_ownGentlemenPeter;

	private static bool m_ownTerry;

	private static bool m_ownVikingTerry;

	private static string m_duplicatorinatorKey = "CoinDuplicatorInator";

	private static bool m_coinDuplicatorInator = false;

	private static string m_doubleTokensEnabledStr = "_doubleTokensEnabled";

	private static bool m_doubleTokensEnabled = false;

	private static string m_tripleTokensEnabledStr = "_tripleTokensEnabledStr";

	private static bool m_tripleTokensEnabled = false;

	private static string m_doubleTokenAppearTimeStr = "_doubleTokenAppearTime";

	private static int m_doubleTokenAppearTime = 3000;

	private static string m_tripleTokenAppearTimeStr = "_tripleTokenAppearTime";

	private static int m_tripleTokenAppearTime = 6000;

	private static string m_eagleUpgradeTimeStr = "_eagleUpgradeTime";

	private static float m_eagleUpgradeTime = 8f;

	private static string m_invulnerabilityUpgradeTimeStr = "_invulnerabilityUpgradeTime";

	private static float m_invulnerabilityUpgradeTime = 4f;

	private static string m_magnetUpgradeTimeStr = "_magnetUpgradeTime";

	private static float m_magnetUpgradeTime = 4f;

	private static string m_scoreMultUpgradeTimeStr = "_scoreMultUpgradeTime";

	private static float m_scoreMultUpgradeTime = 4f;

	private static string m_powerUpFreqChanceStr = "_powerUpFreqChance";

	private static float m_powerUpFreqChance = 10f;

	private static string m_jumpStartsStr = "_jumpStarts";

	private static int m_jumpStarts = 0;

	private static bool m_RoundDidUseJumpStart = false;

	private static string m_totalUsedJumpStartsStr = "_totalJumpStartsUsed";

	private static int m_totalUsedJumpStarts;

	private static string m_momoUnlockedStr = "_momoUnlocked";

	private static bool m_momoUnlocked = false;

	private static bool m_RoundIsDuckyTokenCollected = false;

	private static string m_momoUpgradeLevelStr = "_momoUpgradeLevel";

	private static int m_momoUpgradeLevel = -1;

	private static string m_HasMomoBeenSeenStr = "_momoHasBeenSeen";

	private static bool m_HasMomoBeenSeen = false;

	private static int m_roundDuckySeen = 0;

	private static string m_allTimeDuckySeenCountStr = "_allTimeDuckySeen";

	private static int m_allTimeDuckySeenCount = 0;

	private static int m_RoundBabyHeadSeenCount = 0;

	private static bool m_RoundHasCollectedFedoraFromBabyHead = false;

	private static bool m_RoundHasCollectedItemFromBabyHead = false;

	private static string m_babyUnlockedStr = "_babyHeadUnlocked";

	private static bool m_babyUnlocked = false;

	private static string m_babyFedoraPercentStr = "_babyFedoraPercent";

	private static float m_babyFedoraPercent = 0f;

	private static string m_babyChancePercentStr = "_babyChancePercent";

	private static float m_babyChancePercent = 10f;

	private static string m_HasBabyBeenSeenStr = "_babyHasBeenSeen";

	private static bool m_HasBabyBeenSeen = false;

	public static GadgetTypes m_currentGadgetType = GadgetTypes.None;

	private static string m_hasWaterWeaponKey = "hasWaterWeapon";

	private static string m_hasFireWeaponKey = "hasFireWeapon";

	private static string m_hasElectricWeaponKey = "hasElectricWeapon";

	private static string m_hasPinShooterKey = "hasPinShooter";

	private static bool m_hasWaterWeapon = false;

	private static bool m_hasFireWeapon = false;

	private static bool m_hasElectricWeapon = false;

	private static bool m_hasPinWeapon = false;

	private static ArrayList m_currentMissions = null;

	private static int m_roundMissionsComplete = 0;

	private static string m_MaxMissionLevelSeenByUserIndexStr = "_MaxMissionLevelSeenByUserIndex";

	private static int m_MaxMissionLevelSeenByUserIndex = 0;

	private static string m_ShouldNotShowTutorialStr = "_ShouldNotShowTutorial";

	private static bool m_ShouldNotShowTutorial = false;

	private static string m_ShouldNotShowBossTutorialStr = "_ShouldNotShowBossTutorial";

	private static bool m_ShouldNotShowBossTutorial = false;

	private static string m_ShouldNotShowBossTutorialNoGadgetStr = "_ShouldNotShowBossTutorialNoGadget";

	private static bool m_ShouldNotShowBossTutorialNoGadget = false;

	public static bool HasBeenAgeGated
	{
		get
		{
			return PerryiCloudManager.The.GetBoolItem(m_ageVerifiedKey);
		}
		set
		{
			PerryiCloudManager.The.SetItem(m_ageVerifiedKey, value);
		}
	}

	public static bool PlayerIsChild
	{
		get
		{
			return PerryiCloudManager.The.GetBoolItem(m_ageGroupKey);
		}
		set
		{
			PerryiCloudManager.The.SetItem(m_ageGroupKey, value);
		}
	}

	public static bool IsiCloudDisabledByUser
	{
		get
		{
			if (!m_iCloudSettinghasBeenInitialized)
			{
				InitializeUseriCloudPreference();
			}
			return m_IsiCloudDisabledByUser;
		}
		set
		{
			if (value)
			{
				PlayerPrefs.SetInt(m_IsiCloudDisableByUserKey, 1);
			}
			else
			{
				PlayerPrefs.SetInt(m_IsiCloudDisableByUserKey, 0);
			}
			m_IsiCloudDisabledByUser = true;
		}
	}

	public static int playerTokens
	{
		get
		{
			return m_playerTokens;
		}
		set
		{
			m_playerTokens = value;
		}
	}

	public static int playerFedoras
	{
		get
		{
			return m_playerFedoras;
		}
		set
		{
			m_playerFedoras = value;
		}
	}

	public static int TotalTokenMultiplier
	{
		get
		{
			if (coinDuplicatorInator)
			{
				return 2;
			}
			return 1;
		}
	}

	public static int RoundScore
	{
		get
		{
			return m_roundScore;
		}
		set
		{
			m_roundScore = value;
			if (m_roundScore == 0)
			{
				m_nextScoreAchievementCheckThreshold = 500;
			}
			UpdateCheckMissions();
			m_nextScoreAchievementCheckThreshold += 500;
			float percent = AllAchievementCheckUpdates.BigScore();
			float percent2 = AllAchievementCheckUpdates.BiggerScore();
			float percent3 = AllAchievementCheckUpdates.PowerStarved();
			if (PerryGameServices.m_perryAchievements != null)
			{
				if (PerryGameServices.m_perryAchievements.ContainsKey("Big Score"))
				{
					PerryGameServices.m_perryAchievements["Big Score"].UpdateProgress(percent);
				}
				if (PerryGameServices.m_perryAchievements != null && PerryGameServices.m_perryAchievements.ContainsKey("Bigger Score"))
				{
					PerryGameServices.m_perryAchievements["Bigger Score"].UpdateProgress(percent2);
				}
				if (PerryGameServices.m_perryAchievements.ContainsKey("Power Starved"))
				{
					PerryGameServices.m_perryAchievements["Power Starved"].UpdateProgress(percent3);
				}
			}
		}
	}

	public static int RoundScoreBonus
	{
		get
		{
			return m_roundScoreBonus;
		}
		set
		{
			m_roundScoreBonus = value * AllMissionData.TotalScoreMultiplier;
			UpdateCheckMissions();
		}
	}

	public static int RoundTokens
	{
		get
		{
			return m_roundTokens;
		}
		set
		{
			m_roundTokens = value;
			UpdateCheckMissions();
		}
	}

	public static int RoundFedoras
	{
		get
		{
			return m_roundFedoras;
		}
		set
		{
			m_roundFedoras = value;
			UpdateCheckMissions();
		}
	}

	public static int RoundMeters
	{
		get
		{
			return m_roundMeters;
		}
		set
		{
			m_roundMeters = value;
			if (m_roundMeters == 0)
			{
				m_nextMeterThreshold = 1000;
				m_NextMissionCheckThreshold = 500;
			}
			if (m_roundMeters > m_nextMeterThreshold)
			{
				MainMenuEventManager.TriggerShowMeterNotification(m_nextMeterThreshold);
				if (m_nextMeterThreshold == 1000)
				{
					m_nextMeterThreshold = 10000;
				}
				else
				{
					m_nextMeterThreshold += 10000;
				}
			}
			if (m_roundMeters >= m_NextMissionCheckThreshold)
			{
				m_NextMissionCheckThreshold += 500;
				UpdateCheckMissions();
				float percent = AllAchievementCheckUpdates.GoingTheDistance();
				if (PerryGameServices.m_perryAchievements != null && PerryGameServices.m_perryAchievements.ContainsKey("Going the Distance"))
				{
					PerryGameServices.m_perryAchievements["Going the Distance"].UpdateProgress(percent);
				}
			}
		}
	}

	public static int RoundPowerups
	{
		get
		{
			return m_roundPowerups;
		}
		set
		{
			m_roundPowerups = value;
			UpdateCheckMissions();
		}
	}

	public static int RoundMagnetizers
	{
		get
		{
			return m_roundMagnetizers;
		}
		set
		{
			m_roundMagnetizers = value;
			UpdateCheckMissions();
		}
	}

	public static int RoundInvincibility
	{
		get
		{
			return m_roundInvincibility;
		}
		set
		{
			m_roundInvincibility = value;
			UpdateCheckMissions();
		}
	}

	public static int RoundEaglePowerUpCount
	{
		get
		{
			return m_roundEaglePowerUpCount;
		}
		set
		{
			m_roundEaglePowerUpCount = value;
			UpdateCheckMissions();
		}
	}

	public static int RoundPlayerSlidesUnderObstacles
	{
		get
		{
			return m_roundPlayerSlidesUnderObstacles;
		}
		set
		{
			m_roundPlayerSlidesUnderObstacles = value;
			UpdateCheckMissions();
		}
	}

	public static int RoundPlayerSlides
	{
		get
		{
			return m_roundPlayerSlides;
		}
		set
		{
			m_roundPlayerSlides = value;
			UpdateCheckMissions();
		}
	}

	public static int RoundScoreMultipliers
	{
		get
		{
			return m_roundScoreMultipliers;
		}
		set
		{
			m_roundScoreMultipliers = value;
			UpdateCheckMissions();
		}
	}

	public static int RoundBossBonusScore
	{
		get
		{
			return m_roundBossBonusScore;
		}
		set
		{
			m_roundBossBonusScore = value * AllMissionData.TotalScoreMultiplier;
			m_roundScore += m_roundBossBonusScore;
			UpdateCheckMissions();
		}
	}

	public static int RoundDoofenschmirtzEncounterCount
	{
		get
		{
			return m_RoundDoofenschmirtzEncounterCount;
		}
		set
		{
			m_RoundDoofenschmirtzEncounterCount = value;
			PerryiCloudManager.The.SetItem(m_RoundDoofenschmirtzEncounterCountStr, m_RoundDoofenschmirtzEncounterCount);
			UpdateCheckMissions();
		}
	}

	public static int RoundBalloonyEncounterCount
	{
		get
		{
			return m_RoundBalloonyEncounterCount;
		}
		set
		{
			m_RoundBalloonyEncounterCount = value;
			PerryiCloudManager.The.SetItem(m_RoundBalloonyEncounterCountStr, m_RoundBalloonyEncounterCount);
			UpdateCheckMissions();
		}
	}

	public static int RoundBossEncounters
	{
		get
		{
			return m_roundBossEncounters;
		}
		set
		{
			m_roundBossEncounters = value;
			UpdateCheckMissions();
		}
	}

	public static int AllTimeBossDefeats
	{
		get
		{
			return m_allTimeBossDefeats;
		}
		set
		{
			m_allTimeBossDefeats = value;
			PerryiCloudManager.The.SetItem(m_allTimeBossDefeatsStr, m_allTimeBossDefeats);
			float percent = AllAchievementCheckUpdates.Doofensmash();
			if (PerryGameServices.m_perryAchievements != null && PerryGameServices.m_perryAchievements.ContainsKey("Doofensmash"))
			{
				PerryGameServices.m_perryAchievements["Doofensmash"].UpdateProgress(percent);
			}
			UpdateCheckMissions();
		}
	}

	public static int RoundBossDefeats
	{
		get
		{
			return m_roundBossDefeats;
		}
		set
		{
			m_roundBossDefeats = value;
			float percent = AllAchievementCheckUpdates.BossBeGone();
			if (PerryGameServices.m_perryAchievements != null && PerryGameServices.m_perryAchievements.ContainsKey("Boss Be Gone"))
			{
				PerryGameServices.m_perryAchievements["Boss Be Gone"].UpdateProgress(percent);
			}
			UpdateCheckMissions();
		}
	}

	public static bool HasFetchedFaceBookHighScores
	{
		get
		{
			return PlayerPrefs.GetInt(m_HasFetchedFaceBookHighScoresString) > 0;
		}
		set
		{
			m_HasFetchedFaceBookHighScores = value;
			int value2 = 0;
			if (m_HasFetchedFaceBookHighScores)
			{
				value2 = 1;
			}
			PlayerPrefs.SetInt(m_HasFetchedFaceBookHighScoresString, value2);
		}
	}

	public static bool RoundHasConnectedToFacebook
	{
		get
		{
			return m_RoundHasConnectedToFacebook;
		}
		set
		{
			m_RoundHasConnectedToFacebook = value;
			UpdateCheckMissions();
		}
	}

	public static int RoundBouncesWhileInvincible
	{
		get
		{
			return m_roundBouncesWhileInvincible;
		}
		set
		{
			m_roundBouncesWhileInvincible = value;
			UpdateCheckMissions();
		}
	}

	public static int RoundJumpOverBotsCount
	{
		get
		{
			return m_RoundJumpOverBotsCount;
		}
		set
		{
			m_RoundJumpOverBotsCount = value;
			UpdateCheckMissions();
		}
	}

	public static int RoundJumpOverBoxesCount
	{
		get
		{
			return m_RoundJumpOverBoxesCount;
		}
		set
		{
			m_RoundJumpOverBoxesCount = value;
			UpdateCheckMissions();
		}
	}

	public static int AllTimeWindowsBroken
	{
		get
		{
			return m_allTimeWindowsBroken;
		}
		set
		{
			m_allTimeWindowsBroken = value;
			PerryiCloudManager.The.SetItem(m_allTimeWindowsBrokenStr, m_allTimeWindowsBroken);
			UpdateCheckMissions();
		}
	}

	public static int RoundWindowsBroken
	{
		get
		{
			return m_roundWindowsBroken;
		}
		set
		{
			m_roundWindowsBroken = value;
			UpdateCheckMissions();
		}
	}

	public static int RoundContinues
	{
		get
		{
			return m_roundContinues;
		}
		set
		{
			m_roundContinues = value;
			UpdateCheckMissions();
		}
	}

	public static int BaseContinueCost
	{
		get
		{
			return m_baseContinueCost;
		}
	}

	public static int ContinueCost
	{
		get
		{
			int num = m_continueCost - ContinueCostReduction;
			if (num < 1)
			{
				num = 1;
			}
			return num;
		}
		set
		{
			if (value > MAX_CONTINUE_COST || value < 0)
			{
				m_continueCost = MAX_CONTINUE_COST;
			}
			else
			{
				m_continueCost = value;
			}
			UpdateCheckMissions();
		}
	}

	public static int RoundGadgetUses
	{
		get
		{
			return m_roundGadgetUses;
		}
		set
		{
			m_roundGadgetUses = value;
			UpdateCheckMissions();
		}
	}

	public static bool RoundHasFiredMaxedOutWeapon
	{
		get
		{
			return m_RoundHasFiredMaxedOutWeapon;
		}
		set
		{
			m_RoundHasFiredMaxedOutWeapon = value;
			UpdateCheckMissions();
		}
	}

	public static int RoundMaxPowerGadgetFire
	{
		get
		{
			return m_roundMaxPowerGadgetFire;
		}
		set
		{
			m_roundMaxPowerGadgetFire = value;
			UpdateCheckMissions();
		}
	}

	public static int RoundJumps
	{
		get
		{
			return m_roundJumps;
		}
		set
		{
			m_roundJumps = value;
			UpdateCheckMissions();
		}
	}

	public static int RoundScenesChanged
	{
		get
		{
			return m_roundScenesChanged;
		}
		set
		{
			m_roundScenesChanged = value;
			float percent = AllAchievementCheckUpdates.SceneJumper();
			if (PerryGameServices.m_perryAchievements != null && PerryGameServices.m_perryAchievements.ContainsKey("Scene Jumper"))
			{
				PerryGameServices.m_perryAchievements["Scene Jumper"].UpdateProgress(percent);
			}
			UpdateCheckMissions();
		}
	}

	public static int HighestAllTimeScore
	{
		get
		{
			return m_highestAllTimeScore;
		}
		set
		{
			if (value > m_highestAllTimeScore)
			{
				m_highestAllTimeScore = value;
				PerryiCloudManager.The.SetItem(m_highestAllTimeScoreStr, m_highestAllTimeScore);
				PerryiCloudManager.The.Synchronize();
				if (m_perryLeaderboards != null && m_menuHighScoreIndex > -1 && m_menuHighScoreIndex < m_perryLeaderboards.Count)
				{
					PerryGameServices.SubmitScore(m_perryLeaderboards[m_menuHighScoreIndex].LeaderboardID, m_highestAllTimeScore);
				}
				HighScoreGUIManager.The.SortHighScores();
			}
		}
	}

	public static int AllTimeTokens
	{
		get
		{
			return m_allTimeTokens;
		}
		set
		{
			if (m_allTimeTokens < MAX_ALL_TIME_TOKENS)
			{
				m_allTimeTokens = value;
				UpdateTokenAchievements();
				UpdateCheckMissions();
			}
		}
	}

	public static int AllTimePowerups
	{
		get
		{
			return m_allTimePowerups;
		}
		set
		{
			if (m_allTimePowerups < MAX_ALL_TIME_POWERUPS)
			{
				m_allTimePowerups = value;
				float percent = AllAchievementCheckUpdates.PowerTrip();
				if (PerryGameServices.m_perryAchievements != null && PerryGameServices.m_perryAchievements.ContainsKey("Power Trip"))
				{
					PerryGameServices.m_perryAchievements["Power Trip"].UpdateProgress(percent);
				}
			}
			UpdateCheckMissions();
		}
	}

	public static int AllTimeFedoras
	{
		get
		{
			return m_allTimeFedoras;
		}
		set
		{
			m_allTimeFedoras = value;
			if (m_allTimeFedoras < MAX_ALL_TIME_FEDORAS)
			{
				UpdateFedoraAchievements();
			}
			UpdateCheckMissions();
		}
	}

	public static int AllTimeAppLaunches
	{
		get
		{
			return PerryiCloudManager.The.GetIntItem(m_allTimeAppLaunchesKey, PerryiCloudManager.RetrieveType.Max);
		}
		set
		{
			PerryiCloudManager.The.SetItem(m_allTimeAppLaunchesKey, value);
		}
	}

	public static int AllTimeMagnetizers
	{
		get
		{
			return m_allTimeMagnetizers;
		}
		set
		{
			m_allTimeMagnetizers = value;
			float percent = AllAchievementCheckUpdates.MagneticPersonality();
			if (PerryGameServices.m_perryAchievements != null && PerryGameServices.m_perryAchievements.ContainsKey("Magnetic Personality"))
			{
				PerryGameServices.m_perryAchievements["Magnetic Personality"].UpdateProgress(percent);
			}
			UpdateCheckMissions();
		}
	}

	public static int AllTimeEaglePowerUpCount
	{
		get
		{
			return m_allTimeEaglePowerUpCount;
		}
		set
		{
			m_allTimeEaglePowerUpCount = value;
			float percent = AllAchievementCheckUpdates.FlyingHigh();
			if (PerryGameServices.m_perryAchievements != null && PerryGameServices.m_perryAchievements.ContainsKey("Flying High"))
			{
				PerryGameServices.m_perryAchievements["Flying High"].UpdateProgress(percent);
			}
			UpdateCheckMissions();
		}
	}

	public static int AllTimeScoreMultipliers
	{
		get
		{
			return m_allTimeScoreMultipliers;
		}
		set
		{
			m_allTimeScoreMultipliers = value;
			float percent = AllAchievementCheckUpdates.DoubleDown();
			if (PerryGameServices.m_perryAchievements != null && PerryGameServices.m_perryAchievements.ContainsKey("Double Down"))
			{
				PerryGameServices.m_perryAchievements["Double Down"].UpdateProgress(percent);
			}
			UpdateCheckMissions();
		}
	}

	public static int AllTimeMeters
	{
		get
		{
			return m_AllTimeMeters;
		}
		set
		{
			m_AllTimeMeters = value;
		}
	}

	public static int AllTimeScore
	{
		get
		{
			return m_AllTimeScore;
		}
		set
		{
			m_AllTimeScore = value;
		}
	}

	public static List<PerryLeaderboard> PerryLeaderboards
	{
		get
		{
			return m_perryLeaderboards;
		}
		set
		{
			m_perryLeaderboards = value;
			UpdateCheckMissions();
		}
	}

	public static long NextScoreToBeat
	{
		get
		{
			return m_NextScoreToBeat;
		}
		set
		{
			m_NextScoreToBeat = value;
		}
	}

	public static int CurrentCharacterIndex
	{
		set
		{
			m_currentCharacterNameIndex = value;
		}
	}

	public static string CurrentCharacterName
	{
		get
		{
			PlayableCharacterChooser playableCharacterChooser = GlobalGUIManager.The.FindPlayableCharacterChooser(m_currentCharacterNameIndex);
			return playableCharacterChooser.FindModelName().ToLower();
		}
	}

	public static int CostumesOwned
	{
		get
		{
			return m_costumesOwned;
		}
		set
		{
			m_costumesOwned = value;
			float percent = AllAchievementCheckUpdates.DressedToThrill();
			if (PerryGameServices.m_perryAchievements != null && PerryGameServices.m_perryAchievements.ContainsKey("Dressed to Thrill"))
			{
				PerryGameServices.m_perryAchievements["Dressed to Thrill"].UpdateProgress(percent);
			}
		}
	}

	public static bool OwnSuperPerry
	{
		get
		{
			return m_ownSuperPery;
		}
		set
		{
			m_ownSuperPery = value;
		}
	}

	public static bool OwnCoconutPerry
	{
		get
		{
			return m_ownCoconutPerry;
		}
		set
		{
			m_ownCoconutPerry = value;
		}
	}

	public static bool OwnPinky
	{
		get
		{
			return m_ownPinky;
		}
		set
		{
			m_ownPinky = value;
			if (m_ownPinky && PerryGameServices.m_perryAchievements != null && PerryGameServices.m_perryAchievements.ContainsKey("Chihuahua Chasing"))
			{
				PerryGameServices.m_perryAchievements["Chihuahua Chasing"].UpdateProgress(100f);
			}
		}
	}

	public static bool OwnPinkyPrincess
	{
		get
		{
			return m_ownPinkyPrincess;
		}
		set
		{
			m_ownPinkyPrincess = value;
		}
	}

	public static bool OwnPeter
	{
		get
		{
			return m_ownPeter;
		}
		set
		{
			m_ownPeter = value;
			if (m_ownPeter && PerryGameServices.m_perryAchievements != null && PerryGameServices.m_perryAchievements.ContainsKey("Pandamonium"))
			{
				PerryGameServices.m_perryAchievements["Pandamonium"].UpdateProgress(100f);
			}
		}
	}

	public static bool OwnGentlemenPeter
	{
		get
		{
			return m_ownGentlemenPeter;
		}
		set
		{
			m_ownGentlemenPeter = value;
		}
	}

	public static bool OwnTerry
	{
		get
		{
			return m_ownTerry;
		}
		set
		{
			m_ownTerry = value;
			if (m_ownTerry && PerryGameServices.m_perryAchievements != null && PerryGameServices.m_perryAchievements.ContainsKey("Terrapin Time"))
			{
				PerryGameServices.m_perryAchievements["Terrapin Time"].UpdateProgress(100f);
			}
		}
	}

	public static bool OwnVikingTerry
	{
		get
		{
			return m_ownVikingTerry;
		}
		set
		{
			m_ownVikingTerry = value;
		}
	}

	public static bool coinDuplicatorInator
	{
		get
		{
			return m_coinDuplicatorInator;
		}
		set
		{
			m_coinDuplicatorInator = value;
			PerryiCloudManager.The.SetItem(m_duplicatorinatorKey, m_coinDuplicatorInator);
			UpdateCheckMissions();
		}
	}

	public static bool DoubleTokensEnabled
	{
		get
		{
			return m_doubleTokensEnabled;
		}
		set
		{
			m_doubleTokensEnabled = true;
			PerryiCloudManager.The.SetItem(m_doubleTokensEnabledStr, m_doubleTokensEnabled);
			UpdateCheckMissions();
		}
	}

	public static bool TripleTokensEnabled
	{
		get
		{
			return m_tripleTokensEnabled;
		}
		set
		{
			m_tripleTokensEnabled = true;
			PerryiCloudManager.The.SetItem(m_tripleTokensEnabledStr, m_tripleTokensEnabled);
			UpdateCheckMissions();
		}
	}

	public static int DoubleTokenAppearTime
	{
		get
		{
			return m_doubleTokenAppearTime;
		}
		set
		{
			m_doubleTokenAppearTime = value;
			PerryiCloudManager.The.SetItem(m_doubleTokenAppearTimeStr, m_doubleTokenAppearTime);
			UpdateCheckMissions();
		}
	}

	public static int TripleTokenAppearTime
	{
		get
		{
			return m_tripleTokenAppearTime;
		}
		set
		{
			m_tripleTokenAppearTime = value;
			PerryiCloudManager.The.SetItem(m_tripleTokenAppearTimeStr, m_tripleTokenAppearTime);
			UpdateCheckMissions();
		}
	}

	public static float EagleUpgradeTime
	{
		get
		{
			return m_eagleUpgradeTime;
		}
		set
		{
			m_eagleUpgradeTime = value;
			PerryiCloudManager.The.SetItem(m_eagleUpgradeTimeStr, m_eagleUpgradeTime);
			UpdateCheckMissions();
		}
	}

	public static float InvulnerabilityUpgradeTime
	{
		get
		{
			return m_invulnerabilityUpgradeTime;
		}
		set
		{
			m_invulnerabilityUpgradeTime = value;
			PerryiCloudManager.The.SetItem(m_invulnerabilityUpgradeTimeStr, m_invulnerabilityUpgradeTime);
			UpdateCheckMissions();
		}
	}

	public static float MagnetUpgradeTime
	{
		get
		{
			return m_magnetUpgradeTime;
		}
		set
		{
			m_magnetUpgradeTime = value;
			PerryiCloudManager.The.SetItem(m_magnetUpgradeTimeStr, m_magnetUpgradeTime);
			UpdateCheckMissions();
		}
	}

	public static float ScoreMultUpgradeTime
	{
		get
		{
			return m_scoreMultUpgradeTime;
		}
		set
		{
			m_scoreMultUpgradeTime = value;
			PerryiCloudManager.The.SetItem(m_scoreMultUpgradeTimeStr, m_scoreMultUpgradeTime);
			UpdateCheckMissions();
		}
	}

	public static float PowerUpFreqChance
	{
		get
		{
			return m_powerUpFreqChance;
		}
		set
		{
			m_powerUpFreqChance = value;
			PerryiCloudManager.The.SetItem(m_powerUpFreqChanceStr, m_powerUpFreqChance);
			UpdateCheckMissions();
		}
	}

	public static int ContinueCostReduction
	{
		get
		{
			return AllItemData.FindUpgradableItem(AllItemData.ms_ContinueCostReductionUID).m_numOwned;
		}
	}

	public static int JumpStarts
	{
		get
		{
			return m_jumpStarts;
		}
		set
		{
			m_jumpStarts = value;
			AllItemData.SetJumpStartSpecialText(m_jumpStarts.ToString("N0"));
			PerryiCloudManager.The.SetItem(m_jumpStartsStr, m_jumpStarts);
			UpdateCheckMissions();
		}
	}

	public static bool RoundDidUseJumpStart
	{
		get
		{
			return m_RoundDidUseJumpStart;
		}
		set
		{
			m_RoundDidUseJumpStart = value;
		}
	}

	public static int TotalUsedJumpStarts
	{
		get
		{
			return m_totalUsedJumpStarts;
		}
		set
		{
			m_totalUsedJumpStarts = value;
			PerryiCloudManager.The.SetItem(m_totalUsedJumpStartsStr, m_totalUsedJumpStarts);
			float percent = AllAchievementCheckUpdates.JumpStartEngines();
			if (PerryGameServices.m_perryAchievements != null && PerryGameServices.m_perryAchievements.ContainsKey("Jumpstart Your Engines"))
			{
				PerryGameServices.m_perryAchievements["Jumpstart Your Engines"].UpdateProgress(percent);
			}
			UpdateCheckMissions();
		}
	}

	public static bool MomoUnlocked
	{
		get
		{
			return m_momoUnlocked;
		}
		set
		{
			m_momoUnlocked = value;
			PerryiCloudManager.The.SetItem(m_momoUnlockedStr, m_momoUnlocked);
			UpdateCheckMissions();
		}
	}

	public static bool RoundIsDuckyTokenCollected
	{
		get
		{
			return m_RoundIsDuckyTokenCollected;
		}
		set
		{
			m_RoundIsDuckyTokenCollected = value;
			UpdateCheckMissions();
		}
	}

	public static int MomoUpgradeLevel
	{
		get
		{
			return m_momoUpgradeLevel;
		}
		set
		{
			m_momoUpgradeLevel = value;
			PerryiCloudManager.The.SetItem(m_momoUpgradeLevelStr, m_momoUpgradeLevel);
			UpdateCheckMissions();
		}
	}

	public static bool HasMomoBeenSeen
	{
		get
		{
			return m_HasMomoBeenSeen;
		}
		set
		{
			m_HasMomoBeenSeen = value;
			PerryiCloudManager.The.SetItem(m_HasMomoBeenSeenStr, m_HasMomoBeenSeen);
			UpdateCheckMissions();
		}
	}

	public static int RoundDuckySeenCount
	{
		get
		{
			return m_roundDuckySeen;
		}
		set
		{
			m_roundDuckySeen = value;
			UpdateCheckMissions();
		}
	}

	public static int AllTimeDuckySeenCount
	{
		get
		{
			return m_allTimeDuckySeenCount;
		}
		set
		{
			m_allTimeDuckySeenCount = value;
			PerryiCloudManager.The.SetItem(m_allTimeDuckySeenCountStr, m_allTimeDuckySeenCount);
			UpdateCheckMissions();
		}
	}

	public static int RoundBabyHeadSeenCount
	{
		get
		{
			return m_RoundBabyHeadSeenCount;
		}
		set
		{
			m_RoundBabyHeadSeenCount = value;
			UpdateCheckMissions();
		}
	}

	public static bool RoundHasCollectedFedoraFromBabyHead
	{
		get
		{
			return m_RoundHasCollectedFedoraFromBabyHead;
		}
		set
		{
			m_RoundHasCollectedFedoraFromBabyHead = value;
			UpdateCheckMissions();
		}
	}

	public static bool RoundHasCollectedItemFromBabyHead
	{
		get
		{
			return m_RoundHasCollectedItemFromBabyHead;
		}
		set
		{
			m_RoundHasCollectedItemFromBabyHead = value;
			UpdateCheckMissions();
		}
	}

	public static bool BabyUnlocked
	{
		get
		{
			return m_babyUnlocked;
		}
		set
		{
			m_babyUnlocked = value;
			PerryiCloudManager.The.SetItem(m_babyUnlockedStr, m_babyUnlocked);
			UpdateCheckMissions();
		}
	}

	public static float BabyFedoraPercent
	{
		get
		{
			return m_babyFedoraPercent;
		}
		set
		{
			m_babyFedoraPercent = value;
			PerryiCloudManager.The.SetItem(m_babyFedoraPercentStr, m_babyFedoraPercent);
			UpdateCheckMissions();
		}
	}

	public static float BabyChancePercent
	{
		get
		{
			return m_babyChancePercent;
		}
		set
		{
			m_babyChancePercent = value;
			PerryiCloudManager.The.SetItem(m_babyChancePercentStr, m_babyChancePercent);
			UpdateCheckMissions();
		}
	}

	public static bool HasBabyBeenSeen
	{
		get
		{
			return m_HasBabyBeenSeen;
		}
		set
		{
			m_HasBabyBeenSeen = value;
			PerryiCloudManager.The.SetItem(m_HasBabyBeenSeenStr, m_HasBabyBeenSeen);
			UpdateCheckMissions();
		}
	}

	public static int GadgetsUnlocked
	{
		get
		{
			int num = 0;
			if (hasElectricWeapon)
			{
				num++;
			}
			if (hasWaterWeapon)
			{
				num++;
			}
			if (hasFireWeapon)
			{
				num++;
			}
			if (HasPinWeapon)
			{
				num++;
			}
			return num;
		}
	}

	public static bool hasWaterWeapon
	{
		get
		{
			return m_hasWaterWeapon;
		}
		set
		{
			m_hasWaterWeapon = value;
			AllAchievementCheckUpdates.GadgetUnlocked();
			UpdateCheckMissions();
			if (PerryiCloudManager.The != null)
			{
				PerryiCloudManager.The.SetItem(m_hasWaterWeaponKey, m_hasWaterWeapon);
			}
		}
	}

	public static bool hasFireWeapon
	{
		get
		{
			return m_hasFireWeapon;
		}
		set
		{
			m_hasFireWeapon = value;
			AllAchievementCheckUpdates.GadgetUnlocked();
			UpdateCheckMissions();
			if (PerryiCloudManager.The != null)
			{
				PerryiCloudManager.The.SetItem(m_hasFireWeaponKey, m_hasFireWeapon);
			}
		}
	}

	public static bool hasElectricWeapon
	{
		get
		{
			return m_hasElectricWeapon;
		}
		set
		{
			m_hasElectricWeapon = value;
			AllAchievementCheckUpdates.GadgetUnlocked();
			UpdateCheckMissions();
			if (PerryiCloudManager.The != null)
			{
				PerryiCloudManager.The.SetItem(m_hasElectricWeaponKey, m_hasElectricWeapon);
			}
		}
	}

	public static bool HasPinWeapon
	{
		get
		{
			return m_hasPinWeapon;
		}
		set
		{
			m_hasPinWeapon = value;
			UpdateCheckMissions();
			if (PerryiCloudManager.The != null)
			{
				PerryiCloudManager.The.SetItem(m_hasPinShooterKey, m_hasPinWeapon);
			}
		}
	}

	public static int WaterWeaponUpgrades
	{
		get
		{
			PurchasableGadgetItem gadgetItem = AllItemData.GetGadgetItem(0);
			return gadgetItem.UpgradeNums;
		}
		set
		{
			PurchasableGadgetItem gadgetItem = AllItemData.GetGadgetItem(0);
			gadgetItem.UpgradeNums = value;
			CheckForGadgetUpgradeAchievements();
			UpdateCheckMissions();
		}
	}

	public static int FireWeaponUpgrades
	{
		get
		{
			PurchasableGadgetItem gadgetItem = AllItemData.GetGadgetItem(1);
			return gadgetItem.UpgradeNums;
		}
		set
		{
			PurchasableGadgetItem gadgetItem = AllItemData.GetGadgetItem(1);
			gadgetItem.UpgradeNums = value;
			CheckForGadgetUpgradeAchievements();
			UpdateCheckMissions();
		}
	}

	public static int ElectricWeaponUpgrades
	{
		get
		{
			PurchasableGadgetItem gadgetItem = AllItemData.GetGadgetItem(2);
			return gadgetItem.UpgradeNums;
		}
		set
		{
			PurchasableGadgetItem gadgetItem = AllItemData.GetGadgetItem(2);
			gadgetItem.UpgradeNums = value;
			CheckForGadgetUpgradeAchievements();
			UpdateCheckMissions();
		}
	}

	public static int RoundMissionsComplete
	{
		get
		{
			return m_roundMissionsComplete;
		}
		set
		{
			m_roundMissionsComplete = value;
		}
	}

	public static int MaxMissionLevelSeenByUserIndex
	{
		get
		{
			return m_MaxMissionLevelSeenByUserIndex;
		}
		set
		{
			m_MaxMissionLevelSeenByUserIndex = value;
			PerryiCloudManager.The.SetItem(m_MaxMissionLevelSeenByUserIndexStr, m_MaxMissionLevelSeenByUserIndex);
		}
	}

	public static Level CurrentLevel
	{
		get
		{
			return AllLevelData.FindLevel(MaxMissionLevelSeenByUserIndex);
		}
	}

	public static int CurrentLevelBadgeVal
	{
		get
		{
			return AllMissionData.CalcMissionGroupMissionCompletedCount(MaxMissionLevelSeenByUserIndex);
		}
	}

	public static bool ShouldNotShowTutorial
	{
		get
		{
			return m_ShouldNotShowTutorial;
		}
		set
		{
			m_ShouldNotShowTutorial = value;
			PerryiCloudManager.The.SetItem(m_ShouldNotShowTutorialStr, m_ShouldNotShowTutorial);
		}
	}

	public static bool ShouldNotShowBossTutorial
	{
		get
		{
			return m_ShouldNotShowBossTutorial;
		}
		set
		{
			m_ShouldNotShowBossTutorial = value;
			PerryiCloudManager.The.SetItem(m_ShouldNotShowBossTutorialStr, m_ShouldNotShowBossTutorial);
		}
	}

	public static bool ShouldNotShowBossTutorialNoGadget
	{
		get
		{
			return m_ShouldNotShowBossTutorialNoGadget;
		}
		set
		{
			m_ShouldNotShowBossTutorialNoGadget = value;
			PerryiCloudManager.The.SetItem(m_ShouldNotShowBossTutorialNoGadgetStr, m_ShouldNotShowBossTutorialNoGadget);
		}
	}

	public static void InitializeUseriCloudPreference()
	{
	}

	public static void UpdateFedoraAchievements()
	{
		float percent = AllAchievementCheckUpdates.HatCollector();
		if (PerryGameServices.m_perryAchievements != null && PerryGameServices.m_perryAchievements.ContainsKey("Hat Collector"))
		{
			PerryGameServices.m_perryAchievements["Hat Collector"].UpdateProgress(percent);
		}
	}

	public static void UpdateTokenAchievements()
	{
		float percent = AllAchievementCheckUpdates.BigMoney();
		if (PerryGameServices.m_perryAchievements != null && PerryGameServices.m_perryAchievements.ContainsKey("Big Money"))
		{
			PerryGameServices.m_perryAchievements["Big Money"].UpdateProgress(percent);
		}
	}

	public static int GetTotalRoundScore()
	{
		return HighestAllTimeScore = m_roundScore + m_roundScoreBonus;
	}

	public static bool IsHighscoresLoaded()
	{
		if (m_perryLeaderboards == null || m_menuHighScoreIndex < 0 || m_menuHighScoreIndex >= m_perryLeaderboards.Count)
		{
			return false;
		}
		if (m_perryLeaderboards[m_menuHighScoreIndex].TotalScores > 0)
		{
			return false;
		}
		return true;
	}

	public static void LoadHighScores()
	{
		Debug.Log("LoadHighScores == null " + (m_perryLeaderboards == null));
		if (m_perryLeaderboards != null)
		{
			if (m_menuHighScoreIndex < 0 || m_menuHighScoreIndex >= m_perryLeaderboards.Count)
			{
				Debug.Log("MenuHighScoreIndex > " + m_menuHighScoreIndex + " " + m_perryLeaderboards.Count);
			}
			else
			{
				Debug.Log("Attempting to Load HighScores for leaderboard: " + m_perryLeaderboards[m_menuHighScoreIndex].LeaderboardID);
				m_perryLeaderboards[m_menuHighScoreIndex].LoadScores(PerryLeaderboard.PerryLeaderboardTimeScope.AllTime);
			}
		}
	}

	public static void AddAndroidScore(PerryHighScore score)
	{
	}

	public static PerryHighScore GetMyPerryHighScore()
	{
		if (m_perryLeaderboards != null && m_perryLeaderboards.Count > 0)
		{
			using (List<PerryLeaderboard>.Enumerator enumerator = m_perryLeaderboards.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					PerryLeaderboard current = enumerator.Current;
					return current.GetMyPerryHighScore();
				}
			}
		}
		return null;
	}

	public static void AddFbScore(PerryHighScore phs)
	{
		Debug.Log("PLAYERDATA AddFbScore");
		if (m_perryLeaderboards == null || m_perryLeaderboards.Count == 0)
		{
			Debug.Log("we need to add a new leaderboard for FB");
			GPGLeaderboardMetadata gPGLeaderboardMetadata = new GPGLeaderboardMetadata();
			gPGLeaderboardMetadata.leaderboardId = "fbleaderboard";
			gPGLeaderboardMetadata.title = "Facebook Leaderboard";
			PerryLeaderboard item = new PerryLeaderboard(gPGLeaderboardMetadata);
			if (m_perryLeaderboards == null)
			{
				m_perryLeaderboards = new List<PerryLeaderboard>();
			}
			m_perryLeaderboards.Add(item);
		}
		if (m_menuHighScoreIndex < m_perryLeaderboards.Count)
		{
			m_perryLeaderboards[m_menuHighScoreIndex].AddFbScore(phs);
		}
	}

	private static void CheckForGadgetUpgradeAchievements()
	{
		float percent = AllAchievementCheckUpdates.GoesToEleven();
		float percent2 = AllAchievementCheckUpdates.ToTheNines();
		if (PerryGameServices.m_perryAchievements != null)
		{
			if (PerryGameServices.m_perryAchievements.ContainsKey("Goes to Eleven"))
			{
				PerryGameServices.m_perryAchievements["Goes to Eleven"].UpdateProgress(percent);
			}
			if (PerryGameServices.m_perryAchievements.ContainsKey("To the Nines"))
			{
				PerryGameServices.m_perryAchievements["To the Nines"].UpdateProgress(percent2);
			}
		}
	}

	public static void SetInitialTokenAndFedoraValues()
	{
		playerFedoras = 0;
		playerTokens = 250;
		PerryiCloudManager.The.SetItem(m_tokenKey, playerTokens);
		PerryiCloudManager.The.SetItem(m_fedoraKey, playerFedoras);
	}

	public static void LoadPersistentData()
	{
		playerTokens = PerryiCloudManager.The.GetIntItem(m_tokenKey, PerryiCloudManager.RetrieveType.Natural);
		playerFedoras = PerryiCloudManager.The.GetIntItem(m_fedoraKey, PerryiCloudManager.RetrieveType.Natural);
		if (PerryiCloudManager.The.HasKey(m_highestAllTimeScoreStr))
		{
			m_highestAllTimeScore = PerryiCloudManager.The.GetIntItem(m_highestAllTimeScoreStr, PerryiCloudManager.RetrieveType.Max);
		}
		if (PerryiCloudManager.The.HasKey(m_RoundDoofenschmirtzEncounterCountStr))
		{
			m_RoundDoofenschmirtzEncounterCount = PerryiCloudManager.The.GetIntItem(m_RoundDoofenschmirtzEncounterCountStr, PerryiCloudManager.RetrieveType.Natural);
		}
		if (PerryiCloudManager.The.HasKey(m_RoundBalloonyEncounterCountStr))
		{
			m_RoundBalloonyEncounterCount = PerryiCloudManager.The.GetIntItem(m_RoundBalloonyEncounterCountStr, PerryiCloudManager.RetrieveType.Natural);
		}
		if (PerryiCloudManager.The.HasKey(m_allTimeBossDefeatsStr))
		{
			m_allTimeBossDefeats = PerryiCloudManager.The.GetIntItem(m_allTimeBossDefeatsStr, PerryiCloudManager.RetrieveType.Max);
		}
		if (PerryiCloudManager.The.HasKey(m_allTimeWindowsBrokenStr))
		{
			m_allTimeWindowsBroken = PerryiCloudManager.The.GetIntItem(m_allTimeWindowsBrokenStr, PerryiCloudManager.RetrieveType.Max);
		}
		if (PerryiCloudManager.The.HasKey(m_allTimeTokensStr))
		{
			m_allTimeTokens = PerryiCloudManager.The.GetIntItem(m_allTimeTokensStr, PerryiCloudManager.RetrieveType.Max);
		}
		if (PerryiCloudManager.The.HasKey(m_allTimePowerupsStr))
		{
			m_allTimePowerups = PerryiCloudManager.The.GetIntItem(m_allTimePowerupsStr, PerryiCloudManager.RetrieveType.Max);
		}
		if (PerryiCloudManager.The.HasKey(m_allTimeFedorasStr))
		{
			m_allTimeFedoras = PerryiCloudManager.The.GetIntItem(m_allTimeFedorasStr, PerryiCloudManager.RetrieveType.Max);
		}
		if (PerryiCloudManager.The.HasKey(m_allTimeMagnetizersStr))
		{
			m_allTimeMagnetizers = PerryiCloudManager.The.GetIntItem(m_allTimeMagnetizersStr, PerryiCloudManager.RetrieveType.Max);
		}
		if (PerryiCloudManager.The.HasKey(m_allTimeEaglePowerUpCountStr))
		{
			m_allTimeEaglePowerUpCount = PerryiCloudManager.The.GetIntItem(m_allTimeEaglePowerUpCountStr, PerryiCloudManager.RetrieveType.Max);
		}
		if (PerryiCloudManager.The.HasKey(m_allTimeScoreMultipliersStr))
		{
			m_allTimeScoreMultipliers = PerryiCloudManager.The.GetIntItem(m_allTimeScoreMultipliersStr, PerryiCloudManager.RetrieveType.Max);
		}
		if (PerryiCloudManager.The.HasKey(m_AllTimeMetersStr))
		{
			m_AllTimeMeters = PerryiCloudManager.The.GetIntItem(m_AllTimeMetersStr, PerryiCloudManager.RetrieveType.Max);
		}
		if (PerryiCloudManager.The.HasKey(m_AllTimeScoreStr))
		{
			m_AllTimeScore = PerryiCloudManager.The.GetIntItem(m_AllTimeScoreStr, PerryiCloudManager.RetrieveType.Max);
		}
		if (PerryiCloudManager.The.HasKey(m_eagleUpgradeTimeStr))
		{
			m_eagleUpgradeTime = PerryiCloudManager.The.GetFloatItem(m_eagleUpgradeTimeStr);
		}
		if (PerryiCloudManager.The.HasKey(m_invulnerabilityUpgradeTimeStr))
		{
			m_invulnerabilityUpgradeTime = PerryiCloudManager.The.GetFloatItem(m_invulnerabilityUpgradeTimeStr);
		}
		if (PerryiCloudManager.The.HasKey(m_magnetUpgradeTimeStr))
		{
			m_magnetUpgradeTime = PerryiCloudManager.The.GetFloatItem(m_magnetUpgradeTimeStr);
		}
		if (PerryiCloudManager.The.HasKey(m_scoreMultUpgradeTimeStr))
		{
			m_scoreMultUpgradeTime = PerryiCloudManager.The.GetFloatItem(m_scoreMultUpgradeTimeStr);
		}
		if (PerryiCloudManager.The.HasKey(m_powerUpFreqChanceStr))
		{
			m_powerUpFreqChance = PerryiCloudManager.The.GetFloatItem(m_powerUpFreqChanceStr);
		}
		if (PerryiCloudManager.The.HasKey(m_jumpStartsStr))
		{
			JumpStarts = PerryiCloudManager.The.GetIntItem(m_jumpStartsStr, PerryiCloudManager.RetrieveType.Max);
		}
		if (PerryiCloudManager.The.HasKey(m_totalUsedJumpStartsStr))
		{
			m_totalUsedJumpStarts = PerryiCloudManager.The.GetIntItem(m_totalUsedJumpStartsStr, PerryiCloudManager.RetrieveType.Natural);
		}
		if (PerryiCloudManager.The.HasKey(m_allTimeDuckySeenCountStr))
		{
			m_allTimeDuckySeenCount = PerryiCloudManager.The.GetIntItem(m_allTimeDuckySeenCountStr, PerryiCloudManager.RetrieveType.Max);
		}
		m_coinDuplicatorInator = PerryiCloudManager.The.GetBoolItem(m_duplicatorinatorKey, PerryiCloudManager.RetrieveType.Max);
		m_hasWaterWeapon = PerryiCloudManager.The.GetBoolItem(m_hasWaterWeaponKey, PerryiCloudManager.RetrieveType.Max);
		m_hasFireWeapon = PerryiCloudManager.The.GetBoolItem(m_hasFireWeaponKey, PerryiCloudManager.RetrieveType.Max);
		m_hasElectricWeapon = PerryiCloudManager.The.GetBoolItem(m_hasElectricWeaponKey, PerryiCloudManager.RetrieveType.Max);
		m_hasPinWeapon = PerryiCloudManager.The.GetBoolItem(m_hasPinShooterKey, PerryiCloudManager.RetrieveType.Max);
		m_babyUnlocked = PerryiCloudManager.The.GetBoolItem(m_babyUnlockedStr, PerryiCloudManager.RetrieveType.Max);
		m_babyFedoraPercent = PerryiCloudManager.The.GetFloatItem(m_babyFedoraPercentStr);
		m_HasBabyBeenSeen = PerryiCloudManager.The.GetBoolItem(m_HasBabyBeenSeenStr, PerryiCloudManager.RetrieveType.Max);
		m_momoUnlocked = PerryiCloudManager.The.GetBoolItem(m_momoUnlockedStr, PerryiCloudManager.RetrieveType.Max);
		if (PerryiCloudManager.The.HasKey(m_momoUpgradeLevelStr))
		{
			m_momoUpgradeLevel = PerryiCloudManager.The.GetIntItem(m_momoUpgradeLevelStr, PerryiCloudManager.RetrieveType.Max);
		}
		m_HasMomoBeenSeen = PerryiCloudManager.The.GetBoolItem(m_HasMomoBeenSeenStr, PerryiCloudManager.RetrieveType.Max);
		if (PerryiCloudManager.The.HasKey(m_ShouldNotShowTutorialStr))
		{
			m_ShouldNotShowTutorial = PerryiCloudManager.The.GetBoolItem(m_ShouldNotShowTutorialStr, PerryiCloudManager.RetrieveType.Max);
		}
		if (PerryiCloudManager.The.HasKey(m_ShouldNotShowBossTutorialStr))
		{
			m_ShouldNotShowBossTutorial = PerryiCloudManager.The.GetBoolItem(m_ShouldNotShowBossTutorialStr, PerryiCloudManager.RetrieveType.Max);
		}
		if (PerryiCloudManager.The.HasKey(m_ShouldNotShowBossTutorialNoGadgetStr))
		{
			m_ShouldNotShowBossTutorialNoGadget = PerryiCloudManager.The.GetBoolItem(m_ShouldNotShowBossTutorialNoGadgetStr, PerryiCloudManager.RetrieveType.Max);
		}
		if (PerryiCloudManager.The.HasKey(m_MaxMissionLevelSeenByUserIndexStr))
		{
			m_MaxMissionLevelSeenByUserIndex = PerryiCloudManager.The.GetIntItem(m_MaxMissionLevelSeenByUserIndexStr, PerryiCloudManager.RetrieveType.Max);
		}
	}

	public static void SavePersistentData()
	{
		PerryiCloudManager.The.SetItem(PerryiCloudManager.DataStoredWithDeviceUIDKey, SystemInfo.deviceUniqueIdentifier);
		PerryiCloudManager.The.SetItem(m_tokenKey, playerTokens);
		PerryiCloudManager.The.SetItem(m_fedoraKey, playerFedoras);
		PerryiCloudManager.The.SetItem(m_highestAllTimeScoreStr, m_highestAllTimeScore);
		PerryiCloudManager.The.SetItem(m_RoundDoofenschmirtzEncounterCountStr, m_RoundDoofenschmirtzEncounterCount);
		PerryiCloudManager.The.SetItem(m_RoundBalloonyEncounterCountStr, m_RoundBalloonyEncounterCount);
		PerryiCloudManager.The.SetItem(m_allTimeBossDefeatsStr, m_allTimeBossDefeats);
		PerryiCloudManager.The.SetItem(m_allTimeWindowsBrokenStr, m_allTimeWindowsBroken);
		PerryiCloudManager.The.SetItem(m_allTimeTokensStr, m_allTimeTokens);
		PerryiCloudManager.The.SetItem(m_allTimePowerupsStr, m_allTimePowerups);
		PerryiCloudManager.The.SetItem(m_allTimeFedorasStr, m_allTimeFedoras);
		PerryiCloudManager.The.SetItem(m_allTimeMagnetizersStr, m_allTimeMagnetizers);
		PerryiCloudManager.The.SetItem(m_allTimeEaglePowerUpCountStr, m_allTimeEaglePowerUpCount);
		PerryiCloudManager.The.SetItem(m_allTimeScoreMultipliersStr, m_allTimeScoreMultipliers);
		PerryiCloudManager.The.SetItem(m_AllTimeMetersStr, m_AllTimeMeters);
		PerryiCloudManager.The.SetItem(m_AllTimeScoreStr, m_AllTimeScore);
		PerryiCloudManager.The.SetItem(m_eagleUpgradeTimeStr, m_eagleUpgradeTime);
		PerryiCloudManager.The.SetItem(m_invulnerabilityUpgradeTimeStr, m_invulnerabilityUpgradeTime);
		PerryiCloudManager.The.SetItem(m_magnetUpgradeTimeStr, m_magnetUpgradeTime);
		PerryiCloudManager.The.SetItem(m_scoreMultUpgradeTimeStr, m_scoreMultUpgradeTime);
		PerryiCloudManager.The.SetItem(m_powerUpFreqChanceStr, m_powerUpFreqChance);
		PerryiCloudManager.The.SetItem(m_jumpStartsStr, m_jumpStarts);
		PerryiCloudManager.The.SetItem(m_totalUsedJumpStartsStr, m_totalUsedJumpStarts);
		PerryiCloudManager.The.SetItem(m_allTimeDuckySeenCountStr, m_allTimeDuckySeenCount);
		PerryiCloudManager.The.SetItem(m_duplicatorinatorKey, m_coinDuplicatorInator);
		PerryiCloudManager.The.SetItem(m_hasWaterWeaponKey, m_hasWaterWeapon);
		PerryiCloudManager.The.SetItem(m_hasFireWeaponKey, m_hasFireWeapon);
		PerryiCloudManager.The.SetItem(m_hasElectricWeaponKey, m_hasElectricWeapon);
		PerryiCloudManager.The.SetItem(m_hasPinShooterKey, m_hasPinWeapon);
		PerryiCloudManager.The.SetItem(m_babyUnlockedStr, m_babyUnlocked);
		PerryiCloudManager.The.SetItem(m_babyFedoraPercentStr, m_babyFedoraPercent);
		PerryiCloudManager.The.SetItem(m_HasBabyBeenSeenStr, m_HasBabyBeenSeen);
		PerryiCloudManager.The.SetItem(m_momoUnlockedStr, m_momoUnlocked);
		PerryiCloudManager.The.SetItem(m_momoUpgradeLevelStr, m_momoUpgradeLevel);
		PerryiCloudManager.The.SetItem(m_HasMomoBeenSeenStr, m_HasMomoBeenSeen);
		PerryiCloudManager.The.SetItem(m_ShouldNotShowTutorialStr, m_ShouldNotShowTutorial);
		PerryiCloudManager.The.SetItem(m_ShouldNotShowBossTutorialStr, m_ShouldNotShowBossTutorial);
		PerryiCloudManager.The.SetItem(m_ShouldNotShowBossTutorialNoGadgetStr, m_ShouldNotShowBossTutorialNoGadget);
		PerryiCloudManager.The.SetItem(m_MaxMissionLevelSeenByUserIndexStr, m_MaxMissionLevelSeenByUserIndex);
	}

	public static void SaveRoundPersistentData()
	{
		PerryiCloudManager.The.SetItem(PerryiCloudManager.DataStoredWithDeviceUIDKey, SystemInfo.deviceUniqueIdentifier);
		PerryiCloudManager.The.SetItem(m_tokenKey, playerTokens);
		PerryiCloudManager.The.SetItem(m_fedoraKey, playerFedoras);
		PerryiCloudManager.The.SetItem(m_highestAllTimeScoreStr, m_highestAllTimeScore);
		PerryiCloudManager.The.SetItem(m_RoundDoofenschmirtzEncounterCountStr, m_RoundDoofenschmirtzEncounterCount);
		PerryiCloudManager.The.SetItem(m_RoundBalloonyEncounterCountStr, m_RoundBalloonyEncounterCount);
		PerryiCloudManager.The.SetItem(m_allTimeBossDefeatsStr, m_allTimeBossDefeats);
		PerryiCloudManager.The.SetItem(m_allTimeWindowsBrokenStr, m_allTimeWindowsBroken);
		PerryiCloudManager.The.SetItem(m_allTimeTokensStr, m_allTimeTokens);
		PerryiCloudManager.The.SetItem(m_allTimePowerupsStr, m_allTimePowerups);
		PerryiCloudManager.The.SetItem(m_allTimeFedorasStr, m_allTimeFedoras);
		PerryiCloudManager.The.SetItem(m_allTimeMagnetizersStr, m_allTimeMagnetizers);
		PerryiCloudManager.The.SetItem(m_allTimeEaglePowerUpCountStr, m_allTimeEaglePowerUpCount);
		PerryiCloudManager.The.SetItem(m_allTimeScoreMultipliersStr, m_allTimeScoreMultipliers);
		PerryiCloudManager.The.SetItem(m_AllTimeMetersStr, m_AllTimeMeters);
		PerryiCloudManager.The.SetItem(m_AllTimeScoreStr, m_AllTimeScore);
		PerryiCloudManager.The.SetItem(m_jumpStartsStr, m_jumpStarts);
		PerryiCloudManager.The.SetItem(m_totalUsedJumpStartsStr, m_totalUsedJumpStarts);
	}

	public static void SavePersistentDataInitialValues()
	{
		PerryiCloudManager.The.SetItemInitialValue(m_tokenKey, playerTokens);
		PerryiCloudManager.The.SetItemInitialValue(m_fedoraKey, playerFedoras);
		PerryiCloudManager.The.SetItemInitialValue(m_RoundDoofenschmirtzEncounterCountStr, m_RoundDoofenschmirtzEncounterCount);
		PerryiCloudManager.The.SetItemInitialValue(m_RoundBalloonyEncounterCountStr, m_RoundBalloonyEncounterCount);
		PerryiCloudManager.The.SetItemInitialValue(m_allTimeBossDefeatsStr, m_allTimeBossDefeats);
		PerryiCloudManager.The.SetItemInitialValue(m_allTimeWindowsBrokenStr, m_allTimeWindowsBroken);
		PerryiCloudManager.The.SetItemInitialValue(m_allTimeTokensStr, m_allTimeTokens);
		PerryiCloudManager.The.SetItemInitialValue(m_allTimePowerupsStr, m_allTimePowerups);
		PerryiCloudManager.The.SetItemInitialValue(m_allTimeFedorasStr, m_allTimeFedoras);
		PerryiCloudManager.The.SetItemInitialValue(m_allTimeMagnetizersStr, m_allTimeMagnetizers);
		PerryiCloudManager.The.SetItemInitialValue(m_allTimeEaglePowerUpCountStr, m_allTimeEaglePowerUpCount);
		PerryiCloudManager.The.SetItemInitialValue(m_allTimeScoreMultipliersStr, m_allTimeScoreMultipliers);
		PerryiCloudManager.The.SetItemInitialValue(m_AllTimeMetersStr, m_AllTimeMeters);
		PerryiCloudManager.The.SetItemInitialValue(m_AllTimeScoreStr, m_AllTimeScore);
		PerryiCloudManager.The.SetItemInitialValue(m_jumpStartsStr, m_jumpStarts);
		PerryiCloudManager.The.SetItemInitialValue(m_totalUsedJumpStartsStr, m_totalUsedJumpStarts);
		PerryiCloudManager.The.SetItemInitialValue(m_allTimeDuckySeenCountStr, m_allTimeDuckySeenCount);
	}

	public static void RemovePersistentDataInitialValues()
	{
		PerryiCloudManager.The.RemoveItemInitialValues(m_tokenKey);
		PerryiCloudManager.The.RemoveItemInitialValues(m_fedoraKey);
		PerryiCloudManager.The.RemoveItemInitialValues(m_RoundDoofenschmirtzEncounterCountStr);
		PerryiCloudManager.The.RemoveItemInitialValues(m_RoundBalloonyEncounterCountStr);
		PerryiCloudManager.The.RemoveItemInitialValues(m_allTimeBossDefeatsStr);
		PerryiCloudManager.The.RemoveItemInitialValues(m_allTimeWindowsBrokenStr);
		PerryiCloudManager.The.RemoveItemInitialValues(m_allTimeTokensStr);
		PerryiCloudManager.The.RemoveItemInitialValues(m_allTimePowerupsStr);
		PerryiCloudManager.The.RemoveItemInitialValues(m_allTimeFedorasStr);
		PerryiCloudManager.The.RemoveItemInitialValues(m_allTimeMagnetizersStr);
		PerryiCloudManager.The.RemoveItemInitialValues(m_allTimeEaglePowerUpCountStr);
		PerryiCloudManager.The.RemoveItemInitialValues(m_allTimeScoreMultipliersStr);
		PerryiCloudManager.The.RemoveItemInitialValues(m_AllTimeMetersStr);
		PerryiCloudManager.The.RemoveItemInitialValues(m_AllTimeScoreStr);
		PerryiCloudManager.The.RemoveItemInitialValues(m_jumpStartsStr);
		PerryiCloudManager.The.RemoveItemInitialValues(m_totalUsedJumpStartsStr);
		PerryiCloudManager.The.RemoveItemInitialValues(m_allTimeDuckySeenCountStr);
	}

	public static void SetNewMissions()
	{
		AllMissionData.UpdateMissionCompletes();
		ArrayList groupofMissionsForLevel = AllMissionData.GetGroupofMissionsForLevel(m_MaxMissionLevelSeenByUserIndex);
		if (groupofMissionsForLevel == null)
		{
			m_currentMissions = null;
			return;
		}
		if (m_currentMissions == null)
		{
			m_currentMissions = new ArrayList();
		}
		else
		{
			m_currentMissions.Clear();
		}
		for (int i = 0; i < groupofMissionsForLevel.Count; i++)
		{
			m_currentMissions.Add((Mission)groupofMissionsForLevel[i]);
		}
	}

	public static bool ReplaceMission(int index)
	{
		if (index < m_currentMissions.Count)
		{
			((Mission)m_currentMissions[index]).Completed = true;
			AllMissionData.UpdateMissionCompletes();
			ArrayList firstGroupOfIncompleteMissions = AllMissionData.GetFirstGroupOfIncompleteMissions();
			if (firstGroupOfIncompleteMissions == null)
			{
				m_currentMissions.RemoveAt(index);
				return false;
			}
			for (int i = 0; i < firstGroupOfIncompleteMissions.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < m_currentMissions.Count; j++)
				{
					if (flag)
					{
						break;
					}
					if (m_currentMissions[j] == firstGroupOfIncompleteMissions[i])
					{
						flag = true;
					}
				}
				if (!flag)
				{
					Debug.Log("Choosing this one... " + firstGroupOfIncompleteMissions[i]);
					m_currentMissions[index] = firstGroupOfIncompleteMissions[i];
					return true;
				}
			}
			m_currentMissions.RemoveAt(index);
			Debug.Log("No New Missions!");
		}
		return false;
	}

	public static void UpdateCheckMissions(bool forceUpdate = false)
	{
		if (m_currentMissions == null || (!forceUpdate && !GameManager.The.IsInGamePlay()))
		{
			return;
		}
		for (int i = 0; i < m_currentMissions.Count; i++)
		{
			Mission mission = (Mission)m_currentMissions[i];
			bool completed = mission.Completed;
			if (mission != null && !completed && ((Mission)m_currentMissions[i]).CheckInGameUpdate())
			{
				((Mission)m_currentMissions[i]).Completed = true;
				MainMenuEventManager.TriggerMissionNotification((Mission)m_currentMissions[i]);
				m_roundScoreBonus += ((Mission)m_currentMissions[i]).m_scoreBonus;
			}
		}
	}

	public static void SendCompletedMissionsToExternalStorage()
	{
		if (m_currentMissions == null)
		{
			return;
		}
		for (int i = 0; i < m_currentMissions.Count; i++)
		{
			Mission mission = (Mission)m_currentMissions[i];
			if (mission.Completed)
			{
				AllMissionData.StoreMissionData(mission);
			}
		}
	}

	public static void UpdateMissionProgressAchievements()
	{
		float percent = AllAchievementCheckUpdates.AgencyDarling();
		if (PerryGameServices.m_perryAchievements != null && PerryGameServices.m_perryAchievements.ContainsKey("Agency Darling"))
		{
			PerryGameServices.m_perryAchievements["Agency Darling"].UpdateProgress(percent);
		}
		percent = AllAchievementCheckUpdates.MissionUnlikely();
		if (PerryGameServices.m_perryAchievements != null && PerryGameServices.m_perryAchievements.ContainsKey("Mission Unlikely"))
		{
			PerryGameServices.m_perryAchievements["Mission Unlikely"].UpdateProgress(percent);
		}
	}

	public static bool BuyCurrentMission(Mission mission)
	{
		if (mission == null)
		{
			return false;
		}
		playerTokens -= mission.m_skipTokenCost;
		mission.Completed = true;
		AllMissionData.UpdateMissionCompletes();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("MissionName", mission.m_name);
		FlurryFacade.Instance.LogEvent("MissionPurchase", dictionary, false);
		return true;
	}

	public static bool BuyCurrentMission(int index)
	{
		if (index >= m_currentMissions.Count)
		{
			return false;
		}
		return BuyCurrentMission((Mission)m_currentMissions[index]);
	}

	public static Mission GetMissionData(int index)
	{
		if (index >= m_currentMissions.Count)
		{
			return null;
		}
		return (Mission)m_currentMissions[index];
	}

	public static int GetMissionCountPerGroup()
	{
		return 3;
	}
}

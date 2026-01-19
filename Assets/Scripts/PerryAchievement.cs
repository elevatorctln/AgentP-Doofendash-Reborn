using UnityEngine;

public class PerryAchievement
{
	public delegate float AchievementCheckUpdate();

	public static string[] iosAchievementIDs = new string[22]
	{
		"Majesco.Doofendash.BigMoney", "Majesco.Doofendash.PowerTrip", "Majesco.Doofendash.HatCollector", "Majesco.Doofendash.BigScore", "Majesco.Doofendash.AgencyDarling", "Majesco.Doofendash.MagneticPersonality", "Majesco.Doofendash.FlyingHigh", "Majesco.Doofendash.DoubleDown", "Majesco.Doofendash.Doofensmash", "Majesco.Doofendash.GoesToEleven",
		"Majesco.Doofendash.JumpStartYourEngines", "Majesco.Doofendash.GoingTheDistance", "Majesco.Doofendash.BiggerScore", "Majesco.Doofendash.ChihuahuaChasing", "Majesco.Doofendash.Pandamonium", "Majesco.Doofendash.TerrapinTime", "Majesco.Doofendash.DressedToThrill", "Majesco.Doofendash.MissionUnlikely", "Majesco.Doofendash.BossBeGone", "Majesco.Doofendash.SceneJumper",
		"Majesco.Doofendash.UtilityBelt", "Majesco.Doofendash.ToTheNines"
	};

	public static string[] m_googlePlayAchievementIDs = new string[22]
	{
		"CgkIxuHk6ekaEAIQAQ", "CgkIxuHk6ekaEAIQAg", "CgkIxuHk6ekaEAIQAw", "CgkIxuHk6ekaEAIQBA", "CgkIxuHk6ekaEAIQBQ", "CgkIxuHk6ekaEAIQBg", "CgkIxuHk6ekaEAIQBw", "CgkIxuHk6ekaEAIQCA", "CgkIxuHk6ekaEAIQCQ", "CgkIxuHk6ekaEAIQCg",
		"CgkIxuHk6ekaEAIQCw", "CgkIxuHk6ekaEAIQDA", "CgkIxuHk6ekaEAIQDQ", "CgkIxuHk6ekaEAIQDg", "CgkIxuHk6ekaEAIQDw", "CgkIxuHk6ekaEAIQEA", "CgkIxuHk6ekaEAIQEQ", "CgkIxuHk6ekaEAIQEg", "CgkIxuHk6ekaEAIQEw", "CgkIxuHk6ekaEAIQFA",
		"CgkIxuHk6ekaEAIQFQ", "CgkIxuHk6ekaEAIQFg"
	};

	private GPGAchievementMetadata m_googleAchievement;

	private float m_percentComplete;

	private bool m_unlocked;

	private AchievementCheckUpdate m_achievementCheckUpdateCallback;

	public string ID
	{
		get
		{
			return m_googleAchievement.achievementId;
		}
	}

	public bool IsHidden
	{
		get
		{
			return m_googleAchievement.state == 0;
		}
	}

	public bool IsCompleted
	{
		get
		{
			return m_percentComplete == 100f;
		}
	}

	public float PercentComplete
	{
		get
		{
			return m_percentComplete;
		}
		set
		{
			m_percentComplete = value;
		}
	}

	public bool IsUnlocked
	{
		get
		{
			return m_unlocked;
		}
		set
		{
			m_unlocked = value;
		}
	}

	public PerryAchievement(GPGAchievementMetadata achievement)
	{
		m_googleAchievement = achievement;
		SetCallbackFunction();
	}

	public void UpdateProgress(float percent)
	{
		if (!IsUnlocked)
		{
			PercentComplete = percent;
			if (percent >= 100f)
			{
				IsUnlocked = true;
				Debug.Log("Unlock " + m_googleAchievement.name + " percent " + percent);
				PerryGameServices.UnlockAchievement(ID);
			}
			else
			{
				Debug.Log("Unlock " + m_googleAchievement.name + " percent " + percent);
				PerryGameServices.UnlockAchievement(ID);
			}
		}
	}

	public float CheckInGameUpdate()
	{
		Debug.Log("CheckInGameUpdate " + m_googleAchievement.name);
		if (m_achievementCheckUpdateCallback != null)
		{
			return m_achievementCheckUpdateCallback();
		}
		Debug.Log("Achievement Check Update Function Callback not set!");
		return 0f;
	}

	public override string ToString()
	{
		return string.Format("Perry Achievement Item\nID: " + ID + "\nIsHidden: " + IsHidden + "\nIsCompleted: " + IsCompleted + "\nPercent Complete: " + PercentComplete);
	}

	public void SetCallbackFunction()
	{
		switch (ID)
		{
		case "Majesco.Doofendash.BigMoney":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.BigMoney;
			break;
		case "Majesco.Doofendash.PowerTrip":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.PowerTrip;
			break;
		case "Majesco.Doofendash.HatCollector":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.HatCollector;
			break;
		case "Majesco.Doofendash.BigScore":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.BigScore;
			break;
		case "Majesco.Doofendash.AgencyDarling":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.AgencyDarling;
			break;
		case "Majesco.Doofendash.MagneticPersonality":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.MagneticPersonality;
			break;
		case "Majesco.Doofendash.FlyingHigh":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.FlyingHigh;
			break;
		case "Majesco.Doofendash.DoubleDown":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.DoubleDown;
			break;
		case "Majesco.Doofendash.Doofensmash":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.Doofensmash;
			break;
		case "Majesco.Doofendash.GoesToEleven":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.GoesToEleven;
			break;
		case "Majesco.Doofendash.JumpStartYourEngines":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.JumpStartEngines;
			break;
		case "Majesco.Doofendash.PauperForTokens":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.PauperForTokens;
			break;
		case "Majesco.Doofendash.PowerStarved":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.PowerStarved;
			break;
		case "Majesco.Doofendash.ChihuahuaChasing":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.ChihuahuaChasing;
			break;
		case "Majesco.Doofendash.Pandamonium":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.Pandamonium;
			break;
		case "Majesco.Doofendash.TerrapinTime":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.TerrapinTime;
			break;
		case "Majesco.Doofendash.DressedToThrill":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.DressedToThrill;
			break;
		case "Majesco.Doofendash.MissionUnlikely":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.MissionUnlikely;
			break;
		case "Majesco.Doofendash.BossBeGone":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.BossBeGone;
			break;
		case "Majesco.Doofendash.SceneJumper":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.SceneJumper;
			break;
		case "Majesco.Doofendash.UtilityBelt":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.UtilityBelt;
			break;
		case "Majesco.Doofendash.ToTheNines":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.ToTheNines;
			break;
		case "Majesco.Doofendash.BiggerScore":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.BiggerScore;
			break;
		case "Majesco.Doofendash.GoingTheDistance":
			m_achievementCheckUpdateCallback = AllAchievementCheckUpdates.GoingTheDistance;
			break;
		}
	}
}

using System.Collections.Generic;
using UnityEngine;

public class PerryGameServices : MonoBehaviour
{
	private static int m_toastOffset = 50;

	private static string clientID = "921760772294.apps.googleusercontent.com";

	private static bool m_hasLoadedAchievements;

	private static bool m_hasInit;

	private static GPGPlayerInfo m_playerInfo;

	public static Dictionary<string, PerryAchievement> m_perryAchievements;

	public static bool ms_ShouldShowAchievementsOnAuthenticateSuccess;

	private static PerryGameServices m_the;

	public static PerryGameServices The
	{
		get
		{
			if (m_the == null)
			{
				Debug.Log("Trying to access PerryGameCenter before it is initialized");
			}
			return m_the;
		}
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		m_the = this;
	}

	private void Start()
	{
		Init();
		GPGManager.authenticationSucceededEvent += AuthenticationSucceededEventListener;
		GPGManager.authenticationFailedEvent += AuthenticationFailedListener;
	}

	private void OnDestroy()
	{
		GPGManager.authenticationSucceededEvent -= AuthenticationSucceededEventListener;
		GPGManager.authenticationFailedEvent -= AuthenticationFailedListener;
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	public static void Init()
	{
		m_hasInit = true;
		return;
	}

	public static void SetToastsLocation(GPGToastPlacement toastPlacement)
	{
		switch (toastPlacement)
		{
		case GPGToastPlacement.Bottom:
			PlayGameServices.setAchievementToastSettings(toastPlacement, m_toastOffset);
			break;
		case GPGToastPlacement.Top:
			PlayGameServices.setAchievementToastSettings(toastPlacement, m_toastOffset);
			break;
		case GPGToastPlacement.Center:
			PlayGameServices.setAchievementToastSettings(toastPlacement, 0);
			break;
		}
	}

	public static void AuthenticationSucceededEventListener(string userID)
	{
		if (ms_ShouldShowAchievementsOnAuthenticateSuccess)
		{
			ShowAchievements();
			ms_ShouldShowAchievementsOnAuthenticateSuccess = false;
		}
	}

	public static void AuthenticationFailedListener(string error)
	{
		ms_ShouldShowAchievementsOnAuthenticateSuccess = false;
		Debug.Log("GPG Auth error = " + error);
	}

	public static void AuthenticateAndShowAchievements()
	{
			Authenticate();
	}

	public static void Authenticate()
{
    Debug.Log("GPG Authenticate - STUBBED");
    // Just pretend we succeeded, I don't think this will ever be called anyways
    AuthenticationSucceededEventListener("stub_user");
}

	public static void SignOut()
	{
		PlayGameServices.signOut();
	}

	public static bool IsSignedIn()
	{
		return PlayGameServices.isSignedIn();
	}

	public static void GetPlayerInfo()
	{
		m_playerInfo = PlayGameServices.getLocalPlayerInfo();
		Debug.Log("GetPlayerInfo " + m_playerInfo);
		GetLeaderboardMetadata();
		GetAchievementMetadata();
	}

	public static void SetPlayerFaceUrl(string path)
	{
	}

	public static void GetAchievementMetadata()
	{
		Debug.Log("GetAchievementMetadata");
		List<GPGAchievementMetadata> allAchievementMetadata = PlayGameServices.getAllAchievementMetadata();
		foreach (GPGAchievementMetadata item in allAchievementMetadata)
		{
			Debug.Log(item.name + " id " + item.achievementId + " progress " + item.progress);
			PerryAchievement perryAchievement = new PerryAchievement(item);
			perryAchievement.PercentComplete = (float)item.progress;
			m_perryAchievements.Add(item.name, perryAchievement);
		}
	}

	public static void LoadAchievements()
	{
		if (!m_hasLoadedAchievements)
		{
			ShowAchievements();
		}
	}

	public static void ShowAchievements()
	{
		Debug.Log("Show Achievements");
		PlayGameServices.showAchievements();
	}

	public static void IncrementAchievements(string achievementID)
	{
		PlayGameServices.incrementAchievement(achievementID, 2);
	}

	public static void UnlockAchievement(string achievementID)
	{
		PlayGameServices.unlockAchievement(achievementID);
	}

	public static void ShowLeaderboard(string leaderboardID, GPGLeaderboardTimeScope timeScope)
	{
		PlayGameServices.showLeaderboard(leaderboardID, timeScope);
	}

	public static void ShowAllLeaderboards()
	{
		PlayGameServices.showLeaderboards();
	}

	public static void SubmitScore(string leaderboardID, long score)
	{
		PlayGameServices.submitScore(leaderboardID, score);
	}

	public static void LoadRawScoreData(string leaderboardID, GPGLeaderboardTimeScope timeScope)
	{
		Debug.Log("LoadRawScoreData " + leaderboardID);
		PlayGameServices.loadScoresForLeaderboard(leaderboardID, timeScope, false, false);
	}

	public static void loadScoresSucceeded(List<GPGScore> scores)
	{
		Debug.Log("loadScoresSucceeded " + scores.Count);
		foreach (GPGScore score in scores)
		{
			Debug.Log("convert to perryScore " + score.displayName);
			PerryHighScore perryHighScore = new PerryHighScore(score);
			if (m_playerInfo.playerId == score.playerId)
			{
				perryHighScore.IsMe = true;
			}
			PlayerData.AddAndroidScore(perryHighScore);
		}
	}

	public static void GetLeaderboardMetadata()
	{
		List<GPGLeaderboardMetadata> allLeaderboardMetadata = PlayGameServices.getAllLeaderboardMetadata();
		foreach (GPGLeaderboardMetadata item2 in allLeaderboardMetadata)
		{
			PerryLeaderboard item = new PerryLeaderboard(item2);
			if (PlayerData.PerryLeaderboards == null)
			{
				PlayerData.PerryLeaderboards = new List<PerryLeaderboard>();
			}
			PlayerData.PerryLeaderboards.Add(item);
		}
		PlayerData.LoadHighScores();
	}
}

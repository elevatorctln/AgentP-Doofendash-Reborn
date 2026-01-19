using System.Diagnostics;
using PlayHaven;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayHavenController : MonoBehaviour
{
	public enum PlayHavenPlacement
	{
		MoreGames = 0,
		WindowShow = 1,
		ChildLock = 2,
		AppStart = 3,
		LevelEndWin1 = 4,
		LevelEndWin2 = 5,
		LevelEndWin3 = 6,
		CumulativeWins5 = 7,
		CumulativeWins10 = 8,
		CumulativeWins15 = 9,
		LevelEndNoUpgrade = 10,
		UserLevelUp3 = 11,
		UserLevelUp6 = 12,
		UserLevelUp9 = 13,
		UserLevelUp12 = 14,
		UserLevelUp15 = 15,
		IAP = 16,
		SoftPurchase = 17,
		StoreFromMain = 18,
		StoreFromLevelEnd = 19,
		UserFacebookShare = 20,
		UserTwitterShare = 21,
		VisitLeaderboardFromMain = 22,
		VisitLeaderboardFromLevelEnd = 23,
		UserZeroTokens = 24,
		UserZeroFedoras = 25,
		UserTokenHording = 26,
		UserFedoraHording = 27,
		LevelEnd = 28,
		AppExit = 29
	}

	private static bool m_show_PlayHaven_placements = true;

	private static string[] m_placements = new string[30]
	{
		"more_games", "window_shopper", "childlock_test", "app_start", "levelend_win1", "levelend_win2", "levelend_win3", "cumulative_win5", "cumulative_win10", "cumulative_win15",
		"levelend_noupgrade", "user_levelup3", "user_levelup6", "user_levelup9", "user_levelup12", "user_levelup15", "iap_purchased", "soft_purchase", "store_frommain", "store_fromlevelend",
		"user_facebook_share", "user_twitter_share", "visit_leaderboard_frommain", "visit_leaderboard_fromlevelend", "user_zero_tokens", "user_zero_fedoras", "user_token_hording", "user_fedora_hording", "level_end", "app_exit"
	};

	private PurchasableItem pi;

	public static bool ShowPlayHavenPlacements
	{
		get
		{
			return m_show_PlayHaven_placements;
		}
		set
		{
		}
	}

	private void OnEnable()
	{

	}

	private void OnDisable()
	{

	}

	private void Awake()
	{
	}

	public static void ContentRequest(PlayHavenPlacement placement)
	{
		Debug.Log("PlayHaven ContentRequest was called, we don't want this.");
		return;
	}

	private void HandlePlayHavenManagerinstanceOnDismissContent(int requestId, DismissType dismissType)
	{
	}

	private void HandlePlayHavenManagerinstanceOnDidDisplayContent(int requestId)
	{
	}

	private void HandlePlayHavenManagerinstanceOnPurchasePresented(int requestId, Purchase purchase)
	{
		
	}

	private void OnPurchaseVGP(bool storeLoaded)
	{
		return;
	}

	private void HandlePlayHavenManagerinstanceOnRewardGiven(int requestId, Reward reward)
	{
		return;
	}

	private void HandlePlayHavenManagerinstanceOnSuccessCancelRequest(int requestId)
	{
	}

	private void HandlePlayHavenManagerinstanceOnRequestCompleted(int requestId)
	{
	}

	private void HandlePlayHavenManagerinstanceOnErrorOpenRequest(int requestId, Error error)
	{
	}
}

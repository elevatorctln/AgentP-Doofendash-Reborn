using System;
using System.Collections;
using UnityEngine;

namespace PlayHaven
{

	[AddComponentMenu("PlayHaven/Manager")]
	public class PlayHavenManager : MonoBehaviour, IPlayHavenListener
	{
		public enum WhenToOpen
		{
			Awake = 0,
			Start = 1,
			Manual = 2
		}

		public enum WhenToGetNotifications
		{
			Disabled = 0,
			Awake = 1,
			Start = 2,
			OnEnable = 3,
			Manual = 4,
			Poll = 5
		}

		public delegate void CancelRequestHandler(int requestId);

		public const int NO_HASH_CODE = 0;

		public static string KEY_LAUNCH_COUNT = "playhaven-launch-count";

		public string token = string.Empty;

		[HideInInspector]
		public bool lockToken;

		public string secret = string.Empty;

		[HideInInspector]
		public bool lockSecret;

		public string tokenAndroid = string.Empty;

		[HideInInspector]
		public bool lockTokenAndroid;

		public string secretAndroid = string.Empty;

		[HideInInspector]
		public bool lockSecretAndroid;

		public bool doNotDestroyOnLoad = true;

		public bool defaultShowsOverlayImmediately;

		public bool maskShowsOverlayImmediately;

		public WhenToOpen whenToSendOpen;

		public WhenToGetNotifications whenToGetNotifications = WhenToGetNotifications.Disabled;

		public string badgeMoreGamesPlacement = "more_games";

		public float notificationPollDelay = 1f;

		public float notificationPollRate = 15f;

		public bool cancelAllOnLevelLoad;

		public int suppressContentRequestsForLaunches;

		public string[] suppressedPlacements;

		public string[] suppressionExceptions;

		public bool showContentUnitsInEditor = true;

		public bool maskNetworkReachable;

		public bool isAndroidSupported;

		private static PlayHavenManager _instance;

		public static PlayHavenManager instance
		{
			get
			{
				if (_instance == null)
				{
					GameObject go = new GameObject("PlayHavenManager_Stub");
					_instance = go.AddComponent<PlayHavenManager>();
					DontDestroyOnLoad(go);
				}
				return _instance;
			}
		}

		public string CustomUDID { get; set; }

		public bool OptOutStatus
		{
			get { return true; }
			set { }
		}

		public static bool IsAndroidSupported
		{
			get { return false; }
		}

		public string Badge
		{
			get { return string.Empty; }
		}

		public event RequestCompletedHandler OnRequestCompleted;
		public event BadgeUpdateHandler OnBadgeUpdate;
		public event RewardTriggerHandler OnRewardGiven;
		public event PurchasePresentedTriggerHandler OnPurchasePresented;
		public event SimpleDismissHandler OnDismissCrossPromotionWidget;
		public event DismissHandler OnDismissContent;
		public event WillDisplayContentHandler OnWillDisplayContent;
		public event DidDisplayContentHandler OnDidDisplayContent;
		public event SuccessHandler OnSuccessOpenRequest;
		public event SuccessHandler OnSuccessPreloadRequest;
		public event ErrorHandler OnErrorOpenRequest;
		public event ErrorHandler OnErrorCrossPromotionWidget;
		public event ErrorHandler OnErrorContentRequest;
		public event ErrorHandler OnErrorMetadataRequest;
		public event CancelRequestHandler OnSuccessCancelRequest;
		public event CancelRequestHandler OnErrorCancelRequest;

		private void Awake()
		{
			if (_instance == null)
			{
				_instance = this;
				if (doNotDestroyOnLoad)
				{
					DontDestroyOnLoad(gameObject);
				}
			}
			else if (_instance != this)
			{
				Destroy(gameObject);
			}
		}

		public int OpenNotification(string customUDID) { return 0; }
		public int OpenNotification() { return 0; }
		public void CancelAllPendingRequests() { }
		public void ProductPurchaseResolutionRequest(PurchaseResolution resolution) { }
		public void ProductPurchaseTrackingRequest(Purchase purchase, PurchaseResolution resolution) { }
		public int ContentPreloadRequest(string placement) { return 0; }
		public int ContentRequest(string placement) { return 0; }
		public int ContentRequest(string placement, bool showsOverlayImmediately) { return 0; }
		public int ShowCrossPromotionWidget() { return 0; }
		public int BadgeRequest(string placement) { return 0; }
		public int BadgeRequest() { return 0; }
		public void PollForBadgeRequests() { }
		public bool IsPlacementSuppressed(string placement) { return true; }
		public void ClearBadge() { }

		public void NotifyRequestCompleted(int requestId) { }
		public void NotifyOpenSuccess(int requestId) { }
		public void NotifyOpenError(int requestId, Error error) { }
		public void NotifyWillDisplayContent(int requestId) { }
		public void NotifyDidDisplayContent(int requestId) { }
		public void NotifyPreloadSuccess(int requestId) { }
		public void NotifyBadgeUpdate(int requestId, string badge) { }
		public void NotifyRewardGiven(int requestId, Reward reward) { }
		public void NotifyPurchasePresented(int requestId, Purchase purchase) { }
		public void NotifyCrossPromotionWidgetDismissed() { }
		public void NotifyCrossPromotionWidgetError(int requestId, Error error) { }
		public void NotifyContentDismissed(int requestId, DismissType dismissType) { }
		public void NotifyContentError(int requestId, Error error) { }
		public void NotifyMetaDataError(int requestId, Error error) { }
		public void HandleNativeEvent(string json) { }
		public void RequestCancelSuccess(string hashCodeString) { }
		public void RequestCancelFailed(string hashCodeString) { }
	}
}

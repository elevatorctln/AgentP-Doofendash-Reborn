using System.Collections;
using UnityEngine;

namespace PlayHaven
{
	[AddComponentMenu("PlayHaven/Content Requester")]
	public class PlayHavenContentRequester : MonoBehaviour
	{
		public enum WhenToRequest
		{
			Awake = 0,
			Start = 1,
			OnEnable = 2,
			OnDisable = 3,
			Manual = 4
		}

		public enum InternetConnectivity
		{
			WiFiOnly = 0,
			CarrierNetworkOnly = 1,
			WiFiAndCarrierNetwork = 2,
			Always = 100
		}

		public enum MessageType
		{
			None = 0,
			Send = 1,
			Broadcast = 2,
			Upwards = 3
		}

		public enum ExhaustedAction
		{
			None = 0,
			DestroySelf = 1,
			DestroyGameObject = 2,
			DestroyRoot = 3
		}

		public WhenToRequest whenToRequest = WhenToRequest.Manual;

		public string placement = string.Empty;

		public WhenToRequest prefetch = WhenToRequest.Manual;

		public InternetConnectivity connectionForPrefetch;

		public bool refetchWhenUsed;

		public bool showsOverlayImmediately;

		public bool rewardMayBeDelivered;

		public MessageType rewardMessageType = MessageType.Broadcast;

		public bool useDefaultTestReward;

		public string defaultTestRewardName = string.Empty;

		public int defaultTestRewardQuantity = 1;

		public float requestDelay;

		public bool limitedUse;

		public int maxUses;

		public ExhaustedAction exhaustAction;

		private PlayHavenManager playHaven;

		private bool exhausted;

		private int uses;

		private int contentRequestId;

		private int prefetchRequestId;

		private bool requestIsInProgress;

		private bool prefetchIsInProgress;

		private bool refetch;

		private void Awake()
		{
			return;
		}

		private void OnEnable()
		{
			return;
		}

		private void OnDisable()
		{
			return;
		}

		private void OnDestroy()
		{
			return;
		}

		private void Start()
		{
			return;
		}

		private void RequestPlayHavenContent()
		{
			return;
		}

		public void PreFetch()
		{
			return;
		}

		private void HandleManagerOnSuccessPreloadRequest(int requestId)
		{
			return;
		}

		public void Request()
		{
			return;
		}

		public void Request(bool refetch)
		{
			return;
		}

		private void Exhaust()
		{
			return;
		}

		private void HandlePlayHavenManagerOnDismissContent(int hashCode, DismissType dismissType)
		{
			return;
		}

		public void HandlePlayHavenManagerOnRewardGiven(int hashCode, Reward reward)
		{
			return;
		}
	}
}

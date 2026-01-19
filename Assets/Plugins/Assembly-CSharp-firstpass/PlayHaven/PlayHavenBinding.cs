using System;
using System.Collections;
using UnityEngine;

namespace PlayHaven
{
	public class PlayHavenBinding : IDisposable
	{
		public enum RequestType
		{
			Open = 0,
			Metadata = 1,
			Content = 2,
			Preload = 3,
			CrossPromotionWidget = 4
		}

		public interface IPlayHavenRequest
		{
			int HashCode { get; }
			event GeneralHandler OnWillDisplay;
			event GeneralHandler OnDidDisplay;
			event SuccessHandler OnSuccess;
			event ErrorHandler OnError;
			event DismissHandler OnDismiss;
			event RewardHandler OnReward;
			event PurchaseHandler OnPurchasePresented;
			void Send();
			void Send(bool showsOverlayImmediately);
			void TriggerEvent(string eventName, object eventData);
		}

		public class OpenRequest : IPlayHavenRequest
		{
			public int HashCode { get { return GetHashCode(); } }
			public event SuccessHandler OnSuccess;
			public event ErrorHandler OnError;
			public event DismissHandler OnDismiss;
			public event RewardHandler OnReward;
			public event PurchaseHandler OnPurchasePresented;
			public event GeneralHandler OnWillDisplay;
			public event GeneralHandler OnDidDisplay;
			public OpenRequest() { }
			public OpenRequest(string customUDID) { }
			public void Send() { }
			public void Send(bool showsOverlayImmediately) { }
			public void TriggerEvent(string eventName, object eventData) { }
		}

		public class MetadataRequest : IPlayHavenRequest
		{
			public int HashCode { get { return GetHashCode(); } }
			public event SuccessHandler OnSuccess;
			public event ErrorHandler OnError;
			public event DismissHandler OnDismiss;
			public event RewardHandler OnReward;
			public event PurchaseHandler OnPurchasePresented;
			public event GeneralHandler OnWillDisplay;
			public event GeneralHandler OnDidDisplay;
			public MetadataRequest(string placement) { }
			public void Send() { }
			public void Send(bool showsOverlayImmediately) { }
			public void TriggerEvent(string eventName, object eventData) { }
		}

		public class ContentRequest : IPlayHavenRequest
		{
			public int HashCode { get { return GetHashCode(); } }
			public event SuccessHandler OnSuccess;
			public event ErrorHandler OnError;
			public event DismissHandler OnDismiss;
			public event RewardHandler OnReward;
			public event PurchaseHandler OnPurchasePresented;
			public event GeneralHandler OnWillDisplay;
			public event GeneralHandler OnDidDisplay;
			public ContentRequest(string placement) { }
			public void Send() { }
			public void Send(bool showsOverlayImmediately) { }
			public void TriggerEvent(string eventName, object eventData) { }
		}

		public class ContentPreloadRequest : IPlayHavenRequest
		{
			public int HashCode { get { return GetHashCode(); } }
			public event SuccessHandler OnSuccess;
			public event ErrorHandler OnError;
			public event DismissHandler OnDismiss;
			public event RewardHandler OnReward;
			public event PurchaseHandler OnPurchasePresented;
			public event GeneralHandler OnWillDisplay;
			public event GeneralHandler OnDidDisplay;
			public ContentPreloadRequest(string placement) { }
			public void Send() { }
			public void Send(bool showsOverlayImmediately) { }
			public void TriggerEvent(string eventName, object eventData) { }
		}

		public delegate void SuccessHandler(IPlayHavenRequest request, object responseData);
		public delegate void ErrorHandler(IPlayHavenRequest request, object errorData);
		public delegate void RewardHandler(IPlayHavenRequest request, object rewardData);
		public delegate void PurchaseHandler(IPlayHavenRequest request, object purchaseData);
		public delegate void DismissHandler(IPlayHavenRequest request, object dismissData);
		public delegate void GeneralHandler(IPlayHavenRequest request);

		public static string token;
		public static string secret;
		public static IPlayHavenListener listener;

		public static bool OptOutStatus { get; set; }

		public void Dispose() { }
		public static void Initialize() { }
		public static void SetKeys(string token, string secret) { }
		public static int Open() { return 0; }
		public static int Open(string customUDID) { return 0; }
		public static void CancelRequest(int requestId) { }
		public static void RegisterActivityForTracking(bool register) { }
		public static void SendProductPurchaseResolution(PurchaseResolution resolution) { }
		public static void SendIAPTrackingRequest(Purchase purchase, PurchaseResolution resolution) { }
		public static int SendRequest(RequestType type, string placement) { return 0; }
		public static int SendRequest(RequestType type, string placement, bool showsOverlayImmediately) { return 0; }
		public static IPlayHavenRequest GetRequestWithHash(int hash) { return null; }
		public static void ClearRequestWithHash(int hash) { }
	}
}

using UnityEngine;

public class TokenAdsAgent
{
	public enum PopupContentType
	{
		VIDEOs = 0,
		APP_INSTALLs = 1
	}

	private static TokenAdsListener mTokenAdsListener;

	private static bool isPluginVerbose;

	private static AndroidJavaObject bridge;

	private static void InitializeListener()
	{
		Debug.Log("InitializeListener()..");
		TokenAdsListener.isListenerVerbose = isPluginVerbose;
		Debug.Log("InitTokenAds: looking for an exiting instance of the listener");
		if (!mTokenAdsListener)
		{
			mTokenAdsListener = Object.FindObjectOfType(typeof(TokenAdsListener)) as TokenAdsListener;
		}
		if (!mTokenAdsListener)
		{
			GameObject gameObject = new GameObject();
			gameObject.AddComponent(typeof(TokenAdsListener));
			mTokenAdsListener = gameObject.GetComponent(typeof(TokenAdsListener)) as TokenAdsListener;
		}
		mTokenAdsListener.gameObject.name = "TokenAdsListenerGameObject";
		Debug.Log("InitializeListener() - DONE");
	}

	private static void InitializeBridge()
	{
		Debug.Log("InitializeBridge()..");
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.tokenads.unity.TokenAdsUnityBridge");
		bridge = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[1] { "TokenAdsListenerGameObject" });
		Debug.Log("InitializeBridge() - DONE");
	}

	public static void Init(string iOSAppId, string iOSSecretKey, string androidAppId, string androidSecretKey, string clientId, string itemId)
	{
		InitializeListener();
		InitializeBridge();
		Init_Internal(androidAppId, androidSecretKey, clientId, itemId);
	}

	private static void Init_Internal(string appId, string secretKey, string clientId, string itemId)
	{
		if (isPluginVerbose)
		{
			Debug.Log("Init(): appId=" + appId + " clientId=" + clientId + " itemId=" + itemId);
		}
		bridge.Call("init", appId, secretKey, clientId, itemId);
		if (isPluginVerbose)
		{
			Debug.Log("exiting Init()");
		}
	}

	public static void StartAgent()
	{
		if (isPluginVerbose)
		{
			Debug.Log("entering StartAgent()");
		}
		bridge.Call("start");
		if (isPluginVerbose)
		{
			Debug.Log("exiting StartAgent()");
		}
	}

	public static void StopAgent()
	{
		if (isPluginVerbose)
		{
			Debug.Log("entering StopAgent()");
		}
		bridge.Call("stop");
		if (isPluginVerbose)
		{
			Debug.Log("exiting StopAgent()");
		}
	}

	public static void ShowOffers()
	{
		if (isPluginVerbose)
		{
			Debug.Log("entering ShowOffers()");
		}
		bridge.Call("showOffers");
		if (isPluginVerbose)
		{
			Debug.Log("exiting ShowOffers()");
		}
	}

	public static void ShowPopup()
	{
		if (isPluginVerbose)
		{
			Debug.Log("entering ShowPopup()");
		}
		bridge.Call("showPopup");
		if (isPluginVerbose)
		{
			Debug.Log("exiting ShowPopup()");
		}
	}

	public static void ShowPopupInRect(int left, int top, int right, int bottom)
	{
		if (isPluginVerbose)
		{
			Debug.Log("entering ShowPopupInRect(), left: " + left + ", top: " + top + ", right: " + right + ", height: " + bottom);
		}
		bridge.Call("showPopupInRect", left, top, right, bottom);
		if (isPluginVerbose)
		{
			Debug.Log("exiting ShowPopupInRect()");
		}
	}

	public static void SetPopupContent(PopupContentType type)
	{
		if (isPluginVerbose)
		{
			Debug.Log("entering SetPopupContent(): " + type);
		}
		bridge.Call("setPopupContent", (int)type);
		if (isPluginVerbose)
		{
			Debug.Log("exiting SetPopupContent()");
		}
	}

	public static void QueryPoints()
	{
		if (isPluginVerbose)
		{
			Debug.Log("entering QueryPoints()");
		}
		bridge.Call("queryPoints");
		if (isPluginVerbose)
		{
			Debug.Log("exiting QueryPoints()");
		}
	}

	public static void QueryOffersCount()
	{
		if (isPluginVerbose)
		{
			Debug.Log("entering QueryOffersCount()");
		}
		bridge.Call("queryOffersCount");
		if (isPluginVerbose)
		{
			Debug.Log("exiting QueryOffersCount()");
		}
	}

	public static void QueryPopupCount()
	{
		if (isPluginVerbose)
		{
			Debug.Log("entering QueryPopupCount()");
		}
		bridge.Call("queryPopupCount");
		if (isPluginVerbose)
		{
			Debug.Log("exiting QueryPopupCount()");
		}
	}

	public static void SetVerbose(bool verbose)
	{
		Debug.Log("SetVerbose(): " + verbose);
		isPluginVerbose = verbose;
		TokenAdsListener.isListenerVerbose = verbose;
		bridge.Call("setVerbose", verbose);
		if (isPluginVerbose)
		{
			Debug.Log("exiting SetVerbose()");
		}
	}

	public static void SetCustomParams(string customParams)
	{
		if (isPluginVerbose)
		{
			Debug.Log("entering SetCustomParams(): " + customParams);
		}
		bridge.Call("setCustomParams", customParams);
		if (isPluginVerbose)
		{
			Debug.Log("exiting SetCustomParams()");
		}
	}

	public static bool IsStarted()
	{
		return bridge.Call<bool>("isStarted", new object[0]);
	}

	public static string GetSdkVersion()
	{
		return bridge.Call<string>("getSdkVersion", new object[0]);
	}

	public static string GetClientId()
	{
		return bridge.Call<string>("getClientId", new object[0]);
	}

	public static string GetAppId()
	{
		return bridge.Call<string>("getAppId", new object[0]);
	}

	public static string GetItemId()
	{
		return bridge.Call<string>("getItemId", new object[0]);
	}

	public static string GetCustomParams()
	{
		return bridge.Call<string>("getCustomParams", new object[0]);
	}

	public static string GetUuid()
	{
		return bridge.Call<string>("getUuid", new object[0]);
	}
}

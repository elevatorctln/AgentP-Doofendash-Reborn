using System;
using UnityEngine;

public class AGSClient : MonoBehaviour
{
	private static AndroidJavaObjectFrameManagerWrapper JavaObject;

	private static readonly string PROXY_CLASS_NAME;

	private static bool IsReady;

	public static event Action ServiceReadyEvent;

	public static event Action<string> ServiceNotReadyEvent;

	static AGSClient()
	{
		PROXY_CLASS_NAME = "com.amazon.ags.api.unity.AmazonGamesClientProxyImpl";
		JavaObject = new AndroidJavaObjectFrameManagerWrapper();
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(PROXY_CLASS_NAME))
		{
			JavaObject.setAndroidJavaObject(androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]));
		}
	}

	public static void Init()
	{
		Init(false, false, false);
	}

	public static void Init(bool supportsLeaderboards, bool supportsAchievements, bool supportsWhispersync)
	{
		JavaObject.Call("init", supportsLeaderboards, supportsAchievements, supportsWhispersync);
	}

	public static void SetPopUpLocation(GameCirclePopupLocation location)
	{
		JavaObject.Call("setPopUpLocation", location.ToString());
	}

	public static void ServiceReady(string empty)
	{
		Debug.Log("Client GameCircle - Service is ready");
		IsReady = true;
		if (AGSClient.ServiceReadyEvent != null)
		{
			AGSClient.ServiceReadyEvent();
		}
	}

	public static bool IsServiceReady()
	{
		return IsReady;
	}

	public static void release()
	{
		JavaObject.Call("release");
	}

	public static void ServiceNotReady(string param)
	{
		IsReady = false;
		if (AGSClient.ServiceNotReadyEvent != null)
		{
			AGSClient.ServiceNotReadyEvent(param);
		}
	}
}

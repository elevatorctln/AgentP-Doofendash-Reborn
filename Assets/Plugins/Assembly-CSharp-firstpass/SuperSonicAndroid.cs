using System.Collections.Generic;
using SuperSonicMiniJSON;
using UnityEngine;

public class SuperSonicAndroid
{
	private static AndroidJavaObject _plugin;

	static SuperSonicAndroid()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.supersonic.SuperSonicPlugin"))
		{
			_plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
		}
	}

	public static void initializeBrandConnnect(string applicationKey, string applicationUserId, bool shouldGetLocation, Dictionary<string, object> additionalParameters)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			string text = ((additionalParameters == null) ? string.Empty : Json.Serialize(additionalParameters));
			_plugin.Call("initializeBrandConnnect", applicationKey ?? string.Empty, applicationUserId ?? string.Empty, shouldGetLocation, text ?? string.Empty);
		}
	}

	public static void showBrandConnect()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("showBrandConnect");
		}
	}

	public static void showOfferWall(string applicationKey, string applicationUserId, bool shouldGetLocation, Dictionary<string, object> additionalParameters)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			string text = ((additionalParameters == null) ? string.Empty : Json.Serialize(additionalParameters));
			_plugin.Call("showOfferWall", applicationKey ?? string.Empty, applicationUserId ?? string.Empty, shouldGetLocation, text ?? string.Empty);
		}
	}

	public static void release()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("release");
		}
	}
}

using System;
using System.Collections;
using UnityEngine;

public class AGSProfilesClient : MonoBehaviour
{
	private static AndroidJavaObjectFrameManagerWrapper JavaObject;

	private static readonly string PROXY_CLASS_NAME = "com.amazon.ags.api.unity.ProfilesClientProxyImpl";

	public static event Action<AGSProfile> PlayerAliasReceivedEvent;

	public static event Action<string> PlayerAliasFailedEvent;

	public static void Initialize()
	{
		JavaObject = new AndroidJavaObjectFrameManagerWrapper();
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(PROXY_CLASS_NAME))
		{
			JavaObject.setAndroidJavaObject(androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]));
		}
	}

	public static void RequestLocalPlayerProfile()
	{
		JavaObject.Call("requestLocalPlayerProfile");
	}

	public static void PlayerAliasReceived(string json)
	{
		if (AGSProfilesClient.PlayerAliasReceivedEvent != null)
		{
			Hashtable ht = json.hashtableFromJson();
			AGSProfilesClient.PlayerAliasReceivedEvent(AGSProfile.fromHashtable(ht));
		}
	}

	public static void PlayerAliasFailed(string json)
	{
		if (AGSProfilesClient.PlayerAliasFailedEvent != null)
		{
			Hashtable ht = json.hashtableFromJson();
			string stringFromHashtable = GetStringFromHashtable(ht, "error");
			AGSProfilesClient.PlayerAliasFailedEvent(stringFromHashtable);
		}
	}

	private static string GetStringFromHashtable(Hashtable ht, string key)
	{
		string result = null;
		if (ht.Contains(key))
		{
			result = ht[key].ToString();
		}
		return result;
	}
}

using System.Collections.Generic;
using UnityEngine;

public class FlurryFacade : MonoBehaviour
{
	private string API_Key;

	private static FlurryFacade _instance;

	public static FlurryFacade Instance
	{
		get
		{
			if (_instance == null)
			{
				GameObject gameObject = new GameObject("FlurryFacade");
				gameObject.AddComponent<FlurryFacade>().Init();
			}
			return _instance;
		}
	}

	private void OnApplicationQuit()
	{
		#if UNITY_ANDROID
		FlurryAndroid.onEndSession();
		#endif
	}

	public void Init()
	{
		if (!(_instance != null))
		{
			_instance = this;
			API_Key = "SZK7VN6MJN88V8B36C3W";
			#if UNITY_ANDROID
			FlurryAndroid.setUserID(SystemInfo.deviceUniqueIdentifier);
			FlurryAndroid.onStartSession(API_Key, false, false);
			#endif
		}
	}

	public void LogEvent(string eventName, bool isTimed = false)
	{
		#if UNITY_ANDROID
		FlurryAndroid.logEvent(eventName);
		#endif
	}

	public void LogEvent(string eventName, Dictionary<string, string> parameters, bool isTimed)
	{
		#if UNITY_ANDROID
		FlurryAndroid.logEvent(eventName, parameters);
		#endif
	}

	private string DictionaryToString(Dictionary<string, string> dict)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, string> item in dict)
		{
			list.Add(string.Format("{0}||{1}", item.Key, item.Value));
		}
		return string.Join("|||", list.ToArray());
	}
}

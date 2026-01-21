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

	}

	public void Init()
	{
		if (!(_instance != null))
		{
			_instance = this;
			API_Key = "SZK7VN6MJN88V8B36C3W";
		}
	}

	public void LogEvent(string eventName, bool isTimed = false)
	{
	}

	public void LogEvent(string eventName, Dictionary<string, string> parameters, bool isTimed)
	{
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

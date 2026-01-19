using System;
using System.Collections.Generic;
using SuperSonicMiniJSON;
using UnityEngine;

public class SuperSonicAndroidManager : MonoBehaviour
{
	public static event Action noMoreOffersEvent;

	public static event Action<Dictionary<string, object>> onAdFinishedEvent;

	public static event Action<Dictionary<string, object>> onInitFailEvent;

	public static event Action<Dictionary<string, object>> onInitSuccessEvent;

	private void Awake()
	{
		base.gameObject.name = "SuperSonicAndroidManager";
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void noMoreOffers(string empty)
	{
		if (SuperSonicAndroidManager.noMoreOffersEvent != null)
		{
			SuperSonicAndroidManager.noMoreOffersEvent();
		}
	}

	public void onAdFinished(string json)
	{
		if (SuperSonicAndroidManager.onAdFinishedEvent != null)
		{
			SuperSonicAndroidManager.onAdFinishedEvent(Json.Deserialize(json) as Dictionary<string, object>);
		}
	}

	public void onInitFail(string json)
	{
		if (SuperSonicAndroidManager.onInitFailEvent != null)
		{
			SuperSonicAndroidManager.onInitFailEvent(Json.Deserialize(json) as Dictionary<string, object>);
		}
	}

	public void onInitSuccess(string json)
	{
		if (SuperSonicAndroidManager.onInitSuccessEvent != null)
		{
			SuperSonicAndroidManager.onInitSuccessEvent(Json.Deserialize(json) as Dictionary<string, object>);
		}
	}
}

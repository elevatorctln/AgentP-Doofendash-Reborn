using System;
using UnityEngine;

public class AgeGateAndroid : MonoBehaviour
{
	public static event Action<string> ageGateDateEnteredEvent;

	public static event Action ageGateCanceledEvent;

	public static event Action alertButtonClickedEvent;

	public void ageGateDateEntered(string age)
	{
		if (AgeGateAndroid.ageGateDateEnteredEvent != null)
		{
			AgeGateAndroid.ageGateDateEnteredEvent(age);
		}
	}

	public void ageGateCanceled(string message)
	{
		if (AgeGateAndroid.ageGateCanceledEvent != null)
		{
			AgeGateAndroid.ageGateCanceledEvent();
		}
	}

	public void alertButtonClicked(string message)
	{
		if (AgeGateAndroid.alertButtonClickedEvent != null)
		{
			AgeGateAndroid.alertButtonClickedEvent();
		}
	}

	public static void ShowAlert(string title, string message, string dismissText)
	{
		// Only valid on an actual Android device build.
		#if !UNITY_ANDROID || UNITY_EDITOR
		Debug.Log("AgeGateAndroid.ShowAlert called outside Android - ignoring");
		return;
		#endif
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("majesco.Dialogs.Dialogs", androidJavaObject);
				androidJavaObject2.Call("showDialog", title, message, dismissText);
			}
		}
	}

	public static void ShowDatePicker(string title, string message, string dismissText)
	{
		// Only valid on an actual Android device build.
		#if !UNITY_ANDROID || UNITY_EDITOR
		Debug.Log("AgeGateAndroid.ShowDatePicker called outside Android - ignoring");
		return;
		#endif
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("majesco.Dialogs.Dialogs", androidJavaObject);
				androidJavaObject2.Call("showDateDialog", title, message, dismissText);
			}
		}
	}
}

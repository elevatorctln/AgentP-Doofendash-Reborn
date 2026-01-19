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
		
	}

	public static void ShowDatePicker(string title, string message, string dismissText)
	{
		
	}
}

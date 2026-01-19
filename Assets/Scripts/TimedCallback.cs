using System.Collections;
using UnityEngine;

public static class TimedCallback
{
	public delegate void CallBackInTime();

	public delegate void CallBackInTimeBool(bool val);

	public static IEnumerator SetCallBackTime(float duration, CallBackInTime callbackFunc)
	{
		yield return new WaitForSeconds(duration);
		callbackFunc();
	}

	public static IEnumerator SetCallBackTime(float duration, CallBackInTimeBool callbackFunc, bool val)
	{
		yield return new WaitForSeconds(duration);
		callbackFunc(val);
	}
}

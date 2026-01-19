using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DebugInGame : MonoBehaviour
{
	[Conditional("USE_DEBUG_INGAME")]
	public static void DebugLog(string debugMessage)
	{
		Debug.Log(debugMessage);
	}

	[Conditional("USE_DEBUG_INGAME")]
	public static void DebugLogError(string debugMessage)
	{
		Debug.LogError(debugMessage);
	}
}

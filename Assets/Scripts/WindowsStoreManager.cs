using Prime31;
using Reign;
using UnityEngine;

public class WindowsStoreManager : MonoBehaviourGUI
{
	private InAppPurchaseID[] inAppIDs;

	public InAppPurchaseID[] GetInAppIDs()
	{
		return inAppIDs;
	}

	private void Start()
	{
	}

	private void log(string log)
	{
		Debug.Log(log);
	}
}

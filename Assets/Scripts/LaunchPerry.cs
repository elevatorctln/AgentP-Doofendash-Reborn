using UnityEngine;

public class LaunchPerry : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider trigger)
	{
		GameEventManager.TriggerLaunchPerry();
	}
}

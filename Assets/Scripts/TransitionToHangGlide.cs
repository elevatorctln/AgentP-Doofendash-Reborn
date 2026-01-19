using UnityEngine;

public class TransitionToHangGlide : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider trigger)
	{
		GameEventManager.TriggerHangGlideStart();
	}
}

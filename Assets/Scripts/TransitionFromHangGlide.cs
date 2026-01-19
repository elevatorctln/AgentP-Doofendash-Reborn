using UnityEngine;

public class TransitionFromHangGlide : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider trigger)
	{
		GameEventManager.TriggerHangGlideEnd();
	}
}

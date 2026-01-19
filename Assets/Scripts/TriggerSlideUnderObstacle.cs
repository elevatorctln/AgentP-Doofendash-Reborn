using UnityEngine;

public class TriggerSlideUnderObstacle : MonoBehaviour
{
	private void OnTriggerEnter(Collider trigger)
	{
		PlayerData.RoundPlayerSlidesUnderObstacles++;
	}
}

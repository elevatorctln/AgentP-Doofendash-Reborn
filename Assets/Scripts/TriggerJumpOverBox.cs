using UnityEngine;

public class TriggerJumpOverBox : MonoBehaviour
{
	private void OnTriggerEnter(Collider trigger)
	{
		PlayerData.RoundJumpOverBoxesCount++;
	}
}

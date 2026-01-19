using UnityEngine;

public class TriggerJumpOverNormBot : MonoBehaviour
{
	private void OnTriggerEnter(Collider trigger)
	{
		PlayerData.RoundJumpOverBotsCount++;
	}
}

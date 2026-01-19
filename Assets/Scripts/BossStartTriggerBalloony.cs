using UnityEngine;

public class BossStartTriggerBalloony : MonoBehaviour
{
	private void OnTriggerEnter(Collider trigger)
	{
		GameEventManager.TriggerBossStart(MiniGameManager.BossType.Balloony);
	}
}

using UnityEngine;

public class BossStartTriggerDoofenCruiser : MonoBehaviour
{
	private void OnTriggerEnter(Collider trigger)
	{
		GameEventManager.TriggerBossStart(MiniGameManager.BossType.DoofenCruiser);
	}
}

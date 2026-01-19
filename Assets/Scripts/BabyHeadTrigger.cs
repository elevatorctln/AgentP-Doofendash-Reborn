using UnityEngine;

public class BabyHeadTrigger : MonoBehaviour
{
	public BabyHead m_BabyHeadToSpawn;

	public static bool ms_IsBabyPlaying;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void InstantiateBaby()
	{
		if (!ms_IsBabyPlaying)
		{
			BabyHead babyHead = CacheManager.The().Spawn(m_BabyHeadToSpawn);
			babyHead.ResetBabyHead();
			ms_IsBabyPlaying = true;
			PlayerData.RoundBabyHeadSeenCount++;
		}
	}

	private void OnTriggerEnter(Collider trigger)
	{
		InstantiateBaby();
	}
}

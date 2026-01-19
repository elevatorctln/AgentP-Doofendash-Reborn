using UnityEngine;

public class DuckyMomoTrigger : MonoBehaviour
{
	public DuckyMomo m_DuckyToSpawn;

	private DuckyMomo m_DuckySpawned;

	public static bool ms_IsDuckyPlaying;

	public static bool ms_IsDuckyDisabled;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void DestroyDucky()
	{
		if (m_DuckySpawned != null)
		{
			CacheManager.The().Unspawn(m_DuckySpawned.gameObject);
			ms_IsDuckyPlaying = false;
		}
	}

	private void InstantiateDucky()
	{
		if (!ms_IsDuckyPlaying)
		{
			DuckyMomo.ResetCutOffDucky();
			m_DuckySpawned = CacheManager.The().Spawn(m_DuckyToSpawn);
			m_DuckySpawned.ResetDucky();
			ms_IsDuckyPlaying = true;
			PlayerData.RoundDuckySeenCount++;
			PlayerData.AllTimeDuckySeenCount++;
		}
	}

	private void OnTriggerEnter(Collider trigger)
	{
		InstantiateDucky();
	}
}

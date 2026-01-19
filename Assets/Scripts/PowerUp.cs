using UnityEngine;

public class PowerUp : MonoBehaviour
{
	private static float m_CollideWithRunnerDist = 10f;

	private static float ms_RotateYDelta = 180f;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void UpdateCollisionWithRunner()
	{
		if (DidCollideWithRunner())
		{
			OnRunnerCollide();
		}
	}

	private bool DidCollideWithRunner()
	{
		if (Runner.The() == null)
		{
			return false;
		}
		if ((Runner.The().transform.position - base.transform.position).sqrMagnitude < m_CollideWithRunnerDist * m_CollideWithRunnerDist)
		{
			return true;
		}
		return false;
	}

	public virtual void OnRunnerCollide()
	{
		if (BabyHeadTrigger.ms_IsBabyPlaying)
		{
			PlayerData.RoundHasCollectedItemFromBabyHead = true;
		}
	}

	public virtual void Update()
	{
		base.transform.Rotate(0f, ms_RotateYDelta * Time.deltaTime, 0f);
		UpdateCollisionWithRunner();
	}

	private void OnTriggerEnter(Collider trigger)
	{
	}

	public static PowerUp Spawn(PowerUp spawnObj, Transform parent, float laneOffset, int lane)
	{
		return CollectibleOps.Spawn(spawnObj, parent, laneOffset, lane);
	}

	public static PowerUp Spawn(PowerUp spawnObj, Transform parent, float laneOffset, int lane, float offsetY, float offsetZ)
	{
		return CollectibleOps.Spawn(spawnObj, parent, laneOffset, lane, offsetY, offsetZ);
	}
}

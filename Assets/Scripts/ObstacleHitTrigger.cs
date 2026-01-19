using UnityEngine;

public class ObstacleHitTrigger : MonoBehaviour
{
	public enum RunnerResponse
	{
		Collide = 0,
		Fall = 1
	}

	public RunnerResponse m_RunnerResponse;

	public bool m_ShouldUseDeathPlacementDistance;

	public float m_DeathPlacementDistance;

	private void Start()
	{
	}

	private void Update()
	{
	}
}

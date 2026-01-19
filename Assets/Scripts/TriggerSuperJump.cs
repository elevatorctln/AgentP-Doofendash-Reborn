using UnityEngine;

public class TriggerSuperJump : MonoBehaviour
{
	public float m_JumpBoostFactor = 1.55f;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider trigger)
	{
		Runner.The().m_IsInSuperJumpTrigger = true;
		Runner.The().m_JumpBoostFactor = m_JumpBoostFactor;
	}

	private void OnTriggerExit(Collider trigger)
	{
		Runner.The().m_IsInSuperJumpTrigger = false;
	}
}

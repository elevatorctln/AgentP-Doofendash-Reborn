using UnityEngine;

public class TreeFallTrigger : Obstacle
{
	private Animator m_anim;

	private void Start()
	{
		m_anim = GetComponent<Animator>();
	}

	private void OnTriggerEnter(Collider trigger)
	{
		TriggerFallAnim();
	}

	private void TriggerFallAnim()
	{
		m_anim.SetBool("Falling", true);
	}
}

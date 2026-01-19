using UnityEngine;

public class TriggerDoofenCruiserAttack : Obstacle
{
	public DoofenCruiser.Attacks m_DoofenCruiserAttack;

	private bool m_BeenTriggered;

	private void OnTriggerEnter(Collider trigger)
	{
		if (!m_BeenTriggered)
		{
			m_BeenTriggered = true;
			DoofenCruiser.The().TriggerAction(m_DoofenCruiserAttack);
		}
	}

	public override void ResetObstacle()
	{
		m_BeenTriggered = false;
		base.ResetObstacle();
	}
}

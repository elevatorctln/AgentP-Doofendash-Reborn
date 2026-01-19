using UnityEngine;

public class InvincibilityPowerup : PowerUp
{
	private void Start()
	{
	}

	public override void Update()
	{
		base.Update();
	}

	public override void OnRunnerCollide()
	{
		base.OnRunnerCollide();
		GameEventManager.TriggerInvincibility();
		base.gameObject.SetActive(false);
	}

	private void OnTriggerEnter(Collider trigger)
	{
	}
}

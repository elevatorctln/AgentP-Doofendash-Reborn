public class PowerUpFeather : PowerUp
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
		if (Runner.The() != null && !Runner.The().IsInEagleState())
		{
			GameEventManager.TriggerPowerUpFeatherOn();
			base.gameObject.SetActive(false);
		}
	}
}

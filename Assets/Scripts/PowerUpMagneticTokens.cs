public class PowerUpMagneticTokens : PowerUp
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
		Token.StartMagneticTokenState();
		GameEventManager.TriggerPowerUpMagnetOn();
		base.gameObject.SetActive(false);
	}
}

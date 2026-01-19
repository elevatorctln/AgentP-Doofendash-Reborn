public class DoubleScoreMultiplier : PowerUp
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
		GameEventManager.TriggerScoreMultiplier();
		base.gameObject.SetActive(false);
	}
}

public class Fedora : PowerUp
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
		base.gameObject.SetActive(false);
		GameEventManager.TriggerFedoraHit(1);
		GameManager.The.PlayClip(AudioClipFiles.UIPOPUP);
		if (BabyHeadTrigger.ms_IsBabyPlaying)
		{
			PlayerData.RoundHasCollectedFedoraFromBabyHead = true;
		}
	}
}

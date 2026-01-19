public class WreckingBallObstacle : Obstacle
{
	public override void ResetObstacle()
	{
		base.ResetObstacle();
		if (m_SoundClipName.Length > 1)
		{
			GameManager.The.PlayClip(AudioClipFiles.ROOFTOPFOLDER + m_SoundClipName);
		}
	}
}

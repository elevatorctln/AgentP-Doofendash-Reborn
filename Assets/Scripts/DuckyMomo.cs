using UnityEngine;

public class DuckyMomo : MonoBehaviour
{
	public float m_DuckyDuration = 13f;

	private float m_DuckyTimeCur;

	private static bool ms_ShouldCutOffDucky;

	private void Start()
	{
		m_DuckyTimeCur = 0f;
		GameManager.The.PlayMusic(AudioClipFiles.DUCKYTHEME);
	}

	public void ResetDucky()
	{
		m_DuckyTimeCur = 0f;
	}

	public static void TriggerCutOffDucky()
	{
		ms_ShouldCutOffDucky = true;
	}

	public static void ResetCutOffDucky()
	{
		ms_ShouldCutOffDucky = false;
	}

	private void Update()
	{
		if (ms_ShouldCutOffDucky)
		{
			CacheManager.The().Unspawn(base.gameObject);
			DuckyMomoTrigger.ms_IsDuckyPlaying = false;
			ms_ShouldCutOffDucky = false;
		}
		if (GameManager.The.IsGamePaused())
		{
			return;
		}
		m_DuckyTimeCur += Time.deltaTime;
		if (!(Runner.The() != null))
		{
			return;
		}
		base.transform.position = Runner.The().CalcHalfwayCamTargetPosition();
		base.transform.rotation = Runner.The().transform.rotation;
		if (m_DuckyTimeCur > m_DuckyDuration)
		{
			CacheManager.The().Unspawn(base.gameObject);
			DuckyMomoTrigger.ms_IsDuckyPlaying = false;
			switch (MiniGameManager.The().RetrieveMiniGameCur())
			{
			case MiniGameManager.MiniGameNames.Rooftop:
				GameManager.The.PlayMusic(AudioClipFiles.ROOFTOPTHEME);
				break;
			case MiniGameManager.MiniGameNames.HangGliding:
				GameManager.The.PlayMusic(AudioClipFiles.HANGLIDERTHEME);
				break;
			case MiniGameManager.MiniGameNames.EvilInterior:
				GameManager.The.PlayMusic(AudioClipFiles.LAIRTHEME);
				break;
			}
		}
	}
}

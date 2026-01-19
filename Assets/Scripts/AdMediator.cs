using UnityEngine;

public class AdMediator : MonoBehaviour
{
	private const int FREQUENCY = 2;

	public BaseAdProvider m_videoAd;

	public BaseAdProvider m_interstitialAd;

	private int m_iteration;

	private static AdMediator m_instance;

	public static AdMediator Instance
	{
		get
		{
			return m_instance;
		}
	}

	public void Awake()
	{
		if (m_instance != null)
		{
			Object.DestroyImmediate(base.gameObject);
		}
		else
		{
			m_instance = this;
		}
	}

	public void ShowInterstitial()
	{

	}

	public void ShowVideo(string currencyReward)
	{

	}

	public bool HasVideo()
	{
		return m_videoAd != null && m_videoAd.HasAd();
	}
}

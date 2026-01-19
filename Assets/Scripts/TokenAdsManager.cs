using UnityEngine;

public class TokenAdsManager : MonoBehaviour
{
	private const string IOS_APP_ID = "14950";

	private const string IOS_SECRET_KEY = "f576cdb27f09274d";

	private const string ANDROID_APP_ID = "14846";

	private const string ANDROID_SERCRET_KEY = "bc28e824cf8f3250";

	private const string CLIENT_ID = "";

	private const string ITEM_ID = "";

	private const string CUSTOM_PARAMS = "";

	public void Awake()
	{
	}

	public void Start()
	{
		return;
	}

	public void OnEnable()
	{
		return;
	}

	public void OnDisable()
	{
		return;
	}

	public void ShowOffers()
	{
		return;
	}

	public void Update()
	{
	}

	public void OnReceivePoints(string val)
	{
		Debug.Log("TokenAds reward called, figure out why that happened");
		return;
	}

	public void OnError(string val)
	{
		Debug.Log("TokenAd :: Error :: str = " + val);
	}

	public void OnAgentStarted()
	{
	}

	public void OnAgentStopped()
	{
	}

	public void OnReceivedOffersCount(int count)
	{
	}

	public void OnReceivedPopupCount(int count)
	{
	}
}

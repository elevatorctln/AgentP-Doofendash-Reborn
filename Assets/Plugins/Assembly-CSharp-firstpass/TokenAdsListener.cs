using UnityEngine;

public class TokenAdsListener : MonoBehaviour
{
	public delegate void OnErrorDelegate(string error);

	public delegate void OnReceivePointsDelegate(string points);

	public delegate void OnShowOffersDelegate();

	public delegate void OnCloseOffersDelegate();

	public delegate void OnShowPopupDelegate();

	public delegate void OnClosePopupDelegate();

	public delegate void OnOffersCountDelegate(int count);

	public delegate void OnPopupCountDelegate(int count);

	public delegate void OnStartedDelegate();

	public delegate void OnStoppedDelegate();

	public const string GAME_OBJECT_NAME = "TokenAdsListenerGameObject";

	public static bool isListenerVerbose;

	private static TokenAdsListener instance;

	public static event OnErrorDelegate onErrorListener;

	public static event OnReceivePointsDelegate onReceivePointsListener;

	public static event OnShowOffersDelegate onShowOffersListener;

	public static event OnCloseOffersDelegate onCloseOffersListener;

	public static event OnShowPopupDelegate onShowPopupListener;

	public static event OnClosePopupDelegate onClosePopupListener;

	public static event OnOffersCountDelegate onOffersCountListener;

	public static event OnPopupCountDelegate onPopupCountListener;

	public static event OnStartedDelegate onStartedListener;

	public static event OnStoppedDelegate onStoppedListener;

	private TokenAdsListener()
	{
		if (instance != null)
		{
			if (isListenerVerbose)
			{
				Debug.Log("TokenAdsListener init: Already has instance");
			}
			Object.Destroy(this);
		}
		else
		{
			if (isListenerVerbose)
			{
				Debug.Log("TokenAdsListener init: Don't have instance");
			}
			instance = this;
		}
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void onError(string error)
	{
		if (isListenerVerbose)
		{
			Debug.Log("EVENT: onError: " + error);
		}
		if (TokenAdsListener.onErrorListener != null)
		{
			TokenAdsListener.onErrorListener(error);
		}
	}

	private void onReceivePoints(string points)
	{
		if (isListenerVerbose)
		{
			Debug.Log("EVENT: onReceivePoints " + points);
		}
		if (TokenAdsListener.onReceivePointsListener != null)
		{
			TokenAdsListener.onReceivePointsListener(points);
		}
	}

	private void onShowOffers()
	{
		if (isListenerVerbose)
		{
			Debug.Log("EVENT: onShowOffers");
		}
		if (TokenAdsListener.onShowOffersListener != null)
		{
			TokenAdsListener.onShowOffersListener();
		}
	}

	private void onCloseOffers()
	{
		if (isListenerVerbose)
		{
			Debug.Log("EVENT: onCloseOffers");
		}
		if (TokenAdsListener.onCloseOffersListener != null)
		{
			TokenAdsListener.onCloseOffersListener();
		}
	}

	private void onShowPopup()
	{
		if (isListenerVerbose)
		{
			Debug.Log("EVENT: onShowPopup");
		}
		if (TokenAdsListener.onShowPopupListener != null)
		{
			TokenAdsListener.onShowPopupListener();
		}
	}

	private void onClosePopup()
	{
		if (isListenerVerbose)
		{
			Debug.Log("EVENT: onClosePopup");
		}
		if (TokenAdsListener.onClosePopupListener != null)
		{
			TokenAdsListener.onClosePopupListener();
		}
	}

	private void onOffersCount(string count)
	{
		if (isListenerVerbose)
		{
			Debug.Log("EVENT: onOffersCount: " + count);
		}
		if (TokenAdsListener.onOffersCountListener != null)
		{
			TokenAdsListener.onOffersCountListener(int.Parse(count));
		}
	}

	private void onPopupCount(string count)
	{
		if (isListenerVerbose)
		{
			Debug.Log("EVENT: onPopupCount: " + count);
		}
		if (TokenAdsListener.onPopupCountListener != null)
		{
			TokenAdsListener.onPopupCountListener(int.Parse(count));
		}
	}

	private void onStarted()
	{
		if (isListenerVerbose)
		{
			Debug.Log("EVENT: onStarted");
		}
		if (TokenAdsListener.onStartedListener != null)
		{
			TokenAdsListener.onStartedListener();
		}
	}

	private void onStopped()
	{
		if (isListenerVerbose)
		{
			Debug.Log("EVENT: onStopped");
		}
		if (TokenAdsListener.onStoppedListener != null)
		{
			TokenAdsListener.onStoppedListener();
		}
	}
}

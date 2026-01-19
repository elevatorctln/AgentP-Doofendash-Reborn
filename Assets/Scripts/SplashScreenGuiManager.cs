using UnityEngine;

public sealed class SplashScreenGuiManager : MonoBehaviour
{
	public UIToolkit splashToolkit;

	private static SplashScreenGuiManager m_the;

	public static SplashScreenGuiManager The
	{
		get
		{
			return m_the;
		}
	}

	private void Awake()
	{
		m_the = this;
	}

	private void Start()
	{
	}

	public void InitSplashScreen()
	{
	}

	private void Update()
	{
	}
}

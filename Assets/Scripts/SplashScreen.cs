using System.Collections;
using UnityEngine;

public sealed class SplashScreen : MonoBehaviour
{
	public Camera m_mainCam;

	private static float m_majescoLogoTime = 0.5f;

	private static float m_oddLogoTime = 0.1f;

	public Texture2D m_frontLoadingBar;

	public Texture2D m_backLoadingBar;

	private static Vector2 LOADING_BAR_FRONT_POS = new Vector2(314f, 1203.5f);

	private static Vector2 LOADING_BAR_BACK_POS = new Vector2(311f, 1200f);

	private int LOADING_TEXTURE_WIDTH = 768;

	private int LOADING_TEXTURE_HEIGHT = 1366;

	public Texture2D m_blankBackground;

	public Texture2D m_sidekickLogo;

	public Texture2D m_oddLogo;

	private SceneLoader.LoadRequest m_loadingState;

	private bool m_showedMajesco;

	private bool m_showedOddGents;

	private float m_currentTime;

	private static SplashScreen m_the;

	public static SplashScreen m_The
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
		m_mainCam.backgroundColor = Color.white;
	}

	private void Update()
	{
		m_currentTime += Time.deltaTime;
		UpdateSplashScreen();
	}

	private void UpdateSplashScreen()
	{
		if (!m_showedMajesco)
		{
			if (m_currentTime > m_majescoLogoTime)
			{
				m_currentTime = 0f;
				m_showedMajesco = true;
			}
		}
		else if (!m_showedOddGents && m_currentTime > m_oddLogoTime)
		{
			m_currentTime = 0f;
			m_showedOddGents = true;
			StartCoroutine(LoadScene());
		}
	}

	private IEnumerator LoadScene()
	{
		DebugManager.Log("Load LoadScene");
		m_loadingState = new SceneLoader.LoadRequest("MainGamePlay");
		
		float elapsed = 0f;
		float duration = 1f;
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			m_loadingState.Progress = Mathf.Clamp01(elapsed / duration * 0.9f);
			yield return null;
		}
		
		m_loadingState.Progress = 1f;
		Debug.Log("[SplashScreen] About to call Application.LoadLevel(MainGamePlay)");
		Application.LoadLevel("MainGamePlay");
	}

	private void OnGUI()
	{
		float num = LOADING_TEXTURE_WIDTH * Screen.height / LOADING_TEXTURE_HEIGHT;
		float num2 = ((float)Screen.width - num) / 2f;
		float num3 = num / (float)LOADING_TEXTURE_WIDTH;
		float num4 = (float)Screen.height / (float)LOADING_TEXTURE_HEIGHT;
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), m_blankBackground);
		if (m_showedOddGents && m_loadingState != null)
		{
			GUI.DrawTexture(new Rect(num2, 0f, num, Screen.height), m_sidekickLogo);
			GUI.DrawTexture(new Rect(num2 + LOADING_BAR_BACK_POS.x * num3, LOADING_BAR_BACK_POS.y * num4, (float)m_backLoadingBar.width * num4, (float)m_backLoadingBar.height * num4), m_backLoadingBar);
			GUI.DrawTexture(new Rect(num2 + LOADING_BAR_FRONT_POS.x * num3, LOADING_BAR_FRONT_POS.y * num4, (float)m_frontLoadingBar.width * num4 * m_loadingState.Progress, (float)m_frontLoadingBar.height * num4), m_frontLoadingBar);
		}
		else if (!m_showedOddGents)
		{
			GUI.DrawTexture(new Rect(num2, 0f, num, Screen.height), m_oddLogo);
			GUI.DrawTexture(new Rect(num2 + LOADING_BAR_BACK_POS.x * num3, LOADING_BAR_BACK_POS.y * num4, (float)m_backLoadingBar.width * num4, (float)m_backLoadingBar.height * num4), m_backLoadingBar);
			GUI.DrawTexture(new Rect(num2 + LOADING_BAR_FRONT_POS.x * num3, LOADING_BAR_FRONT_POS.y * num4, (float)m_frontLoadingBar.width * num4 * 0f, (float)m_frontLoadingBar.height * num4), m_frontLoadingBar);
		}
	}
}

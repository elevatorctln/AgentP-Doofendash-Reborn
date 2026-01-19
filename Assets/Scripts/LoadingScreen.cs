using System.Collections;
using UnityEngine;

public sealed class LoadingScreen : MonoBehaviour
{
	public GameObject m_perry;

	public Camera m_mainCam;

	private Animator m_perryAnim;

	private bool m_showedLoading;

	private bool m_showedPerry;

	private float m_currentTime;

	private float m_minLoadingTime;

	public string m_nextSceneToLoad = "MainMenu";

	public GameObject m_sceneCam;

	private static bool m_initGameCenter;

	private bool m_hideAll;

	private static LoadingScreen m_the;

	public bool HideAll
	{
		set
		{
			m_hideAll = value;
			LoadingGuiManager.The.HideAll(true);
		}
	}

	public static LoadingScreen The
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
		m_perryAnim = m_perry.GetComponent<Animator>();
		m_perryAnim.speed = 0f;
		m_perry.SetActive(false);
		if (!m_initGameCenter)
		{
			PerryGameServices.Init();
			PerryGameServices.LoadAchievements();
			m_initGameCenter = true;
		}
	}

	private void Update()
	{
	}

	private void FixedUpdate()
	{
		m_currentTime += Time.deltaTime;
		if (!m_showedPerry && !m_hideAll)
		{
			ShowPerryLoad();
		}
		if (GameManager.currentGameplayState == GameManager.GameplayState.Loading_GameStart)
		{
			UpdateLoadingMainMenu();
		}
		else if (GameManager.currentGameplayState == GameManager.GameplayState.GamePlay_Intro)
		{
			UpdateLoadingGamePlay();
		}
		else if (GameManager.currentGameplayState == GameManager.GameplayState.GameRestart_Menu)
		{
			UpdateLoadingInGameMenu();
		}
	}

	private void UpdateLoadingMainMenu()
	{
		if (!m_showedLoading && m_currentTime > m_minLoadingTime)
		{
			m_nextSceneToLoad = "MainMenu";
			m_currentTime = 0f;
			m_showedLoading = true;
			StartCoroutine(LoadNextScene());
		}
	}

	private void UpdateLoadingGamePlay()
	{
		if (!m_showedLoading && m_currentTime > m_minLoadingTime)
		{
			m_nextSceneToLoad = "MainGamePlay";
			m_currentTime = 0f;
			m_showedLoading = true;
			StartCoroutine(LoadNextScene());
		}
	}

	private void UpdateLoadingInGameMenu()
	{
		if (!m_showedLoading)
		{
			PlayerData.LoadHighScores();
			if (m_currentTime > m_minLoadingTime)
			{
				m_nextSceneToLoad = "InGameMenu";
				m_currentTime = 0f;
				m_showedLoading = true;
				StartCoroutine(LoadNextScene());
			}
		}
	}

	private void ShowPerryLoad()
	{
		m_mainCam.backgroundColor = Color.black;
		LoadingGuiManager.ToggleLoadingText();
		m_perry.SetActive(true);
		m_perryAnim.speed = 0.5f;
		m_perryAnim.SetFloat("Speed", 1f);
		m_showedPerry = true;
	}

	private IEnumerator LoadNextScene()
	{
		Debug.Log("LoadNextScene");
		// Use SceneLoader so we preserve the old async semantics (yielding) even on Unity 4 Personal.
		// In hindsight this was completely unnecessary
		SceneLoader.LoadRequest req = SceneLoader.LoadScene(m_nextSceneToLoad, this, 0.1f);
		while (!req.IsDone)
		{
			yield return null;
		}
		Debug.Log("Loading next scene is complete");
	}
}

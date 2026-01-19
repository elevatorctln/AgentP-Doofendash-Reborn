using UnityEngine;

public class GamePlay : MonoBehaviour
{
	private static bool m_introDone;

	private static float m_introTimer;

	private static float m_MaxTimeInIntro = 12f;

	public static bool ms_ShouldSkipTubeIntro;

	private static GamePlay m_the;

	public static GamePlay The
	{
		get
		{
			if (m_the == null)
			{
				m_the = GameObject.Find("GamePlay").GetComponent<GamePlay>();
			}
			return m_the;
		}
	}

	private void OnDestroy()
	{
		GameEventManager.GameIntroEndEvents -= GameIntroEndListener;
		MainMenuEventManager.PlayWithCurrentCharacter -= CharacterSelectedListener;
	}

	private void Awake()
	{
		m_the = this;
		GameEventManager.GameIntroEndEvents += GameIntroEndListener;
		MainMenuEventManager.PlayWithCurrentCharacter += CharacterSelectedListener;
	}

	private void Start()
	{
	}

	public void GamePlayStart()
	{
		GameEventManager.TriggerGameIntro();
		m_introTimer = 0f;
		m_introDone = false;
	}

	public void GamePlayStartNoTubeIntro()
	{
		GameEventManager.TriggerGameIntro();
		m_introTimer = 0f;
		m_introDone = false;
		Runner.The().ForceStopTubeIntro();
		PerryCamera.The().ResetCameraPosition();
		GlobalGUIManager.The.ShowHUDonGameStart();
		GameEventManager.TriggerGameStart();
	}

	private void Update()
	{
		GameplayIntroUpdate();
	}

	private void GameplayIntroUpdate()
	{
		if (GameManager.currentGameplayState != GameManager.GameplayState.GamePlay_Intro)
		{
			return;
		}
		if (Runner.The().IsInTubeIntroState())
		{
			if (!m_introDone && m_introTimer >= m_MaxTimeInIntro)
			{
				Runner.The().StopTubeIntro();
				m_introDone = true;
			}
			m_introTimer += Time.deltaTime;
		}
		if (Runner.The().IsTubeIntroFinshed())
		{
			GameEventManager.TriggerGameStart();
		}
	}

	private void GameIntroEndListener()
	{
	}

	private void CharacterSelectedListener(int characterIndex)
	{
		if (!ms_ShouldSkipTubeIntro)
		{
			GamePlayStart();
		}
		else
		{
			GamePlayStartNoTubeIntro();
		}
	}
}

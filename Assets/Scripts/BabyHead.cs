using UnityEngine;

public class BabyHead : MonoBehaviour
{
	public PowerUpChooser m_PowerUpChooser;

	public float m_BabyDuration = 10f;

	private Timer m_BabyTimer;

	private float m_HalfAnimationLength;

	private bool m_HasPowerupBeenDropped;

	private PowerUp m_PowerUpSpawned;

	private Animation m_Animation;

	private void Awake()
	{
		m_Animation = GetComponent<Animation>();
		AnimationClip clip = m_Animation.clip;
		m_HalfAnimationLength = clip.length * 0.5f;
		GameEventManager.GamePause += GamePauseListener;
		GameEventManager.GameUnPause += GameUnPauseListener;
		GameEventManager.GameRestartMenu += GameRestartMenuListener;
	}

	private Timer TheBabyTimer()
	{
		if (m_BabyTimer == null)
		{
			m_BabyTimer = TimerManager.The().SpawnTimer();
		}
		return m_BabyTimer;
	}

	private void OnDestroy()
	{
		GameEventManager.GamePause -= GamePauseListener;
		GameEventManager.GameUnPause -= GameUnPauseListener;
		GameEventManager.GameRestartMenu -= GameRestartMenuListener;
	}

	private void GamePauseListener()
	{
		AnimationOps.SetAnimationSpeed(m_Animation, 0f);
	}

	private void GameUnPauseListener()
	{
		AnimationOps.SetAnimationSpeed(m_Animation, 1f);
	}

	private void GameRestartMenuListener()
	{
		AnimationOps.SetAnimationSpeed(m_Animation, 1f);
	}

	public void ResetBabyHead()
	{
		TheBabyTimer().Start(m_BabyDuration);
		m_HasPowerupBeenDropped = false;
		AnimationOps.SetAnimationSpeed(m_Animation, 1f);
	}

	private void DropPowerup()
	{
		m_HasPowerupBeenDropped = true;
		PowerUp prefabObject = m_PowerUpChooser.ChooseRandom(PlayerData.BabyFedoraPercent);
		Vector3 position = Runner.The().CalcObjectLaneBasePosition(base.transform.position, Waypoint.Lane.Middle);
		position += Runner.The().transform.forward * 62.5f;
		position.y += 8f;
		m_PowerUpSpawned = CacheManager.The().Spawn(prefabObject);
		m_PowerUpSpawned.transform.position = position;
		m_PowerUpSpawned.transform.rotation = Runner.The().transform.rotation;
		m_PowerUpSpawned.gameObject.SetActive(true);
	}

	private void Update()
	{
		if (!(Runner.The() != null))
		{
			return;
		}
		base.transform.position = Runner.The().CalcRunnerLaneBasePosition(Waypoint.Lane.Middle);
		base.transform.rotation = Runner.The().transform.rotation;
		if (TheBabyTimer().RetrieveTimeElapsed() >= m_HalfAnimationLength && !m_HasPowerupBeenDropped)
		{
			m_HasPowerupBeenDropped = true;
			DropPowerup();
		}
		if (TheBabyTimer().IsFinished())
		{
			if (m_PowerUpSpawned != null)
			{
				CacheManager.The().Unspawn(m_PowerUpSpawned.gameObject);
				m_PowerUpSpawned = null;
			}
			CacheManager.The().Unspawn(base.gameObject);
			BabyHeadTrigger.ms_IsBabyPlaying = false;
		}
	}
}

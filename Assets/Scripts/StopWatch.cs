using UnityEngine;

public class StopWatch
{
	private float m_TimeCur;

	private bool m_IsPaused = true;

	public void Start()
	{
		m_TimeCur = 0f;
		m_IsPaused = false;
	}

	public void Stop()
	{
		m_TimeCur = 0f;
		m_IsPaused = true;
	}

	public void Pause()
	{
		m_IsPaused = true;
	}

	public void Unpause()
	{
		m_IsPaused = false;
	}

	public void Reset()
	{
		m_TimeCur = 0f;
	}

	public void Update()
	{
		if (!GameManager.The.IsGamePaused() && !m_IsPaused)
		{
			m_TimeCur += Time.deltaTime;
		}
	}

	public float RetrieveTimeElapsed()
	{
		return m_TimeCur;
	}

	public bool IsPaused()
	{
		return m_IsPaused;
	}

	public void SetTimerValue(float newValue)
	{
		m_TimeCur = newValue;
	}
}

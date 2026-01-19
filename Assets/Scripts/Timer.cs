using UnityEngine;

public class Timer
{
	private float m_TimeCur;

	private float m_Duration;

	public Timer()
	{
	}

	public Timer(float duration)
	{
		Start(duration);
	}

	public void Start(float duration)
	{
		m_TimeCur = 0f;
		m_Duration = duration;
	}

	public void Update()
	{
		if (!GameManager.The.IsGamePaused() && !IsFinished())
		{
			m_TimeCur += Time.deltaTime;
		}
	}

	public float RetrieveTimeElapsed()
	{
		return m_TimeCur;
	}

	public float RetrieveTimeRemaining()
	{
		float num = m_Duration - m_TimeCur;
		if (num < 0f)
		{
			num = 0f;
		}
		return num;
	}

	public bool IsFinished()
	{
		if (m_TimeCur > m_Duration)
		{
			return true;
		}
		return false;
	}
}

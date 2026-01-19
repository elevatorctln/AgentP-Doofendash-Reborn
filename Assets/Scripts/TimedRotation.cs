using UnityEngine;

public class TimedRotation
{
	private float m_StartTime;

	private float m_Duration;

	private Quaternion m_StartRotation;

	private Quaternion m_DesiredRotation;

	public void Start(Quaternion startRotation, Quaternion desiredRotation, float duration)
	{
		m_StartTime = Time.time;
		m_Duration = duration;
		m_StartRotation = startRotation;
		m_DesiredRotation = desiredRotation;
	}

	public void NextRotation(out Quaternion rotation, out bool isFinished)
	{
		float num = Time.time - m_StartTime;
		float num2 = num / m_Duration;
		isFinished = num2 >= 1f;
		rotation = Quaternion.Slerp(m_StartRotation, m_DesiredRotation, num2);
	}
}

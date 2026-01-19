public class RunnerDistanceChecker
{
	private float m_StartDistance;

	private float m_DistanceToTravel;

	public void Start(float distanceToTravel)
	{
		if (!(Runner.The() == null))
		{
			m_StartDistance = Runner.The().m_Distance;
			m_DistanceToTravel = distanceToTravel;
		}
	}

	public bool IsFinished()
	{
		if (Runner.The() == null)
		{
			return false;
		}
		float num = Runner.The().m_Distance - m_StartDistance;
		if (num > m_DistanceToTravel)
		{
			return true;
		}
		return false;
	}
}

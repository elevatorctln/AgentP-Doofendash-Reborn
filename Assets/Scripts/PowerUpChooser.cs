using UnityEngine;

public class PowerUpChooser : MonoBehaviour
{
	public PowerUp[] m_PowerUpList;

	private static float m_MinSpawnDist = 500f;

	private static float m_MaxSpawnDist = 750f;

	private static RunnerDistanceChecker ms_RunnerDistanceChecker = new RunnerDistanceChecker();

	private static bool ms_HasPowerUpBeenSpawned = false;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public static void SetHasPowerUpBeenSpawned(bool hasPowerUpBeenSpawned)
	{
		ms_HasPowerUpBeenSpawned = hasPowerUpBeenSpawned;
	}

	public static bool GetHasPowerUpBeenSpawned()
	{
		return ms_HasPowerUpBeenSpawned;
	}

	public static bool CanSpawnPowerUp()
	{
		if (Runner.The() != null && Runner.The().IsInPowerUpMode())
		{
			return false;
		}
		if (ms_RunnerDistanceChecker.IsFinished())
		{
			return true;
		}
		return false;
	}

	public PowerUp ChooseRandom()
	{
		if (m_PowerUpList.Length == 0)
		{
			return null;
		}
		int num = m_PowerUpList.Length;
		if (!MiniGameManager.The().CanSpawnEaglePowerUp())
		{
			num--;
		}
		int num2 = Random.Range(0, num);
		float distanceToTravel = Random.Range(m_MinSpawnDist, m_MaxSpawnDist);
		ms_RunnerDistanceChecker.Start(distanceToTravel);
		return m_PowerUpList[num2];
	}

	public PowerUp ChooseRandom(float fedoraPercentage)
	{
		if (m_PowerUpList.Length == 0)
		{
			return null;
		}
		float distanceToTravel = Random.Range(m_MinSpawnDist, m_MaxSpawnDist);
		ms_RunnerDistanceChecker.Start(distanceToTravel);
		int num = m_PowerUpList.Length;
		if (!MiniGameManager.The().CanSpawnEaglePowerUp())
		{
			num--;
		}
		float num2 = 100f - fedoraPercentage;
		float num3 = num2 / (float)(num - 1);
		float[] array = new float[num];
		float num4 = 0f;
		for (int i = 0; i < num; i++)
		{
			Fedora component = m_PowerUpList[i].GetComponent<Fedora>();
			num4 = (array[i] = ((!(component == null)) ? (num4 + fedoraPercentage) : (num4 + num3)));
		}
		float num5 = Random.Range(0f, 100f);
		for (int j = 0; j < num; j++)
		{
			if (num5 < array[j])
			{
				return m_PowerUpList[j];
			}
		}
		return m_PowerUpList[m_PowerUpList.Length - 1];
	}
}

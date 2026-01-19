using UnityEngine;

public class StreamPlaneObstacleHitTrigger : MonoBehaviour
{
	private static GameObject[] m_smokeCloudParticles;

	private static int m_numCloudObjects = 10;

	private static bool m_init;

	private static int m_currentCloudIndex;

	private void Awake()
	{
		if (m_init)
		{
			return;
		}
		GameObject gameObject = GameObject.Find("ExplosionVFX");
		if (gameObject != null)
		{
			m_smokeCloudParticles = new GameObject[m_numCloudObjects];
			for (int i = 0; i < m_numCloudObjects; i++)
			{
				m_smokeCloudParticles[i] = (GameObject)Object.Instantiate(gameObject);
				m_smokeCloudParticles[i].SetActive(false);
			}
			m_currentCloudIndex = 0;
			gameObject.SetActive(false);
		}
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider trigger)
	{
		Obstacle component = trigger.gameObject.GetComponent<Obstacle>();
		if (component != null)
		{
			m_smokeCloudParticles[m_currentCloudIndex].transform.position = new Vector3(component.transform.position.x, component.transform.position.y + 5f, component.transform.position.z);
			m_smokeCloudParticles[m_currentCloudIndex].SetActive(false);
			m_smokeCloudParticles[m_currentCloudIndex].SetActive(true);
			component.gameObject.SetActive(false);
			IncreaseCloudIndex();
		}
	}

	private static void IncreaseCloudIndex()
	{
		m_currentCloudIndex++;
		if (m_currentCloudIndex >= m_smokeCloudParticles.Length)
		{
			m_currentCloudIndex = 0;
		}
	}
}

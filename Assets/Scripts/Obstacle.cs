using System.Collections;
using UnityEngine;

public class Obstacle : PlatformAddon
{
	public enum SpawnFrequencyType
	{
		Normal = 0,
		Billboard = 1,
		Pipe = 2,
		Clothesline = 3,
		WaterTower = 4,
		TreeFall = 5
	}

	public ObstacleSelector.LaneType m_LaneType = ObstacleSelector.LaneType.MultiLaner;

	public ObstacleSelector.ObstacleType m_ObstacleType;

	[HideInInspector]
	public int m_ObstacleLane;

	[HideInInspector]
	public int[] m_LaneDisallowList;

	public SpawnFrequencyType m_SpawnFrequencyType;

	public int m_IgnoreMeCount;

	public bool m_IsScaffold;

	public float[] m_OffsetsY;

	public bool m_IsPartialScaffold;

	public float m_PartialScaffoldOffset;

	public bool m_IsRamp;

	public float m_RampAngle;

	public bool m_IsEndCap;

	public GameObject m_ShadowRect;

	private static IgnoreMeHandler m_IgnoreMeHandler = new IgnoreMeHandler();

	public static bool m_ForceSpawn = false;

	protected bool b_SoundTimerHasFinished;

	protected Timer m_ObstacleSoundTimer;

	public float m_SoundClipTimerOffset;

	public string m_SoundClipName = string.Empty;

	public static float CalcLanePosition(int obstacleLane, float laneOffset)
	{
		return laneOffset * (float)obstacleLane;
	}

	public static bool SpawnObstacle(Obstacle obstacle, Transform parent, float laneOffset, ArrayList obstaclesInstancedList, int laneLocations)
	{
		if (!m_ForceSpawn && m_IgnoreMeHandler.UpdateOnNewSpawn(obstacle.m_SpawnFrequencyType, obstacle.m_IgnoreMeCount) == IgnoreMeHandler.Action.Ignore)
		{
			return false;
		}
		m_ForceSpawn = false;
		if (obstacle.m_LaneType != ObstacleSelector.LaneType.MultiLaner)
		{
			obstaclesInstancedList.Add(SpawnInOneLane(obstacle, parent, laneOffset, obstacle.m_ObstacleLane));
		}
		else
		{
			obstaclesInstancedList.AddRange(SpawnOneLanerGroup(obstacle, parent, laneOffset, laneLocations));
		}
		return true;
	}

	public static Obstacle SpawnInOneLane(Obstacle obstacle, Transform parent, float laneOffset, int obstacleLane)
	{
		Obstacle obstacle2 = CacheManager.The().Spawn(obstacle);
		obstacle2.ResetObstacle();
		obstacle2.Init();
		obstacle2.transform.position = parent.transform.position;
		obstacle2.transform.rotation = parent.transform.rotation;
		obstacle2.transform.localScale = Vector3.one;
		obstacle2.transform.parent = parent.transform;
		obstacle2.transform.localPosition = obstacle.SpawnLocalPosition;
		TransformOps.SetLocalPositionX(obstacle2.transform, CalcLanePosition(obstacleLane, laneOffset));
		return obstacle2;
	}

	public static Obstacle SpawnInOneLane(Obstacle obstacle, Transform parent, float laneOffset, int obstacleLane, float offsetY, float offsetZ)
	{
		Obstacle obstacle2 = CacheManager.The().Spawn(obstacle);
		obstacle2.ResetObstacle();
		obstacle2.Init();
		obstacle2.transform.position = parent.transform.position;
		TransformOps.AddPositionY(obstacle2.transform, offsetY);
		TransformOps.AddPositionZ(obstacle2.transform, offsetZ);
		obstacle2.transform.localScale = Vector3.one;
		obstacle2.transform.rotation = parent.transform.rotation;
		obstacle2.transform.parent = parent.transform;
		TransformOps.SetLocalPositionX(obstacle2.transform, CalcLanePosition(obstacleLane, laneOffset));
		if (obstacle2.m_ShadowRect != null)
		{
			TransformOps.SetPositionY(obstacle2.m_ShadowRect.transform, parent.transform.position.y + 0.1f);
		}
		return obstacle2;
	}

	public static bool DoesIgnoreListContainDuplicates(Obstacle obstacle)
	{
		bool result = false;
		for (int i = 0; i < obstacle.m_LaneDisallowList.Length; i++)
		{
			for (int j = i + 1; j < obstacle.m_LaneDisallowList.Length; j++)
			{
				if (obstacle.m_LaneDisallowList[i] == obstacle.m_LaneDisallowList[j] && obstacle.m_LaneDisallowList[i] != 0)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	protected Timer SoundTimer()
	{
		if (m_ObstacleSoundTimer == null)
		{
			m_ObstacleSoundTimer = TimerManager.The().SpawnTimer();
		}
		return m_ObstacleSoundTimer;
	}

	public static Obstacle[] SpawnOneLanerGroup(Obstacle obstacle, Transform parent, float laneOffset, int laneLocations)
	{
		int num = laneLocations & LaneLocations.L;
		num += (laneLocations & LaneLocations.M) >> 1;
		num += (laneLocations & LaneLocations.R) >> 2;
		Obstacle[] array = new Obstacle[num];
		int num2 = 0;
		if ((laneLocations & LaneLocations.L) != 0)
		{
			array[num2++] = SpawnInOneLane(obstacle, parent, laneOffset, -1);
		}
		if ((laneLocations & LaneLocations.M) != 0)
		{
			array[num2++] = SpawnInOneLane(obstacle, parent, laneOffset, 0);
		}
		if ((laneLocations & LaneLocations.R) != 0)
		{
			array[num2++] = SpawnInOneLane(obstacle, parent, laneOffset, 1);
		}
		return array;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		GameEventManager.GamePause += HandleGameStopped;
		GameEventManager.GameOver += HandleGameStopped;
		GameEventManager.GameUnPause += HandleGameUnPause;
	}

	private void OnDisable()
	{
		GameEventManager.GamePause -= HandleGameStopped;
		GameEventManager.GameOver -= HandleGameStopped;
		GameEventManager.GameUnPause -= HandleGameUnPause;
	}

	public virtual void ResetObstacle()
	{
		if (GameManager.The.m_SoundEnabled)
		{
			AudioSource component = GetComponent<AudioSource>();
			if (component != null)
			{
				component.minDistance = 15f;
				component.Play();
			}
		}
	}

	private void HandleGameUnPause()
	{
		if (GameManager.The.m_SoundEnabled)
		{
			AudioSource component = GetComponent<AudioSource>();
			if (component != null)
			{
				component.Play();
			}
		}
	}

	private void HandleGameStopped()
	{
		if (GameManager.The.m_SoundEnabled)
		{
			AudioSource component = GetComponent<AudioSource>();
			if (component != null)
			{
				component.Pause();
			}
		}
	}

	public virtual void Init()
	{
	}

	public virtual void Uninit()
	{
	}
}

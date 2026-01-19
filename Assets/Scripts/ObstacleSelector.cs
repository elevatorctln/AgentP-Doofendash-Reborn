using System;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSelector : MonoBehaviour
{
	[Serializable]
	public class ObstacleLaneTypes
	{
		[Serializable]
		public class ObstacleTypes
		{
			public List<Obstacle> m_ObstacleList = new List<Obstacle>();
		}

		public ObstacleTypes[] m_ObstacleTypes;
	}

	public enum LanesNotUsed
	{
		ThreeLane = 0,
		TwoLaneL = 1,
		TwoLaneR = 2,
		LMR = 3,
		LM = 4,
		LR = 5,
		MR = 6,
		L = 7,
		M = 8,
		R = 9
	}

	public enum LaneType
	{
		ThreeLane = 0,
		TwoLaneL = 1,
		TwoLaneR = 2,
		MultiLaner = 3,
		LM = 4,
		LR = 5,
		MR = 6,
		L = 7,
		M = 8,
		R = 9
	}

	public enum ObstacleType
	{
		NonAvoid = 0,
		Avoid = 1
	}

	[Serializable]
	public class ObstacleSequence
	{
		[Serializable]
		public class ObstacleTuple
		{
			[Serializable]
			public class ObstacleSpecs
			{
				public Obstacle m_Obstacle;

				public int m_ScaffoldLevel;

				public float m_OffsetZ;

				public TokenGroup m_TokenGroup;

				public PowerUpChooser m_PowerUpChooser;

				public ObstacleSpecs Duplicate()
				{
					ObstacleSpecs obstacleSpecs = new ObstacleSpecs();
					obstacleSpecs.m_Obstacle = m_Obstacle;
					obstacleSpecs.m_OffsetZ = m_OffsetZ;
					obstacleSpecs.m_PowerUpChooser = m_PowerUpChooser;
					obstacleSpecs.m_ScaffoldLevel = m_ScaffoldLevel;
					obstacleSpecs.m_TokenGroup = m_TokenGroup;
					return obstacleSpecs;
				}
			}

			public ObstacleSpecs m_Left;

			public ObstacleSpecs m_Middle;

			public ObstacleSpecs m_Right;

			public ObstacleTuple Duplicate()
			{
				ObstacleTuple obstacleTuple = new ObstacleTuple();
				obstacleTuple.m_Left = m_Left.Duplicate();
				obstacleTuple.m_Middle = m_Middle.Duplicate();
				obstacleTuple.m_Right = m_Right.Duplicate();
				return obstacleTuple;
			}
		}

		public ObstacleTuple[] m_ObstacleTuple;

		public string m_Name;

		public ObstacleSequence Duplicate()
		{
			ObstacleSequence obstacleSequence = new ObstacleSequence();
			obstacleSequence.m_Name = string.Copy(m_Name);
			obstacleSequence.m_ObstacleTuple = new ObstacleTuple[m_ObstacleTuple.Length];
			for (int i = 0; i < obstacleSequence.m_ObstacleTuple.Length; i++)
			{
				obstacleSequence.m_ObstacleTuple[i] = m_ObstacleTuple[i].Duplicate();
			}
			return obstacleSequence;
		}
	}

	[HideInInspector]
	public ObstacleLaneTypes[] m_LaneTypes;

	private static string m_ForcedObstacleName;

	private static bool m_ShouldUseForcedObstacle;

	public ObstacleSequence[] m_ObstacleSequence;

	public float m_SequenceProbability = 1f;

	public bool m_DontShuffleSequences;

	public bool m_ShouldPlayAllSequences;

	public void Create()
	{
		int num = Enum.GetNames(typeof(LaneType)).Length;
		int num2 = Enum.GetNames(typeof(ObstacleType)).Length;
		m_LaneTypes = new ObstacleLaneTypes[num];
		for (int i = 0; i < num; i++)
		{
			m_LaneTypes[i] = new ObstacleLaneTypes();
			m_LaneTypes[i].m_ObstacleTypes = new ObstacleLaneTypes.ObstacleTypes[num2];
			for (int j = 0; j < num2; j++)
			{
				m_LaneTypes[i].m_ObstacleTypes[j] = new ObstacleLaneTypes.ObstacleTypes();
			}
		}
	}

	public void Destroy()
	{
		m_LaneTypes = null;
	}

	public void Add(LaneType lane, ObstacleType obstacleType, Obstacle obstacle)
	{
		if (m_LaneTypes == null || m_LaneTypes.Length == 0)
		{
			Create();
		}
		m_LaneTypes[(int)lane].m_ObstacleTypes[(int)obstacleType].m_ObstacleList.Add(obstacle);
	}

	private bool IsEqual(OSTLaneType laneType, int laneLocation)
	{
		if (laneType == OSTLaneType.LMR && laneLocation == LaneLocations.LMR)
		{
			return true;
		}
		if (laneType == OSTLaneType.LM && laneLocation == LaneLocations.LM)
		{
			return true;
		}
		if (laneType == OSTLaneType.LR && laneLocation == LaneLocations.LR)
		{
			return true;
		}
		if (laneType == OSTLaneType.MR && laneLocation == LaneLocations.MR)
		{
			return true;
		}
		if (laneType == OSTLaneType.L && laneLocation == LaneLocations.L)
		{
			return true;
		}
		if (laneType == OSTLaneType.M && laneLocation == LaneLocations.M)
		{
			return true;
		}
		if (laneType == OSTLaneType.R && laneLocation == LaneLocations.R)
		{
			return true;
		}
		return false;
	}

	private bool IsObstacleAllowed(Obstacle obstacle, OSTLaneType ostLaneType)
	{
		for (int i = 0; i < obstacle.m_LaneDisallowList.Length; i++)
		{
			if (IsEqual(ostLaneType, obstacle.m_LaneDisallowList[i]))
			{
				return false;
			}
		}
		return true;
	}

	public static void ForceNextObstacle(string obstacleName)
	{
		m_ShouldUseForcedObstacle = true;
		m_ForcedObstacleName = obstacleName;
	}

	private Obstacle FindObstacleByName(string name)
	{
		ObstacleLaneTypes[] laneTypes = m_LaneTypes;
		foreach (ObstacleLaneTypes obstacleLaneTypes in laneTypes)
		{
			for (int j = 0; j < obstacleLaneTypes.m_ObstacleTypes.Length; j++)
			{
				List<Obstacle> obstacleList = obstacleLaneTypes.m_ObstacleTypes[j].m_ObstacleList;
				foreach (Obstacle item in obstacleList)
				{
					if (item.name.Contains(name))
					{
						return item;
					}
				}
			}
		}
		return null;
	}

	public Obstacle Select(LaneType lane, OSTLaneType ostLaneType, ObstacleType obstacleType)
	{
		if (m_ShouldUseForcedObstacle)
		{
			m_ShouldUseForcedObstacle = false;
			return FindObstacleByName(m_ForcedObstacleName);
		}
		if (m_LaneTypes == null || m_LaneTypes.Length == 0)
		{
			return null;
		}
		if ((int)lane >= m_LaneTypes.Length)
		{
			return null;
		}
		if ((int)obstacleType >= m_LaneTypes[(int)lane].m_ObstacleTypes.Length)
		{
			return null;
		}
		List<Obstacle> obstacleList = m_LaneTypes[(int)lane].m_ObstacleTypes[(int)obstacleType].m_ObstacleList;
		if (obstacleList == null || obstacleList.Count <= 0)
		{
			return null;
		}
		Obstacle obstacle = obstacleList[UnityEngine.Random.Range(0, obstacleList.Count)];
		if (obstacle == null)
		{
			Debug.Log("obstacle == null");
			return null;
		}
		if (IsObstacleAllowed(obstacle, ostLaneType))
		{
			return obstacle;
		}
		for (int i = 0; i < obstacleList.Count; i++)
		{
			obstacle = obstacleList[i];
			if (IsObstacleAllowed(obstacle, ostLaneType))
			{
				return obstacle;
			}
		}
		return null;
	}
}

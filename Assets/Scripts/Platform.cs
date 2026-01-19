using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObstacleSelector))]
public class Platform : MonoBehaviour
{
	public enum PlatformType
	{
		StraightAway = 0,
		HardLeft = 1,
		HardRight = 2,
		Rotate = 3,
		LeftAndRight = 4,
		Ramp = 5,
		BeamHopping = 6,
		Scaffolding = 7,
		DuckyMomo = 8,
		BabyHead = 9,
		BossIdle = 10,
		BossOneArm = 11,
		BossTwoArm = 12,
		Gap = 13,
		Break = 14,
		PizzaSlice = 15,
		DoofObstacles = 16,
		BalloonyIdle = 17
	}

	public enum PlatformDifficulty
	{
		Easy = 0,
		Medium = 1,
		Hard = 2
	}

	private class ObstacleShuffles
	{
		public int[] m_Shuffles;

		public int m_Index;
	}

	[Serializable]
	public class RotationSequence
	{
		[Serializable]
		public class RotationTuple
		{
			public float m_RotateXAngle;

			public float m_RotateYAngle;

			public Waypoint.PizzaSliceType m_PizzaSlice;

			public int m_Count = 1;

			public static RotationTuple Clone(RotationTuple rt)
			{
				RotationTuple rotationTuple = new RotationTuple();
				rotationTuple.m_RotateXAngle = rt.m_RotateXAngle;
				rotationTuple.m_RotateYAngle = rt.m_RotateYAngle;
				rotationTuple.m_PizzaSlice = rt.m_PizzaSlice;
				rotationTuple.m_Count = rt.m_Count;
				return rotationTuple;
			}
		}

		public RotationTuple[] m_RotationTuple;

		public string m_Name;

		public static RotationSequence Clone(RotationSequence rs)
		{
			RotationSequence rotationSequence = new RotationSequence();
			rotationSequence.m_Name = rs.m_Name;
			rotationSequence.m_RotationTuple = new RotationTuple[rs.m_RotationTuple.Length];
			for (int i = 0; i < rs.m_RotationTuple.Length; i++)
			{
				rotationSequence.m_RotationTuple[i] = RotationTuple.Clone(rs.m_RotationTuple[i]);
			}
			return rotationSequence;
		}

		public int CalcTotalTupleCount()
		{
			int num = 0;
			RotationTuple[] rotationTuple = m_RotationTuple;
			foreach (RotationTuple rotationTuple2 in rotationTuple)
			{
				num += rotationTuple2.m_Count;
			}
			return num;
		}
	}

	public Waypoint[] m_Waypoints;

	public Obstacle[] m_ObstaclesToBeReset;

	public bool m_ShouldSetNewPathHeight;

	public float m_NewHeightOffset;

	public PlatformSequence[] m_LinkToSequenceList;

	public Platform[] m_LinkToPlatformList;

	public PlatformType m_PlatformType;

	public float m_RotateXAngle;

	public float m_RotateYAngle;

	public float m_SelectMeWeight = 10f;

	public int m_SpawnMeCount = 1;

	public int m_IgnoreMeCount;

	private float m_SelectMeWeightInterval;

	private float m_PlatformListWeightTotal;

	private bool m_IsWithinTokenCullDistance;

	private float m_TokenCullDistanceSquared = 5625f;

	private bool m_IsWithinMagnetDistance;

	private float m_MagnetTokenDistanceSquared = 22500f;

	private bool m_IsWithinTokenRotateDistance;

	private float m_TokenRotateDistanceSquared = 140625f;

	public float m_LaneOffset = 0.13f;

	private ArrayList m_ObstacleInstancedList = new ArrayList();

	public Scenery[] m_SceneryToInstanceList;

	private ArrayList m_SceneryInstancedList = new ArrayList();

	public UnityEngine.Object m_BeamHopping;

	public UnityEngine.Object m_BeamHoppingEmpty;

	private ArrayList m_AddonsInstancedList = new ArrayList();

	private List<TokenGroup> m_TokenGroupsInstancedList = new List<TokenGroup>();

	public bool m_ShouldNotRenderSceneryOnLowEndDevices;

	private ObstacleSelector m_ObstacleSelector;

	private static ArrayList m_PlatformBuckets = new ArrayList();

	private static ArrayList m_SequenceBuckets = new ArrayList();

	private static Hashtable m_IgnoreMeList = new Hashtable();

	private static bool ms_AllowTurns = true;

	private static bool ms_AllowRamps = true;

	private static bool ms_AllowBeamHoppings = true;

	private static bool ms_AllowSequences = true;

	private static bool ms_AllowRotations = true;

	private static bool ms_AllowScaffolds = true;

	private static bool ms_AllowGaps = true;

	private static bool ms_AllowBreaks = true;

	public PlatformDifficulty m_PlatformDifficulty;

	private static PlatformDifficulty ms_PlatformDifficultyCur = PlatformDifficulty.Easy;

	private static int ms_LeftTurnCount = 0;

	private static int ms_RightTurnCount = 0;

	private bool m_DidSpawnPowerUpOnThisPlatform;

	private static bool ms_IsDoingObstacleSequence = false;

	private static ObstacleSelector.ObstacleSequence ms_ObstacleSequence = null;

	private static bool ms_IsFinalObstacleSequence = false;

	private static bool ms_HasSpawnedAllTutorialSequences = false;

	private static int ms_ObstacleTupleCur = 0;

	private static bool ms_IsSpawningTokensThisSequence = false;

	private static bool ms_ShouldForceSpawnSpecificObstacleSequence = false;

	private static bool ms_DidForceSpawnSpecificObstacleSequence = false;

	private static int ms_ForceSpawnSpecificObstacleSequenceIndex = 0;

	private static Hashtable m_ShuffleObstaclesHash = new Hashtable();

	private int m_LanesMoved;

	private static int[] ms_StrippedBeamIterations = new int[4] { 2, 3, 6, 7 };

	private static int ms_BeamDistributionPrev = 0;

	private static bool ms_IsDoingRotationSequence = false;

	private static RotationSequence ms_RotationSequenceCur = null;

	private static int ms_RotationTupleIndexCur = 0;

	public RotationSequence[] m_RotationSequence;

	public bool m_UseRotationSequence;

	private ObstacleSelector FindObstacleSelector()
	{
		if (m_ObstacleSelector == null)
		{
			m_ObstacleSelector = GetComponent<ObstacleSelector>();
		}
		return m_ObstacleSelector;
	}

	private bool IsBossPlatform()
	{
		return m_PlatformType == PlatformType.BossIdle || m_PlatformType == PlatformType.BossOneArm || m_PlatformType == PlatformType.BossTwoArm;
	}

	private Platform ChooseBossPlatform()
	{
		Platform platform = this;
		Platform[] linkToPlatformList = m_LinkToPlatformList;
		foreach (Platform platform2 in linkToPlatformList)
		{
			if (DoofenCruiser.The().IsActive())
			{
				if (DoofenCruiser.The().m_BossDeck == DoofenCruiser.BossDeck.Idle && platform2.m_PlatformType == PlatformType.BossIdle)
				{
					platform = platform2;
				}
				if (DoofenCruiser.The().m_BossDeck == DoofenCruiser.BossDeck.OneArm && platform2.m_PlatformType == PlatformType.BossOneArm)
				{
					platform = platform2;
				}
				if (DoofenCruiser.The().m_BossDeck == DoofenCruiser.BossDeck.TwoArm && platform2.m_PlatformType == PlatformType.BossTwoArm)
				{
					platform = platform2;
				}
			}
			else if (Balloony.The != null && Balloony.The.IsActive())
			{
				platform = platform2;
			}
		}
		if (platform != null)
		{
			platform.name = platform.name.Replace("(Clone)", string.Empty);
		}
		return platform;
	}

	public Platform RandomPrefab(bool allowLeftAndRights, out PlatformSequence selectedPlatformSequence, out bool foundPlatformSequence)
	{
		Platform platform = null;
		selectedPlatformSequence = null;
		foundPlatformSequence = false;
		if (!ms_IsFinalObstacleSequence && FindObstacleSelector().m_ShouldPlayAllSequences && !ms_HasSpawnedAllTutorialSequences)
		{
			base.name = base.name.Replace("(Clone)", string.Empty);
			return this;
		}
		if (IsBossPlatform())
		{
			Platform platform2 = ChooseBossPlatform();
			if (platform2 != null)
			{
				return platform2;
			}
		}
		RefreshBucketList(allowLeftAndRights);
		if (ms_AllowSequences)
		{
			RefreshSequenceBucketList();
		}
		float num = UnityEngine.Random.Range(0f, m_PlatformListWeightTotal);
		foreach (Platform platformBucket in m_PlatformBuckets)
		{
			if (num < platformBucket.m_SelectMeWeightInterval)
			{
				platform = platformBucket;
				break;
			}
		}
		if (platform != null)
		{
			foundPlatformSequence = false;
			if (platform.m_IgnoreMeCount > 0)
			{
				if (!m_IgnoreMeList.Contains(platform))
				{
					m_IgnoreMeList.Add(platform, platform.m_IgnoreMeCount);
				}
				else
				{
					m_IgnoreMeList[platform] = platform.m_IgnoreMeCount;
				}
			}
			platform.name = platform.name.Replace("(Clone)", string.Empty);
			if (platform.m_PlatformType == PlatformType.HardLeft)
			{
				IncLeftTurnCount();
				ResetRightTurnCount();
			}
			else if (platform.m_PlatformType == PlatformType.HardRight)
			{
				IncRightTurnCount();
				ResetLeftTurnCount();
			}
			return platform;
		}
		foreach (PlatformSequence sequenceBucket in m_SequenceBuckets)
		{
			if (num < sequenceBucket.m_SelectMeWeightInterval)
			{
				foundPlatformSequence = true;
				selectedPlatformSequence = sequenceBucket;
				return null;
			}
		}
		base.name = base.name.Replace("(Clone)", string.Empty);
		return this;
	}

	private void AddToIgnoreMeList(Transform prefab, int ignoreMeCount)
	{
		m_IgnoreMeList.Add(prefab, ignoreMeCount);
	}

	private void RemoveFromIgnoreMeList(Transform prefab)
	{
		m_IgnoreMeList.Remove(prefab);
	}

	public static void ClearIgnoreMeList()
	{
		m_IgnoreMeList.Clear();
	}

	private void RefreshWeights()
	{
		m_PlatformListWeightTotal = 0f;
		foreach (Platform platformBucket in m_PlatformBuckets)
		{
			float selectMeWeight = platformBucket.m_SelectMeWeight;
			m_PlatformListWeightTotal += selectMeWeight;
			platformBucket.m_SelectMeWeightInterval = m_PlatformListWeightTotal;
		}
	}

	private void RefreshSequenceWeights()
	{
		foreach (PlatformSequence sequenceBucket in m_SequenceBuckets)
		{
			m_PlatformListWeightTotal += sequenceBucket.m_SelectMeWeight;
			sequenceBucket.m_SelectMeWeightInterval = m_PlatformListWeightTotal;
		}
	}

	public static void SetAllowTurns(bool allowTurns)
	{
		ms_AllowTurns = allowTurns;
	}

	public static bool GetAllowTurns()
	{
		return ms_AllowTurns;
	}

	public static void SetAllowRamps(bool allowRamps)
	{
		ms_AllowRamps = allowRamps;
	}

	public static bool GetAllowRamps()
	{
		return ms_AllowRamps;
	}

	public static void SetAllowBeamHoppings(bool allowBeamHoppings)
	{
		ms_AllowBeamHoppings = allowBeamHoppings;
	}

	public static bool GetAllowBeamHoppings()
	{
		return ms_AllowBeamHoppings;
	}

	public static void SetAllowSequences(bool allowSequences)
	{
		ms_AllowSequences = allowSequences;
	}

	public static bool GetAllowSequences()
	{
		return ms_AllowSequences;
	}

	public static void SetAllowRotations(bool allowRotations)
	{
		ms_AllowRotations = allowRotations;
	}

	public static bool GetAllowRotations()
	{
		return ms_AllowRotations;
	}

	public static void SetAllowScaffolds(bool allowScaffolds)
	{
		ms_AllowScaffolds = allowScaffolds;
	}

	public static bool GetAllowScaffolds()
	{
		return ms_AllowScaffolds;
	}

	public static void SetAllowGaps(bool allowGaps)
	{
		ms_AllowGaps = allowGaps;
	}

	public static bool GetAllowGaps()
	{
		return ms_AllowGaps;
	}

	public static void SetAllowBreaks(bool allowBreaks)
	{
		ms_AllowBreaks = allowBreaks;
	}

	public static bool GetAllowBreaks()
	{
		return ms_AllowBreaks;
	}

	public static void IncPlatformDifficultyState()
	{
		if (ms_PlatformDifficultyCur != PlatformDifficulty.Hard)
		{
			int num = (int)ms_PlatformDifficultyCur;
			num++;
			ms_PlatformDifficultyCur = (PlatformDifficulty)num;
		}
	}

	public static void DecPlatformDifficultyState()
	{
		if (ms_PlatformDifficultyCur != PlatformDifficulty.Easy)
		{
			int num = (int)ms_PlatformDifficultyCur;
			num--;
			ms_PlatformDifficultyCur = (PlatformDifficulty)num;
		}
	}

	private bool IsThisPlatformAllowedWithCurrentDifficulty(Platform p)
	{
		int num = (int)ms_PlatformDifficultyCur;
		int platformDifficulty = (int)p.m_PlatformDifficulty;
		return num >= platformDifficulty;
	}

	private void RefreshBucketList(bool allowLeftAndRights)
	{
		m_PlatformBuckets.Clear();
		Platform[] linkToPlatformList = m_LinkToPlatformList;
		foreach (Platform platform in linkToPlatformList)
		{
			if (platform == null || (!allowLeftAndRights && platform.m_PlatformType == PlatformType.LeftAndRight) || ((platform.m_PlatformType == PlatformType.HardLeft || platform.m_PlatformType == PlatformType.HardRight || platform.m_PlatformType == PlatformType.LeftAndRight) && (!ms_AllowTurns || GameManager.The.CanUseCopterBoostJumpStart())) || (!IsAllowedToSpawnLeftTurns() && (platform.m_PlatformType == PlatformType.HardLeft || platform.m_PlatformType == PlatformType.LeftAndRight)) || (!IsAllowedToSpawnRightTurns() && (platform.m_PlatformType == PlatformType.HardRight || platform.m_PlatformType == PlatformType.LeftAndRight)) || ((!ms_AllowRamps || GameManager.The.CanUseCopterBoostJumpStart()) && platform.m_PlatformType == PlatformType.Ramp) || (!ms_AllowBeamHoppings && platform.m_PlatformType == PlatformType.BeamHopping) || (!ms_AllowRotations && platform.m_PlatformType == PlatformType.Rotate) || (!ms_AllowGaps && platform.m_PlatformType == PlatformType.Gap) || (!ms_AllowBreaks && platform.m_PlatformType == PlatformType.Break) || ((Runner.The().IsInCopterBoostState() || GameManager.The.CanUseCopterBoostJumpStart()) && platform.m_PlatformType == PlatformType.DoofObstacles) || !IsThisPlatformAllowedWithCurrentDifficulty(platform))
			{
				continue;
			}
			if (!m_IgnoreMeList.ContainsKey(platform))
			{
				m_PlatformBuckets.Add(platform);
				continue;
			}
			int num = (int)m_IgnoreMeList[platform];
			num--;
			m_IgnoreMeList[platform] = num;
			if (num <= 0)
			{
				m_IgnoreMeList.Remove(platform);
			}
		}
		RefreshWeights();
	}

	private void RefreshSequenceBucketList()
	{
		m_SequenceBuckets.Clear();
		PlatformSequence[] linkToSequenceList = m_LinkToSequenceList;
		foreach (PlatformSequence platformSequence in linkToSequenceList)
		{
			if (!(platformSequence == null))
			{
				m_SequenceBuckets.Add(platformSequence);
			}
		}
		RefreshSequenceWeights();
	}

	public void ResetPlatform()
	{
		if (m_DidSpawnPowerUpOnThisPlatform)
		{
			PowerUpChooser.SetHasPowerUpBeenSpawned(false);
		}
		m_DidSpawnPowerUpOnThisPlatform = false;
		m_IsWithinTokenCullDistance = false;
		m_IsWithinMagnetDistance = false;
		m_IsWithinTokenRotateDistance = false;
		Waypoint[] waypoints = m_Waypoints;
		foreach (Waypoint waypoint in waypoints)
		{
			waypoint.ResetWaypoint();
		}
		for (int j = 0; j < m_ObstaclesToBeReset.Length; j++)
		{
			m_ObstaclesToBeReset[j].ResetObstacle();
		}
	}

	private void Start()
	{
		Waypoint[] waypoints = m_Waypoints;
		foreach (Waypoint waypoint in waypoints)
		{
			waypoint.GetComponent<MeshRenderer>().enabled = false;
			waypoint.m_LeftLane.GetComponent<MeshRenderer>().enabled = false;
			waypoint.m_RightLane.GetComponent<MeshRenderer>().enabled = false;
		}
	}

	private void OnDestroy()
	{
		if (m_DidSpawnPowerUpOnThisPlatform)
		{
			PowerUpChooser.SetHasPowerUpBeenSpawned(false);
		}
	}

	private void Update()
	{
		if (!m_IsWithinTokenRotateDistance)
		{
			if ((Runner.The().transform.position - base.transform.position).sqrMagnitude < m_TokenRotateDistanceSquared)
			{
				m_IsWithinTokenRotateDistance = true;
				for (int i = 0; i < m_TokenGroupsInstancedList.Count; i++)
				{
					m_TokenGroupsInstancedList[i].StopCullingRotation();
				}
			}
		}
		else if (!m_IsWithinMagnetDistance)
		{
			if ((Runner.The().transform.position - base.transform.position).sqrMagnitude < m_MagnetTokenDistanceSquared)
			{
				m_IsWithinMagnetDistance = true;
				for (int j = 0; j < m_TokenGroupsInstancedList.Count; j++)
				{
					m_TokenGroupsInstancedList[j].StopCullingMagnet();
					m_TokenGroupsInstancedList[j].StopCullingParticles();
				}
			}
		}
		else if (!m_IsWithinTokenCullDistance && (Runner.The().transform.position - base.transform.position).sqrMagnitude < m_TokenCullDistanceSquared)
		{
			m_IsWithinTokenCullDistance = true;
			for (int k = 0; k < m_TokenGroupsInstancedList.Count; k++)
			{
				m_TokenGroupsInstancedList[k].StopCullingCollision();
			}
		}
	}

	public void InstantiateScenery()
	{
		if (GameManager.The.IsLowEndDevice() && m_ShouldNotRenderSceneryOnLowEndDevices)
		{
			return;
		}
		Scenery[] sceneryToInstanceList = m_SceneryToInstanceList;
		foreach (Scenery scenery in sceneryToInstanceList)
		{
			GameObject gameObject = Scenery.SpawnScenery(scenery, base.transform);
			if (gameObject != null)
			{
				m_SceneryInstancedList.Add(gameObject);
			}
		}
	}

	public void DestroyScenery()
	{
		foreach (GameObject sceneryInstanced in m_SceneryInstancedList)
		{
			CacheManager.The().Unspawn(sceneryInstanced);
		}
		m_SceneryInstancedList.Clear();
	}

	private ObstacleSelector.LaneType ToLaneTypeGeneralized(OSTLaneType laneType, out int laneLocations)
	{
		laneLocations = LaneLocations.None;
		ObstacleSelector.LaneType result;
		switch (laneType)
		{
		case OSTLaneType.ThreeLane:
			result = ObstacleSelector.LaneType.ThreeLane;
			laneLocations = LaneLocations.L | LaneLocations.M | LaneLocations.R;
			break;
		case OSTLaneType.TwoLaneL:
			result = ObstacleSelector.LaneType.TwoLaneL;
			laneLocations = LaneLocations.L | LaneLocations.M;
			break;
		case OSTLaneType.TwoLaneR:
			result = ObstacleSelector.LaneType.TwoLaneR;
			laneLocations = LaneLocations.M | LaneLocations.R;
			break;
		case OSTLaneType.LM:
			result = ObstacleSelector.LaneType.LM;
			laneLocations = LaneLocations.L | LaneLocations.M;
			break;
		case OSTLaneType.MR:
			result = ObstacleSelector.LaneType.MR;
			laneLocations = LaneLocations.L | LaneLocations.M;
			break;
		case OSTLaneType.LR:
			result = ObstacleSelector.LaneType.LR;
			laneLocations = LaneLocations.L | LaneLocations.R;
			break;
		default:
			result = ObstacleSelector.LaneType.MultiLaner;
			switch (laneType)
			{
			case OSTLaneType.LMR:
				laneLocations = LaneLocations.L | LaneLocations.M | LaneLocations.R;
				break;
			case OSTLaneType.L:
				laneLocations = LaneLocations.L;
				break;
			case OSTLaneType.M:
				laneLocations = LaneLocations.M;
				break;
			case OSTLaneType.R:
				laneLocations = LaneLocations.R;
				break;
			}
			break;
		}
		return result;
	}

	private OSTLaneType LaneLocationsToOSTLaneType(int laneLocations)
	{
		if ((laneLocations & LaneLocations.L) == LaneLocations.L && (laneLocations & LaneLocations.M) == LaneLocations.M && (laneLocations & LaneLocations.R) == LaneLocations.R)
		{
			return OSTLaneType.LMR;
		}
		if ((laneLocations & LaneLocations.L) == LaneLocations.L && (laneLocations & LaneLocations.M) == LaneLocations.M)
		{
			return OSTLaneType.LM;
		}
		if ((laneLocations & LaneLocations.L) == LaneLocations.L && (laneLocations & LaneLocations.R) == LaneLocations.R)
		{
			return OSTLaneType.LR;
		}
		if ((laneLocations & LaneLocations.M) == LaneLocations.M && (laneLocations & LaneLocations.R) == LaneLocations.R)
		{
			return OSTLaneType.MR;
		}
		if ((laneLocations & LaneLocations.L) == LaneLocations.L)
		{
			return OSTLaneType.L;
		}
		if ((laneLocations & LaneLocations.M) == LaneLocations.M)
		{
			return OSTLaneType.M;
		}
		if ((laneLocations & LaneLocations.R) == LaneLocations.R)
		{
			return OSTLaneType.R;
		}
		return OSTLaneType.NoObstacle;
	}

	public static void IncLeftTurnCount()
	{
		ms_LeftTurnCount++;
	}

	public static void IncRightTurnCount()
	{
		ms_RightTurnCount++;
	}

	public static void ResetLeftTurnCount()
	{
		ms_LeftTurnCount = 0;
	}

	public static void ResetRightTurnCount()
	{
		ms_RightTurnCount = 0;
	}

	private static bool IsAllowedToSpawnLeftTurns()
	{
		if (ms_LeftTurnCount >= 2)
		{
			return false;
		}
		return true;
	}

	private static bool IsAllowedToSpawnRightTurns()
	{
		if (ms_RightTurnCount >= 2)
		{
			return false;
		}
		return true;
	}

	public static void ForceSpawnSpecificObstacleSequence(int specificSequence)
	{
		ms_ForceSpawnSpecificObstacleSequenceIndex = specificSequence;
		ms_ShouldForceSpawnSpecificObstacleSequence = true;
	}

	private static int CalcObstacleSequenceIndex(ObstacleSelector obstacleSelector)
	{
		if (obstacleSelector.m_ObstacleSequence.Length == 1)
		{
			return 0;
		}
		int num = 0;
		ObstacleShuffles obstacleShuffles;
		if (!m_ShuffleObstaclesHash.Contains(obstacleSelector.name))
		{
			int[] array = new int[obstacleSelector.m_ObstacleSequence.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = i;
			}
			if (!obstacleSelector.m_DontShuffleSequences)
			{
				MathOps.Shuffle(array);
			}
			obstacleShuffles = new ObstacleShuffles();
			obstacleShuffles.m_Shuffles = array;
			obstacleShuffles.m_Index = 0;
			m_ShuffleObstaclesHash.Add(obstacleSelector.name, obstacleShuffles);
		}
		else
		{
			obstacleShuffles = (ObstacleShuffles)m_ShuffleObstaclesHash[obstacleSelector.name];
		}
		num = obstacleShuffles.m_Shuffles[obstacleShuffles.m_Index];
		obstacleShuffles.m_Index++;
		if (obstacleShuffles.m_Index >= obstacleShuffles.m_Shuffles.Length)
		{
			if (!obstacleSelector.m_DontShuffleSequences)
			{
				MathOps.Shuffle(obstacleShuffles.m_Shuffles);
			}
			obstacleShuffles.m_Index = 0;
		}
		return num;
	}

	private void StartObstacleSequence(ObstacleSelector obstacleSelector)
	{
		if (obstacleSelector.m_ObstacleSequence != null && obstacleSelector.m_ObstacleSequence.Length != 0 && !ms_IsDoingObstacleSequence)
		{
			ms_IsDoingObstacleSequence = true;
			int num = CalcObstacleSequenceIndex(obstacleSelector);
			if (ms_ShouldForceSpawnSpecificObstacleSequence)
			{
				num = ms_ForceSpawnSpecificObstacleSequenceIndex;
				ms_DidForceSpawnSpecificObstacleSequence = true;
			}
			ms_ObstacleSequence = obstacleSelector.m_ObstacleSequence[num];
			
			// Debug logging for Unity 2017 migration
			if (ms_ObstacleSequence == null)
			{
				Debug.LogError("Platform.StartObstacleSequence: Selected ObstacleSequence at index " + num + " is NULL on platform: " + base.name);
			}
			else if (ms_ObstacleSequence.m_ObstacleTuple == null)
			{
				Debug.LogError("Platform.StartObstacleSequence: ObstacleSequence.m_ObstacleTuple is NULL on platform: " + base.name);
			}
			else if (ms_ObstacleSequence.m_ObstacleTuple.Length == 0)
			{
				Debug.LogError("Platform.StartObstacleSequence: ObstacleSequence.m_ObstacleTuple is EMPTY on platform: " + base.name);
			}
			else
			{
				Debug.Log("Platform.StartObstacleSequence: Successfully started sequence '" + ms_ObstacleSequence.m_Name + "' with " + ms_ObstacleSequence.m_ObstacleTuple.Length + " tuples on platform: " + base.name);
			}
			
			ms_ObstacleTupleCur = 0;
			if (num >= obstacleSelector.m_ObstacleSequence.Length - 1)
			{
				ms_IsFinalObstacleSequence = true;
				ms_HasSpawnedAllTutorialSequences = true;
			}
			else
			{
				ms_IsFinalObstacleSequence = false;
			}
			if (PlatformManager.The().m_SpawnMeCount < ms_ObstacleSequence.m_ObstacleTuple.Length)
			{
				PlatformManager.The().m_SpawnMeCount = ms_ObstacleSequence.m_ObstacleTuple.Length;
			}
			if (PlatformManager.The().IsForkingButDoneWithPostTurn())
			{
				PlatformManager.The().m_SpawnMeCount++;
			}
			if (Runner.The() != null && !Runner.The().IsInEagleState())
			{
				ms_IsSpawningTokensThisSequence = true;
			}
		}
		else
		{
			// Debug logging for why sequence wasn't started
			if (obstacleSelector.m_ObstacleSequence == null)
			{
				Debug.LogWarning("Platform.StartObstacleSequence: m_ObstacleSequence array is NULL on platform: " + base.name);
			}
			else if (obstacleSelector.m_ObstacleSequence.Length == 0)
			{
				Debug.LogWarning("Platform.StartObstacleSequence: m_ObstacleSequence array is EMPTY on platform: " + base.name);
			}
			else if (ms_IsDoingObstacleSequence)
			{
				Debug.Log("Platform.StartObstacleSequence: Already doing an obstacle sequence, skipping for platform: " + base.name);
			}
		}
	}

	private void EndObstacleSequence()
	{
		ms_IsDoingObstacleSequence = false;
		ms_ObstacleSequence = null;
		ms_IsSpawningTokensThisSequence = false;
		if (ms_DidForceSpawnSpecificObstacleSequence)
		{
			ms_ShouldForceSpawnSpecificObstacleSequence = false;
			ms_ForceSpawnSpecificObstacleSequenceIndex = 0;
		}
	}

	private void WaypointOffsetFromScaffoldDetachChildren()
	{
		for (int i = 0; i < m_Waypoints.Length; i++)
		{
			m_Waypoints[i].transform.DetachChildren();
		}
	}

	private void WaypointOffsetFromScaffoldAttachChildren()
	{
		for (int i = 0; i < m_Waypoints.Length; i++)
		{
			m_Waypoints[i].m_LeftLane.parent = m_Waypoints[i].transform;
			m_Waypoints[i].m_RightLane.parent = m_Waypoints[i].transform;
		}
	}

	private void SetRampWaypointPositions(Obstacle o, Waypoint.Lane lane, int scaffoldLevel)
	{
		Transform transform = null;
		for (int i = 0; i < m_Waypoints.Length; i++)
		{
			if (!m_Waypoints[i].m_FirstInSplitScaffoldRamp)
			{
				Transform transform2 = m_Waypoints[i].RetrieveTransform(lane);
				TransformOps.SetPositionY(transform2, base.transform.position.y + o.m_OffsetsY[scaffoldLevel]);
				transform = transform2;
				TransformOps.SetLocalEulerX(transform2, 0f - o.m_RampAngle);
			}
		}
		for (int j = 0; j < m_Waypoints.Length; j++)
		{
			if (m_Waypoints[j].m_FirstInSplitScaffoldRamp)
			{
				Transform transform3 = m_Waypoints[j].RetrieveTransform(lane);
				float rampAngle = o.m_RampAngle;
				float num = o.m_OffsetsY[scaffoldLevel];
				float num2 = num / Mathf.Sin((float)Math.PI / 180f * rampAngle);
				Vector3 vector = transform.forward * num2;
				Vector3 position = transform.position - vector;
				transform3.position = position;
				TransformOps.SetPositionY(transform3, base.transform.position.y);
				m_LanesMoved |= LaneLocations.ToLaneLocations(lane);
				m_Waypoints[j].BringUpToMaxZDepth(lane, m_LanesMoved);
			}
		}
	}

	private void SetPartialScaffoldWaypointPositions(Obstacle o, Waypoint.Lane lane, int scaffoldLevel)
	{
		Transform transform = null;
		for (int i = 0; i < m_Waypoints.Length; i++)
		{
			if (!m_Waypoints[i].m_FirstInSplitScaffoldRamp)
			{
				Transform transform2 = m_Waypoints[i].RetrieveTransform(lane);
				TransformOps.SetPositionY(transform2, base.transform.position.y + o.m_OffsetsY[scaffoldLevel]);
				transform = transform2;
			}
		}
		for (int j = 0; j < m_Waypoints.Length; j++)
		{
			if (m_Waypoints[j].m_FirstInSplitScaffoldRamp)
			{
				Transform transform3 = m_Waypoints[j].RetrieveTransform(lane);
				Vector3 vector = transform.forward * o.m_PartialScaffoldOffset;
				Vector3 position = transform.position - vector;
				transform3.position = position;
				TransformOps.SetPositionY(transform3, base.transform.position.y);
				m_LanesMoved |= LaneLocations.ToLaneLocations(lane);
				m_Waypoints[j].BringUpToMaxZDepth(lane, m_LanesMoved);
			}
		}
	}

	private void HandleScaffold(int obstacleLane, ObstacleSelector.ObstacleSequence.ObstacleTuple.ObstacleSpecs specs)
	{
		Obstacle obstacle = specs.m_Obstacle;
		int scaffoldLevel = specs.m_ScaffoldLevel;
		float y = base.transform.position.y + obstacle.m_OffsetsY[scaffoldLevel];
		float offsetZ = specs.m_OffsetZ;
		m_ObstacleInstancedList.Add(Obstacle.SpawnInOneLane(obstacle, base.transform, m_LaneOffset, obstacleLane, obstacle.m_OffsetsY[scaffoldLevel], offsetZ));
		if (obstacleLane == -1)
		{
			if (obstacle.m_IsRamp)
			{
				SetRampWaypointPositions(obstacle, Waypoint.Lane.Left, scaffoldLevel);
			}
			else if (obstacle.m_IsPartialScaffold)
			{
				SetPartialScaffoldWaypointPositions(obstacle, Waypoint.Lane.Left, scaffoldLevel);
			}
			else if (!obstacle.m_IsEndCap)
			{
				for (int i = 0; i < m_Waypoints.Length; i++)
				{
					TransformOps.SetPositionY(m_Waypoints[i].m_LeftLane, y);
				}
			}
		}
		if (obstacleLane == 0)
		{
			if (obstacle.m_IsRamp)
			{
				SetRampWaypointPositions(obstacle, Waypoint.Lane.Middle, scaffoldLevel);
			}
			else if (obstacle.m_IsPartialScaffold)
			{
				SetPartialScaffoldWaypointPositions(obstacle, Waypoint.Lane.Middle, scaffoldLevel);
			}
			else if (!obstacle.m_IsEndCap)
			{
				for (int j = 0; j < m_Waypoints.Length; j++)
				{
					TransformOps.SetPositionY(m_Waypoints[j].transform, y);
				}
			}
		}
		if (obstacleLane != 1)
		{
			return;
		}
		if (obstacle.m_IsRamp)
		{
			SetRampWaypointPositions(obstacle, Waypoint.Lane.Right, scaffoldLevel);
		}
		else if (obstacle.m_IsPartialScaffold)
		{
			SetPartialScaffoldWaypointPositions(obstacle, Waypoint.Lane.Right, scaffoldLevel);
		}
		else if (!obstacle.m_IsEndCap)
		{
			for (int k = 0; k < m_Waypoints.Length; k++)
			{
				TransformOps.SetPositionY(m_Waypoints[k].m_RightLane, y);
			}
		}
	}

	private int InstantiateObstacleSequencePerLane(ObstacleSelector.ObstacleSequence.ObstacleTuple.ObstacleSpecs specs, int lane, int laneLocations)
	{
		Obstacle obstacle = specs.m_Obstacle;
		if (PowerUpChooser.CanSpawnPowerUp() && specs.m_PowerUpChooser != null)
		{
			PowerUp spawnObj = specs.m_PowerUpChooser.ChooseRandom(5f);
			if (obstacle != null && obstacle.m_IsScaffold)
			{
				m_AddonsInstancedList.Add(PowerUp.Spawn(spawnObj, base.transform, m_LaneOffset, lane, obstacle.m_OffsetsY[specs.m_ScaffoldLevel], 0f).gameObject);
			}
			else
			{
				m_AddonsInstancedList.Add(PowerUp.Spawn(spawnObj, base.transform, m_LaneOffset, lane).gameObject);
			}
			PowerUpChooser.SetHasPowerUpBeenSpawned(true);
			m_DidSpawnPowerUpOnThisPlatform = true;
		}
		if (specs.m_TokenGroup != null && ms_IsSpawningTokensThisSequence && specs.m_TokenGroup.m_Probability >= UnityEngine.Random.Range(0f, 1f))
		{
			if (obstacle != null && obstacle.m_IsScaffold)
			{
				m_TokenGroupsInstancedList.Add(TokenGroup.Spawn(specs.m_TokenGroup, base.transform, m_LaneOffset, lane, obstacle.m_OffsetsY[specs.m_ScaffoldLevel], 0f));
			}
			else
			{
				m_TokenGroupsInstancedList.Add(TokenGroup.Spawn(specs.m_TokenGroup, base.transform, m_LaneOffset, lane));
			}
		}
		if (obstacle != null)
		{
			if (obstacle.m_IsScaffold)
			{
				if (GetAllowScaffolds())
				{
					HandleScaffold(lane, specs);
				}
			}
			else
			{
				m_ObstacleInstancedList.Add(Obstacle.SpawnInOneLane(obstacle, base.transform, m_LaneOffset, lane, 0f, specs.m_OffsetZ));
			}
			laneLocations = ((obstacle.m_LaneType != ObstacleSelector.LaneType.MultiLaner) ? (laneLocations | LaneLocations.ToLaneLocations(obstacle.m_LaneType)) : (laneLocations | LaneLocations.ToLaneLocations(lane)));
		}
		return laneLocations;
	}

	private void InstantiateObstacleSequence(bool shouldIncSequences)
	{
		if (ms_ObstacleSequence == null || ms_ObstacleSequence.m_ObstacleTuple == null || ms_ObstacleSequence.m_ObstacleTuple.Length == 0)
		{
			return;
		}
		int laneLocations = 0;
		m_LanesMoved = 0;
		WaypointOffsetFromScaffoldDetachChildren();
		laneLocations = InstantiateObstacleSequencePerLane(ms_ObstacleSequence.m_ObstacleTuple[ms_ObstacleTupleCur].m_Left, -1, laneLocations);
		laneLocations = InstantiateObstacleSequencePerLane(ms_ObstacleSequence.m_ObstacleTuple[ms_ObstacleTupleCur].m_Middle, 0, laneLocations);
		laneLocations = InstantiateObstacleSequencePerLane(ms_ObstacleSequence.m_ObstacleTuple[ms_ObstacleTupleCur].m_Right, 1, laneLocations);
		WaypointOffsetFromScaffoldAttachChildren();
		for (int i = 0; i < m_Waypoints.Length; i++)
		{
			m_Waypoints[i].m_LaneForPlacementOnDeath = ~laneLocations;
			m_Waypoints[i].m_IsLanePlacementFromObstacleSequence = true;
		}
		if (shouldIncSequences)
		{
			ms_ObstacleTupleCur++;
			if (ms_ObstacleTupleCur >= ms_ObstacleSequence.m_ObstacleTuple.Length)
			{
				EndObstacleSequence();
			}
		}
	}

	public void InstantiateObstacles(bool shouldIncSequences)
	{
		ObstacleSelector component = GetComponent<ObstacleSelector>();
		if (!(component == null))
		{
			// Debug logging for Unity 2017 migration issue
			if (component.m_ObstacleSequence == null || component.m_ObstacleSequence.Length == 0)
			{
				Debug.LogWarning("Platform.InstantiateObstacles: ObstacleSequence is null or empty on platform: " + base.name);
				return;
			}
			
			float num = UnityEngine.Random.Range(0f, 1f);
			if (num < component.m_SequenceProbability)
			{
				StartObstacleSequence(component);
			}
			if (ms_IsDoingObstacleSequence)
			{
				InstantiateObstacleSequence(shouldIncSequences);
			}
		}
		else
		{
			Debug.LogWarning("Platform.InstantiateObstacles: ObstacleSelector component is null on platform: " + base.name);
		}
	}

	public void DestroyObstacles()
	{
		foreach (Obstacle obstacleInstanced in m_ObstacleInstancedList)
		{
			obstacleInstanced.Uninit();
			CacheManager.The().Unspawn(obstacleInstanced);
		}
		m_ObstacleInstancedList.Clear();
	}

	public void InstantiateEagleTokens()
	{
		if (!(EagleTokenGroups.The() == null))
		{
			TokenGroup tokenGroup = EagleTokenGroups.The().InstantiateEagleTokens(this);
			if (tokenGroup != null)
			{
				m_TokenGroupsInstancedList.Add(tokenGroup);
				tokenGroup.StopCullingCollision();
				tokenGroup.StopCullingMagnet();
				tokenGroup.StopCullingParticles();
				tokenGroup.StopCullingRotation();
			}
		}
	}

	private void DestroyAdditionalAddonsFromList()
	{
		foreach (GameObject addonsInstanced in m_AddonsInstancedList)
		{
			CacheManager.The().Unspawn(addonsInstanced);
		}
		m_AddonsInstancedList.Clear();
	}

	private void DestroyTokenGroups()
	{
		for (int i = 0; i < m_TokenGroupsInstancedList.Count; i++)
		{
			m_TokenGroupsInstancedList[i].Uninit();
			CacheManager.The().Unspawn(m_TokenGroupsInstancedList[i].gameObject);
		}
		m_TokenGroupsInstancedList.Clear();
	}

	public void DestroyAddOns()
	{
		DestroyScenery();
		DestroyObstacles();
		DestroyAdditionalAddonsFromList();
		DestroyTokenGroups();
	}

	public void DestroyObstaclesAndTokens()
	{
		DestroyObstacles();
		DestroyTokenGroups();
	}

	private void SpawnBeamHopping(UnityEngine.Object o, int lane)
	{
		GameObject gameObject = CacheManager.The().Spawn(o);
		m_AddonsInstancedList.Add(gameObject);
		gameObject.transform.position = base.transform.position;
		gameObject.transform.rotation = base.transform.rotation;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.parent = base.transform;
		TransformOps.SetLocalPositionX(gameObject.transform, Obstacle.CalcLanePosition(lane, m_LaneOffset));
	}

	public void InstantiateBeamHoppings(bool shouldUseRandomDistribution)
	{
		if (m_PlatformType == PlatformType.BeamHopping)
		{
			int num = (ms_BeamDistributionPrev = ((!shouldUseRandomDistribution) ? ms_BeamDistributionPrev : ((ms_BeamDistributionPrev != 1 && ms_BeamDistributionPrev != 4 && ms_BeamDistributionPrev != 5) ? UnityEngine.Random.Range(1, 7) : ms_StrippedBeamIterations[UnityEngine.Random.Range(0, ms_StrippedBeamIterations.Length)])));
			if ((num & LaneLocations.L) == LaneLocations.L)
			{
				SpawnBeamHopping(m_BeamHopping, -1);
			}
			else
			{
				SpawnBeamHopping(m_BeamHoppingEmpty, -1);
			}
			if ((num & LaneLocations.M) == LaneLocations.M)
			{
				SpawnBeamHopping(m_BeamHopping, 0);
			}
			else
			{
				SpawnBeamHopping(m_BeamHoppingEmpty, 0);
			}
			if ((num & LaneLocations.R) == LaneLocations.R)
			{
				SpawnBeamHopping(m_BeamHopping, 1);
			}
			else
			{
				SpawnBeamHopping(m_BeamHoppingEmpty, 1);
			}
			for (int i = 0; i < m_Waypoints.Length; i++)
			{
				m_Waypoints[i].m_LaneForPlacementOnDeath = num;
			}
		}
	}

	public static void ResetAll()
	{
		ResetLeftTurnCount();
		ResetRightTurnCount();
		ResetAllDisallows();
		ResetObstacleSequence();
		ms_HasSpawnedAllTutorialSequences = false;
	}

	private static void ResetAllDisallows()
	{
		ms_AllowBeamHoppings = true;
		ms_AllowRamps = true;
		ms_AllowRotations = true;
		ms_AllowScaffolds = true;
		ms_AllowSequences = true;
		ms_AllowTurns = true;
		ms_AllowGaps = true;
		ms_AllowBreaks = true;
	}

	public static void ResetObstacleSequence()
	{
		ms_IsDoingObstacleSequence = false;
		ms_ObstacleSequence = null;
		ms_ObstacleTupleCur = 0;
	}

	public void HandleRotationSequences()
	{
		if (m_UseRotationSequence)
		{
			if (!ms_IsDoingRotationSequence)
			{
				StartRotationSequence();
			}
			else
			{
				UpdateRotationSequence();
			}
		}
	}

	private void StartRotationSequence()
	{
		if (m_RotationSequence == null || m_RotationSequence.Length == 0)
		{
			Debug.LogWarning("Use Rotation Sequence is Checked when there is no rotation sequence on Platform = " + base.name);
			return;
		}
		ms_RotationTupleIndexCur = 0;
		int num = UnityEngine.Random.Range(0, m_RotationSequence.Length);
		ms_RotationSequenceCur = RotationSequence.Clone(m_RotationSequence[num]);
		PlatformManager.The().m_SpawnMeCount = ms_RotationSequenceCur.CalcTotalTupleCount();
		UpdateRotationSequence();
		ms_IsDoingRotationSequence = true;
	}

	private void UpdateRotationSequence()
	{
		RotationSequence.RotationTuple rotationTuple = ms_RotationSequenceCur.m_RotationTuple[ms_RotationTupleIndexCur];
		m_RotateXAngle = rotationTuple.m_RotateXAngle;
		m_RotateYAngle = rotationTuple.m_RotateYAngle;
		rotationTuple.m_Count--;
		for (int i = 0; i < m_Waypoints.Length; i++)
		{
			m_Waypoints[i].m_PizzaSlice = rotationTuple.m_PizzaSlice;
		}
		if (rotationTuple.m_Count <= 0)
		{
			ms_RotationTupleIndexCur++;
		}
		if (ms_RotationTupleIndexCur >= ms_RotationSequenceCur.m_RotationTuple.Length)
		{
			EndRotationSequence();
		}
	}

	private void EndRotationSequence()
	{
		ms_RotationSequenceCur = null;
		ms_IsDoingRotationSequence = false;
	}
}

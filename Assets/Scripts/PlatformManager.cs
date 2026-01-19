using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlatformManager : MonoBehaviour
{
	private enum Forking
	{
		NotForking = 0,
		Forking = 1
	}

	public Platform[] m_StraightAwayPlatformList;

	public int m_MinStraightAwayCount = 10;

	public int m_PlatformCount;

	public float m_RecycleOffset;

	public int m_NoObstaclesCount = 3;

	[HideInInspector]
	public float m_PlatformDepth = 125f;

	[HideInInspector]
	public int m_EagleNoObstaclesPlatformCount;

	[HideInInspector]
	public int m_EagleTokenSpawnPlatformCount;

	[HideInInspector]
	public LinkedList<Platform> m_PlatformList = new LinkedList<Platform>();

	private LinkedListNode<Platform> m_CurPlatformNode;

	private LinkedList<LinkedListNode<Platform>> m_LeftList = new LinkedList<LinkedListNode<Platform>>();

	private LinkedList<LinkedListNode<Platform>> m_RightList = new LinkedList<LinkedListNode<Platform>>();

	private PlatformPath m_MainPath = new PlatformPath();

	private PlatformPath m_VeerPathLeft = new PlatformPath();

	private PlatformPath m_VeerPathRight = new PlatformPath();

	private LinkedList<Platform> m_DestroyList = new LinkedList<Platform>();

	private int m_StraightAwayCur;

	private Forking m_Forking;

	private Platform m_ForkingPlatform;

	private int m_ForkingPlatformCount;

	private static PlatformManager m_The;

	private bool m_PlatformsLoadedForTheFirstTime;

	private bool m_ArePlatformsLoaded;

	private Waypoint.Lane m_ChosenLane = Waypoint.Lane.None;

	private bool m_AllowLeftAndRights = true;

	[HideInInspector]
	public int m_SpawnMeCount;

	private Platform m_SpawnMePrefab;

	private bool m_ShouldSpawnForceMePrefab;

	private Platform[] m_ForceMePrefabs;

	private int m_ForceMePrefabsCount;

	private bool m_AreSpecialForceMesQueued;

	private bool m_IsPushingPlatform;

	private bool m_ShouldSpawnForceMeSequence;

	private PlatformSequence.PlatformTuple[] m_PlatformSequence;

	private int m_PlatformSequenceCount;

	private bool m_ShouldSetNewHeight;

	private float m_NewHeight;

	public bool IsForking()
	{
		if (m_Forking == Forking.Forking)
		{
			return true;
		}
		return false;
	}

	public bool IsForkingButDoneWithPostTurn()
	{
		if (m_Forking == Forking.Forking && m_ForkingPlatformCount >= 2)
		{
			return true;
		}
		return false;
	}

	public static PlatformManager The()
	{
		if (m_The == null)
		{
			m_The = GameObject.Find("PlatformManager").GetComponent<PlatformManager>();
		}
		return m_The;
	}

	private void OnDestroy()
	{
		GameEventManager.ForceNextPlatform -= ForceNextPlatform;
		GameEventManager.ForceNextPlatforms -= ForceNextPlatforms;
		GameEventManager.ForceNewStraightAways -= ForceNewStraightAway;
		GameEventManager.ForcePlatformSequences -= ForceNextPlatformSequence;
		GameEventManager.GameStart -= GameStart;
		GameEventManager.GameOver -= GameOver;
		GameEventManager.GameContinue -= GameContinueEvent;
		GameEventManager.HangGlideEndPlatformEvents -= HangGlideEndPlatformListener;
	}

	private void Awake()
	{
		m_The = this;
		GameEventManager.ForceNextPlatform += ForceNextPlatform;
		GameEventManager.ForceNextPlatforms += ForceNextPlatforms;
		GameEventManager.ForceNewStraightAways += ForceNewStraightAway;
		GameEventManager.ForcePlatformSequences += ForceNextPlatformSequence;
		GameEventManager.GameIntro += GameIntro;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GameContinue += GameContinueEvent;
		GameEventManager.HangGlideEndPlatformEvents += HangGlideEndPlatformListener;
	}

	private void Init()
	{
	}

	private void Uninit()
	{
	}

	private void Start()
	{
		Platform.ResetAll();
		if (GameManager.The.IsLowEndDevice())
		{
			m_PlatformCount = 7;
		}
	}

	private void Update()
	{
		if (GameManager.The != null && GameManager.The.IsInGamePlay() && Runner.The() != null)
		{
			Vector3 vector = Runner.The().transform.localPosition - m_PlatformList.First.Value.transform.localPosition;
			vector.y = 0f;
			if (m_RecycleOffset * m_RecycleOffset < vector.sqrMagnitude)
			{
				ForkRecycle();
				DestroyLeftOverPlatforms();
			}
		}
	}

	private void ForkRecycle()
	{
		if (m_Forking == Forking.NotForking)
		{
			if (IsRunnerFinishedWithFirstPlatform())
			{
				Recycle(m_MainPath);
			}
		}
		else if (m_Forking == Forking.Forking && IsRunnerFinishedWithFirstPlatform() && IsRunnerFinishedWithSecondPlatform())
		{
			Recycle(m_VeerPathLeft, false);
			m_LeftList.AddLast(m_PlatformList.Last);
			Recycle(m_VeerPathRight);
			m_RightList.AddLast(m_PlatformList.Last);
		}
	}

	private void ForkPush()
	{
		if (m_Forking == Forking.NotForking)
		{
			PushPlatform(m_MainPath);
		}
		else if (m_Forking == Forking.Forking)
		{
			PushPlatform(m_VeerPathLeft, false);
			m_LeftList.AddLast(m_PlatformList.Last);
			PushPlatform(m_VeerPathRight);
			m_RightList.AddLast(m_PlatformList.Last);
		}
	}

	public void ChoosePath(Waypoint.Lane lane)
	{
		switch (lane)
		{
		case Waypoint.Lane.Left:
			Platform.IncLeftTurnCount();
			Platform.ResetRightTurnCount();
			if (m_ChosenLane == Waypoint.Lane.None)
			{
				m_Forking = Forking.NotForking;
				m_MainPath.Clone(m_VeerPathLeft);
				int num2 = m_RightList.Count;
				PopFork(m_RightList);
				while (num2 > 0)
				{
					PushPlatform(m_MainPath);
					num2--;
				}
			}
			break;
		case Waypoint.Lane.Right:
			Platform.IncRightTurnCount();
			Platform.ResetLeftTurnCount();
			if (m_ChosenLane == Waypoint.Lane.None)
			{
				m_Forking = Forking.NotForking;
				m_MainPath.Clone(m_VeerPathRight);
				int num = m_LeftList.Count;
				PopFork(m_LeftList);
				while (num > 0)
				{
					PushPlatform(m_MainPath);
					num--;
				}
			}
			break;
		case Waypoint.Lane.Middle:
			m_Forking = Forking.NotForking;
			break;
		}
		m_ChosenLane = lane;
	}

	private void PopFork(LinkedList<LinkedListNode<Platform>> forkList)
	{
		while (forkList.Count > 0)
		{
			LinkedListNode<Platform> value = forkList.First.Value;
			forkList.RemoveFirst();
			m_PlatformList.Remove(value);
			Platform value2 = value.Value;
			m_DestroyList.AddLast(value2);
		}
	}

	private void DestroyLeftOverPlatforms()
	{
		foreach (Platform destroy in m_DestroyList)
		{
			destroy.DestroyAddOns();
			CacheManager.The().Unspawn(destroy);
		}
		m_DestroyList.Clear();
	}

	private void LoadPlatformPrefabs()
	{
		if (!m_ArePlatformsLoaded)
		{
			m_MainPath.m_NextPosition = base.transform.localPosition;
			for (int i = 0; i < m_PlatformCount; i++)
			{
				ForkPush();
			}
			m_PlatformsLoadedForTheFirstTime = true;
			m_ArePlatformsLoaded = true;
		}
	}

	private void UnloadPlatformPrefabs()
	{
		PopAllPlatforms();
		m_PlatformsLoadedForTheFirstTime = false;
		m_ArePlatformsLoaded = false;
	}

	private void PrintListPositions()
	{
		foreach (Platform platform in m_PlatformList)
		{
			Debug.Log("PlatformManager - UpdatePlatformPrefabs() - PrintListPositions() - t = " + platform.transform.localPosition);
		}
	}

	private void UpdatePlatformPrefabs()
	{
		m_StraightAwayCur = 0;
		while (m_StraightAwayCur < m_PlatformCount)
		{
			ForkRecycle();
		}
	}

	private Platform InstantiateMiniGameStartPrefab()
	{
		return InstantiatePrefab(m_SpawnMePrefab = MiniGameManager.The().ChooseStartPlatform());
	}

	private Platform InstantiateRandomPrefab(Platform in_platform, bool shouldDecContinuations)
	{
		Platform platform = null;
		PlatformSequence selectedPlatformSequence;
		bool foundPlatformSequence;
		Platform platform2 = in_platform.RandomPrefab(m_AllowLeftAndRights, out selectedPlatformSequence, out foundPlatformSequence);
		if (!foundPlatformSequence)
		{
			m_SpawnMePrefab = platform2;
			platform = InstantiatePrefab(platform2);
			m_SpawnMeCount = platform.m_SpawnMeCount;
		}
		else
		{
			ForceNextPlatformSequence(selectedPlatformSequence.m_PlatformSequence);
			platform = HandleSpawnSequence(shouldDecContinuations);
		}
		return platform;
	}

	private Platform InstantiatePrefab(Platform prefab)
	{
		if (prefab == null)
		{
			Debug.LogError("PlatformManager.InstantiatePrefab: prefab is null! Check that MiniGameManager's platform references (m_TutorialStartPlatform, m_BabyHeadStartPlatform, m_MiniGameStartPlatform) are assigned in the Inspector.");
			return null;
		}
		Platform platform = CacheManager.The().Spawn(prefab);
		platform.ResetPlatform();
		platform.transform.parent = base.transform;
		return platform;
	}

	public Waypoint RetrieveFollowingWaypoint()
	{
		LinkedListNode<Platform> next = m_CurPlatformNode.Next;
		if (next == null)
		{
			return null;
		}
		Platform value = next.Value;
		if (value == null)
		{
			return null;
		}
		return value.m_Waypoints[0];
	}

	public Waypoint RetrieveFollowingWaypoint(Waypoint.Lane lane)
	{
		LinkedListNode<Platform> value;
		switch (lane)
		{
		case Waypoint.Lane.Left:
			value = m_LeftList.First.Value;
			break;
		case Waypoint.Lane.Right:
			value = m_RightList.First.Value;
			break;
		default:
			return null;
		}
		if (value == null)
		{
			return null;
		}
		Platform value2 = value.Value;
		if (value2 == null)
		{
			return null;
		}
		return value2.m_Waypoints[0];
	}

	private void MoveToNextPlatformNode()
	{
		Platform value = m_CurPlatformNode.Value;
		Platform.PlatformType platformType = value.m_PlatformType;
		if (platformType == Platform.PlatformType.LeftAndRight)
		{
			if (m_ChosenLane == Waypoint.Lane.Left)
			{
				m_CurPlatformNode = m_LeftList.First.Value;
				m_LeftList.Clear();
				m_ChosenLane = Waypoint.Lane.None;
			}
			else if (m_ChosenLane == Waypoint.Lane.Right)
			{
				m_CurPlatformNode = m_RightList.First.Value;
				m_RightList.Clear();
				m_ChosenLane = Waypoint.Lane.None;
			}
			else
			{
				m_CurPlatformNode = m_CurPlatformNode.Next;
			}
			m_AllowLeftAndRights = true;
		}
		else
		{
			m_CurPlatformNode = m_CurPlatformNode.Next;
		}
		GameEventManager.TriggerMoveToNextPlatform(m_CurPlatformNode.Value);
	}

	public void MoveCurPlatformToFirstPlatform()
	{
		foreach (Platform platform in m_PlatformList)
		{
			for (int i = 0; i < platform.m_Waypoints.Length; i++)
			{
				platform.m_Waypoints[i].SetIsPassed(false);
				platform.m_Waypoints[i].SetIsRunnerFinished(false);
			}
			if (platform.GetInstanceID() == m_CurPlatformNode.Value.GetInstanceID())
			{
				break;
			}
		}
		m_CurPlatformNode = m_PlatformList.First;
	}

	public Platform RetrievePlatformCur()
	{
		return m_CurPlatformNode.Value;
	}

	public Waypoint RetrieveNextWaypoint(Transform runner, Waypoint.Lane lane)
	{
		while (m_CurPlatformNode != null)
		{
			Platform value = m_CurPlatformNode.Value;
			if (value == null)
			{
				return null;
			}
			Waypoint[] waypoints = value.m_Waypoints;
			if (waypoints == null)
			{
				return null;
			}
			foreach (Waypoint waypoint in waypoints)
			{
				if (waypoint != null && !waypoint.GetIsPassed())
				{
					return waypoint;
				}
			}
			MoveToNextPlatformNode();
		}
		return null;
	}

	private void RotatePathAboutY(PlatformPath platformPath, float angle)
	{
		platformPath.m_Rotation *= Quaternion.AngleAxis(angle, Vector3.up);
		platformPath.m_Rotation.eulerAngles = new Vector3(platformPath.m_Rotation.eulerAngles.x, platformPath.m_Rotation.eulerAngles.y, 0f);
	}

	private void RotatePathAboutX(PlatformPath platformPath, float angle)
	{
		platformPath.m_Rotation *= Quaternion.AngleAxis(angle, Vector3.right);
		platformPath.m_Rotation.eulerAngles = new Vector3(platformPath.m_Rotation.eulerAngles.x, platformPath.m_Rotation.eulerAngles.y, 0f);
	}

	private void SetNextPosition(PlatformPath platformPath, Platform o)
	{
		Vector3 forward = Vector3.forward;
		forward.z *= o.transform.localScale.z;
		forward = platformPath.m_Rotation * forward;
		platformPath.m_NextPosition += forward;
	}

	private void AddNextPosition(PlatformPath platformPath, Vector3 vec)
	{
		platformPath.m_NextPosition += vec;
	}

	public void AddToAllPathsXZPostions(float x, float z)
	{
		m_MainPath.m_NextPosition.x += x;
		m_VeerPathLeft.m_NextPosition.x += x;
		m_VeerPathRight.m_NextPosition.x += x;
		m_MainPath.m_NextPosition.z += z;
		m_VeerPathLeft.m_NextPosition.z += z;
		m_VeerPathRight.m_NextPosition.z += z;
	}

	public void SetAllPathsXZPositions(float x, float z)
	{
		m_MainPath.m_NextPosition.x = x;
		m_VeerPathLeft.m_NextPosition.x = x;
		m_VeerPathRight.m_NextPosition.x = x;
		m_MainPath.m_NextPosition.z = z;
		m_VeerPathLeft.m_NextPosition.z = z;
		m_VeerPathRight.m_NextPosition.z = z;
	}

	public void AddToAllPathsYPositions(float y)
	{
		m_MainPath.m_NextPosition.y += y;
		m_VeerPathLeft.m_NextPosition.y += y;
		m_VeerPathRight.m_NextPosition.y += y;
	}

	private void AddToPathYPosition(PlatformPath p, float y)
	{
		p.m_NextPosition.y += y;
	}

	public void SetAllPathsYPositions(float y)
	{
		m_MainPath.m_NextPosition.y = y;
		m_VeerPathLeft.m_NextPosition.y = y;
		m_VeerPathRight.m_NextPosition.y = y;
	}

	private void ForceNextPlatform(Platform prefab)
	{
		m_ShouldSpawnForceMePrefab = true;
		m_ForceMePrefabs = new Platform[1];
		m_ForceMePrefabs[0] = prefab;
		m_ForceMePrefabsCount = m_ForceMePrefabs.Length;
	}

	private void ForceNextPlatforms(Platform[] prefabArray)
	{
		m_ShouldSpawnForceMePrefab = true;
		m_ForceMePrefabs = prefabArray;
		m_ForceMePrefabsCount = prefabArray.Length;
	}

	public void ResetSpawnMePrefab()
	{
		m_SpawnMeCount = 0;
		m_SpawnMePrefab = null;
	}

	public void ResetForceMePrefabs()
	{
		m_ShouldSpawnForceMePrefab = false;
		m_ForceMePrefabs = null;
		m_ForceMePrefabsCount = 0;
	}

	public void ResetAllSpecialForceMes()
	{
		if (m_IsPushingPlatform)
		{
			m_AreSpecialForceMesQueued = true;
			return;
		}
		ResetSpawnMePrefab();
		ResetForceMePrefabs();
		ResetSpawnSequence();
	}

	private Platform HandleSpawnSequence(bool shouldDecContinuations)
	{
		int num = m_PlatformSequenceCount - 1;
		Platform platform = null;
		platform = ((!m_PlatformSequence[num].m_ShouldSelectFromGroup || m_PlatformSequence[num].m_Platforms == null || m_PlatformSequence[num].m_Platforms.Length != PlatformSequence.ms_PlatformGroupCount) ? m_PlatformSequence[num].m_Platform : ((PlayerData.MomoUpgradeLevel == 0) ? m_PlatformSequence[num].m_Platforms[0] : ((PlayerData.MomoUpgradeLevel == 1) ? m_PlatformSequence[num].m_Platforms[1] : ((PlayerData.MomoUpgradeLevel != 2) ? m_PlatformSequence[num].m_Platforms[0] : m_PlatformSequence[num].m_Platforms[2]))));
		GameEventManager.ExecuteTriggerFromEnum(m_PlatformSequence[num].m_GameEventToExecute);
		m_PlatformSequence[num].m_GameEventToExecute = GameEventManager.TriggerIndeces.None;
		m_SpawnMePrefab = platform;
		Platform platform2 = InstantiatePrefab(platform);
		if (m_PlatformSequence[num].m_ShouldSetPathHeight)
		{
			int num2 = platform2.m_Waypoints.Length - 1;
			m_NewHeight = platform2.m_Waypoints[num2].transform.position.y;
			m_ShouldSetNewHeight = true;
		}
		if (shouldDecContinuations)
		{
			m_PlatformSequence[num].m_Count--;
			if (m_PlatformSequence[num].m_Count <= 0)
			{
				m_PlatformSequence[num] = null;
				m_PlatformSequenceCount--;
			}
			if (m_PlatformSequenceCount <= 0)
			{
				m_PlatformSequence = null;
				m_ShouldSpawnForceMeSequence = false;
				m_SpawnMeCount = platform2.m_SpawnMeCount;
			}
		}
		return platform2;
	}

	private Platform SpawnForceMePrefabs()
	{
		Platform platform = m_ForceMePrefabs[m_ForceMePrefabsCount - 1];
		Platform platform2 = InstantiatePrefab(platform);
		m_ForceMePrefabsCount--;
		m_ForceMePrefabs[m_ForceMePrefabsCount] = null;
		if (m_ForceMePrefabsCount <= 0)
		{
			m_ShouldSpawnForceMePrefab = false;
		}
		m_SpawnMePrefab = platform;
		m_SpawnMeCount = platform2.m_SpawnMeCount;
		return platform2;
	}

	public void ResetSpawnSequence()
	{
		m_ShouldSpawnForceMeSequence = false;
		m_PlatformSequence = null;
		m_PlatformSequenceCount = 0;
		m_ShouldSetNewHeight = false;
		m_NewHeight = 0f;
	}

	private void ForceNextPlatformSequence(PlatformSequence.PlatformTuple[] platformSequence)
	{
		try
		{
			m_PlatformSequence = PlatformSequence.PlatformTuple.Clone(platformSequence);
		}
		catch (ExecutionEngineException ex)
		{
			Debug.Log("ForceNextPlatformSequence - ExecutionEngineException: " + ex.Message + ex.StackTrace);
			return;
		}
		m_ShouldSpawnForceMeSequence = true;
		m_PlatformSequenceCount = platformSequence.Length;
	}

	private void ForceNewStraightAway(Platform prefab, int minStraightAwayCount)
	{
		m_MinStraightAwayCount = minStraightAwayCount;
		m_StraightAwayCur = 0;
		m_NewHeight = 0f;
	}

	private void PushPlatform(PlatformPath platformPath, bool shouldDecContinuations = true)
	{
		m_IsPushingPlatform = true;
		Platform platform;
		if (m_StraightAwayCur < m_MinStraightAwayCount)
		{
			platform = InstantiateMiniGameStartPrefab();
		}
		else if (m_SpawnMeCount > 1 && m_SpawnMePrefab != null)
		{
			if (shouldDecContinuations)
			{
				m_SpawnMeCount--;
			}
			platform = InstantiatePrefab(m_SpawnMePrefab);
		}
		else if (m_ShouldSpawnForceMeSequence && m_PlatformSequence != null)
		{
			platform = HandleSpawnSequence(shouldDecContinuations);
		}
		else if (m_ShouldSpawnForceMePrefab && m_ForceMePrefabs != null)
		{
			platform = SpawnForceMePrefabs();
		}
		else
		{
			Platform platform2 = null;
			if (m_Forking == Forking.Forking && m_ForkingPlatform != null && m_ForkingPlatformCount < 2)
			{
				platform2 = m_ForkingPlatform;
				m_ForkingPlatformCount++;
			}
			else
			{
				platform2 = m_PlatformList.Last.Value;
			}
			platform = InstantiateRandomPrefab(platform2, shouldDecContinuations);
		}
		if (platform.m_ShouldSetNewPathHeight)
		{
			if (platform == null)
			{
				Debug.LogWarning(platform + " is null! this may cause an issue, but I don't know why it's happening");
				return;
			} else{
			AddToAllPathsYPositions(platform.m_NewHeightOffset);
			}
		}
		platform.HandleRotationSequences();
		m_PlatformList.AddLast(platform);
		Vector3 nextPosition = platformPath.m_NextPosition;
		platform.transform.localPosition = Vector3.zero;
		platform.transform.position = nextPosition;
		Platform.PlatformType platformType = platform.m_PlatformType;
		if (m_EagleNoObstaclesPlatformCount > 0)
		{
			m_EagleNoObstaclesPlatformCount--;
		}
		else if (m_StraightAwayCur >= m_NoObstaclesCount)
		{
			platform.InstantiateObstacles(shouldDecContinuations);
		}
		if (m_EagleTokenSpawnPlatformCount > 0)
		{
			m_EagleTokenSpawnPlatformCount--;
			platform.InstantiateEagleTokens();
		}
		platform.InstantiateBeamHoppings(shouldDecContinuations);
		switch (platformType)
		{
		case Platform.PlatformType.HardLeft:
			platform.transform.localRotation *= platformPath.m_Rotation;
			RotatePathAboutY(platformPath, -90f);
			SetNextPosition(platformPath, platform);
			break;
		case Platform.PlatformType.HardRight:
			platform.transform.localRotation *= platformPath.m_Rotation;
			RotatePathAboutY(platformPath, 90f);
			SetNextPosition(platformPath, platform);
			break;
		case Platform.PlatformType.Rotate:
			platform.transform.localRotation *= platformPath.m_Rotation;
			RotatePathAboutY(platformPath, platform.m_RotateYAngle);
			RotatePathAboutX(platformPath, platform.m_RotateXAngle);
			SetNextPosition(platformPath, platform);
			break;
		case Platform.PlatformType.PizzaSlice:
		{
			Vector3 right = platform.transform.right;
			if (platform.m_Waypoints[0].m_PizzaSlice == Waypoint.PizzaSliceType.Right)
			{
				right *= -1f;
			}
			Vector3 vector = right * 500f;
			Vector3 vector2 = right * 500f;
			vector2 = Quaternion.Euler(0f, platform.m_RotateYAngle, 0f) * vector;
			Vector3 vector3 = vector2 - vector;
			Quaternion quaternion = Quaternion.LookRotation(vector3);
			Vector3 vec = platform.m_Waypoints[0].transform.position - vector;
			platform.m_Waypoints[0].SetPizzaCenter(Waypoint.Lane.Left, vec);
			platform.m_Waypoints[0].SetPizzaCenter(Waypoint.Lane.Middle, vec);
			platform.m_Waypoints[0].SetPizzaCenter(Waypoint.Lane.Right, vec);
			platform.transform.localRotation *= platformPath.m_Rotation;
			AddNextPosition(platformPath, platformPath.m_Rotation * vector3);
			platformPath.m_Rotation *= quaternion;
			break;
		}
		case Platform.PlatformType.LeftAndRight:
			platform.transform.localRotation *= platformPath.m_Rotation;
			m_VeerPathLeft.Clone(platformPath);
			m_VeerPathRight.Clone(platformPath);
			RotatePathAboutY(m_VeerPathLeft, -90f);
			RotatePathAboutY(m_VeerPathRight, 90f);
			SetNextPosition(m_VeerPathLeft, platform);
			SetNextPosition(m_VeerPathRight, platform);
			m_ForkingPlatform = platform;
			m_ForkingPlatformCount = 0;
			m_Forking = Forking.Forking;
			m_AllowLeftAndRights = false;
			break;
		case Platform.PlatformType.Ramp:
		{
			float rotateXAngle = platform.m_RotateXAngle;
			float num = Mathf.Sin((float)Math.PI / 180f * (0f - rotateXAngle));
			float y = num * platform.transform.localScale.z * 0.5f;
			RotatePathAboutX(platformPath, rotateXAngle);
			SetNextPosition(platformPath, platform);
			TransformOps.AddLocalPositionY(platform.transform, y);
			platform.transform.localRotation *= platformPath.m_Rotation;
			RotatePathAboutX(platformPath, 0f - rotateXAngle);
			break;
		}
		case Platform.PlatformType.Scaffolding:
			platform.transform.localRotation *= platformPath.m_Rotation;
			SetNextPosition(platformPath, platform);
			break;
		default:
			platform.transform.localRotation *= platformPath.m_Rotation;
			SetNextPosition(platformPath, platform);
			break;
		}
		if (m_ShouldSetNewHeight)
		{
			AddToPathYPosition(platformPath, m_NewHeight);
			m_ShouldSetNewHeight = false;
		}
		platform.InstantiateScenery();
		m_StraightAwayCur++;
		if (m_CurPlatformNode == null)
		{
			m_CurPlatformNode = m_PlatformList.First;
			GameEventManager.TriggerMoveToNextPlatform(m_CurPlatformNode.Value);
		}
		if (m_AreSpecialForceMesQueued)
		{
			ResetSpawnMePrefab();
			ResetForceMePrefabs();
			ResetSpawnSequence();
			m_AreSpecialForceMesQueued = false;
		}
		m_IsPushingPlatform = false;
	}

	private bool IsRunnerFinishedWithFirstPlatform()
	{
		Platform value = m_PlatformList.First.Value;
		for (int i = 0; i < value.m_Waypoints.Length; i++)
		{
			if (!value.m_Waypoints[i].GetIsRunnerFinished())
			{
				return false;
			}
		}
		return true;
	}

	private bool IsRunnerFinishedWithSecondPlatform()
	{
		LinkedListNode<Platform> first = m_PlatformList.First;
		first = first.Next;
		if (first == null)
		{
			Debug.LogError("PlatformManager - IsRunnerFinishedWithSecondPlatform() Second Node is null.  This should never happen");
		}
		Platform value = first.Value;
		for (int i = 0; i < value.m_Waypoints.Length; i++)
		{
			if (!value.m_Waypoints[i].GetIsRunnerFinished())
			{
				return false;
			}
		}
		return true;
	}

	public void InstantiateEagleTokens(int indexStart, int indexEnd)
	{
		int num = 0;
		foreach (Platform platform in m_PlatformList)
		{
			if (num >= indexStart && num < indexEnd)
			{
				platform.InstantiateEagleTokens();
			}
			num++;
		}
	}

	public void DestroyObstaclesAndTokensOnAllPlatforms()
	{
		foreach (Platform platform in m_PlatformList)
		{
			platform.DestroyObstaclesAndTokens();
		}
	}

	private void PopAllPlatforms()
	{
		while (m_PlatformList.Count > 0)
		{
			Platform value = m_PlatformList.First.Value;
			m_PlatformList.RemoveFirst();
			value.DestroyAddOns();
			CacheManager.The().Unspawn(value);
		}
		m_PlatformList.Clear();
	}

	private void PopPlatform()
	{
		Platform value = m_PlatformList.First.Value;
		m_PlatformList.RemoveFirst();
		value.DestroyAddOns();
		CacheManager.The().Unspawn(value);
	}

	private void Recycle(PlatformPath platformPath, bool shouldDecContinuations = true)
	{
		PopPlatform();
		PushPlatform(platformPath, shouldDecContinuations);
	}

	public Vector3 RetrieveForwardVec()
	{
		return m_CurPlatformNode.Value.transform.rotation * Vector3.forward;
	}

	public Vector3 RetrieveRightVec()
	{
		return m_CurPlatformNode.Value.transform.rotation * Vector3.right;
	}

	public Quaternion RetrieveRotationCurPlatform()
	{
		return m_CurPlatformNode.Value.transform.rotation;
	}

	public Quaternion RetrieveRotation()
	{
		return m_MainPath.m_Rotation;
	}

	private void ResetPlatformManager()
	{
		m_MainPath.Identity();
		m_VeerPathLeft.Identity();
		m_VeerPathRight.Identity();
		m_LeftList.Clear();
		m_RightList.Clear();
		m_ChosenLane = Waypoint.Lane.None;
		m_Forking = Forking.NotForking;
		m_ForkingPlatform = null;
		m_ForkingPlatformCount = 0;
		m_AllowLeftAndRights = true;
		m_CurPlatformNode = null;
		DestroyLeftOverPlatforms();
	}

	public void ReloadLevel()
	{
		UnloadPlatformPrefabs();
		m_StraightAwayCur = 0;
		ResetPlatformManager();
		Platform.ClearIgnoreMeList();
		LoadPlatformPrefabs();
	}

	private void GameIntro()
	{
		Platform.ResetAll();
		ResetAllSpecialForceMes();
		MiniGameManager.The().m_BabyHeadRules.ResetCalculationsAll();
		MiniGameManager.The().m_DuckyMomoRules.ResetCalculationsAll();
		if (!m_PlatformsLoadedForTheFirstTime && m_ArePlatformsLoaded)
		{
			ResetPlatformManager();
			Platform.ClearIgnoreMeList();
			ReloadLevel();
		}
		if (!m_ArePlatformsLoaded)
		{
			LoadPlatformPrefabs();
		}
		m_PlatformsLoadedForTheFirstTime = false;
	}

	private void GameStart()
	{
	}

	private void GameOver()
	{
	}

	private void GameContinueEvent()
	{
	}

	public void MinusOffMaxWorldBoundary(float x, float z)
	{
		foreach (Platform platform in m_PlatformList)
		{
			TransformOps.AddPositionXZ(platform.transform, 0f - x, 0f - z);
		}
		AddToAllPathsXZPostions(0f - x, 0f - z);
	}

	public void RemoveYawRoll()
	{
		m_MainPath.m_Rotation.eulerAngles = new Vector3(0f, m_MainPath.m_Rotation.eulerAngles.y, 0f);
		m_VeerPathLeft.m_Rotation.eulerAngles = new Vector3(0f, m_VeerPathLeft.m_Rotation.eulerAngles.y, 0f);
		m_VeerPathRight.m_Rotation.eulerAngles = new Vector3(0f, m_VeerPathRight.m_Rotation.eulerAngles.y, 0f);
	}

	public void RemoveRoll()
	{
		m_MainPath.m_Rotation.eulerAngles = new Vector3(m_MainPath.m_Rotation.eulerAngles.x, m_MainPath.m_Rotation.eulerAngles.y, 0f);
		m_VeerPathLeft.m_Rotation.eulerAngles = new Vector3(m_VeerPathLeft.m_Rotation.eulerAngles.x, m_VeerPathLeft.m_Rotation.eulerAngles.y, 0f);
		m_VeerPathRight.m_Rotation.eulerAngles = new Vector3(m_VeerPathRight.m_Rotation.eulerAngles.x, m_VeerPathRight.m_Rotation.eulerAngles.y, 0f);
	}

	private void HangGlideEndPlatformListener()
	{
		RemoveRoll();
	}
}

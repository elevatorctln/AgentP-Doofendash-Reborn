using System.Collections.Generic;
using UnityEngine;

public class PathStateMachine : MonoBehaviour
{
	public enum PathMessages
	{
		StartedPath = 0,
		FinishedPath = 1,
		StartedRotation = 2,
		FinishedRotation = 3
	}

	public enum BossRotationState
	{
		LookAtPerry = 0,
		LookAtPerryBase = 1,
		LookAtPerryXZBase = 2,
		LookAtPerryBaseReverse = 3,
		LookAtPerryXZBaseReverse = 4,
		LookAtPerryTimed = 5,
		FollowPathRotation = 6,
		NoRotation = 7,
		LookAtPoint = 8,
		RotateToQuat = 9
	}

	public delegate void PathMessageHandler(PathMessages pathMessage);

	public delegate void PathEventHandler(EventPoint eventPoint);

	protected delegate void PathState();

	public Runner m_TargetPerry;

	protected Vector3 m_OffsetToPerry;

	protected Vector3 m_BaseDoofPosition;

	private DPath m_CurPath;

	private Dictionary<string, DPath> m_PathDictionary;

	private DPath[] m_Paths;

	private Vector3 m_PathFirstPoint;

	private ControlPoint m_ControlPointCur;

	private BossRotationState m_BossRotationState = BossRotationState.NoRotation;

	private TimedRotation m_TimedRotation = new TimedRotation();

	public Runner TargetPerry
	{
		get
		{
			return m_TargetPerry;
		}
	}

	public DPath CurPath
	{
		get
		{
			return m_CurPath;
		}
		set
		{
			m_CurPath = value;
		}
	}

	public ControlPoint ControlPointCur
	{
		get
		{
			return m_ControlPointCur;
		}
		set
		{
			m_ControlPointCur = value;
		}
	}

	public event PathMessageHandler PathMessageEvents;

	public event PathEventHandler PathEvents;

	private event PathState m_PathState;

	protected void SetDoofState(PathState pathState)
	{
		this.m_PathState = pathState;
	}

	protected PathState GetDoofState()
	{
		return this.m_PathState;
	}

	public void SetRotationState(BossRotationState bossRotationState)
	{
		m_BossRotationState = bossRotationState;
	}

	public void SetRotationState(BossRotationState bossRotationState, Quaternion customRotation, float duration)
	{
		m_BossRotationState = bossRotationState;
		m_TimedRotation.Start(base.transform.rotation, customRotation, duration);
	}

	public BossRotationState GetRotationState()
	{
		return m_BossRotationState;
	}

	private void UpdateBossRotationState(Quaternion rotation)
	{
		switch (m_BossRotationState)
		{
		case BossRotationState.LookAtPerry:
			LookAtPerry();
			break;
		case BossRotationState.LookAtPerryBase:
			LookAtPerryBase();
			break;
		case BossRotationState.LookAtPerryXZBase:
			LookAtPerryXZBase();
			break;
		case BossRotationState.LookAtPerryBaseReverse:
			LookAtPerryBaseReverse();
			break;
		case BossRotationState.LookAtPerryXZBaseReverse:
			LookAtPerryXZBaseReverse();
			break;
		case BossRotationState.LookAtPerryTimed:
		case BossRotationState.RotateToQuat:
			RotateToQuat();
			break;
		case BossRotationState.FollowPathRotation:
			FollowPathRotation(rotation);
			break;
		case BossRotationState.LookAtPoint:
			LookAtPoint();
			break;
		case BossRotationState.NoRotation:
			break;
		}
	}

	private void Awake()
	{
		Init();
	}

	private void Start()
	{
	}

	protected void Init()
	{
		this.m_PathState = FollowBase;
		m_BossRotationState = BossRotationState.LookAtPerry;
		m_OffsetToPerry = base.transform.position - m_TargetPerry.transform.position;
		m_BaseDoofPosition = base.transform.position;
		CreatePaths();
		DisableMeshRenderers();
	}

	private void Update()
	{
	}

	public void SetOffsetToPerry(Vector3 newOffset)
	{
		m_OffsetToPerry = newOffset;
	}

	public void RecalcOffsetToPerry()
	{
		m_OffsetToPerry = Quaternion.Inverse(m_TargetPerry.transform.rotation) * (base.transform.position - m_TargetPerry.CalcRunnerLaneBasePosition(Waypoint.Lane.Middle));
	}

	public void PathUpdate()
	{
		if (GameManager.The.IsInGamePlay())
		{
			this.m_PathState();
		}
	}

	private void CreatePaths()
	{
		DPath[] array = RetrievePaths();
		DPath[] array2 = array;
		foreach (DPath dPath in array2)
		{
			dPath.CreateBezierLocal();
		}
	}

	private void DisableMeshRenderers()
	{
		DPath[] array = RetrievePaths();
		DPath[] array2 = array;
		foreach (DPath dPath in array2)
		{
			dPath.DisableMeshRenderers();
		}
	}

	private DPath[] RetrievePaths()
	{
		if (m_Paths == null)
		{
			m_Paths = GetComponentsInChildren<DPath>();
		}
		return m_Paths;
	}

	private Dictionary<string, DPath> RetrievePathDictionary()
	{
		if (m_PathDictionary == null)
		{
			m_PathDictionary = new Dictionary<string, DPath>();
			DPath[] componentsInChildren = GetComponentsInChildren<DPath>();
			DPath[] array = componentsInChildren;
			foreach (DPath dPath in array)
			{
				m_PathDictionary.Add(dPath.name, dPath);
			}
		}
		return m_PathDictionary;
	}

	public void StartPathStayOnFirstPoint(string name)
	{
		DPath dPath = RetrievePathDictionary()[name];
		m_PathFirstPoint = dPath.FirstPoint();
		this.m_PathState = FollowPathFirstPosition;
		FollowPathPosition(m_PathFirstPoint);
	}

	public void StartOnPath(string name)
	{
		DPath curPath = RetrievePathDictionary()[name];
		m_CurPath = curPath;
		m_CurPath.IsPaused = false;
		m_CurPath.StartDistanceBased();
		this.m_PathState = FollowPath;
	}

	public void StartRotateTowardPath(string name, float duration)
	{
		Quaternion quaternion = (m_CurPath = RetrievePathDictionary()[name]).RetrieveRotationTimeBased(0f) * m_TargetPerry.transform.rotation;
		m_TimedRotation.Start(base.transform.rotation, quaternion, duration);
		SetRotationState(BossRotationState.RotateToQuat, quaternion, duration);
	}

	public void PauseCurrentPath()
	{
		if (m_CurPath != null)
		{
			m_CurPath.IsPaused = true;
		}
	}

	public void UnpauseCurrentPath()
	{
		if (m_CurPath != null)
		{
			m_CurPath.IsPaused = false;
		}
	}

	public bool IsCurrentPathPaused()
	{
		if (m_CurPath != null)
		{
			return m_CurPath.IsPaused;
		}
		return false;
	}

	private void FollowBasePosition()
	{
		Vector3 position = m_TargetPerry.CalcRunnerLaneBasePosition(Waypoint.Lane.Middle);
		Vector3 vector = m_TargetPerry.transform.rotation * m_OffsetToPerry;
		position += vector;
		base.transform.position = position;
	}

	private void FollowPathPosition(Vector3 pathOffset)
	{
		Vector3 position = m_TargetPerry.CalcRunnerLaneBasePosition(Waypoint.Lane.Middle);
		Vector3 vector = m_TargetPerry.transform.rotation * (m_OffsetToPerry + pathOffset);
		position += vector;
		base.transform.position = position;
	}

	private void FollowPathRotation(Quaternion rotation)
	{
		base.transform.rotation = rotation * m_TargetPerry.transform.rotation;
	}

	private void RotateToQuat()
	{
		Quaternion rotation;
		bool isFinished;
		m_TimedRotation.NextRotation(out rotation, out isFinished);
		base.transform.rotation = rotation;
		if (isFinished)
		{
			OnPathMessage(PathMessages.FinishedRotation);
			if (this.PathMessageEvents != null)
			{
				this.PathMessageEvents(PathMessages.FinishedRotation);
			}
		}
	}

	private void LookAtPoint()
	{
		Debug.LogWarning("LookAtPoint Not Yet Implemented");
	}

	private void LookAtPerry()
	{
		Vector3 forward = m_TargetPerry.transform.position - base.transform.position;
		Quaternion to = QuatOps.QuatFromForwardVec(forward);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, to, 0.2f);
	}

	private void LookAtPerryBase()
	{
		Vector3 forward = m_TargetPerry.transform.rotation * -m_OffsetToPerry;
		Quaternion to = QuatOps.QuatFromForwardVec(forward);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, to, 0.2f);
	}

	private void LookAtPerryXZBase()
	{
		Vector3 forward = m_TargetPerry.transform.rotation * -m_OffsetToPerry;
		forward.y = 0f;
		Quaternion to = QuatOps.QuatFromForwardVec(forward);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, to, 0.2f);
	}

	private void LookAtPerryBaseReverse()
	{
		Vector3 forward = m_TargetPerry.transform.rotation * Quaternion.Euler(0f, 180f, 0f) * -m_OffsetToPerry;
		Quaternion to = QuatOps.QuatFromForwardVec(forward);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, to, 0.2f);
	}

	private void LookAtPerryXZBaseReverse()
	{
		Vector3 forward = m_TargetPerry.transform.rotation * Quaternion.Euler(0f, 180f, 0f) * -m_OffsetToPerry;
		forward.y = 0f;
		Quaternion to = QuatOps.QuatFromForwardVec(forward);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, to, 0.2f);
	}

	public void LookAtPerryTimedStart(float duration)
	{
		Vector3 forward = m_TargetPerry.transform.rotation * -m_OffsetToPerry;
		Quaternion desiredRotation = QuatOps.QuatFromForwardVec(forward);
		m_TimedRotation.Start(base.transform.rotation, desiredRotation, duration);
		SetRotationState(BossRotationState.LookAtPerryTimed);
	}

	public void LookAtPerryXZTimedStart(float duration)
	{
		Vector3 forward = m_TargetPerry.transform.rotation * -m_OffsetToPerry;
		forward.y = 0f;
		Quaternion desiredRotation = QuatOps.QuatFromForwardVec(forward);
		m_TimedRotation.Start(base.transform.rotation, desiredRotation, duration);
		SetRotationState(BossRotationState.LookAtPerryTimed);
	}

	protected void FollowPath()
	{
		Vector3 point;
		Quaternion rotation;
		EventPoint eventPoint;
		m_CurPath.NextPointAndRotationDistanceBased(out point, out rotation, out eventPoint);
		FollowPathPosition(point);
		UpdateBossRotationState(rotation);
		HandlePathEvents(eventPoint);
		if (m_CurPath.IsPathCompleted())
		{
			OnPathMessage(PathMessages.FinishedPath);
			this.m_PathState = FollowBase;
			if (this.PathMessageEvents != null)
			{
				this.PathMessageEvents(PathMessages.FinishedPath);
			}
		}
	}

	protected void FollowBase()
	{
		FollowBasePosition();
		UpdateBossRotationState(Quaternion.identity);
	}

	protected void FollowPathFirstPosition()
	{
		FollowPathPosition(m_PathFirstPoint);
		UpdateBossRotationState(Quaternion.identity);
	}

	public virtual void OnPathMessage(PathMessages pathMessage)
	{
	}

	public virtual void OnPathEvent(EventPoint eventPoint)
	{
	}

	protected virtual void HandlePathEvents(EventPoint eventPoint)
	{
		if (eventPoint != null)
		{
			OnPathEvent(eventPoint);
			if (this.PathEvents != null)
			{
				this.PathEvents(eventPoint);
			}
		}
	}

	public bool IsOnPath()
	{
		return this.m_PathState == new PathState(FollowPath);
	}
}

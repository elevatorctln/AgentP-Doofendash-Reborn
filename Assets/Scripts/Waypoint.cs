using UnityEngine;

public class Waypoint : MonoBehaviour
{
	public enum Lane
	{
		Left = -1,
		Middle = 0,
		Right = 1,
		None = 2,
		AllOk = 3,
		LeftOrRight = 4
	}

	public enum Triggers
	{
		HangGlideEnd = 0,
		None = 1,
		HangGlideDiveDown = 2
	}

	public enum PizzaSliceType
	{
		None = 0,
		Left = 1,
		Right = 2,
		Up = 3,
		Down = 4
	}

	private bool m_IsPassed;

	private bool m_IsRunnerFinished;

	private Vector3[] m_OriginalLocalPosition;

	public Transform m_LeftLane;

	public Transform m_RightLane;

	public Lane m_LaneToBeIn = Lane.AllOk;

	[HideInInspector]
	public int m_LaneForPlacementOnDeath;

	[HideInInspector]
	public bool m_IsLanePlacementFromObstacleSequence;

	public bool m_TurnConstrain;

	public bool m_IsLaneWidthChange;

	public bool m_FirstInRamp;

	public bool m_FirstInSplitScaffoldRamp;

	public Triggers m_TriggerType = Triggers.None;

	public PizzaSliceType m_PizzaSlice;

	[HideInInspector]
	public Vector3[] m_PizzaCenter;

	private Vector3 m_SliceVecLeftLane;

	private Vector3 m_SliceVecMiddleLane;

	private Vector3 m_SliceVecRightLane;

	[HideInInspector]
	public float m_SliceAngle;

	public float m_LaneDamp = 0.2f;

	public float m_SlerpFactor = 0.2f;

	public float m_Radius = 30f;

	public static Lane IncLaneLeft(Lane lane)
	{
		if (lane > Lane.Left)
		{
			lane--;
		}
		return lane;
	}

	public static Lane IncLaneRight(Lane lane)
	{
		if (lane < Lane.Right)
		{
			lane++;
		}
		return lane;
	}

	public void ResetWaypoint()
	{
		m_IsPassed = false;
		m_IsRunnerFinished = false;
		m_LaneForPlacementOnDeath = 0;
		m_IsLanePlacementFromObstacleSequence = false;
		ResetOriginalPositionRotation();
	}

	public Transform RetrieveTransform(Lane lane)
	{
		switch (lane)
		{
		case Lane.Left:
			return m_LeftLane.transform;
		case Lane.Right:
			return m_RightLane.transform;
		default:
			return base.transform;
		}
	}

	public void SetSliceVec(Lane lane, Vector3 vec)
	{
		switch (lane)
		{
		case Lane.Left:
			m_SliceVecLeftLane = vec;
			break;
		case Lane.Right:
			m_SliceVecRightLane = vec;
			break;
		case Lane.Middle:
			m_SliceVecMiddleLane = vec;
			break;
		default:
			Debug.LogWarning(string.Concat("SetSliceVec() called on Lane - ", lane, " Which is Invalid"));
			break;
		}
	}

	public Vector3 GetSliceVec(Lane lane)
	{
		switch (lane)
		{
		case Lane.Left:
			return m_SliceVecLeftLane;
		case Lane.Right:
			return m_SliceVecRightLane;
		case Lane.Middle:
			return m_SliceVecMiddleLane;
		default:
			Debug.LogWarning(string.Concat("GetSliceVec() called on Lane - ", lane, " Which is Invalid"));
			return Vector3.zero;
		}
	}

	public void SetPizzaCenter(Lane lane, Vector3 vec)
	{
		switch (lane)
		{
		case Lane.Left:
			m_PizzaCenter[0] = vec;
			break;
		case Lane.Middle:
			m_PizzaCenter[1] = vec;
			break;
		case Lane.Right:
			m_PizzaCenter[2] = vec;
			break;
		default:
			Debug.LogWarning(string.Concat("SetPizzaCenter() called on Lane - ", lane, " Which is Invalid"));
			break;
		}
	}

	public Vector3 GetPizzaCenter(Lane lane)
	{
		switch (lane)
		{
		case Lane.Left:
			return m_PizzaCenter[0];
		case Lane.Middle:
			return m_PizzaCenter[1];
		case Lane.Right:
			return m_PizzaCenter[2];
		default:
			Debug.LogWarning(string.Concat("GetPizzaCenter() called on Lane - ", lane, " Which is Invalid"));
			return Vector3.zero;
		}
	}

	public bool IsPizzaSlice()
	{
		return m_PizzaSlice != PizzaSliceType.None;
	}

	public bool IsPizzaSliceHorizontal()
	{
		bool result = false;
		if (m_PizzaSlice == PizzaSliceType.Left || m_PizzaSlice == PizzaSliceType.Right)
		{
			result = true;
		}
		return result;
	}

	public bool IsPizzaSliceVertical()
	{
		bool result = false;
		if (m_PizzaSlice == PizzaSliceType.Up || m_PizzaSlice == PizzaSliceType.Down)
		{
			result = true;
		}
		return result;
	}

	private void InitOriginalLocalPosition()
	{
		if (m_OriginalLocalPosition == null)
		{
			m_OriginalLocalPosition = new Vector3[3];
			m_OriginalLocalPosition[0] = m_LeftLane.transform.localPosition;
			m_OriginalLocalPosition[1] = base.transform.localPosition;
			m_OriginalLocalPosition[2] = m_RightLane.transform.localPosition;
		}
	}

	private void UninitOriginalPositionRotation()
	{
		m_OriginalLocalPosition = null;
	}

	private void ResetOriginalPositionRotation()
	{
		if (m_OriginalLocalPosition == null)
		{
			InitOriginalLocalPosition();
		}
		base.transform.position = base.transform.parent.position;
		base.transform.localPosition = m_OriginalLocalPosition[1];
		base.transform.rotation = base.transform.parent.rotation;
		m_LeftLane.transform.position = base.transform.position;
		m_LeftLane.transform.localPosition = m_OriginalLocalPosition[0];
		m_LeftLane.transform.rotation = base.transform.parent.rotation;
		m_RightLane.transform.position = base.transform.position;
		m_RightLane.transform.localPosition = m_OriginalLocalPosition[2];
		m_RightLane.transform.rotation = base.transform.parent.rotation;
	}

	private void Awake()
	{
		m_PizzaCenter = new Vector3[3];
		InitOriginalLocalPosition();
	}

	private void Update()
	{
	}

	public void SetIsPassed(bool isPassed)
	{
		m_IsPassed = isPassed;
	}

	public bool GetIsPassed()
	{
		return m_IsPassed;
	}

	public void SetIsRunnerFinished(bool isRunnerFinished)
	{
		m_IsRunnerFinished = isRunnerFinished;
	}

	public bool GetIsRunnerFinished()
	{
		return m_IsRunnerFinished;
	}

	public void BringUpToMaxZDepth(Lane lane, int laneLocations)
	{
		Transform transform = RetrieveTransform(lane);
		if ((laneLocations & LaneLocations.L) != LaneLocations.L)
		{
			Transform transform2 = RetrieveTransform(Lane.Left);
			Vector3 position = MathOps.IntersectPointLineLine(transform.position, transform.right, transform2.position, transform2.forward);
			transform2.position = position;
		}
		if ((laneLocations & LaneLocations.M) != LaneLocations.M)
		{
			Transform transform3 = RetrieveTransform(Lane.Middle);
			Vector3 position2 = MathOps.IntersectPointLineLine(transform.position, transform.right, transform3.position, transform3.forward);
			transform3.position = position2;
		}
		if ((laneLocations & LaneLocations.R) != LaneLocations.R)
		{
			Transform transform4 = RetrieveTransform(Lane.Right);
			Vector3 position3 = MathOps.IntersectPointLineLine(transform.position, transform.right, transform4.position, transform4.forward);
			transform4.position = position3;
		}
	}
}

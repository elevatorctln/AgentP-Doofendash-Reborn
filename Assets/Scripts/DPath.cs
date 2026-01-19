using UnityEngine;

[ExecuteInEditMode]
public class DPath : MonoBehaviour
{
	public enum CurveType
	{
		StartToEnd = 0,
		EndToStart = 1,
		OutAndBack = 2
	}

	public enum Direction
	{
		GoingOut = 0,
		ComingBack = 1
	}

	private Bezier m_Bezier;

	public float m_SecondsStartToFinish = 5f;

	public float m_EndMinusStartLength;

	public float m_SpawnDelta = 5f;

	public float m_LoopRadius = 50f;

	private EventPoint[] m_EventPoints;

	private bool m_IsPaused;

	private float m_ArcPosCur;

	private float m_ArcSpeed;

	public CurveType m_CurveType;

	private Direction m_Direction;

	public bool IsPaused
	{
		get
		{
			return m_IsPaused;
		}
		set
		{
			m_IsPaused = value;
		}
	}

	public int ControlPointCount
	{
		get
		{
			ControlPoint[] componentsInChildren = GetComponentsInChildren<ControlPoint>();
			if (componentsInChildren == null)
			{
				return 0;
			}
			return componentsInChildren.Length;
		}
	}

	public float TotalLength
	{
		get
		{
			return m_Bezier.TotalLength;
		}
	}

	private void Awake()
	{
		FindEventPoints();
	}

	private void FindEventPoints()
	{
		m_EventPoints = GetComponentsInChildren<EventPoint>();
		ResetEventPoints();
	}

	private void ResetEventPoints()
	{
		if (m_EventPoints == null)
		{
			m_EventPoints = GetComponentsInChildren<EventPoint>();
		}
		EventPoint[] eventPoints = m_EventPoints;
		foreach (EventPoint eventPoint in eventPoints)
		{
			eventPoint.BeenTriggered = false;
		}
	}

	private EventPoint FindTriggeredEventPoint(float t)
	{
		EventPoint[] eventPoints = m_EventPoints;
		foreach (EventPoint eventPoint in eventPoints)
		{
			if (!eventPoint.BeenTriggered && MathOps.AboutEqual(t, eventPoint.m_Time, eventPoint.m_TriggerEpsilon_0to1))
			{
				eventPoint.BeenTriggered = true;
				return eventPoint;
			}
		}
		return null;
	}

	public Vector3[] ControlPointsWorld()
	{
		ControlPoint[] componentsInChildren = GetComponentsInChildren<ControlPoint>();
		if (componentsInChildren.Length > 2)
		{
			Vector3[] array = new Vector3[componentsInChildren.Length];
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				array[i] = componentsInChildren[i].transform.position;
			}
			return array;
		}
		return null;
	}

	public void CreateBezierWorld()
	{
		Vector3[] array = ControlPointsWorld();
		if (array != null)
		{
			m_Bezier = new Bezier(array.Length, array);
		}
	}

	public void UpdateBezierWorld()
	{
		Vector3[] array = ControlPointsWorld();
		if (array != null)
		{
			if (m_Bezier.ControlPointCount != array.Length)
			{
				m_Bezier = new Bezier(array.Length, array);
			}
			else
			{
				m_Bezier.Update(array);
			}
		}
	}

	public Vector3[] ControlPointsLocal()
	{
		ControlPoint[] componentsInChildren = GetComponentsInChildren<ControlPoint>();
		if (componentsInChildren.Length > 2)
		{
			Vector3[] array = new Vector3[componentsInChildren.Length];
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				array[i] = componentsInChildren[i].transform.position - componentsInChildren[i].transform.root.position;
			}
			return array;
		}
		return null;
	}

	public void CreateBezierLocal()
	{
		Vector3[] array = ControlPointsLocal();
		if (array != null)
		{
			m_Bezier = new Bezier(array.Length, array);
		}
	}

	public void UpdateBezierLocal()
	{
		Vector3[] array = ControlPointsLocal();
		if (array != null)
		{
			m_Bezier.Update(array);
		}
	}

	public void DisableMeshRenderers()
	{
		MeshRenderer[] componentsInChildren = GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
	}

	public bool IsBezierInited()
	{
		return m_Bezier != null;
	}

	public Vector3 RetrievePositionTimeBased(float t)
	{
		return m_Bezier.GetPosition(t);
	}

	public Vector3 RetrievePositionArcLengthBased(float s)
	{
		return m_Bezier.GetPositionFromLength(s);
	}

	public Quaternion RetrieveRotationTimeBased(float t)
	{
		return QuatOps.QuatFromForwardVec(m_Bezier.GetFirstDerivative(t));
	}

	public Quaternion RetrieveRotationArcLengthBased(float s)
	{
		return QuatOps.QuatFromForwardVec(m_Bezier.GetFirstDerivativeFromLength(s));
	}

	public void StartDistanceBased()
	{
		if (!IsBezierInited())
		{
			CreateBezierLocal();
		}
		ResetEventPoints();
		m_ArcSpeed = TotalLength / m_SecondsStartToFinish;
		m_ArcPosCur = 0f;
		m_Direction = Direction.GoingOut;
		if (m_CurveType == CurveType.EndToStart)
		{
			m_ArcPosCur = TotalLength;
			m_Direction = Direction.ComingBack;
			m_ArcSpeed = 0f - m_ArcSpeed;
		}
	}

	public Vector3 FirstPoint()
	{
		return m_Bezier.GetPosition(0f);
	}

	public Vector3 NextPointDistanceBased()
	{
		Vector3 positionFromLength = m_Bezier.GetPositionFromLength(m_ArcPosCur);
		if (!m_IsPaused)
		{
			float num = m_ArcSpeed * Time.deltaTime;
			m_ArcPosCur += num;
		}
		if (m_CurveType == CurveType.OutAndBack && m_Direction == Direction.GoingOut && m_ArcPosCur > TotalLength)
		{
			m_Direction = Direction.ComingBack;
			m_ArcSpeed = 0f - m_ArcSpeed;
		}
		return positionFromLength;
	}

	public void NextPointAndRotationDistanceBased(out Vector3 point, out Quaternion rotation, out EventPoint eventPoint)
	{
		Vector3 tangent;
		NextPointAndTangentDistanceBased(out point, out tangent, out eventPoint);
		rotation = QuatOps.QuatFromForwardVec(tangent);
	}

	public void NextPointAndRotationXZDistanceBased(out Vector3 point, out Quaternion rotation, out EventPoint eventPoint)
	{
		Vector3 tangent;
		NextPointAndTangentDistanceBased(out point, out tangent, out eventPoint);
		rotation = QuatOps.QuatFromForwardVecXZ(tangent);
	}

	private void NextPointAndTangentDistanceBased(out Vector3 point, out Vector3 tangent, out EventPoint eventPoint)
	{
		float t;
		m_Bezier.GetPositionAndFirstDerivativeFromLength(m_ArcPosCur, out t, out point, out tangent);
		if (m_CurveType == CurveType.EndToStart)
		{
			tangent = -tangent;
		}
		eventPoint = FindTriggeredEventPoint(t);
		if (!m_IsPaused)
		{
			float num = m_ArcSpeed * Time.deltaTime;
			m_ArcPosCur += num;
		}
		if (m_CurveType == CurveType.OutAndBack && m_Direction == Direction.GoingOut && m_ArcPosCur > TotalLength)
		{
			m_Direction = Direction.ComingBack;
			m_ArcSpeed = 0f - m_ArcSpeed;
		}
	}

	public bool IsPathCompleted()
	{
		if (m_CurveType == CurveType.StartToEnd && m_ArcPosCur > TotalLength)
		{
			return true;
		}
		if (m_CurveType == CurveType.EndToStart && m_ArcPosCur < 0f)
		{
			return true;
		}
		if (m_CurveType == CurveType.OutAndBack && m_Direction == Direction.ComingBack && m_ArcPosCur < 0f)
		{
			return true;
		}
		return false;
	}
}

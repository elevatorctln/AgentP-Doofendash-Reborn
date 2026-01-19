using UnityEngine;

public class TokenGroup : MonoBehaviour
{
	public enum WeavyPathElement
	{
		L = 0,
		M = 1,
		R = 2
	}

	public enum WeavyPath
	{
		L_To_M = 0,
		M_To_L = 1,
		M_To_R = 2,
		M_To_L_To_M = 3,
		M_To_R_To_M = 4,
		R_To_M = 5,
		None = 6
	}

	private const int L = 0;

	private const int M = 1;

	private const int R = 2;

	public float m_Probability = 0.5f;

	public WeavyPathElement[] m_WeavyPathElements;

	public bool m_IsOverObstacle;

	[HideInInspector]
	public Object m_ParentObject;

	[HideInInspector]
	public float m_DeltaZ = 0.1f;

	[HideInInspector]
	public int m_ControlPointCount = 3;

	[HideInInspector]
	public Vector3[] m_ControlPoints;

	[HideInInspector]
	public float[] m_ControlPointsOffsetX;

	[HideInInspector]
	public float[] m_ControlPointsOffsetY;

	public Bezier m_Bezier;

	private Token[] m_Tokens;

	private Vector3 m_LocalOffset;

	public Object ParentObject
	{
		get
		{
			return m_ParentObject;
		}
		set
		{
			m_ParentObject = value;
		}
	}

	public float DeltaZ
	{
		get
		{
			return m_DeltaZ;
		}
		set
		{
			m_DeltaZ = value;
		}
	}

	public int ControlPointCount
	{
		get
		{
			return m_ControlPointCount;
		}
		set
		{
			m_ControlPointCount = value;
		}
	}

	public Vector3[] ControlPoints
	{
		get
		{
			return m_ControlPoints;
		}
		set
		{
			m_ControlPoints = value;
		}
	}

	public float[] ControlPointsOffsetX
	{
		get
		{
			return m_ControlPointsOffsetX;
		}
		set
		{
			m_ControlPointsOffsetX = value;
		}
	}

	public float[] ControlPointsOffsetY
	{
		get
		{
			return m_ControlPointsOffsetY;
		}
		set
		{
			m_ControlPointsOffsetY = value;
		}
	}

	public static int LaneNotIncluded(WeavyPathElement[] weavyPath)
	{
		for (int i = 0; i < 3; i++)
		{
			bool flag = false;
			for (int j = 0; j < weavyPath.Length; j++)
			{
				if (weavyPath[j] == (WeavyPathElement)i)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return i;
			}
		}
		return 0;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void ResetTokenGroup()
	{
		if (m_Tokens != null)
		{
			for (int i = 0; i < m_Tokens.Length; i++)
			{
				m_Tokens[i].transform.rotation = Quaternion.identity;
				m_Tokens[i].gameObject.SetActive(true);
				m_Tokens[i].TokenReset();
			}
		}
	}

	public void Init()
	{
	}

	public void Uninit()
	{
		if (m_Tokens != null)
		{
			for (int i = 0; i < m_Tokens.Length; i++)
			{
				m_Tokens[i].UnspawnHalo();
				m_Tokens[i].UnspawnParticles();
			}
		}
	}

	public void StopCullingCollision()
	{
		for (int i = 0; i < m_Tokens.Length; i++)
		{
			m_Tokens[i].StopCullingCollision();
		}
	}

	public void StopCullingMagnet()
	{
		for (int i = 0; i < m_Tokens.Length; i++)
		{
			m_Tokens[i].StopCullingMagnet();
		}
	}

	public void StopCullingParticles()
	{
		for (int i = 0; i < m_Tokens.Length; i++)
		{
			m_Tokens[i].StopCullingParticles();
		}
	}

	public void StopCullingRotation()
	{
		for (int i = 0; i < m_Tokens.Length; i++)
		{
			m_Tokens[i].StopCullingRotation();
		}
	}

	private void Awake()
	{
		m_LocalOffset = base.transform.position;
		m_Tokens = GetComponentsInChildren<Token>();
	}

	public static TokenGroup Spawn(TokenGroup tokenGroup, Transform parent, float laneOffset, int lane)
	{
		Object prefab = tokenGroup.gameObject;
		GameObject gameObject = CacheManager.The().Spawn(prefab);
		TokenGroup component = gameObject.GetComponent<TokenGroup>();
		Vector3 localOffset = component.m_LocalOffset;
		gameObject.transform.position = parent.transform.position;
		gameObject.transform.rotation = parent.transform.rotation;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.parent = parent.transform;
		TransformOps.SetLocalPositionX(gameObject.transform, Obstacle.CalcLanePosition(lane, laneOffset));
		TransformOps.AddPositionY(gameObject.transform, localOffset.y * parent.transform.localScale.y);
		TransformOps.AddPositionZ(gameObject.transform, localOffset.z * parent.transform.localScale.z);
		component.ResetTokenGroup();
		component.Init();
		return component;
	}

	public static TokenGroup Spawn(TokenGroup tokenGroup, Transform parent, float laneOffset, int lane, float offsetY, float offsetZ)
	{
		Object prefab = tokenGroup.gameObject;
		GameObject gameObject = CacheManager.The().Spawn(prefab);
		TokenGroup component = gameObject.GetComponent<TokenGroup>();
		Vector3 localOffset = component.m_LocalOffset;
		gameObject.transform.position = parent.transform.position;
		gameObject.transform.rotation = parent.transform.rotation;
		TransformOps.AddPositionY(gameObject.transform, offsetY);
		TransformOps.AddPositionZ(gameObject.transform, offsetZ);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.parent = parent.transform;
		TransformOps.SetLocalPositionX(gameObject.transform, Obstacle.CalcLanePosition(lane, laneOffset));
		TransformOps.AddPositionY(gameObject.transform, localOffset.y * parent.transform.localScale.y);
		TransformOps.AddPositionZ(gameObject.transform, localOffset.z * parent.transform.localScale.z);
		component.ResetTokenGroup();
		component.Init();
		return component;
	}

	public static TokenGroup SpawnIgnoreParentRotation(TokenGroup tokenGroup, Transform parent, float laneOffset, int lane, float yPos)
	{
		Object prefab = tokenGroup.gameObject;
		GameObject gameObject = CacheManager.The().Spawn(prefab);
		TokenGroup component = gameObject.GetComponent<TokenGroup>();
		Vector3 localOffset = component.m_LocalOffset;
		gameObject.transform.position = parent.transform.position;
		gameObject.transform.rotation = Quaternion.identity;
		TransformOps.SetPositionY(gameObject.transform, yPos);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.parent = parent.transform;
		TransformOps.SetLocalPositionX(gameObject.transform, Obstacle.CalcLanePosition(lane, laneOffset));
		TransformOps.AddLocalPositionY(gameObject.transform, localOffset.y);
		TransformOps.AddLocalPositionZ(gameObject.transform, localOffset.z);
		component.ResetTokenGroup();
		component.Init();
		gameObject.transform.localRotation = Quaternion.identity;
		return component;
	}
}

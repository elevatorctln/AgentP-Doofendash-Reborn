using System;
using UnityEngine;

public class ObstacleSelectorTable : MonoBehaviour
{
	[Serializable]
	public class LinkTo
	{
		public Array2DFloat m_Weight;

		public Array2DFloat m_WeightCumulative;

		public float m_TotalWeight;

		public LinkTo()
		{
		}

		public LinkTo(int rows, int columns)
		{
			m_Weight = ScriptableObject.CreateInstance<Array2DFloat>();
			m_Weight.Create(rows, columns);
			m_WeightCumulative = ScriptableObject.CreateInstance<Array2DFloat>();
			m_WeightCumulative.Create(rows, columns);
		}

		public void Clone(LinkTo linkTo)
		{
			m_Weight = ScriptableObject.CreateInstance<Array2DFloat>();
			m_WeightCumulative = ScriptableObject.CreateInstance<Array2DFloat>();
			m_TotalWeight = linkTo.m_TotalWeight;
			m_Weight.Clone(linkTo.m_Weight);
			m_WeightCumulative.Clone(linkTo.m_WeightCumulative);
		}
	}

	[Serializable]
	public class Array2DLinkTo
	{
		public LinkTo[] m_Array;

		public int m_RowCount;

		public int m_ColumnCount;

		public int m_Length;

		public int Length
		{
			get
			{
				return m_Length;
			}
		}

		public Array2DLinkTo()
		{
		}

		public Array2DLinkTo(int rows, int columns)
		{
			m_RowCount = rows;
			m_ColumnCount = columns;
			m_Length = m_RowCount * m_ColumnCount;
			m_Array = new LinkTo[m_Length];
		}

		public void Clone(Array2DLinkTo array2DLinkTo)
		{
			m_RowCount = array2DLinkTo.m_RowCount;
			m_ColumnCount = array2DLinkTo.m_ColumnCount;
			m_Length = array2DLinkTo.m_Length;
			m_Array = new LinkTo[m_Length];
			for (int i = 0; i < m_Length; i++)
			{
				m_Array[i] = new LinkTo();
				m_Array[i].Clone(array2DLinkTo.m_Array[i]);
			}
		}

		public LinkTo Get(int r, int c)
		{
			return m_Array[r * m_ColumnCount + c];
		}

		public void Set(int r, int c, LinkTo val)
		{
			m_Array[r * m_ColumnCount + c] = val;
		}
	}

	[HideInInspector]
	public Array2DLinkTo m_LinkToArray;

	public void Clone(ObstacleSelectorTable ost)
	{
		m_LinkToArray = new Array2DLinkTo();
		m_LinkToArray.Clone(ost.m_LinkToArray);
	}

	public void Create()
	{
		int num = Enum.GetNames(typeof(OSTLaneType)).Length;
		int num2 = Enum.GetNames(typeof(ObstacleSelector.ObstacleType)).Length;
		if (m_LinkToArray != null && m_LinkToArray.Length != 0 && m_LinkToArray.Length == num * num2)
		{
			return;
		}
		m_LinkToArray = new Array2DLinkTo(num, num2);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				m_LinkToArray.Set(i, j, new LinkTo(num, num2));
			}
		}
	}

	public float GetWeight(OSTLaneType lane, ObstacleSelector.ObstacleType type, OSTLaneType linkLane, ObstacleSelector.ObstacleType linkType)
	{
		if (m_LinkToArray == null)
		{
			Debug.LogWarning("GetProbability() called when m_LinkToArray == null");
			return 0f;
		}
		return m_LinkToArray.Get((int)lane, (int)type).m_Weight.Get((int)linkLane, (int)linkType);
	}

	public void SetWeight(OSTLaneType lane, ObstacleSelector.ObstacleType type, OSTLaneType linkLane, ObstacleSelector.ObstacleType linkType, float probability)
	{
		if (m_LinkToArray != null)
		{
			m_LinkToArray.Get((int)lane, (int)type).m_Weight.Set((int)linkLane, (int)linkType, probability);
		}
		else
		{
			Debug.LogWarning("SetProbability() called when m_LinkToArray == null");
		}
	}

	public void CalcNextObstacleType(OSTLaneType lane, ObstacleSelector.ObstacleType type, out OSTLaneType nextLane, out ObstacleSelector.ObstacleType nextObstacleType)
	{
		Array2DFloat weight = m_LinkToArray.Get((int)lane, (int)type).m_Weight;
		Array2DFloat weightCumulative = m_LinkToArray.Get((int)lane, (int)type).m_WeightCumulative;
		float totalWeight = m_LinkToArray.Get((int)lane, (int)type).m_TotalWeight;
		int num = Enum.GetNames(typeof(OSTLaneType)).Length;
		int num2 = Enum.GetNames(typeof(ObstacleSelector.ObstacleType)).Length;
		float num3 = UnityEngine.Random.Range(0f, totalWeight);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				float num4 = weightCumulative.Get(i, j);
				float num5 = weight.Get(i, j);
				if (num3 >= num4 && num3 < num4 + num5)
				{
					nextLane = (OSTLaneType)i;
					nextObstacleType = (ObstacleSelector.ObstacleType)j;
					return;
				}
			}
		}
		nextLane = OSTLaneType.NoObstacle;
		nextObstacleType = ObstacleSelector.ObstacleType.NonAvoid;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}

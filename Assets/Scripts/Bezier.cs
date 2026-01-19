using UnityEngine;

public class Bezier
{
	private Vector3[] m_ctrlPoints;

	private Vector3[] m_akDer1CtrlPoints;

	private int m_ctrlPointCount;

	private int m_iDegree;

	private float m_TotalLength;

	private float m_tMin;

	private float m_tMax = 1f;

	public static int m_ControlPointCountMax = 10;

	public int ControlPointCount
	{
		get
		{
			return m_ctrlPoints.Length;
		}
	}

	public float TotalLength
	{
		get
		{
			return m_TotalLength;
		}
	}

	public Bezier(int ctrlPtCnt, Vector3[] ctrlPoints)
	{
		m_ctrlPoints = ctrlPoints;
		m_ctrlPointCount = ctrlPtCnt;
		m_iDegree = m_ctrlPointCount - 1;
		ChooseTable.CreateChooseTable(m_ControlPointCountMax, m_ControlPointCountMax - 1);
		m_akDer1CtrlPoints = new Vector3[m_ctrlPointCount - 1];
		ComputeFirstOrderDifferences();
		m_TotalLength = GetTotalLength();
		if (m_ctrlPoints.Length < 1)
		{
			Debug.LogWarning("Bezier() m_ctrlPoints.Length < 1");
		}
	}

	private void ComputeFirstOrderDifferences()
	{
		for (int i = 0; i < m_akDer1CtrlPoints.Length && i + 1 < m_ctrlPoints.Length; i++)
		{
			m_akDer1CtrlPoints[i] = m_ctrlPoints[i + 1] - m_ctrlPoints[i];
		}
	}

	public void Update(Vector3[] ctrlPoint)
	{
		m_ctrlPoints = ctrlPoint;
		ComputeFirstOrderDifferences();
	}

	private float GetBernstein(float t, int i)
	{
		float num = Mathf.Pow(t, i);
		num *= Mathf.Pow(1f - t, (float)m_iDegree - (float)i);
		return num * ChooseTable.m_aafChoose[m_iDegree, i];
	}

	public Vector3 GetPosition(float fTime)
	{
		if (m_ctrlPoints.Length < 1)
		{
			return Vector3.zero;
		}
		float num = 1f - fTime;
		float num2 = fTime;
		Vector3 vector = num * m_ctrlPoints[0];
		for (int i = 1; i < m_iDegree; i++)
		{
			float num3 = ChooseTable.m_aafChoose[m_iDegree, i] * num2;
			vector = (vector + num3 * m_ctrlPoints[i]) * num;
			num2 *= fTime;
		}
		return vector + num2 * m_ctrlPoints[m_iDegree];
	}

	public Vector3 GetPositionFromLength(float s)
	{
		return GetPosition(GetTime(s));
	}

	public void GetPositionAndFirstDerivativeFromLength(float s, out float t, out Vector3 position, out Vector3 tangent)
	{
		t = GetTime(s);
		position = GetPosition(t);
		tangent = GetFirstDerivative(t);
	}

	public Vector3 GetFirstDerivativeFromLength(float s)
	{
		return GetFirstDerivative(GetTime(s));
	}

	public Vector3 GetFirstDerivative(float fTime)
	{
		float num = 1f - fTime;
		float num2 = fTime;
		Vector3 vector = num * m_akDer1CtrlPoints[0];
		int num3 = m_iDegree - 1;
		for (int i = 1; i < num3; i++)
		{
			float num4 = ChooseTable.m_aafChoose[num3, i] * num2;
			vector = (vector + num4 * m_akDer1CtrlPoints[i]) * num;
			num2 *= fTime;
		}
		vector += num2 * m_akDer1CtrlPoints[num3];
		float num5 = m_ctrlPointCount;
		return vector * num5;
	}

	private float GetSpeed(float fTime)
	{
		return GetFirstDerivative(fTime).magnitude;
	}

	private float GetLength(float t)
	{
		return Integration.RombergIntegral(0f, t, GetSpeed);
	}

	private float GetTotalLength()
	{
		return Integration.RombergIntegral(0f, 1f, GetSpeed);
	}

	private float GetTime(float s)
	{
		float num = s / m_TotalLength;
		float num2 = (1f - num) * m_tMin + num * m_tMax;
		for (int i = 0; i < 12; i++)
		{
			float num3 = GetLength(num2) - s;
			if (Mathf.Abs(num3) < 0.1f)
			{
				return num2;
			}
			num2 -= num3 / GetSpeed(num2);
		}
		return num2;
	}
}

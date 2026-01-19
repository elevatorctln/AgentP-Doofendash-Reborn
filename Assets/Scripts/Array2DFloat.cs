using UnityEngine;

public class Array2DFloat : ScriptableObject
{
	public float[] m_Array;

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

	public void Clone(Array2DFloat arrayToCopy)
	{
		m_Array = (float[])arrayToCopy.m_Array.Clone();
		m_RowCount = arrayToCopy.m_RowCount;
		m_ColumnCount = arrayToCopy.m_ColumnCount;
		m_Length = arrayToCopy.m_Length;
	}

	public void Create(int rows, int columns)
	{
		m_RowCount = rows;
		m_ColumnCount = columns;
		m_Length = m_RowCount * m_ColumnCount;
		m_Array = new float[m_Length];
	}

	public float Get(int r, int c)
	{
		return m_Array[r * m_ColumnCount + c];
	}

	public void Set(int r, int c, float val)
	{
		m_Array[r * m_ColumnCount + c] = val;
	}
}

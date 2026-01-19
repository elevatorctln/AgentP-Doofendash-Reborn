using UnityEngine;

internal class PlatformPath
{
	public Vector3 m_NextPosition;

	public Quaternion m_Rotation = Quaternion.identity;

	public void Identity()
	{
		m_NextPosition = Vector3.zero;
		m_Rotation = Quaternion.identity;
	}

	public void Clone(PlatformPath pp)
	{
		m_NextPosition = pp.m_NextPosition;
		m_Rotation = pp.m_Rotation;
	}
}

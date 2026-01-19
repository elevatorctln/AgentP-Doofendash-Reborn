using UnityEngine;

public class CacheObject
{
	public bool m_IsInUse;

	public GameObject m_Object;

	public CacheObject(bool isInUse, GameObject obj)
	{
		m_IsInUse = isInUse;
		m_Object = obj;
	}
}

using UnityEngine;

public class PlatformAddon : MonoBehaviour
{
	public Vector3 m_SpawnLocalPosition;

	public Vector3 m_SpawnLocalScale;

	public Vector3 SpawnLocalPosition
	{
		get
		{
			return m_SpawnLocalPosition;
		}
		set
		{
			m_SpawnLocalPosition = value;
		}
	}

	public Vector3 SpawnLocalScale
	{
		get
		{
			return m_SpawnLocalScale;
		}
		set
		{
			m_SpawnLocalScale = value;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}

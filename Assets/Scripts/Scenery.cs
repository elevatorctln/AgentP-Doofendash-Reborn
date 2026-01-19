using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;


[ExecuteInEditMode]
public class Scenery : PlatformAddon
{
	public enum Type
	{
		Building = 0,
		LongBuilding = 1,
		TelephonePole = 2
	}

	public Object[] m_ChooseOnePrefabList;

	[HideInInspector]
	public Vector3 m_SpawnExtents;

	public float m_SpawnProbability = 0.5f;

	public Type m_Type;

	public int m_IgnoreMeCount;

	private static Hashtable m_IgnoreMeList = new Hashtable();

	public Vector3 SpawnExtents
	{
		get
		{
			return m_SpawnExtents;
		}
		set
		{
			m_SpawnExtents = value;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private static Vector3 RandomPosition(Scenery s)
	{
		Vector3 vector = new Vector3(s.SpawnExtents.x * s.SpawnLocalScale.x, s.SpawnExtents.y * s.SpawnLocalScale.y, s.SpawnExtents.z * s.SpawnLocalScale.z);
		Vector3 zero = Vector3.zero;
		zero.x = vector.x * Random.Range(-1f, 1f);
		zero.y = vector.y * Random.Range(-1f, 1f);
		zero.z = vector.z * Random.Range(-1f, 1f);
		return s.SpawnLocalPosition + zero;
	}

	private static Object RandomPrefab(Scenery s)
	{
		int num = Random.Range(0, s.m_ChooseOnePrefabList.Length);
		return s.m_ChooseOnePrefabList[num];
	}

	public static GameObject SpawnScenery(Scenery scenery, Transform parent)
	{
		if (scenery.m_IgnoreMeCount > 0)
		{
			if (m_IgnoreMeList.Contains(scenery.m_Type))
			{
				int num = (int)m_IgnoreMeList[scenery.m_Type];
				num--;
				if (num > 0)
				{
					m_IgnoreMeList[scenery.m_Type] = num;
				}
				else
				{
					m_IgnoreMeList.Remove(scenery.m_Type);
				}
				Debug.Log("Scenery list is set to " + scenery);
				return null;
			}
			m_IgnoreMeList.Add(scenery.m_Type, scenery.m_IgnoreMeCount);
		}
		Object prefab = RandomPrefab(scenery);
		Vector3 localPosition = RandomPosition(scenery);
		GameObject gameObject = CacheManager.The().Spawn(prefab);
		gameObject.transform.position = parent.transform.position;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.rotation = Quaternion.Euler(0f, parent.transform.rotation.eulerAngles.y, parent.transform.rotation.eulerAngles.z);
		gameObject.transform.parent = parent.transform;
		gameObject.transform.localPosition = localPosition;
		TransformOps.SetEulers(gameObject.transform, 0f, gameObject.transform.rotation.eulerAngles.y, 0f);
		gameObject.AddComponent<Scenery>();
		return gameObject;
	}
}

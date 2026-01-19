using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacheManager : MonoBehaviour
{
	[Serializable]
	public class CacheGroup
	{
		[Serializable]
		public class CacheTuple
		{
			public GameObject m_CachedObject;

			public int m_CacheCount = 1;
		}

		public List<CacheTuple> m_CacheTuples = new List<CacheTuple>();

		public string m_CacheGroupName;
	}

	[HideInInspector]
	public List<CacheGroup> m_CacheGroups = new List<CacheGroup>();

	private Hashtable m_CachedObjects = new Hashtable();

	private static CacheManager m_The;

	public static CacheManager The()
	{
		if (m_The == null)
		{
			m_The = GameObject.Find("CacheManager").GetComponent<CacheManager>();
		}
		return m_The;
	}

	private void Awake()
	{
		m_The = this;
	}

	private void Start()
	{
		LoadAllCachedObjects();
	}

	private void LoadAllCachedObjects()
	{
		foreach (CacheGroup cacheGroup in m_CacheGroups)
		{
			LoadCacheGroup(cacheGroup);
		}
		Debug.Log("Cachemanager loaded cached objects: " + m_CachedObjects);
	}

	private void LoadCacheGroup(string cacheGroupName)
	{
		foreach (CacheGroup cacheGroup in m_CacheGroups)
		{
			if (cacheGroup.m_CacheGroupName == cacheGroupName)
			{
				LoadCacheGroup(cacheGroup);
				break;
			}
		}
	}

	private void LoadCacheGroup(CacheGroup cg)
	{
		foreach (CacheGroup.CacheTuple cacheTuple in cg.m_CacheTuples)
		{
			int cacheCount = cacheTuple.m_CacheCount;
			while (cacheCount-- > 0)
			{
				if (cacheTuple.m_CachedObject != null)
				{
					LoadCachedObject(cacheTuple.m_CachedObject, false);
				}
			}
		}
	}

	private void UnloadAllCachedObjects()
	{
		foreach (CacheGroup cacheGroup in m_CacheGroups)
		{
			UnloadCacheGroup(cacheGroup);
		}
	}

	private void UnloadCacheGroup(string cacheGroupName)
	{
		foreach (CacheGroup cacheGroup in m_CacheGroups)
		{
			if (cacheGroup.m_CacheGroupName == cacheGroupName)
			{
				UnloadCacheGroup(cacheGroup);
				break;
			}
		}
	}

	private void UnloadCacheGroup(CacheGroup cg)
	{
		foreach (CacheGroup.CacheTuple cacheTuple in cg.m_CacheTuples)
		{
			UnloadCachedObject(cacheTuple.m_CachedObject);
		}
	}

	private GameObject LoadCachedObject(string name, bool inUse)
	{
		UnityEngine.Object obj = ResourcesMonitor.Load(name);
		if (obj == null)
		{
			return null;
		}
		return LoadCachedObject(obj, inUse);
	}

	private GameObject LoadCachedObject(UnityEngine.Object prefab, bool inUse)
	{
		string key = prefab.name;
		Hashtable hashtable;
		if (!m_CachedObjects.ContainsKey(key))
		{
			hashtable = new Hashtable();
			m_CachedObjects.Add(key, hashtable);
		}
		else
		{
			hashtable = (Hashtable)m_CachedObjects[key];
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(prefab) as GameObject;
		gameObject.SetActive(false);
		gameObject.name = prefab.name;
		hashtable.Add(gameObject.GetInstanceID(), new CacheObject(inUse, gameObject));
		gameObject.transform.parent = base.transform;
		Animation component = gameObject.GetComponent<Animation>();
		if (component != null)
		{
			AssetGPULoader.The().PreLoadObject(gameObject);
		}
		return gameObject;
	}

	private void UnloadCachedObject(GameObject inGameObject)
	{
		string key = inGameObject.name.Replace("(Clone)", string.Empty);
		if (m_CachedObjects.Contains(key))
		{
			Hashtable hashtable = (Hashtable)m_CachedObjects[key];
			hashtable.Remove(inGameObject.GetInstanceID());
			UnityEngine.Object.Destroy(inGameObject);
		}
	}

	public T Spawn<T>(string keyName) where T : MonoBehaviour
	{
		GameObject gameObject = Spawn(keyName);
		if (gameObject == null)
		{
			return (T)null;
		}
		return gameObject.GetComponent<T>();
	}

	public T Spawn<T>(T prefabObject) where T : MonoBehaviour
	{
		if (prefabObject == null)
    {
        Debug.LogError("CacheManager.Spawn: prefabObject is null! that's not supposed to happen");
        return null;
    }
    
		UnityEngine.Object obj = prefabObject.gameObject;
		string keyName = obj.name;
		T val = Spawn<T>(keyName);
		if (val == null)
		{
			GameObject gameObject = LoadCachedObject(obj, true);
			gameObject.SetActive(true);
			val = gameObject.GetComponent<T>();
		}
		return val;
	}

	public GameObject Spawn(UnityEngine.Object prefab)
	{
		string keyName = prefab.name;
		GameObject gameObject = Spawn(keyName);
		if (gameObject == null)
		{
			gameObject = LoadCachedObject(prefab, true);
			gameObject.SetActive(true);
		}
		return gameObject;
	}

	public GameObject Spawn(string keyName)
	{
		GameObject gameObject = null;
		if (m_CachedObjects.ContainsKey(keyName))
		{
			Hashtable hashtable = (Hashtable)m_CachedObjects[keyName];
			foreach (CacheObject value in hashtable.Values)
			{
				if (!value.m_IsInUse)
				{
					value.m_IsInUse = true;
					value.m_Object.SetActive(true);
					value.m_Object.transform.position = Vector3.zero;
					value.m_Object.transform.rotation = Quaternion.identity;
					return value.m_Object;
				}
			}
		}
		gameObject = LoadCachedObject(keyName, true);
		if (gameObject == null)
		{
			return null;
		}
		gameObject.SetActive(true);
		return gameObject;
	}

	public void Unspawn<T>(T t) where T : MonoBehaviour
	{
		Unspawn(t.gameObject);
	}

	public void Unspawn(GameObject go)
	{
		string key = go.name.Replace("(Clone)", string.Empty);
		if (m_CachedObjects.Contains(key))
		{
			Hashtable hashtable = (Hashtable)m_CachedObjects[key];
			CacheObject cacheObject = (CacheObject)hashtable[go.GetInstanceID()];
			cacheObject.m_IsInUse = false;
			cacheObject.m_Object.transform.parent = base.transform;
			cacheObject.m_Object.SetActive(false);
		}
	}
}

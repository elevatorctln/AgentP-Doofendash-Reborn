using System;
using UnityEngine;

public class ResourcesMonitor
{
	public static UnityEngine.Object Load(string path, Type systemTypeInstance)
	{
		return Resources.Load(path, systemTypeInstance);
	}

	public static UnityEngine.Object Load(string path)
	{
		return Resources.Load(path);
	}
}

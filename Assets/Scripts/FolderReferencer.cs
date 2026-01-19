using System.Collections.Generic;
using UnityEngine;

public class FolderReferencer : ScriptableObject
{
	public List<Object> dataList = new List<Object>();

	public virtual bool ObjectShouldBeAdded(Object o)
	{
		return true;
	}
}

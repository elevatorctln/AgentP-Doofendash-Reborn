using UnityEngine;

public class AGSLatestStringList : AGSSyncableStringList
{
	public AGSLatestStringList(AndroidJavaObject javaObject)
		: base(javaObject)
	{
	}
}

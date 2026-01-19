using UnityEngine;

public class SoundReferencer : FolderReferencer
{
	public override bool ObjectShouldBeAdded(Object o)
	{
		if (o.GetType() == typeof(AudioClip))
		{
			return true;
		}
		return false;
	}
}

using UnityEngine;

public class SimpleCharacterShadow : MonoBehaviour
{
	public void SetPosition(Vector3 position, Vector3 forward, Vector3 up)
	{
		base.transform.position = position;
		base.transform.rotation = Quaternion.LookRotation(forward, up);
	}
}

using UnityEngine;

public class DisableOnLowEnd : MonoBehaviour
{
	public void Start()
	{
		base.gameObject.SetActive(false);
	}
}

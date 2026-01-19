using UnityEngine;

public class DummyObject : MonoBehaviour
{
	private void Start()
	{
		StoreGUIManagerPersistentElements.The.UpdateMoney();
	}

	private void Update()
	{
	}
}

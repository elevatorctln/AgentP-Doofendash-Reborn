using UnityEngine;

public class MaterialSwitch : MonoBehaviour
{
	public Material[] materials;

	public void SwitchMaterial(int id)
	{
		base.GetComponent<Renderer>().material = materials[id];
	}
}

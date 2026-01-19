using UnityEngine;

public class LowEndMaterial : MonoBehaviour
{
	public Material[] m_Materials;

	private bool m_DontAssignOnStart;

	private void Start()
	{
		if (GameManager.The.IsLowEndDevice() && !m_DontAssignOnStart)
		{
			AssignLowEndMaterials();
			m_DontAssignOnStart = true;
		}
	}

	public void AssignLowEndMaterials()
	{
		SkinnedMeshRenderer component = GetComponent<SkinnedMeshRenderer>();
		if (component != null)
		{
			m_DontAssignOnStart = true;
			component.materials = m_Materials;
			return;
		}
		MeshRenderer component2 = GetComponent<MeshRenderer>();
		if (component2 != null)
		{
			component2.materials = m_Materials;
		}
		m_DontAssignOnStart = true;
	}

	public void SetDontAssignOnStart(bool dontAssignOnStart)
	{
		m_DontAssignOnStart = dontAssignOnStart;
	}

	public bool GetDontAssignOnStart()
	{
		return m_DontAssignOnStart;
	}
}

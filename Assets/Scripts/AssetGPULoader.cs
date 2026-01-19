using UnityEngine;

public class AssetGPULoader : MonoBehaviour
{
	public Camera activeCamera;

	private RenderTexture _rt;

	private static AssetGPULoader m_The;

	public static AssetGPULoader The()
	{
		if (m_The == null)
		{
			m_The = GameObject.Find("AssetGPULoader").GetComponent<AssetGPULoader>();
		}
		return m_The;
	}

	private void Awake()
	{
		m_The = this;
		_rt = new RenderTexture(32, 32, 24);
		_rt.Create();
		activeCamera.targetTexture = _rt;
	}

	private void Start()
	{
	}

	public void PreLoadObject(GameObject obj)
	{
		SnapshotObject(obj);
	}

	private void SnapshotObject(GameObject obj)
	{
		Vector3 position = obj.transform.position;
		position += new Vector3(0f, 0.5f, 0f);
		activeCamera.transform.position = position;
		activeCamera.transform.LookAt(obj.transform.position);
		activeCamera.Render();
	}
}

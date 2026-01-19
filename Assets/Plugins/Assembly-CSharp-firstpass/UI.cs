using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class UI : MonoBehaviour
{
	public static UI instance;

	public static UIToolkit firstToolkit;

	public int drawDepth = 100;

	public LayerMask UILayer = 0;

	[HideInInspector]
	public int layer;

	private AudioSource _audioSource;

	private Camera _uiCamera;

	private GameObject _uiCameraHolder;

	private UIToolkit[] _toolkitInstances;

	public bool autoTextureSelectionForHD = true;

	public bool allowPod4GenHD = true;

	public int maxWidthOrHeightForSD = 1080;

	public int maxWidthOrHeightForHD = 1440;

	public int maxWidthOrHeightForUHD = 3840;

	public static bool isHD;

	public static int scaleFactor = 1;

	public string hdExtension = "2x";

	private void Awake()
	{
		Debug.Log("=== UI.Awake START ===");
		instance = this;
		_audioSource = GetComponent<AudioSource>();
		if (_audioSource == null)
		{
			_audioSource = base.gameObject.AddComponent<AudioSource>();
		}
		_uiCameraHolder = new GameObject();
		_uiCameraHolder.transform.parent = base.gameObject.transform;
		_uiCameraHolder.AddComponent<Camera>();
		_uiCamera = _uiCameraHolder.GetComponent<Camera>();
		_uiCamera.name = "UICamera";
		_uiCamera.clearFlags = CameraClearFlags.Depth;
		_uiCamera.nearClipPlane = 0.3f;
		_uiCamera.farClipPlane = 50f;
		_uiCamera.depth = drawDepth;
		_uiCamera.rect = new Rect(0f, 0f, 1f, 1f);
		_uiCamera.orthographic = true;
		_uiCamera.orthographicSize = Screen.height / 2;
		_uiCamera.transform.position = new Vector3(Screen.width / 2, -Screen.height / 2, -10f);
		_uiCamera.cullingMask = UILayer;
		Debug.Log("About to find UIToolkit instances...");
		for (int i = 0; i < 32; i++)
		{
			if ((UILayer.value & (1 << i)) == 1 << i)
			{
				layer = i;
				break;
			}
		}
		
		if (autoTextureSelectionForHD)
		{
			int num = Mathf.Max(Screen.width, Screen.height);
			if (allowPod4GenHD)
			{
				if (num >= maxWidthOrHeightForUHD)
				{
					isHD = true;
					scaleFactor = 4;
					hdExtension = "4x";
				}
				else if (num >= maxWidthOrHeightForHD)
				{
					isHD = true;
					scaleFactor = 2;
					hdExtension = "2x";
				}
				else if (num >= maxWidthOrHeightForSD)
				{
					isHD = true;
					scaleFactor = 2;
					hdExtension = "2x";
				}
				else
				{
					isHD = false;
					scaleFactor = 2;
					hdExtension = "2x";
				}
			}
			else if (num >= maxWidthOrHeightForHD)
			{
				isHD = true;
				scaleFactor = 2;
				hdExtension = "2x";
			}
			else if (num >= maxWidthOrHeightForSD)
			{
				isHD = true;
				scaleFactor = 2;
				hdExtension = "2x";
			}
			else
			{
				isHD = false;
				scaleFactor = 2;
				hdExtension = "2x";
			}
		}
		
		Debug.Log($"[UI] Screen: {Screen.width}x{Screen.height}, DPI: {Screen.dpi}");
		Debug.Log($"[UI] scaleFactor: {scaleFactor}, hdExtension: {hdExtension}");
		_toolkitInstances = GetComponentsInChildren<UIToolkit>();
		Debug.Log("Found " + _toolkitInstances.Length + " UIToolkit instances");
		if (_toolkitInstances.Length == 0) {
        Debug.LogError("NO UITOOLKIT INSTANCES FOUND! UI will not work!");
        return;
    }
		firstToolkit = _toolkitInstances[0];
		Debug.Log("firstToolkit set: " + firstToolkit.name);
		UIToolkit[] toolkitInstances = _toolkitInstances;
		foreach (UIToolkit uIToolkit in toolkitInstances)
		{
			Debug.Log("=== UI.Awake END ===");
			uIToolkit.loadTextureAndPrepareForUse();
		}
	}

	protected void OnApplicationQuit()
	{
		instance = null;
		firstToolkit = null;
	}

	protected void OnDestroy()
	{
		instance = null;
		firstToolkit = null;
	}

	public void playSound(AudioClip clip)
	{
		Debug.Log("now about to play _audioSource.PlayOneShot(clip)");
		_audioSource.PlayOneShot(clip);
	}
}

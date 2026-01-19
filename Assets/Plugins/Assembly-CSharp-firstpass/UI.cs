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

	// Legacy integer scale factor (kept for compatibility with existing code)
	public static int scaleFactor = 1;
	
	// New continuous scale factor for modern resolution scaling
	public static float scaleFactorFloat = 1f;
	
	// Reference resolution (iPad Retina - the original target device)
	public static int referenceWidth = 1536;
	public static int referenceHeight = 2048;
	
	// Aspect ratio helpers
	public static float aspectRatio => (float)Screen.width / Screen.height;
	public static bool isWideAspect => aspectRatio < 0.65f;  // Taller than 3:2 (most modern phones)
	public static bool isUltraWide => aspectRatio < 0.5f;    // 19.5:9 or taller (notched phones)
	public static bool isTabletAspect => aspectRatio >= 0.7f; // 4:3 or wider (tablets)
	
	// Safe area for notched devices
	public static Rect safeArea => Screen.safeArea;
	public static float safeAreaTopOffset => Screen.height - (safeArea.y + safeArea.height);
	public static float safeAreaBottomOffset => safeArea.y;
	public static float safeAreaLeftOffset => safeArea.x;
	public static float safeAreaRightOffset => Screen.width - (safeArea.x + safeArea.width);

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
		
		// Calculate adaptive scaling based on reference resolution
		CalculateAdaptiveScaling();
		
		Debug.Log($"[UI] Screen: {Screen.width}x{Screen.height}, DPI: {Screen.dpi}");
		Debug.Log($"[UI] scaleFactor: {scaleFactor}, scaleFactorFloat: {scaleFactorFloat:F3}");
		Debug.Log($"[UI] aspectRatio: {aspectRatio:F3}, isWideAspect: {isWideAspect}, isUltraWide: {isUltraWide}");
		Debug.Log($"[UI] safeArea: {safeArea}, topOffset: {safeAreaTopOffset}, bottomOffset: {safeAreaBottomOffset}");
		Debug.Log($"[UI] hdExtension: {hdExtension}");
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

	/// <summary>
	/// Calculate adaptive scaling based on screen resolution and DPI.
	/// This replaces the old fixed threshold-based scaling with a more flexible approach.
	/// </summary>
	private void CalculateAdaptiveScaling()
	{
		// Calculate the continuous scale factor based on screen height relative to reference
		// We use height because this is a portrait-oriented mobile game
		float heightRatio = (float)Screen.height / referenceHeight;
		float widthRatio = (float)Screen.width / referenceWidth;
		
		// Use the smaller ratio to ensure UI fits on screen (important for ultra-wide phones)
		scaleFactorFloat = Mathf.Min(heightRatio, widthRatio);
		
		// For the legacy integer scaleFactor, we need to pick the appropriate texture set
		// This determines which texture atlas to load (1x, 2x, or 4x)
		if (autoTextureSelectionForHD)
		{
			// Use DPI-aware scaling when available, fall back to resolution-based
			float effectiveDPI = Screen.dpi > 0 ? Screen.dpi : 160f; // Default to 160 DPI
			
			// Calculate which texture set to use based on screen density
			// Modern phones typically have 400-600 DPI, tablets 200-300 DPI
			if (effectiveDPI >= 400 || Screen.height >= maxWidthOrHeightForUHD || Screen.width >= maxWidthOrHeightForUHD)
			{
				// Ultra HD - use 4x textures
				isHD = true;
				scaleFactor = 4;
				hdExtension = "4x";
			}
			else if (effectiveDPI >= 240 || Screen.height >= maxWidthOrHeightForHD || Screen.width >= maxWidthOrHeightForHD)
			{
				// HD - use 2x textures  
				// Most modern phones fall into this category
				isHD = true;
				scaleFactor = 2;
				hdExtension = "2x";
			}
			else if (effectiveDPI >= 160 || Screen.height >= maxWidthOrHeightForSD || Screen.width >= maxWidthOrHeightForSD)
			{
				// SD-HD boundary - use 2x textures for better quality
				isHD = true;
				scaleFactor = 2;
				hdExtension = "2x";
			}
			else
			{
				// Low resolution - use 1x textures
				isHD = false;
				scaleFactor = 1;
				hdExtension = "";
			}
		}
		
		// Clamp scaleFactorFloat to reasonable bounds
		scaleFactorFloat = Mathf.Clamp(scaleFactorFloat, 0.5f, 4f);
	}
	
	/// <summary>
	/// Get the scale factor to apply to UI elements for proper sizing on current screen.
	/// Use this instead of the raw scaleFactor for positioning calculations.
	/// </summary>
	public static float GetAdaptiveScale()
	{
		return scaleFactorFloat;
	}
	
	/// <summary>
	/// Convert a position from reference resolution to current screen resolution.
	/// </summary>
	public static Vector2 ScalePosition(Vector2 referencePos)
	{
		return new Vector2(
			referencePos.x * scaleFactorFloat,
			referencePos.y * scaleFactorFloat
		);
	}
	
	/// <summary>
	/// Get the horizontal offset needed to center content on ultra-wide screens.
	/// </summary>
	public static float GetHorizontalCenterOffset()
	{
		if (!isWideAspect) return 0f;
		
		// Calculate how much wider the screen is compared to reference aspect ratio
		float referenceAspect = (float)referenceWidth / referenceHeight;
		float currentAspect = aspectRatio;
		
		// If current is wider (smaller ratio in portrait), we need to offset
		if (currentAspect < referenceAspect)
		{
			float expectedWidth = Screen.height * referenceAspect;
			return (Screen.width - expectedWidth) / 2f;
		}
		return 0f;
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

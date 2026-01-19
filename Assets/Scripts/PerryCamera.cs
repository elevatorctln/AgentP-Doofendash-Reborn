using UnityEngine;

public class PerryCamera : MonoBehaviour
{
	public enum CameraType
	{
		BackCam = 0,
		FrontCam = 1,
		RightCam = 2,
		LeftCam = 3,
		BossBackCam = 4,
		BossFrontCam = 5,
		BossBackCinematicCam = 6,
		BackCamHangGlide = 7,
		BackCamEagle = 8
	}

	public GameObject m_runnerTarget;

	public GameObject m_bossTarget;

	private float m_TargetYOffsetDesired = 18f;

	private float m_TargetYOffsetStart = 18f;

	private float m_TargetYOffset = 18f;

	public float m_TargetYOffsetDefault = 18f;

	public float m_TargetYOffsetHangGlide = 5f;

	public float m_TargetYOffsetBoss = 14.5f;

	public float m_TargetYOffsetEagle = 30f;

	public float m_distance = 1f;

	public float m_heightDamping = 2f;

	public float m_rotationDamping = 3f;

	public CameraType m_currentCam;

	public Vector3 m_backCameraOffset;

	public Vector3 m_frontCameraOffset;

	public Vector3 m_rightCameraOffset;

	public Vector3 m_leftCameraOffset;

	public Vector3 m_BackCameraOffsetHangGlide;

	public Vector3 m_BackCameraOffsetEagle;

	public Vector3 m_bossBackCameraOffset;

	public Vector3 m_bossFrontCameraOffset;

	public Vector3 m_bossBackCameraCinematicOffset;

	public float m_FOVDefault = 60f;

	public float m_FOVHangGlide = 110f;

	public float m_FOVEagle = 80f;

	public float m_FOVLerpTime = 2f;

	private float m_FOVDesired;

	private float m_FOVStart;

	private float m_FOVChangeTime;

	[HideInInspector]
	public Vector3 m_introCameraOffset;

	public float m_lookDamping = 3.5f;

	public bool m_lookSmoothing = true;

	private bool m_IsInIntro;

	private Transform m_target;

	private Vector3 m_CameraOffsetCur;

	private Vector3 m_CameraOffsetStart;

	private Vector3 m_CameraOffsetDesired;

	private static Camera m_TheCamera;

	private static PerryCamera m_The;

	private float m_ChangeCamTime;

	public static PerryCamera The()
	{
		return m_The;
	}

	public static Camera TheCamera()
	{
		return m_TheCamera;
	}

	public static void ShowGame()
	{
		if (m_TheCamera != null)
		{
			m_TheCamera.enabled = true;
		}
		else
		{
			Debug.LogWarning("Tried to access PerryCamera.m_The == null");
		}
	}

	public static void HideGame()
	{
		if (m_TheCamera != null)
		{
			m_TheCamera.enabled = false;
		}
		else
		{
			Debug.LogWarning("Tried to access PerryCamera.m_The == null");
		}
	}

	private void OnDestroy()
	{
		GameEventManager.GameIntro -= GameIntro;
		GameEventManager.GameIntroEndEvents -= GameIntroEndListener;
		GameEventManager.GameStart -= GameStart;
		GameEventManager.GameOver -= GameOver;
		GameEventManager.BossStartEvents -= BossStartListener;
		GameEventManager.BossEndEvents -= BossEndListener;
		GameEventManager.HangGlideStartEvents -= HangGlideStartListener;
		GameEventManager.LaunchPerryEvents -= LaunchPerryListener;
	}

	private void Awake()
	{
		m_TheCamera = GetComponent<Camera>();
		m_The = this;
		GameEventManager.GameIntro += GameIntro;
		GameEventManager.GameIntroEndEvents += GameIntroEndListener;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.BossStartEvents += BossStartListener;
		GameEventManager.BossEndEvents += BossEndListener;
		GameEventManager.HangGlideStartEvents += HangGlideStartListener;
		GameEventManager.LaunchPerryEvents += LaunchPerryListener;
		Init();
	}

	private void Init()
	{
		m_target = m_runnerTarget.transform;
		ChangeCam(m_currentCam);
		m_CameraOffsetDesired = (m_CameraOffsetStart = (m_CameraOffsetCur = m_backCameraOffset));
	}

	private void Start()
	{
		if (GameManager.The.IsLowEndDevice())
		{
			TheCamera().farClipPlane = 600f;
			RenderSettings.fog = false;
		}
	}

	public void ChangeCam(CameraType cameraType)
	{
		m_FOVChangeTime = 0f;
		m_FOVStart = m_TheCamera.fieldOfView;
		m_FOVDesired = m_FOVDefault;
		m_ChangeCamTime = 0f;
		if (m_IsInIntro)
		{
			m_CameraOffsetStart = base.transform.position;
		}
		else
		{
			m_CameraOffsetStart = m_CameraOffsetDesired;
		}
		m_TargetYOffsetStart = m_TargetYOffsetDesired;
		m_TargetYOffsetDesired = m_TargetYOffsetDefault;
		m_currentCam = cameraType;
		switch (m_currentCam)
		{
		case CameraType.BackCam:
			m_CameraOffsetDesired = m_backCameraOffset;
			break;
		case CameraType.FrontCam:
			m_CameraOffsetDesired = m_frontCameraOffset;
			break;
		case CameraType.RightCam:
			m_CameraOffsetDesired = m_rightCameraOffset;
			break;
		case CameraType.LeftCam:
			m_CameraOffsetDesired = m_leftCameraOffset;
			break;
		case CameraType.BossBackCam:
			m_CameraOffsetDesired = m_bossBackCameraOffset;
			m_TargetYOffsetDesired = m_TargetYOffsetBoss;
			break;
		case CameraType.BossFrontCam:
			m_CameraOffsetDesired = m_bossFrontCameraOffset;
			m_TargetYOffsetDesired = m_TargetYOffsetBoss;
			m_lookSmoothing = true;
			break;
		case CameraType.BossBackCinematicCam:
			m_CameraOffsetDesired = m_bossBackCameraCinematicOffset;
			m_TargetYOffsetDesired = m_TargetYOffsetBoss;
			break;
		case CameraType.BackCamHangGlide:
			m_CameraOffsetDesired = m_BackCameraOffsetHangGlide;
			m_TargetYOffsetDesired = m_TargetYOffsetHangGlide;
			m_FOVDesired = m_FOVHangGlide;
			break;
		case CameraType.BackCamEagle:
			m_CameraOffsetDesired = m_BackCameraOffsetEagle;
			m_TargetYOffsetDesired = m_TargetYOffsetEagle;
			m_FOVDesired = m_FOVEagle;
			break;
		}
	}

	private void Update()
	{
		m_ChangeCamTime += Time.deltaTime;
		if (m_ChangeCamTime > 1f)
		{
			m_ChangeCamTime = 1f;
		}
		if (!Runner.The().IsInTubeIntroState() && !Runner.The().IsTubeIntroStartedState())
		{
			m_CameraOffsetCur = Vector3.Lerp(m_CameraOffsetStart, m_CameraOffsetDesired, m_ChangeCamTime);
			m_TargetYOffset = Mathf.Lerp(m_TargetYOffsetStart, m_TargetYOffsetDesired, m_ChangeCamTime);
			UpdateFOV();
		}
	}

	private void UpdateFOV()
	{
		if (GameManager.The != null && GameManager.The.IsInGamePlay())
		{
			m_FOVChangeTime += Time.deltaTime / m_FOVLerpTime;
			if (m_FOVChangeTime > 1f)
			{
				m_FOVChangeTime = 1f;
			}
			m_TheCamera.fieldOfView = Mathf.Lerp(m_FOVStart, m_FOVDesired, m_FOVChangeTime);
		}
	}

	private void LateUpdate()
	{
		if ((bool)m_target)
		{
			CameraPositionUpdate();
		}
	}

	private void CameraPositionIntroUpdate()
	{
		Vector3 localPosition = Runner.The().introCamPos.localPosition;
		base.transform.position = localPosition;
		base.transform.position = Runner.The().introCamPos.position;
		base.transform.rotation = Runner.The().introCamPos.rotation;
	}

	public void ResetCameraPosition()
	{
		Vector3 vector = Runner.The().CalcHalfwayCamTargetPosition();
		vector.y = Runner.The().CalcBasePositionY();
		m_IsInIntro = false;
		m_lookSmoothing = false;
		Vector3 worldPosition = new Vector3(vector.x, vector.y + m_TargetYOffset, vector.z);
		Vector3 vector2 = m_target.rotation * m_CameraOffsetCur;
		m_CameraOffsetDesired = (m_CameraOffsetStart = m_backCameraOffset);
		base.transform.position = vector + vector2;
		base.transform.LookAt(worldPosition);
	}

	private void ResetAll()
	{
		m_TargetYOffset = (m_TargetYOffsetStart = (m_TargetYOffsetDesired = m_TargetYOffsetDefault));
		m_CameraOffsetCur = (m_CameraOffsetStart = (m_CameraOffsetDesired = m_backCameraOffset));
		m_TheCamera.fieldOfView = (m_FOVStart = (m_FOVDesired = m_FOVDefault));
	}

	private void CameraPositionUpdate()
	{
		if (Runner.The() == null)
		{
			return;
		}
		if (Runner.The().IsTubeIntroStartedState())
		{
			base.transform.position = Runner.The().HackedIntroCamPosition;
			base.transform.rotation = Runner.The().HackedIntroCamRotation;
			return;
		}
		if (Runner.The().IsInTubeIntroState())
		{
			CameraPositionIntroUpdate();
			return;
		}
		Vector3 vector = Runner.The().CalcHalfwayCamTargetPosition();
		vector.y = Runner.The().CalcBasePositionY();
		Vector3 vector2 = new Vector3(vector.x, vector.y + m_TargetYOffset, vector.z);
		Vector3 vector3 = m_target.rotation * m_CameraOffsetCur;
		base.transform.position = vector + vector3;
		if (m_lookSmoothing)
		{
			Quaternion a = Quaternion.LookRotation(vector2 - base.transform.position);
			Quaternion b = base.transform.rotation;
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, a, Time.deltaTime * m_lookDamping);
			if (m_IsInIntro && QuatOps.AboutEqual(ref a, ref b))
			{
				m_IsInIntro = false;
				m_lookSmoothing = false;
			}
		}
		else
		{
			base.transform.LookAt(vector2);
		}
	}

	private void GameIntro()
	{
		ResetAll();
		m_IsInIntro = true;
		m_lookSmoothing = true;
	}

	private void GameIntroEndListener()
	{
		ChangeCam(CameraType.BackCam);
	}

	private void GameStart()
	{
		if (!m_IsInIntro)
		{
			m_lookSmoothing = false;
			ChangeCam(CameraType.BackCam);
		}
	}

	private void GameOver()
	{
	}

	private void BossStartListener(MiniGameManager.BossType bossType)
	{
		ChangeCam(CameraType.BossBackCam);
	}

	private void BossEndListener(MiniGameManager.BossType bossType)
	{
		ChangeCam(CameraType.BackCam);
	}

	private void HangGlideStartListener()
	{
		ChangeCam(CameraType.BackCamHangGlide);
	}

	private void LaunchPerryListener()
	{
	}
}

using UnityEngine;

public class Token : MonoBehaviour
{
	private enum MagneticStates
	{
		Start = 0,
		Update = 1,
		None = 2
	}

	public enum TokenStates
	{
		Single = 0,
		Double = 1,
		Triple = 2
	}

	private Quaternion m_DeltaQuaternion;

	private bool m_ShouldCullCollision = true;

	private bool m_ShouldCullMagnet = true;

	private bool m_ShouldCullRotation = true;

	private static float m_CollideWithRunnerDist = 13f;

	public static bool ms_IsInMagneticTokenState;

	public static float ms_MagneticTokenStateDuration = 5f;

	private static float ms_MoveCloserDistHangGliding = 200f;

	private static float ms_MoveCloserDistNonHangGliding = 75f;

	private static float ms_MoveCloserDist = 75f;

	private static Timer ms_TokenTimer;

	public static TokenStates ms_TokenStates;

	private Vector3 m_InitialScale;

	private Vector3 m_InitialLocalPosition;

	private float m_RotateYDelta;

	private GameObject m_HaloAndParticlesGameObject;

	private GameObject m_HaloGameObject;

	private GameObject m_ParticlesGameObject;

	public bool m_IsOnDuckyPlatform;

	public bool m_IsOnBabyPlatform;

	private void Awake()
	{
		Vector3 localScale = base.transform.localScale;
		localScale *= 0.75f;
		base.transform.localScale = localScale;
		m_RotateYDelta = 90f;
		m_InitialScale = base.transform.localScale;
		m_InitialLocalPosition = base.transform.localPosition;
	}

	private void Update()
	{
		if (GameManager.The.IsGamePaused())
		{
			return;
		}
		if (ms_IsInMagneticTokenState)
		{
			UpdateMagneticTokenPosition();
			if (TheTokenTimer().IsFinished())
			{
				GameEventManager.TriggerPowerUpMagnetOff();
				EndMagneticTokenState();
			}
		}
		else
		{
			UpdateCollisionWithRunner();
		}
		if (!m_ShouldCullRotation)
		{
			base.transform.Rotate(0f, m_RotateYDelta * Time.deltaTime, 0f);
		}
	}

	public void StopCullingCollision()
	{
		m_ShouldCullCollision = false;
	}

	public void StopCullingMagnet()
	{
		m_ShouldCullMagnet = false;
	}

	public void StopCullingParticles()
	{
	}

	public void StopCullingRotation()
	{
		m_ShouldCullRotation = false;
	}

	private void UpdateCollisionWithRunner()
	{
		if (!m_ShouldCullCollision && DidTokenCollideWithRunner())
		{
			OnRunnerCollided();
		}
	}

	private bool DidTokenCollideWithRunner()
	{
		if ((Runner.The().transform.position - base.transform.position).sqrMagnitude < m_CollideWithRunnerDist * m_CollideWithRunnerDist)
		{
			return true;
		}
		return false;
	}

	private void OnRunnerCollided()
	{
		GameManager.The.DoTokenPickUpSound();
		if (PlayerData.coinDuplicatorInator)
		{
			GameEventManager.TriggerTokenHit(2);
		}
		else
		{
			GameEventManager.TriggerTokenHit(1);
		}
		if (DuckyMomoTrigger.ms_IsDuckyPlaying && !PlayerData.RoundIsDuckyTokenCollected)
		{
			PlayerData.RoundIsDuckyTokenCollected = true;
		}
		base.gameObject.SetActive(false);
	}

	private static Timer TheTokenTimer()
	{
		if (ms_TokenTimer == null)
		{
			ms_TokenTimer = TimerManager.The().SpawnTimer();
		}
		return ms_TokenTimer;
	}

	public static void StartMagneticTokenState()
	{
		if (MiniGameManager.The().RetrieveMiniGameCur() == MiniGameManager.MiniGameNames.HangGliding)
		{
			ms_MoveCloserDist = ms_MoveCloserDistHangGliding;
		}
		else
		{
			ms_MoveCloserDist = ms_MoveCloserDistNonHangGliding;
		}
		ms_IsInMagneticTokenState = true;
		ms_MagneticTokenStateDuration = PlayerData.MagnetUpgradeTime;
		TheTokenTimer().Start(ms_MagneticTokenStateDuration);
	}

	public static void EndMagneticTokenState()
	{
		ms_IsInMagneticTokenState = false;
	}

	public static void IncTokenState()
	{
		if (ms_TokenStates != TokenStates.Triple)
		{
			int num = (int)ms_TokenStates;
			num++;
			ms_TokenStates = (TokenStates)num;
		}
	}

	public static void DecTokenState()
	{
		if (ms_TokenStates != TokenStates.Single)
		{
			int num = (int)ms_TokenStates;
			num--;
			ms_TokenStates = (TokenStates)num;
		}
	}

	public static void ResetTokenState()
	{
		ms_TokenStates = TokenStates.Single;
	}

	private void SpawnHaloAndParticles()
	{
		m_HaloAndParticlesGameObject = CacheManager.The().Spawn("TokenGoldHaloAndParticles");
		m_HaloAndParticlesGameObject.transform.position = base.transform.position;
		m_HaloAndParticlesGameObject.transform.rotation = base.transform.rotation;
		m_HaloAndParticlesGameObject.transform.localScale = new Vector3(0.375f, 0.375f, 0.75f);
		m_HaloAndParticlesGameObject.transform.parent = base.transform;
	}

	private void SpawnHalo()
	{
		m_HaloGameObject = CacheManager.The().Spawn("TokenGoldHalo");
		m_HaloGameObject.transform.position = base.transform.position;
		m_HaloGameObject.transform.rotation = base.transform.rotation;
		m_HaloGameObject.transform.localScale = new Vector3(0.375f, 0.375f, 0.75f);
		m_HaloGameObject.transform.parent = base.transform;
	}

	private void SpawnParticles()
	{
		m_ParticlesGameObject = CacheManager.The().Spawn("TokenGoldParticles");
		m_ParticlesGameObject.transform.position = base.transform.position;
		m_ParticlesGameObject.transform.rotation = base.transform.rotation;
		m_ParticlesGameObject.transform.localScale = new Vector3(0.375f, 0.375f, 0.75f);
		m_ParticlesGameObject.transform.parent = base.transform;
	}

	public void UnspawnHalo()
	{
		if (m_HaloGameObject != null)
		{
			CacheManager.The().Unspawn(m_HaloGameObject);
		}
	}

	public void UnspawnParticles()
	{
		if (m_ParticlesGameObject != null)
		{
			CacheManager.The().Unspawn(m_ParticlesGameObject);
		}
	}

	public void UnspawnHaloAndParticles()
	{
		if (m_HaloAndParticlesGameObject != null)
		{
			CacheManager.The().Unspawn(m_HaloAndParticlesGameObject);
		}
	}

	public void TokenReset()
	{
		if (Application.isPlaying)
		{
			base.transform.localScale = m_InitialScale;
			base.transform.localPosition = m_InitialLocalPosition;
			base.transform.Rotate(Vector3.up, 90f * base.transform.localPosition.z / 125f);
			m_ShouldCullCollision = true;
			m_ShouldCullMagnet = true;
			m_ShouldCullRotation = true;
		}
	}

	private void UpdateMagneticTokenPosition()
	{
		if (m_ShouldCullMagnet)
		{
			return;
		}
		Vector3 vector = Runner.The().transform.position + 6f * Runner.The().transform.forward;
		vector.y += Runner.The().GetColliderCenterY();
		Vector3 vector2 = vector - base.transform.position;
		float sqrMagnitude = vector2.sqrMagnitude;
		if (sqrMagnitude < m_CollideWithRunnerDist * m_CollideWithRunnerDist)
		{
			OnRunnerCollided();
		}
		else if (!(sqrMagnitude > ms_MoveCloserDist * ms_MoveCloserDist))
		{
			float num = Mathf.Abs(ms_MoveCloserDist - vector2.magnitude);
			num /= ms_MoveCloserDist;
			float num2 = 1f - 0.5f * num;
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			base.transform.localScale = num2 * m_InitialScale;
			num *= 0.35f;
			vector2 *= num;
			Vector3 position = base.transform.position + vector2;
			base.transform.position = position;
		}
	}
}

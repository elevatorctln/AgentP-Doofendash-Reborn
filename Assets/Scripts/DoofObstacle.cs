using UnityEngine;

public class DoofObstacle : Obstacle
{
	private Animation m_Animation;

	private Animation m_DoofAnimation;

	public Object m_Doofenschmirtz;

	public string m_IdleAnimName = "Idle";

	public string m_DoofIdleAnimName = "Idle";

	private GameObject m_DoofSpawned;

	private void InitAnim()
	{
		if (m_Animation == null)
		{
			m_Animation = GetComponent<Animation>();
		}
	}

	private void InitDoofAnim()
	{
		if (m_DoofSpawned != null)
		{
			m_DoofAnimation = m_DoofSpawned.GetComponent<Animation>();
		}
	}

	public override void Init()
	{
		base.Init();
		if (m_Doofenschmirtz != null)
		{
			m_DoofSpawned = CacheManager.The().Spawn(m_Doofenschmirtz);
			InitDoofAnim();
			m_DoofAnimation.Rewind(m_DoofIdleAnimName);
			m_DoofAnimation.Play(m_DoofIdleAnimName);
			m_DoofSpawned.transform.position = base.transform.position;
			m_DoofSpawned.transform.parent = base.transform;
			PlayerData.RoundDoofenschmirtzEncounterCount++;
		}
	}

	public override void Uninit()
	{
		if (m_DoofSpawned != null)
		{
			CacheManager.The().Unspawn(m_DoofSpawned);
		}
		m_DoofSpawned = null;
	}

	public override void ResetObstacle()
	{
		base.ResetObstacle();
		InitAnim();
		m_Animation.Stop();
		m_Animation.Rewind(m_IdleAnimName);
		m_Animation.Play(m_IdleAnimName);
	}
}

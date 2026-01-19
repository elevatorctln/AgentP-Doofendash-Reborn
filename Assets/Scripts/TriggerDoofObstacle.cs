using UnityEngine;

public class TriggerDoofObstacle : TriggerObstacle
{
	private Animation m_DoofAnimation;

	public Object m_Doofenschmirtz;

	public Object m_DoofStand;

	public string m_IdleAnimName = "Idle";

	public string m_OneShotAnimName = "OneShot";

	private GameObject m_DoofSpawned;

	private GameObject m_DoofStandSpawned;

	public override void Init()
	{
		base.Init();
		if (m_Doofenschmirtz != null)
		{
			m_DoofSpawned = CacheManager.The().Spawn(m_Doofenschmirtz);
			m_DoofAnimation = m_DoofSpawned.GetComponent<Animation>();
			if (m_DoofAnimation != null)
			{
				m_DoofAnimation.Play(m_IdleAnimName);
			}
			m_DoofSpawned.transform.position = base.transform.position;
			m_DoofSpawned.transform.parent = base.transform;
			PlayerData.RoundDoofenschmirtzEncounterCount++;
		}
		if (m_DoofStand != null)
		{
			m_DoofStandSpawned = CacheManager.The().Spawn(m_DoofStand);
			m_DoofStandSpawned.transform.position = base.transform.position;
			m_DoofStandSpawned.transform.parent = base.transform;
		}
	}

	public override void Uninit()
	{
		if (m_DoofSpawned != null)
		{
			CacheManager.The().Unspawn(m_DoofSpawned);
		}
		if (m_DoofStandSpawned != null)
		{
			CacheManager.The().Unspawn(m_DoofStandSpawned);
		}
		m_DoofSpawned = null;
		m_DoofStandSpawned = null;
	}

	private void Awake()
	{
	}

	private void Update()
	{
		if (m_ObstacleSoundTimer != null && !b_SoundTimerHasFinished && SoundTimer().IsFinished())
		{
			GameManager.The.PlayClip(AudioClipFiles.ROOFTOPFOLDER + m_SoundClipName);
			b_SoundTimerHasFinished = true;
		}
	}

	private void TriggerDoofAnim()
	{
		if (m_SoundClipName.Length > 1)
		{
			SoundTimer().Start(m_SoundClipTimerOffset);
		}
		if (m_DoofAnimation != null)
		{
			m_DoofAnimation.Play(m_OneShotAnimName);
		}
	}

	private void OnTriggerEnter(Collider trigger)
	{
		TriggerDoofAnim();
		TriggerAll();
	}
}

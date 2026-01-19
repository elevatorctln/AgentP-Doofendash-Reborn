using UnityEngine;

public class TriggerObstacle : Obstacle
{
	private Animation m_Animation;

	public string[] m_AnimationList;

	public ParticleSystem m_ParticleSystem;

	private void InitAnim()
	{
		if (m_Animation == null)
		{
			m_Animation = GetComponent<Animation>();
		}
	}

	private void Awake()
	{
		InitAnim();
		m_Animation.Play("Idle");
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (m_ObstacleSoundTimer != null && !b_SoundTimerHasFinished && SoundTimer().IsFinished())
		{
			GameManager.The.PlayClip(m_SoundClipName);
			b_SoundTimerHasFinished = true;
		}
	}

	public override void ResetObstacle()
	{
		base.ResetObstacle();
		m_ObstacleSoundTimer = null;
		b_SoundTimerHasFinished = false;
		InitAnim();
		m_Animation.Stop();
		m_Animation.Rewind("Idle");
		m_Animation.Play("Idle");
	}

	private void OnTriggerEnter(Collider trigger)
	{
		TriggerAll();
	}

	private void TriggerFallAnim()
	{
		if (m_SoundClipName.Length > 1)
		{
			SoundTimer().Start(m_SoundClipTimerOffset);
		}
		if (m_AnimationList != null && m_AnimationList.Length > 0)
		{
			int num = Random.Range(0, m_AnimationList.Length);
			m_Animation.Play(m_AnimationList[num]);
		}
		else
		{
			m_Animation.Play("OneShot");
		}
	}

	private void TriggerParticleSystem()
	{
		if (m_ParticleSystem != null)
		{
			m_ParticleSystem.Play();
		}
	}

	public void TriggerAll()
	{
		TriggerFallAnim();
		TriggerParticleSystem();
	}
}

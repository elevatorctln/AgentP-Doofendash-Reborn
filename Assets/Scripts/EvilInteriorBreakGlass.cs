using UnityEngine;

public class EvilInteriorBreakGlass : MonoBehaviour
{
	public ParticleSystem m_ParticleSystem;

	private MeshRenderer m_MeshRenderer;

	private Collider m_Collider;

	private void Awake()
	{
		m_MeshRenderer = GetComponent<MeshRenderer>();
		m_Collider = GetComponent<Collider>();
		GameEventManager.GameIntro += GameIntroListener;
	}

	private void OnDestroy()
	{
		GameEventManager.GameIntro -= GameIntroListener;
	}

	private void GameIntroListener()
	{
		ResetGlass();
	}

	private void ResetGlass()
	{
		m_MeshRenderer.enabled = true;
		m_Collider.enabled = true;
	}

	private void OnTriggerEnter(Collider trigger)
	{
		m_MeshRenderer.enabled = false;
		m_Collider.enabled = false;
		PlayerData.RoundWindowsBroken++;
		PlayerData.AllTimeWindowsBroken++;
		m_ParticleSystem.Play();
		GameManager.The.PlayClip(AudioClipFiles.ROOFTOPFOLDER + AudioClipFiles.GLASSBREAK);
	}
}

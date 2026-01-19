using UnityEngine;

public class PlayableCharacterChooser : MonoBehaviour
{
	public bool m_owned;

	public int m_tokenCost = 1000;

	public string[] m_DisplayNames;

	public string[] m_ModelNames;

	public Animation m_RunnerAnim;

	public Animation m_CapsuleAnim;

	[HideInInspector]
	public int m_CurrentCostumeIndex;

	public string[] m_CostumeThumbnail;

	public string[] m_UniqueIDs;

	private GameObject m_CurrentRunner;

	private bool m_IsPlayingEndAnimations;

	public string FindDisplayName()
	{
		return FindDisplayName(m_CurrentCostumeIndex);
	}

	public string FindDisplayName(int index)
	{
		if (index < 0 || index >= m_DisplayNames.Length)
		{
			return string.Empty;
		}
		return m_DisplayNames[index];
	}

	public string FindModelName()
	{
		if (m_CurrentCostumeIndex >= m_ModelNames.Length)
		{
			return string.Empty;
		}
		return m_ModelNames[m_CurrentCostumeIndex];
	}

	public string FindUniqueID()
	{
		if (m_CurrentCostumeIndex >= m_UniqueIDs.Length)
		{
			return string.Empty;
		}
		return m_UniqueIDs[m_CurrentCostumeIndex];
	}

	public string FindUniqueID(int index)
	{
		if (index < 0 || index >= m_UniqueIDs.Length)
		{
			return string.Empty;
		}
		return m_UniqueIDs[index];
	}

	public void ChooseModelFromRunnerModelChooser()
	{
		if (RunnerModelChooser.The() != null)
		{
			if (m_CurrentRunner != null)
			{
				RunnerModelChooser.The().DeselectRunnerModel(m_CurrentRunner);
			}
			GameObject gameObject = (m_CurrentRunner = RunnerModelChooser.The().SelectRunnerModelForMenu(m_ModelNames[m_CurrentCostumeIndex]));
			m_RunnerAnim = gameObject.GetComponent<Animation>();
			m_IsPlayingEndAnimations = false;
			m_RunnerAnim.Rewind("RunnerCapsuleLoop");
			m_RunnerAnim.Play("RunnerCapsuleLoop");
			m_CapsuleAnim.Rewind("CapsuleLoop");
			m_CapsuleAnim.Play("CapsuleLoop");
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
		}
	}

	public void PlayIdleAnimations()
	{
		m_RunnerAnim.Rewind("RunnerCapsuleLoop");
		m_RunnerAnim.Play("RunnerCapsuleLoop");
		m_CapsuleAnim.Rewind("CapsuleLoop");
		m_CapsuleAnim.Play("CapsuleLoop");
	}

	public void PlayOpenDoorAnimation()
	{
		m_CapsuleAnim.Rewind("CapsuleOpenDoors");
		m_CapsuleAnim.Play("CapsuleOpenDoors");
	}

	public void PlayCloseDoorAnimation()
	{
		m_CapsuleAnim.Rewind("CapsuleCloseDoors");
		m_CapsuleAnim.Play("CapsuleCloseDoors");
	}

	public bool IsOpenDoorAnimationPlaying()
	{
		return m_CapsuleAnim.IsPlaying("CapsuleOpenDoors");
	}

	public bool IsCloseDoorAnimationPlaying()
	{
		return m_CapsuleAnim.IsPlaying("CapsuleCloseDoors");
	}

	public void PlayEndAnimations()
	{
		m_IsPlayingEndAnimations = true;
		m_RunnerAnim.Rewind("RunnerCapsuleEnd");
		m_RunnerAnim.Play("RunnerCapsuleEnd");
		m_CapsuleAnim.Rewind("CapsuleEnd");
		m_CapsuleAnim.Play("CapsuleEnd");
	}

	private void Update()
	{
		if (m_IsPlayingEndAnimations && (!m_RunnerAnim.IsPlaying("RunnerCapsuleEnd") || !m_CapsuleAnim.IsPlaying("CapsuleEnd")))
		{
			m_IsPlayingEndAnimations = false;
			m_RunnerAnim.Stop();
			m_CapsuleAnim.Stop();
			MainMenuEventManager.TriggerFinishedCapsuleEndAnims();
		}
	}
}

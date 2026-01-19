using UnityEngine;

public class EventPoint : MonoBehaviour
{
	public float m_TriggerEpsilon_0to1 = 0.05f;

	public float m_Time;

	private bool m_BeenTriggered;

	public bool BeenTriggered
	{
		get
		{
			return m_BeenTriggered;
		}
		set
		{
			m_BeenTriggered = value;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}

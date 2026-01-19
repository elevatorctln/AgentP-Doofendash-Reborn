using System.Collections.Generic;
using UnityEngine;

public class RewardsEventListener : MonoBehaviour
{
	private static bool m_showSuperSonic;

	public static bool ShowSuperSonic
	{
		get
		{
			return m_showSuperSonic;
		}
		set
		{
			m_showSuperSonic = value;
		}
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(this);
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Start()
	{

	}

	public void TrialPayMessage(string message)
	{
		
	}

	public void RewardTokens(int tokens)
	{
		
	}

	private void logDictionary(Dictionary<string, object> dict)
	{
		
	}
}

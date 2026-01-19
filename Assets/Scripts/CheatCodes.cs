using UnityEngine;

public class CheatCodes
{
	public enum Dudrant
	{
		TopHalf = 0,
		BottomHalf = 1,
		LeftHalf = 2,
		RightHalf = 3
	}

	private delegate bool TouchCheckDelegate(Vector3 vec);

	public delegate void OnCodeCompletedDelegate();

	private static TouchCheckDelegate[] m_TouchChecks = new TouchCheckDelegate[4] { IsInTopHalf, IsInBottomHalf, IsInLeftHalf, IsInRightHalf };

	public OnCodeCompletedDelegate m_OnCodeCompletedDelegate;

	private int m_CurrentTouchPosition;

	private Dudrant[] m_Code = new Dudrant[2]
	{
		Dudrant.TopHalf,
		Dudrant.BottomHalf
	};

	private bool m_MouseDown;

	private bool m_StartedSequence;

	private Timer m_Timer;

	public CheatCodes(Dudrant[] code, OnCodeCompletedDelegate occd)
	{
		m_Code = code;
		m_OnCodeCompletedDelegate = occd;
	}

	public void Start()
	{
		m_Timer = TimerManager.The().SpawnTimer();
	}

	public void UpdateInput()
	{
		if (m_StartedSequence && m_Timer.IsFinished())
		{
			m_StartedSequence = false;
			m_CurrentTouchPosition = 0;
		}
		if (Input.GetMouseButtonDown(0))
		{
			m_MouseDown = true;
		}
		if (!Input.GetMouseButtonUp(0) || !m_MouseDown)
		{
			return;
		}
		int num = (int)m_Code[m_CurrentTouchPosition];
		if (m_TouchChecks[num](Input.mousePosition))
		{
			m_Timer.Start(1f);
			m_StartedSequence = true;
			m_CurrentTouchPosition++;
			if (m_CurrentTouchPosition >= m_Code.Length)
			{
				m_CurrentTouchPosition = 0;
				m_OnCodeCompletedDelegate();
			}
		}
		m_MouseDown = false;
	}

	private static bool IsInTopHalf(Vector3 screenPosition)
	{
		if (screenPosition.y > (float)(Screen.height / 2))
		{
			return true;
		}
		return false;
	}

	private static bool IsInBottomHalf(Vector3 screenPosition)
	{
		if (screenPosition.y < (float)(Screen.height / 2))
		{
			return true;
		}
		return false;
	}

	private static bool IsInLeftHalf(Vector3 screenPosition)
	{
		if (screenPosition.x < (float)(Screen.width / 2))
		{
			return true;
		}
		return false;
	}

	private static bool IsInRightHalf(Vector3 screenPosition)
	{
		if (screenPosition.x > (float)(Screen.width / 2))
		{
			return true;
		}
		return false;
	}
}

using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
	private static TimerManager m_The;

	private List<Timer> m_TimerList = new List<Timer>();

	private List<StopWatch> m_StopWatchList = new List<StopWatch>();

	public static TimerManager The()
	{
		if (m_The == null)
		{
			m_The = GameObject.Find("TimerManager").GetComponent<TimerManager>();
		}
		return m_The;
	}

	private void Awake()
	{
		m_The = this;
	}

	private void OnDestroy()
	{
		m_TimerList.Clear();
		m_StopWatchList.Clear();
	}

	private void Start()
	{
	}

	private void Update()
	{
		for (int i = 0; i < m_TimerList.Count; i++)
		{
			m_TimerList[i].Update();
		}
		for (int j = 0; j < m_StopWatchList.Count; j++)
		{
			m_StopWatchList[j].Update();
		}
	}

	public Timer SpawnTimer()
	{
		Timer timer = new Timer();
		m_TimerList.Add(timer);
		return timer;
	}

	public void UnspawnTimer(Timer timer)
	{
		m_TimerList.Remove(timer);
	}

	public StopWatch SpawnStopWatch()
	{
		StopWatch stopWatch = new StopWatch();
		m_StopWatchList.Add(stopWatch);
		return stopWatch;
	}

	public void UnspawnStopWatch(StopWatch sw)
	{
		m_StopWatchList.Remove(sw);
	}
}

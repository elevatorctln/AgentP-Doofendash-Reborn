using System.Collections.Generic;
using UnityEngine;

public class PerryLeaderboard
{
	public enum PerryLeaderboardTimeScope
	{
		Today = 0,
		Week = 1,
		AllTime = 2
	}

	public enum TempXBLALeaderboardTimeScope
	{
		Unknown = -1,
		Today = 1,
		ThisWeek = 2,
		AllTime = 3
	}

	public delegate void AddScoreCallback(PerryHighScore score);

	private GPGLeaderboardMetadata m_googleLeaderboard;

	private List<PerryHighScore> m_scores;

	private List<PerryHighScore> m_playerScores;

	private static List<PerryHighScore> m_combinedOrderedScores;

	public int TotalScores
	{
		get
		{
			return m_combinedOrderedScores.Count;
		}
	}

	public string LeaderboardID
	{
		get
		{
			return m_googleLeaderboard.leaderboardId;
		}
	}

	public string Title
	{
		get
		{
			return m_googleLeaderboard.title;
		}
	}

	public PerryLeaderboard(GPGLeaderboardMetadata lb)
	{
		Debug.Log("new Android PerryLeaderboard");
		m_googleLeaderboard = lb;
		m_scores = new List<PerryHighScore>();
		m_playerScores = new List<PerryHighScore>();
		m_combinedOrderedScores = new List<PerryHighScore>();
	}

	public void ClearScores()
	{
		m_scores.Clear();
		m_playerScores.Clear();
		m_combinedOrderedScores.Clear();
	}

	public void LoadScores(PerryLeaderboardTimeScope timeScope)
	{
		GPGLeaderboardTimeScope timeScope2 = GPGLeaderboardTimeScope.AllTime;
		switch (timeScope)
		{
		case PerryLeaderboardTimeScope.AllTime:
			timeScope2 = GPGLeaderboardTimeScope.AllTime;
			break;
		case PerryLeaderboardTimeScope.Week:
			timeScope2 = GPGLeaderboardTimeScope.ThisWeek;
			break;
		case PerryLeaderboardTimeScope.Today:
			timeScope2 = GPGLeaderboardTimeScope.Today;
			break;
		}
		LoadScores(timeScope2);
	}

	private void LoadScores(GPGLeaderboardTimeScope timeScope)
	{
		Debug.Log("LoadScores Android");
		ClearScores();
		PerryGameServices.LoadRawScoreData(LeaderboardID, timeScope);
	}

	public void AddAndroidScores(List<GPGScore> scores)
	{
	}

	public void AddScore(PerryHighScore score)
	{
		m_scores.Add(score);
		AddToOrderedScoreList(ref m_combinedOrderedScores, score);
	}

	public void AddPlayerScore(PerryHighScore score)
	{
		score.IsMe = true;
		m_playerScores.Add(score);
		AddToOrderedScoreList(ref m_combinedOrderedScores, score);
	}

	public void AddFbScore(PerryHighScore score)
	{
		m_scores.Add(score);
		AddToOrderedScoreList(ref m_combinedOrderedScores, score);
	}

	public PerryHighScore GetCombinedOrderedScoreAtIndex(int index)
	{
		if (index < m_combinedOrderedScores.Count)
		{
			if (m_combinedOrderedScores[index] == null)
			{
				Debug.Log("this entry is null!");
				return null;
			}
			return m_combinedOrderedScores[index];
		}
		return null;
	}

	public PerryHighScore GetMyPerryHighScore()
	{
		foreach (PerryHighScore combinedOrderedScore in m_combinedOrderedScores)
		{
			if (combinedOrderedScore.IsMe)
			{
				return combinedOrderedScore;
			}
		}
		return null;
	}

	private static void AddToOrderedScoreList(ref List<PerryHighScore> orderedList, PerryHighScore score)
	{
		if (orderedList.Count == 0)
		{
			orderedList.Add(score);
			return;
		}
		for (int i = 0; i < orderedList.Count; i++)
		{
			if (score.Name == orderedList[i].Name)
			{
				return;
			}
			if (score.ScoreVal > orderedList[i].ScoreVal)
			{
				orderedList.Insert(i, score);
				return;
			}
		}
		orderedList.Add(score);
	}

	private static void OrderScoreList(ref List<PerryHighScore> scoreList)
	{
		List<PerryHighScore> orderedList = new List<PerryHighScore>();
		foreach (PerryHighScore score in scoreList)
		{
			AddToOrderedScoreList(ref orderedList, score);
		}
		scoreList.Clear();
		scoreList = null;
		scoreList = orderedList;
	}

	public static void SortCombinedOrderedScores()
	{
		if (m_combinedOrderedScores != null)
		{
			OrderScoreList(ref m_combinedOrderedScores);
		}
	}
}

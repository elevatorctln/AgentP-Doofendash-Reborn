using System.Collections;

public static class AllLevelData
{
	private static ArrayList m_allLevels;

	public static void Init()
	{
		InitAllLevels();
		GetAllStoredLevelData();
	}

	public static Level FindLevel(int index)
	{
		return (Level)m_allLevels[index];
	}

	private static void InitAllLevels()
	{
		if (m_allLevels != null && m_allLevels.Count > 0)
		{
			return;
		}
		m_allLevels = new ArrayList();
		string text = "_LEVEL_";
		string text2 = "_NAME_";
		int num = 1;
		int num2 = 1;
		int num3 = 6;
		int num4 = 14;
		int num5 = 19;
		int num6 = 40;
		for (int i = 0; i < 40; i++)
		{
			int badgeVal = 3;
			int multiplier = 1;
			if (i + 1 <= num2)
			{
				multiplier = 1;
			}
			else if (i + 1 <= num3)
			{
				multiplier = 2;
			}
			else if (i + 1 <= num4)
			{
				multiplier = 3;
			}
			else if (i + 1 <= num5)
			{
				multiplier = 4;
			}
			else if (i + 1 <= num6)
			{
				multiplier = 5;
			}
			string text3 = text + num.ToString("N0");
			Level value = new Level(text3, LocalTextManager.GetLevelText(text3 + text2), badgeVal, multiplier, i);
			m_allLevels.Add(value);
			num++;
		}
	}

	private static void GetAllStoredLevelData()
	{
		if (m_allLevels != null)
		{
			for (int i = 0; i < m_allLevels.Count; i++)
			{
				Level l = (Level)m_allLevels[i];
				GetStoredLevelData(ref l);
				m_allLevels[i] = l;
			}
		}
	}

	private static void StoreAllLevels()
	{
		for (int i = 0; i < m_allLevels.Count; i++)
		{
			StoreLevelData((Level)m_allLevels[i]);
		}
	}

	private static void StoreLevelData(Level l)
	{
		PerryiCloudManager.The.SetItem(l.Key, l);
	}

	private static void GetStoredLevelData(ref Level l)
	{
		PerryiCloudManager.The.GetLevel(l.Key);
	}

	public static void ReloadAllLevelText()
	{
		if (m_allLevels != null)
		{
			string text = "_LEVEL_";
			string text2 = "_NAME_";
			int num = 1;
			for (int i = 0; i < m_allLevels.Count; i++)
			{
				string text3 = text + num.ToString("N0");
				((Level)m_allLevels[i]).m_title = LocalTextManager.GetLevelText(text3 + text2);
				num++;
			}
		}
	}
}

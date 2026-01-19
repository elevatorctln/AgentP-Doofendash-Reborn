public class Level
{
	private string m_key = "00000";

	public string m_title;

	public int m_badgeVal = 1;

	public int m_multiplier = 1;

	public bool m_isComplete;

	public int m_numericLevel = 1;

	public string Key
	{
		get
		{
			return m_key;
		}
	}

	public Level(string key)
	{
		m_key = key;
	}

	public Level(string key, string title, int badgeVal, int multiplier, int numericLevel)
	{
		m_key = key;
		m_title = title;
		m_badgeVal = 3;
		m_multiplier = 1;
		m_numericLevel = numericLevel;
	}

	public override string ToString()
	{
		return string.Format("Level Item\nTitle: " + m_title + "\nBadgVal: " + m_badgeVal + "\nMultiplier: " + m_multiplier + "\n");
	}
}

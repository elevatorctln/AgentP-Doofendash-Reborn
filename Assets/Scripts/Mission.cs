public class Mission
{
	public delegate bool MissionCheckUpdate();

	private string m_key = "00000";

	public int m_levelVal = 1;

	public string m_name = "Mission Name";

	public string m_desc = "Mission description";

	public int m_multiplier = 1;

	public int m_scoreBonus;

	public int m_skipTokenCost = 1000;

	private bool m_completed;

	private MissionCheckUpdate m_missionCheckUpdateCallback;

	public string Key
	{
		get
		{
			return m_key;
		}
	}

	public bool Completed
	{
		get
		{
			return m_completed;
		}
		set
		{
			m_completed = value;
		}
	}

	public MissionCheckUpdate MissionCheckUpdateCallback
	{
		set
		{
			m_missionCheckUpdateCallback = value;
		}
	}

	public Mission(string key)
	{
		m_key = key;
	}

	public Mission(string key, string name, string desc, int levelVal, int multiplier, int scoreBonus, int tokenCost, MissionCheckUpdate updateCallback = null)
	{
		m_key = key;
		m_name = name;
		m_desc = desc;
		m_levelVal = 1;
		m_multiplier = multiplier;
		m_scoreBonus = scoreBonus;
		m_skipTokenCost = tokenCost;
		m_missionCheckUpdateCallback = updateCallback;
	}

	public bool CheckInGameUpdate()
	{
		if (m_missionCheckUpdateCallback != null)
		{
			return m_missionCheckUpdateCallback();
		}
		return false;
	}

	public override string ToString()
	{
		return string.Format("Mission Item\nName: " + m_name + "\nDesc: " + m_desc + "\nLevelVal: " + m_levelVal + "\nMultiplier: " + m_multiplier + "\nScore Bonus: " + m_scoreBonus + "\nCompleted: " + m_completed + "\n");
	}
}

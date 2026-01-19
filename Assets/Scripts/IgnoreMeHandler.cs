using System.Collections;

public class IgnoreMeHandler
{
	public enum Action
	{
		Ignore = 0,
		OkToSpawn = 1
	}

	private Hashtable m_IgnoreMeList = new Hashtable();

	public Action UpdateOnNewSpawn(object key, int in_ignoreMeCount)
	{
		if (in_ignoreMeCount > 0)
		{
			if (m_IgnoreMeList.Contains(key))
			{
				int num = (int)m_IgnoreMeList[key];
				num--;
				if (num > 0)
				{
					m_IgnoreMeList[key] = num;
				}
				else
				{
					m_IgnoreMeList.Remove(key);
				}
				return Action.Ignore;
			}
			m_IgnoreMeList.Add(key, in_ignoreMeCount);
		}
		return Action.OkToSpawn;
	}
}

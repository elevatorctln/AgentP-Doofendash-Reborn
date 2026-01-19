using System.Collections;

public class AGSProfile
{
	public string alias;

	public string playerId;

	public static AGSProfile fromHashtable(Hashtable ht)
	{
		AGSProfile aGSProfile = new AGSProfile();
		aGSProfile.alias = getStringValue(ht, "alias");
		aGSProfile.playerId = getStringValue(ht, "playerId");
		return aGSProfile;
	}

	private static string getStringValue(Hashtable ht, string key)
	{
		if (ht.Contains(key))
		{
			return ht[key].ToString();
		}
		return null;
	}

	public override string ToString()
	{
		return string.Format("alias: {0}, playerId: {1}", alias, playerId);
	}
}

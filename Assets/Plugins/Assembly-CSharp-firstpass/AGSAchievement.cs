using System;
using System.Collections;
using UnityEngine;

public class AGSAchievement
{
	public string title;

	public string id;

	public int pointValue;

	public bool isHidden;

	public bool isUnlocked;

	public float progress;

	public int position;

	public string decription;

	public DateTime dateUnlocked;

	public static AGSAchievement fromHashtable(Hashtable ht)
	{
		AGSAchievement aGSAchievement = new AGSAchievement();
		aGSAchievement.title = getStringValue(ht, "title");
		aGSAchievement.id = getStringValue(ht, "id");
		aGSAchievement.decription = getStringValue(ht, "description");
		try
		{
			string stringValue = getStringValue(ht, "pointValue");
			aGSAchievement.pointValue = int.Parse(stringValue);
		}
		catch (FormatException ex)
		{
			Debug.Log("Unable to parse pointValue from achievement " + ex.Message);
		}
		catch (ArgumentNullException ex2)
		{
			Debug.Log("pointValue not found  " + ex2.Message);
		}
		try
		{
			string stringValue2 = getStringValue(ht, "position");
			aGSAchievement.position = int.Parse(stringValue2);
		}
		catch (FormatException ex3)
		{
			Debug.Log("Unable to parse position from achievement " + ex3.Message);
		}
		catch (ArgumentNullException ex4)
		{
			Debug.Log("position not found " + ex4.Message);
		}
		try
		{
			string stringValue3 = getStringValue(ht, "progress");
			aGSAchievement.progress = float.Parse(stringValue3);
		}
		catch (FormatException ex5)
		{
			Debug.Log("Unable to parse progress from achievement " + ex5.Message);
		}
		catch (ArgumentNullException ex6)
		{
			Debug.Log("progress not found " + ex6.Message);
		}
		try
		{
			string stringValue4 = getStringValue(ht, "hidden");
			aGSAchievement.isHidden = bool.Parse(stringValue4);
		}
		catch (FormatException ex7)
		{
			Debug.Log("Unable to parse isHidden from achievement " + ex7.Message);
		}
		catch (ArgumentNullException ex8)
		{
			Debug.Log("isHidden not found " + ex8.Message);
		}
		try
		{
			string stringValue5 = getStringValue(ht, "unlocked");
			aGSAchievement.isUnlocked = bool.Parse(stringValue5);
		}
		catch (FormatException ex9)
		{
			Debug.Log("Unable to parse isUnlocked from achievement " + ex9.Message);
		}
		catch (ArgumentNullException ex10)
		{
			Debug.Log("isUnlocked not found " + ex10.Message);
		}
		try
		{
			string stringValue6 = getStringValue(ht, "dateUnlocked");
			long num = long.Parse(stringValue6);
			aGSAchievement.dateUnlocked = getTimefromEpochTime(num);
		}
		catch (FormatException ex11)
		{
			Debug.Log("Unable to parse dateUnlocked from achievement " + ex11.Message);
		}
		catch (ArgumentNullException ex12)
		{
			Debug.Log("dateUnlocked not found " + ex12.Message);
		}
		return aGSAchievement;
	}

	private static DateTime getTimefromEpochTime(double javaTimeStamp)
	{
		return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(javaTimeStamp / 1000.0)).ToLocalTime();
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
		return string.Format("title: {0}, id: {1}, pointValue: {2}, hidden: {3}, unlocked: {4}, progress: {5}, position: {6}, date: {7} ", title, id, pointValue, isHidden, isUnlocked, progress, position, dateUnlocked);
	}
}

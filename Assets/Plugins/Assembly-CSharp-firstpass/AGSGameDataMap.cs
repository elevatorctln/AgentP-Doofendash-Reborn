using System.Collections.Generic;
using UnityEngine;

public class AGSGameDataMap : AGSSyncable
{
	public AGSGameDataMap(AndroidJavaObject javaObject)
		: base(javaObject)
	{
	}

	public AGSSyncableNumber GetHighestNumber(string name)
	{
		return GetAGSSyncable<AGSSyncableNumber>("getHighestNumber", name);
	}

	public HashSet<string> GetHighestNumberKeys()
	{
		return GetHashSet("getHighestNumberKeys");
	}

	public AGSSyncableNumber GetLowestNumber(string name)
	{
		return GetAGSSyncable<AGSSyncableNumber>("getLowestNumber", name);
	}

	public HashSet<string> GetLowestNumberKeys()
	{
		return GetHashSet("getLowestNumberKeys");
	}

	public AGSSyncableNumber GetLatestNumber(string name)
	{
		return GetAGSSyncable<AGSSyncableNumber>("getLatestNumber", name);
	}

	public HashSet<string> GetLatestNumberKeys()
	{
		return GetHashSet("getLatestNumberKeys");
	}

	public AGSSyncableNumberList GetHighNumberList(string name)
	{
		return GetAGSSyncable<AGSSyncableNumberList>("getHighNumberList", name);
	}

	public HashSet<string> GetHighNumberListKeys()
	{
		return GetHashSet("getHighNumberListKeys");
	}

	public AGSSyncableNumberList GetLowNumberList(string name)
	{
		return GetAGSSyncable<AGSSyncableNumberList>("getLowNumberList", name);
	}

	public HashSet<string> GetLowNumberListKeys()
	{
		return GetHashSet("getLowNumberListKeys");
	}

	public AGSSyncableNumberList GetLatestNumberList(string name)
	{
		return GetAGSSyncable<AGSSyncableNumberList>("getLatestNumberList", name);
	}

	public HashSet<string> GetLatestNumberListKeys()
	{
		return GetHashSet("getLatestNumberListKeys");
	}

	public AGSSyncableAccumulatingNumber GetAccumulatingNumber(string name)
	{
		return GetAGSSyncable<AGSSyncableAccumulatingNumber>("getAccumulatingNumber", name);
	}

	public HashSet<string> GetAccumulatingNumberKeys()
	{
		return GetHashSet("getAccumulatingNumberKeys");
	}

	public AGSSyncableString GetLatestString(string name)
	{
		return GetAGSSyncable<AGSSyncableString>("getLatestString", name);
	}

	public HashSet<string> GetLatestStringKeys()
	{
		return GetHashSet("getLatestStringKeys");
	}

	public AGSSyncableStringList GetLatestStringList(string name)
	{
		return GetAGSSyncable<AGSSyncableStringList>("getLatestStringList", name);
	}

	public HashSet<string> GetLatestStringListKeys()
	{
		return GetHashSet("getLatestStringListKeys");
	}

	public AGSSyncableStringSet GetStringSet(string name)
	{
		return GetAGSSyncable<AGSSyncableStringSet>("getStringSet", name);
	}

	public HashSet<string> GetStringSetKeys()
	{
		return GetHashSet("getStringSetKeys");
	}

	public AGSGameDataMap GetMap(string name)
	{
		return GetAGSSyncable<AGSGameDataMap>("getMap");
	}

	public HashSet<string> GetMapKeys()
	{
		return GetHashSet("getMapKeys");
	}
}

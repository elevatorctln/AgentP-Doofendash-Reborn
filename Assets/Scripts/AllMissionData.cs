using System.Collections;
using System.Diagnostics;
using UnityEngine;

public static class AllMissionData
{
	private static ArrayList m_allMissions;

	private static int ms_CompletedMissionsCount;

	public static bool ms_IsCompletedMissionsCountCalculated;

	private static bool m_TotalScoreMultiplierIsCalculated;

	private static int m_totalScoreMultipier = 1;

	public static bool TotalScoreMultiplierIsCalculated
	{
		get
		{
			return m_TotalScoreMultiplierIsCalculated;
		}
		set
		{
			m_TotalScoreMultiplierIsCalculated = value;
		}
	}

	public static int TotalScoreMultiplier
	{
		get
		{
			int num = CalcTotalScoreMultiplier();
			if (GameManager.The.IsScoreMultiplierOn())
			{
				num *= 2;
			}
			return num;
		}
	}

	public static void Init()
	{
		InitAllMissions();
		InitAllMissionsFromStoredData();
	}

	private static ArrayList TheAllMissionsArray()
	{
		if (m_allMissions == null)
		{
			Init();
		}
		return m_allMissions;
	}

	private static void InitAllMissions()
	{
		if (m_allMissions == null || m_allMissions.Count <= 0)
		{
			m_allMissions = new ArrayList();
			string text = "_NAME_";
			string text2 = "_DESC_";
			int num = 1;
			int tokenCost = 2000;
			string text3 = "_MISSION_" + num.ToString("N0");
			Mission value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 1, 1, 200, tokenCost, AllMissionCheckUpdates.Mission1);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 200, tokenCost, AllMissionCheckUpdates.Mission2);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 5, 1, 200, tokenCost, AllMissionCheckUpdates.Mission3);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 1, 1, 200, tokenCost, AllMissionCheckUpdates.Mission4);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 1, 1, 200, tokenCost, AllMissionCheckUpdates.Mission5);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 1, 1, 400, tokenCost, AllMissionCheckUpdates.Mission6);
			m_allMissions.Add(value);
			tokenCost = 4000;
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 1, 1, 400, tokenCost, AllMissionCheckUpdates.Mission7);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 1, 1, 400, tokenCost, AllMissionCheckUpdates.Mission8);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 1, 1, 400, tokenCost, AllMissionCheckUpdates.Mission9);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 1, 1, 400, tokenCost, AllMissionCheckUpdates.Mission10);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 1, 1, 800, tokenCost, AllMissionCheckUpdates.Mission11);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 800, tokenCost, AllMissionCheckUpdates.Mission12);
			m_allMissions.Add(value);
			tokenCost = 8000;
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 800, tokenCost, AllMissionCheckUpdates.Mission13);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 800, tokenCost, AllMissionCheckUpdates.Mission14);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 800, tokenCost, AllMissionCheckUpdates.Mission15);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 800, tokenCost, AllMissionCheckUpdates.Mission16);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 800, tokenCost, AllMissionCheckUpdates.Mission17);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 1200, tokenCost, AllMissionCheckUpdates.Mission18);
			m_allMissions.Add(value);
			tokenCost = 12000;
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 1200, tokenCost, AllMissionCheckUpdates.Mission19);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 1200, tokenCost, AllMissionCheckUpdates.Mission20);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 1200, tokenCost, AllMissionCheckUpdates.Mission21);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 1200, tokenCost, AllMissionCheckUpdates.Mission22);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 1200, tokenCost, AllMissionCheckUpdates.Mission23);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 1200, tokenCost, AllMissionCheckUpdates.Mission24);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 1200, tokenCost, AllMissionCheckUpdates.Mission25);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 2000, tokenCost, AllMissionCheckUpdates.Mission26);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 2000, tokenCost, AllMissionCheckUpdates.Mission27);
			m_allMissions.Add(value);
			tokenCost = 20000;
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 2000, tokenCost, AllMissionCheckUpdates.Mission28);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 2000, tokenCost, AllMissionCheckUpdates.Mission29);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 2000, tokenCost, AllMissionCheckUpdates.Mission30);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 2000, tokenCost, AllMissionCheckUpdates.Mission31);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 2000, tokenCost, AllMissionCheckUpdates.Mission32);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 2000, tokenCost, AllMissionCheckUpdates.Mission33);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 2000, tokenCost, AllMissionCheckUpdates.Mission34);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 2000, tokenCost, AllMissionCheckUpdates.Mission35);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 2, 1, 2000, tokenCost, AllMissionCheckUpdates.Mission36);
			m_allMissions.Add(value);
			tokenCost = 0;
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 3, 1, 5000, tokenCost, AllMissionCheckUpdates.Mission37);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 3, 1, 5000, tokenCost, AllMissionCheckUpdates.Mission38);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 3, 1, 5000, tokenCost, AllMissionCheckUpdates.Mission39);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 3, 1, 5000, tokenCost, AllMissionCheckUpdates.Mission40);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 3, 1, 5000, tokenCost, AllMissionCheckUpdates.Mission41);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 3, 1, 5000, tokenCost, AllMissionCheckUpdates.Mission42);
			m_allMissions.Add(value);
			tokenCost = 150000;
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 3, 1, 15000, tokenCost, AllMissionCheckUpdates.Mission43);
			m_allMissions.Add(value);
			num++;
			text3 = "_MISSION_" + num.ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 3, 1, 15000, tokenCost, AllMissionCheckUpdates.Mission44);
			m_allMissions.Add(value);
			text3 = "_MISSION_" + (num + 1).ToString("N0");
			value = new Mission(text3, LocalTextManager.GetMissionText(text3 + text), LocalTextManager.GetMissionText(text3 + text2), 3, 1, 15000, tokenCost, AllMissionCheckUpdates.Mission45);
			m_allMissions.Add(value);
		}
	}

	private static void InitAllMissionsFromStoredData()
	{
		if (m_allMissions != null && m_allMissions.Count > 0)
		{
			for (int i = 0; i < m_allMissions.Count; i++)
			{
				Mission m = (Mission)m_allMissions[i];
				GetMissionData(ref m);
			}
		}
	}

	private static void StoreAllMissionData()
	{
		ArrayList arrayList = TheAllMissionsArray();
		for (int i = 0; i < arrayList.Count; i++)
		{
			StoreMissionData((Mission)arrayList[i]);
		}
	}

	public static void StoreMissionData(Mission m)
	{
		PerryiCloudManager.The.SetItem(m.Key, m);
	}

	private static void GetMissionData(ref Mission m)
	{
		Mission mission = PerryiCloudManager.The.CreateMission(m.Key);
		if (mission != null)
		{
			m.Completed = mission.Completed;
			m.m_levelVal = mission.m_levelVal;
			m.m_multiplier = mission.m_multiplier;
			m.m_scoreBonus = mission.m_scoreBonus;
			m.m_skipTokenCost = mission.m_skipTokenCost;
		}
	}

	public static ArrayList GetGroupofMissionsForLevel(int level)
	{
		level++;
		level = Mathf.Max(1, level);
		ArrayList arrayList = TheAllMissionsArray();
		int[] array = new int[3]
		{
			level * 3 - 3,
			level * 3 - 2,
			level * 3 - 1
		};
		if (array[0] < 0 || array[2] >= arrayList.Count)
		{
			return null;
		}
		ArrayList arrayList2 = new ArrayList();
		arrayList2.Add((Mission)arrayList[array[0]]);
		arrayList2.Add((Mission)arrayList[array[1]]);
		arrayList2.Add((Mission)arrayList[array[2]]);
		return arrayList2;
	}

	public static Mission GetMissionForLevel(int level, int index)
	{
		level++;
		level = Mathf.Max(1, level);
		ArrayList arrayList = TheAllMissionsArray();
		int[] array = new int[3]
		{
			level * 3 - 3,
			level * 3 - 2,
			level * 3 - 1
		};
		if (array[0] < 0 || array[2] >= arrayList.Count)
		{
			return null;
		}
		return (Mission)arrayList[array[index]];
	}

	public static bool IsMissionGroupCompleted(int level)
	{
		bool result = false;
		if (CalcMissionGroupMissionCompletedCount(level) == 3)
		{
			result = true;
		}
		return result;
	}

	public static int CalcMissionGroupMissionCompletedCount(int level)
	{
		level++;
		level = Mathf.Max(1, level);
		ArrayList arrayList = TheAllMissionsArray();
		int[] array = new int[3]
		{
			level * 3 - 3,
			level * 3 - 2,
			level * 3 - 1
		};
		if (array[0] < 0 || array[2] >= arrayList.Count)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			if (((Mission)arrayList[array[i]]).Completed)
			{
				num++;
			}
		}
		return num;
	}

	public static int CalcCompletedMissionsCount()
	{
		if (ms_IsCompletedMissionsCountCalculated)
		{
			return ms_CompletedMissionsCount;
		}
		ArrayList arrayList = TheAllMissionsArray();
		ms_CompletedMissionsCount = 0;
		for (int i = 0; i < arrayList.Count; i++)
		{
			if (((Mission)arrayList[i]).Completed)
			{
				ms_CompletedMissionsCount++;
			}
		}
		ms_IsCompletedMissionsCountCalculated = true;
		return ms_CompletedMissionsCount;
	}

	public static int CalcMissionLevelCompletedCount()
	{
		return CalcCompletedMissionsCount() / 3;
	}

	public static ArrayList GetFirstGroupOfIncompleteMissions()
	{
		return GetGroupofMissionsForLevel(CalcMissionLevelCompletedCount());
	}

	public static bool AreAllMissionsCompleted()
	{
		if (CalcCompletedMissionsCount() == m_allMissions.Count)
		{
			return true;
		}
		return false;
	}

	private static int CalcTotalScoreMultiplier()
	{
		if (m_TotalScoreMultiplierIsCalculated)
		{
			return m_totalScoreMultipier;
		}
		int num = CalcMissionLevelCompletedCount();
		m_totalScoreMultipier = 1;
		for (int i = 0; i < num; i++)
		{
			m_totalScoreMultipier += AllLevelData.FindLevel(i).m_multiplier;
		}
		m_TotalScoreMultiplierIsCalculated = true;
		return m_totalScoreMultipier;
	}

	public static void UpdateMissionCompletes()
	{
		ms_IsCompletedMissionsCountCalculated = false;
		m_TotalScoreMultiplierIsCalculated = false;
		StoreAllMissionData();
	}

	public static void ReloadAllMissionText()
	{
		ArrayList arrayList = TheAllMissionsArray();
		if (arrayList != null && arrayList.Count > 0)
		{
			string text = "_NAME_";
			string text2 = "_DESC_";
			int num = 1;
			for (int i = 0; i < arrayList.Count; i++)
			{
				string text3 = "_MISSION_" + num.ToString("N0");
				((Mission)arrayList[i]).m_name = LocalTextManager.GetMissionText(text3 + text);
				((Mission)arrayList[i]).m_desc = LocalTextManager.GetMissionText(text3 + text2);
				num++;
			}
		}
	}

	[Conditional("USE_DEBUG_INGAME")]
	public static void DebugSetAllMissionsComplete()
	{
		if (m_allMissions != null)
		{
			for (int i = 0; i < m_allMissions.Count; i++)
			{
				Mission mission = (Mission)m_allMissions[i];
				mission.Completed = true;
			}
			PlayerData.MaxMissionLevelSeenByUserIndex = m_allMissions.Count;
			ms_IsCompletedMissionsCountCalculated = false;
		}
	}

	[Conditional("USE_DEBUG_INGAME")]
	public static void DebugSetAllMissionsIncomplete()
	{
		if (m_allMissions != null)
		{
			for (int i = 0; i < m_allMissions.Count; i++)
			{
				Mission mission = (Mission)m_allMissions[i];
				mission.Completed = false;
			}
			PlayerData.MaxMissionLevelSeenByUserIndex = 0;
			ms_IsCompletedMissionsCountCalculated = false;
		}
	}

	[Conditional("USE_DEBUG_INGAME")]
	public static void DebugSetMissionsComplete(int missionCount)
	{
		if (m_allMissions != null)
		{
			for (int i = 0; i < missionCount && i < m_allMissions.Count; i++)
			{
				Mission mission = (Mission)m_allMissions[i];
				mission.Completed = true;
			}
			PlayerData.MaxMissionLevelSeenByUserIndex = missionCount / PlayerData.GetMissionCountPerGroup();
			ms_IsCompletedMissionsCountCalculated = false;
		}
	}
}

using System;
using System.Collections;
using UnityEngine;

public static class LocalTextManager
{
	public enum PerryLanguages
	{
		English = 1,
		French = 2,
		Italian = 3,
		German = 4,
		Spanish = 5,
		Portuguese = 6,
		Korean = 7,
		Japanese = 8,
		Chinese = 9,
		Russian = 10
	}

	private class LocalizedProvider : IFormatProvider
	{
		public object GetFormat(Type formatType)
		{
			throw new NotImplementedException();
		}
	}

	public delegate void AllDoneLoadNotify();

	public delegate void FileDoneLoad(string fileTxt);

	private static int MAX_LANGUAGE_VAL = 10;

	private static string[] m_supportedLanguages = new string[10] { "English", "French", "Italian", "German", "Spanish", "Portuguese", "Korean", "Japanese", "Chinese", "Russian" };

	private static AllDoneLoadNotify m_doneAllLoadingCallback = null;

	private static int m_filesLoaded = 0;

	private static int ALL_FILES_COUNT = 5;

	private static PerryLanguages m_language = PerryLanguages.English;

	private static Hashtable m_missionTable = null;

	private static Hashtable m_levelTable = null;

	private static Hashtable m_monogramTable = null;

	private static Hashtable m_storeItemsTable = null;

	private static Hashtable m_uiTable = null;

	private static bool didInit = false;

	public static PerryLanguages CurrentLanguageType
	{
		get
		{
			return m_language;
		}
	}

	public static bool isAsianLanguageActive
	{
		get
		{
			return m_language == PerryLanguages.Chinese || m_language == PerryLanguages.Japanese || m_language == PerryLanguages.Korean || m_language == PerryLanguages.Russian;
		}
	}

	public static string CurrentLanguageString
	{
		get
		{
			int num = (int)(m_language - 1);
			return m_supportedLanguages[num];
		}
	}

	public static void Init()
	{
		if (!didInit)
		{
			LoadDefaultLanguage();
			LoadAllLanguageFiles();
			didInit = true;
		}
	}

	public static void LoadAllLanguageFiles(AllDoneLoadNotify doneAllLoading = null)
	{
		m_doneAllLoadingCallback = doneAllLoading;
		m_filesLoaded = 0;
		LoadMissionFiles();
		LoadLevelFiles();
		LoadAllDialogueFiles();
		LoadStoreItemsFiles();
		LoadUIFiles();
	}

	private static void LoadDefaultLanguage()
	{
		if (PlayerData.AllTimeAppLaunches <= 1)
		{
			SetLanguageFromDevice();
			return;
		}
		PerryLanguages intItem = (PerryLanguages)PerryiCloudManager.The.GetIntItem("language", PerryiCloudManager.RetrieveType.Natural);
		ChangeLanguage(intItem);
	}

	public static void ChangeLanguage(PerryLanguages language)
	{
		if ((int)language > MAX_LANGUAGE_VAL)
		{
			language = PerryLanguages.English;
		}
		if (language <= (PerryLanguages)0)
		{
			SetLanguageFromDevice();
		}
		else
		{
			SetLanguageFromSavedFile(language);
		}
		PerryiCloudManager.The.SetItem("language", (int)m_language);
	}

	private static void SetLanguageFromDevice()
	{
		switch (Application.systemLanguage)
		{
		case SystemLanguage.Chinese:
			m_language = PerryLanguages.Chinese;
			break;
		case SystemLanguage.French:
			m_language = PerryLanguages.French;
			break;
		case SystemLanguage.German:
			m_language = PerryLanguages.German;
			break;
		case SystemLanguage.Italian:
			m_language = PerryLanguages.Italian;
			break;
		case SystemLanguage.Japanese:
			m_language = PerryLanguages.Japanese;
			break;
		case SystemLanguage.Korean:
			m_language = PerryLanguages.Korean;
			break;
		case SystemLanguage.Portuguese:
			m_language = PerryLanguages.Portuguese;
			break;
		case SystemLanguage.Russian:
			m_language = PerryLanguages.Russian;
			break;
		case SystemLanguage.Spanish:
			m_language = PerryLanguages.Spanish;
			break;
		default:
			m_language = PerryLanguages.English;
			break;
		}
		SetGlobalCultureInfoFromLanguage(m_language);
		PerryiCloudManager.The.SetItem("language", (int)m_language);
		GameManager.The.DoLanguageBillboards();
	}

	private static void SetLanguageFromSavedFile(PerryLanguages savedLanguage)
	{
		m_language = savedLanguage;
		SetGlobalCultureInfoFromLanguage(m_language);
		GameManager.The.DoLanguageBillboards();
	}

	private static void SetGlobalCultureInfoFromLanguage(PerryLanguages language)
	{
	}

	public static void LoadMissionFiles()
	{
		LoadFile("Missions.txt", DoneLoadMissions);
	}

	public static void LoadLevelFiles()
	{
		LoadFile("Levels.txt", DoneLoadLevels);
	}

	public static void LoadAllDialogueFiles()
	{
		LoadFile("Monogram.txt", DoneLoadMonogramFiles);
	}

	public static void LoadStoreItemsFiles()
	{
		LoadFile("StoreItems.txt", DoneLoadStoreItemsFiles);
	}

	public static void LoadUIFiles()
	{
		LoadFile("UI.txt", DoneLoadUIFiles);
	}

	private static void DoneLoadMissions(string fileText)
	{
		if (!string.IsNullOrEmpty(fileText))
		{
			FillHashTable(fileText, ref m_missionTable);
			AllMissionData.Init();
			AllMissionData.ReloadAllMissionText();
		}
		else
		{
			Debug.LogWarning("[LocalTextManager] Missions file empty or failed to load");
			if (m_missionTable == null) m_missionTable = new Hashtable();
		}
		IncrementAndCheckIfAllFilesLoaded();
	}

	private static void DoneLoadLevels(string fileText)
	{
		if (!string.IsNullOrEmpty(fileText))
		{
			FillHashTable(fileText, ref m_levelTable);
			AllLevelData.Init();
			AllLevelData.ReloadAllLevelText();
		}
		else
		{
			Debug.LogWarning("[LocalTextManager] Levels file empty or failed to load");
			if (m_levelTable == null) m_levelTable = new Hashtable();
		}
		IncrementAndCheckIfAllFilesLoaded();
	}

	private static void DoneLoadMonogramFiles(string fileText)
	{
		if (!string.IsNullOrEmpty(fileText))
		{
			FillHashTable(fileText, ref m_monogramTable);
		}
		else
		{
			Debug.LogWarning("[LocalTextManager] Monogram file empty or failed to load");
			if (m_monogramTable == null) m_monogramTable = new Hashtable();
		}
		IncrementAndCheckIfAllFilesLoaded();
	}

	private static void DoneLoadStoreItemsFiles(string fileText)
	{
		if (!string.IsNullOrEmpty(fileText))
		{
			FillHashTable(fileText, ref m_storeItemsTable);
			AllItemData.ReloadAllText();
		}
		else
		{
			Debug.LogWarning("[LocalTextManager] StoreItems file empty or failed to load");
			if (m_storeItemsTable == null) m_storeItemsTable = new Hashtable();
		}
		IncrementAndCheckIfAllFilesLoaded();
	}

	private static void DoneLoadUIFiles(string fileText)
	{
		if (!string.IsNullOrEmpty(fileText))
		{
			FillHashTable(fileText, ref m_uiTable);
		}
		else
		{
			Debug.LogWarning("[LocalTextManager] UI file empty or failed to load");
			if (m_uiTable == null) m_uiTable = new Hashtable();
		}
		IncrementAndCheckIfAllFilesLoaded();
	}

	public static string GetMissionText(string key)
	{
		if (m_missionTable == null || m_missionTable.Count <= 0)
		{
			return key;
		}
		if (!m_missionTable.ContainsKey(key))
		{
			return "key not found " + key;
		}
		return (string)m_missionTable[key];
	}

	public static string GetLevelText(string key)
	{
		if (m_levelTable == null || m_levelTable.Count <= 0)
		{
			DebugManager.Log("Trying to get levels before table is initialized");
			return key;
		}
		if (!m_levelTable.ContainsKey(key))
		{
			return "key not found " + key;
		}
		return (string)m_levelTable[key];
	}

	public static string GetMonogramText(string key)
	{
		if (m_monogramTable == null || m_monogramTable.Count <= 0)
		{
			return key;
		}
		if (!m_monogramTable.ContainsKey(key))
		{
			return "key not found " + key;
		}
		return (string)m_monogramTable[key];
	}

	public static string GetStoreItemText(string key)
	{
		if (m_storeItemsTable == null || m_storeItemsTable.Count <= 0)
		{
			return key;
		}
		if (!m_storeItemsTable.ContainsKey(key))
		{
			return "key not found " + key;
		}
		return (string)m_storeItemsTable[key];
	}

	public static string GetUIText(string key)
	{
		if (m_uiTable == null || m_uiTable.Count <= 0)
		{
			return key;
		}
		if (!m_uiTable.ContainsKey(key))
		{
			return "key not found " + key;
		}
		return (string)m_uiTable[key];
	}

	private static void LoadFile(string fileName, FileDoneLoad callback)
	{
		// Resources paths must be lowercase to match actual folder names on case-sensitive platforms.
		string filePath = "streamingassets/" + CurrentLanguageString.ToLower() + "/" + fileName.Replace(".txt", string.Empty);
		Debug.Log("[LocalTextManager] Loading file: " + filePath);
		GameManager.The.FileLoad(filePath, callback);
	}

	private static void FillHashTable(string fileText, ref Hashtable hashTable)
	{
		hashTable = new Hashtable();
		string text = null;
		string text2 = null;
		int num = 1;
		string[] array = fileText.Split('\n');
		string[] array2 = array;
		foreach (string text3 in array2)
		{
			if (num % 2 == 0)
			{
				text2 = text3.TrimEnd('\r').Replace("\\n", Environment.NewLine);
			}
			else
			{
				text = text3.TrimEnd('\r');
			}
			if (text2 != null && text != null)
			{
				hashTable.Add(text, text2);
				text2 = null;
				text = null;
			}
			num++;
		}
	}

	private static void IncrementAndCheckIfAllFilesLoaded()
	{
		m_filesLoaded++;
		if (m_filesLoaded >= ALL_FILES_COUNT)
		{
			GlobalGUIManager.The.OnLocalTextManagerComplete();
			if (m_doneAllLoadingCallback != null)
			{
				m_doneAllLoadingCallback();
			}
		}
	}
}

using System.Collections;
using UnityEngine;

// I think it's interesting that they named this "iCloud Manager" despite it having nothing to do with iCloud or 
// even cloud storage of any kind.

public class PerryiCloudManager : MonoBehaviour
{
	public enum RetrieveType
	{
		Natural = 0,
		Max = 1
	}

	private static string m_DataStoredWithDeviceUIDKey = "_DataStoredWithDeviceUniqueIDKey";

	private static PerryiCloudManager m_the;

	public static string DataStoredWithDeviceUIDKey
	{
		get
		{
			return m_DataStoredWithDeviceUIDKey;
		}
	}

	public static PerryiCloudManager The
	{
		get
		{
			if (m_the == null)
			{
				GameObject gameObject = new GameObject("PerryiCloudManager");
				m_the = (PerryiCloudManager)gameObject.AddComponent<PerryiCloudManager>();
			}
			return m_the;
		}
	}

	private void Awake()
	{
		if (m_the == null)
		{
			m_the = this;
			PlayerData.InitializeUseriCloudPreference();
		}
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	public bool HasKey(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	public bool GetBoolItemLocal(string key)
	{
		return PlayerPrefs.GetInt(key) == 1;
	}

	public void SetItemLocal(string key, bool val)
	{
		PlayerPrefs.SetInt(key, val ? 1 : 0);
	}

	public void SetItem(string key, bool val)
	{
		int value = 0;
		if (val)
		{
			value = 1;
		}
		PlayerPrefs.SetInt(key, value);
	}

	public void SetItem(string key, int val)
	{
		PlayerPrefs.SetInt(key, val);
	}

	public void SetItemInitialValue(string key, int val)
	{
	}

	public void SetItem(string key, float val)
	{
		PlayerPrefs.SetFloat(key, val);
	}

	public void SetItem(string key, string val)
	{
		PlayerPrefs.SetString(key, val);
	}

	public void SetItem(string key, PurchasableItem val)
	{
		SetItem(DataStoredWithDeviceUIDKey, SystemInfo.deviceUniqueIdentifier);
		SetItem(key + "_owned", val.m_owned);
		SetItem(key + "_token", val.m_tokenCost);
		SetItem(key + "_fedora", val.m_fedoraCost);
		SetItem(key + "_name", val.m_name);
		SetItem(key + "_desc", val.m_desc);
		SetItem(key + "_numOwned", val.m_numOwned);
	}

	public void SetItemInitialValues(string key, PurchasableItem val)
	{
		SetItem(key + "_numOwned_initialValue", val.m_numOwned);
	}

	public void SetGadgetItem(string key, PurchasableGadgetItem val)
	{
		SetItem(key, val);
		SetItem(key + "_maxUpgrades", val.m_maxUpgrades);
		SetItem(key + "_upgradeNums", val.UpgradeNums);
		SetItem(key + "_upgradeFedoraCost", val.m_upgradeFedoraCost);
		SetItem(key + "_hasBoughtGadget", val.hasBoughtGadget);
		SetItem(key + "_upgradeExtraText", val.m_upgradeExtraText);
	}

	public void SetGadgetItemInitialValues(string key, PurchasableGadgetItem val)
	{
		SetItemInitialValue(key + "_numOwned_initialValue", val.m_numOwned);
		SetItemInitialValue(key + "_upgradeNums_initialValue", val.UpgradeNums);
	}

	public void SetUpgradableItem(string key, UpgradableItem val)
	{
		SetItem(key, val);
		SetItem(key + "_maxUpgradeTimes", val.m_maxUpgradeTimes);
		SetItem(key + "_upgradeTokenMult", val.m_upgradeTokenMultiple);
		SetItem(key + "_upgradeFedoraMult", val.m_upgradeFedoraMultiple);
		SetItem(key + "_upgradesOwned", val.upgradesOwned);
		if (val.upgradeTitles != null)
		{
			for (int i = 0; i < val.upgradeTitles.Length; i++)
			{
				SetItem(key + "_upgradeTitle" + i, val.upgradeTitles[i]);
			}
		}
		if (val.upgradeDescs != null)
		{
			for (int j = 0; j < val.upgradeDescs.Length; j++)
			{
				SetItem(key + "_upgradeDesc" + j, val.upgradeDescs[j]);
			}
		}
	}

	public void SetUpgradableItemInitialValues(string key, UpgradableItem val)
	{
		SetItemInitialValue(key + "_numOwned_initialValue", val.m_numOwned);
		SetItemInitialValue(key + "_upgradesOwned_initialValue", val.upgradesOwned);
	}

	public void SetItem(string key, Mission val)
	{
		SetItem(key + "_name", val.m_name);
		SetItem(key + "_desc", val.m_desc);
		SetItem(key + "_levelVal", val.m_levelVal);
		SetItem(key + "_multiplier", val.m_multiplier);
		SetItem(key + "_scoreBonus", val.m_scoreBonus);
		SetItem(key + "_skipTokenCost", val.m_skipTokenCost);
		SetItem(key + "_completed", val.Completed);
	}

	public void SetItem(string key, Level val)
	{
		SetItem(key + "_title", val.m_title);
		SetItem(key + "_badgeVal", val.m_badgeVal);
		SetItem(key + "_multiplier", val.m_multiplier);
		SetItem(key + "_isComplete", val.m_isComplete);
	}

	public bool GetBoolItem(string key, RetrieveType retrieveType = RetrieveType.Natural)
	{
		bool result = false;
		int num = PlayerPrefs.GetInt(key);
		if (num == 1)
		{
			result = true;
		}
		return result;
	}

	public int GetIntItem(string key, RetrieveType retrieveType)
	{
		return PlayerPrefs.GetInt(key);
	}

	public float GetFloatItem(string key)
	{
		return PlayerPrefs.GetFloat(key);
	}

	public string GetStringItem(string key)
	{
		string text = PlayerPrefs.GetString(key);
		if (text == null)
		{
			return null;
		}
		if (text == string.Empty || text.Length <= 0)
		{
			return null;
		}
		return text;
	}

	public void RemoveItem(string key)
	{
	}

	public void RemoveItemInitialValues(string key)
	{
	}

	public void RemoveItemInitialValues(string key, PurchasableItem val)
	{
		RemoveItem(key + "_numOwned_initialValue");
	}

	public void RemoveItemInitialValues(string key, PurchasableGadgetItem val)
	{
		RemoveItem(key + "_numOwned_initialValue");
		RemoveItem(key + "_upgradeNums_initialValue");
	}

	public void RemoveItemInitialValues(string key, UpgradableItem val)
	{
		RemoveItem(key + "_numOwned_initialValue");
		RemoveItem(key + "_upgradesOwned_initialValue");
	}

	public PurchasableItem CreatePurchasableItem(string key)
	{
		PurchasableItem purchasableItem = new PurchasableItem(key);
		purchasableItem.m_name = GetStringItem(key + "_name");
		if (purchasableItem.m_name == null)
		{
			return null;
		}
		UpdatePurchasableItemDataFromPrefs(purchasableItem);
		Debug.Log("CreatePurchasableItem: is name null? = " + (purchasableItem.m_name == null));
		Debug.Log("GetPurchasableItem: name length = " + purchasableItem.m_name.Length);
		return purchasableItem;
	}

	public void UpdatePurchasableItemDataFromPrefs(PurchasableItem pi)
	{
		if (pi != null)
		{
			string uID = pi.UID;
			string stringItem = GetStringItem(uID + "_name");
			if (stringItem != null)
			{
				pi.m_name = stringItem;
				pi.m_desc = GetStringItem(uID + "_desc");
				pi.m_owned = GetBoolItem(uID + "_owned", RetrieveType.Max);
				pi.m_numOwned = GetIntItem(uID + "_numOwned", RetrieveType.Max);
				pi.m_tokenCost = GetIntItem(uID + "_token", RetrieveType.Natural);
				pi.m_fedoraCost = GetIntItem(uID + "_fedora", RetrieveType.Natural);
			}
		}
	}

	public PurchasableGadgetItem CreatePurchasableGadgetItem(string key)
	{
		PurchasableGadgetItem purchasableGadgetItem = new PurchasableGadgetItem(key);
		purchasableGadgetItem.m_name = GetStringItem(key + "_name");
		if (purchasableGadgetItem.m_name == null)
		{
			return null;
		}
		UpdatePurchasableGadgetItemDataFromPrefs(purchasableGadgetItem);
		return purchasableGadgetItem;
	}

	public void UpdatePurchasableGadgetItemDataFromPrefs(PurchasableGadgetItem pgi)
	{
		if (pgi != null)
		{
			string uID = pgi.UID;
			string stringItem = GetStringItem(uID + "_name");
			if (stringItem != null)
			{
				pgi.m_name = stringItem;
				pgi.m_desc = GetStringItem(uID + "_desc");
				pgi.m_tokenCost = GetIntItem(uID + "_token", RetrieveType.Natural);
				pgi.m_fedoraCost = GetIntItem(uID + "_fedora", RetrieveType.Natural);
				pgi.m_numOwned = GetIntItem(uID + "_numOwned", RetrieveType.Max);
				pgi.m_owned = GetBoolItem(uID + "_owned", RetrieveType.Max);
				pgi.UpgradeNums = GetIntItem(uID + "_upgradeNums", RetrieveType.Max);
				pgi.m_maxUpgrades = GetIntItem(uID + "_maxUpgrades", RetrieveType.Natural);
				pgi.m_upgradeFedoraCost = GetIntItem(uID + "_upgradeFedoraCost", RetrieveType.Natural);
				pgi.m_upgradeExtraText = GetStringItem(uID + "_upgradeExtraText");
				pgi.hasBoughtGadget = GetBoolItem(uID + "_hasBoughtGadget", RetrieveType.Max);
			}
		}
	}

	public UpgradableItem CreateUpgradableItem(string key)
	{
		UpgradableItem upgradableItem = new UpgradableItem(key);
		upgradableItem.m_name = GetStringItem(key + "_name");
		if (upgradableItem.m_name == null)
		{
			return null;
		}
		UpdateUpgradableItemDataFromPrefs(upgradableItem);
		return upgradableItem;
	}

	public void UpdateUpgradableItemDataFromPrefs(UpgradableItem ui)
	{
		if (ui == null)
		{
			return;
		}
		string uID = ui.UID;
		string stringItem = GetStringItem(uID + "_name");
		if (stringItem == null)
		{
			return;
		}
		ui.m_name = stringItem;
		ui.m_desc = GetStringItem(uID + "_desc");
		ui.m_tokenCost = GetIntItem(uID + "_token", RetrieveType.Natural);
		ui.m_fedoraCost = GetIntItem(uID + "_fedora", RetrieveType.Natural);
		ui.m_numOwned = GetIntItem(uID + "_numOwned", RetrieveType.Natural);
		ui.m_owned = GetBoolItem(uID + "_owned", RetrieveType.Max);
		ui.m_maxUpgradeTimes = GetIntItem(uID + "_maxUpgradeTimes", RetrieveType.Natural);
		ui.m_upgradeTokenMultiple = GetFloatItem(uID + "_upgradeTokenMult");
		ui.m_upgradeFedoraMultiple = GetFloatItem(uID + "_upgradeFedoraMult");
		ui.upgradesOwned = GetIntItem(uID + "_upgradesOwned", RetrieveType.Max);
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		for (int i = 0; i < ui.m_maxUpgradeTimes; i++)
		{
			string stringItem2 = GetStringItem(uID + "_upgradeTitle" + i);
			if (stringItem2 != null && stringItem2 != string.Empty)
			{
				arrayList.Add(stringItem2);
			}
			string stringItem3 = GetStringItem(uID + "_upgradeDesc" + i);
			if (stringItem3 != null && stringItem3 != string.Empty)
			{
				arrayList2.Add(stringItem3);
			}
		}
		if (arrayList.Count > 0)
		{
			string[] upgradeTitles = (string[])arrayList.ToArray(typeof(string));
			ui.SetUpgradeTitles(upgradeTitles);
		}
		if (arrayList2.Count > 0)
		{
			string[] upgradeDescriptions = (string[])arrayList2.ToArray(typeof(string));
			ui.SetUpgradeDescriptions(upgradeDescriptions);
		}
	}

	public Mission CreateMission(string key)
	{
		Mission mission = new Mission(key);
		mission.m_name = GetStringItem(key + "_name");
		if (mission.m_name == null)
		{
			return null;
		}
		mission.m_desc = GetStringItem(key + "_desc");
		mission.m_levelVal = GetIntItem(key + "_levelVal", RetrieveType.Natural);
		mission.m_multiplier = GetIntItem(key + "_multiplier", RetrieveType.Natural);
		mission.m_scoreBonus = GetIntItem(key + "_scoreBonus", RetrieveType.Natural);
		mission.m_skipTokenCost = GetIntItem(key + "_skipTokenCost", RetrieveType.Natural);
		mission.Completed = GetBoolItem(key + "_completed", RetrieveType.Max);
		return mission;
	}

	public Level GetLevel(string key)
	{
		Level level = new Level(key);
		level.m_title = GetStringItem(key + "_title");
		if (level.m_title == null)
		{
			return null;
		}
		level.m_isComplete = GetBoolItem(key + "_isComplete", RetrieveType.Max);
		return level;
	}

	public void ClearAll()
	{
		PlayerPrefs.DeleteAll();
	}

	public void Synchronize()
	{
		Debug.Log("Synchronize");
	}
}

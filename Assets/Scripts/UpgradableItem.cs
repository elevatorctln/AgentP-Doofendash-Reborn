using UnityEngine;

public class UpgradableItem : PurchasableItem
{
	public int m_maxUpgradeTimes;

	public float m_upgradeTokenMultiple = 2f;

	public float m_upgradeFedoraMultiple;

	private int m_upgradesOwned;

	private string[] m_upgradeTitles;

	private string[] m_upgradeDescs;

	private int[] m_upgradeTokenPrices;

	private int[] m_upgradeFedoraPrices;

	private float[] m_upgradeMoneyPrices;

	public int upgradesOwned
	{
		get
		{
			return m_upgradesOwned;
		}
		set
		{
			m_upgradesOwned = value;
			if (m_upgradesOwned >= m_maxUpgradeTimes)
			{
				m_owned = true;
			}
			else
			{
				m_owned = false;
			}
			UpdateCost();
			UpdateTitle();
			UpdateDescription();
		}
	}

	public string[] upgradeTitles
	{
		get
		{
			return m_upgradeTitles;
		}
	}

	public string[] upgradeDescs
	{
		get
		{
			return m_upgradeDescs;
		}
	}

	public UpgradableItem(string userID)
		: base(userID)
	{
	}

	public UpgradableItem(string name, string desc, bool owned, int tokenCost, int fedoraCost, float realMoneyCost, int upgradeTimes, int upgradeTokenMultiple, int upgradeFedoraMultiple, int upgradesOwned, string fileName, string userID)
		: base(name, desc, owned, tokenCost, fedoraCost, realMoneyCost, false, 0, 0, fileName, userID)
	{
		m_maxUpgradeTimes = upgradeTimes;
		m_upgradeTokenMultiple = upgradeTokenMultiple;
		m_upgradeFedoraMultiple = upgradeFedoraMultiple;
		m_upgradesOwned = upgradesOwned;
	}

	public void SetUpgradeTitles(string[] titles)
	{
		if (m_maxUpgradeTimes <= 0)
		{
			Debug.Log("ERROR! cannot set upgrade titles for an object with no upgrade times!");
			return;
		}
		int num = titles.Length;
		m_upgradeTitles = new string[num];
		for (int i = 0; i < titles.Length; i++)
		{
			m_upgradeTitles[i] = titles[i];
		}
		UpdateTitle();
	}

	public void SetUpgradeCosts(int[] tokenPrices, int[] fedoraPrices, float[] moneyPrices)
	{
		if (m_maxUpgradeTimes <= 0)
		{
			Debug.Log("ERROR! cannot set upgrade prices for an object with no upgrade times!");
			return;
		}
		if (tokenPrices != null)
		{
			int num = tokenPrices.Length;
			m_upgradeTokenPrices = new int[num];
			for (int i = 0; i < tokenPrices.Length; i++)
			{
				m_upgradeTokenPrices[i] = tokenPrices[i];
			}
		}
		if (fedoraPrices != null)
		{
			int num2 = fedoraPrices.Length;
			m_upgradeFedoraPrices = new int[num2];
			for (int j = 0; j < fedoraPrices.Length; j++)
			{
				m_upgradeFedoraPrices[j] = fedoraPrices[j];
			}
		}
		if (moneyPrices != null)
		{
			int num3 = moneyPrices.Length;
			m_upgradeMoneyPrices = new float[num3];
			for (int k = 0; k < moneyPrices.Length; k++)
			{
				m_upgradeMoneyPrices[k] = moneyPrices[k];
			}
		}
		UpdateCost();
	}

	public void SetUpgradeDescriptions(string[] descs)
	{
		if (m_maxUpgradeTimes <= 0)
		{
			Debug.Log("ERROR! cannot set upgrade descriptions for an object with no upgrade times!");
			return;
		}
		int num = descs.Length;
		m_upgradeDescs = new string[num];
		for (int i = 0; i < descs.Length; i++)
		{
			m_upgradeDescs[i] = descs[i];
		}
		UpdateDescription();
	}

	private void UpdateCost()
	{
		if (m_upgradeTokenPrices != null)
		{
			m_tokenCost = ((m_upgradesOwned < m_upgradeTokenPrices.Length) ? m_upgradeTokenPrices[m_upgradesOwned] : m_upgradeTokenPrices[m_upgradeTokenPrices.Length - 1]);
		}
		if (m_upgradeFedoraPrices != null)
		{
			m_fedoraCost = ((m_upgradesOwned < m_upgradeFedoraPrices.Length) ? m_upgradeFedoraPrices[m_upgradesOwned] : m_upgradeFedoraPrices[m_upgradeFedoraPrices.Length - 1]);
		}
		if (m_upgradeMoneyPrices != null)
		{
			m_realMoneyCost = ((m_upgradesOwned < m_upgradeMoneyPrices.Length) ? m_upgradeMoneyPrices[m_upgradesOwned] : m_upgradeMoneyPrices[m_upgradeTokenPrices.Length - 1]);
		}
	}

	public int CalcCumulativeTokenCost(int start, int ownedCount)
	{
		int num = 0;
		if (m_upgradeTokenPrices != null)
		{
			for (int i = start; i < ownedCount && i < m_upgradeTokenPrices.Length; i++)
			{
				num += m_upgradeTokenPrices[i];
			}
		}
		return num;
	}

	public int CalcCumulativeFedoraCost(int start, int ownedCount)
	{
		int num = 0;
		if (m_upgradeFedoraPrices != null)
		{
			for (int i = start; i < ownedCount && i < m_upgradeFedoraPrices.Length; i++)
			{
				num += m_upgradeFedoraPrices[i];
			}
		}
		return num;
	}

	private void UpdateTitle()
	{
		if (m_upgradeTitles != null && m_upgradeTitles.Length > 0 && m_upgradesOwned > 0)
		{
			int num = m_upgradesOwned;
			if (num > m_upgradeTitles.Length)
			{
				num = m_upgradeTitles.Length;
			}
			m_name = m_upgradeTitles[num - 1];
		}
	}

	private void UpdateDescription()
	{
		if (m_upgradeDescs != null && m_upgradeDescs.Length > 0 && m_upgradesOwned > 0)
		{
			int num = m_upgradesOwned;
			if (num > m_upgradeDescs.Length)
			{
				num = m_upgradeDescs.Length;
			}
			m_desc = m_upgradeDescs[num - 1];
		}
	}

	public override string ToString()
	{
		return string.Format("Upgradeable Item\nName: " + m_name + "\nDesc: " + m_desc + "\nToken Cost: " + m_tokenCost + "\nFedora Cost: " + m_fedoraCost + "\nOwned: " + m_owned + "\nUpgrade Times: " + m_maxUpgradeTimes + "\nUpgrades Owned: " + m_upgradesOwned + "\nUpgrade Token Multiple: " + m_upgradeTokenMultiple + "\nUpgrade Fedora Multiple: " + m_upgradeFedoraMultiple + "\nUniqueID: " + m_UID);
	}
}

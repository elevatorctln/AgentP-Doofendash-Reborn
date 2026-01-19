public class PurchasableGadgetItem : PurchasableItem
{
	public int m_maxUpgrades = 12;

	private int m_upgradeNums;

	private bool m_hasBoughtGadget;

	public int m_upgradeFedoraCost;

	public float m_damageUpgrade = 0.2f;

	public string m_upgradeExtraText;

	public bool hasBoughtGadget
	{
		get
		{
			return m_hasBoughtGadget;
		}
		set
		{
			m_hasBoughtGadget = value;
			UpdateBuyButtonExtraText();
			UpdatePrice();
			if (m_maxUpgrades <= 0)
			{
				m_owned = true;
			}
		}
	}

	public int UpgradeNums
	{
		get
		{
			return m_upgradeNums;
		}
		set
		{
			m_upgradeNums = value;
			if (m_upgradeNums >= m_maxUpgrades)
			{
				m_upgradeNums = m_maxUpgrades;
				m_owned = true;
			}
			else
			{
				m_owned = false;
			}
			UpdateBuyButtonExtraText();
			UpdatePrice();
		}
	}

	public PurchasableGadgetItem(string userID)
		: base(userID)
	{
	}

	public PurchasableGadgetItem(string name, string desc, bool owned, int tokenCost, int fedoraCost, float realMoneyCost, int upgradeNums, float damageUpdgrade, int upgradeFedoraCost, string upgradeExtraText, string fileName, string userID)
		: base(name, desc, owned, tokenCost, fedoraCost, realMoneyCost, false, 0, 0, fileName, userID)
	{
		m_upgradeNums = upgradeNums;
		m_damageUpgrade = damageUpdgrade;
		m_upgradeFedoraCost = upgradeFedoraCost;
		m_upgradeExtraText = upgradeExtraText;
		m_isBuyOnce = false;
	}

	public override string ToString()
	{
		return string.Format("Purchasable Gadget Item\nName: " + m_name + "\nDesc: " + m_desc + "\nToken Cost: " + m_tokenCost + "\nFedora Cost: " + m_fedoraCost + "\nOwned: " + m_owned + "\nUpgrade Nums: " + m_upgradeNums + "\nDamage Upgrade: " + m_damageUpgrade + "\nUpgrad Fedora Cost: " + m_upgradeFedoraCost + "\nUniqueID: " + m_UID);
	}

	private void UpdateBuyButtonExtraText()
	{
		if (m_hasBoughtGadget && m_upgradeExtraText != null)
		{
			m_buyExtraText = m_upgradeExtraText;
		}
	}

	private void UpdatePrice()
	{
		if (m_hasBoughtGadget)
		{
			m_tokenCost = 0;
			m_fedoraCost = m_upgradeFedoraCost;
		}
	}
}

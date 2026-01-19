public class PurchasableItem
{
	public delegate void HandlePurchase(object item = null);

	public object m_userData;

	public bool m_owned;

	public int m_tokenCost;

	public int m_fedoraCost;

	public float m_realMoneyCost;

	public bool m_isBuyOnce = true;

	public string m_name = "sampleName";

	public string m_desc = "sample description";

	public bool m_gotFromStore;

	public bool m_isConsumable;

	public int m_tokenReward;

	public int m_fedoraReward;

	public int m_numOwned;

	public string m_iconFileName = "defaultFileName.png";

	public string m_nameLocKey = "sampleKey";

	public string m_descLocKey = "sampleKey";

	protected string m_UID = "sampleUID";

	public string m_buyExtraText;

	private HandlePurchase m_immediatePurchaseEffectCallback;

	private HandlePurchase m_inGamePurchaseEffectCallback;

	public string UID
	{
		get
		{
			return m_UID;
		}
	}

	public HandlePurchase ImmediatePurchaseEffectCallback
	{
		set
		{
			m_immediatePurchaseEffectCallback = value;
		}
	}

	public HandlePurchase InGamePurchaseEffectCallback
	{
		set
		{
			m_inGamePurchaseEffectCallback = value;
		}
	}

	public PurchasableItem(string userID)
	{
		m_UID = userID;
	}

	public PurchasableItem(string name, string desc, bool owned, int tokenCost, int fedoraCost, float realMoneyCost, bool isBuyOnce, int tokenReward, int fedoraReward, string iconFileName, string userID)
	{
		m_name = name;
		m_desc = desc;
		m_owned = owned;
		m_tokenCost = tokenCost;
		m_fedoraCost = fedoraCost;
		m_realMoneyCost = realMoneyCost;
		m_isBuyOnce = isBuyOnce;
		m_tokenReward = tokenReward;
		m_fedoraReward = fedoraReward;
		m_iconFileName = iconFileName;
		m_UID = userID;
	}

	public override string ToString()
	{
		return string.Format("Purchasable Item\nName: " + m_name + "\nDesc: " + m_desc + "\nToken Cost: " + m_tokenCost + "\nFedora Cost: " + m_fedoraCost + "\nReal Money Cost: " + m_realMoneyCost + "\nOwned: " + m_owned + "\nIsBuyOnce" + m_isBuyOnce + "\nUniqueID: " + m_UID);
	}

	public void Purchase(int numPurchased)
	{
		if (m_isBuyOnce)
		{
			m_owned = true;
		}
		else
		{
			m_numOwned += numPurchased;
		}
		ApplyImmediatePurchaseEffect();
	}

	public void ApplyImmediatePurchaseEffect()
	{
		if (m_immediatePurchaseEffectCallback != null)
		{
			m_immediatePurchaseEffectCallback(this);
		}
	}

	public void ApplyInGamePurchaseEffect()
	{
		if (m_inGamePurchaseEffectCallback != null)
		{
			m_inGamePurchaseEffectCallback(this);
		}
	}
}

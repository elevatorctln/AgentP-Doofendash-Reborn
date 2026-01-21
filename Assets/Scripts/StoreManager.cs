using System;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
	private class BuyItemWaiting : MonoBehaviour
	{
		private static List<BuyItemWaiting> m_buyItemWaitingList;

		public Action<bool> buyItemCallback;

		public float buyItemTimeout;

		public IAPProduct buyProductItem;

		public BuyItemWaiting(Action<bool> callback, float timeout, IAPProduct product)
		{
			if (m_buyItemWaitingList == null)
			{
				DebugManager.Log("Initializing waitinglist");
				m_buyItemWaitingList = new List<BuyItemWaiting>();
			}
			buyItemCallback = callback;
			buyItemTimeout = timeout;
			buyProductItem = product;
			DebugManager.Log("Adding this to waitinglist");
			m_buyItemWaitingList.Add(this);
		}

		public static bool IsItemAlreadyOnList(IAPProduct item)
		{
			if (m_buyItemWaitingList == null)
			{
				return false;
			}
			for (int i = 0; i < m_buyItemWaitingList.Count; i++)
			{
				if (m_buyItemWaitingList[i].buyProductItem.productId == item.productId)
				{
					return true;
				}
			}
			return false;
		}

		public static void ClearAll()
		{
			m_buyItemWaitingList.Clear();
		}

		public static BuyItemWaiting GetFirst()
		{
			if (m_buyItemWaitingList != null && m_buyItemWaitingList.Count > 0)
			{
				return m_buyItemWaitingList[0];
			}
			return null;
		}

		public void BuySuccess(IAPProduct product)
		{
			if (buyItemCallback != null)
			{
				buyItemCallback(true);
			}
			m_buyItemWaitingList.Remove(this);
		}

		public void BuyFail()
		{
			if (buyItemCallback != null)
			{
				buyItemCallback(false);
			}
			m_buyItemWaitingList.Remove(this);
		}
	}

	public delegate void BuyItemCallback(bool success);

	public delegate void GetItemCallback(bool success);

	private const float GET_ITEM_TIMEOUT = 30f;

	private const float BUY_ITEM_TIMEOUT = 30f;

	private GetItemCallback m_currentGetItemCallback;

	private bool m_isWaitingForProducts;

	private BuyItemCallback m_currentBuyItemCallback;

	private Action<bool> m_lastBuyItemCallback;

	private bool m_isWaitingForPurchase;

	private bool m_recievedAllProducts;

	private static string[] m_iosProductIdentifiers = new string[31]
	{
		"Majesco.Doofendash.Currency_Starter_Pack", "Majesco.Doofendash.Currency_Value_Pack", "Majesco.Doofendash.Currency_Bucket_of_Tokens", "Majesco.Doofendash.Currency_Bucket_of_Fedoras", "Majesco.Doofendash.Currency_Fishbowl_Full_of_Tokens", "Majesco.Doofendash.Currency_Fishbowl_Full_of_Fedoras", "Majesco.Doofendash.Currency_Laundry_Bag_of_Tokens", "Majesco.Doofendash.Currency_Laundry_Bag_of_Fedoras", "Majesco.Doofendash.Currency_Wheelbarrow_of_Tokens1", "Majesco.Doofendash.Currency_Wheelbarrow_of_Fedoras",
		"Majesco.Doofendash.Currency_Whalebelly_Full_of_Tokens", "Majesco.Doofendash.Currency_Whalebelly_Full_of_Fedoras", "Majesco.Doofendash.Currency_Giant_Vault_of_Tokens", "Majesco.Doofendash.Currency_Giant_Vault_of_Fedoras", "Majesco.Doofendash.Upgrade_Coin_Duplicatorinator", "Majesco.Doofendash.Consumable_Double_Token_After_Run", "Majesco.Doofendash.Currency_Fishbowl_Full_of_Tokensx40", "Majesco.Doofendash.Currency_Laundry_Bag_of_Tokensx40", "Majesco.Doofendash.Currency_Wheelbarrow_of_Tokens1x40", "Majesco.Doofendash.Currency_Whalebelly_Full_of_Tokensx40",
		"Majesco.Doofendash.Currency_Wheelbarrow_of_Tokens1x70", "Majesco.Doofendash.Currency_Whalebelly_Full_of_Tokensx70", "Majesco.Doofendash.Currency_Bucket_of_Fedorasx40", "Majesco.Doofendash.Currency_Fishbowl_Full_of_Fedorasx40", "Majesco.Doofendash.Currency_Laundry_Bag_of_Fedorasx40", "Majesco.Doofendash.Currency_Wheelbarrow_of_Fedorasx40", "Majesco.Doofendash.Currency_Whalebelly_Full_of_Fedorasx40", "Majesco.Doofendash.Currency_Fishbowl_Full_of_Fedorasx70", "Majesco.Doofendash.Currency_Laundry_Bag_of_Fedorasx70", "Majesco.Doofendash.Currency_Wheelbarrow_of_Fedorasx70",
		"Majesco.Doofendash.Currency_Whalebelly_Full_of_Fedorasx70"
	};

	private static string[] m_androidProductIdentifiers = new string[31]
	{
		"majesco.doofendash.consumable_double_token_after_run", "majesco.doofendash.currency_bucket_of_fedoras", "majesco.doofendash.currency_bucket_of_tokens", "majesco.doofendash.currency_fishbowl_full_of_fedoras", "majesco.doofendash.currency_fishbowl_full_of_tokens", "majesco.doofendash.currency_giant_vault_of_fedoras", "majesco.doofendash.currency_giant_vault_of_tokens", "majesco.doofendash.currency_laundry_bag_of_fedoras", "majesco.doofendash.currency_laundry_bag_of_tokens", "majesco.doofendash.currency_starter_pack",
		"majesco.doofendash.currency_value_pack", "majesco.doofendash.currency_whalebelly_full_of_fedoras", "majesco.doofendash.currency_whalebelly_full_of_tokens", "majesco.doofendash.currency_wheelbarrow_of_fedoras", "majesco.doofendash.currency_wheelbarrow_of_tokens1", "majesco.doofendash.upgrade_coin_duplicatorinator", "majesco.doofendash.currency_fishbowl_full_of_tokensx40", "majesco.doofendash.currency_laundry_bag_of_tokensx40", "majesco.doofendash.currency_wheelbarrow_of_tokens1x40", "majesco.doofendash.currency_whalebelly_full_of_tokensx40",
		"majesco.doofendash.currency_wheelbarrow_of_tokensx70", "majesco.doofendash.currency_whalebelly_full_of_tokensx70", "majesco.doofendash.currency_bucket_of_fedorasx40", "majesco.doofendash.currency_fishbowl_full_of_fedorasx40", "majesco.doofendash.currency_laundry_bag_of_fedorasx40", "majesco.doofendash.currency_wheelbarrow_of_fedorasx40", "majesco.doofendash.currency_whalebelly_full_of_fedorasx40", "majesco.doofendash.currency_fishbowl_full_of_fedorasx70", "majesco.doofendash.currency_laundry_bag_of_fedorasx70", "majesco.doofendash.currency_wheelbarrow_of_fedorasx70",
		"majesco.doofendash.currency_whalebelly_full_of_fedorasx70"
	};

	private static string[] m_amazonProductIdentifiers = new string[16]
	{
		"Majesco.Doofendash.Currency_Starter_Pack", "Majesco.Doofendash.Currency_Value_Pack", "Majesco.Doofendash.Currency_Bucket_of_Tokens", "Majesco.Doofendash.Currency_Bucket_of_Fedoras", "Majesco.Doofendash.Currency_Fishbowl_Full_of_Tokens", "Majesco.Doofendash.Currency_Fishbowl_Full_of_Fedoras", "Majesco.Doofendash.Currency_Laundry_Bag_of_Tokens", "Majesco.Doofendash.Currency_Laundry_Bag_of_Fedoras", "Majesco.Doofendash.Currency_Wheelbarrow_of_Tokens1", "Majesco.Doofendash.Currency_Wheelbarrow_of_Fedoras",
		"Majesco.Doofendash.Currency_Whalebelly_Full_of_Tokens", "Majesco.Doofendash.Currency_Whalebelly_Full_of_Fedoras", "Majesco.Doofendash.Currency_Giant_Vault_of_Tokens", "Majesco.Doofendash.Currency_Giant_Vault_of_Fedoras", "Majesco.Doofendash.Upgrade_Coin_Duplicatorinator", "Majesco.Doofendash.Consumable_Double_Token_After_Run"
	};

	private List<IAPProduct> m_products;

	private static StoreManager m_the;

	public bool isWaitingForProducts
	{
		get
		{
			return m_isWaitingForProducts;
		}
	}

	public bool isWaitingForPurchase
	{
		get
		{
			return m_isWaitingForPurchase;
		}
	}

	public bool recievedAllProducts
	{
		get
		{
			return m_recievedAllProducts;
		}
	}

	public static StoreManager The
	{
		get
		{
			if (m_the == null)
			{
				GameObject gameObject = new GameObject("StoreManager");
				gameObject.AddComponent<StoreManager>();
			}
			return m_the;
		}
	}

	public int GetNumberOfIAPItems()
	{
		if (m_products != null)
		{
			return m_products.Count;
		}
		return 0;
	}

	private void Awake()
	{
		if (m_the == null)
		{
			m_the = this;
		}
		else
		{
			DebugManager.Log("Attempting to create a new StoreManager but THERE CAN BE ONLY ONE!");
		}
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Start()
	{
		Init();
	}

	public void Init()
	{
		string androidPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAnOm2eRzAz8SauLfYYVHujHdPN55hIgHgY2a3dhvxHKOAiL4tGvya/JPDNuWqCFFYMOxvxF9NbUkoGJWkZzr1dphK3cDEFN8tdZPwTjTHhdeSYyzq/rmLs6H5pQEthPt7F9q/8zV5u37IvhO/ShuzYRAGnH8Sr2KbrAZtmpaJ4K1D/ZSOOkbFmsj/4a3pRa86JuTrRj/1Lhm1OOTMbHaf7DFDnjpRxr9HmezVtziQWh+RUuJypoRA16JtGfDI12HJSFz8BgwUCxYndDAlRonttA6wqWLP7NPiJjuUSKJyRbJLDAHjULPRBGuoeels8qynms8MUwnNSdkdfZim/mySvwIDAQAB";
	}

	private void SetupOnce()
	{
	}

	public void GetProducts(GetItemCallback callback = null)
	{
		DebugManager.Log("Attempting to download all products");
		m_currentGetItemCallback = callback;
		m_isWaitingForProducts = true;
		DebugManager.Log("Returned from IAP.requestProductData, in StoreManager.GetProducts()");
		Invoke("GetProductTimeout", 30f);
	}

	private void GetProductTimeout()
	{
		DebugManager.Log("Entering StoreManager.GetProductTimeout()");
		m_isWaitingForProducts = false;
		if (m_currentGetItemCallback != null)
		{
			m_currentGetItemCallback(false);
			m_currentGetItemCallback = null;
		}
	}

	public void RestoreCompletedTransactions(bool fromSettings)
	{
	}

	private void RecievedAllProducts(List<IAPProduct> prods)
	{
		DebugManager.Log("Entering StoreManager.RecievedAllProducts()");
		if (prods == null)
		{
			DebugManager.Log("StoreManager: Recieved Products? false.  Exiting StoreManager.RecievedAllProducts()");
			return;
		}
		DebugManager.Log("StoreManager: Recieved Products? true");
		m_products = prods;
		bool success = false;
		if (m_products != null && m_products.Count > 0)
		{
			DebugManager.Log("recievedallprod bool true");
			m_recievedAllProducts = true;
			DebugManager.Log("About to call to AllItemData.SetAllStoreItems()");
			AllItemData.SetAllStoreItems();
			DebugManager.Log("Just finished call to AllItemData.SetAllStoreItems()");
			success = true;
		}
		CancelInvoke("GetProductTimeout");
		if (m_currentGetItemCallback != null)
		{
			DebugManager.Log("m_currentGetItemCallback != null");
			m_currentGetItemCallback(success);
		}
		else
		{
			DebugManager.Log("m_currentGetItemCallback == null");
		}
		m_isWaitingForProducts = false;
		m_currentGetItemCallback = null;
		DebugManager.Log("Exiting StoreManager.RecievedAllProducts()");
	}

	public IAPProduct GetProduct(string productID)
	{
		if (!m_recievedAllProducts)
		{
			DebugManager.Log("Didn't download products! Exiting out of GetProduct!");
			return null;
		}
		DebugManager.Log("m_products == null?: " + (m_products == null));
		for (int i = 0; i < m_products.Count; i++)
		{
			DebugManager.Log("Product Lookup: " + m_products[i].productId + " | " + productID);
			if (m_products[i].productId == productID)
			{
				return m_products[i];
			}
		}
		DebugManager.Log("Couldn't get the product: " + productID);
		return null;
	}

	public void BuyProduct(int index, bool isConsumable, BuyItemCallback callback = null)
	{
		if (m_recievedAllProducts && index < m_products.Count)
		{
			IAPProduct product = m_products[index];
			BuyProduct(product, isConsumable, callback);
		}
	}

	public void BuyProduct(string productID, bool isConsumable, BuyItemCallback callback = null)
	{
		IAPProduct product = GetProduct(productID);
		DebugManager.Log("Got Product: " + product.productId);
		BuyProduct(product, isConsumable, callback);
	}

	public void BuyProduct(string productID, bool isConsumable, Action<bool> buyItemCallback, bool dummy)
	{
		IAPProduct product = GetProduct(productID);
		if (product != null)
		{
			DebugManager.Log("Got Product: " + product.productId);
			BuyProduct(product, isConsumable, buyItemCallback, dummy);
		}
	}

	private void BuyProduct(IAPProduct product, bool isConsumable, Action<bool> buyItemCallback, bool dummy)
	{
		
	}

	private void BuyProduct(IAPProduct product, bool isConsumable, BuyItemCallback callback = null)
	{
		if (product.productId == null)
		{
			return;
		}
		DebugManager.Log("Buying Product: " + product.productId + ", isConsumable: " + isConsumable);
		m_isWaitingForPurchase = true;
		m_currentBuyItemCallback = callback;
		Invoke("BuyItemTimeout", 30f);
	}
		

	private void BuyItemTimeoutForAction()
	{
		BuyItemWaiting first = BuyItemWaiting.GetFirst();
		if (first != null)
		{
			first.BuyFail();
		}
	}

	private void BuyItemTimeout()
	{
		m_isWaitingForPurchase = false;
		m_currentBuyItemCallback(false);
		m_currentBuyItemCallback = null;
	}

	private void HandleBoughtProduct(IAPProduct product)
	{
		DebugManager.Log("Bought " + product.productId);
		AllItemData.FinishBuyingStoreProduct(product.productId);
	}

	private void purchaseCancelled(string error)
	{
	}
}

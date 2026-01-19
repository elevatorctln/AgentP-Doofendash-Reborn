using System;
using System.Collections.Generic;
using PlayHaven;
using UnityEngine;

public static class IAP
{
	private const string CONSUMABLE_PAYLOAD = "consume";

	private const string NON_CONSUMABLE_PAYLOAD = "nonconsume";

	private static Action<List<IAPProduct>> _productListReceivedAction;

	private static Action<bool> _purchaseCompletionAction;

	private static string _purchasingProductID;

	private static Action<string> _purchaseRestorationAction;

	static IAP()
	{
		_purchasingProductID = string.Empty;
		GoogleIABManager.queryInventorySucceededEvent += delegate(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
		{
			Debug.Log("queryInventorySucceededEvent " + purchases.Count + " skus " + skus.Count);
			List<IAPProduct> list = new List<IAPProduct>();
			foreach (GoogleSkuInfo sku in skus)
			{
				list.Add(new IAPProduct(sku));
			}
			foreach (GoogleSkuInfo sku2 in skus)
			{
				Debug.Log("prod " + sku2.title + " price " + sku2.price);
			}
			if (_productListReceivedAction != null)
			{
				_productListReceivedAction(list);
			}
		};
		GoogleIABManager.queryInventoryFailedEvent += delegate(string error)
		{
			Debug.Log("fetching prouduct data failed: " + error);
			if (_productListReceivedAction != null)
			{
				_productListReceivedAction(null);
			}
		};
		GoogleIABManager.purchaseSucceededEvent += delegate(GooglePurchase purchase)
		{
			if (purchase.developerPayload == "nonconsume")
			{
				if (_purchaseCompletionAction != null)
				{
					_purchaseCompletionAction(true);
				}
			}
			else
			{
				GoogleIAB.consumeProduct(purchase.productId);
			}
			PlayHavenManager.instance.ContentRequest("iap_purchased");
		};
		GoogleIABManager.purchaseFailedEvent += delegate(string error)
		{
			Debug.Log("purchase failed: " + error);
			if (_purchaseCompletionAction != null)
			{
				_purchaseCompletionAction(false);
			}
		};
		GoogleIABManager.consumePurchaseSucceededEvent += delegate
		{
			if (_purchaseCompletionAction != null)
			{
				_purchaseCompletionAction(true);
			}
		};
		GoogleIABManager.consumePurchaseFailedEvent += delegate
		{
			if (_purchaseCompletionAction != null)
			{
				_purchaseCompletionAction(false);
			}
		};
	}

	public static void init(string androidPublicKey)
	{
		GoogleIAB.init(androidPublicKey);
	}

	public static void requestProductData(string[] iosProductIdentifiers, string[] androidSkus, string[] amazonSkus, Action<List<IAPProduct>> completionHandler)
	{
		Debug.Log("Entering IAP.requestProductData()");
		_productListReceivedAction = completionHandler;
		Debug.Log("Request Product Data: Android");
		GoogleIAB.queryInventory(androidSkus);
	}

	public static void purchaseConsumableProduct(string productId, Action<bool> completionHandler)
	{
		_purchaseCompletionAction = completionHandler;
		_purchasingProductID = productId;
		GoogleIAB.purchaseProduct(productId, "consume");
	}

	public static void purchaseNonconsumableProduct(string productId, Action<bool> completionHandler)
	{
		_purchaseCompletionAction = completionHandler;
		_purchasingProductID = productId;
		GoogleIAB.purchaseProduct(productId, "nonconsume");
	}

	public static void restoreCompletedTransactions(Action<string> completionHandler)
	{
		_purchaseCompletionAction = null;
		_purchaseRestorationAction = completionHandler;
	}
}

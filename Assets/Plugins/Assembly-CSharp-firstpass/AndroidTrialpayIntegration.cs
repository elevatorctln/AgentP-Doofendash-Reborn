using System;
using UnityEngine;

public class AndroidTrialpayIntegration : ITrialpayIntegration
{
	private AndroidJavaClass trialpayManager;

	private bool initWithVic;

	private bool enabledBalanceCheck;

	public AndroidTrialpayIntegration()
	{
		trialpayManager = new AndroidJavaClass("com.trialpay.android.unity.TrialpayManager");
	}

	public string getSdkVersion()
	{
		return trialpayManager.CallStatic<string>("staticGetSdkVersion", new object[0]);
	}

	public void setVerbose(bool verbosity)
	{
	}

	public void setSid(string sid)
	{
		trialpayManager.CallStatic("staticSetSid", sid);
	}

	public void registerVic(string touchpointName, string vic)
	{
		trialpayManager.CallStatic("staticRegisterVic", touchpointName, vic);
		initWithVic = true;
	}

	public void registerUnitySendMessage(string objectName, string methodName)
	{
		trialpayManager.CallStatic("staticRegisterUnitySendMessage", objectName, methodName);
	}

	public void open(string touchpointName)
	{
		if (initWithVic)
		{
			trialpayManager.CallStatic("staticOpen", touchpointName);
			return;
		}
		throw new Exception("vic is not set in the Trialpay integration");
	}

	public void open(string touchpointName, TrialpayViewMode mode)
	{
		if (initWithVic)
		{
			trialpayManager.CallStatic("staticOpen", touchpointName, (int)mode);
			return;
		}
		throw new Exception("vic is not set in the Trialpay integration");
	}

	public void initiateBalanceChecks()
	{
		if (initWithVic)
		{
			trialpayManager.CallStatic("staticInitiateBalanceChecks");
			enabledBalanceCheck = true;
			return;
		}
		throw new Exception("VIC is not set in the Trialpay integration");
	}

	public void enableBalanceCheck()
	{
		initiateBalanceChecks();
	}

	public int withdrawBalance(string touchpointName)
	{
		if (!enabledBalanceCheck)
		{
			enableBalanceCheck();
		}
		return trialpayManager.CallStatic<int>("staticWithdrawBalance", new object[1] { touchpointName });
	}

	public void startAvailabilityCheck(string touchpointName)
	{
		if (initWithVic)
		{
			trialpayManager.CallStatic("staticStartAvailabilityCheck", touchpointName);
			return;
		}
		throw new Exception("No VIC is set in the Trialpay integration");
	}

	public bool isAvailable(string touchpointName)
	{
		if (initWithVic)
		{
			return trialpayManager.CallStatic<bool>("staticIsAvailable", new object[1] { touchpointName });
		}
		throw new Exception("No VIC is set in the Trialpay integration");
	}

	public void openForTouchpoint(string touchpointName)
	{
		open(touchpointName);
	}

	public void openOfferwallForTouchpoint(string touchpointName)
	{
		open(touchpointName);
	}

	public void openOfferwall(string vic, string sid)
	{
		setSid(sid);
		registerVic(vic, vic);
		openOfferwallForTouchpoint(vic);
	}

	public void setAge(int age)
	{
		trialpayManager.CallStatic("staticSetAge", age);
	}

	public void setGender(char gender)
	{
		trialpayManager.CallStatic("staticSetGender", gender);
	}

	public void updateLevel(int level)
	{
		trialpayManager.CallStatic("staticUpdateLevel", level);
	}

	public void setCustomParam(string paramName, string paramValue)
	{
		trialpayManager.CallStatic("staticSetCustomParam", paramName, paramValue);
	}

	public void clearCustomParam(string paramName)
	{
		trialpayManager.CallStatic("staticClearCustomParam", paramName);
	}

	public void updateVcPurchaseInfo(string touchpointName, float dollarAmount, int vcAmount)
	{
		trialpayManager.CallStatic("staticUpdateVcPurchaseInfo", touchpointName, dollarAmount, vcAmount);
	}

	public void updateVcBalance(string touchpointName, int vcAmount)
	{
		trialpayManager.CallStatic("staticUpdateVcBalance", touchpointName, vcAmount);
	}
}

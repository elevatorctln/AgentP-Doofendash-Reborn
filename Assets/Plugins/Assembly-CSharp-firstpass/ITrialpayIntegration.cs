public interface ITrialpayIntegration
{
	string getSdkVersion();

	void setVerbose(bool verbose);

	void setSid(string sid);

	void registerVic(string touchpointName, string vic);

	void registerUnitySendMessage(string objectName, string methodName);

	void open(string touchpointName);

	void open(string touchpointName, TrialpayViewMode mode);

	void initiateBalanceChecks();

	int withdrawBalance(string touchpointName);

	void startAvailabilityCheck(string touchpointName);

	bool isAvailable(string touchpointName);

	void setAge(int age);

	void setGender(char gender);

	void updateLevel(int level);

	void setCustomParam(string paramName, string paramValue);

	void clearCustomParam(string paramName);

	void updateVcPurchaseInfo(string touchpointName, float dollarAmount, int vcAmount);

	void updateVcBalance(string touchpointName, int vcAmount);

	void openOfferwall(string vic, string sid);

	void openOfferwallForTouchpoint(string touchpointName);

	void enableBalanceCheck();
}

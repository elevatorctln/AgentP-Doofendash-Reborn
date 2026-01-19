public class GPGQuestMilestone
{
	public string questMilestoneId;

	public string questId;

	public int state;

	public long currentCount;

	public long targetCount;

	public string rewardData;

	public GPGQuestMilestoneState stateEnum
	{
		get
		{
			return (GPGQuestMilestoneState)state;
		}
	}
}

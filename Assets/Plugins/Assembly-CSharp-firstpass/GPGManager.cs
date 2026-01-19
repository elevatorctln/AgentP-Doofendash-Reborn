using System;
using System.Collections.Generic;
using Prime31;
using UnityEngine;

public class GPGManager : AbstractManager
{
	public static event Action<string> authenticationSucceededEvent;

	public static event Action<string> authenticationFailedEvent;

	public static event Action userSignedOutEvent;

	public static event Action<string> reloadDataForKeyFailedEvent;

	public static event Action<string> reloadDataForKeySucceededEvent;

	public static event Action licenseCheckFailedEvent;

	public static event Action<string> profileImageLoadedAtPathEvent;

	public static event Action<string> finishedSharingEvent;

	public static event Action<GPGPlayerInfo, string> loadPlayerCompletedEvent;

	public static event Action<string> loadCloudDataForKeyFailedEvent;

	public static event Action<int, string> loadCloudDataForKeySucceededEvent;

	public static event Action<string> updateCloudDataForKeyFailedEvent;

	public static event Action<int, string> updateCloudDataForKeySucceededEvent;

	public static event Action<string> clearCloudDataForKeyFailedEvent;

	public static event Action<string> clearCloudDataForKeySucceededEvent;

	public static event Action<string> deleteCloudDataForKeyFailedEvent;

	public static event Action<string> deleteCloudDataForKeySucceededEvent;

	public static event Action<string, string> unlockAchievementFailedEvent;

	public static event Action<string, bool> unlockAchievementSucceededEvent;

	public static event Action<string, string> incrementAchievementFailedEvent;

	public static event Action<string, bool> incrementAchievementSucceededEvent;

	public static event Action<string, string> revealAchievementFailedEvent;

	public static event Action<string> revealAchievementSucceededEvent;

	public static event Action<string, string> submitScoreFailedEvent;

	public static event Action<string, Dictionary<string, object>> submitScoreSucceededEvent;

	public static event Action<string, string> loadScoresFailedEvent;

	public static event Action<List<GPGScore>> loadScoresSucceededEvent;

	public static event Action<GPGScore> loadCurrentPlayerLeaderboardScoreSucceededEvent;

	public static event Action<string, string> loadCurrentPlayerLeaderboardScoreFailedEvent;

	public static event Action<List<GPGEvent>> allEventsLoadedEvent;

	public static event Action<GPGQuest> questListLauncherAcceptedQuestEvent;

	public static event Action<GPGQuestMilestone> questClaimedRewardsForQuestMilestoneEvent;

	public static event Action<GPGQuest> questCompletedEvent;

	static GPGManager()
	{
		AbstractManager.initialize(typeof(GPGManager));
	}

	private void fireEventWithIdentifierAndError(Action<string, string> theEvent, string json)
	{
		if (theEvent != null)
		{
			Dictionary<string, object> dictionary = json.dictionaryFromJson();
			if (dictionary != null && dictionary.ContainsKey("identifier") && dictionary.ContainsKey("error"))
			{
				theEvent(dictionary["identifier"].ToString(), dictionary["error"].ToString());
			}
			else
			{
				Debug.LogError("json could not be deserialized to an identifier and an error: " + json);
			}
		}
	}

	private void fireEventWithIdentifierAndBool(Action<string, bool> theEvent, string param)
	{
		if (theEvent != null)
		{
			string[] array = param.Split(',');
			if (array.Length == 2)
			{
				theEvent(array[0], array[1] == "1");
			}
			else
			{
				Debug.LogError("param could not be deserialized to an identifier and an error: " + param);
			}
		}
	}

	private void userSignedOut(string empty)
	{
		GPGManager.userSignedOutEvent.fire();
	}

	private void reloadDataForKeyFailed(string error)
	{
		GPGManager.reloadDataForKeyFailedEvent.fire(error);
	}

	private void reloadDataForKeySucceeded(string param)
	{
		GPGManager.reloadDataForKeySucceededEvent.fire(param);
	}

	private void licenseCheckFailed(string param)
	{
		GPGManager.licenseCheckFailedEvent.fire();
	}

	private void profileImageLoadedAtPath(string path)
	{
		GPGManager.profileImageLoadedAtPathEvent.fire(path);
	}

	private void finishedSharing(string errorOrNull)
	{
		GPGManager.finishedSharingEvent.fire(errorOrNull);
	}

	private void loadPlayerCompleted(string playerOrError)
	{
		if (GPGManager.loadPlayerCompletedEvent != null)
		{
			if (playerOrError.StartsWith("{"))
			{
				GPGManager.loadPlayerCompletedEvent(Json.decode<GPGPlayerInfo>(playerOrError), null);
			}
			else
			{
				GPGManager.loadPlayerCompletedEvent(null, playerOrError);
			}
		}
	}

	private void loadCloudDataForKeyFailed(string error)
	{
		GPGManager.loadCloudDataForKeyFailedEvent.fire(error);
	}

	private void loadCloudDataForKeySucceeded(string json)
	{
		Dictionary<string, object> dictionary = json.dictionaryFromJson();
		GPGManager.loadCloudDataForKeySucceededEvent.fire(int.Parse(dictionary["key"].ToString()), dictionary["data"].ToString());
	}

	private void updateCloudDataForKeyFailed(string error)
	{
		GPGManager.updateCloudDataForKeyFailedEvent.fire(error);
	}

	private void updateCloudDataForKeySucceeded(string json)
	{
		Dictionary<string, object> dictionary = json.dictionaryFromJson();
		GPGManager.updateCloudDataForKeySucceededEvent.fire(int.Parse(dictionary["key"].ToString()), dictionary["data"].ToString());
	}

	private void clearCloudDataForKeyFailed(string error)
	{
		GPGManager.clearCloudDataForKeyFailedEvent.fire(error);
	}

	private void clearCloudDataForKeySucceeded(string param)
	{
		GPGManager.clearCloudDataForKeySucceededEvent.fire(param);
	}

	private void deleteCloudDataForKeyFailed(string error)
	{
		GPGManager.deleteCloudDataForKeyFailedEvent.fire(error);
	}

	private void deleteCloudDataForKeySucceeded(string param)
	{
		GPGManager.deleteCloudDataForKeySucceededEvent.fire(param);
	}

	private void unlockAchievementFailed(string json)
	{
		fireEventWithIdentifierAndError(GPGManager.unlockAchievementFailedEvent, json);
	}

	private void unlockAchievementSucceeded(string param)
	{
		fireEventWithIdentifierAndBool(GPGManager.unlockAchievementSucceededEvent, param);
	}

	private void incrementAchievementFailed(string json)
	{
		fireEventWithIdentifierAndError(GPGManager.incrementAchievementFailedEvent, json);
	}

	private void incrementAchievementSucceeded(string param)
	{
		string[] array = param.Split(',');
		if (array.Length == 2)
		{
			GPGManager.incrementAchievementSucceededEvent.fire(array[0], array[1] == "1");
		}
	}

	private void revealAchievementFailed(string json)
	{
		fireEventWithIdentifierAndError(GPGManager.revealAchievementFailedEvent, json);
	}

	private void revealAchievementSucceeded(string achievementId)
	{
		GPGManager.revealAchievementSucceededEvent.fire(achievementId);
	}

	private void submitScoreFailed(string json)
	{
		fireEventWithIdentifierAndError(GPGManager.submitScoreFailedEvent, json);
	}

	private void submitScoreSucceeded(string json)
	{
		if (GPGManager.submitScoreSucceededEvent != null)
		{
			Dictionary<string, object> dictionary = json.dictionaryFromJson();
			string arg = "Unknown";
			if (dictionary.ContainsKey("leaderboardId"))
			{
				arg = dictionary["leaderboardId"].ToString();
			}
			GPGManager.submitScoreSucceededEvent(arg, dictionary);
		}
	}

	private void loadScoresFailed(string json)
	{
		fireEventWithIdentifierAndError(GPGManager.loadScoresFailedEvent, json);
	}

	private void loadScoresSucceeded(string json)
	{
		if (GPGManager.loadScoresSucceededEvent != null)
		{
			GPGManager.loadScoresSucceededEvent(Json.decode<List<GPGScore>>(json));
		}
	}

	private void loadCurrentPlayerLeaderboardScoreSucceeded(string json)
	{
		if (GPGManager.loadCurrentPlayerLeaderboardScoreSucceededEvent != null)
		{
			GPGManager.loadCurrentPlayerLeaderboardScoreSucceededEvent(Json.decode<GPGScore>(json));
		}
	}

	private void loadCurrentPlayerLeaderboardScoreFailed(string json)
	{
		fireEventWithIdentifierAndError(GPGManager.loadCurrentPlayerLeaderboardScoreFailedEvent, json);
	}

	private void authenticationSucceeded(string param)
	{
		GPGManager.authenticationSucceededEvent.fire(param);
	}

	private void authenticationFailed(string error)
	{
		GPGManager.authenticationFailedEvent.fire(error);
	}

	private void allEventsLoaded(string json)
	{
		if (GPGManager.allEventsLoadedEvent != null)
		{
			GPGManager.allEventsLoadedEvent(Json.decode<List<GPGEvent>>(json));
		}
	}

	private void questListLauncherClaimedRewardsForQuestMilestone(string json)
	{
		if (GPGManager.questClaimedRewardsForQuestMilestoneEvent != null)
		{
			GPGManager.questClaimedRewardsForQuestMilestoneEvent(Json.decode<GPGQuestMilestone>(json));
		}
	}

	private void questCompleted(string json)
	{
		if (GPGManager.questCompletedEvent != null)
		{
			GPGManager.questCompletedEvent(Json.decode<GPGQuest>(json));
		}
	}

	private void questListLauncherAcceptedQuest(string json)
	{
		if (GPGManager.questListLauncherAcceptedQuestEvent != null)
		{
			GPGManager.questListLauncherAcceptedQuestEvent(Json.decode<GPGQuest>(json));
		}
	}
}

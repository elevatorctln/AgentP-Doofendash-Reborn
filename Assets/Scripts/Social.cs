using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;
using UnityEngine;
using WyrmTale;

public class Social : MonoBehaviour
{
	private const string fbAppId = "579749735410028";

	private static Social singleton;

	public bool showDebugUi = true;

	public Dictionary<string, object> fbMe;

	[HideInInspector]
	public bool isFbLogedIn;

	private List<string> fbPermissions;

	private JSON scoresJson;

	private int m_fbFriendsCount;

	private int m_fbFriendsLoadedCount;

	private bool m_loginBtnPressed;

	private SocialWindows socialWindows;

	public static Social Instance
	{
		get
		{
			return singleton;
		}
	}

	public string GetFBID()
	{
		return "579749735410028";
	}

	private void Awake()
	{
		
	}

	private void OnEnable()
	{
		return;
	}

	private void OnDisable()
	{
		return;
	}

	private void Start()
	{
		return;
	}

	public void FbLogin()
	{
		return;
	}

	public void FbBackgroundLogin()
	{
		return;
	}

	public void FbLoginWithPublishActions()
	{
		return;
	}

	public void FbLoginWithGameActions()
	{
		return;
	}

	public void FbSessionOpenedEvent()
	{
		return;
	}

	public void FbLoginFailedEvent(P31Error error)
	{
		return;
	}

	public void FbLogout()
	{
		return;
	}

	public void FbGraphRequest(string graphPath, HTTPVerb httpMethod)
	{
		return;
	}

	private void OnFbGraphRequest(string error, object result)
	{
		return;
	}

	public void GetFBScoresInBackground()
	{
		return;
	}

	public void FbGetMe()
	{
		return;
	}

	private void OnFbGetMe(string error, object result)
	{
		return;
	}

	public void FbPostScore(int score)
	{
		return;
	}

	private void OnFbPostScore(bool result)
	{
		return;
	}

	public void FbGetScores()
	{
		return;
	}

	public void FbGetMyScore()
	{
		return;
	}
	public void FbGetUser(string fbId)
	{
		return;
	}

	private void OnFbGetUser(string error, object result)
	{
		return;
	}

	public void IsDoneLoadingFriends()
	{
		return;
	}

	public void ShareItems(string[] items)
	{
		return;
	}
}

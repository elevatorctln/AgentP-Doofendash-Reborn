using UnityEngine;

public class ScriptedMovementCamera : PathStateMachine
{
	private void OnDestroy()
	{
		base.PathMessageEvents -= PathMessageCallback;
	}

	private void Start()
	{
		base.PathMessageEvents += PathMessageCallback;
		Init();
		SetDoofState(base.FollowBase);
		SetRotationState(BossRotationState.LookAtPerry);
	}

	private void Update()
	{
	}

	private void PathMessageCallback(PathMessages pathMessage)
	{
		switch (pathMessage)
		{
		case PathMessages.StartedPath:
			break;
		case PathMessages.FinishedPath:
			SetDoofState(base.FollowBase);
			SetRotationState(BossRotationState.LookAtPerryBase);
			break;
		case PathMessages.StartedRotation:
			break;
		case PathMessages.FinishedRotation:
			SetRotationState(BossRotationState.LookAtPerry);
			if (!IsOnPath())
			{
				StartOnPath("TestPath");
				SetRotationState(BossRotationState.FollowPathRotation);
			}
			break;
		}
	}

	protected override void HandlePathEvents(EventPoint eventPoint)
	{
		if (eventPoint.name.Contains("PauseHere"))
		{
			PauseCurrentPath();
		}
		if (eventPoint.name.Contains("Event_1"))
		{
			LookAtPerryTimedStart(1f);
		}
		Debug.Log("BossController PathEventHandler");
		base.HandlePathEvents(eventPoint);
	}
}

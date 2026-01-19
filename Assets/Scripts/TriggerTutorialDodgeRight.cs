using UnityEngine;

public class TriggerTutorialDodgeRight : Obstacle
{
	private void OnTriggerEnter(Collider trigger)
	{
		Runner.The().SetTutorialState(Runner.TutorialState.PauseWaitForSwipeRight);
		TutorialGUIManager.The.ShowSwipeAnim(0, TutorialGUIManager.TutorialSwipeDirection.RIGHT, true);
	}
}

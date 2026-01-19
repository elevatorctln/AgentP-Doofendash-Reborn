using UnityEngine;

public class TriggerTutorialJump : Obstacle
{
	private void OnTriggerEnter(Collider trigger)
	{
		Runner.The().SetTutorialState(Runner.TutorialState.PauseWaitForJump);
		TutorialGUIManager.The.ShowSwipeAnim(0, TutorialGUIManager.TutorialSwipeDirection.UP, true);
	}
}

using UnityEngine;

public class TriggerTutorialDodgeLeft : Obstacle
{
	private void OnTriggerEnter(Collider trigger)
	{
		Runner.The().SetTutorialState(Runner.TutorialState.PauseWaitForSwipeLeft);
		TutorialGUIManager.The.ShowSwipeAnim(0, TutorialGUIManager.TutorialSwipeDirection.LEFT, true);
	}
}

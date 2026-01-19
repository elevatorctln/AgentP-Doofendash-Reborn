using UnityEngine;

public class TriggerTutorialSlide : Obstacle
{
	private void OnTriggerEnter(Collider trigger)
	{
		Runner.The().SetTutorialState(Runner.TutorialState.PauseWaitForSlide);
		TutorialGUIManager.The.ShowSwipeAnim(0, TutorialGUIManager.TutorialSwipeDirection.DOWN, true);
	}
}

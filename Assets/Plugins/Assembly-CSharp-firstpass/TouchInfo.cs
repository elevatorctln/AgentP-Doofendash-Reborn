using UnityEngine;

public class TouchInfo
{
	public Vector2 startPoint;

	public float startTime;

	public SwipeDirection swipeDetectionState;

	public SwipeDirection completedSwipeDirection;

	public SwipeDirection swipesToDetect;

	public SwipeDetectionStatus swipeDetectionStatus;

	public TouchInfo(SwipeDirection swipesToDetect)
	{
		this.swipesToDetect = swipesToDetect;
		startPoint = Vector2.zero;
		startTime = 0f;
		swipeDetectionState = SwipeDirection.Horizontal;
		completedSwipeDirection = (SwipeDirection)0;
		swipeDetectionStatus = SwipeDetectionStatus.Waiting;
	}

	public void resetWithTouch(Touch touch)
	{
		swipeDetectionState = swipesToDetect;
		startPoint = touch.position;
		startTime = Time.time;
		swipeDetectionStatus = SwipeDetectionStatus.Waiting;
	}
}

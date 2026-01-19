using System;
using UnityEngine;

public class UISwipeDetector : UITouchableSprite
{
	public delegate void UISwipeDetectorDetectedSwipe(UISwipeDetector sender, SwipeDirection direction);

	private TouchInfo[] touchInfoArray;

	public float timeToSwipe = 0.5f;

	public float allowedVariance = 35f;

	public float minimumDistance = 40f;

	public SwipeDirection swipesToDetect = SwipeDirection.All;

	public float swipeVelocity;

	public event UISwipeDetectorDetectedSwipe onSwipe;

	public UISwipeDetector(Rect frame, int depth, UIUVRect uvFrame)
		: base(frame, depth, uvFrame)
	{
		touchInfoArray = new TouchInfo[12];
	}

	public static UISwipeDetector create(Rect frame, int depth)
	{
		return create(UI.firstToolkit, frame, depth);
	}

	public static UISwipeDetector create(UIToolkit manager, Rect frame, int depth)
	{
		UISwipeDetector uISwipeDetector = new UISwipeDetector(frame, depth, UIUVRect.zero);
		manager.addTouchableSprite(uISwipeDetector);
		return uISwipeDetector;
	}

	public static UISwipeDetector create(UIToolkit manager, string filename, int xPos, int yPos, int depth)
	{
		UITextureInfo uITextureInfo = manager.textureInfoForFilename(filename);
		Rect frame = new Rect(xPos, yPos, uITextureInfo.frame.width, uITextureInfo.frame.height);
		UISwipeDetector uISwipeDetector = new UISwipeDetector(frame, depth, uITextureInfo.uvRect);
		manager.addTouchableSprite(uISwipeDetector);
		return uISwipeDetector;
	}

	public override void onTouchBegan(Touch touch, Vector2 touchPos)
	{
		if (touchInfoArray[touch.fingerId] == null)
		{
			touchInfoArray[touch.fingerId] = new TouchInfo(swipesToDetect);
		}
		touchInfoArray[touch.fingerId].resetWithTouch(touch);
	}

	public override void onTouchMoved(Touch touch, Vector2 touchPos)
	{
		if (processTouchInfoWithTouch(touchInfoArray[touch.fingerId], touch))
		{
			if (this.onSwipe != null)
			{
				this.onSwipe(this, touchInfoArray[touch.fingerId].completedSwipeDirection);
			}
			touchInfoArray[touch.fingerId].swipeDetectionStatus = SwipeDetectionStatus.Done;
		}
	}

	private bool processTouchInfoWithTouch(TouchInfo touchInfo, Touch touch)
	{
		if (touchInfo.swipeDetectionStatus != SwipeDetectionStatus.Waiting)
		{
			return false;
		}
		if (timeToSwipe > 0f && Time.time - touchInfo.startTime > timeToSwipe)
		{
			touchInfo.swipeDetectionStatus = SwipeDetectionStatus.Failed;
			return false;
		}
		if (touch.deltaPosition.x > 0f)
		{
			touchInfo.swipeDetectionState &= ~SwipeDirection.Left;
		}
		if (touch.deltaPosition.x < 0f)
		{
			touchInfo.swipeDetectionState &= ~SwipeDirection.Right;
		}
		if (touch.deltaPosition.y < 0f)
		{
			touchInfo.swipeDetectionState &= ~SwipeDirection.Up;
		}
		if (touch.deltaPosition.y > 0f)
		{
			touchInfo.swipeDetectionState &= ~SwipeDirection.Down;
		}
		float num = Math.Abs(touchInfo.startPoint.x - touch.position.x);
		float num2 = Math.Abs(touchInfo.startPoint.y - touch.position.y);
		if ((touchInfo.swipeDetectionState & SwipeDirection.Left) != 0 && num > minimumDistance)
		{
			if (num2 < allowedVariance)
			{
				touchInfo.completedSwipeDirection = SwipeDirection.Left;
				swipeVelocity = num / timeToSwipe;
				return true;
			}
			touchInfo.swipeDetectionState &= ~SwipeDirection.Left;
		}
		if ((touchInfo.swipeDetectionState & SwipeDirection.Right) != 0 && num > minimumDistance)
		{
			if (num2 < allowedVariance)
			{
				touchInfo.completedSwipeDirection = SwipeDirection.Right;
				swipeVelocity = num / timeToSwipe;
				return true;
			}
			touchInfo.swipeDetectionState &= ~SwipeDirection.Right;
		}
		if ((touchInfo.swipeDetectionState & SwipeDirection.Up) != 0 && num2 > minimumDistance)
		{
			if (num < allowedVariance)
			{
				touchInfo.completedSwipeDirection = SwipeDirection.Up;
				swipeVelocity = num2 / timeToSwipe;
				return true;
			}
			touchInfo.swipeDetectionState &= ~SwipeDirection.Up;
		}
		if ((touchInfo.swipeDetectionState & SwipeDirection.Down) != 0 && num2 > minimumDistance)
		{
			if (num < allowedVariance)
			{
				touchInfo.completedSwipeDirection = SwipeDirection.Down;
				swipeVelocity = num2 / timeToSwipe;
				return true;
			}
			touchInfo.swipeDetectionState &= ~SwipeDirection.Down;
		}
		return false;
	}
}

using UnityEngine;

public class UIScrollableHorizontalLayout : UIAbstractTouchableContainer
{
	public UIScrollableHorizontalLayout(int spacing)
		: base(UILayoutType.Horizontal, spacing)
	{
	}

	protected override void clipChild(UISprite child)
	{
		if (child.hidden && child.hiddenOutsideOfLayoutContainer)
		{
			return;
		}
		bool flag = child.position.x >= touchFrame.xMin && child.position.x <= touchFrame.xMax;
		bool flag2 = child.position.x + child.width >= touchFrame.xMin && child.position.x + child.width <= touchFrame.xMax;
		if (flag && flag2)
		{
			if (child.clipped)
			{
				child.clipped = false;
			}
			if (!child.hiddenOutsideOfLayoutContainer)
			{
				child.manager.showSprite(child);
			}
		}
		else if (flag || flag2)
		{
			child.beginUpdates();
			if (!child.hiddenOutsideOfLayoutContainer)
			{
				child.manager.showSprite(child);
			}
			if (flag)
			{
				float num = touchFrame.xMax - child.position.x;
				child.uvFrameClipped = child.uvFrame.rectClippedToBounds(num / child.scale.x, child.height / child.scale.y, UIClippingPlane.Right, child.manager.textureSize);
				child.setClippedSize(num / child.scale.x, child.height / child.scale.y, UIClippingPlane.Right);
			}
			else
			{
				float num2 = child.width + child.position.x - touchFrame.xMin;
				child.uvFrameClipped = child.uvFrame.rectClippedToBounds(num2 / child.scale.x, child.height / child.scale.y, UIClippingPlane.Left, child.manager.textureSize);
				child.setClippedSize(num2 / child.scale.x, child.height / child.scale.y, UIClippingPlane.Left);
			}
			child.endUpdates();
		}
		else
		{
			child.manager.hideSprite(child);
			child.hiddenOutsideOfLayoutContainer = false;
		}
		recurseAndClipChildren(child);
	}

	public override void onTouchMoved(Touch touch, Vector2 touchPos)
	{
		_deltaTouch += touch.deltaPosition.x;
		_lastTouch = touch;
		if (_activeTouchable != null && Mathf.Abs(_deltaTouch) > TOUCH_MAX_DELTA_FOR_ACTIVATION)
		{
			_activeTouchable.onTouchEnded(touch, touchPos, true);
			_activeTouchable = null;
		}
		float num = _scrollPosition + touch.deltaPosition.x;
		_isDraggingPastExtents = num > 0f || num < _minEdgeInset.x;
		if (_isDraggingPastExtents)
		{
			float num2 = 0f;
			num2 = ((!(num > 0f)) ? Mathf.Abs(_contentWidth + num - width) : num);
			float p = num2 / width;
			num = _scrollPosition + touch.deltaPosition.x * Mathf.Pow(0.04f, p);
		}
		_scrollPosition = num;
		layoutChildren();
		if (_velocities.Count == 3)
		{
			_velocities.Dequeue();
		}
		_velocities.Enqueue(touch.deltaPosition.x / Time.deltaTime);
	}
}

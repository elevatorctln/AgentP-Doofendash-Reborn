using UnityEngine;

public class UIScrollableVerticalLayoutNoClip : UIAbstractTouchableContainer
{
	private bool m_clipTop;

	private bool m_clipBot;

	public UIScrollableVerticalLayoutNoClip(int spacing, bool clipTop = false, bool clipBot = false)
		: base(UILayoutType.Vertical, spacing)
	{
		m_clipTop = clipTop;
		m_clipBot = clipBot;
	}

	protected override void clipChild(UISprite child)
	{
		if (child.hidden)
		{
			return;
		}
		bool flag = child.position.y < 0f - touchFrame.yMin && child.position.y > 0f - touchFrame.yMax;
		bool flag2 = child.position.y - child.height < 0f - touchFrame.yMin && child.position.y - child.height > 0f - touchFrame.yMax;
		if (flag && flag2)
		{
			if (child.clipped)
			{
				child.clipped = false;
			}
			child.hidden = false;
		}
		else if (flag || flag2)
		{
			child.beginUpdates();
			child.hidden = false;
			if (flag && m_clipBot)
			{
				float num = child.position.y + touchFrame.yMax;
				child.uvFrameClipped = child.uvFrame.rectClippedToBounds(child.width / child.scale.x, num / child.scale.y, UIClippingPlane.Bottom, child.manager.textureSize);
				child.setClippedSize(child.width / child.scale.x, num / child.scale.y, UIClippingPlane.Bottom);
			}
			else if (flag2 && m_clipTop)
			{
				float num2 = child.height - child.position.y - touchFrame.yMin;
				child.uvFrameClipped = child.uvFrame.rectClippedToBounds(child.width / child.scale.x, num2 / child.scale.y, UIClippingPlane.Top, child.manager.textureSize);
				child.setClippedSize(child.width / child.scale.x, num2 / child.scale.y, UIClippingPlane.Top);
			}
			child.endUpdates();
		}
		else if (!flag2 && m_clipTop)
		{
			child.hidden = true;
		}
		else if (!flag && m_clipBot)
		{
			child.hidden = true;
		}
		recurseAndClipChildren(child);
	}

	public override void onTouchMoved(Touch touch, Vector2 touchPos)
	{
		_deltaTouch += touch.deltaPosition.y;
		_lastTouch = touch;
		if (_activeTouchable != null && Mathf.Abs(_deltaTouch) > TOUCH_MAX_DELTA_FOR_ACTIVATION)
		{
			_activeTouchable.onTouchEnded(touch, touchPos, true);
			_activeTouchable = null;
		}
		float num = _scrollPosition - touch.deltaPosition.y;
		_isDraggingPastExtents = num > 0f || num < _minEdgeInset.y;
		if (_isDraggingPastExtents)
		{
			float num2 = 0f;
			num2 = ((!(num > 0f)) ? Mathf.Abs(_contentHeight + num - height) : num);
			float p = num2 / height;
			num = _scrollPosition - touch.deltaPosition.y * Mathf.Pow(0.04f, p);
		}
		_scrollPosition = num;
		layoutChildren();
		if (_velocities.Count == 3)
		{
			_velocities.Dequeue();
		}
		_velocities.Enqueue(touch.deltaPosition.y / Time.deltaTime);
	}
}

using System;
using UnityEngine;

public class UIScrollableVerticalLayout : UIAbstractTouchableContainer
{
	public delegate void UIScrollChanged(UIScrollableVerticalLayout sender, float value);

	public delegate void UIScrollStartedMove(UIScrollableVerticalLayout sender);

	public bool m_disableTouch;

	public event UIScrollChanged onScrollChange;

	public event UIScrollStartedMove onScrollStartedMoveEvent;

	public event Action<UIScrollableVerticalLayout> onTouchEndedAction;

	public UIScrollableVerticalLayout(int spacing)
		: base(UILayoutType.Vertical, spacing)
	{
	}

	private void Update()
	{
	}

	public void refreshClip()
	{
		foreach (UISprite child in _children)
		{
			clipChild(child);
		}
	}

	protected override void clipToBounds()
	{
		if (!hidden)
		{
			base.clipToBounds();
		}
	}

	public override void onTouchEnded(Touch touch, Vector2 touchPos, bool touchWasInsideTouchFrame)
	{
		if (m_disableTouch)
		{
			m_disableTouch = false;
			return;
		}
		base.onTouchEnded(touch, touchPos, touchWasInsideTouchFrame);
		if (this.onTouchEndedAction != null)
		{
			this.onTouchEndedAction(this);
		}
	}

	protected override void clipChild(UISprite child)
	{
		if (child.hidden && child.hiddenOutsideOfLayoutContainer)
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
				float num = child.position.y + touchFrame.yMax;
				child.uvFrameClipped = child.uvFrame.rectClippedToBounds(child.width / child.scale.x, num / child.scale.y, UIClippingPlane.Bottom, child.manager.textureSize);
				child.setClippedSize(child.width / child.scale.x, num / child.scale.y, UIClippingPlane.Bottom);
			}
			else
			{
				float num2 = child.height - child.position.y - touchFrame.yMin;
				child.uvFrameClipped = child.uvFrame.rectClippedToBounds(child.width / child.scale.x, num2 / child.scale.y, UIClippingPlane.Top, child.manager.textureSize);
				child.setClippedSize(child.width / child.scale.x, num2 / child.scale.y, UIClippingPlane.Top);
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
		if (!m_disableTouch)
		{
			float num = 1f;
			if (Screen.dpi > 0f)
			{
				num *= Screen.dpi / 160f * 3f;
			}
			_deltaTouch += touch.deltaPosition.y * num;
			_lastTouch = touch;
			if (_activeTouchable != null && Mathf.Abs(_deltaTouch) > TOUCH_MAX_DELTA_FOR_ACTIVATION)
			{
				_activeTouchable.onTouchEnded(touch, touchPos, true);
				_activeTouchable = null;
			}
			float num2 = _scrollPosition - touch.deltaPosition.y * num;
			_isDraggingPastExtents = num2 > 0f || num2 < _minEdgeInset.y;
			if (_isDraggingPastExtents)
			{
				float num3 = 0f;
				num3 = ((!(num2 > 0f)) ? Mathf.Abs(_contentHeight + num2 - height) : num2);
				float p = num3 / height;
				num2 = _scrollPosition - touch.deltaPosition.y * num * Mathf.Pow(0.04f, p);
			}
			_scrollPosition = num2;
			layoutChildren();
			if (_velocities.Count == 3)
			{
				_velocities.Dequeue();
			}
			_velocities.Enqueue(touch.deltaPosition.y * num / Time.deltaTime);
			if (Mathf.Abs(_firstTouch.position.y - touch.position.y) * num > 2f && this.onScrollStartedMoveEvent != null)
			{
				this.onScrollStartedMoveEvent(this);
			}
		}
	}

	protected override void layoutChildren()
	{
		base.layoutChildren();
		if (this.onScrollChange != null)
		{
			this.onScrollChange(this, 0f - _scrollPosition);
		}
	}
}

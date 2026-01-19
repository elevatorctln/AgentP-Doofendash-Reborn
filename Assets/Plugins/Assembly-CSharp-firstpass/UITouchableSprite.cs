using System;
using UnityEngine;

public abstract class UITouchableSprite : UISprite, IComparable, ITouchable
{
	public int touchCount;

	public UIUVRect disabledUVframe;

	public UIUVRect hoveredUVframe;

	protected UIEdgeOffsets _normalTouchOffsets;

	protected UIEdgeOffsets _highlightedTouchOffsets;

	protected Rect _highlightedTouchFrame;

	protected Rect _normalTouchFrame;

	protected UIUVRect _tempUVframe;

	protected bool touchFrameIsDirty = true;

	protected bool _highlighted;

	protected bool _disabled;

	public UIEdgeOffsets highlightedTouchOffsets
	{
		get
		{
			return _highlightedTouchOffsets;
		}
		set
		{
			_highlightedTouchOffsets = value;
			touchFrameIsDirty = true;
		}
	}

	public UIEdgeOffsets normalTouchOffsets
	{
		get
		{
			return _normalTouchOffsets;
		}
		set
		{
			_normalTouchOffsets = value;
			touchFrameIsDirty = true;
		}
	}

	public Rect touchFrame
	{
		get
		{
			if (_disabled)
			{
				return UISprite._rectZero;
			}
			if (touchFrameIsDirty)
			{
				touchFrameIsDirty = false;
				Rect frame = new Rect(clientTransform.position.x, 0f - clientTransform.position.y, width, height);
				if (gameObjectOriginInCenter)
				{
					frame.x -= width / 2f;
					frame.y -= height / 2f;
				}
				_normalTouchFrame = addOffsetsAndClipToScreen(frame, _normalTouchOffsets);
				_highlightedTouchFrame = addOffsetsAndClipToScreen(frame, _highlightedTouchOffsets);
			}
			return (!_highlighted) ? _normalTouchFrame : _highlightedTouchFrame;
		}
	}

	public virtual bool highlighted
	{
		get
		{
			return _highlighted;
		}
		set
		{
			_highlighted = value;
		}
	}

	public override bool hidden
	{
		get
		{
			return ___hidden;
		}
		set
		{
			base.hidden = value;
			if (value)
			{
				touchFrameIsDirty = true;
			}
		}
	}

	public bool allowTouchBeganWhenMovedOver { get; set; }

	public virtual bool disabled
	{
		get
		{
			return _disabled;
		}
		set
		{
			if (_disabled != value)
			{
				_disabled = value;
				if (value && !disabledUVframe.Equals(UIUVRect.zero))
				{
					uvFrame = disabledUVframe;
				}
				else
				{
					uvFrame = _tempUVframe;
				}
			}
		}
	}

	public UITouchableSprite(Rect frame, int depth, UIUVRect uvFrame)
		: base(frame, depth, uvFrame)
	{
		_tempUVframe = uvFrame;
	}

	public UITouchableSprite(Rect frame, int depth, UIUVRect uvFrame, bool gameObjectOriginInCenter)
		: base(frame, depth, uvFrame, gameObjectOriginInCenter)
	{
	}

	private Rect addOffsetsAndClipToScreen(Rect frame, UIEdgeOffsets offsets)
	{
		return Rect.MinMaxRect(Mathf.Clamp(frame.x - (float)offsets.left, 0f, Screen.width), Mathf.Clamp(frame.y - (float)offsets.top, 0f, Screen.height), Mathf.Clamp(frame.x + frame.width + (float)offsets.right, 0f, Screen.width), Mathf.Clamp(frame.y + frame.height + (float)offsets.bottom, 0f, Screen.height));
	}

	public override void updateTransform()
	{
		base.updateTransform();
		touchFrameIsDirty = true;
	}

	public bool hitTest(Vector2 point)
	{
		return touchFrame.Contains(point);
	}

	protected Vector2 inverseTranformPoint(Vector2 point)
	{
		return new Vector2(point.x - _normalTouchFrame.xMin, point.y - _normalTouchFrame.yMin);
	}

	public override void centerize()
	{
		touchFrameIsDirty = true;
		base.centerize();
	}

	public virtual void onTouchBegan(Touch touch, Vector2 touchPos)
	{
		highlighted = true;
	}

	public virtual void onTouchMoved(Touch touch, Vector2 touchPos)
	{
	}

	public virtual void onTouchEnded(Touch touch, Vector2 touchPos, bool touchWasInsideTouchFrame)
	{
		highlighted = false;
	}

	public int CompareTo(object obj)
	{
		if (obj is ITouchable)
		{
			ITouchable touchable = obj as ITouchable;
			return position.z.CompareTo(touchable.position.z);
		}
		return -1;
	}
}

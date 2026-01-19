using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIAbstractTouchableContainer : UIAbstractContainer, IComparable, ITouchable
{
	protected const int TOTAL_VELOCITY_SAMPLE_COUNT = 3;

	protected const float SCROLL_DECELERATION_MODIFIER = 0.93f;

	protected const float CONTENT_TOUCH_DELAY = 0.1f;

	protected UIToolkit _manager;

	protected Vector2 _minEdgeInset;

	protected float TOUCH_MAX_DELTA_FOR_ACTIVATION = 5f;

	protected bool _isDragging;

	protected bool _isDraggingPastExtents;

	protected Queue<float> _velocities = new Queue<float>(3);

	protected Touch _firstTouch;

	protected float _deltaTouch;

	protected Touch _lastTouch;

	protected Vector2 _lastTouchPosition;

	protected ITouchable _activeTouchable;

	public bool pagingEnabled;

	public float pageWidth;

	protected Rect _touchFrame;

	public override float width
	{
		get
		{
			return _touchFrame.width;
		}
	}

	public override float height
	{
		get
		{
			return _touchFrame.height;
		}
	}

	public bool highlighted { get; set; }

	public Rect touchFrame
	{
		get
		{
			return _touchFrame;
		}
	}

	public bool allowTouchBeganWhenMovedOver { get; set; }

	public UIAbstractTouchableContainer(UILayoutType layoutType, int spacing)
		: this(UI.firstToolkit, layoutType, spacing)
	{
	}

	public UIAbstractTouchableContainer(UIToolkit manager, UILayoutType layoutType, int spacing)
		: base(layoutType)
	{
		TOUCH_MAX_DELTA_FOR_ACTIVATION *= UI.scaleFactor;
		_spacing = spacing * UI.scaleFactor;
		_manager = manager;
		_manager.addToTouchables(this);
	}

	protected IEnumerator springBackToBounds(float elasticityModifier)
	{
		float targetScrollPosition = 0f;
		if (_scrollPosition < 0f)
		{
			targetScrollPosition = ((base.layoutType != UILayoutType.Horizontal) ? _minEdgeInset.y : _minEdgeInset.x);
		}
		while (!_isDragging)
		{
			float distanceFromTarget = _scrollPosition - targetScrollPosition;
			float divisor = ((base.layoutType != UILayoutType.Horizontal) ? height : width);
			if (distanceFromTarget > divisor)
			{
				distanceFromTarget = divisor;
			}
			else if (distanceFromTarget < 0f - divisor)
			{
				distanceFromTarget = 0f - divisor;
			}
			float percentFromSource = distanceFromTarget / divisor;
			float factor = Mathf.Abs(Mathf.Pow(elasticityModifier, percentFromSource * percentFromSource) - 0.9f);
			float snapBack = distanceFromTarget * factor;
			_scrollPosition -= snapBack;
			layoutChildren();
			if (Mathf.Abs(snapBack) < 0.2f)
			{
				_scrollPosition = targetScrollPosition;
				break;
			}
			yield return null;
		}
		layoutChildren();
		_isDraggingPastExtents = false;
	}

	protected virtual IEnumerator decelerate()
	{
		if (_isDraggingPastExtents)
		{
			yield return _manager.StartCoroutine(springBackToBounds(2f));
		}
		else
		{
			if (_velocities.Count <= 0)
			{
				yield break;
			}
			float total = 0f;
			foreach (float velocity in _velocities)
			{
				float v = velocity;
				total += v;
			}
			float avgVelocity = total / (float)_velocities.Count;
			float elasticDecelerationModifier = 0.7f;
			while (!_isDragging)
			{
				float deltaMovement = avgVelocity * Time.deltaTime;
				float newOffset = _scrollPosition;
				newOffset = ((base.layoutType != UILayoutType.Horizontal) ? (newOffset - deltaMovement) : (newOffset + deltaMovement));
				float absVelocity = Mathf.Abs(avgVelocity);
				if (pagingEnabled && absVelocity < 2500f)
				{
					if (!_isDraggingPastExtents)
					{
						scrollToNearestPage();
					}
					break;
				}
				if (absVelocity < 25f)
				{
					break;
				}
				float lowerBounds = ((base.layoutType != UILayoutType.Horizontal) ? _minEdgeInset.y : _minEdgeInset.x);
				if (newOffset < 0f && newOffset > lowerBounds)
				{
					_scrollPosition = newOffset;
					layoutChildren();
					avgVelocity *= 0.93f;
					yield return null;
					continue;
				}
				_isDraggingPastExtents = true;
				_scrollPosition = newOffset;
				layoutChildren();
				avgVelocity *= elasticDecelerationModifier;
				elasticDecelerationModifier -= 0.1f;
				if (elasticDecelerationModifier <= 0f)
				{
					break;
				}
				yield return null;
			}
			if (_isDraggingPastExtents)
			{
				yield return _manager.StartCoroutine(springBackToBounds(0.9f));
			}
		}
	}

	private void scrollToNearestPage()
	{
		int page = Mathf.RoundToInt(Math.Abs(_scrollPosition) / (pageWidth + (float)base.spacing));
		scrollToPage(page);
	}

	protected IEnumerator scrollToInset(int target)
	{
		float start = _scrollPosition;
		float startTime = Time.time;
		float duration = 0.4f;
		bool running = true;
		while (!_isDragging && running)
		{
			float easPos = Mathf.Clamp01((Time.time - startTime) / duration);
			easPos = Easing.Quartic.easeOut(easPos);
			_scrollPosition = (int)Mathf.Lerp(start, target, easPos);
			layoutChildren();
			if (easPos == 1f)
			{
				running = false;
			}
			yield return null;
		}
		layoutChildren();
	}

	protected IEnumerator checkDelayedContentTouch()
	{
		yield return new WaitForSeconds(0.1f);
		if (_isDragging && Mathf.Abs(_deltaTouch) < TOUCH_MAX_DELTA_FOR_ACTIVATION)
		{
			Vector2 fixedTouchPosition = new Vector2(_lastTouch.position.x, (float)Screen.height - _lastTouch.position.y);
			_activeTouchable = getButtonForScreenPosition(fixedTouchPosition);
			if (_activeTouchable != null)
			{
				_activeTouchable.onTouchBegan(_lastTouch, fixedTouchPosition);
			}
		}
	}

	protected abstract void clipChild(UISprite child);

	protected virtual void clipToBounds()
	{
		foreach (UISprite child in _children)
		{
			clipChild(child);
		}
	}

	protected void recurseAndClipChildren(UIObject child)
	{
		foreach (Transform item in child.client.transform)
		{
			UIElement component = item.GetComponent<UIElement>();
			if (!(component != null))
			{
				continue;
			}
			UIObject uIObject = item.GetComponent<UIElement>().UIObject;
			if (uIObject == null)
			{
				continue;
			}
			UISprite uISprite = uIObject as UISprite;
			if (uISprite != null)
			{
				clipChild(uISprite);
				continue;
			}
			UITextInstance uITextInstance = uIObject as UITextInstance;
			if (uITextInstance != null)
			{
				foreach (UISprite textSprite in uITextInstance.textSprites)
				{
					clipChild(textSprite);
				}
			}
			recurseAndClipChildren(uITextInstance);
		}
	}

	protected override void layoutChildren()
	{
		base.layoutChildren();
		clipToBounds();
	}

	public override void transformChanged()
	{
		setSize(_touchFrame.width, _touchFrame.height);
		base.transformChanged();
	}

	public override void endUpdates()
	{
		base.endUpdates();
		calculateMinMaxInsets();
	}

	private void calculateMinMaxInsets()
	{
		_minEdgeInset.x = 0f - _contentWidth + _touchFrame.width;
		_minEdgeInset.y = 0f - _contentHeight + _touchFrame.height;
		clipToBounds();
	}

	private ITouchable testTouchable(UIObject touchableObj, Vector2 touchPosition)
	{
		foreach (Transform item in touchableObj.client.transform)
		{
			UIElement component = item.GetComponent<UIElement>();
			if (!(component != null))
			{
				continue;
			}
			UIObject uIObject = item.GetComponent<UIElement>().UIObject;
			if (uIObject != null)
			{
				ITouchable touchable = testTouchable(uIObject, touchPosition);
				if (touchable != null)
				{
					return touchable;
				}
			}
		}
		ITouchable touchable2 = touchableObj as ITouchable;
		if (touchable2 != null && touchable2.hitTest(touchPosition))
		{
			return touchable2;
		}
		return null;
	}

	protected ITouchable getButtonForScreenPosition(Vector2 touchPosition)
	{
		for (int num = _children.Count - 1; num >= 0; num--)
		{
			UISprite uISprite = _children[num];
			if (uISprite != null)
			{
				ITouchable touchable = testTouchable(uISprite, touchPosition);
				if (touchable != null)
				{
					return touchable;
				}
			}
		}
		return null;
	}

	public bool hitTest(Vector2 point)
	{
		return touchFrame.Contains(point);
	}

	public virtual void onTouchBegan(Touch touch, Vector2 touchPos)
	{
		_firstTouch = touch;
		if (_activeTouchable != null)
		{
			_activeTouchable.highlighted = false;
			_activeTouchable = null;
		}
		_deltaTouch = 0f;
		_isDragging = true;
		_velocities.Clear();
		_manager.StartCoroutine(checkDelayedContentTouch());
	}

	public virtual void onTouchMoved(Touch touch, Vector2 touchPos)
	{
	}

	public virtual void onTouchEnded(Touch touch, Vector2 touchPos, bool touchWasInsideTouchFrame)
	{
		_isDragging = false;
		if (_activeTouchable != null)
		{
			_activeTouchable.onTouchEnded(touch, touchPos, true);
			_activeTouchable.highlighted = false;
			_activeTouchable = null;
		}
		else
		{
			_manager.StartCoroutine(decelerate());
		}
	}

	public void scrollTo(int newOffset, bool animated)
	{
		if (animated)
		{
			_manager.StartCoroutine(scrollToInset(newOffset));
			return;
		}
		_scrollPosition = newOffset;
		layoutChildren();
	}

	public void scrollToPage(int page)
	{
		int num = page * base.spacing;
		_manager.StartCoroutine(scrollToInset((int)((float)(-page) * pageWidth - (float)num)));
	}

	public void setSize(float width, float height)
	{
		_touchFrame = new Rect(position.x, 0f - position.y, width, height);
		calculateMinMaxInsets();
	}

	public void Clear()
	{
		_children.Clear();
		layoutChildren();
	}

	public override void addChild(params UISprite[] children)
	{
		base.addChild(children);
		if (!_suspendUpdates)
		{
			calculateMinMaxInsets();
		}
		foreach (UISprite uISprite in children)
		{
			if (uISprite is ITouchable)
			{
				uISprite.manager.removeFromTouchables(uISprite as ITouchable);
			}
		}
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

using System;
using System.Collections.Generic;
using UnityEngine;

public class UIToolkit : UISpriteManager
{
	public static UIToolkit instance;

	public bool displayTouchDebugAreas;

	private ITouchable[] _spriteSelected;

	private List<ITouchable> _touchableSprites = new List<ITouchable>();

	public bool useOutsideTouchControl;

	public List<ITouchable> touchableSprites
	{
		get
		{
			return _touchableSprites;
		}
	}

	protected override void Awake()
	{
		instance = this;
		base.Awake();
		_spriteSelected = new ITouchable[12];
		for (int i = 0; i < 12; i++)
		{
			_spriteSelected[i] = null;
		}
	}

	protected void Update()
{
    if (useOutsideTouchControl) return;

    // 1. Handle Real Touches (Mobile)
    if (Input.touchCount > 0)
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            handleInput(touch.position, touch.phase, touch.fingerId);
        }
    }
    // 2. Handle Mouse Clicks (Unity Editor / PC)
    else if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
    {
        TouchPhase phase = TouchPhase.Moved;
        if (Input.GetMouseButtonDown(0)) phase = TouchPhase.Began;
        else if (Input.GetMouseButtonUp(0)) phase = TouchPhase.Ended;

        handleInput(Input.mousePosition, phase, 0);
    }
}

	private void handleInput(Vector2 screenPos, TouchPhase phase, int fingerId)
{
    // Convert Y coordinate for the toolkit's coordinate system
    Vector2 vector = new Vector2(screenPos.x, (float)Screen.height - screenPos.y);
    ITouchable buttonForScreenPosition = getButtonForScreenPosition(vector);
    
    bool isEnding = (phase == TouchPhase.Ended || phase == TouchPhase.Canceled);

    if (phase == TouchPhase.Began)
    {
        if (buttonForScreenPosition != null)
        {
            _spriteSelected[fingerId] = buttonForScreenPosition;
            buttonForScreenPosition.onTouchBegan(new Touch(), vector); // Passing empty touch as legacy support
        }
        else
        {
            _spriteSelected[fingerId] = null;
        }
    }
    else if (phase == TouchPhase.Moved || phase == TouchPhase.Stationary)
    {
        if (buttonForScreenPosition != null && _spriteSelected[fingerId] == buttonForScreenPosition)
        {
            _spriteSelected[fingerId].onTouchMoved(new Touch(), vector);
        }
        else if (_spriteSelected[fingerId] != null)
        {
            _spriteSelected[fingerId].onTouchEnded(new Touch(), vector, false);
            _spriteSelected[fingerId] = null;
        }
    }
    else if (isEnding)
    {
        if (_spriteSelected[fingerId] != null)
        {
            bool hit = (_spriteSelected[fingerId] == buttonForScreenPosition);
            _spriteSelected[fingerId].onTouchEnded(new Touch(), vector, hit);
            _spriteSelected[fingerId] = null;
        }
    }
}

	protected void LateUpdate()
	{
		if (meshIsDirty)
		{
			meshIsDirty = false;
			updateMeshProperties();
		}
	}

	protected void OnApplicationQuit()
	{
		material.mainTexture = null;
		instance = null;
		Resources.UnloadUnusedAssets();
	}

	protected void OnDestroy()
	{
		material.mainTexture = null;
		instance = null;
		Resources.UnloadUnusedAssets();
	}

	public void addTouchableSprite(ITouchable touchableSprite)
	{
		if (touchableSprite is UISprite)
		{
			addSprite(touchableSprite as UISprite);
		}
		addToTouchables(touchableSprite);
	}

	public void removeElement(UISprite sprite)
	{
		if (sprite is ITouchable)
		{
			_touchableSprites.Remove(sprite as ITouchable);
		}
		removeSprite(sprite);
	}

	public void removeFromTouchables(ITouchable touchable)
	{
		_touchableSprites.Remove(touchable);
	}

	public void addToTouchables(ITouchable touchable)
	{
		_touchableSprites.Add(touchable);
		_touchableSprites.Sort();
	}

	private void lookAtTouch(Touch touch)
	{
		Vector2 vector = new Vector2(touch.position.x, (float)Screen.height - touch.position.y);
		ITouchable buttonForScreenPosition = getButtonForScreenPosition(vector);
		bool flag = touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;
		if (touch.phase == TouchPhase.Began)
		{
			if (buttonForScreenPosition != null)
			{
				_spriteSelected[touch.fingerId] = buttonForScreenPosition;
				buttonForScreenPosition.onTouchBegan(touch, vector);
			}
			else
			{
				_spriteSelected[touch.fingerId] = null;
			}
		}
		else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
		{
			if (buttonForScreenPosition != null && _spriteSelected[touch.fingerId] == buttonForScreenPosition)
			{
				_spriteSelected[touch.fingerId].onTouchMoved(touch, vector);
			}
			else if (_spriteSelected[touch.fingerId] != null)
			{
				_spriteSelected[touch.fingerId].onTouchEnded(touch, vector, false);
				_spriteSelected[touch.fingerId] = null;
			}
			else if (buttonForScreenPosition != null && _spriteSelected[touch.fingerId] == null && buttonForScreenPosition.allowTouchBeganWhenMovedOver)
			{
				_spriteSelected[touch.fingerId] = buttonForScreenPosition;
				buttonForScreenPosition.onTouchBegan(touch, vector);
			}
		}
		else
		{
			if (!flag)
			{
				return;
			}
			if (buttonForScreenPosition != null)
			{
				if (_spriteSelected[touch.fingerId] != buttonForScreenPosition && _spriteSelected[touch.fingerId] != null)
				{
					_spriteSelected[touch.fingerId].onTouchEnded(touch, vector, false);
				}
				else if (_spriteSelected[touch.fingerId] == buttonForScreenPosition)
				{
					_spriteSelected[touch.fingerId].onTouchEnded(touch, vector, true);
				}
				_spriteSelected[touch.fingerId] = null;
			}
			else if (_spriteSelected[touch.fingerId] != null)
			{
				_spriteSelected[touch.fingerId].onTouchEnded(touch, vector, false);
				_spriteSelected[touch.fingerId] = null;
			}
		}
	}

	private ITouchable getButtonForScreenPosition(Vector2 touchPosition)
	{
		int i = 0;
		for (int count = _touchableSprites.Count; i < count; i++)
		{
			if (!_touchableSprites[i].hidden && _touchableSprites[i].hitTest(touchPosition))
			{
				return _touchableSprites[i];
			}
		}
		return null;
	}

	public bool IsButtonTouched(Vector2 touchPosition)
	{
		if (getButtonForScreenPosition(touchPosition) != null)
		{
			return true;
		}
		return false;
	}
}

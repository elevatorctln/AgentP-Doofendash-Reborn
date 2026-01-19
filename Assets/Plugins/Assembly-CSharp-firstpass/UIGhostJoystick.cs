using UnityEngine;

public class UIGhostJoystick : UITouchableSprite
{
	public Vector2 joystickPosition;

	public Vector2 deadZone = Vector2.zero;

	public bool normalize = true;

	public UIUVRect highlightedUVframe = UIUVRect.zero;

	private UISprite _joystickSprite;

	private UISprite _backgroundSprite;

	private Vector2 _joystickCenter;

	public float maxJoystickMovement = 20f;

	private UIToolkit _manager;

	private int currentTouchId = -1;

	public override bool hidden
	{
		set
		{
			if (value != ___hidden)
			{
				___hidden = value;
				_joystickSprite.hidden = value;
				if (_backgroundSprite != null)
				{
					_backgroundSprite.hidden = value;
				}
				base.hidden = value;
			}
		}
	}

	public UIGhostJoystick(UIToolkit manager, Rect frame, int depth, UISprite joystickSprite)
		: base(frame, depth, UIUVRect.zero)
	{
		_tempUVframe = joystickSprite.uvFrame;
		_joystickSprite = joystickSprite;
		_joystickSprite.parentUIObject = this;
		resetJoystick();
		manager.addTouchableSprite(this);
		_manager = manager;
	}

	public static UIGhostJoystick create(string joystickFilename, Rect hitArea)
	{
		return create(UI.firstToolkit, joystickFilename, hitArea);
	}

	public static UIGhostJoystick create(UIToolkit manager, string joystickFilename, Rect hitArea)
	{
		UISprite joystickSprite = manager.addSprite(joystickFilename, 0, 0, 1, true);
		return new UIGhostJoystick(manager, hitArea, 1, joystickSprite);
	}

	public void setJoystickHighlightedFilename(string filename)
	{
		highlightedUVframe = _manager.textureInfoForFilename(filename).uvRect;
	}

	public void addBackgroundSprite(string filename)
	{
		_backgroundSprite = _manager.addSprite(filename, 0, 0, 2, true);
		_backgroundSprite.parentUIObject = this;
		_backgroundSprite.hidden = true;
	}

	private void resetJoystick()
	{
		_joystickSprite.localPosition = _joystickCenter;
		joystickPosition.x = (joystickPosition.y = 0f);
		if (highlightedUVframe != UIUVRect.zero)
		{
			_joystickSprite.uvFrame = _tempUVframe;
		}
		if (_backgroundSprite != null)
		{
			_backgroundSprite.hidden = true;
		}
		_joystickSprite.localPosition = new Vector3(-1000f, -1000f, -1000f);
		_joystickSprite.hidden = true;
	}

	private void displayJoystick(Vector2 localTouchPos)
	{
		_joystickCenter = localTouchPos;
		_joystickSprite.localPosition = _joystickCenter;
		joystickPosition.x = (joystickPosition.y = 0f);
		if (_backgroundSprite != null)
		{
			_backgroundSprite.localPosition = new Vector3(_joystickCenter.x, _joystickCenter.y, 2f);
			_backgroundSprite.hidden = false;
		}
		_joystickSprite.hidden = false;
	}

	private void layoutJoystick(Vector2 localTouchPosition)
	{
		Vector2 vector = localTouchPosition;
		float sqrMagnitude = vector.sqrMagnitude;
		if (sqrMagnitude > maxJoystickMovement * maxJoystickMovement * (float)UI.scaleFactor * (float)UI.scaleFactor)
		{
			vector = vector.normalized * maxJoystickMovement * UI.scaleFactor;
		}
		_joystickSprite.localPosition = new Vector2(vector.x + _joystickCenter.x, _joystickCenter.y + vector.y);
		joystickPosition = vector / (maxJoystickMovement * (float)UI.scaleFactor);
		float num = Mathf.Abs(joystickPosition.x);
		float num2 = Mathf.Abs(joystickPosition.y);
		if (num < deadZone.x)
		{
			joystickPosition.x = 0f;
		}
		else if (normalize)
		{
			joystickPosition.x = Mathf.Sign(joystickPosition.x) * (num - deadZone.x) / (1f - deadZone.x);
		}
		if (num2 < deadZone.y)
		{
			joystickPosition.y = 0f;
		}
		else if (normalize)
		{
			joystickPosition.y = Mathf.Sign(joystickPosition.y) * (num2 - deadZone.y) / (1f - deadZone.y);
		}
	}

	public override void onTouchBegan(Touch touch, Vector2 touchPos)
	{
		if (currentTouchId == -1)
		{
			currentTouchId = touch.fingerId;
			touchPos.y = 0f - touchPos.y;
			highlighted = true;
			displayJoystick(touchPos);
			layoutJoystick(inverseTranformPoint(touchPos - _joystickCenter));
			if (highlightedUVframe != UIUVRect.zero)
			{
				_joystickSprite.uvFrame = highlightedUVframe;
			}
		}
	}

	public override void onTouchMoved(Touch touch, Vector2 touchPos)
	{
		if (touch.fingerId == currentTouchId)
		{
			touchPos.y = 0f - touchPos.y;
			layoutJoystick(inverseTranformPoint(touchPos - _joystickCenter));
		}
	}

	public override void onTouchEnded(Touch touch, Vector2 touchPos, bool touchWasInsideTouchFrame)
	{
		if (touch.fingerId == currentTouchId)
		{
			highlighted = false;
			resetJoystick();
			currentTouchId = -1;
		}
	}
}

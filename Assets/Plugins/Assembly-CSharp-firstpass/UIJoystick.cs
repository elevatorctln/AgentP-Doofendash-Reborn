using UnityEngine;

public class UIJoystick : UITouchableSprite
{
	public Vector2 joystickPosition;

	public Vector2 deadZone = Vector2.zero;

	public bool normalize = true;

	public UIUVRect highlightedUVframe = UIUVRect.zero;

	private UISprite _joystickSprite;

	private UISprite _backgroundSprite;

	private Vector3 _joystickOffset;

	private UIBoundary _joystickBoundary;

	private float _maxJoystickMovement = 40f;

	private UIToolkit _manager;

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

	public float maxJoystickMovement
	{
		get
		{
			return _maxJoystickMovement;
		}
		set
		{
			_maxJoystickMovement = value;
			_joystickBoundary = UIBoundary.boundaryFromPoint(_joystickOffset, _maxJoystickMovement);
		}
	}

	public UIJoystick(UIToolkit manager, Rect frame, int depth, UISprite joystickSprite, float xPos, float yPos)
		: base(frame, depth, UIUVRect.zero)
	{
		_tempUVframe = joystickSprite.uvFrame;
		_joystickSprite = joystickSprite;
		_joystickSprite.parentUIObject = this;
		_joystickOffset = new Vector3(xPos, yPos);
		maxJoystickMovement = _maxJoystickMovement;
		resetJoystick();
		manager.addTouchableSprite(this);
		_manager = manager;
	}

	public static UIJoystick create(string joystickFilename, Rect hitAreaFrame, float xPos, float yPos)
	{
		return create(UI.firstToolkit, joystickFilename, hitAreaFrame, xPos, yPos);
	}

	public static UIJoystick create(UIToolkit manager, string joystickFilename, Rect hitAreaFrame, float xPos, float yPos)
	{
		UISprite joystickSprite = manager.addSprite(joystickFilename, 0, 0, 1, true);
		return new UIJoystick(manager, hitAreaFrame, 1, joystickSprite, xPos, yPos);
	}

	public void setJoystickHighlightedFilename(string filename)
	{
		highlightedUVframe = _manager.textureInfoForFilename(filename).uvRect;
	}

	public void addBackgroundSprite(string filename)
	{
		_backgroundSprite = _manager.addSprite(filename, 0, 0, 2, true);
		_backgroundSprite.parentUIObject = this;
		_backgroundSprite.localPosition = new Vector3(_joystickOffset.x, _joystickOffset.y, 2f);
	}

	private void resetJoystick()
	{
		_joystickSprite.localPosition = _joystickOffset;
		joystickPosition.x = (joystickPosition.y = 0f);
		if (highlightedUVframe != UIUVRect.zero)
		{
			_joystickSprite.uvFrame = _tempUVframe;
		}
	}

	private void layoutJoystick(Vector2 localTouchPosition)
	{
		Vector3 zero = Vector3.zero;
		float value = localTouchPosition.x * _joystickSprite.localScale.x;
		float num = localTouchPosition.y * _joystickSprite.localScale.y;
		zero.x = Mathf.Clamp(value, _joystickBoundary.minX, _joystickBoundary.maxX);
		zero.y = Mathf.Clamp(0f - num, _joystickBoundary.minY, _joystickBoundary.maxY);
		_joystickSprite.localPosition = zero;
		joystickPosition = (zero - _joystickOffset) / _maxJoystickMovement;
		float num2 = Mathf.Abs(joystickPosition.x);
		float num3 = Mathf.Abs(joystickPosition.y);
		if (num2 < deadZone.x)
		{
			joystickPosition.x = 0f;
		}
		else if (normalize)
		{
			joystickPosition.x = Mathf.Sign(joystickPosition.x) * (num2 - deadZone.x) / (1f - deadZone.x);
		}
		if (num3 < deadZone.y)
		{
			joystickPosition.y = 0f;
		}
		else if (normalize)
		{
			joystickPosition.y = Mathf.Sign(joystickPosition.y) * (num3 - deadZone.y) / (1f - deadZone.y);
		}
	}

	public override void onTouchBegan(Touch touch, Vector2 touchPos)
	{
		highlighted = true;
		layoutJoystick(inverseTranformPoint(touchPos));
		if (highlightedUVframe != UIUVRect.zero)
		{
			_joystickSprite.uvFrame = highlightedUVframe;
		}
	}

	public override void onTouchMoved(Touch touch, Vector2 touchPos)
	{
		layoutJoystick(inverseTranformPoint(touchPos));
	}

	public override void onTouchEnded(Touch touch, Vector2 touchPos, bool touchWasInsideTouchFrame)
	{
		highlighted = false;
		resetJoystick();
	}
}

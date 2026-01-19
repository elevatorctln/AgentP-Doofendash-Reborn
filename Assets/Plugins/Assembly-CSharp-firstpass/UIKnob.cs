using UnityEngine;

public class UIKnob : UITouchableSprite
{
	public delegate void UIKnobChanged(UIKnob sender, float value);

	public bool continuous;

	private float _value;

	public UIUVRect highlightedUVframe;

	public override UIUVRect uvFrame
	{
		get
		{
			return _uvFrame;
		}
		set
		{
			_uvFrame = value;
			_tempUVframe = value;
			manager.updateUV(this);
		}
	}

	public override bool highlighted
	{
		set
		{
			if (_highlighted != value)
			{
				_highlighted = value;
				if (value)
				{
					base.uvFrame = highlightedUVframe;
				}
				else
				{
					base.uvFrame = _tempUVframe;
				}
			}
		}
	}

	public float value
	{
		get
		{
			return _value;
		}
		set
		{
			if (value != _value)
			{
				_value = Mathf.Clamp(value, 0f, 1f);
				clientTransform.rotation = Quaternion.Euler(0f, 0f, (0f - _value) * 360f);
				updateTransform();
			}
		}
	}

	public event UIKnobChanged onKnobChanged;

	public UIKnob(UIToolkit manager, Rect frame, int depth, UIUVRect uvFrame, UIUVRect highlightedUVframe)
		: base(frame, depth, uvFrame, true)
	{
		_tempUVframe = uvFrame;
		if (highlightedUVframe == UIUVRect.zero)
		{
			highlightedUVframe = uvFrame;
		}
		this.highlightedUVframe = highlightedUVframe;
		manager.addTouchableSprite(this);
	}

	public static UIKnob create(string filename, string highlightedFilename, int xPos, int yPos)
	{
		return create(UI.firstToolkit, filename, highlightedFilename, xPos, yPos);
	}

	public static UIKnob create(UIToolkit manager, string filename, string highlightedFilename, int xPos, int yPos)
	{
		return create(manager, filename, highlightedFilename, xPos, yPos, 1);
	}

	public static UIKnob create(UIToolkit manager, string filename, string highlightedFilename, int xPos, int yPos, int depth)
	{
		UITextureInfo uITextureInfo = manager.textureInfoForFilename(filename);
		Rect frame = new Rect(xPos, yPos, uITextureInfo.frame.width, uITextureInfo.frame.height);
		UITextureInfo uITextureInfo2 = manager.textureInfoForFilename(highlightedFilename);
		return new UIKnob(manager, frame, depth, uITextureInfo.uvRect, uITextureInfo2.uvRect);
	}

	private void updateKnobForTouchPosition(Vector2 touchPos)
	{
		Vector2 vector = inverseTranformPoint(touchPos);
		float num = vector.x - width / 2f;
		Vector3 to = new Vector3(num, height / 2f - vector.y, 0f) - Vector3.zero;
		float z = Vector3.Angle(Vector3.up, to) * (0f - Mathf.Sign(num));
		clientTransform.rotation = Quaternion.Euler(0f, 0f, z);
		updateTransform();
		_value = (360f - eulerAngles.z) / 360f;
		if (continuous && this.onKnobChanged != null)
		{
			this.onKnobChanged(this, _value);
		}
	}

	public override void onTouchBegan(Touch touch, Vector2 touchPos)
	{
		highlighted = true;
		updateKnobForTouchPosition(touchPos);
	}

	public override void onTouchMoved(Touch touch, Vector2 touchPos)
	{
		updateKnobForTouchPosition(touchPos);
	}

	public override void onTouchEnded(Touch touch, Vector2 touchPos, bool touchWasInsideTouchFrame)
	{
		highlighted = false;
		if (this.onKnobChanged != null)
		{
			this.onKnobChanged(this, _value);
		}
	}
}

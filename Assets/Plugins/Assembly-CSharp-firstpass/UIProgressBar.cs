using UnityEngine;

public class UIProgressBar : UISprite
{
	public bool rightToLeft;

	private float _value;

	private float _barOriginalWidth;

	private float _barOriginalHeight;

	private UIUVRect _barOriginalUVframe;

	private bool _resizeTextureOnChange;

	public bool resizeTextureOnChange
	{
		get
		{
			return _resizeTextureOnChange;
		}
		set
		{
			if (_resizeTextureOnChange != value)
			{
				if (value)
				{
					UIUVRect barOriginalUVframe = _barOriginalUVframe;
					barOriginalUVframe.uvDimensions.x *= _value;
					uvFrame = barOriginalUVframe;
				}
				else
				{
					uvFrame = _barOriginalUVframe;
				}
				if (rightToLeft)
				{
					setSize(_value * (0f - _barOriginalWidth), height);
				}
				else
				{
					setSize(_value * _barOriginalWidth, height);
				}
				_resizeTextureOnChange = value;
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
				if (resizeTextureOnChange)
				{
					UIUVRect barOriginalUVframe = _barOriginalUVframe;
					barOriginalUVframe.uvDimensions.x *= _value;
					uvFrame = barOriginalUVframe;
				}
				if (rightToLeft)
				{
					setSize(_value * (0f - _barOriginalWidth), _barOriginalHeight);
				}
				else
				{
					setSize(_value * _barOriginalWidth, _barOriginalHeight);
				}
			}
		}
	}

	public UIProgressBar(UIToolkit manager, Rect frame, int depth, UIUVRect uvFrame, bool rightToLeft)
		: base(frame, depth, uvFrame)
	{
		manager.addSprite(this);
		_barOriginalWidth = frame.width;
		_barOriginalHeight = frame.height;
		_barOriginalUVframe = uvFrame;
		this.rightToLeft = rightToLeft;
		if (rightToLeft)
		{
			setSize(_value * (0f - _barOriginalWidth), frame.height);
		}
		else
		{
			setSize(_value * _barOriginalWidth, frame.height);
		}
	}

	public static UIProgressBar create(string barFilename, int xPos, int yPos)
	{
		return create(UI.firstToolkit, barFilename, xPos, yPos, false, 1);
	}

	public static UIProgressBar create(UIToolkit manager, string barFilename, int xPos, int yPos, bool rightToLeft, int depth)
	{
		UITextureInfo uITextureInfo = manager.textureInfoForFilename(barFilename);
		Rect frame = new Rect(xPos, yPos, uITextureInfo.frame.width, uITextureInfo.frame.height);
		if (rightToLeft)
		{
			frame.x = xPos + (int)uITextureInfo.frame.width;
		}
		return new UIProgressBar(manager, frame, depth, uITextureInfo.uvRect, rightToLeft);
	}
}

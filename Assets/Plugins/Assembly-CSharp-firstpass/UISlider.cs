using UnityEngine;

public class UISlider : UITouchableSprite
{
	public delegate void UISliderChanged(UISlider sender, float value);

	public bool continuous;

	private float _knobMinimumXY;

	private float _knobMaximumXY;

	private float _value;

	public UISprite _sliderKnob;

	private UISliderLayout layout;

	public override bool hidden
	{
		get
		{
			return ___hidden;
		}
		set
		{
			if (value != ___hidden)
			{
				base.hidden = value;
				_sliderKnob.hidden = value;
			}
		}
	}

	public override Color color
	{
		get
		{
			return base.color;
		}
		set
		{
			base.color = value;
			_sliderKnob.color = value;
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
				updateSliderKnobWithNormalizedValue(_value);
			}
		}
	}

	public event UISliderChanged onChange;

	public UISlider(UIToolkit manager, Rect frame, int depth, UIUVRect uvFrame, UISprite sliderKnob, UISliderLayout layout)
		: base(frame, depth, uvFrame)
	{
		this.layout = layout;
		_sliderKnob = sliderKnob;
		_sliderKnob.parentUIObject = this;
		updateSliderKnobConstraints();
		updateSliderKnobWithNormalizedValue(_value);
		manager.addTouchableSprite(this);
	}

	public static UISlider create(string knobFilename, string trackFilename, int trackxPos, int trackyPos, UISliderLayout layout, int depth = 2, bool knobInFront = true)
	{
		return create(UI.firstToolkit, knobFilename, trackFilename, trackxPos, trackyPos, layout, depth, knobInFront);
	}

	public static UISlider create(UIToolkit manager, string knobFilename, string trackFilename, int trackxPos, int trackyPos, UISliderLayout layout, int depth = 2, bool knobInFront = true)
	{
		UITextureInfo uITextureInfo = manager.textureInfoForFilename(trackFilename);
		Rect frame = new Rect(trackxPos, trackyPos, uITextureInfo.frame.width, uITextureInfo.frame.height);
		int depth2 = ((!knobInFront) ? (depth + 1) : (depth - 1));
		UISprite sliderKnob = manager.addSprite(knobFilename, trackxPos, trackyPos, depth2, true);
		return new UISlider(manager, frame, depth, uITextureInfo.uvRect, sliderKnob, layout);
	}

	public override void destroy()
	{
		_sliderKnob.destroy();
		base.destroy();
	}

	public override void updateTransform()
	{
		base.updateTransform();
		updateSliderKnobConstraints();
	}

	private void updateSliderKnobConstraints()
	{
		if (layout == UISliderLayout.Horizontal)
		{
			_knobMaximumXY = (width - _sliderKnob.width) / 2f;
			_knobMinimumXY = 0f - _knobMaximumXY;
		}
		else
		{
			_knobMaximumXY = (height - _sliderKnob.height) / 2f;
			_knobMinimumXY = 0f - _knobMaximumXY;
		}
		float num = 1f / (float)UI.scaleFactor;
		_knobMaximumXY *= num;
		_knobMinimumXY *= num;
	}

	private void updateSliderKnobWithNormalizedValue(float normalizedKnobValue)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = normalizedKnobValue - 0.5f;
		float num4 = 1f / (float)UI.scaleFactor;
		if (layout == UISliderLayout.Horizontal)
		{
			num = Mathf.Clamp((width - _sliderKnob.width) * num4 * num3, _knobMinimumXY, _knobMaximumXY);
		}
		else
		{
			num2 = 0f - Mathf.Clamp((height - _sliderKnob.height) * num4 * num3, _knobMinimumXY, _knobMaximumXY);
		}
		_sliderKnob.pixelsFromCenter((int)num2, (int)num);
	}

	private void updateSliderKnobForTouchPosition(Vector2 touchPos)
	{
		Vector2 vector = inverseTranformPoint(touchPos);
		float num = 0f;
		num = ((layout != UISliderLayout.Horizontal) ? (1f - (vector.y - _sliderKnob.height / 2f) / (height - _sliderKnob.height)) : ((vector.x - _sliderKnob.width / 2f) / (width - _sliderKnob.width)));
		value = num;
		if (continuous && this.onChange != null)
		{
			this.onChange(this, _value);
		}
	}

	public override void onTouchBegan(Touch touch, Vector2 touchPos)
	{
		highlighted = true;
		updateSliderKnobForTouchPosition(touchPos);
	}

	public override void onTouchMoved(Touch touch, Vector2 touchPos)
	{
		updateSliderKnobForTouchPosition(touchPos);
	}

	public override void onTouchEnded(Touch touch, Vector2 touchPos, bool touchWasInsideTouchFrame)
	{
		if (touchCount == 0)
		{
			highlighted = false;
		}
		if (this.onChange != null)
		{
			this.onChange(this, _value);
		}
	}
}

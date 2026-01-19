using UnityEngine;

public class UIToggleButton : UITouchableSprite
{
	public delegate void UIToggleButtonChanged(UIToggleButton sender, bool selected);

	public UIUVRect highlightedUVframe;

	public UIUVRect selectedUVframe;

	private bool _selected;

	public override UIUVRect uvFrame
	{
		get
		{
			return (!_clipped) ? _uvFrame : _uvFrameClipped;
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
				else if (_selected)
				{
					base.uvFrame = selectedUVframe;
				}
				else
				{
					base.uvFrame = _tempUVframe;
				}
			}
		}
	}

	public override bool disabled
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
					base.uvFrame = disabledUVframe;
				}
				else if (_selected)
				{
					base.uvFrame = selectedUVframe;
				}
				else
				{
					base.uvFrame = _tempUVframe;
				}
			}
		}
	}

	public bool selected
	{
		get
		{
			return _selected;
		}
		set
		{
			if (_selected != value)
			{
				_selected = value;
				if (value)
				{
					base.uvFrame = selectedUVframe;
				}
				else
				{
					base.uvFrame = _tempUVframe;
				}
			}
		}
	}

	public event UIToggleButtonChanged onToggle;

	public UIToggleButton(UIToolkit manager, Rect frame, int depth, UIUVRect uvFrame, UIUVRect selectedUVframe, UIUVRect highlightedUVframe)
		: base(frame, depth, uvFrame)
	{
		this.selectedUVframe = selectedUVframe;
		if (highlightedUVframe == UIUVRect.zero)
		{
			highlightedUVframe = uvFrame;
		}
		this.highlightedUVframe = highlightedUVframe;
		manager.addTouchableSprite(this);
	}

	public static UIToggleButton create(string filename, string selectedFilename, string highlightedFilename, int xPos, int yPos)
	{
		return create(UI.firstToolkit, filename, selectedFilename, highlightedFilename, xPos, yPos);
	}

	public static UIToggleButton create(UIToolkit manager, string filename, string selectedFilename, string highlightedFilename, int xPos, int yPos)
	{
		return create(manager, filename, selectedFilename, highlightedFilename, xPos, yPos, 1);
	}

	public static UIToggleButton create(UIToolkit manager, string filename, string selectedFilename, string highlightedFilename, int xPos, int yPos, int depth)
	{
		UITextureInfo uITextureInfo = manager.textureInfoForFilename(filename);
		Rect frame = new Rect(xPos, yPos, uITextureInfo.frame.width, uITextureInfo.frame.height);
		UITextureInfo uITextureInfo2 = manager.textureInfoForFilename(selectedFilename);
		UITextureInfo uITextureInfo3 = manager.textureInfoForFilename(highlightedFilename);
		return new UIToggleButton(manager, frame, depth, uITextureInfo.uvRect, uITextureInfo2.uvRect, uITextureInfo3.uvRect);
	}

	public override void onTouchEnded(Touch touch, Vector2 touchPos, bool touchWasInsideTouchFrame)
	{
		highlighted = false;
		if (touchWasInsideTouchFrame)
		{
			selected = !_selected;
			if (this.onToggle != null)
			{
				this.onToggle(this, _selected);
			}
		}
	}
}

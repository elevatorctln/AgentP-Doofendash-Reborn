using System;
using UnityEngine;

public class UIButton : UITouchableSprite
{
	public UIUVRect highlightedUVframe;

	public AudioClip touchDownSound;

	public Vector2 initialTouchPosition;

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
				else
				{
					base.uvFrame = _tempUVframe;
				}
			}
		}
	}

	public event Action<UIButton> onTouchUpInside;

	public event Action<UIButton> onTouchDown;

	public event Action<UIButton> onTouchUp;

	public UIButton(UIToolkit manager, Rect frame, int depth, UIUVRect uvFrame, UIUVRect highlightedUVframe)
		: base(frame, depth, uvFrame)
	{
		if (highlightedUVframe == UIUVRect.zero)
		{
			highlightedUVframe = uvFrame;
		}
		this.highlightedUVframe = highlightedUVframe;
		manager.addTouchableSprite(this);
	}

	public static UIButton create(string filename, string highlightedFilename, int xPos, int yPos)
	{
		return create(UI.firstToolkit, filename, highlightedFilename, xPos, yPos);
	}

	public static UIButton create(UIToolkit manager, string filename, string highlightedFilename, int xPos, int yPos)
	{
		return create(manager, filename, highlightedFilename, xPos, yPos, 1);
	}

	public static UIButton create(UIToolkit manager, string filename, string highlightedFilename, int xPos, int yPos, int depth)
	{
		UITextureInfo uITextureInfo = manager.textureInfoForFilename(filename);
		Rect frame = new Rect(xPos, yPos, uITextureInfo.frame.width, uITextureInfo.frame.height);
		UITextureInfo uITextureInfo2 = manager.textureInfoForFilename(highlightedFilename);
		return new UIButton(manager, frame, depth, uITextureInfo.uvRect, uITextureInfo2.uvRect);
	}

	public override void setSpriteImage(string filename)
	{
		uvFrame = manager.uvRectForFilename(filename);
	}

	public void SetHighlightedImage(string filename)
	{
		highlightedUVframe = manager.uvRectForFilename(filename);
	}

	public override void onTouchBegan(Touch touch, Vector2 touchPos)
	{
		highlighted = true;
		initialTouchPosition = touch.position;
		if (touchDownSound != null)
		{
			UI.instance.playSound(touchDownSound);
		}
		if (this.onTouchDown != null)
		{
			this.onTouchDown(this);
		}
	}

	public override void onTouchEnded(Touch touch, Vector2 touchPos, bool touchWasInsideTouchFrame)
	{
		if (highlighted)
		{
			highlighted = false;
			if (this.onTouchUp != null)
			{
				this.onTouchUp(this);
			}
			if (touchWasInsideTouchFrame && this.onTouchUpInside != null)
			{
				this.onTouchUpInside(this);
			}
		}
	}

	public override void destroy()
	{
		base.destroy();
		highlighted = false;
	}
}

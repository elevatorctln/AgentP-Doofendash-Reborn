using UnityEngine;

public class UISlice9 : UITouchableSprite
{
	public delegate void UISlice9TouchUpInside(UISlice9 sender);

	public delegate void UISlice9TouchDown(UISlice9 sender);

	public delegate void UISlice9TouchUp(UISlice9 sender);

	public UIUVRect highlightedUVframe;

	public AudioClip touchDownSound;

	public Vector2 initialTouchPosition;

	private UISprite[] spriteSlices = new UISprite[9];

	public override UIUVRect uvFrame
	{
		get
		{
			return (!_clipped) ? _uvFrame : _uvFrameClipped;
		}
		set
		{
			base.uvFrame = value;
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

	public override bool hidden
	{
		get
		{
			return ___hidden;
		}
		set
		{
			base.hidden = value;
			UISprite[] array = spriteSlices;
			foreach (UISprite uISprite in array)
			{
				uISprite.hidden = value;
			}
		}
	}

	public event UISlice9TouchUpInside onTouchUpInside;

	public event UISlice9TouchDown onTouchDown;

	public event UISlice9TouchUp onTouchUp;

	public UISlice9(UIToolkit manager, Rect frame, int depth, UIUVRect uvFrame, UIUVRect highlightedUVframe, int width, int height, int sTP, int sRT, int sBT, int sLT)
		: base(frame, depth, uvFrame)
	{
		if (highlightedUVframe == UIUVRect.zero)
		{
			highlightedUVframe = uvFrame;
		}
		this.highlightedUVframe = highlightedUVframe;
		int num = width - (sLT + sRT);
		int num2 = height - (sTP + sBT);
		base.manager = manager;
		spriteSlices[0] = new UISprite(new Rect(frame.x, frame.y, sLT, sTP), depth, new UIUVRect(uvFrame.frame.x, uvFrame.frame.y, sLT, sTP, manager.textureSize));
		spriteSlices[0].client.transform.parent = base.client.transform;
		manager.addSprite(spriteSlices[0]);
		spriteSlices[1] = new UISprite(new Rect(frame.x + (float)sLT, frame.y, num, sTP), depth, new UIUVRect(uvFrame.frame.x + sLT, uvFrame.frame.y, uvFrame.frame.width - (sRT + sLT), sTP, manager.textureSize));
		spriteSlices[1].client.transform.parent = base.client.transform;
		manager.addSprite(spriteSlices[1]);
		spriteSlices[2] = new UISprite(new Rect(frame.x + (float)sLT + (float)num, frame.y, sRT, sTP), depth, new UIUVRect(uvFrame.frame.x + sLT + (uvFrame.frame.width - (sRT + sLT)), uvFrame.frame.y, sRT, sTP, manager.textureSize));
		spriteSlices[2].client.transform.parent = base.client.transform;
		manager.addSprite(spriteSlices[2]);
		spriteSlices[3] = new UISprite(new Rect(frame.x, frame.y + (float)sTP, sLT, num2), depth, new UIUVRect(uvFrame.frame.x, uvFrame.frame.y + sTP, sLT, uvFrame.frame.height - (sTP + sBT), manager.textureSize));
		spriteSlices[3].client.transform.parent = base.client.transform;
		manager.addSprite(spriteSlices[3]);
		spriteSlices[4] = new UISprite(new Rect(frame.x + (float)sLT, frame.y + (float)sTP, num, num2), depth, new UIUVRect(uvFrame.frame.x + sLT, uvFrame.frame.y + sTP, uvFrame.frame.height - (sTP + sBT), (int)frame.width - (sLT + sRT), manager.textureSize));
		spriteSlices[4].client.transform.parent = base.client.transform;
		manager.addSprite(spriteSlices[4]);
		spriteSlices[5] = new UISprite(new Rect(frame.x + (float)sLT + (float)num, frame.y + (float)sTP, sRT, num2), depth, new UIUVRect(uvFrame.frame.x + (uvFrame.frame.width - sRT), uvFrame.frame.y + sTP, sRT, uvFrame.frame.height - (sBT + sTP), manager.textureSize));
		spriteSlices[5].client.transform.parent = base.client.transform;
		manager.addSprite(spriteSlices[5]);
		spriteSlices[6] = new UISprite(new Rect(frame.x, frame.y + (float)sTP + (float)num2, sLT, sBT), depth, new UIUVRect(uvFrame.frame.x, uvFrame.frame.y + (uvFrame.frame.height - sBT), sLT, sBT, manager.textureSize));
		spriteSlices[6].client.transform.parent = base.client.transform;
		manager.addSprite(spriteSlices[6]);
		spriteSlices[7] = new UISprite(new Rect(frame.x + (float)sLT, frame.y + (float)sTP + (float)num2, num, sBT), depth, new UIUVRect(uvFrame.frame.x + sLT, uvFrame.frame.y + (uvFrame.frame.height - sBT), uvFrame.frame.width - (sLT + sRT), sBT, manager.textureSize));
		spriteSlices[7].client.transform.parent = base.client.transform;
		manager.addSprite(spriteSlices[7]);
		spriteSlices[8] = new UISprite(new Rect(frame.x + (float)sLT + (float)num, frame.y + (float)sTP + (float)num2, sRT, sBT), depth, new UIUVRect(uvFrame.frame.x + sLT + (uvFrame.frame.width - (sRT + sLT)), uvFrame.frame.y + (uvFrame.frame.height - sBT), sRT, sBT, manager.textureSize));
		spriteSlices[8].client.transform.parent = base.client.transform;
		manager.addSprite(spriteSlices[8]);
		setSize(width, height);
		manager.addTouchableSprite(this);
	}

	public static UISlice9 create(string filename, string highlightedFilename, int xPos, int yPos, int width, int height, int sTP, int sRT, int sBT, int sLT)
	{
		return create(UI.firstToolkit, filename, highlightedFilename, xPos, yPos, width, height, sTP, sRT, sBT, sLT);
	}

	public static UISlice9 create(UIToolkit manager, string filename, string highlightedFilename, int xPos, int yPos, int width, int height, int sTP, int sRT, int sBT, int sLT)
	{
		return create(manager, filename, highlightedFilename, xPos, yPos, width, height, sTP, sRT, sBT, sLT, 1);
	}

	public static UISlice9 create(UIToolkit manager, string filename, string highlightedFilename, int xPos, int yPos, int width, int height, int sTP, int sRT, int sBT, int sLT, int depth)
	{
		UITextureInfo uITextureInfo = manager.textureInfoForFilename(filename);
		Rect frame = new Rect(xPos, yPos, uITextureInfo.frame.width, uITextureInfo.frame.height);
		UITextureInfo uITextureInfo2 = manager.textureInfoForFilename(highlightedFilename);
		return new UISlice9(manager, frame, depth, uITextureInfo.uvRect, uITextureInfo2.uvRect, width, height, sTP, sRT, sBT, sLT);
	}

	public override void updateTransform()
	{
		manager.updatePositions();
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
		UISprite[] array = spriteSlices;
		foreach (UISprite uISprite in array)
		{
			uISprite.destroy();
		}
		base.destroy();
		highlighted = false;
	}

	public override void centerize()
	{
		base.centerize();
		UISprite[] array = spriteSlices;
		foreach (UISprite uISprite in array)
		{
			uISprite.centerize();
		}
	}

	public override void setSpriteImage(string filename)
	{
		UISprite[] array = spriteSlices;
		foreach (UISprite uISprite in array)
		{
			uISprite.setSpriteImage(filename);
		}
	}

	public override void transformChanged()
	{
		base.transformChanged();
		UISprite[] array = spriteSlices;
		foreach (UISprite uISprite in array)
		{
			uISprite.transformChanged();
		}
	}
}

using UnityEngine;

public class UISlice9v2 : UITouchableSprite
{
	private enum SliceIndexes
	{
		TopLeft = 0,
		TopCenter = 1,
		TopRight = 2,
		MidLeft = 3,
		MidCenter = 4,
		MidRight = 5,
		BotLeft = 6,
		BotCenter = 7,
		BotRight = 8
	}

	public delegate void UISlice9TouchUpInside(UISlice9v2 sender);

	public delegate void UISlice9TouchDown(UISlice9v2 sender);

	public delegate void UISlice9TouchUp(UISlice9v2 sender);

	public UIUVRect highlightedUVframe;

	public AudioClip touchDownSound;

	public Vector2 initialTouchPosition;

	private UISprite[] spriteSlices = new UISprite[9];

	private int topMarginPx;

	private int rightMarginPx;

	private int bottomMarginPx;

	private int leftMarginPx;

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

	public UISlice9v2(UIToolkit manager, Rect frame, int depth, UIUVRect uvFrame, UIUVRect highlightedUVframe, int width, int height, int sTP, int sRT, int sBT, int sLT)
		: base(frame, depth, uvFrame)
	{
		if (highlightedUVframe == UIUVRect.zero)
		{
			highlightedUVframe = uvFrame;
		}
		this.highlightedUVframe = highlightedUVframe;
		topMarginPx = sTP;
		rightMarginPx = sRT;
		bottomMarginPx = sBT;
		leftMarginPx = sLT;
		int num = width - (sLT + sRT);
		int num2 = height - (sTP + sBT);
		base.manager = manager;
		spriteSlices[0] = new UISprite(new Rect(frame.x, frame.y, sLT, sTP), depth, new UIUVRect(uvFrame.frame.x, uvFrame.frame.y, sLT, sTP, manager.textureSize));
		manager.addSprite(spriteSlices[0]);
		spriteSlices[0].client.transform.parent = base.client.transform;
		spriteSlices[1] = new UISprite(new Rect(frame.x + (float)sLT, frame.y, num, sTP), depth, new UIUVRect(uvFrame.frame.x + sLT, uvFrame.frame.y, uvFrame.frame.width - (sRT + sLT), sTP, manager.textureSize));
		manager.addSprite(spriteSlices[1]);
		spriteSlices[1].client.transform.parent = base.client.transform;
		spriteSlices[2] = new UISprite(new Rect(frame.x + (float)sLT + (float)num, frame.y, sRT, sTP), depth, new UIUVRect(uvFrame.frame.x + sLT + (uvFrame.frame.width - (sRT + sLT)), uvFrame.frame.y, sRT, sTP, manager.textureSize));
		manager.addSprite(spriteSlices[2]);
		spriteSlices[2].client.transform.parent = base.client.transform;
		spriteSlices[3] = new UISprite(new Rect(frame.x, frame.y + (float)sTP, sLT, num2), depth, new UIUVRect(uvFrame.frame.x, uvFrame.frame.y + sTP, sLT, uvFrame.frame.height - (sTP + sBT), manager.textureSize));
		manager.addSprite(spriteSlices[3]);
		spriteSlices[3].client.transform.parent = base.client.transform;
		spriteSlices[4] = new UISprite(new Rect(frame.x + (float)sLT, frame.y + (float)sTP, num, num2), depth, new UIUVRect(uvFrame.frame.x + sLT, uvFrame.frame.y + sTP, uvFrame.frame.height - (sTP + sBT), (int)frame.width - (sLT + sRT), manager.textureSize));
		manager.addSprite(spriteSlices[4]);
		spriteSlices[4].client.transform.parent = base.client.transform;
		spriteSlices[5] = new UISprite(new Rect(frame.x + (float)sLT + (float)num, frame.y + (float)sTP, sRT, num2), depth, new UIUVRect(uvFrame.frame.x + (uvFrame.frame.width - sRT), uvFrame.frame.y + sTP, sRT, uvFrame.frame.height - (sBT + sTP), manager.textureSize));
		manager.addSprite(spriteSlices[5]);
		spriteSlices[5].client.transform.parent = base.client.transform;
		spriteSlices[6] = new UISprite(new Rect(frame.x, frame.y + (float)sTP + (float)num2, sLT, sBT), depth, new UIUVRect(uvFrame.frame.x, uvFrame.frame.y + (uvFrame.frame.height - sBT), sLT, sBT, manager.textureSize));
		manager.addSprite(spriteSlices[6]);
		spriteSlices[6].client.transform.parent = base.client.transform;
		spriteSlices[7] = new UISprite(new Rect(frame.x + (float)sLT, frame.y + (float)sTP + (float)num2, num, sBT), depth, new UIUVRect(uvFrame.frame.x + sLT, uvFrame.frame.y + (uvFrame.frame.height - sBT), uvFrame.frame.width - (sLT + sRT), sBT, manager.textureSize));
		manager.addSprite(spriteSlices[7]);
		spriteSlices[7].client.transform.parent = base.client.transform;
		spriteSlices[8] = new UISprite(new Rect(frame.x + (float)sLT + (float)num, frame.y + (float)sTP + (float)num2, sRT, sBT), depth, new UIUVRect(uvFrame.frame.x + sLT + (uvFrame.frame.width - (sRT + sLT)), uvFrame.frame.y + (uvFrame.frame.height - sBT), sRT, sBT, manager.textureSize));
		manager.addSprite(spriteSlices[8]);
		spriteSlices[8].client.transform.parent = base.client.transform;
		_width = width;
		_height = height;
		UpdateSlicePositionAndSize();
		updateVertPositions();
		updateTransform();
		manager.addTouchableSprite(this);
	}

	public static UISlice9v2 create(string filename, string highlightedFilename, int xPos, int yPos, int width, int height, int sTP, int sRT, int sBT, int sLT)
	{
		return create(UI.firstToolkit, filename, highlightedFilename, xPos, yPos, width, height, sTP, sRT, sBT, sLT);
	}

	public static UISlice9v2 create(UIToolkit manager, string filename, string highlightedFilename, int xPos, int yPos, int width, int height, int sTP, int sRT, int sBT, int sLT)
	{
		return create(manager, filename, highlightedFilename, xPos, yPos, width, height, sTP, sRT, sBT, sLT, 1);
	}

	public static UISlice9v2 create(UIToolkit manager, string filename, string highlightedFilename, int xPos, int yPos, int width, int height, int sTP, int sRT, int sBT, int sLT, int depth)
	{
		UITextureInfo uITextureInfo = manager.textureInfoForFilename(filename);
		Rect frame = new Rect(xPos, yPos, uITextureInfo.frame.width, uITextureInfo.frame.height);
		UITextureInfo uITextureInfo2 = manager.textureInfoForFilename(highlightedFilename);
		return new UISlice9v2(manager, frame, depth, uITextureInfo.uvRect, uITextureInfo2.uvRect, width, height, sTP, sRT, sBT, sLT);
	}

	public static UISlice9v2 create(UIToolkit manager, string filename, int xPos, int yPos, float topMargin, float rightMargin, float bottomMargin, float leftMargin, int depth)
	{
		UITextureInfo uITextureInfo = manager.textureInfoForFilename(filename);
		Rect frame = new Rect(xPos, yPos, uITextureInfo.frame.width, uITextureInfo.frame.height);
		int sTP = (int)(frame.height * topMargin);
		int sRT = (int)(frame.height * rightMargin);
		int sBT = (int)(frame.height * bottomMargin);
		int sLT = (int)(frame.height * leftMargin);
		return new UISlice9v2(manager, frame, depth, uITextureInfo.uvRect, UIUVRect.zero, (int)frame.width, (int)frame.height, sTP, sRT, sBT, sLT);
	}

	public override void updateTransform()
	{
		UISprite[] array = spriteSlices;
		foreach (UISprite uISprite in array)
		{
			uISprite.updateTransform();
		}
		manager.updatePositions();
	}

	private void UpdateSlicePositionAndSize()
	{
		int num = (int)width;
		int num2 = (int)height;
		int num3 = num - (leftMarginPx + rightMarginPx);
		int num4 = num2 - (topMarginPx + bottomMarginPx);
		float z = position.z;
		Vector3 vector = Vector3.zero;
		if (gameObjectOriginInCenter)
		{
			vector = new Vector3(-num / 2, num2 / 2, 0f);
		}
		spriteSlices[0].position = new Vector3(num, -num2, z) + vector;
		spriteSlices[1].position = new Vector3(num + leftMarginPx, -num2, z) + vector;
		spriteSlices[2].position = new Vector3(num + leftMarginPx + num3, -num2, z) + vector;
		spriteSlices[3].position = new Vector3(num, -num2 - topMarginPx, z) + vector;
		spriteSlices[4].position = new Vector3(num + leftMarginPx, -num2 - topMarginPx, z) + vector;
		spriteSlices[5].position = new Vector3(num + leftMarginPx + num3, -num2 - topMarginPx, z) + vector;
		spriteSlices[6].position = new Vector3(num, -num2 - topMarginPx - num4, z) + vector;
		spriteSlices[7].position = new Vector3(num + leftMarginPx, -num2 - topMarginPx - num4, z) + vector;
		spriteSlices[8].position = new Vector3(num + leftMarginPx + num3, -num2 - topMarginPx - num4, z) + vector;
		spriteSlices[1].setSize(num3, topMarginPx);
		spriteSlices[3].setSize(leftMarginPx, num4);
		spriteSlices[4].setSize(num3, num4);
		spriteSlices[5].setSize(rightMarginPx, num4);
		spriteSlices[7].setSize(num3, bottomMarginPx);
		if (gameObjectOriginInCenter)
		{
			UISprite[] array = spriteSlices;
			foreach (UISprite uISprite in array)
			{
				uISprite.gameObjectOriginInCenter = false;
				uISprite.centerize();
			}
		}
	}

	public override void setSize(float width, float height)
	{
		beginUpdates();
		base.setSize(width, height);
		UpdateSlicePositionAndSize();
		endUpdates();
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
		Vector3 vector = position;
		base.centerize();
		UISprite[] array = spriteSlices;
		foreach (UISprite uISprite in array)
		{
			uISprite.position -= position - vector;
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

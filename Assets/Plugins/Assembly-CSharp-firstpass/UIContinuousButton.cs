using UnityEngine;

public class UIContinuousButton : UIButton
{
	public delegate void UIContinousButtonDelegate(UIButton sender);

	public bool onlyFireStartAndEndEvents;

	public event UIContinousButtonDelegate onTouchIsDown;

	public event UIContinousButtonDelegate onActivationStarted;

	public event UIContinousButtonDelegate onActivationEnded;

	public UIContinuousButton(UIToolkit manager, Rect frame, int depth, UIUVRect uvFrame, UIUVRect highlightedUVframe)
		: base(manager, frame, depth, uvFrame, highlightedUVframe)
	{
	}

	public new static UIContinuousButton create(string filename, string highlightedFilename, int xPos, int yPos)
	{
		return create(UI.firstToolkit, filename, highlightedFilename, xPos, yPos);
	}

	public new static UIContinuousButton create(UIToolkit manager, string filename, string highlightedFilename, int xPos, int yPos)
	{
		return create(manager, filename, highlightedFilename, xPos, yPos, 1);
	}

	public new static UIContinuousButton create(UIToolkit manager, string filename, string highlightedFilename, int xPos, int yPos, int depth)
	{
		UITextureInfo uITextureInfo = manager.textureInfoForFilename(filename);
		Rect frame = new Rect(xPos, yPos, uITextureInfo.frame.width, uITextureInfo.frame.height);
		UITextureInfo uITextureInfo2 = manager.textureInfoForFilename(highlightedFilename);
		return new UIContinuousButton(manager, frame, depth, uITextureInfo.uvRect, uITextureInfo2.uvRect);
	}

	public override void onTouchBegan(Touch touch, Vector2 touchPos)
	{
		base.onTouchBegan(touch, touchPos);
		if (onlyFireStartAndEndEvents && this.onActivationStarted != null)
		{
			this.onActivationStarted(this);
		}
	}

	public override void onTouchMoved(Touch touch, Vector2 touchPos)
	{
		if (!onlyFireStartAndEndEvents && this.onTouchIsDown != null)
		{
			this.onTouchIsDown(this);
		}
	}

	public override void onTouchEnded(Touch touch, Vector2 touchPos, bool touchWasInsideTouchFrame)
	{
		base.onTouchEnded(touch, touchPos, touchWasInsideTouchFrame);
		if (onlyFireStartAndEndEvents && this.onActivationEnded != null)
		{
			this.onActivationEnded(this);
		}
	}
}

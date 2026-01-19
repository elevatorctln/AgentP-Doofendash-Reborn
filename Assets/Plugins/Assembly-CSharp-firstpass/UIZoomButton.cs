using UnityEngine;

public class UIZoomButton : UIButton
{
	private UIAnimation _zoomInAnimation;

	private UIAnimation _zoomOutAnimation;

	public float animationDuration
	{
		set
		{
			_zoomOutAnimation.setDuration(value);
			_zoomInAnimation.setDuration(value);
		}
	}

	public Vector3 animationTargetScale
	{
		set
		{
			_zoomInAnimation.setTarget(value);
		}
	}

	public UIZoomButton(UIToolkit manager, Rect frame, int depth, UIUVRect uvFrame, UIUVRect highlightedUVframe)
		: base(manager, frame, depth, uvFrame, highlightedUVframe)
	{
		centerize();
		autoRefreshPositionOnScaling = false;
		_zoomInAnimation = new UIAnimation(this, 0.3f, UIAnimationProperty.Scale, new Vector3(1f, 1f, 1f), new Vector3(1.3f, 1.3f, 1.3f), Easing.Quartic.easeInOut);
		_zoomOutAnimation = new UIAnimation(this, 0.3f, UIAnimationProperty.Scale, new Vector3(1.3f, 1.3f, 1.3f), new Vector3(1f, 1f, 1f), Easing.Quartic.easeInOut);
	}

	public new static UIZoomButton create(string filename, string highlightedFilename, int xPos, int yPos)
	{
		return create(UI.firstToolkit, filename, highlightedFilename, xPos, yPos);
	}

	public new static UIZoomButton create(UIToolkit manager, string filename, string highlightedFilename, int xPos, int yPos)
	{
		return create(manager, filename, highlightedFilename, xPos, yPos, 1);
	}

	public new static UIZoomButton create(UIToolkit manager, string filename, string highlightedFilename, int xPos, int yPos, int depth)
	{
		UITextureInfo uITextureInfo = manager.textureInfoForFilename(filename);
		Rect frame = new Rect(xPos, yPos, uITextureInfo.frame.width, uITextureInfo.frame.height);
		UITextureInfo uITextureInfo2 = manager.textureInfoForFilename(highlightedFilename);
		return new UIZoomButton(manager, frame, depth, uITextureInfo.uvRect, uITextureInfo2.uvRect);
	}

	public override void onTouchBegan(Touch touch, Vector2 touchPos)
	{
		base.onTouchBegan(touch, touchPos);
		_zoomInAnimation.restartStartToCurrent();
		UI.instance.StartCoroutine(_zoomInAnimation.animate());
	}

	public override void onTouchEnded(Touch touch, Vector2 touchPos, bool touchWasInsideTouchFrame)
	{
		base.onTouchEnded(touch, touchPos, touchWasInsideTouchFrame);
		_zoomInAnimation.stop();
		_zoomOutAnimation.restartStartToCurrent();
		UI.instance.StartCoroutine(_zoomOutAnimation.animate());
	}
}

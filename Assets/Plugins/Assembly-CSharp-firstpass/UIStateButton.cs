using UnityEngine;

public class UIStateButton : UIButton
{
	public delegate void UIStateButtonStateChange(UIStateButton sender, int state);

	private bool _rollOverState = true;

	private int _state;

	private int maxState;

	private UIUVRect[] _uvFrames;

	private UIUVRect[] _uvHighlightFrames;

	public bool rollOverState
	{
		get
		{
			return _rollOverState;
		}
		set
		{
			_rollOverState = value;
		}
	}

	public int state
	{
		get
		{
			return _state;
		}
		set
		{
			if (_state != value)
			{
				_state = value;
				adjustForStateRollover(_state);
				setFramesForState(_state);
			}
		}
	}

	public UIUVRect[] uvFrames
	{
		get
		{
			return _uvFrames;
		}
	}

	public UIUVRect[] uvHighlightFrames
	{
		get
		{
			return _uvHighlightFrames;
		}
	}

	public event UIStateButtonStateChange onStateChange;

	public UIStateButton(UIToolkit manager, Rect frame, int depth, UIUVRect uvFrame, UIUVRect highlightedUVframe)
		: base(manager, frame, depth, uvFrame, highlightedUVframe)
	{
	}

	public new static UIStateButton create(string filename, string highlightedFilename, int xPos, int yPos)
	{
		return create(UI.firstToolkit, filename, highlightedFilename, xPos, yPos);
	}

	public new static UIStateButton create(UIToolkit manager, string filename, string highlightedFilename, int xPos, int yPos)
	{
		return create(manager, filename, highlightedFilename, xPos, yPos, 1);
	}

	public new static UIStateButton create(UIToolkit manager, string filename, string highlightedFilename, int xPos, int yPos, int depth)
	{
		string[] filenames = new string[1] { filename };
		string[] highlightedFilenames = new string[1] { highlightedFilename };
		return create(manager, filenames, highlightedFilenames, xPos, yPos, depth);
	}

	public static UIStateButton create(string[] filenames, string[] highlightedFilenames, int xPos, int yPos)
	{
		return create(UI.firstToolkit, filenames, highlightedFilenames, xPos, yPos);
	}

	public static UIStateButton create(UIToolkit manager, string[] filenames, string[] highlightedFilenames, int xPos, int yPos)
	{
		return create(manager, filenames, highlightedFilenames, xPos, yPos, 1);
	}

	public static UIStateButton create(UIToolkit manager, string[] filenames, string[] highlightedFilenames, int xPos, int yPos, int depth)
	{
		UITextureInfo uITextureInfo = manager.textureInfoForFilename(filenames[0]);
		Rect frame = new Rect(xPos, yPos, uITextureInfo.frame.width, uITextureInfo.frame.height);
		UITextureInfo uITextureInfo2 = uITextureInfo;
		if (highlightedFilenames.Length > 0)
		{
			manager.textureInfoForFilename(highlightedFilenames[0]);
		}
		UIStateButton uIStateButton = new UIStateButton(manager, frame, depth, uITextureInfo.uvRect, uITextureInfo2.uvRect);
		uIStateButton.addFrames(filenames, highlightedFilenames);
		return uIStateButton;
	}

	public void addFrames(string[] normal, string[] highlighted)
	{
		UIUVRect[] normal2 = loadFrames(normal);
		UIUVRect[] array = loadFrames(highlighted);
		addFrames(normal2, array);
	}

	public void addFrames(UIUVRect[] normal, UIUVRect[] highlighted)
	{
		_uvFrames = normal;
		maxState = _uvFrames.Length;
		_state = 0;
		if (highlighted == null || highlighted.Length == 0)
		{
			_uvHighlightFrames = normal;
			return;
		}
		if (normal.Length == highlighted.Length)
		{
			_uvHighlightFrames = highlighted;
			return;
		}
		Debug.LogError("Highlight frames count does not match normal frames count");
		_uvHighlightFrames = normal;
	}

	public override void onTouchEnded(Touch touch, Vector2 touchPos, bool touchWasInsideTouchFrame)
	{
		if (touchWasInsideTouchFrame)
		{
			_state++;
			adjustForStateRollover(_state);
		}
		base.onTouchEnded(touch, touchPos, touchWasInsideTouchFrame);
		if (touchWasInsideTouchFrame)
		{
			setFramesForState(_state);
			if (this.onStateChange != null)
			{
				this.onStateChange(this, _state);
			}
		}
	}

	private void adjustForStateRollover(int newState)
	{
		if (_state >= maxState)
		{
			if (_rollOverState)
			{
				_state = 0;
			}
			else
			{
				_state = maxState - 1;
			}
		}
	}

	private void setFramesForState(int newState)
	{
		uvFrame = _uvFrames[newState];
		highlightedUVframe = _uvHighlightFrames[newState];
	}

	private UIUVRect[] loadFrames(string[] filenames)
	{
		UIUVRect[] array = new UIUVRect[filenames.Length];
		for (int i = 0; i < filenames.Length; i++)
		{
			UIUVRect uvRect = manager.textureInfoForFilename(filenames[i]).uvRect;
			array[i] = uvRect;
		}
		return array;
	}
}

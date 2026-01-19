using UnityEngine;

public class UIStateSprite : UISprite
{
	private bool _rollOverState = true;

	private int _state;

	private int maxState;

	private UIUVRect[] _uvFrames;

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
				setFrameForState(_state);
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

	public UIStateSprite(UIToolkit manager, Rect frame, int depth, UIUVRect uvFrame)
		: base(frame, depth, uvFrame)
	{
		manager.addSprite(this);
	}

	public static UIStateSprite create(string filename, int xPos, int yPos)
	{
		return create(UI.firstToolkit, filename, xPos, yPos);
	}

	public static UIStateSprite create(UIToolkit manager, string filename, int xPos, int yPos)
	{
		return create(manager, filename, xPos, yPos, 1);
	}

	public static UIStateSprite create(UIToolkit manager, string filename, int xPos, int yPos, int depth)
	{
		string[] filenames = new string[1] { filename };
		return create(manager, filenames, xPos, yPos, depth);
	}

	public static UIStateSprite create(string[] filenames, int xPos, int yPos)
	{
		return create(UI.firstToolkit, filenames, xPos, yPos);
	}

	public static UIStateSprite create(UIToolkit manager, string[] filenames, int xPos, int yPos)
	{
		return create(manager, filenames, xPos, yPos, 1);
	}

	public static UIStateSprite create(UIToolkit manager, string[] filenames, int xPos, int yPos, int depth)
	{
		UITextureInfo uITextureInfo = manager.textureInfoForFilename(filenames[0]);
		Rect frame = new Rect(xPos, yPos, uITextureInfo.frame.width, uITextureInfo.frame.height);
		UIStateSprite uIStateSprite = new UIStateSprite(manager, frame, depth, uITextureInfo.uvRect);
		uIStateSprite.addFrames(filenames);
		return uIStateSprite;
	}

	public void addFrames(string[] normal)
	{
		addFrames(loadFrames(normal));
	}

	public void addFrames(UIUVRect[] normal)
	{
		_uvFrames = normal;
		maxState = _uvFrames.Length;
		_state = 0;
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
		else if (_state < 0)
		{
			if (_rollOverState)
			{
				_state = maxState - 1;
			}
			else
			{
				_state = 0;
			}
		}
	}

	private void setFrameForState(int newState)
	{
		base.uvFrame = _uvFrames[newState];
	}

	private UIUVRect[] loadFrames(string[] filenames)
	{
		UIUVRect[] array = new UIUVRect[filenames.Length];
		for (int i = 0; i < filenames.Length; i++)
		{
			array[i] = manager.textureInfoForFilename(filenames[i]).uvRect;
		}
		return array;
	}
}

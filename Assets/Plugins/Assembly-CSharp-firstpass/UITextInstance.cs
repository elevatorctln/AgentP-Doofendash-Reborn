using System.Collections.Generic;
using UnityEngine;

public class UITextInstance : UIObject, IPositionable
{
	private UIText _parentText;

	private string _text;

	private float _scale;

	private UITextAlignMode _alignMode;

	private UITextVerticalAlignMode _verticalAlignMode;

	public Color[] colors;

	public List<UISprite> textSprites = new List<UISprite>();

	private bool _hidden;

	public UIToolkit manager
	{
		get
		{
			return _parentText.manager;
		}
	}

	public string text
	{
		get
		{
			return _text;
		}
		set
		{
			_text = ((value.Length < 1) ? " " : value);
			if (_text.Length < textSprites.Count)
			{
				for (int num = textSprites.Count - 1; num >= _text.Length; num--)
				{
					UISprite sprite = textSprites[num];
					textSprites.RemoveAt(num);
					_parentText.manager.removeElement(sprite);
				}
			}
			_parentText.updateText(this);
			updateSize();
		}
	}

	public float textScale
	{
		get
		{
			return _scale;
		}
		set
		{
			if (_scale != value)
			{
				_scale = value;
				_parentText.updateText(this);
				updateSize();
			}
		}
	}

	public bool hidden
	{
		get
		{
			return _hidden;
		}
		set
		{
			_hidden = value;
			foreach (UISprite textSprite in textSprites)
			{
				textSprite.hidden = _hidden;
			}
		}
	}

	public float xPos
	{
		get
		{
			return position.x;
		}
		set
		{
			Vector3 vector = position;
			vector.x = value;
			position = vector;
		}
	}

	public float yPos
	{
		get
		{
			return position.y;
		}
		set
		{
			Vector3 vector = position;
			vector.y = 0f - value;
			position = vector;
		}
	}

	public int depth
	{
		get
		{
			return (int)position.z;
		}
		set
		{
			Vector3 vector = position;
			vector.z = value;
			position = vector;
		}
	}

	public UITextAlignMode alignMode
	{
		get
		{
			return _alignMode;
		}
		set
		{
			if (_alignMode != value)
			{
				_alignMode = value;
				switch (_alignMode)
				{
				case UITextAlignMode.Left:
					_anchorInfo.OriginUIxAnchor = UIxAnchor.Left;
					break;
				case UITextAlignMode.Center:
					_anchorInfo.OriginUIxAnchor = UIxAnchor.Center;
					break;
				case UITextAlignMode.Right:
					_anchorInfo.OriginUIxAnchor = UIxAnchor.Right;
					break;
				}
				if (text.Length > 0)
				{
					_parentText.updateText(this);
					updateSize();
				}
			}
		}
	}

	public UITextVerticalAlignMode verticalAlignMode
	{
		get
		{
			return _verticalAlignMode;
		}
		set
		{
			if (_verticalAlignMode != value)
			{
				_verticalAlignMode = value;
				switch (_verticalAlignMode)
				{
				case UITextVerticalAlignMode.Top:
					_anchorInfo.OriginUIyAnchor = UIyAnchor.Top;
					break;
				case UITextVerticalAlignMode.Middle:
					_anchorInfo.OriginUIyAnchor = UIyAnchor.Center;
					break;
				case UITextVerticalAlignMode.Bottom:
					_anchorInfo.OriginUIyAnchor = UIyAnchor.Bottom;
					break;
				}
				if (text.Length > 0)
				{
					_parentText.updateText(this);
					updateSize();
				}
			}
		}
	}

	public override Color color
	{
		get
		{
			return colors[0];
		}
		set
		{
			setColorForAllLetters(value);
		}
	}

	public UITextInstance(UIText parentText, string text, float xPos, float yPos, float scale, int depth, Color color)
		: this(parentText, text, xPos, yPos, scale, depth, new Color[1] { color }, parentText.alignMode, parentText.verticalAlignMode)
	{
	}

	public UITextInstance(UIText parentText, string text, float xPos, float yPos, float scale, int depth, Color[] colors, UITextAlignMode alignMode, UITextVerticalAlignMode verticalAlignMode)
	{
		_parentText = parentText;
		_text = text;
		_scale = scale;
		this.colors = colors;
		position = new Vector3(xPos, 0f - yPos, depth);
		_hidden = false;
		_alignMode = alignMode;
		switch (alignMode)
		{
		case UITextAlignMode.Left:
			_anchorInfo.OriginUIxAnchor = UIxAnchor.Left;
			break;
		case UITextAlignMode.Center:
			_anchorInfo.OriginUIxAnchor = UIxAnchor.Center;
			break;
		case UITextAlignMode.Right:
			_anchorInfo.OriginUIxAnchor = UIxAnchor.Right;
			break;
		}
		_verticalAlignMode = verticalAlignMode;
		switch (verticalAlignMode)
		{
		case UITextVerticalAlignMode.Top:
			_anchorInfo.OriginUIyAnchor = UIyAnchor.Top;
			break;
		case UITextVerticalAlignMode.Middle:
			_anchorInfo.OriginUIyAnchor = UIyAnchor.Center;
			break;
		case UITextVerticalAlignMode.Bottom:
			_anchorInfo.OriginUIyAnchor = UIyAnchor.Bottom;
			break;
		}
		_anchorInfo.OffsetX = xPos;
		_anchorInfo.OffsetY = yPos;
		updateSize();
	}

	private void updateSize()
	{
		Vector2 vector = _parentText.sizeForText(_text, _scale);
		_width = vector.x;
		_height = vector.y;
		this.refreshPosition();
	}

	private void applyColorToSprites()
	{
		int count = textSprites.Count;
		if (colors.Length == 1)
		{
			for (int i = 0; i < count; i++)
			{
				textSprites[i].color = colors[0];
			}
		}
		else
		{
			for (int j = 0; j < count; j++)
			{
				textSprites[j].color = colors[j];
			}
		}
	}

	public UISprite textSpriteAtIndex(int index)
	{
		if (textSprites.Count > index)
		{
			return textSprites[index];
		}
		return null;
	}

	public void clear()
	{
		_text = null;
		foreach (UISprite textSprite in textSprites)
		{
			_parentText.manager.removeElement(textSprite);
		}
		textSprites.Clear();
	}

	public void destroy()
	{
		clear();
		Object.Destroy(base.client);
	}

	public void setColorForAllLetters(Color color)
	{
		colors = new Color[1] { color };
		applyColorToSprites();
	}

	public void setColorPerLetter(Color[] colors)
	{
		if (colors.Length >= _text.Length)
		{
			this.colors = colors;
			applyColorToSprites();
		}
	}

	public override void transformChanged()
	{
		base.transformChanged();
		this.refreshPosition();
	}
}

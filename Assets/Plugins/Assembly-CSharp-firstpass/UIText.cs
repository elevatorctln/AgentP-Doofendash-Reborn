using System;
using System.Collections.Generic;
using UnityEngine;

public class UIText
{
	private struct UIFontCharInfo
	{
		public int charID;

		public int posX;

		public int posY;

		public int w;

		public int h;

		public int offsetx;

		public int offsety;

		public int xadvance;
	}

	public static int ASCII_NEWLINE = 10;

	public static int ASCII_SPACE = 32;

	public static int ASCII_HYPHEN_MINUS = 45;

	public static int ASCII_LINEHEIGHT_REFERENCE = 77;

	public bool forceLowAscii;

	private bool hasLowAsciiQuotes;

	public float lineSpacing = 1.2f;

	private Dictionary<int, UIFontCharInfo> _fontDetails = new Dictionary<int, UIFontCharInfo>();

	private Vector2 _textureOffset;

	private UIToolkit _manager;

	public List<UITextInstance> m_TextInstances = new List<UITextInstance>();

	public UITextAlignMode alignMode;

	public UITextVerticalAlignMode verticalAlignMode;

	public UITextLineWrapMode wrapMode;

	public float lineWrapWidth = 500f;

	public UIToolkit manager
	{
		get
		{
			return _manager;
		}
	}

	public UIText(string fontFilename, string textureFilename)
		: this(UI.firstToolkit, fontFilename, textureFilename)
	{
	}

	public UIText(UIToolkit manager, string fontFilename, string textureFilename)
	{
		_manager = manager;
		loadConfigfile(fontFilename);
		Rect rect = _manager.frameForFilename(textureFilename);
		UITextureInfo uITextureInfo = _manager.textureInfoForFilename(textureFilename);
		_textureOffset = new Vector2(rect.x - uITextureInfo.spriteSourceSize.x, rect.y - uITextureInfo.spriteSourceSize.y);
	}

	public void ReConfigureUIText(UIToolkit manager, string fontFilename, string textureFilename, List<UITextInstance> preservedList)
	{
		_manager = manager;
		if (_fontDetails.Count > 1)
		{
			_fontDetails.Clear();
		}
		loadConfigfile(fontFilename);
		Rect rect = _manager.frameForFilename(textureFilename);
		UITextureInfo uITextureInfo = _manager.textureInfoForFilename(textureFilename);
		_textureOffset = new Vector2(rect.x - uITextureInfo.spriteSourceSize.x, rect.y - uITextureInfo.spriteSourceSize.y);
		m_TextInstances = preservedList;
	}

	private void loadConfigfile(string filename)
	{
		if (UI.isHD)
		{
			filename += UI.instance.hdExtension;
		}
		TextAsset textAsset = Resources.Load(filename, typeof(TextAsset)) as TextAsset;
		if (textAsset == null)
		{
			Debug.LogError("Could not find font config file in Resources folder: " + filename);
		}
		int num = 0;
		string[] array = textAsset.text.Split(new string[1] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		foreach (string text in array)
		{
			string[] array2 = text.Split(' ');
			string[] array3 = array2;
			foreach (string text2 in array3)
			{
				UIFontCharInfo value = default(UIFontCharInfo);
				string[] array4 = text2.Split('=');
				string[] array5 = array4;
				foreach (string a in array5)
				{
					if (string.Equals(a, "id"))
					{
						string character = array4[1].Substring(0, array4[1].Length);
						forceLowAsciiChar(ref character);
						num = int.Parse(character);
						if (num == 145 || num == 146 || num == 147 || num == 148)
						{
							hasLowAsciiQuotes = true;
						}
						value.charID = 0;
						value.charID = num;
						if (!_fontDetails.ContainsKey(num))
						{
							_fontDetails.Add(num, value);
						}
						_fontDetails[num] = value;
					}
					else if (string.Equals(a, "x"))
					{
						string character2 = array4[1].Substring(0, array4[1].Length);
						forceLowAsciiChar(ref character2);
						value = _fontDetails[num];
						value.posX = 0;
						value.posX = int.Parse(character2);
						_fontDetails[num] = value;
					}
					else if (string.Equals(a, "y"))
					{
						string character3 = array4[1].Substring(0, array4[1].Length);
						forceLowAsciiChar(ref character3);
						value = _fontDetails[num];
						value.posY = 0;
						value.posY = int.Parse(character3);
						_fontDetails[num] = value;
					}
					else if (string.Equals(a, "width"))
					{
						string character4 = array4[1].Substring(0, array4[1].Length);
						forceLowAsciiChar(ref character4);
						value = _fontDetails[num];
						value.w = 0;
						value.w = int.Parse(character4);
						_fontDetails[num] = value;
					}
					else if (string.Equals(a, "height"))
					{
						string character5 = array4[1].Substring(0, array4[1].Length);
						forceLowAsciiChar(ref character5);
						value = _fontDetails[num];
						value.h = 0;
						value.h = int.Parse(character5);
						_fontDetails[num] = value;
					}
					else if (string.Equals(a, "xoffset"))
					{
						string character6 = array4[1].Substring(0, array4[1].Length);
						forceLowAsciiChar(ref character6);
						value = _fontDetails[num];
						value.offsetx = 0;
						value.offsetx = int.Parse(character6);
						_fontDetails[num] = value;
					}
					else if (string.Equals(a, "yoffset"))
					{
						string character7 = array4[1].Substring(0, array4[1].Length);
						forceLowAsciiChar(ref character7);
						value = _fontDetails[num];
						value.offsety = 0;
						value.offsety = int.Parse(character7);
						_fontDetails[num] = value;
					}
					else if (string.Equals(a, "xadvance"))
					{
						string character8 = array4[1].Substring(0, array4[1].Length);
						forceLowAsciiChar(ref character8);
						value = _fontDetails[num];
						value.xadvance = 0;
						value.xadvance = int.Parse(character8);
						_fontDetails[num] = value;
					}
				}
			}
		}
	}

	private void drawText(UITextInstance textInstance, float xPos, float yPos, float scale, int depth, Color[] color, UITextAlignMode instanceAlignMode, UITextVerticalAlignMode instanceVerticalAlignMode)
	{
		textInstance.position = Vector3.zero;
		bool hidden = textInstance.hidden;
		float num = 0f;
		float yPos2 = 0f;
		int num2 = 0;
		int num3 = 0;
		string text = textInstance.text;
		text = wrapText(text, scale);
		int lineStartChar = 0;
		int lineEndChar = 0;
		float num4 = (float)((!_fontDetails.ContainsKey(ASCII_LINEHEIGHT_REFERENCE)) ? default(UIFontCharInfo) : _fontDetails[ASCII_LINEHEIGHT_REFERENCE]).h * scale * lineSpacing;
		for (int i = 0; i < text.Length; i++)
		{
			num3 = Convert.ToInt32(text[i]);
			UIFontCharInfo uIFontCharInfo = ((!_fontDetails.ContainsKey(num3)) ? default(UIFontCharInfo) : _fontDetails[num3]);
			if (num3 == ASCII_NEWLINE)
			{
				if (_fontDetails.ContainsKey(77))
				{
					num2 += (int)((float)_fontDetails[77].h * scale * lineSpacing);
					num4 += (float)(int)((float)_fontDetails[77].h * scale * lineSpacing);
				}
				else
				{
					num2 += (int)(scale * lineSpacing);
					num4 += (float)(int)(scale * lineSpacing);
				}
				alignLine(textInstance.textSprites, lineStartChar, lineEndChar, num, instanceAlignMode);
				lineStartChar = i + 1;
				num = 0f;
			}
			else
			{
				float num5 = (float)uIFontCharInfo.offsety * scale;
				yPos2 = num5 + (float)num2;
			}
			lineEndChar = i;
			UISprite uISprite = textInstance.textSpriteAtIndex(i);
			bool flag = uISprite == null;
			uISprite = configureSpriteForCharId(uISprite, num3, num, yPos2, scale, 0);
			if (flag)
			{
				uISprite.color = ((color.Length != 1) ? color[i] : color[0]);
				uISprite.parentUIObject = textInstance;
				textInstance.textSprites.Add(uISprite);
			}
			uISprite.hidden = hidden;
			num += (float)uIFontCharInfo.xadvance * scale;
		}
		alignLine(textInstance.textSprites, lineStartChar, lineEndChar, num, instanceAlignMode);
		verticalAlignText(textInstance.textSprites, num4, (float)_fontDetails[ASCII_LINEHEIGHT_REFERENCE].offsety * scale * lineSpacing, instanceVerticalAlignMode);
		textInstance.position = new Vector3(xPos, yPos, depth);
	}

	private void alignLine(List<UISprite> sprites, int lineStartChar, int lineEndChar, float lineWidth, UITextAlignMode instanceAlignMode)
	{
		switch (instanceAlignMode)
		{
		case UITextAlignMode.Center:
		{
			for (int j = lineStartChar; j <= lineEndChar; j++)
			{
				if (sprites[j] != null)
				{
					sprites[j].position = new Vector3(sprites[j].position.x - lineWidth / 2f, sprites[j].position.y, sprites[j].position.z);
				}
			}
			break;
		}
		case UITextAlignMode.Right:
		{
			for (int i = lineStartChar; i <= lineEndChar; i++)
			{
				if (i < sprites.Count && sprites[i] != null)
				{
					sprites[i].position = new Vector3(sprites[i].position.x - lineWidth, sprites[i].position.y, sprites[i].position.z);
				}
			}
			break;
		}
		}
	}

	private void verticalAlignText(List<UISprite> sprites, float totalHeight, float charOffset, UITextVerticalAlignMode instanceVerticalAlignMode)
	{
		if (instanceVerticalAlignMode == UITextVerticalAlignMode.Top)
		{
			return;
		}
		int count = sprites.Count;
		for (int i = 0; i < count; i++)
		{
			switch (instanceVerticalAlignMode)
			{
			case UITextVerticalAlignMode.Middle:
				sprites[i].position = new Vector3(sprites[i].position.x, sprites[i].position.y + totalHeight / 2f + charOffset, sprites[i].position.z);
				break;
			case UITextVerticalAlignMode.Bottom:
				sprites[i].position = new Vector3(sprites[i].position.x, sprites[i].position.y + totalHeight + charOffset, sprites[i].position.z);
				break;
			}
		}
	}

	private string wrapText(string text, float scale)
	{
		string text2 = string.Empty;
		float num = 0f;
		int num2 = 0;
		float num3 = lineWrapWidth * (float)UI.scaleFactor;
		switch (wrapMode)
		{
		case UITextLineWrapMode.None:
			text2 = text;
			break;
		case UITextLineWrapMode.AlwaysHyphenate:
		{
			num2 = text.Length;
			for (int j = 0; j < num2; j++)
			{
				int num7 = Convert.ToInt32(text[j]);
				int xadvance = _fontDetails[num7].xadvance;
				if (num7 == ASCII_NEWLINE)
				{
					text2 += "\n";
					num = 0f;
				}
				else if (num > num3)
				{
					int num8 = ASCII_SPACE;
					if (j > 1)
					{
						num8 = text[j - 1];
					}
					text2 = ((num7 == ASCII_SPACE) ? (text2 + "\n") : ((num8 != ASCII_SPACE) ? (text2 + "-\n" + text[j]) : (text2 + "\n" + text[j])));
					num = 0f;
				}
				else
				{
					text2 += text[j];
				}
				num += (float)xadvance;
			}
			break;
		}
		case UITextLineWrapMode.MinimumLength:
		{
			string[] array = text.Split(' ');
			num2 = array.Length;
			float num4 = wordWidth(" ", scale);
			float num5 = num3;
			for (int i = 0; i < num2; i++)
			{
				float num6 = wordWidth(array[i], scale);
				if (num6 + num4 > num5)
				{
					text2 = text2 + "\n" + array[i] + " ";
					num5 = num3 - num6;
				}
				else
				{
					text2 = text2 + array[i] + " ";
					num5 -= num6 + num4;
				}
			}
			break;
		}
		}
		return text2;
	}

	private float wordWidth(string word, float scale)
	{
		float num = 0f;
		foreach (char value in word)
		{
			int key = Convert.ToInt32(value);
			num += (float)((!_fontDetails.ContainsKey(key)) ? default(UIFontCharInfo) : _fontDetails[key]).xadvance * scale;
		}
		return num;
	}

	private UISprite configureSpriteForCharId(UISprite sprite, int charId, float xPos, float yPos, float scale, int depth)
	{
		UIFontCharInfo uIFontCharInfo = ((!_fontDetails.ContainsKey(charId)) ? default(UIFontCharInfo) : _fontDetails[charId]);
		UIUVRect uvFrame = new UIUVRect((int)_textureOffset.x + uIFontCharInfo.posX, (int)_textureOffset.y + uIFontCharInfo.posY, uIFontCharInfo.w, uIFontCharInfo.h, _manager.textureSize);
		Rect frame = new Rect(xPos + (float)uIFontCharInfo.offsetx * scale, yPos, uIFontCharInfo.w, uIFontCharInfo.h);
		if (sprite == null)
		{
			sprite = new UISprite(frame, depth, uvFrame, false);
			_manager.addSprite(sprite);
		}
		else
		{
			sprite.uvFrame = uvFrame;
			sprite.position = new Vector3(frame.x, 0f - frame.y, depth);
			sprite.setSize(frame.width, frame.height);
		}
		sprite.autoRefreshPositionOnScaling = false;
		sprite.scale = new Vector3(scale, scale, 1f);
		return sprite;
	}

	public Vector2 sizeForText(string text)
	{
		return sizeForText(text, 1f);
	}

	public Vector2 sizeForText(string text, float scale)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		int num4 = 0;
		int num5 = 0;
		text = wrapText(text, scale);
		for (int i = 0; i < text.Length; i++)
		{
			num5 = Convert.ToInt32(text[i]);
			UIFontCharInfo uIFontCharInfo = ((!_fontDetails.ContainsKey(num5)) ? default(UIFontCharInfo) : _fontDetails[num5]);
			if (num5 == ASCII_NEWLINE)
			{
				num4 += (int)((float)_fontDetails[77].h * scale * lineSpacing);
				num = 0f;
			}
			else
			{
				float num6 = (float)uIFontCharInfo.offsety * scale;
				num3 = 0f + num6 + (float)num4;
			}
			num += (float)uIFontCharInfo.xadvance * scale;
			if (num2 < num)
			{
				num2 = num;
			}
		}
		return new Vector2((!(num2 > 0f)) ? num : num2, num3 + (float)_fontDetails[77].h * scale);
	}

	public UITextInstance addTextInstance(string text, float xPos, float yPos)
	{
		return addTextInstance(text, xPos, yPos, 1f, 1);
	}

	public UITextInstance addTextInstance(string text, float xPos, float yPos, float scale)
	{
		return addTextInstance(text, xPos, yPos, scale, 1);
	}

	public UITextInstance addTextInstance(string text, float xPos, float yPos, float scale, int depth)
	{
		return addTextInstance(text, xPos, yPos, scale, depth, Color.white);
	}

	public UITextInstance addTextInstance(string text, float xPos, float yPos, float scale, int depth, Color color)
	{
		return addTextInstance(text, xPos, yPos, scale, depth, color, alignMode, verticalAlignMode);
	}

	public UITextInstance addTextInstance(string text, float xPos, float yPos, float scale, int depth, Color color, UITextAlignMode alignMode, UITextVerticalAlignMode verticalAlignMode)
	{
		return addTextInstance(text, xPos, yPos, scale, depth, new Color[1] { color }, alignMode, verticalAlignMode);
	}

	public UITextInstance addTextInstance(string text, float xPos, float yPos, float scale, int depth, Color[] colors, UITextAlignMode alignMode, UITextVerticalAlignMode verticalAlignMode)
	{
		if (forceLowAscii)
		{
			forceLowAsciiString(ref text);
		}
		UITextInstance uITextInstance = new UITextInstance(this, text, xPos, yPos, scale, depth, colors, alignMode, verticalAlignMode);
		uITextInstance.parent = _manager.transform;
		drawText(uITextInstance, uITextInstance.xPos, uITextInstance.yPos, uITextInstance.textScale, uITextInstance.depth, colors, uITextInstance.alignMode, uITextInstance.verticalAlignMode);
		m_TextInstances.Add(uITextInstance);
		return uITextInstance;
	}

	public void updateAllTextInstances()
	{
		for (int i = 0; i < m_TextInstances.Count; i++)
		{
			if (m_TextInstances[i] != null)
			{
				updateText(m_TextInstances[i]);
			}
		}
	}

	public void updateText(UITextInstance textInstance)
	{
		Vector3 localScale = textInstance.localScale;
		bool flag = false;
		if (localScale.x != 1f || localScale.y != 1f)
		{
			textInstance.localScale = new Vector3(1f, 1f, localScale.z);
			flag = true;
		}
		drawText(textInstance, textInstance.xPos, textInstance.yPos, textInstance.textScale, textInstance.depth, textInstance.colors, textInstance.alignMode, textInstance.verticalAlignMode);
		if (flag)
		{
			textInstance.localScale = localScale;
		}
	}

	private void forceLowAsciiChar(ref string character)
	{
		if (character == "8211")
		{
			character = "150";
		}
		else if (character == "8212")
		{
			character = "151";
		}
		else if (character == "8216")
		{
			character = "145";
		}
		else if (character == "8217")
		{
			character = "146";
		}
		else if (character == "8220")
		{
			character = "147";
		}
		else if (character == "8221")
		{
			character = "148";
		}
	}

	private void forceLowAsciiString(ref string text)
	{
		text = text.Replace(char.ConvertFromUtf32(8211), char.ConvertFromUtf32(150));
		text = text.Replace(char.ConvertFromUtf32(8212), char.ConvertFromUtf32(151));
		if (hasLowAsciiQuotes)
		{
			text = text.Replace(char.ConvertFromUtf32(8216), char.ConvertFromUtf32(145));
			text = text.Replace(char.ConvertFromUtf32(8217), char.ConvertFromUtf32(146));
		}
		else
		{
			text = text.Replace(char.ConvertFromUtf32(8216), char.ConvertFromUtf32(39));
			text = text.Replace(char.ConvertFromUtf32(8217), char.ConvertFromUtf32(39));
		}
		if (hasLowAsciiQuotes)
		{
			text = text.Replace(char.ConvertFromUtf32(8220), char.ConvertFromUtf32(147));
			text = text.Replace(char.ConvertFromUtf32(8221), char.ConvertFromUtf32(148));
		}
		else
		{
			text = text.Replace(char.ConvertFromUtf32(8220), char.ConvertFromUtf32(34));
			text = text.Replace(char.ConvertFromUtf32(8221), char.ConvertFromUtf32(34));
		}
	}
}

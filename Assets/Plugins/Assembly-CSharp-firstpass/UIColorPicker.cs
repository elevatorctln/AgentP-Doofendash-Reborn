using UnityEngine;

public class UIColorPicker : UITouchableSprite
{
	public delegate void UIColorPickerChanged(UIColorPicker sender, Color newColor, Color oldColor);

	private Color _colorPicked = Color.white;

	private Vector2 textureCoords;

	public Color colorPicked
	{
		get
		{
			return _colorPicked;
		}
		set
		{
			if (value != _colorPicked)
			{
				_colorPicked = value;
			}
		}
	}

	public event UIColorPickerChanged onColorChangeBegan;

	public event UIColorPickerChanged onColorChange;

	public UIColorPicker(UIToolkit manager, Rect frame, int depth, UIUVRect uvFrame, Vector2 textureCoords)
		: base(frame, depth, uvFrame)
	{
		this.textureCoords = textureCoords;
		manager.addTouchableSprite(this);
	}

	public static UIColorPicker create(string filename, int xPos, int yPos, int depth)
	{
		return create(UI.firstToolkit, filename, xPos, yPos, depth);
	}

	public static UIColorPicker create(UIToolkit manager, string filename, int xPos, int yPos, int depth)
	{
		UITextureInfo uITextureInfo = manager.textureInfoForFilename(filename);
		Vector2 vector = new Vector2(uITextureInfo.frame.x, uITextureInfo.frame.y);
		Rect frame = new Rect(xPos, yPos, uITextureInfo.frame.width, uITextureInfo.frame.height);
		return new UIColorPicker(manager, frame, depth, uITextureInfo.uvRect, vector);
	}

	public override void onTouchBegan(Touch touch, Vector2 touchPos)
	{
		highlighted = true;
		Color oldColor = colorPicked;
		Vector2 touchTextureCoords = getTouchTextureCoords(touchPos);
		colorPicked = getColorForPixel((int)touchTextureCoords.x, (int)touchTextureCoords.y);
		if (this.onColorChangeBegan != null)
		{
			this.onColorChangeBegan(this, colorPicked, oldColor);
		}
	}

	public override void onTouchMoved(Touch touch, Vector2 touchPos)
	{
		Color oldColor = colorPicked;
		Vector2 touchTextureCoords = getTouchTextureCoords(touchPos);
		colorPicked = getColorForPixel((int)touchTextureCoords.x, (int)touchTextureCoords.y);
		if (this.onColorChange != null)
		{
			this.onColorChange(this, colorPicked, oldColor);
		}
	}

	private Vector2 getTouchTextureCoords(Vector2 touchPos)
	{
		float num = touchPos.x - position.x;
		num = Mathf.Clamp(num + 0.5f * width, 0f, width - 1f);
		float x = textureCoords.x + num;
		float num2 = touchPos.y - -1f * position.y;
		num2 = Mathf.Clamp(num2 + 0.5f * height, 1f, height);
		float y = manager.textureSize.y - (textureCoords.y + num2);
		return new Vector2(x, y);
	}

	public Color getColorForPixel(int xPos, int yPos)
	{
		Texture2D texture2D = manager.material.mainTexture as Texture2D;
		return texture2D.GetPixel(xPos, yPos);
	}
}

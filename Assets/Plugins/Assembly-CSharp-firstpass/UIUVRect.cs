using UnityEngine;

public struct UIUVRect
{
	public struct IntRect
	{
		public int x;

		public int y;

		public int width;

		public int height;

		public IntRect(int x, int y, int width, int height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}
	}

	public Vector2 lowerLeftUV;

	public Vector2 uvDimensions;

	public UIClippingPlane clippingPlane;

	public IntRect frame;

	private Vector2 _originalCoordinates;

	private int _originalWidth;

	public static UIUVRect zero;

	public UIUVRect(int x, int y, int width, int height, Vector2 textureSize)
	{
		_originalCoordinates.x = x;
		_originalCoordinates.y = y;
		_originalWidth = width;
		lowerLeftUV = new Vector2((float)x / textureSize.x, 1f - (float)(y + height) / textureSize.y);
		uvDimensions = new Vector2((float)width / textureSize.x, (float)height / textureSize.y);
		clippingPlane = UIClippingPlane.None;
		frame = new IntRect(x, y, width, height);
	}

	public UIUVRect rectClippedToBounds(float width, float height, UIClippingPlane clippingPlane, Vector2 textureSize)
	{
		UIUVRect result = this;
		result.clippingPlane = clippingPlane;
		switch (clippingPlane)
		{
		case UIClippingPlane.Left:
		{
			float num = (float)_originalWidth - width;
			result.lowerLeftUV = new Vector2((_originalCoordinates.x + num) / textureSize.x, 1f - (_originalCoordinates.y + height) / textureSize.y);
			result.uvDimensions = new Vector2(width / textureSize.x, height / textureSize.y);
			break;
		}
		case UIClippingPlane.Right:
			result.lowerLeftUV = new Vector2(_originalCoordinates.x / textureSize.x, 1f - (_originalCoordinates.y + height) / textureSize.y);
			result.uvDimensions = new Vector2(width / textureSize.x, height / textureSize.y);
			break;
		case UIClippingPlane.Top:
			result.uvDimensions = new Vector2(width / textureSize.x, height / textureSize.y);
			break;
		case UIClippingPlane.Bottom:
			result.lowerLeftUV = new Vector2(_originalCoordinates.x / textureSize.x, 1f - (_originalCoordinates.y + height) / textureSize.y);
			result.uvDimensions = new Vector2(width / textureSize.x, height / textureSize.y);
			break;
		}
		return result;
	}

	public float getWidth(Vector2 textureSize)
	{
		return uvDimensions.x * textureSize.x;
	}

	public float getHeight(Vector2 textureSize)
	{
		return uvDimensions.y * textureSize.y;
	}

	public void doubleForHD()
	{
		lowerLeftUV.x *= 2f;
		lowerLeftUV.y *= 2f;
		uvDimensions.x *= 2f;
		uvDimensions.y *= 2f;
	}

	public override bool Equals(object obj)
	{
		if (obj is UIUVRect && this == (UIUVRect)obj)
		{
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return lowerLeftUV.GetHashCode() ^ uvDimensions.GetHashCode();
	}

	public override string ToString()
	{
		return string.Format("x: {0}, y: {1}, w: {2}, h: {3}", lowerLeftUV.x, lowerLeftUV.y, uvDimensions.x, uvDimensions.y);
	}

	public static bool operator ==(UIUVRect lhs, UIUVRect rhs)
	{
		return lhs.lowerLeftUV == rhs.lowerLeftUV && lhs.uvDimensions == rhs.uvDimensions;
	}

	public static bool operator !=(UIUVRect lhs, UIUVRect rhs)
	{
		return lhs.lowerLeftUV != rhs.lowerLeftUV || lhs.uvDimensions != rhs.uvDimensions;
	}
}

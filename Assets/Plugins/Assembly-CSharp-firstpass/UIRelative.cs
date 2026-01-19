public static class UIRelative
{
	public static float xPercentFrom(UIxAnchor anchor, float width, float percentOffset)
	{
		float num = width * percentOffset;
		if (anchor == UIxAnchor.Right)
		{
			num = 0f - num;
		}
		return num;
	}

	public static float yPercentFrom(UIyAnchor anchor, float height, float percentOffset)
	{
		float num = height * percentOffset;
		if (anchor == UIyAnchor.Bottom)
		{
			num = 0f - num;
		}
		return num;
	}

	public static float xPercentTo(UIxAnchor anchor, float width, float offset)
	{
		if (width == 0f)
		{
			return 0f;
		}
		float num = offset / width;
		if (anchor == UIxAnchor.Right)
		{
			num = 0f - num;
		}
		return num;
	}

	public static float yPercentTo(UIyAnchor anchor, float height, float offset)
	{
		if (height == 0f)
		{
			return 0f;
		}
		float num = offset / height;
		if (anchor == UIyAnchor.Bottom)
		{
			num = 0f - num;
		}
		return num;
	}

	public static float xPixelsFrom(UIxAnchor anchor, float pixelOffset)
	{
		float num = pixelOffset * (float)UI.scaleFactor;
		if (anchor == UIxAnchor.Right)
		{
			num = 0f - num;
		}
		return num;
	}

	public static float yPixelsFrom(UIyAnchor anchor, float pixelOffset)
	{
		float num = pixelOffset * (float)UI.scaleFactor;
		if (anchor == UIyAnchor.Bottom)
		{
			num = 0f - num;
		}
		return num;
	}

	public static float xPixelsTo(UIxAnchor anchor, float offset)
	{
		float num = offset / (float)UI.scaleFactor;
		if (anchor == UIxAnchor.Right)
		{
			num = 0f - num;
		}
		return num;
	}

	public static float yPixelsTo(UIyAnchor anchor, float offset)
	{
		float num = offset / (float)UI.scaleFactor;
		if (anchor == UIyAnchor.Bottom)
		{
			num = 0f - num;
		}
		return num;
	}

	public static float xAnchorAdjustment(UIxAnchor anchor, float width, UIxAnchor originAnchor)
	{
		float num = 0f;
		switch (anchor)
		{
		case UIxAnchor.Left:
			switch (originAnchor)
			{
			case UIxAnchor.Center:
				num -= width / 2f;
				break;
			case UIxAnchor.Right:
				num -= width;
				break;
			}
			break;
		case UIxAnchor.Right:
			switch (originAnchor)
			{
			case UIxAnchor.Left:
				num += width;
				break;
			case UIxAnchor.Center:
				num += width / 2f;
				break;
			}
			break;
		case UIxAnchor.Center:
			switch (originAnchor)
			{
			case UIxAnchor.Left:
				num += width / 2f;
				break;
			case UIxAnchor.Right:
				num -= width / 2f;
				break;
			}
			break;
		}
		return num;
	}

	public static float yAnchorAdjustment(UIyAnchor anchor, float height, UIyAnchor originAnchor)
	{
		float num = 0f;
		switch (anchor)
		{
		case UIyAnchor.Top:
			switch (originAnchor)
			{
			case UIyAnchor.Center:
				num -= height / 2f;
				break;
			case UIyAnchor.Bottom:
				num -= height;
				break;
			}
			break;
		case UIyAnchor.Bottom:
			switch (originAnchor)
			{
			case UIyAnchor.Top:
				num += height;
				break;
			case UIyAnchor.Center:
				num += height / 2f;
				break;
			}
			break;
		case UIyAnchor.Center:
			switch (originAnchor)
			{
			case UIyAnchor.Top:
				num += height / 2f;
				break;
			case UIyAnchor.Bottom:
				num -= height / 2f;
				break;
			}
			break;
		}
		return num;
	}
}

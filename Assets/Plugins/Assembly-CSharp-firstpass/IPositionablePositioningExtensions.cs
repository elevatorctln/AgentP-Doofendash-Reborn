using UnityEngine;

public static class IPositionablePositioningExtensions
{
	public static void positionFromCenter(this IPositionable sprite, float percentFromTop, float percentFromLeft)
	{
		sprite.positionFromCenter(percentFromTop, percentFromLeft, UIyAnchor.Center, UIxAnchor.Center);
	}

	public static void positionFromCenter(this IPositionable sprite, float percentFromTop, float percentFromLeft, UIyAnchor yAnchor, UIxAnchor xAnchor)
	{
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		anchorInfo.ParentUIxAnchor = UIxAnchor.Center;
		anchorInfo.ParentUIyAnchor = UIyAnchor.Center;
		anchorInfo.UIxAnchor = xAnchor;
		anchorInfo.UIyAnchor = yAnchor;
		anchorInfo.OffsetX = percentFromLeft;
		anchorInfo.OffsetY = percentFromTop;
		anchorInfo.UIPrecision = UIPrecision.Percentage;
		sprite.anchorInfo = anchorInfo;
		sprite.refreshPosition();
	}

	public static void positionFromTopLeft(this IPositionable sprite, float percentFromTop, float percentFromLeft)
	{
		sprite.positionFromTopLeft(percentFromTop, percentFromLeft, UIyAnchor.Top, UIxAnchor.Left);
	}

	public static void positionFromTopLeft(this IPositionable sprite, float percentFromTop, float percentFromLeft, UIyAnchor yAnchor, UIxAnchor xAnchor)
	{
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		anchorInfo.ParentUIxAnchor = UIxAnchor.Left;
		anchorInfo.ParentUIyAnchor = UIyAnchor.Top;
		anchorInfo.UIxAnchor = xAnchor;
		anchorInfo.UIyAnchor = yAnchor;
		anchorInfo.OffsetX = percentFromLeft;
		anchorInfo.OffsetY = percentFromTop;
		anchorInfo.UIPrecision = UIPrecision.Percentage;
		sprite.anchorInfo = anchorInfo;
		sprite.refreshPosition();
	}

	public static void positionFromTopRight(this IPositionable sprite, float percentFromTop, float percentFromRight)
	{
		sprite.positionFromTopRight(percentFromTop, percentFromRight, UIyAnchor.Top, UIxAnchor.Right);
	}

	public static void positionFromTopRight(this IPositionable sprite, float percentFromTop, float percentFromRight, UIyAnchor yAnchor, UIxAnchor xAnchor)
	{
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		anchorInfo.ParentUIxAnchor = UIxAnchor.Right;
		anchorInfo.ParentUIyAnchor = UIyAnchor.Top;
		anchorInfo.UIxAnchor = xAnchor;
		anchorInfo.UIyAnchor = yAnchor;
		anchorInfo.OffsetX = percentFromRight;
		anchorInfo.OffsetY = percentFromTop;
		anchorInfo.UIPrecision = UIPrecision.Percentage;
		sprite.anchorInfo = anchorInfo;
		sprite.refreshPosition();
	}

	public static void positionFromBottomLeft(this IPositionable sprite, float percentFromBottom, float percentFromLeft)
	{
		sprite.positionFromBottomLeft(percentFromBottom, percentFromLeft, UIyAnchor.Bottom, UIxAnchor.Left);
	}

	public static void positionFromBottomLeft(this IPositionable sprite, float percentFromBottom, float percentFromLeft, UIyAnchor yAnchor, UIxAnchor xAnchor)
	{
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		anchorInfo.ParentUIxAnchor = UIxAnchor.Left;
		anchorInfo.ParentUIyAnchor = UIyAnchor.Bottom;
		anchorInfo.UIxAnchor = xAnchor;
		anchorInfo.UIyAnchor = yAnchor;
		anchorInfo.OffsetX = percentFromLeft;
		anchorInfo.OffsetY = percentFromBottom;
		anchorInfo.UIPrecision = UIPrecision.Percentage;
		sprite.anchorInfo = anchorInfo;
		sprite.refreshPosition();
	}

	public static void positionFromBottomRight(this IPositionable sprite, float percentFromBottom, float percentFromRight)
	{
		sprite.positionFromBottomRight(percentFromBottom, percentFromRight, UIyAnchor.Bottom, UIxAnchor.Right);
	}

	public static void positionFromBottomRight(this IPositionable sprite, float percentFromBottom, float percentFromRight, UIyAnchor yAnchor, UIxAnchor xAnchor)
	{
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		anchorInfo.ParentUIxAnchor = UIxAnchor.Right;
		anchorInfo.ParentUIyAnchor = UIyAnchor.Bottom;
		anchorInfo.UIxAnchor = xAnchor;
		anchorInfo.UIyAnchor = yAnchor;
		anchorInfo.OffsetX = percentFromRight;
		anchorInfo.OffsetY = percentFromBottom;
		anchorInfo.UIPrecision = UIPrecision.Percentage;
		sprite.anchorInfo = anchorInfo;
		sprite.refreshPosition();
	}

	public static void positionFromTop(this IPositionable sprite, float percentFromTop)
	{
		sprite.positionFromTop(percentFromTop, 0f, UIyAnchor.Top, UIxAnchor.Center);
	}

	public static void positionFromTop(this IPositionable sprite, float percentFromTop, float percentFromLeft)
	{
		sprite.positionFromTop(percentFromTop, percentFromLeft, UIyAnchor.Top, UIxAnchor.Center);
	}

	public static void positionFromTop(this IPositionable sprite, float percentFromTop, float percentFromLeft, UIyAnchor yAnchor, UIxAnchor xAnchor)
	{
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		anchorInfo.ParentUIxAnchor = UIxAnchor.Center;
		anchorInfo.ParentUIyAnchor = UIyAnchor.Top;
		anchorInfo.UIxAnchor = xAnchor;
		anchorInfo.UIyAnchor = yAnchor;
		anchorInfo.OffsetX = percentFromLeft;
		anchorInfo.OffsetY = percentFromTop;
		anchorInfo.UIPrecision = UIPrecision.Percentage;
		sprite.anchorInfo = anchorInfo;
		sprite.refreshPosition();
	}

	public static void positionFromBottom(this IPositionable sprite, float percentFromBottom)
	{
		sprite.positionFromBottom(percentFromBottom, 0f, UIyAnchor.Bottom, UIxAnchor.Center);
	}

	public static void positionFromBottom(this IPositionable sprite, float percentFromBottom, float percentFromLeft)
	{
		sprite.positionFromBottom(percentFromBottom, percentFromLeft, UIyAnchor.Bottom, UIxAnchor.Center);
	}

	public static void positionFromBottom(this IPositionable sprite, float percentFromBottom, float percentFromLeft, UIyAnchor yAnchor, UIxAnchor xAnchor)
	{
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		anchorInfo.ParentUIxAnchor = UIxAnchor.Center;
		anchorInfo.ParentUIyAnchor = UIyAnchor.Bottom;
		anchorInfo.UIxAnchor = xAnchor;
		anchorInfo.UIyAnchor = yAnchor;
		anchorInfo.OffsetX = percentFromLeft;
		anchorInfo.OffsetY = percentFromBottom;
		anchorInfo.UIPrecision = UIPrecision.Percentage;
		sprite.anchorInfo = anchorInfo;
		sprite.refreshPosition();
	}

	public static void positionFromLeft(this IPositionable sprite, float percentFromLeft)
	{
		sprite.positionFromLeft(0f, percentFromLeft, UIyAnchor.Center, UIxAnchor.Left);
	}

	public static void positionFromLeft(this IPositionable sprite, float percentFromTop, float percentFromLeft)
	{
		sprite.positionFromLeft(percentFromTop, percentFromLeft, UIyAnchor.Center, UIxAnchor.Left);
	}

	public static void positionFromLeft(this IPositionable sprite, float percentFromTop, float percentFromLeft, UIyAnchor yAnchor, UIxAnchor xAnchor)
	{
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		anchorInfo.ParentUIxAnchor = UIxAnchor.Left;
		anchorInfo.ParentUIyAnchor = UIyAnchor.Center;
		anchorInfo.UIxAnchor = xAnchor;
		anchorInfo.UIyAnchor = yAnchor;
		anchorInfo.OffsetX = percentFromLeft;
		anchorInfo.OffsetY = percentFromTop;
		anchorInfo.UIPrecision = UIPrecision.Percentage;
		sprite.anchorInfo = anchorInfo;
		sprite.refreshPosition();
	}

	public static void positionFromRight(this IPositionable sprite, float percentFromRight)
	{
		sprite.positionFromRight(0f, percentFromRight, UIyAnchor.Center, UIxAnchor.Right);
	}

	public static void positionFromRight(this IPositionable sprite, float percentFromTop, float percentFromRight)
	{
		sprite.positionFromRight(percentFromTop, percentFromRight, UIyAnchor.Center, UIxAnchor.Right);
	}

	public static void positionFromRight(this IPositionable sprite, float percentFromTop, float percentFromRight, UIyAnchor yAnchor, UIxAnchor xAnchor)
	{
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		anchorInfo.ParentUIxAnchor = UIxAnchor.Right;
		anchorInfo.ParentUIyAnchor = UIyAnchor.Center;
		anchorInfo.UIxAnchor = xAnchor;
		anchorInfo.UIyAnchor = yAnchor;
		anchorInfo.OffsetX = percentFromRight;
		anchorInfo.OffsetY = percentFromTop;
		anchorInfo.UIPrecision = UIPrecision.Percentage;
		sprite.anchorInfo = anchorInfo;
		sprite.refreshPosition();
	}

	public static void positionCenter(this IPositionable sprite)
	{
		sprite.pixelsFromCenter(0, 0);
	}

	public static void pixelsFromCenter(this IPositionable sprite, int pixelsFromTop, int pixelsFromLeft)
	{
		sprite.pixelsFromCenter(pixelsFromTop, pixelsFromLeft, UIyAnchor.Center, UIxAnchor.Center);
	}

	public static void pixelsFromCenter(this IPositionable sprite, int pixelsFromTop, int pixelsFromLeft, UIyAnchor yAnchor, UIxAnchor xAnchor)
	{
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		anchorInfo.ParentUIxAnchor = UIxAnchor.Center;
		anchorInfo.ParentUIyAnchor = UIyAnchor.Center;
		anchorInfo.UIxAnchor = xAnchor;
		anchorInfo.UIyAnchor = yAnchor;
		anchorInfo.OffsetX = pixelsFromLeft;
		anchorInfo.OffsetY = pixelsFromTop;
		anchorInfo.UIPrecision = UIPrecision.Pixel;
		sprite.anchorInfo = anchorInfo;
		sprite.refreshPosition();
	}

	public static void pixelsFromTopLeft(this IPositionable sprite, int pixelsFromTop, int pixelsFromLeft)
	{
		sprite.pixelsFromTopLeft(pixelsFromTop, pixelsFromLeft, UIyAnchor.Top, UIxAnchor.Left);
	}

	public static void pixelsFromTopLeft(this IPositionable sprite, int pixelsFromTop, int pixelsFromLeft, UIyAnchor yAnchor, UIxAnchor xAnchor)
	{
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		anchorInfo.ParentUIxAnchor = UIxAnchor.Left;
		anchorInfo.ParentUIyAnchor = UIyAnchor.Top;
		anchorInfo.UIxAnchor = xAnchor;
		anchorInfo.UIyAnchor = yAnchor;
		anchorInfo.OffsetX = pixelsFromLeft;
		anchorInfo.OffsetY = pixelsFromTop;
		anchorInfo.UIPrecision = UIPrecision.Pixel;
		sprite.anchorInfo = anchorInfo;
		sprite.refreshPosition();
	}

	public static void pixelsFromTopRight(this IPositionable sprite, int pixelsFromTop, int pixelsFromRight)
	{
		sprite.pixelsFromTopRight(pixelsFromTop, pixelsFromRight, UIyAnchor.Top, UIxAnchor.Right);
	}

	public static void pixelsFromTopRight(this IPositionable sprite, int pixelsFromTop, int pixelsFromRight, UIyAnchor yAnchor, UIxAnchor xAnchor)
	{
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		anchorInfo.ParentUIxAnchor = UIxAnchor.Right;
		anchorInfo.ParentUIyAnchor = UIyAnchor.Top;
		anchorInfo.UIxAnchor = xAnchor;
		anchorInfo.UIyAnchor = yAnchor;
		anchorInfo.OffsetX = pixelsFromRight;
		anchorInfo.OffsetY = pixelsFromTop;
		anchorInfo.UIPrecision = UIPrecision.Pixel;
		sprite.anchorInfo = anchorInfo;
		sprite.refreshPosition();
	}

	public static void pixelsFromBottomLeft(this IPositionable sprite, int pixelsFromBottom, int pixelsFromLeft)
	{
		sprite.pixelsFromBottomLeft(pixelsFromBottom, pixelsFromLeft, UIyAnchor.Bottom, UIxAnchor.Left);
	}

	public static void pixelsFromBottomLeft(this IPositionable sprite, int pixelsFromBottom, int pixelsFromLeft, UIyAnchor yAnchor, UIxAnchor xAnchor)
	{
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		anchorInfo.ParentUIxAnchor = UIxAnchor.Left;
		anchorInfo.ParentUIyAnchor = UIyAnchor.Bottom;
		anchorInfo.UIxAnchor = xAnchor;
		anchorInfo.UIyAnchor = yAnchor;
		anchorInfo.OffsetX = pixelsFromLeft;
		anchorInfo.OffsetY = pixelsFromBottom;
		anchorInfo.UIPrecision = UIPrecision.Pixel;
		sprite.anchorInfo = anchorInfo;
		sprite.refreshPosition();
	}

	public static void pixelsFromBottomRight(this IPositionable sprite, int pixelsFromBottom, int pixelsFromRight)
	{
		sprite.pixelsFromBottomRight(pixelsFromBottom, pixelsFromRight, UIyAnchor.Bottom, UIxAnchor.Right);
	}

	public static void pixelsFromBottomRight(this IPositionable sprite, int pixelsFromBottom, int pixelsFromRight, UIyAnchor yAnchor, UIxAnchor xAnchor)
	{
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		anchorInfo.ParentUIxAnchor = UIxAnchor.Right;
		anchorInfo.ParentUIyAnchor = UIyAnchor.Bottom;
		anchorInfo.UIxAnchor = xAnchor;
		anchorInfo.UIyAnchor = yAnchor;
		anchorInfo.OffsetX = pixelsFromRight;
		anchorInfo.OffsetY = pixelsFromBottom;
		anchorInfo.UIPrecision = UIPrecision.Pixel;
		sprite.anchorInfo = anchorInfo;
		sprite.refreshPosition();
	}

	public static void pixelsFromTop(this IPositionable sprite, int pixelsFromTop)
	{
		sprite.pixelsFromTop(pixelsFromTop, 0, UIyAnchor.Top, UIxAnchor.Center);
	}

	public static void pixelsFromTop(this IPositionable sprite, int pixelsFromTop, int pixelsFromLeft)
	{
		sprite.pixelsFromTop(pixelsFromTop, pixelsFromLeft, UIyAnchor.Top, UIxAnchor.Center);
	}

	public static void pixelsFromTop(this IPositionable sprite, int pixelsFromTop, int pixelsFromLeft, UIyAnchor yAnchor, UIxAnchor xAnchor)
	{
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		anchorInfo.ParentUIxAnchor = UIxAnchor.Center;
		anchorInfo.ParentUIyAnchor = UIyAnchor.Top;
		anchorInfo.UIxAnchor = xAnchor;
		anchorInfo.UIyAnchor = yAnchor;
		anchorInfo.OffsetX = pixelsFromLeft;
		anchorInfo.OffsetY = pixelsFromTop;
		anchorInfo.UIPrecision = UIPrecision.Pixel;
		sprite.anchorInfo = anchorInfo;
		sprite.refreshPosition();
	}

	public static void pixelsFromBottom(this IPositionable sprite, int pixelsFromBottom)
	{
		sprite.pixelsFromBottom(pixelsFromBottom, 0, UIyAnchor.Bottom, UIxAnchor.Center);
	}

	public static void pixelsFromBottom(this IPositionable sprite, int pixelsFromBottom, int pixelsFromLeft)
	{
		sprite.pixelsFromBottom(pixelsFromBottom, pixelsFromLeft, UIyAnchor.Bottom, UIxAnchor.Center);
	}

	public static void pixelsFromBottom(this IPositionable sprite, int pixelsFromBottom, int pixelsFromLeft, UIyAnchor yAnchor, UIxAnchor xAnchor)
	{
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		anchorInfo.ParentUIxAnchor = UIxAnchor.Center;
		anchorInfo.ParentUIyAnchor = UIyAnchor.Bottom;
		anchorInfo.UIxAnchor = xAnchor;
		anchorInfo.UIyAnchor = yAnchor;
		anchorInfo.OffsetX = pixelsFromLeft;
		anchorInfo.OffsetY = pixelsFromBottom;
		anchorInfo.UIPrecision = UIPrecision.Pixel;
		sprite.anchorInfo = anchorInfo;
		sprite.refreshPosition();
	}

	public static void pixelsFromLeft(this IPositionable sprite, int pixelsFromLeft)
	{
		sprite.pixelsFromLeft(0, pixelsFromLeft, UIyAnchor.Center, UIxAnchor.Left);
	}

	public static void pixelsFromLeft(this IPositionable sprite, int pixelsFromTop, int pixelsFromLeft)
	{
		sprite.pixelsFromLeft(pixelsFromTop, pixelsFromLeft, UIyAnchor.Center, UIxAnchor.Left);
	}

	public static void pixelsFromLeft(this IPositionable sprite, int pixelsFromTop, int pixelsFromLeft, UIyAnchor yAnchor, UIxAnchor xAnchor)
	{
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		anchorInfo.ParentUIxAnchor = UIxAnchor.Left;
		anchorInfo.ParentUIyAnchor = UIyAnchor.Center;
		anchorInfo.UIxAnchor = xAnchor;
		anchorInfo.UIyAnchor = yAnchor;
		anchorInfo.OffsetX = pixelsFromLeft;
		anchorInfo.OffsetY = pixelsFromTop;
		anchorInfo.UIPrecision = UIPrecision.Pixel;
		sprite.anchorInfo = anchorInfo;
		sprite.refreshPosition();
	}

	public static void pixelsFromRight(this IPositionable sprite, int pixelsFromRight)
	{
		sprite.pixelsFromRight(0, pixelsFromRight, UIyAnchor.Center, UIxAnchor.Right);
	}

	public static void pixelsFromRight(this IPositionable sprite, int pixelsFromTop, int pixelsFromRight)
	{
		sprite.pixelsFromRight(pixelsFromTop, pixelsFromRight, UIyAnchor.Center, UIxAnchor.Right);
	}

	public static void pixelsFromRight(this IPositionable sprite, int pixelsFromTop, int pixelsFromRight, UIyAnchor yAnchor, UIxAnchor xAnchor)
	{
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		anchorInfo.ParentUIxAnchor = UIxAnchor.Right;
		anchorInfo.ParentUIyAnchor = UIyAnchor.Center;
		anchorInfo.UIxAnchor = xAnchor;
		anchorInfo.UIyAnchor = yAnchor;
		anchorInfo.OffsetX = pixelsFromRight;
		anchorInfo.OffsetY = pixelsFromTop;
		anchorInfo.UIPrecision = UIPrecision.Pixel;
		sprite.anchorInfo = anchorInfo;
		sprite.refreshPosition();
	}

	public static void refreshAnchorInformation(this IPositionable sprite)
	{
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		Vector3 vector = parentAnchorPosition(anchorInfo.ParentUIObject, anchorInfo.ParentUIyAnchor, anchorInfo.ParentUIxAnchor);
		Vector3 vector2 = sprite.position - vector;
		vector2.x += UIRelative.xAnchorAdjustment(anchorInfo.UIxAnchor, sprite.width, anchorInfo.OriginUIxAnchor);
		vector2.y -= UIRelative.yAnchorAdjustment(anchorInfo.UIyAnchor, sprite.height, anchorInfo.OriginUIyAnchor);
		if (anchorInfo.UIPrecision == UIPrecision.Percentage)
		{
			anchorInfo.OffsetX = UIRelative.xPercentTo(anchorInfo.UIxAnchor, parentWidth(anchorInfo.ParentUIObject), vector2.x);
			anchorInfo.OffsetY = 0f - UIRelative.yPercentTo(anchorInfo.UIyAnchor, parentHeight(anchorInfo.ParentUIObject), vector2.y);
		}
		else
		{
			anchorInfo.OffsetX = UIRelative.xPixelsTo(anchorInfo.UIxAnchor, vector2.x);
			anchorInfo.OffsetY = 0f - UIRelative.yPixelsTo(anchorInfo.UIyAnchor, vector2.y);
		}
		sprite.anchorInfo = anchorInfo;
	}

	public static void refreshPosition(this IPositionable sprite)
	{
		float z = sprite.position.z;
		UIAnchorInfo anchorInfo = sprite.anchorInfo;
		Vector3 position = parentAnchorPosition(anchorInfo.ParentUIObject, anchorInfo.ParentUIyAnchor, anchorInfo.ParentUIxAnchor);
		if (anchorInfo.UIPrecision == UIPrecision.Percentage)
		{
			position.x += UIRelative.xPercentFrom(anchorInfo.UIxAnchor, parentWidth(anchorInfo.ParentUIObject), anchorInfo.OffsetX);
			position.y -= UIRelative.yPercentFrom(anchorInfo.UIyAnchor, parentHeight(anchorInfo.ParentUIObject), anchorInfo.OffsetY);
		}
		else
		{
			position.x += UIRelative.xPixelsFrom(anchorInfo.UIxAnchor, anchorInfo.OffsetX);
			position.y -= UIRelative.yPixelsFrom(anchorInfo.UIyAnchor, anchorInfo.OffsetY);
		}
		position.x -= UIRelative.xAnchorAdjustment(anchorInfo.UIxAnchor, sprite.width, anchorInfo.OriginUIxAnchor);
		position.y += UIRelative.yAnchorAdjustment(anchorInfo.UIyAnchor, sprite.height, anchorInfo.OriginUIyAnchor);
		position.z = z;
		sprite.position = position;
	}

	private static Vector3 parentAnchorPosition(IPositionable sprite, UIyAnchor yAnchor, UIxAnchor xAnchor)
	{
		UIxAnchor originAnchor = UIxAnchor.Left;
		UIyAnchor originAnchor2 = UIyAnchor.Top;
		Vector3 result;
		float width;
		float height;
		if (sprite == null)
		{
			result = Vector3.zero;
			width = Screen.width;
			height = Screen.height;
		}
		else
		{
			result = sprite.position;
			width = sprite.width;
			height = sprite.height;
			originAnchor = sprite.anchorInfo.OriginUIxAnchor;
			originAnchor2 = sprite.anchorInfo.OriginUIyAnchor;
		}
		result.x += UIRelative.xAnchorAdjustment(xAnchor, width, originAnchor);
		result.y -= UIRelative.yAnchorAdjustment(yAnchor, height, originAnchor2);
		return result;
	}

	private static float parentWidth(IPositionable sprite)
	{
		return (sprite != null) ? sprite.width : ((float)Screen.width);
	}

	private static float parentHeight(IPositionable sprite)
	{
		return (sprite != null) ? sprite.height : ((float)Screen.height);
	}
}

public struct UIAnchorInfo
{
	public IPositionable ParentUIObject;

	public UIxAnchor ParentUIxAnchor;

	public UIyAnchor ParentUIyAnchor;

	public UIxAnchor UIxAnchor;

	public UIyAnchor UIyAnchor;

	public UIxAnchor OriginUIxAnchor;

	public UIyAnchor OriginUIyAnchor;

	public UIPrecision UIPrecision;

	public float OffsetX;

	public float OffsetY;

	public static UIAnchorInfo DefaultAnchorInfo()
	{
		return new UIAnchorInfo
		{
			UIPrecision = UIPrecision.Pixel
		};
	}
}

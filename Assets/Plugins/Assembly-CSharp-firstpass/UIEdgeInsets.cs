public struct UIEdgeInsets
{
	public int top;

	public int left;

	public int bottom;

	public int right;

	public UIEdgeInsets(int insetForAllSides)
		: this(insetForAllSides, insetForAllSides, insetForAllSides, insetForAllSides)
	{
	}

	public UIEdgeInsets(int top, int left, int bottom, int right)
	{
		this.top = top * UI.scaleFactor;
		this.left = left * UI.scaleFactor;
		this.bottom = bottom * UI.scaleFactor;
		this.right = right * UI.scaleFactor;
	}
}

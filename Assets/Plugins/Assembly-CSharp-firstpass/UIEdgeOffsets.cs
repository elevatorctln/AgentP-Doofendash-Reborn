using UnityEngine;

public struct UIEdgeOffsets
{
	public int top;

	public int left;

	public int bottom;

	public int right;

	public UIEdgeOffsets(int offsetForAllSides)
		: this(offsetForAllSides, offsetForAllSides, offsetForAllSides, offsetForAllSides)
	{
	}

	public UIEdgeOffsets(int top, int left, int bottom, int right)
	{
		this.top = top * UI.scaleFactor;
		this.left = left * UI.scaleFactor;
		this.bottom = bottom * UI.scaleFactor;
		this.right = right * UI.scaleFactor;
	}

	public Rect addToRect(Rect frame)
	{
		return new Rect(Mathf.Clamp(frame.x - (float)left, 0f, Screen.width), Mathf.Clamp(frame.y - (float)top, 0f, Screen.height), frame.width + (float)right + (float)left, frame.height + (float)bottom + (float)top);
	}
}

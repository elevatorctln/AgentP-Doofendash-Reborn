using UnityEngine;

internal struct UIBoundary
{
	public float minX;

	public float maxX;

	public float minY;

	public float maxY;

	public static UIBoundary boundaryFromPoint(Vector2 point, float maxDistance)
	{
		return new UIBoundary
		{
			minX = point.x - maxDistance,
			maxX = point.x + maxDistance,
			minY = point.y - maxDistance,
			maxY = point.y + maxDistance
		};
	}
}

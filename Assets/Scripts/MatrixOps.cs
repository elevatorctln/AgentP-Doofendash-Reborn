using UnityEngine;

public class MatrixOps
{
	public static float Trace(ref Matrix4x4 matrix)
	{
		return matrix.m00 + matrix.m11 + matrix.m22 + matrix.m33;
	}
}

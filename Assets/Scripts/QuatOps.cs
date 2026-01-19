using UnityEngine;

public class QuatOps
{
	private static int[] s_iNext = new int[3] { 1, 2, 0 };

	public static Quaternion LookAtQuat(Vector3 lookAt, Vector3 lookFrom)
	{
		Matrix4x4 kRot = default(Matrix4x4);
		Vector4 vector = lookAt - lookFrom;
		vector.Normalize();
		Vector4 vector2 = Vector3.Cross(Vector3.up, vector);
		vector2.Normalize();
		Vector4 vector3 = Vector3.Cross(vector, vector2);
		kRot.m00 = vector2.x;
		kRot.m10 = vector2.y;
		kRot.m20 = vector2.z;
		kRot.m01 = vector3.x;
		kRot.m11 = vector3.y;
		kRot.m21 = vector3.z;
		kRot.m02 = vector.x;
		kRot.m12 = vector.y;
		kRot.m22 = vector.z;
		return MatrixToQuat(kRot);
	}

	public static Quaternion QuatFromForwardVec(Vector3 forward)
	{
		Matrix4x4 kRot = default(Matrix4x4);
		forward.Normalize();
		Vector4 vector = Vector3.Cross(Vector3.up, forward);
		vector.Normalize();
		Vector4 vector2 = Vector3.Cross(forward, vector);
		kRot.m00 = vector.x;
		kRot.m10 = vector.y;
		kRot.m20 = vector.z;
		kRot.m01 = vector2.x;
		kRot.m11 = vector2.y;
		kRot.m21 = vector2.z;
		kRot.m02 = forward.x;
		kRot.m12 = forward.y;
		kRot.m22 = forward.z;
		return MatrixToQuat(kRot);
	}

	public static Quaternion QuatFromForwardVecXZ(Vector3 forward)
	{
		Matrix4x4 kRot = default(Matrix4x4);
		forward.y = 0f;
		forward.Normalize();
		Vector4 vector = Vector3.Cross(Vector3.up, forward);
		vector.Normalize();
		Vector4 vector2 = Vector3.Cross(forward, vector);
		kRot.m00 = vector.x;
		kRot.m10 = vector.y;
		kRot.m20 = vector.z;
		kRot.m01 = vector2.x;
		kRot.m11 = vector2.y;
		kRot.m21 = vector2.z;
		kRot.m02 = forward.x;
		kRot.m12 = forward.y;
		kRot.m22 = forward.z;
		return MatrixToQuat(kRot);
	}

	public static Quaternion MatrixToQuat(Matrix4x4 kRot)
	{
		Quaternion result = default(Quaternion);
		float num = kRot.m00 + kRot.m11 + kRot.m22;
		if ((double)num > 0.0)
		{
			float num2 = Mathf.Sqrt(num + 1f);
			result.w = 0.5f * num2;
			num2 = 0.5f / num2;
			result.x = (kRot.m21 - kRot.m12) * num2;
			result.y = (kRot.m02 - kRot.m20) * num2;
			result.z = (kRot.m10 - kRot.m01) * num2;
		}
		else
		{
			int num3 = 0;
			if (kRot[1, 1] > kRot[0, 0])
			{
				num3 = 1;
			}
			if (kRot[2, 2] > kRot[num3, num3])
			{
				num3 = 2;
			}
			int num4 = s_iNext[num3];
			int num5 = s_iNext[num4];
			float num2 = Mathf.Sqrt(kRot[num3, num3] - kRot[num4, num4] - kRot[num5, num5] + 1f);
			result[num3] = 0.5f * num2;
			num2 = 0.5f / num2;
			result.w = (kRot[num5, num4] - kRot[num4, num5]) * num2;
			result[num4] = (kRot[num4, num3] + kRot[num3, num4]) * num2;
			result[num5] = (kRot[num5, num3] + kRot[num3, num5]) * num2;
		}
		return result;
	}

	public static float Norm(ref Quaternion quat)
	{
		return quat.x * quat.x + quat.y * quat.y + quat.z * quat.z + quat.w * quat.w;
	}

	public static float Modulus(ref Quaternion quat)
	{
		return Mathf.Sqrt(Norm(ref quat));
	}

	public static float InverseModulus(ref Quaternion quat)
	{
		return 1f / Mathf.Sqrt(Norm(ref quat));
	}

	public static Quaternion Normalize(ref Quaternion quat)
	{
		float num = InverseModulus(ref quat);
		quat.x *= num;
		quat.y *= num;
		quat.z *= num;
		quat.w *= num;
		return quat;
	}

	public static bool AboutEqual(ref Quaternion a, ref Quaternion b)
	{
		if (!MathOps.AboutEqual(a.w, b.w))
		{
			return false;
		}
		if (!MathOps.AboutEqual(a.x, b.x))
		{
			return false;
		}
		if (!MathOps.AboutEqual(a.y, b.y))
		{
			return false;
		}
		if (!MathOps.AboutEqual(a.z, b.z))
		{
			return false;
		}
		return true;
	}

	public static bool AboutEqual(ref Quaternion a, ref Quaternion b, float epsilon)
	{
		if (!MathOps.AboutEqual(a.w, b.w, epsilon))
		{
			return false;
		}
		if (!MathOps.AboutEqual(a.x, b.x, epsilon))
		{
			return false;
		}
		if (!MathOps.AboutEqual(a.y, b.y, epsilon))
		{
			return false;
		}
		if (!MathOps.AboutEqual(a.z, b.z, epsilon))
		{
			return false;
		}
		return true;
	}
}

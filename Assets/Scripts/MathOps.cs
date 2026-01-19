using UnityEngine;

public class MathOps
{
	public static float ms_Epsilon = 1E-05f;

	public static bool AboutEqual(float valA, float valB)
	{
		return Mathf.Abs(valA - valB) < ms_Epsilon;
	}

	public static bool AboutEqual(float valA, float valB, float epsilon)
	{
		return Mathf.Abs(valA - valB) < epsilon;
	}

	public static bool AboutEqual(double valA, double valB)
	{
		return Mathf.Abs((float)(valA - valB)) < ms_Epsilon;
	}

	public static bool AboutEqual(Vector3 vecA, Vector3 vecB)
	{
		if (!AboutEqual(vecA.x, vecB.x))
		{
			return false;
		}
		if (!AboutEqual(vecA.y, vecB.y))
		{
			return false;
		}
		if (!AboutEqual(vecA.z, vecB.z))
		{
			return false;
		}
		return true;
	}

	public static float Clamp(float val, float min, float max)
	{
		if (val < min)
		{
			return min;
		}
		if (val > max)
		{
			return max;
		}
		return val;
	}

	public static int Clamp(int val, int min, int max)
	{
		if (val < min)
		{
			return min;
		}
		if (val > max)
		{
			return max;
		}
		return val;
	}

	public static float EaseInEaseOut(float t)
	{
		float num = t * 2f;
		float num2 = 1f - num;
		float num3 = num2 * num2 * num2;
		return (1f - num3) * 0.5f;
	}

	public static Vector3 EaseInEaseOut(Vector3 max, Vector3 vec)
	{
		if (AboutEqual(0f, max.x) || AboutEqual(0f, max.y) || AboutEqual(0f, max.z))
		{
			return vec;
		}
		Vector3 result = default(Vector3);
		result.x = vec.x / max.x;
		result.y = vec.y / max.y;
		result.z = vec.z / max.z;
		result.x = EaseInEaseOut(result.x);
		result.y = EaseInEaseOut(result.y);
		result.z = EaseInEaseOut(result.z);
		result.Scale(max);
		return result;
	}

	public static float SquaredDistancePointLine(Vector3 point, Vector3 start, Vector3 dir)
	{
		float t;
		Vector3 pointToStartDiff;
		return SquaredDistancePointLine(point, start, dir, out t, out pointToStartDiff);
	}

	public static float SquaredDistancePointLine(Vector3 point, Vector3 start, Vector3 dir, out Vector3 pointToStartDiff)
	{
		float t;
		return SquaredDistancePointLine(point, start, dir, out t, out pointToStartDiff);
	}

	public static float SquaredDistancePointLine(Vector3 point, Vector3 start, Vector3 dir, out float t, out Vector3 pointToStartDiff)
	{
		pointToStartDiff = point - start;
		t = Vector3.Dot(dir, pointToStartDiff);
		float num = Vector3.Dot(dir, dir);
		t /= num;
		dir.Scale(new Vector3(t, t, t));
		Vector3 vector = pointToStartDiff - dir;
		return Vector3.Dot(vector, vector);
	}

	public static Vector3 NearestPointPointLine(Vector3 point, Vector3 start, Vector3 dir)
	{
		Vector3 rhs = point - start;
		float num = Vector3.Dot(dir, rhs);
		float num2 = Vector3.Dot(dir, dir);
		num /= num2;
		dir.Scale(new Vector3(num, num, num));
		return start + dir;
	}

	public static float SquaredDistanceLineLine(Vector3 start0, Vector3 dir0, Vector3 start1, Vector3 dir1)
	{
		float s;
		float t;
		return SquaredDistanceLineLine(start0, dir0, start1, dir1, out s, out t);
	}

	public static float SquaredDistanceLineLine(Vector3 start0, Vector3 dir0, Vector3 start1, Vector3 dir1, out float s, out float t)
	{
		Vector3 vector = start0 - start1;
		float num = Vector3.Dot(dir0, dir0);
		float num2 = 0f - Vector3.Dot(dir0, dir1);
		float num3 = Vector3.Dot(dir1, dir1);
		float num4 = Vector3.Dot(dir0, vector);
		float num5 = Vector3.Dot(vector, vector);
		float num6 = num * num3 - num2 * num2;
		if (num6 >= ms_Epsilon)
		{
			float num7 = 0f - Vector3.Dot(dir1, vector);
			float num8 = 1f / num6;
			s = (num2 * num7 - num3 * num4) * num8;
			t = (num2 * num4 - num * num7) * num8;
			return s * (num * s + num2 * t + 2f * num4) + t * (num2 * s + num3 * t + 2f * num7) + num5;
		}
		s = (0f - num4) / num;
		t = 0f;
		return num4 * s + num5;
	}

	public static Vector3 IntersectPointLineLine(Vector3 start0, Vector3 dir0, Vector3 start1, Vector3 dir1)
	{
		Vector3 rhs = start0 - start1;
		float num = Vector3.Dot(dir0, dir0);
		float num2 = 0f - Vector3.Dot(dir0, dir1);
		float num3 = Vector3.Dot(dir1, dir1);
		float num4 = Vector3.Dot(dir0, rhs);
		float num5 = num * num3 - num2 * num2;
		if (num5 >= ms_Epsilon)
		{
			float num6 = 0f - Vector3.Dot(dir1, rhs);
			float num7 = 1f / num5;
			float num8 = (num2 * num6 - num3 * num4) * num7;
			dir0.Scale(new Vector3(num8, num8, num8));
			return start0 + dir0;
		}
		return Vector3.zero;
	}

	public static void Shuffle(int[] shuffleInts)
	{
		for (int num = shuffleInts.Length - 1; num >= 1; num--)
		{
			int num2 = Random.Range(0, num);
			int num3 = shuffleInts[num2];
			shuffleInts[num2] = shuffleInts[num];
			shuffleInts[num] = num3;
		}
	}

	public static void Shuffle(int[] shuffleInts, int low)
	{
		for (int num = shuffleInts.Length - 1; num > low; num--)
		{
			int num2 = Random.Range(low, num);
			int num3 = shuffleInts[num2];
			shuffleInts[num2] = shuffleInts[num];
			shuffleInts[num] = num3;
		}
	}

	public static void ShuffleLastCantBeFirst(int[] shuffleInts)
	{
		if (shuffleInts.Length != 0)
		{
			int num = shuffleInts.Length - 1;
			int num2 = shuffleInts[num];
			for (int num3 = num; num3 >= 1; num3--)
			{
				int num4 = Random.Range(0, num3);
				int num5 = shuffleInts[num4];
				shuffleInts[num4] = shuffleInts[num3];
				shuffleInts[num3] = num5;
			}
			if (num2 == shuffleInts[0])
			{
				int num6 = Random.Range(1, num);
				int num7 = shuffleInts[num6];
				shuffleInts[num6] = shuffleInts[0];
				shuffleInts[0] = num7;
			}
		}
	}

	public static void ShuffleSecondToLastCantBeFirst(int[] shuffleInts)
	{
		if (shuffleInts.Length != 0)
		{
			int num = shuffleInts.Length - 1;
			int num2 = shuffleInts.Length - 2;
			int num3 = shuffleInts[num2];
			for (int num4 = num; num4 >= 1; num4--)
			{
				int num5 = Random.Range(0, num4);
				int num6 = shuffleInts[num5];
				shuffleInts[num5] = shuffleInts[num4];
				shuffleInts[num4] = num6;
			}
			if (num3 == shuffleInts[0])
			{
				int num7 = Random.Range(1, num);
				int num8 = shuffleInts[num7];
				shuffleInts[num7] = shuffleInts[0];
				shuffleInts[0] = num8;
			}
		}
	}
}

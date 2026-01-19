using System;
using UnityEngine;

public static class Easing
{
	public static class Linear
	{
		public static float easeIn(float t)
		{
			return t;
		}

		public static float easeOut(float t)
		{
			return t;
		}

		public static float easeInOut(float t)
		{
			return t;
		}
	}

	public static class Quartic
	{
		public static float easeIn(float t)
		{
			return Mathf.Pow(t, 4f);
		}

		public static float easeOut(float t)
		{
			return (Mathf.Pow(t - 1f, 4f) - 1f) * -1f;
		}

		public static float easeInOut(float t)
		{
			if (t <= 0.5f)
			{
				return easeIn(t * 2f) / 2f;
			}
			return easeOut((t - 0.5f) * 2f) / 2f + 0.5f;
		}
	}

	public static class Quintic
	{
		public static float easeIn(float t)
		{
			return Mathf.Pow(t, 5f);
		}

		public static float easeOut(float t)
		{
			return Mathf.Pow(t - 1f, 5f) + 1f;
		}

		public static float easeInOut(float t)
		{
			if (t <= 0.5f)
			{
				return easeIn(t * 2f) / 2f;
			}
			return easeOut((t - 0.5f) * 2f) / 2f + 0.5f;
		}
	}

	public static class Sinusoidal
	{
		public static float easeIn(float t)
		{
			return Mathf.Sin((t - 1f) * ((float)Math.PI / 2f)) + 1f;
		}

		public static float easeOut(float t)
		{
			return Mathf.Sin(t * ((float)Math.PI / 2f));
		}

		public static float easeInOut(float t)
		{
			if (t <= 0.5f)
			{
				return easeIn(t * 2f) / 2f;
			}
			return easeOut((t - 0.5f) * 2f) / 2f + 0.5f;
		}
	}

	public static class Exponential
	{
		public static float easeIn(float t)
		{
			return Mathf.Pow(2f, 10f * (t - 1f));
		}

		public static float easeOut(float t)
		{
			return 1f - Mathf.Pow(2f, -10f * t);
		}

		public static float easeInOut(float t)
		{
			if (t <= 0.5f)
			{
				return easeIn(t * 2f) / 2f;
			}
			return easeOut(t * 2f - 1f) / 2f + 0.5f;
		}
	}

	public static class Circular
	{
		public static float easeIn(float t)
		{
			return -1f * Mathf.Sqrt(1f - t * t) + 1f;
		}

		public static float easeOut(float t)
		{
			return Mathf.Sqrt(1f - Mathf.Pow(t - 1f, 2f));
		}

		public static float easeInOut(float t)
		{
			if (t <= 0.5f)
			{
				return easeIn(t * 2f) / 2f;
			}
			return easeOut((t - 0.5f) * 2f) / 2f + 0.5f;
		}
	}

	public static class Back
	{
		private const float s = 1.70158f;

		private const float s2 = 2.5949094f;

		public static float easeIn(float t)
		{
			return t * t * (2.70158f * t - 2f);
		}

		public static float easeOut(float t)
		{
			t -= 1f;
			return t * t * (2.70158f * t + 1.70158f) + 1f;
		}

		public static float easeInOut(float t)
		{
			t *= 2f;
			if (t < 1f)
			{
				return 0.5f * (t * t * (3.5949094f * t - 2.5949094f));
			}
			t -= 2f;
			return 0.5f * (t * t * (3.5949094f * t + 2.5949094f) + 2f);
		}
	}

	public static class Bounce
	{
		private const float b = 0f;

		private const float c = 1f;

		private const float d = 1f;

		public static float easeOut(float t)
		{
			if ((double)(t /= 1f) < 0.36363636363636365)
			{
				return 1f * (7.5625f * t * t);
			}
			if ((double)t < 0.7272727272727273)
			{
				return 1f * (7.5625f * (t -= 0.54545456f) * t + 0.75f);
			}
			if ((double)t < 0.9090909090909091)
			{
				return 1f * (7.5625f * (t -= 0.8181818f) * t + 0.9375f);
			}
			return 1f * (7.5625f * (t -= 21f / 22f) * t + 63f / 64f);
		}

		public static float easeIn(float t)
		{
			return 1f - easeOut(1f - t);
		}

		public static float easeInOut(float t)
		{
			if (t < 0.5f)
			{
				return easeIn(t * 2f) * 0.5f;
			}
			return easeOut(t * 2f - 1f) * 0.5f + 0.5f;
		}
	}

	public static class Elastic
	{
		private const float p = 0.3f;

		private static float a = 1f;

		private static float calc(float t, bool easingIn)
		{
			if (t == 0f || t == 1f)
			{
				return t;
			}
			float num = ((!(a < 1f)) ? (0.047746483f * Mathf.Asin(1f / a)) : 0.075f);
			if (easingIn)
			{
				t -= 1f;
				return (0f - a * Mathf.Pow(2f, 10f * t)) * Mathf.Sin((t - num) * ((float)Math.PI * 2f) / 0.3f);
			}
			return a * Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - num) * ((float)Math.PI * 2f) / 0.3f) + 1f;
		}

		public static float easeIn(float t)
		{
			return 1f - easeOut(1f - t);
		}

		public static float easeOut(float t)
		{
			if (t < 0.36363637f)
			{
				return 1f;
			}
			if (t < 0.72727275f)
			{
				t -= 0.54545456f;
				return 7.5625f * t * t + 0.75f;
			}
			if (t < 0.90909094f)
			{
				t -= 0.90909094f;
				return 7.5625f * t * t + 0.9375f;
			}
			t -= 21f / 22f;
			return 7.5625f * t * t + 63f / 64f;
		}

		public static float easeInOut(float t)
		{
			if (t <= 0.5f)
			{
				return easeIn(t * 2f) / 2f;
			}
			return easeOut((t - 0.5f) * 2f) / 2f + 0.5f;
		}
	}
}

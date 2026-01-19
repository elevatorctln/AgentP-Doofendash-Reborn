public class Integration
{
	public delegate float MathFunctionDelegate(float tau);

	public static float RombergIntegral(float a, float b, MathFunctionDelegate F)
	{
		int num = 5;
		float[,] array = new float[2, num];
		float num2 = b - a;
		array[0, 0] = num2 * (F(a) + F(b)) / 2f;
		int num3 = 2;
		int num4 = 1;
		while (num3 <= num)
		{
			float num5 = 0f;
			for (int i = 1; i <= num4; i++)
			{
				num5 += F(a + num2 * ((float)i - 0.5f));
			}
			array[1, 0] = (array[0, 0] + num2 * num5) / 2f;
			int num6 = 1;
			int num7 = 4;
			while (num6 < num3)
			{
				array[1, num6] = ((float)num7 * array[1, num6 - 1] - array[0, num6 - 1]) / (float)(num7 - 1);
				num6++;
				num7 *= 4;
			}
			for (int j = 0; j < num3; j++)
			{
				array[0, j] = array[1, j];
			}
			num3++;
			num4 *= 2;
			num2 /= 2f;
		}
		return array[0, num - 1];
	}
}

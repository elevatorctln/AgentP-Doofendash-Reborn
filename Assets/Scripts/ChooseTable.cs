public class ChooseTable
{
	public static float[,] m_aafChoose;

	private static bool m_IsInited;

	public static void CreateChooseTable(int ctrlPtCnt, int iDegree)
	{
		if (m_IsInited)
		{
			return;
		}
		m_aafChoose = new float[ctrlPtCnt, ctrlPtCnt];
		m_aafChoose[0, 0] = 1f;
		m_aafChoose[1, 0] = 1f;
		m_aafChoose[1, 1] = 1f;
		for (int i = 2; i <= iDegree; i++)
		{
			m_aafChoose[i, 0] = 1f;
			m_aafChoose[i, i] = 1f;
			for (int j = 1; j < i; j++)
			{
				m_aafChoose[i, j] = m_aafChoose[i - 1, j] + m_aafChoose[i - 1, j - 1];
			}
		}
		m_IsInited = true;
	}

	private static long Top(int in_n, int k)
	{
		long num = 1L;
		for (int i = in_n - k + 1; i <= in_n; i++)
		{
			num *= i;
		}
		return num;
	}

	private static long Factorial(int k)
	{
		long num = 1L;
		for (int i = 1; i <= k; i++)
		{
			num *= i;
		}
		return num;
	}

	private static float Choose(int in_n, int k)
	{
		if (k == 0)
		{
			return 1f;
		}
		return Top(in_n, k) / Factorial(k);
	}
}

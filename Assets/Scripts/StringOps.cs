public class StringOps
{
	public static int CalcLineFeedCount(string s, out int index)
	{
		int num = 0;
		for (index = 0; index < s.Length; index++)
		{
			if (s[index] == '\n')
			{
				num++;
			}
		}
		return num;
	}

	public static string InsertLineFeeds(string s, int index)
	{
		string text = string.Empty;
		for (int i = 0; i < s.Length; i += index)
		{
			if (i + index > s.Length)
			{
				index = s.Length - i;
			}
			text = text + s.Substring(i, index) + "\n";
		}
		return text;
	}
}

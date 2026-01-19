using UnityEngine;

public class EagleTokenGroups : MonoBehaviour
{
	public TokenGroup[] m_TokenGroups;

	private static EagleTokenGroups m_The;

	private static TokenGroup.WeavyPathElement ms_WeavyPathElementPrev = TokenGroup.WeavyPathElement.M;

	private int[] m_ShuffledInts;

	public static EagleTokenGroups The()
	{
		if (m_The == null)
		{
			Debug.LogWarning("Accessed EagleTokenGroups.m_The before it was set");
		}
		return m_The;
	}

	private void Awake()
	{
		m_The = this;
		ms_WeavyPathElementPrev = (TokenGroup.WeavyPathElement)Random.Range(0, 3);
		m_ShuffledInts = new int[m_TokenGroups.Length];
		for (int i = 0; i < m_TokenGroups.Length; i++)
		{
			m_ShuffledInts[i] = i;
		}
	}

	private void Shuffle()
	{
		for (int num = m_ShuffledInts.Length - 1; num >= 1; num--)
		{
			int num2 = Random.Range(0, num);
			int num3 = m_ShuffledInts[num2];
			m_ShuffledInts[num2] = m_ShuffledInts[num];
			m_ShuffledInts[num] = num3;
		}
	}

	private void Update()
	{
	}

	public TokenGroup InstantiateEagleTokens(Platform platform)
	{
		if (Runner.The() != null && Runner.The().IsInEagleState())
		{
			TokenGroup tokenGroup = RandomTokenGroupAndLane();
			int num = 0;
			return TokenGroup.SpawnIgnoreParentRotation(lane: (int)((tokenGroup.m_WeavyPathElements != null && tokenGroup.m_WeavyPathElements.Length != 0) ? (tokenGroup.m_WeavyPathElements[0] - 1) : (ms_WeavyPathElementPrev - 1)), tokenGroup: tokenGroup, parent: platform.transform, laneOffset: platform.m_LaneOffset, yPos: Runner.The().PeakEagleHeight);
		}
		return null;
	}

	private TokenGroup RandomTokenGroupAndLane()
	{
		Shuffle();
		for (int i = 0; i < m_ShuffledInts.Length; i++)
		{
			TokenGroup tokenGroup = m_TokenGroups[m_ShuffledInts[i]];
			if (tokenGroup.m_WeavyPathElements == null || tokenGroup.m_WeavyPathElements.Length == 0)
			{
				return tokenGroup;
			}
			if (ms_WeavyPathElementPrev == tokenGroup.m_WeavyPathElements[0])
			{
				ms_WeavyPathElementPrev = tokenGroup.m_WeavyPathElements[tokenGroup.m_WeavyPathElements.Length - 1];
				return tokenGroup;
			}
		}
		return null;
	}
}

using System;
using UnityEngine;

public class PlatformSequence : MonoBehaviour
{
	[Serializable]
	public class PlatformTuple
	{
		public Platform m_Platform;

		public Platform[] m_Platforms;

		public bool m_ShouldSelectFromGroup;

		public int m_Count;

		public GameEventManager.TriggerIndeces m_GameEventToExecute;

		public bool m_ShouldSetPathHeight;

		public static PlatformTuple[] Clone(PlatformTuple[] pts)
		{
			PlatformTuple[] array = new PlatformTuple[pts.Length];
			for (int i = 0; i < pts.Length; i++)
			{
				array[i] = new PlatformTuple();
				array[i].m_ShouldSelectFromGroup = pts[i].m_ShouldSelectFromGroup;
				array[i].m_ShouldSetPathHeight = pts[i].m_ShouldSetPathHeight;
				array[i].m_Count = pts[i].m_Count;
				array[i].m_GameEventToExecute = pts[i].m_GameEventToExecute;
				array[i].m_Platform = pts[i].m_Platform;
				array[i].m_Platforms = pts[i].m_Platforms;
			}
			return array;
		}
	}

	public static int ms_PlatformGroupCount = 3;

	public string m_SequenceName;
	public PlatformTuple[] m_PlatformSequence;

	[HideInInspector]
	public int m_PlatformSequenceCount;

	public float m_SelectMeWeight = 10f;

	public float m_SelectMeWeightInterval;
}

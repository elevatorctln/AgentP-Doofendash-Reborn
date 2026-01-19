using UnityEngine;

public class BabyAndDuckyRules
{
	public enum Type
	{
		Baby = 0,
		Ducky = 1
	}

	private bool m_ShouldSpawn;

	private bool m_IsSpecialCaseADetermined;

	private bool m_IsSpecialCaseBDetermined;

	private bool m_WasSeenOnRoofZero;

	private bool m_IsCalculated;

	public int m_RoofPlayedCount;

	private int m_RoofsPerAppearance;

	private Type m_Type;

	public BabyAndDuckyRules(Type type, int roofsPerAppearance)
	{
		m_Type = type;
		m_RoofsPerAppearance = roofsPerAppearance;
	}

	public void ResetShouldSpawn()
	{
		m_ShouldSpawn = false;
	}

	public void ResetCalculationsAll()
	{
		m_ShouldSpawn = false;
		m_IsSpecialCaseADetermined = false;
		m_IsSpecialCaseBDetermined = false;
		m_WasSeenOnRoofZero = false;
		m_IsCalculated = false;
		m_RoofPlayedCount = 0;
	}

	private void CalcSpecialCaseA()
	{
		if (m_Type == Type.Baby)
		{
			if (!m_IsSpecialCaseADetermined)
			{
				m_IsSpecialCaseADetermined = true;
				if (!PlayerData.HasBabyBeenSeen)
				{
					PlayerData.HasBabyBeenSeen = true;
					m_ShouldSpawn = true;
					m_WasSeenOnRoofZero = true;
				}
				else
				{
					m_ShouldSpawn = false;
				}
			}
		}
		else if (!m_IsSpecialCaseADetermined)
		{
			m_IsSpecialCaseADetermined = true;
			if (!PlayerData.HasMomoBeenSeen)
			{
				PlayerData.HasMomoBeenSeen = true;
				m_ShouldSpawn = true;
				m_WasSeenOnRoofZero = true;
			}
			else
			{
				m_ShouldSpawn = false;
			}
		}
	}

	private void CalcSpecialCaseB()
	{
		if (!m_IsSpecialCaseBDetermined)
		{
			m_IsSpecialCaseBDetermined = true;
			m_ShouldSpawn = false;
			if (!m_WasSeenOnRoofZero && Random.Range(0, 100) < 50)
			{
				m_ShouldSpawn = true;
			}
		}
	}

	public void ReCalcShouldSpawn()
	{
		m_IsCalculated = false;
		CalcShouldSpawn();
	}

	private void CalcShouldSpawn()
	{
		if (m_IsCalculated)
		{
			return;
		}
		m_ShouldSpawn = false;
		bool flag = false;
		if ((m_Type != Type.Baby) ? PlayerData.MomoUnlocked : PlayerData.BabyUnlocked)
		{
			if (m_RoofPlayedCount == 0)
			{
				CalcSpecialCaseA();
			}
			else if (m_RoofPlayedCount == 1)
			{
				CalcSpecialCaseB();
			}
			else if ((m_RoofPlayedCount - 1) % m_RoofsPerAppearance == 0)
			{
				m_ShouldSpawn = true;
			}
		}
		m_IsCalculated = true;
	}

	public bool ShouldSpawn()
	{
		CalcShouldSpawn();
		return m_ShouldSpawn;
	}
}

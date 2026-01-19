using System;
using UnityEngine;

public class BossSoundHandler : MonoBehaviour
{
	private void OnDestroy()
	{
		GameEventManager.BossIntroTauntEvents -= BossIntroTauntListener;
		GameEventManager.BossStartEvents -= BossStartListener;
		GameEventManager.BossTeasing -= BossTeasingListener;
		GameEventManager.DamageBoss -= DamageBossListener;
		GameEventManager.BossDead -= BossDeadListener;
		GameEventManager.BossChooseAttack -= BossChooseAttackListener;
		GameEventManager.BossFireAttackStart -= BossFireAttackStartListener;
		GameEventManager.BossWaterAttackStart -= BossWaterAttackStartListener;
		GameEventManager.BossIceAttackStart -= BossIceAttackStartListener;
		GameEventManager.BossTargetVisible -= BossTargetVisibleListener;
		GameEventManager.BossTargetInvisible -= BossTargetInvisibleListener;
		GameEventManager.BossButtonPressEvents -= BossButtonPressListener;
		GameEventManager.BossSlotMachineStartEvents -= BossSlotMachineStartListener;
		GameEventManager.BossSlotMachineEndEvents -= BossSlotMachineEndListener;
		GameEventManager.BossWeaponShotStartEvents -= BossWeaponShotStartListener;
		GameEventManager.BossWeaponShotEndEvents -= BossWeaponShotEndListener;
		GameEventManager.BossWeaponChargeStartEvents -= BossWeaponChargeStartListener;
		GameEventManager.BossWeaponChargeEndEvents -= BossWeaponChargeEndListener;
		GameEventManager.BossEndEvents -= BossEndListener;
	}

	private void Awake()
	{
		GameEventManager.BossIntroTauntEvents += BossIntroTauntListener;
		GameEventManager.BossStartEvents += BossStartListener;
		GameEventManager.BossTeasing += BossTeasingListener;
		GameEventManager.DamageBoss += DamageBossListener;
		GameEventManager.BossDead += BossDeadListener;
		GameEventManager.BossChooseAttack += BossChooseAttackListener;
		GameEventManager.BossFireAttackStart += BossFireAttackStartListener;
		GameEventManager.BossWaterAttackStart += BossWaterAttackStartListener;
		GameEventManager.BossIceAttackStart += BossIceAttackStartListener;
		GameEventManager.BossTargetVisible += BossTargetVisibleListener;
		GameEventManager.BossTargetInvisible += BossTargetInvisibleListener;
		GameEventManager.BossButtonPressEvents += BossButtonPressListener;
		GameEventManager.BossSlotMachineStartEvents += BossSlotMachineStartListener;
		GameEventManager.BossSlotMachineEndEvents += BossSlotMachineEndListener;
		GameEventManager.BossWeaponShotStartEvents += BossWeaponShotStartListener;
		GameEventManager.BossWeaponShotEndEvents += BossWeaponShotEndListener;
		GameEventManager.BossWeaponChargeStartEvents += BossWeaponChargeStartListener;
		GameEventManager.BossWeaponChargeEndEvents += BossWeaponChargeEndListener;
		GameEventManager.BossEndEvents += BossEndListener;
	}

	private void BossArmsExtendListener()
	{
		GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSARMSEXTEND);
	}

	private void BossFireShotStartListener()
	{
		GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSFIREEFFECTSTART);
		GameManager.The.PlayLoopClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSFIREEFFECTLOOP);
	}

	private void BossFireShotEndListener()
	{
		GameManager.The.StopClipLoop(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSFIREEFFECTLOOP);
		GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSFIREEFFECTEND);
	}

	private void BossWaterShotStartListener()
	{
		GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSWATEREFFECTSTART);
		GameManager.The.PlayLoopClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSWATEREFFECTLOOP);
	}

	private void BossWaterShotEndListener()
	{
		GameManager.The.StopClipLoop(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSWATEREFFECTLOOP);
		GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSWATEREFFECTEND);
	}

	private void BossIceShotStartListener()
	{
		GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSICEEFFECTSTART);
		GameManager.The.PlayLoopClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSICEEFFECTLOOP);
	}

	private void BossIceShotEndListener()
	{
		GameManager.The.StopClipLoop(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSICEEFFECTLOOP);
		GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSICEEFFECTEND);
	}

	private void BossIntroTauntListener()
	{
		if (!(Runner.The() == null))
		{
			if ((UnityEngine.Random.Range(1, 10) <= 2) ? true : false)
			{
				GameManager.The.PlayClip(AudioClipFiles.BOSSINTROALL + UnityEngine.Random.Range(1, AudioClipFiles.NUMBERBOSSINTROALL));
			}
			else if (Runner.The().currentRunner.Contains("Perry"))
			{
				GameManager.The.PlayClip(AudioClipFiles.BOSSPERRYTUBEEXIT);
			}
			else if (Runner.The().currentRunner.Contains("Pinky"))
			{
				GameManager.The.PlayClip(AudioClipFiles.BOSSPINKYTUBEEXIT);
			}
			else if (Runner.The().currentRunner.Contains("Peter"))
			{
				GameManager.The.PlayClip(AudioClipFiles.BOSSPETERTUBEEXIT);
			}
			else if (Runner.The().currentRunner.Contains("Terry"))
			{
				GameManager.The.PlayClip(AudioClipFiles.BOSSTERRYTUBEEXIT);
			}
		}
	}

	private void BossSlotMachineStartListener()
	{
		GameManager.The.PlayLoopClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSSLOTSOUNDEFFECT);
	}

	private void BossSlotMachineEndListener()
	{
		GameManager.The.StopClipLoop(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSSLOTSOUNDEFFECT);
	}

	private void BossWeaponShotStartListener(DoofenCruiser.WeaponType weaponType)
	{
		switch (weaponType)
		{
		case DoofenCruiser.WeaponType.Fire:
			GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSFIREEFFECTSTART);
			GameManager.The.PlayLoopClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSFIREEFFECTLOOP);
			break;
		case DoofenCruiser.WeaponType.Water:
			GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSWATEREFFECTSTART);
			GameManager.The.PlayLoopClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSWATEREFFECTLOOP);
			break;
		case DoofenCruiser.WeaponType.Ice:
			GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSICEEFFECTSTART);
			GameManager.The.PlayLoopClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSICEEFFECTLOOP);
			break;
		}
	}

	private void BossWeaponShotEndListener(DoofenCruiser.WeaponType weaponType)
	{
		switch (weaponType)
		{
		case DoofenCruiser.WeaponType.Fire:
			GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSFIREEFFECTEND);
			GameManager.The.StopClipLoop(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSFIREEFFECTLOOP);
			break;
		case DoofenCruiser.WeaponType.Water:
			GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSWATEREFFECTEND);
			GameManager.The.StopClipLoop(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSWATEREFFECTLOOP);
			break;
		case DoofenCruiser.WeaponType.Ice:
			GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSICEEFFECTEND);
			GameManager.The.StopClipLoop(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSICEEFFECTLOOP);
			break;
		}
	}

	private void BossWeaponChargeStartListener()
	{
		GameManager.The.PlayLoopClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSWEAPONSCHARGE);
	}

	private void BossWeaponChargeEndListener()
	{
		GameManager.The.StopClipLoop(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSWEAPONSCHARGE);
	}

	private void BossStartListener(MiniGameManager.BossType bossType)
	{
		switch (bossType)
		{
		case MiniGameManager.BossType.DoofenCruiser:
		{
			GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSMACHINEWHIRLLOOP);
			if (PlayerData.RoundBossEncounters > 1)
			{
				GameManager.The.PlayClip(AudioClipFiles.BOSSREAPPEAR + UnityEngine.Random.Range(1, AudioClipFiles.NUMBERBOSSREAPPEAR + 1));
				break;
			}
			string clip = AudioClipFiles.BOSSINTROCHARACTER.Replace("CHARACTER", Runner.The().CalcCurrentRunnerBaseName()) + UnityEngine.Random.Range(1, AudioClipFiles.NUMBERBOSSINTROCHARACTER + 1);
			GameManager.The.PlayClip(clip);
			GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSINCOMINGALERT);
			GameManager.The.PlayMusic(AudioClipFiles.BOSSTHEME);
			break;
		}
		case MiniGameManager.BossType.Balloony:
			break;
		default:
			throw new ArgumentOutOfRangeException("weaponType " + bossType);
		}
	}

	private void BossTargetVisibleListener(DoofenCruiser.WeaponType weaponType)
	{
		if (weaponType != DoofenCruiser.WeaponType.Pin)
		{
			GameManager.The.PlayClip(AudioClipFiles.BOSSTARGETAPPEAR + UnityEngine.Random.Range(1, AudioClipFiles.NUMBERBOSSTARGETAPPEAR + 1));
			GameManager.The.PlayClip(AudioClipFiles.BOSSARMSEXTEND);
		}
	}

	private void BossTargetInvisibleListener(DoofenCruiser.WeaponType unused)
	{
		if (unused != DoofenCruiser.WeaponType.Pin)
		{
			GameManager.The.PlayClip(AudioClipFiles.BOSSARMSEXTEND);
		}
	}

	private void BossTeasingListener()
	{
		if (Runner.The() != null && Runner.The().CanAttack)
		{
			GameManager.The.PlayClip(AudioClipFiles.BOSSHASWEAPON + UnityEngine.Random.Range(1, AudioClipFiles.NUMBERBOSSHASWEAPON + 1));
		}
		else
		{
			GameManager.The.PlayClip(AudioClipFiles.BOSSNOWEAPON + UnityEngine.Random.Range(1, AudioClipFiles.NUMBERBOSSNOWEAPON + 1));
		}
	}

	private void BossChooseAttackListener()
	{
		GameManager.The.PlayClip(AudioClipFiles.BOSSSLOTMACHINE + UnityEngine.Random.Range(1, AudioClipFiles.NUMBERBOSSSLOTMACHINE + 1));
	}

	private void BossButtonPressListener()
	{
		GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSBUTTONPRESS);
	}

	private void BossFireAttackStartListener()
	{
		GameManager.The.PlayClip(AudioClipFiles.BOSSFIREINTRO + UnityEngine.Random.Range(1, AudioClipFiles.NUMBERBOSSFIREINTRO + 1));
	}

	private void BossWaterAttackStartListener()
	{
		GameManager.The.PlayClip(AudioClipFiles.BOSSWATERINTRO + UnityEngine.Random.Range(1, AudioClipFiles.NUMBERBOSSWATERINTRO + 1));
	}

	private void BossIceAttackStartListener()
	{
		GameManager.The.PlayClip(AudioClipFiles.BOSSICEINTRO + UnityEngine.Random.Range(1, AudioClipFiles.NUMBERBOSSICEINTRO + 1));
	}

	private void DamageBossListener(MiniGameManager.BossType bossType, float unused)
	{
		if (bossType == MiniGameManager.BossType.DoofenCruiser)
		{
			GameManager.The.PlayClip(AudioClipFiles.BOSSHIT + UnityEngine.Random.Range(1, AudioClipFiles.NUMBERBOSSHIT));
			GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSHITSOUNDEFFECT + UnityEngine.Random.Range(1, AudioClipFiles.NUMBERBOSSHITSOUNDEFFECT + 1));
		}
	}

	private void BossDeadListener(MiniGameManager.BossType bossType)
	{
		switch (bossType)
		{
		case MiniGameManager.BossType.DoofenCruiser:
			GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSEXPLOSION);
			break;
		default:
			throw new ArgumentOutOfRangeException("weaponType" + bossType);
		case MiniGameManager.BossType.Balloony:
			break;
		}
		string clip = AudioClipFiles.BOSSDEFEATED + Runner.The().CalcCurrentRunnerBaseName();
		GameManager.The.PlayClip(clip);
		GameManager.The.PlayClip(AudioClipFiles.BOSSFOLDER + AudioClipFiles.BOSSFLEEEND);
	}

	private void BossEndListener(MiniGameManager.BossType bossType)
	{
	}
}

using UnityEngine;

public static class GameEventManager
{
	public enum TriggerIndeces
	{
		None = 0,
		HangGlideEndPlatformIndex = 1,
		ObstacleSequenceEndEventsIndex = 2
	}

	public delegate void GameEvent();

	public delegate void GameSetEvent(int val);

	public delegate void PlatformEvent(Platform prefab);

	public delegate void PlatformsEvent(Platform[] prefabs);

	public delegate void PlatformSequenceEvent(PlatformSequence.PlatformTuple[] platformSequence);

	public delegate void PlatformStraightAwayEvent(Platform prefab, int minStraightAwayCount);

	public delegate void BossTargetVisibleEvent(DoofenCruiser.WeaponType weaponType);

	public delegate void BossTargetInvisibleEvent(DoofenCruiser.WeaponType weaponType);

	public delegate void BossWeaponEvent(DoofenCruiser.WeaponType weaponType);

	public delegate void UpdateBossHealthEvent(MiniGameManager.BossType bossType, float health);

	public delegate void HangGlideEvent();

	public delegate void LaunchPerryEvent();

	public delegate void BossEvent(MiniGameManager.BossType bossType);

	private delegate void TriggerDelegate();

	public delegate void AdCompletedEventHandler(BaseAdProvider provider);

	public delegate void AdHiddenEventHandler(BaseAdProvider provider);

	private static TriggerDelegate[] m_TriggerDelegateArray = new TriggerDelegate[2] { TriggerHangGlideEndPlatform, TriggerObstacleSequenceEnd };

	public static event GameEvent GameLoading;

	public static event GameEvent GameMainMenu;

	public static event GameEvent GameIntro;

	public static event GameEvent GameIntroEndEvents;

	public static event GameEvent GameStart;

	public static event GameEvent GameOver;

	public static event GameEvent GameContinue;

	public static event GameEvent GamePause;

	public static event GameEvent GameUnPause;

	public static event GameEvent GameRestartMenu;

	public static event GameSetEvent TokenHit;

	public static event GameSetEvent FedoraHit;

	public static event GameSetEvent ScoreUpdate;

	public static event BossEvent BossStartEvents;

	public static event BossEvent BossDead;

	public static event BossEvent BossEndEvents;

	public static event GameEvent BossIntroTauntEvents;

	public static event GameEvent BossArmExtendEvents;

	public static event GameEvent BossTeasing;

	public static event GameEvent BossFlyBack;

	public static event GameEvent BossWaiting;

	public static event GameEvent BossChooseAttack;

	public static event GameEvent BossAttack;

	public static event GameEvent BossGenericAttackStart;

	public static event GameEvent BossFireAttackStart;

	public static event GameEvent BossIceAttackStart;

	public static event GameEvent BossWaterAttackStart;

	public static event GameEvent BossSlotMachineStartEvents;

	public static event GameEvent BossSlotMachineEndEvents;

	public static event GameEvent BossWeaponChargeStartEvents;

	public static event GameEvent BossWeaponChargeEndEvents;

	public static event GameEvent BossStartRamAttack;

	public static event GameEvent BossEndRamAttack;

	public static event GameEvent BossBehindAttack;

	public static event GameEvent BossFrontAttack;

	public static event GameEvent BossDone;

	public static event GameEvent BossMoveToNextMiniGameEvents;

	public static event GameEvent BossButtonPressEvents;

	public static event BossWeaponEvent BossWeaponShotStartEvents;

	public static event BossWeaponEvent BossWeaponShotEndEvents;

	public static event BossTargetVisibleEvent BossTargetVisible;

	public static event BossTargetInvisibleEvent BossTargetInvisible;

	public static event UpdateBossHealthEvent UpdateBossHealth;

	public static event UpdateBossHealthEvent DamageBoss;

	public static event PlatformEvent ForceNextPlatform;

	public static event PlatformEvent MoveToNextPlatformEvents;

	public static event PlatformsEvent ForceNextPlatforms;

	public static event PlatformSequenceEvent ForcePlatformSequences;

	public static event PlatformStraightAwayEvent ForceNewStraightAways;

	public static event GameEvent ObstacleSequenceEndEvents;

	public static event GameEvent CollectScoreMultiplier;

	public static event GameEvent ScoreMultiplierOff;

	public static event GameEvent TempInvincibilityOnEvents;

	public static event GameEvent CollectInvincibility;

	public static event GameEvent InvincibilityCooldown;

	public static event GameEvent InvincibilityOff;

	public static event GameEvent PowerUpMagnetOn;

	public static event GameEvent PowerUpMagnetOff;

	public static event GameEvent PowerUpFeatherOnEvents;

	public static event GameEvent PowerUpFeatherOffEvents;

	public static event GameEvent TokensLastSpawnedEvents;

	public static event GameEvent CopterBoostOnEvents;

	public static event GameEvent CopterBoostOffEvents;

	public static event HangGlideEvent HangGlideStartEvents;

	public static event HangGlideEvent HangGlideEndEvents;

	public static event HangGlideEvent HangGlideEndPlatformEvents;

	public static event LaunchPerryEvent LaunchPerryEvents;

	public static event AdCompletedEventHandler OnAdCompleted;

	public static event AdHiddenEventHandler OnAdHidden;

	public static void TriggerGameLoad()
	{
		if (GameEventManager.GameLoading != null)
		{
			GameEventManager.GameLoading();
		}
	}

	public static void TriggerGameMainMenu()
	{
		if (GameEventManager.GameMainMenu != null)
		{
			GameEventManager.GameMainMenu();
		}
	}

	public static void TriggerGameIntro()
	{
		if (GameEventManager.GameIntro != null)
		{
			GameEventManager.GameIntro();
		}
	}

	public static void TriggerGameIntroEnd()
	{
		if (GameEventManager.GameIntroEndEvents != null)
		{
			GameEventManager.GameIntroEndEvents();
		}
	}

	public static void TriggerGameStart()
	{
		if (GameEventManager.GameStart != null)
		{
			GameEventManager.GameStart();
		}
	}

	public static void TriggerGameContinue()
	{
		if (GameEventManager.GameContinue != null)
		{
			GameEventManager.GameContinue();
		}
	}

	public static void TriggerGameOver()
	{
		if (GameEventManager.GameOver != null)
		{
			GameEventManager.GameOver();
		}
	}

	public static void TriggerGameRestartMenu()
	{
		if (GameEventManager.GameRestartMenu != null)
		{
			GameEventManager.GameRestartMenu();
		}
	}

	public static void TriggerGamePause()
	{
		if (GameEventManager.GamePause != null)
		{
			GameEventManager.GamePause();
		}
	}

	public static void TriggerGameUnPause()
	{
		if (GameEventManager.GameUnPause != null)
		{
			GameEventManager.GameUnPause();
		}
	}

	public static void TriggerTokenHit(int val)
	{
		if (GameEventManager.TokenHit != null)
		{
			GameEventManager.TokenHit(val);
		}
	}

	public static void TriggerFedoraHit(int val)
	{
		if (GameEventManager.FedoraHit != null)
		{
			GameEventManager.FedoraHit(val);
		}
	}

	public static void TriggerScoreUpdate(int val)
	{
		if (GameEventManager.ScoreUpdate != null)
		{
			GameEventManager.ScoreUpdate(val);
		}
	}

	public static void TriggerBossIntroTaunt()
	{
		if (GameEventManager.BossIntroTauntEvents != null)
		{
			GameEventManager.BossIntroTauntEvents();
		}
	}

	public static void TriggerBossStart(MiniGameManager.BossType bossType)
	{
		if (GameEventManager.BossStartEvents != null)
		{
			GameEventManager.BossStartEvents(bossType);
		}
		else
		{
			Debug.Log("BossIntroIgnored");
		}
	}

	public static void TriggerBossArmsExtend()
	{
		if (GameEventManager.BossArmExtendEvents != null)
		{
			GameEventManager.BossArmExtendEvents();
		}
	}

	public static void TriggerBossTease()
	{
		if (GameEventManager.BossTeasing != null)
		{
			GameEventManager.BossTeasing();
		}
		else
		{
			Debug.Log("BossTeaseIgnored");
		}
	}

	public static void TriggerBossFlyBack()
	{
		if (GameEventManager.BossFlyBack != null)
		{
			GameEventManager.BossFlyBack();
		}
		else
		{
			Debug.Log("Boss Fly Back ignored");
		}
	}

	public static void TriggerBossWaiting()
	{
		if (GameEventManager.BossWaiting != null)
		{
			GameEventManager.BossWaiting();
		}
	}

	public static void TriggerBossChooseAttack()
	{
		if (GameEventManager.BossChooseAttack != null)
		{
			GameEventManager.BossChooseAttack();
		}
	}

	public static void TriggerBossButtonPress()
	{
		if (GameEventManager.BossButtonPressEvents != null)
		{
			GameEventManager.BossButtonPressEvents();
		}
	}

	public static void TriggerBossAttack()
	{
		if (GameEventManager.BossAttack != null)
		{
			GameEventManager.BossAttack();
		}
	}

	public static void TriggerBossGenericAttackStart()
	{
		if (GameEventManager.BossGenericAttackStart != null)
		{
			GameEventManager.BossGenericAttackStart();
		}
	}

	public static void TriggerBossFireAttackType()
	{
		if (GameEventManager.BossFireAttackStart != null)
		{
			GameEventManager.BossFireAttackStart();
		}
	}

	public static void TriggerBossIceAttackType()
	{
		if (GameEventManager.BossIceAttackStart != null)
		{
			GameEventManager.BossIceAttackStart();
		}
	}

	public static void TriggerBossWaterAttackType()
	{
		if (GameEventManager.BossWaterAttackStart != null)
		{
			GameEventManager.BossWaterAttackStart();
		}
	}

	public static void TriggerBossSlotMachineStart()
	{
		if (GameEventManager.BossSlotMachineStartEvents != null)
		{
			GameEventManager.BossSlotMachineStartEvents();
		}
	}

	public static void TriggerBossSlothMachineEnd()
	{
		if (GameEventManager.BossSlotMachineEndEvents != null)
		{
			GameEventManager.BossSlotMachineEndEvents();
		}
	}

	public static void TriggerBossWeaponChargeStart()
	{
		if (GameEventManager.BossWeaponChargeStartEvents != null)
		{
			GameEventManager.BossWeaponChargeStartEvents();
		}
	}

	public static void TriggerBossWeaponChargeEnd()
	{
		if (GameEventManager.BossWeaponChargeEndEvents != null)
		{
			GameEventManager.BossWeaponChargeEndEvents();
		}
	}

	public static void TriggerBossWeaponShotStart(DoofenCruiser.WeaponType WeaponType)
	{
		if (GameEventManager.BossWeaponShotStartEvents != null)
		{
			GameEventManager.BossWeaponShotStartEvents(WeaponType);
		}
	}

	public static void TriggerBossWeaponShotEnd(DoofenCruiser.WeaponType WeaponType)
	{
		if (GameEventManager.BossWeaponShotEndEvents != null)
		{
			GameEventManager.BossWeaponShotEndEvents(WeaponType);
		}
	}

	public static void TriggerBossStartRamAttack()
	{
		if (GameEventManager.BossStartRamAttack != null)
		{
			GameEventManager.BossStartRamAttack();
		}
	}

	public static void TriggerBossEndRamAttack()
	{
		if (GameEventManager.BossEndRamAttack != null)
		{
			GameEventManager.BossEndRamAttack();
		}
	}

	public static void TriggerBossBehindAttack()
	{
		if (GameEventManager.BossBehindAttack != null)
		{
			GameEventManager.BossBehindAttack();
		}
	}

	public static void TriggerBossFrontAttack()
	{
		if (GameEventManager.BossFrontAttack != null)
		{
			GameEventManager.BossFrontAttack();
		}
	}

	public static void TriggerBossDone()
	{
		if (GameEventManager.BossDone != null)
		{
			GameEventManager.BossDone();
		}
	}

	public static void TriggerBossDead(MiniGameManager.BossType bossType)
	{
		if (GameEventManager.BossDead != null)
		{
			GameEventManager.BossDead(bossType);
		}
	}

	public static void TriggerBossTargetVisible(DoofenCruiser.WeaponType weaponType)
	{
		if (GameEventManager.BossTargetVisible != null)
		{
			GameEventManager.BossTargetVisible(weaponType);
		}
	}

	public static void TriggerBossTargetInvisible(DoofenCruiser.WeaponType weaponType)
	{
		if (GameEventManager.BossTargetInvisible != null)
		{
			GameEventManager.BossTargetInvisible(weaponType);
		}
	}

	public static void TriggerMoveToNextPlatform(Platform platform)
	{
		if (GameEventManager.MoveToNextPlatformEvents != null)
		{
			GameEventManager.MoveToNextPlatformEvents(platform);
		}
	}

	public static void TriggerForceNextPlatform(Platform prefab)
	{
		if (GameEventManager.ForceNextPlatform != null)
		{
			GameEventManager.ForceNextPlatform(prefab);
		}
	}

	public static void TriggerForceNextPlatforms(Platform[] prefabs)
	{
		if (GameEventManager.ForceNextPlatforms != null)
		{
			GameEventManager.ForceNextPlatforms(prefabs);
		}
	}

	public static void TriggerForcePlatformSequence(PlatformSequence.PlatformTuple[] platformSequence)
	{
		if (GameEventManager.ForcePlatformSequences != null)
		{
			GameEventManager.ForcePlatformSequences(platformSequence);
		}
	}

	public static void TriggerForceNewStraightAway(Platform prefab, int minStraightAwayCount)
	{
		if (GameEventManager.ForceNewStraightAways != null)
		{
			GameEventManager.ForceNewStraightAways(prefab, minStraightAwayCount);
		}
	}

	public static void TriggerBossHealthUpdate(MiniGameManager.BossType bossType, float health)
	{
		if (GameEventManager.UpdateBossHealth != null)
		{
			GameEventManager.UpdateBossHealth(bossType, health);
		}
	}

	public static void TriggerBossDamage(float damage)
	{
		if (GameEventManager.DamageBoss != null)
		{
			GameEventManager.DamageBoss((Balloony.The != null && Balloony.The.IsActive()) ? MiniGameManager.BossType.Balloony : MiniGameManager.BossType.DoofenCruiser, damage);
		}
	}

	public static void TriggerBossMoveToNextMiniGame()
	{
		if (GameEventManager.BossMoveToNextMiniGameEvents != null)
		{
			GameEventManager.BossMoveToNextMiniGameEvents();
		}
	}

	public static void TriggerBossEnd(MiniGameManager.BossType bossType)
	{
		if (GameEventManager.BossEndEvents != null)
		{
			GameEventManager.BossEndEvents(bossType);
		}
	}

	public static void TriggerScoreMultiplier()
	{
		if (GameEventManager.CollectScoreMultiplier != null)
		{
			GameEventManager.CollectScoreMultiplier();
		}
	}

	public static void TriggerScoreMultiplierOff()
	{
		if (GameEventManager.ScoreMultiplierOff != null)
		{
			GameEventManager.ScoreMultiplierOff();
		}
	}

	public static void TriggerTempInvincibilityOn()
	{
		if (GameEventManager.TempInvincibilityOnEvents != null)
		{
			GameEventManager.TempInvincibilityOnEvents();
		}
	}

	public static void TriggerInvincibility()
	{
		if (GameEventManager.CollectInvincibility != null)
		{
			GameEventManager.CollectInvincibility();
		}
	}

	public static void TriggerInvincibilityCooldown()
	{
		if (GameEventManager.InvincibilityCooldown != null)
		{
			GameEventManager.InvincibilityCooldown();
		}
	}

	public static void TriggerInvincibilityOff()
	{
		if (GameEventManager.InvincibilityOff != null)
		{
			GameEventManager.InvincibilityOff();
		}
	}

	public static void TriggerPowerUpMagnetOn()
	{
		if (GameEventManager.PowerUpMagnetOn != null)
		{
			GameEventManager.PowerUpMagnetOn();
		}
	}

	public static void TriggerPowerUpMagnetOff()
	{
		if (GameEventManager.PowerUpMagnetOff != null)
		{
			GameEventManager.PowerUpMagnetOff();
		}
	}

	public static void TriggerPowerUpFeatherOn()
	{
		if (GameEventManager.PowerUpFeatherOnEvents != null)
		{
			GameEventManager.PowerUpFeatherOnEvents();
		}
	}

	public static void TriggerPowerUpFeatherOff()
	{
		if (GameEventManager.PowerUpFeatherOffEvents != null)
		{
			GameEventManager.PowerUpFeatherOffEvents();
		}
	}

	public static void TriggerTokensLastSpawned()
	{
		if (GameEventManager.TokensLastSpawnedEvents != null)
		{
			GameEventManager.TokensLastSpawnedEvents();
		}
	}

	public static void TriggerCopterBoostOn()
	{
		if (GameEventManager.CopterBoostOnEvents != null)
		{
			GameEventManager.CopterBoostOnEvents();
		}
	}

	public static void TriggerCopterBoostOff()
	{
		if (GameEventManager.CopterBoostOffEvents != null)
		{
			GameEventManager.CopterBoostOffEvents();
		}
	}

	public static void TriggerHangGlideStart()
	{
		if (GameEventManager.HangGlideStartEvents != null)
		{
			GameEventManager.HangGlideStartEvents();
		}
	}

	public static void TriggerHangGlideEndPlatform()
	{
		if (GameEventManager.HangGlideEndPlatformEvents != null)
		{
			GameEventManager.HangGlideEndPlatformEvents();
		}
	}

	public static void TriggerHangGlideEnd()
	{
		if (GameEventManager.HangGlideEndEvents != null)
		{
			GameEventManager.HangGlideEndEvents();
		}
	}

	public static void TriggerLaunchPerry()
	{
		if (GameEventManager.LaunchPerryEvents != null)
		{
			GameEventManager.LaunchPerryEvents();
		}
	}

	public static void TriggerObstacleSequenceEnd()
	{
		if (GameEventManager.ObstacleSequenceEndEvents != null)
		{
			GameEventManager.ObstacleSequenceEndEvents();
		}
		Platform.ResetObstacleSequence();
	}

	public static void ExecuteTriggerFromEnum(TriggerIndeces ti)
	{
		if (ti == TriggerIndeces.None)
		{
			return;
		}
		int num = (int)(ti - 1);
		if (num < m_TriggerDelegateArray.Length)
		{
			TriggerDelegate triggerDelegate = m_TriggerDelegateArray[num];
			if (triggerDelegate != null)
			{
				triggerDelegate();
			}
		}
	}

	public static void RaiseOnAdCompleted(BaseAdProvider provider)
	{
		if (GameEventManager.OnAdCompleted != null)
		{
			GameEventManager.OnAdCompleted(provider);
		}
	}

	public static void RaiseOnAdHidden(BaseAdProvider provider)
	{
		if (GameEventManager.OnAdHidden != null)
		{
			GameEventManager.OnAdHidden(provider);
		}
	}
}

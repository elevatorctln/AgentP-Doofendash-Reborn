using UnityEngine;
using UnityEditor;
using System.IO;

/// Editor tool for debugging and clearing player data, this has been quite handy but it wasn't in the original code
/// Some of this stuff doesn't work that well, full disclosure

public class EditorDebugTools
{
    [MenuItem("Tools/Debug/Data/Clear All PlayerPrefs Data")]
    public static void ClearAllPlayerPrefs()
    {
        if (EditorUtility.DisplayDialog(
            "Clear All PlayerPrefs Data",
            "This will delete ALL saved player data including:\n\n" +
            "• Tokens and Fedoras\n" +
            "• Purchased Gadgets and Upgrades\n" +
            "• Character Unlocks\n" +
            "• High Scores\n" +
            "• All Progress\n\n" +
            "Are you sure you want to continue?",
            "Yes, Delete All Data",
            "Cancel"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            
            Debug.Log("<color=yellow>✓ All PlayerPrefs data has been cleared!</color>");
            EditorUtility.DisplayDialog(
                "Success",
                "All PlayerPrefs data has been deleted.\n\nThe game will start with default values on next launch.",
                "OK");
        }
        else
        {
            Debug.Log("PlayerPrefs clear operation cancelled.");
        }
    }

    [MenuItem("Tools/Debug/Data/Reset Player Data to Defaults")]
    public static void ResetPlayerDataToDefaults()
    {
        if (EditorUtility.DisplayDialog(
            "Reset to Default Values",
            "This will reset player data to default starting values:\n\n" +
            "• 250 Tokens\n" +
            "• 0 Fedoras\n" +
            "• No gadgets unlocked\n" +
            "• No upgrades purchased\n\n" +
            "Continue?",
            "Yes, Reset",
            "Cancel"))
        {
            PlayerPrefs.DeleteAll();
            
            if (PerryiCloudManager.The != null)
            {
                PerryiCloudManager.The.SetItem("PlayerTokens", 250);
                PerryiCloudManager.The.SetItem("PlayerFedoras", 0);
                PlayerPrefs.Save();
                
                Debug.Log("<color=green>✓ Player data reset to defaults! (250 tokens, 0 fedoras)</color>");
                EditorUtility.DisplayDialog(
                    "Success",
                    "Player data has been reset to default values.\n\n" +
                    "Tokens: 250\nFedoras: 0",
                    "OK");
            }
            else
            {
                Debug.LogWarning("PerryiCloudManager not available in editor. Data cleared but defaults not set.");
            }
        }
    }

    [MenuItem("Tools/Debug/Data/Show PlayerPrefs Location")]
    public static void ShowPlayerPrefsLocation()
    {
        string location = "";
        string platform = Application.platform.ToString();
        
        #if UNITY_EDITOR_WIN
        location = "Windows Registry:\nHKEY_CURRENT_USER\\Software\\Unity\\UnityEditor\\" + Application.companyName + "\\" + Application.productName;
        #elif UNITY_EDITOR_OSX
        location = "~/Library/Preferences/com." + Application.companyName + "." + Application.productName + ".plist";
        #elif UNITY_EDITOR_LINUX
        location = "~/.config/unity3d/" + Application.companyName + "/" + Application.productName + "/prefs";
        #else
        location = "Platform-specific location (check Unity documentation)";
        #endif

        Debug.Log("<color=cyan>PlayerPrefs Location:</color> " + location);
        
        EditorUtility.DisplayDialog(
            "PlayerPrefs Location",
            "Platform: " + platform + "\n\n" +
            "Location:\n" + location,
            "OK");
    }

    [MenuItem("Tools/Debug/Data/Grant Test Currency")]
    public static void GrantTestCurrency()
    {
        if (EditorUtility.DisplayDialog(
            "Grant Test Currency",
            "Add test currency for debugging?\n\n" +
            "+ 50,000 Tokens\n" +
            "+ 100 Fedoras",
            "Grant",
            "Cancel"))
        {
            if (Application.isPlaying)
            {
                PlayerData.playerTokens += 50000;
                PlayerData.playerFedoras += 100;
                PlayerData.SavePersistentData();
                
                Debug.Log("<color=green>✓ Granted test currency! Tokens: " + PlayerData.playerTokens + ", Fedoras: " + PlayerData.playerFedoras + "</color>");
            }
            else
            {
                int currentTokens = PlayerPrefs.GetInt("PlayerTokens", 250);
                int currentFedoras = PlayerPrefs.GetInt("PlayerFedoras", 0);
                
                PlayerPrefs.SetInt("PlayerTokens", currentTokens + 50000);
                PlayerPrefs.SetInt("PlayerFedoras", currentFedoras + 100);
                PlayerPrefs.Save();
                
                Debug.Log("<color=green>✓ Granted test currency! New totals - Tokens: " + (currentTokens + 50000) + ", Fedoras: " + (currentFedoras + 100) + "</color>");
            }
            
            EditorUtility.DisplayDialog(
                "Success",
                "Test currency granted!\n\n" +
                "+ 50,000 Tokens\n" +
                "+ 100 Fedoras",
                "OK");
        }
    }

    [MenuItem("Tools/Debug/Data/Unlock All Gadgets")]
    public static void UnlockAllGadgets()
    {
        if (EditorUtility.DisplayDialog(
            "Unlock All Gadgets",
            "Unlock all gadgets for testing?\n\n" +
            "• Water Cannon\n" +
            "• Fire Cannon\n" +
            "• Electric Cannon\n" +
            "• Pin Shooter",
            "Unlock All",
            "Cancel"))
        {
            if (Application.isPlaying)
            {
                PlayerData.hasWaterWeapon = true;
                PlayerData.hasFireWeapon = true;
                PlayerData.hasElectricWeapon = true;
                PlayerData.HasPinWeapon = true;
                PlayerData.SavePersistentData();
            }
            else
            {
                PlayerPrefs.SetInt("hasWaterWeapon", 1);
                PlayerPrefs.SetInt("hasFireWeapon", 1);
                PlayerPrefs.SetInt("hasElectricWeapon", 1);
                PlayerPrefs.SetInt("hasPinShooter", 1);
                PlayerPrefs.Save();
            }
            
            Debug.Log("<color=green>✓ All gadgets unlocked!</color>");
            EditorUtility.DisplayDialog("Success", "All gadgets have been unlocked!", "OK");
        }
    }

    [MenuItem("Tools/Debug/Data/Log Current PlayerPrefs Data")]
    public static void LogCurrentPlayerPrefsData()
    {
        Debug.Log("=== Current PlayerPrefs Data ===");
        Debug.Log("Tokens: " + PlayerPrefs.GetInt("PlayerTokens", -1));
        Debug.Log("Fedoras: " + PlayerPrefs.GetInt("PlayerFedoras", -1));
        Debug.Log("High Score: " + PlayerPrefs.GetInt("HigestAllTimeScore", -1));
        Debug.Log("\nGadgets:");
        Debug.Log("  Water Cannon: " + (PlayerPrefs.GetInt("hasWaterWeapon", 0) == 1));
        Debug.Log("  Fire Cannon: " + (PlayerPrefs.GetInt("hasFireWeapon", 0) == 1));
        Debug.Log("  Electric Cannon: " + (PlayerPrefs.GetInt("hasElectricWeapon", 0) == 1));
        Debug.Log("  Pin Shooter: " + (PlayerPrefs.GetInt("hasPinShooter", 0) == 1));
        Debug.Log("================================");
        
        EditorUtility.DisplayDialog(
            "Data Logged",
            "Current PlayerPrefs data has been logged to the console.\n\n" +
            "Check the Console window for details.",
            "OK");
    }

    private static bool CheckPlayMode(string actionName)
    {
        if (!Application.isPlaying)
        {
            EditorUtility.DisplayDialog(
                "Not in Play Mode",
                "'" + actionName + "' requires the game to be running.\n\n" +
                "Please enter Play Mode first (Ctrl+P or click Play button).",
                "OK");
            return false;
        }
        return true;
    }

    [MenuItem("Tools/Debug/Runtime Events/Game State/Trigger Game Over")]
    public static void TriggerGameOver()
    {
        if (!CheckPlayMode("Trigger Game Over")) return;
        
        GameEventManager.TriggerGameOver();
        Debug.Log("<color=red>⚡ Triggered: Game Over</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Game State/Trigger Game Pause")]
    public static void TriggerGamePause()
    {
        if (!CheckPlayMode("Trigger Game Pause")) return;
        
        GameEventManager.TriggerGamePause();
        Debug.Log("<color=yellow>⚡ Triggered: Game Pause</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Game State/Trigger Game Unpause")]
    public static void TriggerGameUnpause()
    {
        if (!CheckPlayMode("Trigger Game Unpause")) return;
        
        GameEventManager.TriggerGameUnPause();
        Debug.Log("<color=green>⚡ Triggered: Game Unpause</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Game State/Trigger Game Continue")]
    public static void TriggerGameContinue()
    {
        if (!CheckPlayMode("Trigger Game Continue")) return;
        
        GameEventManager.TriggerGameContinue();
        Debug.Log("<color=green>⚡ Triggered: Game Continue</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Game State/Trigger Game Start")]
    public static void TriggerGameStart()
    {
        if (!CheckPlayMode("Trigger Game Start")) return;
        
        GameEventManager.TriggerGameStart();
        Debug.Log("<color=green>⚡ Triggered: Game Start</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Power-Ups/Trigger Invincibility ON")]
    public static void TriggerInvincibilityOn()
    {
        if (!CheckPlayMode("Trigger Invincibility")) return;
        
        GameEventManager.TriggerInvincibility();
        Debug.Log("<color=cyan>⚡ Triggered: Invincibility ON</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Power-Ups/Trigger Invincibility OFF")]
    public static void TriggerInvincibilityOff()
    {
        if (!CheckPlayMode("Trigger Invincibility OFF")) return;
        
        GameEventManager.TriggerInvincibilityOff();
        Debug.Log("<color=gray>⚡ Triggered: Invincibility OFF</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Power-Ups/Trigger Score Multiplier ON")]
    public static void TriggerScoreMultiplierOn()
    {
        if (!CheckPlayMode("Trigger Score Multiplier")) return;
        
        GameEventManager.TriggerScoreMultiplier();
        Debug.Log("<color=yellow>⚡ Triggered: Score Multiplier ON</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Power-Ups/Trigger Score Multiplier OFF")]
    public static void TriggerScoreMultiplierOff()
    {
        if (!CheckPlayMode("Trigger Score Multiplier OFF")) return;
        
        GameEventManager.TriggerScoreMultiplierOff();
        Debug.Log("<color=gray>⚡ Triggered: Score Multiplier OFF</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Power-Ups/Trigger Magnet ON")]
    public static void TriggerMagnetOn()
    {
        if (!CheckPlayMode("Trigger Magnet")) return;
        
        GameEventManager.TriggerPowerUpMagnetOn();
        Debug.Log("<color=magenta>⚡ Triggered: Magnet ON</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Power-Ups/Trigger Magnet OFF")]
    public static void TriggerMagnetOff()
    {
        if (!CheckPlayMode("Trigger Magnet OFF")) return;
        
        GameEventManager.TriggerPowerUpMagnetOff();
        Debug.Log("<color=gray>⚡ Triggered: Magnet OFF</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Power-Ups/Trigger Eagle (Feather) ON")]
    public static void TriggerEagleOn()
    {
        if (!CheckPlayMode("Trigger Eagle")) return;
        
        GameEventManager.TriggerPowerUpFeatherOn();
        Debug.Log("<color=orange>⚡ Triggered: Eagle Power-Up ON</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Power-Ups/Trigger Eagle (Feather) OFF")]
    public static void TriggerEagleOff()
    {
        if (!CheckPlayMode("Trigger Eagle OFF")) return;
        
        GameEventManager.TriggerPowerUpFeatherOff();
        Debug.Log("<color=gray>⚡ Triggered: Eagle Power-Up OFF</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Power-Ups/Trigger Copter Boost ON")]
    public static void TriggerCopterBoostOn()
    {
        if (!CheckPlayMode("Trigger Copter Boost")) return;
        
        GameEventManager.TriggerCopterBoostOn();
        Debug.Log("<color=blue>⚡ Triggered: Copter Boost ON</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Power-Ups/Trigger Copter Boost OFF")]
    public static void TriggerCopterBoostOff()
    {
        if (!CheckPlayMode("Trigger Copter Boost OFF")) return;
        
        GameEventManager.TriggerCopterBoostOff();
        Debug.Log("<color=gray>⚡ Triggered: Copter Boost OFF</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Boss/Start DoofenCruiser Boss Fight")]
    public static void TriggerBossStartDoofenCruiser()
    {
        if (!CheckPlayMode("Start DoofenCruiser Boss")) return;
        
        GameEventManager.TriggerBossStart(MiniGameManager.BossType.DoofenCruiser);
        Debug.Log("<color=red>⚡ Triggered: DoofenCruiser Boss Start</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Boss/Start Balloony Boss Fight")]
    public static void TriggerBossStartBalloony()
    {
        if (!CheckPlayMode("Start Balloony Boss")) return;
        
        GameEventManager.TriggerBossStart(MiniGameManager.BossType.Balloony);
        Debug.Log("<color=red>⚡ Triggered: Balloony Boss Start</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Boss/Damage Boss (10 damage)")]
    public static void TriggerDamageBoss()
    {
        if (!CheckPlayMode("Damage Boss")) return;
        
        GameEventManager.TriggerBossDamage(10f);
        Debug.Log("<color=orange>⚡ Triggered: Boss Damage (10)</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Boss/Damage Boss (50 damage - heavy)")]
    public static void TriggerHeavyDamageBoss()
    {
        if (!CheckPlayMode("Heavy Damage Boss")) return;
        
        GameEventManager.TriggerBossDamage(50f);
        Debug.Log("<color=red>⚡ Triggered: Boss Heavy Damage (50)</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Boss/Kill DoofenCruiser Boss")]
    public static void TriggerBossDeadDoofenCruiser()
    {
        if (!CheckPlayMode("Kill DoofenCruiser")) return;
        
        GameEventManager.TriggerBossDead(MiniGameManager.BossType.DoofenCruiser);
        Debug.Log("<color=green>⚡ Triggered: DoofenCruiser Boss Dead</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Boss/Kill Balloony Boss")]
    public static void TriggerBossDeadBalloony()
    {
        if (!CheckPlayMode("Kill Balloony")) return;
        
        GameEventManager.TriggerBossDead(MiniGameManager.BossType.Balloony);
        Debug.Log("<color=green>⚡ Triggered: Balloony Boss Dead</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Boss/End DoofenCruiser Encounter")]
    public static void TriggerBossEndDoofenCruiser()
    {
        if (!CheckPlayMode("End DoofenCruiser")) return;
        
        GameEventManager.TriggerBossEnd(MiniGameManager.BossType.DoofenCruiser);
        Debug.Log("<color=yellow>⚡ Triggered: DoofenCruiser Boss End</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Boss/End Balloony Encounter")]
    public static void TriggerBossEndBalloony()
    {
        if (!CheckPlayMode("End Balloony")) return;
        
        GameEventManager.TriggerBossEnd(MiniGameManager.BossType.Balloony);
        Debug.Log("<color=yellow>⚡ Triggered: Balloony Boss End</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Boss/Trigger Boss Move To Next MiniGame")]
    public static void TriggerBossMoveToNext()
    {
        if (!CheckPlayMode("Boss Move To Next")) return;
        
        GameEventManager.TriggerBossMoveToNextMiniGame();
        Debug.Log("<color=cyan>⚡ Triggered: Boss Move To Next MiniGame</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/MiniGame/Force Transition to DoofenCruiser")]
    public static void ForceTransitionToDoofenCruiser()
    {
        if (!CheckPlayMode("Force DoofenCruiser Transition")) return;
        
        if (MiniGameManager.The() != null)
        {
            MiniGameManager.The().MoveToDoofenCruiser();
            Debug.Log("<color=red>⚡ Forcing transition to DoofenCruiser!</color>");
        }
        else
        {
            Debug.LogWarning("MiniGameManager not found!");
        }
    }

    [MenuItem("Tools/Debug/Runtime Events/MiniGame/Force Transition to Balloony")]
    public static void ForceTransitionToBalloony()
    {
        if (!CheckPlayMode("Force Balloony Transition")) return;
        
        if (MiniGameManager.The() != null)
        {
            MiniGameManager.The().MoveToBalloony();
            Debug.Log("<color=magenta>⚡ Forcing transition to Balloony!</color>");
        }
        else
        {
            Debug.LogWarning("MiniGameManager not found!");
        }
    }

    [MenuItem("Tools/Debug/Runtime Events/MiniGame/Log Current MiniGame Info")]
    public static void LogCurrentMiniGameInfo()
    {
        if (!CheckPlayMode("Log MiniGame Info")) return;
        
        if (MiniGameManager.The() != null)
        {
            var currentGame = MiniGameManager.The().RetrieveMiniGameCur();
            var distanceLeft = MiniGameManager.The().CalcDistanceToNextMiniGameLeft();
            var distanceThis = MiniGameManager.The().CalcDistanceThisMiniGame();
            
            Debug.Log("=== Current MiniGame Info ===");
            Debug.Log("Current MiniGame: " + currentGame);
            Debug.Log("Distance in this MiniGame: " + distanceThis.ToString("F1"));
            Debug.Log("Distance to next MiniGame: " + distanceLeft.ToString("F1"));
            Debug.Log("Can spawn Eagle: " + MiniGameManager.The().CanSpawnEaglePowerUp());
            Debug.Log("=============================");
        }
        else
        {
            Debug.LogWarning("MiniGameManager not found!");
        }
    }


    [MenuItem("Tools/Debug/Runtime Events/HangGlide/Start Hang Glide")]
    public static void TriggerHangGlideStart()
    {
        if (!CheckPlayMode("Start Hang Glide")) return;
        
        GameEventManager.TriggerHangGlideStart();
        Debug.Log("<color=blue>⚡ Triggered: Hang Glide Start</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/HangGlide/End Hang Glide")]
    public static void TriggerHangGlideEnd()
    {
        if (!CheckPlayMode("End Hang Glide")) return;
        
        GameEventManager.TriggerHangGlideEnd();
        Debug.Log("<color=gray>⚡ Triggered: Hang Glide End</color>");
    }


    [MenuItem("Tools/Debug/Runtime Events/Currency/Add 100 Tokens (in-game)")]
    public static void TriggerAddTokens()
    {
        if (!CheckPlayMode("Add Tokens")) return;
        
        GameEventManager.TriggerTokenHit(100);
        PlayerData.RoundTokens += 100;
        Debug.Log("<color=yellow>⚡ Triggered: +100 Tokens (Round)</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Currency/Add 10 Fedoras (in-game)")]
    public static void TriggerAddFedoras()
    {
        if (!CheckPlayMode("Add Fedoras")) return;
        
        GameEventManager.TriggerFedoraHit(10);
        PlayerData.RoundFedoras += 10;
        Debug.Log("<color=cyan>⚡ Triggered: +10 Fedoras (Round)</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Currency/Add 10000 Score")]
    public static void TriggerAddScore()
    {
        if (!CheckPlayMode("Add Score")) return;
        
        GameEventManager.TriggerScoreUpdate(10000);
        PlayerData.RoundScore += 10000;
        Debug.Log("<color=green>⚡ Triggered: +10000 Score</color>");
    }

    [MenuItem("Tools/Debug/Runtime Events/Status/Log Current Game State")]
    public static void LogCurrentGameState()
    {
        if (!CheckPlayMode("Log Game State")) return;
        
        Debug.Log("=== Current Runtime Game State ===");
        Debug.Log("Round Score: " + PlayerData.RoundScore);
        Debug.Log("Round Tokens: " + PlayerData.RoundTokens);
        Debug.Log("Round Fedoras: " + PlayerData.RoundFedoras);
        Debug.Log("Round Meters: " + PlayerData.RoundMeters);
        Debug.Log("Round Boss Defeats: " + PlayerData.RoundBossDefeats);
        Debug.Log("Round Boss Encounters: " + PlayerData.RoundBossEncounters);
        Debug.Log("---");
        Debug.Log("Total Tokens: " + PlayerData.playerTokens);
        Debug.Log("Total Fedoras: " + PlayerData.playerFedoras);
        Debug.Log("Highest Score: " + PlayerData.HighestAllTimeScore);
        Debug.Log("==================================");
    }

    [MenuItem("Tools/Debug/Runtime Events/Status/Log Player Weapons Status")]
    public static void LogPlayerWeaponsStatus()
    {
        if (!CheckPlayMode("Log Weapons Status")) return;
        
        Debug.Log("=== Player Weapons Status ===");
        Debug.Log("Current Gadget Type: " + PlayerData.m_currentGadgetType);
        Debug.Log("Has Water Weapon: " + PlayerData.hasWaterWeapon);
        Debug.Log("Has Fire Weapon: " + PlayerData.hasFireWeapon);
        Debug.Log("Has Electric Weapon: " + PlayerData.hasElectricWeapon);
        Debug.Log("Has Pin Weapon: " + PlayerData.HasPinWeapon);
        Debug.Log("Water Upgrades: " + PlayerData.WaterWeaponUpgrades);
        Debug.Log("Fire Upgrades: " + PlayerData.FireWeaponUpgrades);
        Debug.Log("Electric Upgrades: " + PlayerData.ElectricWeaponUpgrades);
        Debug.Log("==============================");
    }
}

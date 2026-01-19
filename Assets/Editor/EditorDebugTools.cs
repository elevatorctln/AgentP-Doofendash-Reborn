using UnityEngine;
using UnityEditor;
using System.IO;

/// Editor tool for debugging and clearning player data, this has been quite handy but it wasn't in the original code

public class EditorDebugTools
{
    [MenuItem("Tools/Debug/Clear All PlayerPrefs Data")]
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

    [MenuItem("Tools/Debug/Reset Player Data to Defaults")]
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

    [MenuItem("Tools/Debug/Show PlayerPrefs Location")]
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

    [MenuItem("Tools/Debug/Grant Test Currency")]
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

    [MenuItem("Tools/Debug/Unlock All Gadgets")]
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

    [MenuItem("Tools/Debug/Log Current PlayerPrefs Data")]
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
}

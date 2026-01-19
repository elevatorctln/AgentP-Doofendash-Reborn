# Editor Debug Tools

This folder contains editor scripts for debugging and testing the game during development.

## EditorDebugTools.cs

Provides convenient menu items in the Unity Editor for managing persistent player data.

### Menu Location
All tools are accessible via: **Tools > Debug** in the Unity Editor menu bar

### Available Tools

#### 1. Clear All PlayerPrefs Data
- **What it does**: Completely wipes all saved player data
- **Use case**: Starting fresh with a clean slate
- **Warning**: This is irreversible! All progress will be lost.

#### 2. Reset Player Data to Defaults
- **What it does**: Resets player to starting values (250 tokens, 0 fedoras)
- **Use case**: Testing new player experience
- **Note**: Clears everything first, then sets initial values

#### 3. Show PlayerPrefs Location
- **What it does**: Displays where PlayerPrefs data is stored on your system
- **Use case**: Finding the actual files/registry keys for manual inspection
- **Locations**:
  - Windows: Registry at `HKEY_CURRENT_USER\Software\Unity\UnityEditor\[Company]\[Product]`
  - macOS: `~/Library/Preferences/com.[Company].[Product].plist`
  - Linux: `~/.config/unity3d/[Company]/[Product]/prefs`

#### 4. Grant Test Currency
- **What it does**: Adds 50,000 tokens and 100 fedoras
- **Use case**: Quickly getting currency for testing purchases
- **Note**: Works both in edit mode and play mode

#### 5. Unlock All Gadgets
- **What it does**: Instantly unlocks all four gadgets
  - Water Cannon
  - Fire Cannon
  - Electric Cannon
  - Pin Shooter
- **Use case**: Testing gameplay with all gadgets available

#### 6. Log Current PlayerPrefs Data
- **What it does**: Prints current saved data to the Console
- **Use case**: Inspecting what's currently saved without clearing
- **Shows**: Tokens, Fedoras, High Score, and Gadget unlock status

## Usage Tips

1. **Before Testing Purchases**: Use "Reset Player Data to Defaults" to ensure consistent starting conditions

2. **Quick Testing**: Use "Grant Test Currency" + "Unlock All Gadgets" to get everything for feature testing

3. **Debugging Save Issues**: Use "Log Current PlayerPrefs Data" to verify what's actually being saved

4. **Clean Slate**: Use "Clear All PlayerPrefs Data" when you need to completely reset for bug reproduction

## Safety Features

- All destructive operations show confirmation dialogs
- Success/failure messages are logged to the Console
- Clear color-coded console output for easy debugging

## Technical Notes

- These tools only work in the Unity Editor (not in builds)
- Uses `[MenuItem]` attributes to add menu items
- Directly manipulates `PlayerPrefs` for immediate results
- Works both in edit mode and play mode where applicable

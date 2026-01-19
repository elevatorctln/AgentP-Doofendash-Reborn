# Keyboard Controls Documentation

This document outlines the keyboard controls available for debugging in the Unity Editor and for eventual PC builds. Both keyboard and touch/mouse controls work simultaneously without any configuration needed.

## Control Scheme

### Lane Changes
**Move Right:**
- `D` key
- `Right Arrow` key

**Move Left:**
- `A` key
- `Left Arrow` key

### Jump
**Jump/Vertical Movement:**
- `W` key
- `Up Arrow` key
- `Space` key
- Unity Input Button: "Jump"

### Slide/Duck
**Slide/Duck Down:**
- `S` key
- `Down Arrow` key
- `X` key
- Unity Input Button: "Slide"

### Fire/Shoot
**Attack/Shoot Weapon:**
- `F` key
- `E` key
- `Left Ctrl` key
- Unity Input Buttons: "Shoot", "Fire1", "Fire2", "Fire3"

## Touch/Mouse Controls

Touch and mouse swipe controls remain fully functional and work alongside keyboard controls:

- **Swipe Up:** Jump
- **Swipe Down:** Slide
- **Swipe Left:** Move Left Lane
- **Swipe Right:** Move Right Lane
- **Tap:** Fire/Shoot (when weapon is equipped)

## Implementation Details

### Simultaneous Input Support
- Keyboard and touch/mouse inputs are processed independently in the `UpdateControls()` method
- Both input methods set the same action flags (`m_jumpOnUpdate`, `m_slideOnUpdate`, etc.)
- No mode switching required - both input types work at all times
- Input checks use `KeyCode` enum for more reliable keyboard detection

### Testing in Unity Editor
- Use keyboard for quick debugging during Play mode
- Mouse swipes still work for testing touch mechanics
- Both inputs can be used interchangeably or simultaneously

### PC Build Support
- All keyboard controls automatically work in PC builds
- No additional configuration or settings menu needed
- Players can use keyboard, mouse, or gamepad (via Unity Input Manager)

## Notes for Developers

1. **Reverse Controls:** The game supports reverse control mode (triggered by certain boss attacks). Both keyboard and touch controls respect this reversal.

2. **Tutorial States:** Controls are validated against tutorial states to ensure proper tutorial flow.

3. **Action Flags:** All input methods set boolean flags that are processed once per frame in `UpdateMovementFromControls()`, ensuring consistent behavior regardless of input source.

4. **Multiple Key Support:** Multiple keys are mapped to the same actions for flexibility and accessibility.

## Future Enhancements

Consider adding:
- Gamepad/controller support (partial support exists via Unity Input Manager)
- Customizable key bindings (would require UI and PlayerPrefs integration)
- Input sensitivity settings for touch controls
- Input display/debugger for testing

---

**Last Updated:** January 17, 2026
**Game Version:** Unity 5.x compatible
**File Modified:** `Assets/Scripts/Runner.cs` - `UpdateControls()` method

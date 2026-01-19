using UnityEngine;

/// This was not in the game originally, this is an addition in order to make the UI scale
/// better on modern devices. Ideally I would just redo the UI with a canvas system
/// but I do not want to do that, so we're keeping the prime31 UIToolkit.
public static class UIScalingHelper
{
    private const float REFERENCE_WIDTH = 1536f;
    private const float REFERENCE_HEIGHT = 2048f;
    private const float REFERENCE_ASPECT = REFERENCE_WIDTH / REFERENCE_HEIGHT; 
    
    public static float AspectRatio => (float)Screen.width / Screen.height;

    public static bool IsModernPhoneAspect => AspectRatio < 0.5625f; 

    public static bool IsTablet => AspectRatio >= 0.7f;

    public static bool HasNotch => 
        Screen.safeArea.y > 1 || 
        Screen.safeArea.x > 1 ||
        Screen.safeArea.width < Screen.width - 2 ||
        Screen.safeArea.height < Screen.height - 2;

    public static float CalcFontScale()
    {
        float baseScale = (float)Screen.height / REFERENCE_HEIGHT;
        
        if (Screen.dpi > 0)
        {
            float dpiScale = Mathf.Clamp(Screen.dpi / 326f, 0.8f, 1.5f); 
            baseScale *= dpiScale;
        }
        
        return Mathf.Clamp(baseScale, 0.5f, 2.5f);
    }
    
    public static float GetTopSafeOffset()
    {
        float safeOffset = Screen.height - (Screen.safeArea.y + Screen.safeArea.height);
        return safeOffset + (safeOffset > 0 ? 10f : 0f);
    }
    
    public static float GetBottomSafeOffset()
    {
        float safeOffset = Screen.safeArea.y;
        return safeOffset + (safeOffset > 0 ? 10f : 0f);
    }
    
    public static float GetLeftSafeOffset()
    {
        return Screen.safeArea.x;
    }
    
    public static float GetRightSafeOffset()
    {
        return Screen.width - (Screen.safeArea.x + Screen.safeArea.width);
    }
    
    public static float GetYPositionFromTop(float percentFromTop, bool respectSafeArea = true)
    {
        if (respectSafeArea && HasNotch)
        {
            float safeHeight = Screen.safeArea.height;
            float topOffset = GetTopSafeOffset();
            return topOffset + (safeHeight * percentFromTop);
        }
        return Screen.height * percentFromTop;
    }
    
    public static float GetYPositionFromBottom(float percentFromBottom, bool respectSafeArea = true)
    {
        if (respectSafeArea && HasNotch)
        {
            float safeHeight = Screen.safeArea.height;
            float bottomOffset = GetBottomSafeOffset();
            return bottomOffset + (safeHeight * percentFromBottom);
        }
        return Screen.height * percentFromBottom;
    }

    public static float ScalePixels(float referencePixels)
    {
        // Use scaleFactor from UI.cs
        return referencePixels * UI.scaleFactor;
    }
    
    public static float GetElementScale()
    {
        float heightScale = (float)Screen.height / REFERENCE_HEIGHT;
        float widthScale = (float)Screen.width / REFERENCE_WIDTH;
        
        float scale = Mathf.Min(heightScale, widthScale);
        

        if (IsModernPhoneAspect)
        {
            scale *= 1.1f;
        }
        
        return Mathf.Clamp(scale, 0.5f, 2f);
    }
    
    public static bool IsNearReferenceAspect(float tolerance = 0.1f)
    {
        return Mathf.Abs(AspectRatio - REFERENCE_ASPECT) < tolerance;
    }
    
    public static float GetHorizontalAspectAdjustment()
    {
        if (IsTablet || IsNearReferenceAspect())
        {
            return 0f;
        }
        
        float expectedWidth = Screen.height * REFERENCE_ASPECT;
        float actualWidth = Screen.width;
        
        return (actualWidth - expectedWidth) / 2f;
    }
    
    public static void LogScreenInfo()
    {
        Debug.Log($"[UIScalingHelper] Screen: {Screen.width}x{Screen.height}");
        Debug.Log($"[UIScalingHelper] DPI: {Screen.dpi}");
        Debug.Log($"[UIScalingHelper] Aspect Ratio: {AspectRatio:F3}");
        Debug.Log($"[UIScalingHelper] Safe Area: {Screen.safeArea}");
        Debug.Log($"[UIScalingHelper] IsModernPhone: {IsModernPhoneAspect}, IsTablet: {IsTablet}, HasNotch: {HasNotch}");
        Debug.Log($"[UIScalingHelper] UI.scaleFactor: {UI.scaleFactor}");
        Debug.Log($"[UIScalingHelper] ElementScale: {GetElementScale():F3}, FontScale: {CalcFontScale():F3}");
    }
}

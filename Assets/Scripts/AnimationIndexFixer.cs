using System.Collections.Generic;
using UnityEngine;

public static class AnimationIndexFixer
{
    private static readonly Dictionary<string, string[]> characterSuffixMap = new Dictionary<string, string[]>()
    {
        { "Perry", new string[] { "", "_2" } },      
        { "Peter", new string[] { "_1", "_0" } },    
        { "Pinky", new string[] { "", "_1", "_2" } }, 
        { "Terry", new string[] { "_0" } }           
    };

    public static string GetAnimationName(string baseAnimName, string characterModelName, Animation animComponent = null)
    {
        if (string.IsNullOrEmpty(characterModelName)) return baseAnimName;

        foreach (var kvp in characterSuffixMap)
        {
            if (characterModelName.Contains(kvp.Key))
            {
                if (animComponent != null)
                {
                    foreach (string suffix in kvp.Value)
                    {
                        string fullName = baseAnimName + suffix;
                        if (animComponent[fullName] != null) 
                        {
                            return fullName; 
                        }
                    }
                }
                
                return baseAnimName + kvp.Value[0];
            }
        }
        return baseAnimName;
    }
}
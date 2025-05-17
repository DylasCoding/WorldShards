using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterClassIcons", menuName = "GameData/CharacterClassIcons")]
public class CharacterClassIcons : ScriptableObject
{
    [System.Serializable]
    public struct ClassIconPair
    {
        public CharacterClass characterClass;
        public Sprite icon;
    }

    public ClassIconPair[] classIcons;

    // Hàm để lấy Sprite dựa trên CharacterClass
    public Sprite GetIcon(CharacterClass characterClass)
    {
        foreach (var pair in classIcons)
        {
            if (pair.characterClass == characterClass)
            {
                return pair.icon;
            }
        }
        return null;
    }
}

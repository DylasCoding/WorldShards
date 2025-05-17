using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementIcons", menuName = "GameData/ElementIcons")]
public class ElementIcons : ScriptableObject
{
    [System.Serializable]
    public struct ElementIconPair
    {
        public ElementType elementType;
        public Sprite icon;
        public Sprite iconGem;
    }
    public ElementIconPair[] elementIcons;

    // Hàm để lấy Sprite dựa trên ElementType
    public Sprite GetIcon(ElementType elementType)
    {
        foreach (var pair in elementIcons)
        {
            if (pair.elementType == elementType)
            {
                return pair.icon;
            }
        }
        return null;
    }

    public Sprite GetIconGem(ElementType elementType)
    {
        foreach (var pair in elementIcons)
        {
            if (pair.elementType == elementType)
            {
                return pair.iconGem;
            }
        }
        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementConfig", menuName = "GameData/Element")]
public class ElementData : ScriptableObject
{
    public enum ElementType
    {
        Fire,
        Water,
        Wood,
        Light,
        Dark,
    }

    [Header("Element Data")]
    public ElementType elementType = ElementType.Fire;
    public Sprite elementIcon;
    public Color elementColor;
    public string elementDescription;

    [Header("Element Effects")]
    public Sprite frameSprite;
}

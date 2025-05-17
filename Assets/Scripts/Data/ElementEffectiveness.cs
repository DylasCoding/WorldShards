using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ElementEffectiveness
{
    private static readonly Dictionary<(ElementType, ElementType), float> ElementBonus = new()
    {
        {(ElementType.Wood, ElementType.Fire), 0.5f},
        {(ElementType.Wood, ElementType.Water), 1.5f},

        {(ElementType.Fire, ElementType.Water), 0.5f},
        {(ElementType.Fire, ElementType.Wood), 1.5f},


        {(ElementType.Water, ElementType.Fire), 1.5f},
        {(ElementType.Water, ElementType.Wood), 0.5f},
    };

    public static float GetElementBonus(ElementType elementAttack, ElementType elementDefense)
    {
        return ElementBonus.TryGetValue((elementAttack, elementDefense), out float bonus) ? bonus : 1.0f;
    }
}

[System.Serializable]
public enum ElementType
{
    None,
    Fire,
    Water,
    Wood
}

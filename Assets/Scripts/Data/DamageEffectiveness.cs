using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageEffectiveness
{
    private static readonly Dictionary<(CharacterClass, CharacterClass), float> Effectiveness = new()
    {
        {(CharacterClass.Warrior, CharacterClass.Mage), 1.5f},
        {(CharacterClass.Warrior, CharacterClass.Archer), 1.2f},
        {(CharacterClass.Warrior, CharacterClass.Tank), 0.8f},

        {(CharacterClass.SwordMaster, CharacterClass.Rogue), 1.5f},
        {(CharacterClass.SwordMaster, CharacterClass.Warrior), 1.2f},
        {(CharacterClass.SwordMaster, CharacterClass.Tank), 0.7f},

        {(CharacterClass.Mage, CharacterClass.Tank), 1.5f},
        {(CharacterClass.Mage, CharacterClass.Warrior), 0.7f},
        {(CharacterClass.Mage, CharacterClass.Monk), 1.3f},

        {(CharacterClass.Archer, CharacterClass.Mage), 1.3f},
        {(CharacterClass.Archer, CharacterClass.Healer), 1.4f},

        {(CharacterClass.Rogue, CharacterClass.Mage), 1.4f},
        {(CharacterClass.Rogue, CharacterClass.Healer), 1.3f},
        {(CharacterClass.Rogue, CharacterClass.Tank), 0.6f},

        {(CharacterClass.Tank, CharacterClass.Rogue), 1.2f},
        {(CharacterClass.Tank, CharacterClass.Archer), 0.8f},

        {(CharacterClass.Monk, CharacterClass.Mage), 1.3f},
        {(CharacterClass.Monk, CharacterClass.Rogue), 1.1f}
    };

    public static float GetMultiplier(CharacterClass attacker, CharacterClass defender)
    {
        return Effectiveness.TryGetValue((attacker, defender), out float multiplier) ? multiplier : 1.0f;
    }
}


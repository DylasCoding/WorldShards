using UnityEngine;

[System.Serializable]
public class DamageCalculator
{
    public int CalculateDamage(int skillDamage, int level, CharacterData characterData, CharacterClass attackerClass, CharacterData characterDataDefense, SkillData skill)
    {
        float classMultiplier = DamageEffectiveness.GetMultiplier(attackerClass, characterDataDefense.characterType);
        float elementBonus = ElementEffectiveness.GetElementBonus(skill.elementType, characterDataDefense.elementType);

        float damage = (characterData.attackDamage + (level * characterData.incrementalAttack) + skillDamage)
                       * classMultiplier * elementBonus;

        if (skill.damageMultiplier != 0)
        {
            damage *= skill.damageMultiplier;
        }

        Debug.Log($"Base Damage: {characterData.attackDamage}, Level: {level}, Incremental Attack: {characterData.incrementalAttack}, Skill Damage: {skillDamage}");
        Debug.Log($"Class Multiplier: {classMultiplier}, Element Bonus: {elementBonus}, Total Damage: {damage}");

        return Mathf.RoundToInt(damage);
    }

}

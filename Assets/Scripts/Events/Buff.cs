using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buff
{
    public GameObject buffPrefab; // Buff prefab to be instantiated
    // public CharController target; // Target character to apply the buff to
    public int remainingTurns; // Number of turns the buff will last
    public SkillData skillData; // Skill data associated with the buff

    public Buff(GameObject buffPrefab, int remainingTurns, SkillData skillData)
    {
        this.buffPrefab = buffPrefab;
        // this.target = target;
        this.remainingTurns = remainingTurns;
        this.skillData = skillData;
    }
}

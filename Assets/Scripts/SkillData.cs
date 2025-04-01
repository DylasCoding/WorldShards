using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillConfig", menuName = "Skill/Create New Skill")]
public class SkillData : ScriptableObject
{
        public string skillName;
        public string description;
        public GameObject skillEffectPrefab;
        public AnimationClip skillAnimation;

        [Header("Damage Settings")]
        public int damageIncrease;
        public float damageMultiplier;

        [Header("Mana Settings")]
        public int manaCost;
        public int manaRegen;

        [Header("Health Settings")]
        public int hpCost;
        public int hpRegen;

        [Header("Movement Settings")]
        public bool isMovementSkill;
        public Vector2 targetPositionOffset = new Vector2(8.5f, 0);
        public float animationEventTime = 0.5f;

        [Header("Jump Settings")]
        public bool isJumpSkill;
        public Vector2 jumpForce = new Vector2(6, 2.7f);
        public float jumpTime;
        public float jumpDelay;
}

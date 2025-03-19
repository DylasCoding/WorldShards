using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharConfig", menuName = "Character/Create New Character")]
public class CharacterData : ScriptableObject
{
        public string characterName;
        public int maxHealth;
        public int attackDamage;
        public Sprite characterSprite;
        public Sprite characterImage;
        public RuntimeAnimatorController animatorController;

        [Header("Skills")]
        public SkillData basicAttack;  // Đánh thường
        public SkillData specialSkill1; // Kỹ năng đặc biệt 1
        public SkillData specialSkill2; // Kỹ năng đặc biệt 2
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharConfig", menuName = "Character/Create New Character")]
public class CharacterData : ScriptableObject
{
        [Header("Character Info")]
        public int characterID;
        public string characterName;
        public CharacterClass characterType;
        public ElementType elementType = ElementType.None;

        [Header("Character Stats")]
        public int maxHealth;
        public int attackDamage;
        public int defense = 0;

        [Header("Character Appearance")]
        public Sprite characterSprite;
        public Sprite characterImage;
        public Sprite CharacterUltimateImage;
        public RuntimeAnimatorController animatorController;

        [Header("Character Incremental Stats")]
        public int incrementalHealth = 10;
        public int incrementalAttack = 2;
        public int incrementalDefense = 1;

        [Header("Audio")]
        public AudioClip turnActionAudioClip;
        public AudioClip damageAudioClip;
        public AudioClip defeatAudioClip;

        [Header("Skills")]
        public SkillData basicAttack;  // Đánh thường
        public SkillData specialSkill1; // Kỹ năng đặc biệt 1
        public SkillData specialSkill2; // Kỹ năng đặc biệt 2
}


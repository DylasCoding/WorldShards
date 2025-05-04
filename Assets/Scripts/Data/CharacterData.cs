using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharConfig", menuName = "Character/Create New Character")]
public class CharacterData : ScriptableObject
{
        public enum CharacterType
        {
                Warrior,
                SwordMaster,
                Mage,
                Archer,
                Rogue,
                Healer,
                Tank,
                Monk,
        }
        [Header("Character Info")]
        public int characterID;
        public string characterName;
        public CharacterType characterType;
        public int maxHealth;
        public int attackDamage;
        public int defense = 0;

        [Header("Character Appearance")]
        public Sprite characterSprite;
        public Sprite characterImage;
        public Sprite CharacterUltimateImage;
        public RuntimeAnimatorController animatorController;

        public int rarity = 3; // 1: Common, 2: Rare, 3: Epic, 4: Legendary

        [Header("Audio")]
        public AudioClip turnActionAudioClip;
        public AudioClip damageAudioClip;
        public AudioClip defeatAudioClip;

        [Header("Skills")]
        public SkillData basicAttack;  // Đánh thường
        public SkillData specialSkill1; // Kỹ năng đặc biệt 1
        public SkillData specialSkill2; // Kỹ năng đặc biệt 2
}


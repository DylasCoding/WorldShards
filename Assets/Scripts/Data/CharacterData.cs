using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharConfig", menuName = "Character/Create New Character")]
public class CharacterData : ScriptableObject
{
        [Header("Character Info")]
        public int characterID;
        public string characterName;
        public int maxHealth;
        public int attackDamage;

        [Header("Character Appearance")]
        public Sprite characterSprite;
        public Sprite characterImage;
        public RuntimeAnimatorController animatorController;

        [Header("Audio")]
        public AudioClip turnActionAudioClip;
        public AudioClip damageAudioClip;
        public AudioClip defeatAudioClip;

        [Header("Skills")]
        public SkillData basicAttack;  // Đánh thường
        public SkillData specialSkill1; // Kỹ năng đặc biệt 1
        public SkillData specialSkill2; // Kỹ năng đặc biệt 2
}


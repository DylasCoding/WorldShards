using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Character/Database")]
public class CharacterDatabase : ScriptableObject
{
    public List<CharacterData> characters;
}
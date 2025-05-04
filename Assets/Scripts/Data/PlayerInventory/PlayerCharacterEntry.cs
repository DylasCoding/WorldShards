using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCharacterEntry
{
    public CharacterData characterData;
    public int level;
    public bool isUnlocked;
}

[CreateAssetMenu(fileName = "PlayerCharacterInventory", menuName = "Character/Player Inventory")]
public class PlayerCharacterInventory : ScriptableObject
{
    public List<PlayerCharacterEntry> ownedCharacters;
}

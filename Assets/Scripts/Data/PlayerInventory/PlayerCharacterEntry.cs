using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCharacterEntry
{
    public CharacterData characterData;
    public int level = 1;
    public bool isUnlocked = true;
}

[CreateAssetMenu(fileName = "PlayerCharacterInventory", menuName = "Character/Player Inventory")]
public class PlayerCharacterInventory : ScriptableObject
{
    public List<PlayerCharacterEntry> ownedCharacters;
}

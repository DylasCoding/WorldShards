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

[System.Serializable]
public class PlayerCharacterInventory
{
    public List<OwnedCharacter> ownedCharacters = new();
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializablePlayerCharacterEntry
{
    public int characterID;
    public int level;
    public bool isUnlocked = true;
}

[System.Serializable]
public class SerializablePlayerInventory
{
    public List<SerializablePlayerCharacterEntry> ownedCharacters = new();
}


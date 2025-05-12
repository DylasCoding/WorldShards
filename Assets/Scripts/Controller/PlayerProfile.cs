using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;

[System.Serializable]
public struct PlayerProfile
{
    public PlayerInfo playerInfo;
    public string Name;
    public int Gems;
    public int Feathers;
    public int Level;

    public List<OwnedCharacter> ownedCharacters;
    public List<TeamMember> team;
}

[System.Serializable]
public struct OwnedCharacter
{
    public int characterID;
    public int level;
    public bool isUnlocked;
}

[System.Serializable]
public struct TeamMember
{
    public int characterID;
    public int level;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStateJson
{
    public int characterID;
    public int level;
}

[System.Serializable]
public class TeamJsonWrapper
{
    public List<CharacterStateJson> team = new List<CharacterStateJson>();
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TeamManager", menuName = "Character/TeamManager")]
public class TeamManager : ScriptableObject
{
    [System.Serializable]
    public class CharacterState
    {
        public CharacterData characterData;
        public int currentHealth;

        public CharacterState(CharacterData characterData)
        {
            this.characterData = characterData;
            this.currentHealth = characterData.maxHealth;
        }
    }

    [SerializeField] private List<CharacterState> team = new List<CharacterState>(4);

    public void InitializeTeam()
    {
        foreach (var character in team)
        {
            if (character.characterData != null)
            {
                character.currentHealth = character.characterData.maxHealth;
            }
        }
    }

    public void SetUpCharacter(CharacterData[] characters)
    {
        if (characters.Length > 4)
        {
            Debug.LogError("Team can't have more than 4 characters");
            return;
        }
        team.Clear();
        foreach (CharacterData character in characters)
        {
            team.Add(new CharacterState(character));
        }
    }

    public CharacterData GetCharacterData(int index)
    {
        if (index >= 0 && index < team.Count)
            return team[index].characterData;
        return null;
    }

    public int GetCurrentHealth(int index)
    {
        if (index < 0 || index >= team.Count)
        {
            Debug.LogError("Invalid index");
            return -1;
        }
        return team[index].currentHealth;
    }

    public void UpdateCurrentHealth(int index, int health)
    {
        if (index < 0 || index >= team.Count)
        {
            Debug.LogError("Invalid index");
            return;
        }
        team[index].currentHealth = health;
    }


    public int GetMaxHealth(int index)
    {
        if (index < 0 || index >= team.Count)
        {
            Debug.LogError("Invalid index");
            return -1;
        }
        return team[index].characterData.maxHealth;
    }

    public void SwapCharacters(int indexA, int indexB)
    {
        if (indexA < 0 || indexA >= team.Count || indexB < 0 || indexB >= team.Count)
        {
            Debug.LogError("Invalid index for swapping");
            return;
        }

        CharacterState temp = team[indexA];
        team[indexA] = team[indexB];
        team[indexB] = temp;
    }

    private void OnEnable()
    {
        InitializeTeam();
    }
}

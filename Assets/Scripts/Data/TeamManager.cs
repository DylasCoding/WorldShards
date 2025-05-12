using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "TeamManager", menuName = "GameData/TeamManager")]
public class TeamManager : ScriptableObject
{
    [SerializeField] private string filename = "player_team.json";
    private string savePath => Path.Combine(Application.persistentDataPath, filename);

    public enum TeamType
    {
        Player,
        Enemy
    }
    public TeamType teamType = TeamType.Enemy;

    [SerializeField] private CharacterDatabase characterDatabase;

    [System.Serializable]
    public class CharacterState
    {
        public CharacterData characterData;
        public int level = 1;
        public int currentHealth;
        public List<Buff> activeBuffs = new List<Buff>(); // List to store active buffs

        public CharacterState(CharacterData characterData)
        {
            this.characterData = characterData;
            this.currentHealth = characterData.maxHealth;
        }
    }

    [SerializeField] private List<CharacterState> team = new List<CharacterState>(4);

    private void Awake()
    {
        LoadTeamFromJson();
        InitializeTeam();
    }

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

    public CharacterState GetCharacterState(int index)
    {
        if (index >= 0 && index < team.Count)
            return team[index];
        return null;
    }

    public void SetCharacterSate(int index, PlayerCharacterEntry playerCharacterEntry)
    {
        if (index < 0 || index >= team.Count)
        {
            Debug.LogError("Invalid index");
            return;
        }
        team[index].characterData = playerCharacterEntry.characterData;
        team[index].level = playerCharacterEntry.level;
        team[index].currentHealth = playerCharacterEntry.characterData.maxHealth;
    }

    public int GetTeamSize()
    {
        return team.Count;
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

    public void ApplyBuff(int index, int turns, SkillData skillData, Transform targetTransform)
    {
        if (index < 0 || index >= team.Count)
        {
            Debug.LogError("Invalid index for applying buff");
            return;
        }
        //add effect to character
        GameObject buffInstance = Instantiate(skillData.buffPrefab, targetTransform.position, Quaternion.identity);
        buffInstance.transform.SetParent(targetTransform); // Set the parent to the character's transform
        Buff newBuff = new Buff(buffInstance, turns, skillData);
        team[index].activeBuffs.Add(newBuff);

        Debug.Log($"Buff on character {index} has {newBuff.remainingTurns} turns left");
    }

    public void UpdateBuffs(int index)
    {
        if (index < 0 || index >= team.Count)
        {
            Debug.LogError("Invalid index for updating buff");
            return;
        }
        if (team[index].activeBuffs.Count == 0)
        {
            Debug.Log("No active buffs to update");
            return;
        }
        for (int i = team[index].activeBuffs.Count - 1; i >= 0; i--)
        {
            Buff buff = team[index].activeBuffs[i];
            buff.remainingTurns--;

            Debug.Log($"Buff on character {index} has {buff.remainingTurns} turns left");
            if (buff.remainingTurns < 0)
            {
                Destroy(buff.buffPrefab); // Destroy the buff prefab
                team[index].activeBuffs.RemoveAt(i); // Remove the buff from the list
                Debug.Log($"Buff on character {index} has expired");
            }
        }
    }

    public void GetAllBuffs(int index, Transform charPosition)
    {
        if (index < 0 || index >= team.Count) { return; }
        if (team[index].activeBuffs.Count == 0) { return; }

        for (int i = 0; i < team[index].activeBuffs.Count; i++)
        {
            Buff buff = team[index].activeBuffs[i];

            buff.buffPrefab.SetActive(true); // Show the buff prefab

            // GameObject instance = Instantiate(buff.skillData.buffPrefab, charPosition.position, Quaternion.identity);
            // instance.transform.SetParent(charPosition); // Set the parent to the character's transform
        }
    }

    public void CancelBuff(int index)
    {
        if (index < 0 || index >= team.Count)
        {
            Debug.LogError("Invalid index for cancelling buff");
            return;
        }
        for (int i = team[index].activeBuffs.Count - 1; i >= 0; i--)
        {
            Buff buff = team[index].activeBuffs[i];
            // Destroy(buff.buffPrefab); // Destroy the buff prefab
            // team[index].activeBuffs.RemoveAt(i); // Remove the buff from the list

            //hide buff prefab but not destroy it
            buff.buffPrefab.SetActive(false);
        }
    }

    public void CancelAllBuffs()
    {
        for (int i = 0; i < team.Count; i++)
        {
            CancelBuff(i);
        }
    }

    public void DestroyAllBuffs()
    {
        for (int i = 0; i < team.Count; i++)
        {
            for (int j = team[i].activeBuffs.Count - 1; j >= 0; j--)
            {
                Buff buff = team[i].activeBuffs[j];
                Destroy(buff.buffPrefab); // Destroy the buff prefab
                team[i].activeBuffs.RemoveAt(j); // Remove the buff from the list
            }
        }
    }

    public void SaveTeamToJson()
    {
        TeamJsonWrapper wrapper = new TeamJsonWrapper();
        foreach (var member in team)
        {
            wrapper.team.Add(new TeamMember
            {
                characterID = member.characterData.characterID,
                level = member.level,
            });
        }

        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    public void LoadTeamFromJson()
    {
        List<CharacterState> loadedTeam = new List<CharacterState>();
        if (teamType == TeamType.Player)
        {
            List<PlayerCharacterEntry> ownedCharacters = GameData.ownedCharacters;
            loadedTeam = TeamLoader.PlayerLoadFromJson(savePath, characterDatabase.characters, ownedCharacters);
            if (loadedTeam == null)
            {
                Debug.LogError("Failed to load team from JSON.");
                return;
            }

        }
        else
        {
            loadedTeam = TeamLoader.EnemyLoadFromJson(savePath, characterDatabase.characters);
        }

        if (loadedTeam != null)
        {
            team = loadedTeam;
            if (teamType == TeamType.Player)
                SaveTeamToJson();
            InitializeTeam();
            // Debug.Log("Team loaded successfully from JSON.");
        }
        else
        {
            Debug.LogError("Failed to load team from JSON.");
        }
    }

    public bool IsCharacterInTeam(CharacterData characterData)
    {
        foreach (var character in team)
        {
            if (character.characterData == characterData)
            {
                return true;
            }
        }
        return false;
    }

    private void OnEnable()
    {
        LoadTeamFromJson();
    }
}

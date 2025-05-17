using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

#if UNITY_ANDROID
using UnityEngine.Networking;
#endif

public static class TeamLoader
{
    [Serializable]
    private class TeamJsonWrapper
    {
        public List<TeamEntry> team;
    }

    [Serializable]
    private class TeamEntry
    {
        public string characterID;
        public int level;
    }

    public static List<TeamManager.CharacterState> PlayerLoadFromJson(string jsonPath, List<CharacterData> allCharacters, List<PlayerCharacterEntry> ownedCharacters)
    {
        if (!File.Exists(jsonPath))
        {
            Debug.LogError($"Team JSON file not found at: {jsonPath}");
            return null;
        }

        List<TeamManager.CharacterState> loadedTeam = new List<TeamManager.CharacterState>();

        string json = File.ReadAllText(jsonPath);
        TeamJsonWrapper wrapper = JsonUtility.FromJson<TeamJsonWrapper>(json);

        foreach (var entry in wrapper.team)
        {
            // Tìm character trong ownedCharacters dựa trên characterID
            PlayerCharacterEntry foundEntry = ownedCharacters.Find(c => c.characterData.characterID == int.Parse(entry.characterID));
            CharacterData found = foundEntry?.characterData;

            if (found != null)
            {
                // Lấy level từ ownedCharacters thay vì entry.level
                var state = new TeamManager.CharacterState(found)
                {
                    level = foundEntry.level,
                    currentHealth = foundEntry.characterData.maxHealth + (foundEntry.level * foundEntry.characterData.incrementalHealth),
                };
                Debug.Log($"Player ID: {entry.characterID}, Level: {state.level}, Current Health: {state.currentHealth}");
                loadedTeam.Add(state);
            }
            else
            {
                Debug.LogWarning($"Character not found for ID: {entry.characterID}");
            }
        }

        return loadedTeam;
    }

    public static List<TeamManager.CharacterState> EnemyLoadFromJson(string jsonPath, List<CharacterData> allCharacters)
    {
        string path = Path.Combine(Application.streamingAssetsPath, jsonPath);
        if (!File.Exists(path))
        {
            Debug.LogError($"Team JSON file not found at: {path}");
            return null;
        }

        List<TeamManager.CharacterState> loadedTeam = new List<TeamManager.CharacterState>();

        string json = File.ReadAllText(path);
        TeamJsonWrapper wrapper = JsonUtility.FromJson<TeamJsonWrapper>(json);

        foreach (var entry in wrapper.team)
        {
            CharacterData found = allCharacters.Find(c => c.characterID == int.Parse(entry.characterID));
            if (found != null)
            {
                var state = new TeamManager.CharacterState(found)
                {
                    level = entry.level,
                    currentHealth = found.maxHealth + (entry.level * found.incrementalHealth),
                };
                Debug.Log($"Enemy ID: {entry.characterID}, Level: {entry.level}, Current Health: {state.currentHealth}");
                loadedTeam.Add(state);
            }
            else
            {
                Debug.LogWarning($"Character not found for ID: {entry.characterID}");
            }
        }

        return loadedTeam;
    }
}

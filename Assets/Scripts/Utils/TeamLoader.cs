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

    public static List<TeamManager.CharacterState> LoadFromStreamingAssets(string filename, List<CharacterData> allCharacters)
    {
        string path = Path.Combine(Application.streamingAssetsPath, filename);
        string json = "";

#if UNITY_ANDROID
        UnityWebRequest www = UnityWebRequest.Get(path);
        www.SendWebRequest();
        while (!www.isDone) { }

        if (www.result == UnityWebRequest.Result.Success)
        {
            json = www.downloadHandler.text;
        }
        else
        {
            Debug.LogError($"Failed to load team file: {www.error} | Path: {path}");
            return null;
        }
#else
        Debug.Log($"Attempting to load file from path: {path}");
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
        }
        else
        {
            Debug.LogError("Team file not found at: " + path);
            return null;
        }
#endif

        TeamJsonWrapper wrapper = JsonUtility.FromJson<TeamJsonWrapper>(json);
        List<TeamManager.CharacterState> loadedTeam = new List<TeamManager.CharacterState>();

        foreach (var entry in wrapper.team)
        {
            CharacterData found = allCharacters.Find(c => c.characterID == int.Parse(entry.characterID));
            if (found != null)
            {
                var state = new TeamManager.CharacterState(found)
                {
                    level = entry.level
                };
                loadedTeam.Add(state);
            }
            else
            {
                Debug.LogWarning($"Character not found: {entry.characterID}");
            }
        }

        return loadedTeam;
    }
}

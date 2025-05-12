using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

public class PlayerInventoryManager : MonoBehaviour
{
    public CharacterDatabase characterDatabase; // Drag từ Editor
    public List<PlayerCharacterEntry> ownedCharacters = new(); // Runtime data

    private string savePath => Path.Combine(Application.persistentDataPath, "player_inventory.json");

    private void Awake()
    {
        LoadFromJson();
    }

    public void SaveToJson()
    {
        PlayerCharacterInventory saveData = new();

        foreach (var entry in ownedCharacters)
        {
            saveData.ownedCharacters.Add(new OwnedCharacter
            {
                characterID = entry.characterData.characterID,
                level = entry.level,
                isUnlocked = entry.isUnlocked
            });
        }

        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(savePath, json);
        Debug.Log("Saved to: " + savePath);

        SyncOwnedCharactersToCloud().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to sync to cloud: " + task.Exception);
            }
            else
            {
                Debug.Log("Synced to cloud successfully.");
            }
        });
    }

    public void LoadFromJson()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found.");
            return;
        }

        string json = File.ReadAllText(savePath);
        PlayerCharacterInventory saveData = JsonUtility.FromJson<PlayerCharacterInventory>(json);

        ownedCharacters.Clear();

        foreach (var entry in saveData.ownedCharacters)
        {
            CharacterData data = characterDatabase.characters.Find(c => c.characterID == entry.characterID);
            if (data != null)
            {
                ownedCharacters.Add(new PlayerCharacterEntry
                {
                    characterData = data,
                    level = entry.level,
                    isUnlocked = entry.isUnlocked
                });
            }
        }
        GameData.ownedCharacters = ownedCharacters;

        Debug.Log("Inventory loaded.");
    }

    public async Task SyncOwnedCharactersToCloud()
    {
        PlayerCharacterInventory saveData = new();

        foreach (var entry in ownedCharacters)
        {
            saveData.ownedCharacters.Add(new OwnedCharacter
            {
                characterID = entry.characterData.characterID,
                level = entry.level,
                isUnlocked = entry.isUnlocked
            });
        }

        string json = JsonUtility.ToJson(saveData);
        await Unity.Services.CloudSave.CloudSaveService.Instance.Data.Player.SaveAsync(new Dictionary<string, object>
    {
        { "OwnedCharacters", json }
    });

        Debug.Log("Synced OwnedCharacters to Cloud.");
    }

    public void UpdateOwnedCharacters(List<OwnedCharacter> newOwnedCharacters)
    {
        ownedCharacters.Clear();

        foreach (var entry in newOwnedCharacters)
        {
            CharacterData data = characterDatabase.characters.Find(c => c.characterID == entry.characterID);
            if (data != null)
            {
                ownedCharacters.Add(new PlayerCharacterEntry
                {
                    characterData = data,
                    level = entry.level,
                    isUnlocked = entry.isUnlocked
                });
            }
        }

        SaveToJson();
    }

}

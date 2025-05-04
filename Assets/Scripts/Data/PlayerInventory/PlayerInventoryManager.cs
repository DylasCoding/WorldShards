using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerInventoryManager : MonoBehaviour
{
    public CharacterDatabase characterDatabase; // Drag từ Editor
    public List<PlayerCharacterEntry> ownedCharacters = new(); // Runtime data

    private string savePath => Path.Combine(Application.persistentDataPath, "player_inventory.json");

    public void SaveToJson()
    {
        SerializablePlayerInventory saveData = new();

        foreach (var entry in ownedCharacters)
        {
            saveData.ownedCharacters.Add(new SerializablePlayerCharacterEntry
            {
                characterID = entry.characterData.characterID,
                level = entry.level,
                isUnlocked = entry.isUnlocked
            });
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Saved to: " + savePath);
    }

    public void LoadFromJson()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found.");
            return;
        }

        string json = File.ReadAllText(savePath);
        SerializablePlayerInventory saveData = JsonUtility.FromJson<SerializablePlayerInventory>(json);

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

        Debug.Log("Inventory loaded.");
    }
}

using System.IO;
using UnityEngine;

[System.Serializable]
public class AuthData
{
    public string accessToken;
    public string playerId;
    public string playerName;
}

public class AuthStorage
{
    private static string FilePath => Application.persistentDataPath + "/auth.json";

    public static void Save(AuthData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(FilePath, json); // Tự động đè nếu đã tồn tại
        Debug.Log("Saved to: " + FilePath);
    }

    public static AuthData Load()
    {
        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            return JsonUtility.FromJson<AuthData>(json);
        }
        else
        {
            Debug.LogWarning("No auth.json found");
            return null;
        }
    }

    public static void Delete()
    {
        if (File.Exists(FilePath)) File.Delete(FilePath);
    }
}

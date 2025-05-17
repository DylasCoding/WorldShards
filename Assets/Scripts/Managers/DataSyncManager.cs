using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;

public static class DataSyncManager
{
    public static async Task SaveGems(int gems)
    {
        await CloudSaveService.Instance.Data.Player.SaveAsync(new Dictionary<string, object>
        {
            { "Gems", gems }
        });
        Debug.Log("Gems saved.");
    }

    public static async Task SaveFeathers(int feathers)
    {
        await CloudSaveService.Instance.Data.Player.SaveAsync(new Dictionary<string, object>
        {
            { "Feathers", feathers }
        });
        Debug.Log("Feathers saved.");
    }

    public static async Task SaveName(string name)
    {
        await CloudSaveService.Instance.Data.Player.SaveAsync(new Dictionary<string, object>
        {
            { "Name", name }
        });
        Debug.Log("Name saved.");
    }

    public static async Task SaveLevel(int level)
    {
        await CloudSaveService.Instance.Data.Player.SaveAsync(new Dictionary<string, object>
        {
            { "Level", level }
        });
        Debug.Log("Level saved.");
    }

    public static async Task SaveReward(int gem, int feather, int level)
    {
        await CloudSaveService.Instance.Data.Player.SaveAsync(new Dictionary<string, object>
        {
            { "Gems", gem },
            { "Feathers", feather },
            { "Level", level }
        });

        var profile = LoginController.Instance.PlayerProfile;
        profile.Gems = gem;
        profile.Feathers = feather;
        profile.Level = level;

        LoginController.Instance.UpdatePlayerProfileWithoutSave(profile);
        Debug.Log("Reward saved.");
    }

    public static async Task SaveTeam(List<TeamMember> team)
    {
        string json = JsonUtility.ToJson(new Wrapper<TeamMember>(team));
        await CloudSaveService.Instance.Data.Player.SaveAsync(new Dictionary<string, object>
        {
            { "Team", json }
        });
        Debug.Log("Team saved.");
    }

    public static async Task SaveOwnedCharacters(List<OwnedCharacter> ownedChars)
    {
        string json = JsonUtility.ToJson(new Wrapper<OwnedCharacter>(ownedChars));
        await CloudSaveService.Instance.Data.Player.SaveAsync(new Dictionary<string, object>
        {
            { "OwnedCharacters", json }
        });
        Debug.Log("OwnedCharacters saved.");
    }

    public static async Task DeleteAllPlayerData()
    {
        var keysToDelete = new HashSet<string>
        {
            "Name", "Gems", "Feathers", "Level", "Team", "OwnedCharacters"
        };

        foreach (var key in keysToDelete)
        {
#pragma warning disable CS0618
            await CloudSaveService.Instance.Data.ForceDeleteAsync(key);
#pragma warning restore CS0618
        }
        Debug.Log("All player data deleted.");
    }
}

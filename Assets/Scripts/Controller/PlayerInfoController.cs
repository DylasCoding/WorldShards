// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Unity.Services.Authentication;
// using Unity.Services.Authentication.PlayerAccounts;
// using Unity.Services.CloudSave;
// using UnityEngine;

// public class PlayerInfoController
// {
//     public PlayerProfile PlayerProfile { get; private set; }

//     public PlayerInfoController(PlayerProfile playerProfile)
//     {
//         this.PlayerProfile = playerProfile;
//     }

//     public void UpdatePlayerName(string newName)
//     {
//         PlayerProfile updatedProfile = PlayerProfile;
//         updatedProfile.Name = newName;
//         PlayerProfile = updatedProfile;
//         SavePlayerNameAsync(newName);
//     }

//     public string GetPlayerName()
//     {
//         return PlayerProfile.Name;
//     }

//     public void UpdateGems(int gems)
//     {
//         PlayerProfile updatedProfile = PlayerProfile;
//         updatedProfile.Gems = gems;
//         PlayerProfile = updatedProfile;
//         SaveGemsAsync(gems);
//     }

//     public void UpdateFeathers(int feathers)
//     {
//         PlayerProfile updatedProfile = PlayerProfile;
//         updatedProfile.Feathers = feathers;
//         PlayerProfile = updatedProfile;
//         SaveFeathersAsync(feathers);
//     }

//     public void UpdateLevel(int level)
//     {
//         PlayerProfile updatedProfile = PlayerProfile;
//         updatedProfile.Level = level;
//         PlayerProfile = updatedProfile;
//         SaveLevelAsync(level);
//     }

//     public async Task LoadPlayerDataAsync()
//     {
//         try
//         {
//             var keys = new HashSet<string> { "name", "gems", "feathers", "level" };
//             var data = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

//             if (data == null || data.Count == 0)
//             {
//                 Debug.Log("No player data found, saving default data.");
//                 await SaveDefaultDataAsync();
//                 return;
//             }

//             PlayerProfile updatedProfile = PlayerProfile;

//             // updatedProfile = DefaultValue(updatedProfile);

//             // Cập nhật từ Cloud Save nếu có
//             if (data.TryGetValue("name", out var nameData))
//             {
//                 string name = nameData.Value.ToString();
//                 updatedProfile.Name = name;
//             }
//             if (data.TryGetValue("gems", out var gemsData) && int.TryParse(gemsData.Value.ToString(), out int gems))
//             {
//                 updatedProfile.Gems = gems;
//             }
//             if (data.TryGetValue("feathers", out var feathersData) && int.TryParse(feathersData.Value.ToString(), out int feathers))
//             {
//                 updatedProfile.Feathers = feathers;
//             }
//             if (data.TryGetValue("level", out var levelData) && int.TryParse(levelData.Value.ToString(), out int level))
//             {
//                 updatedProfile.Level = level;
//             }

//             PlayerProfile = updatedProfile;

//             Debug.Log($"Player Data Loaded: Name={PlayerProfile.Name}, Gems={PlayerProfile.Gems}, Feathers={PlayerProfile.Feathers}, Level={PlayerProfile.Level}");
//         }
//         catch (System.Exception ex)
//         {
//             Debug.LogError($"Failed to load player data: {ex.Message}");
//         }
//     }

//     private async Task SaveDefaultDataAsync()
//     {
//         try
//         {
//             var data = new Dictionary<string, object>
//             {
//                 { "name", PlayerProfile.Name ?? "DefaultPlayer" },
//                 { "gems", 0 },
//                 { "feathers", 0 },
//                 { "level", 1 }
//             };
//             await CloudSaveService.Instance.Data.Player.SaveAsync(data);
//             Debug.Log("Default player data saved successfully.");
//         }
//         catch (System.Exception ex)
//         {
//             Debug.LogError($"Failed to save default player data: {ex.Message}");
//         }
//     }

//     private async void SavePlayerNameAsync(string name)
//     {
//         try
//         {
//             var data = new Dictionary<string, object> { { "name", name } };
//             await CloudSaveService.Instance.Data.Player.SaveAsync(data);
//             Debug.Log($"Player name saved: {name}");
//         }
//         catch (System.Exception ex)
//         {
//             Debug.LogError($"Failed to save player name: {ex.Message}");
//         }
//     }

//     private async void SaveGemsAsync(int gems)
//     {
//         try
//         {
//             var data = new Dictionary<string, object> { { "gems", gems } };
//             await CloudSaveService.Instance.Data.Player.SaveAsync(data);
//             Debug.Log($"Gems saved: {gems}");
//         }
//         catch (System.Exception ex)
//         {
//             Debug.LogError($"Failed to save gems: {ex.Message}");
//         }
//     }

//     private async void SaveFeathersAsync(int feathers)
//     {
//         try
//         {
//             var data = new Dictionary<string, object> { { "feathers", feathers } };
//             await CloudSaveService.Instance.Data.Player.SaveAsync(data);
//             Debug.Log($"Feathers saved: {feathers}");
//         }
//         catch (System.Exception ex)
//         {
//             Debug.LogError($"Failed to save feathers: {ex.Message}");
//         }
//     }

//     private async void SaveLevelAsync(int level)
//     {
//         try
//         {
//             var data = new Dictionary<string, object> { { "level", level } };
//             await CloudSaveService.Instance.Data.Player.SaveAsync(data);
//             Debug.Log($"Level saved: {level}");
//         }
//         catch (System.Exception ex)
//         {
//             Debug.LogError($"Failed to save level: {ex.Message}");
//         }
//     }

//     private static PlayerProfile DefaultValue(PlayerProfile updatedProfile)
//     {
//         // Mặc định nếu không có dữ liệu
//         updatedProfile.Name = updatedProfile.Name ?? "DefaultPlayer";
//         updatedProfile.Gems = 0;
//         updatedProfile.Feathers = 0;
//         updatedProfile.Level = 1;
//         return updatedProfile;
//     }
// }
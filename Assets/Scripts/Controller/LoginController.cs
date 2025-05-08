using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class LoginController : MonoBehaviour
{
    public static LoginController Instance { get; private set; }
    public event Action<PlayerProfile> OnSignedIn;
    // public event Action<PlayerProfile> OnAvatarUpdate;

    private string playerInventoryPath => Path.Combine(Application.persistentDataPath, "player_inventory.json");

    private PlayerInfo playerInfo;
    private PlayerProfile playerProfile;

    public PlayerProfile PlayerProfile => playerProfile;

    private async void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Ensure singleton
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes

        try
        {
            await UnityServices.InitializeAsync(); // Wait for initialization to complete
            PlayerAccountService.Instance.SignedIn += SignedIn;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to initialize Unity Services: {ex.Message}");
        }
    }

    private async void SignedIn()
    {
        try
        {
            var accessToken = PlayerAccountService.Instance.AccessToken;
            await SignInWithUnityAsync(accessToken);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public async Task InitSignIn()
    {
        await PlayerAccountService.Instance.StartSignInAsync();
    }

    private async Task SignInWithUnityAsync(string accessToken)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUnityAsync(accessToken);
            Debug.Log("SignIn is successful.");

            playerInfo = AuthenticationService.Instance.PlayerInfo;
            var name = await AuthenticationService.Instance.GetPlayerNameAsync();

            playerProfile = new PlayerProfile
            {
                playerInfo = playerInfo,
                Name = name
            };

            await LoadPlayerProfileFromCloud();

            OnSignedIn?.Invoke(playerProfile);
            SaveAuthData();

            SceneManager.LoadScene("Main Scene");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    private void SaveAuthData()
    {
        AuthData data = new AuthData
        {
            accessToken = PlayerAccountService.Instance.AccessToken,
            playerId = AuthenticationService.Instance.PlayerId,
            playerName = playerProfile.Name
        };
        AuthStorage.Save(data);
    }

    public async Task SavePlayerProfileToCloud()
    {
        var data = new Dictionary<string, object>
        {
            { "Name", playerProfile.Name },
            { "Gems", playerProfile.Gems },
            { "Feathers", playerProfile.Feathers },
            { "Level", playerProfile.Level },
            { "OwnedCharacters", JsonUtility.ToJson(new PlayerCharacterInventory { ownedCharacters = playerProfile.ownedCharacters }) },
        };

        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        Debug.Log("Player profile saved to cloud.");
    }

    private async Task LoadPlayerProfileFromCloud()
    {
        var keys = new HashSet<string> { "Name", "Gems", "Feathers", "Level", "OwnedCharacters", "Team" };
        var savedData = await CloudSaveService.Instance.Data.LoadAsync(keys);

        if (!savedData.ContainsKey("Level"))
        {
            await SetDefaultProfile();
            await SavePlayerProfileToCloud();
            return;
        }

        playerProfile.playerInfo = AuthenticationService.Instance.PlayerInfo;
        playerProfile.Name = savedData["Name"];
        playerProfile.Gems = int.Parse(savedData["Gems"]);
        playerProfile.Feathers = int.Parse(savedData["Feathers"]);
        playerProfile.Level = int.Parse(savedData["Level"]);

        var ownedJson = savedData["OwnedCharacters"];
        var ownedWrapper = JsonUtility.FromJson<PlayerCharacterInventory>(ownedJson);

        playerProfile.ownedCharacters = ownedWrapper?.ownedCharacters ?? new List<OwnedCharacter>();

        //save owned characters to json file playerInventoryPath
        string json = JsonUtility.ToJson(ownedWrapper);
        File.WriteAllText(playerInventoryPath, json);

        Debug.Log("Player profile loaded from cloud.");
        Debug.Log($"Owned Characters: {playerProfile.ownedCharacters.Count}");
    }

    private async Task SetDefaultProfile()
    {
        playerProfile = new PlayerProfile
        {
            playerInfo = AuthenticationService.Instance.PlayerInfo,
            Name = await AuthenticationService.Instance.GetPlayerNameAsync(),
            Gems = 10,
            Feathers = 0,
            Level = 1,
            ownedCharacters = new List<OwnedCharacter>(),
            team = new List<TeamMember>()
        };

        // characters default
        playerProfile.ownedCharacters.Add(new OwnedCharacter { characterID = 1, level = 1, isUnlocked = true });
    }

    public async void UpdatePlayerProfile(PlayerProfile updatedProfile)
    {
        playerProfile = updatedProfile;
        await SavePlayerProfileToCloud();
    }

    public void UpdatePlayerProfileWithoutSave(PlayerProfile updatedProfile)
    {
        playerProfile = updatedProfile;
    }

    private void OnDestroy()
    {
        PlayerAccountService.Instance.SignedIn -= SignedIn;
    }
}

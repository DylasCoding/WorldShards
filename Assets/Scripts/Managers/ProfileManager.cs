using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ProfileManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerNameText;
    [SerializeField] private TextMeshProUGUI _playerLevelText;
    [SerializeField] private TextMeshProUGUI _playerGemsText;
    [SerializeField] private TextMeshProUGUI _playerFeathersText;

    [Header("Player Profile Data")]
    [SerializeField] private TextMeshProUGUI _playerNameEditText;
    [SerializeField] private TextMeshProUGUI _playerLevelEditText;
    [SerializeField] private TextMeshProUGUI _heroesCountText;
    [SerializeField] private TextMeshProUGUI _playerIDText;

    [Header("Edit Name Panel")]
    [SerializeField] private GameObject _editNamePanel;
    [SerializeField] private TextMeshProUGUI _editNameInputField;

    private void Start()
    {
        // Initialize the UI with player information
        ShowUIPlayerInfo();
    }

    public void ShowUIPlayerInfo()
    {
        var profile = LoginController.Instance.PlayerProfile;

        if (!object.ReferenceEquals(profile, null))
        {
            //limit the name to 10 characters if it is longer than additional dots
            string nameText = profile.Name;
            string idText = profile.playerInfo.Id;

            if (profile.Name.Length > 12)
                nameText = profile.Name.Substring(0, 12) + "...";

            _playerNameText.text = nameText;
            _playerLevelText.text = profile.Level.ToString();

            if (_playerGemsText != null)
                _playerGemsText.text = profile.Gems.ToString();

            if (_playerFeathersText != null)
                _playerFeathersText.text = profile.Feathers.ToString();

            ShowEditPlayerInfo();
        }
        else
        {
            Debug.LogError("PlayerProfile is null!");
        }
    }

    public void ShowEditPlayerInfo()
    {
        var profile = LoginController.Instance.PlayerProfile;

        if (!object.ReferenceEquals(profile, null))
        {
            //limit the name to 10 characters if it is longer than additional dots
            string nameText = profile.Name;
            string idText = profile.playerInfo.Id;

            if (profile.Name.Length > 12)
                nameText = profile.Name.Substring(0, 12) + "...";

            if (profile.playerInfo.Id.Length > 12)
            {
                idText = profile.playerInfo.Id.Substring(0, 12) + "...";
            }

            _playerNameEditText.text = nameText;
            _playerLevelEditText.text = profile.Level.ToString();
            _playerIDText.text = idText;

            if (_heroesCountText != null)
                _heroesCountText.text = "123";
        }
        else
        {
            Debug.LogError("PlayerProfile is null!");
        }
    }

    public void ShowEditNamePanel()
    {
        if (_editNamePanel != null)
        {
            _editNamePanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Edit Name Panel is not assigned in the inspector!");
        }
    }

    public void HideEditNamePanel()
    {
        if (_editNamePanel != null)
        {
            _editNamePanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Edit Name Panel is not assigned in the inspector!");
        }
    }

    public void OnClickSaveNameButtonWrapper()
    {
        _ = OnClickSaveNameButton(); // run async don't need `await`
    }

    public async Task OnClickSaveNameButton()
    {
        string newName = _editNameInputField.text.Trim();

        if (string.IsNullOrEmpty(newName) || newName.Length > 12)
        {
            Debug.LogWarning("New name is empty or exceeds 12 characters!");
            return;
        }

        var profile = LoginController.Instance.PlayerProfile;
        profile.Name = newName;

        await DataSyncManager.SaveName(profile.Name);

        LoginController.Instance.UpdatePlayerProfileWithoutSave(profile);

        ShowUIPlayerInfo();
        HideEditNamePanel();
    }


}

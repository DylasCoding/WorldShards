using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILogin : MonoBehaviour
{
    [SerializeField] private Button loginButton;
    [SerializeField] private GameObject waitingPanel;

    [SerializeField] private TMP_Text userIdText;
    [SerializeField] private TMP_Text userNameText;

    // [SerializeField] private Transform loginPanel, userPanel;

    [SerializeField] private LoginController loginController;

    private PlayerProfile playerProfile;

    private void OnEnable()
    {
        waitingPanel.SetActive(false);
        loginButton.onClick.AddListener(() => AudioManager.Instance.PlayClickSound());
        loginButton.onClick.AddListener(LoginButtonPressed);

        loginController.OnSignedIn += LoginController_OnSignedIn;
        // loginController.OnAvatarUpdate += LoginController_OnAvatarUpdate;
    }

    private void OnDisable()
    {
        loginButton.onClick.RemoveListener(LoginButtonPressed);
        loginController.OnSignedIn -= LoginController_OnSignedIn;
        // loginController.OnAvatarUpdate -= LoginController_OnAvatarUpdate;
    }

    private async void LoginButtonPressed()
    {
        await loginController.InitSignIn();
        showWaitingPanel(true);
    }

    private void showWaitingPanel(bool show)
    {
        waitingPanel.SetActive(show);
    }

    private void LoginController_OnSignedIn(PlayerProfile profile)
    {
        playerProfile = profile;
        // loginPanel.gameObject.SetActive(false);
        // userPanel.gameObject.SetActive(true);

        userIdText.text = $"id_{playerProfile.playerInfo.Id}";
        userNameText.text = profile.Name;

        Debug.Log($"Player ID: {playerProfile.playerInfo.Id}");
        Debug.Log($"Player Name: {profile.Name}");
    }
    private void LoginController_OnAvatarUpdate(PlayerProfile profile)
    {
        playerProfile = profile;
    }


}
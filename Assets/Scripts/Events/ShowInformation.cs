using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Threading.Tasks;
using UnityEngine.UI;

[System.Serializable]
public class ShowInformation : MonoBehaviour
{
    [SerializeField] private PlayerInventoryManager _playerInventoryManager;

    [SerializeField] private TextMeshProUGUI _charName;
    [SerializeField] private TextMeshProUGUI _charLevel;
    [SerializeField] private TextMeshProUGUI _charClass;
    [SerializeField] private TextMeshProUGUI _charHealth;
    [SerializeField] private TextMeshProUGUI _charAttack;
    [SerializeField] private TextMeshProUGUI _charDefense;
    [SerializeField] private TextMeshProUGUI _upgradeGemCost;
    [SerializeField] private TextMeshProUGUI _upgradeFeatherCost;

    [Header("Skill icon")]
    [SerializeField] private Image _skill1Icon;
    [SerializeField] private Image _skill2Icon;
    [SerializeField] private Image _skill3Icon;

    [Header("UI Reload")]
    [SerializeField] private TextMeshProUGUI _gemText;
    [SerializeField] private TextMeshProUGUI _featherText;

    [Header("Notification")]
    [SerializeField] private NotificationManager _notificationManager;

    [Header("Upgrade data")]
    private UpgradeTree _upgradeTree;
    private PlayerCharacterEntry _playerCharacterEntry;
    private bool _isUpgrading = false;

    [Header("Show")]
    [SerializeField] private ShowSkillInfo _showSkillInfoPanel;
    [SerializeField] private GameObject _listCharacterPanel;
    private bool _isShowSkillInfoPanel = false;

    private void Start()
    {
        _upgradeTree = new UpgradeTree();
        _listCharacterPanel.SetActive(true);
        _showSkillInfoPanel.gameObject.SetActive(false);
    }

    public void UpdateCharacterInfo(PlayerCharacterEntry playerCharacterEntry)
    {
        _playerCharacterEntry = playerCharacterEntry;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_playerCharacterEntry == null || _playerCharacterEntry.characterData == null)
        {
            Debug.LogError("PlayerCharacterEntry or CharacterData is null!");
            return;
        }
        int health = _playerCharacterEntry.characterData.maxHealth + _playerCharacterEntry.characterData.incrementalHealth * _playerCharacterEntry.level;
        int attack = _playerCharacterEntry.characterData.attackDamage + _playerCharacterEntry.characterData.incrementalAttack * _playerCharacterEntry.level;
        int defense = _playerCharacterEntry.characterData.defense + _playerCharacterEntry.characterData.incrementalDefense * _playerCharacterEntry.level;
        int upgradeGemCost = _upgradeTree.GetGemCost(_playerCharacterEntry.level);
        int upgradeFeatherCost = _upgradeTree.GetFeatherCost(_playerCharacterEntry.level);

        _charName.text = _playerCharacterEntry.characterData.characterName;

        _charLevel.text = "Level: " + _playerCharacterEntry.level.ToString();

        _charHealth.text = health.ToString();
        _charAttack.text = attack.ToString();
        _charDefense.text = defense.ToString();

        _skill1Icon.sprite = _playerCharacterEntry.characterData.basicAttack.skillImage;
        _skill2Icon.sprite = _playerCharacterEntry.characterData.specialSkill1.skillImage;
        _skill3Icon.sprite = _playerCharacterEntry.characterData.specialSkill2.skillImage;

        _upgradeGemCost.text = upgradeGemCost.ToString();
        _upgradeFeatherCost.text = upgradeFeatherCost.ToString();
    }

    public void OnClickShowSkillInfo()
    {
        if (_isShowSkillInfoPanel)
        {
            _listCharacterPanel.SetActive(true);
            _showSkillInfoPanel.gameObject.SetActive(false);
            _isShowSkillInfoPanel = false;
            return;
        }
        _listCharacterPanel.SetActive(false);
        _showSkillInfoPanel.gameObject.SetActive(true);
        _showSkillInfoPanel.ShowSkillInfoPanel(_playerCharacterEntry.characterData);
        _isShowSkillInfoPanel = true;
    }

    public void OnClickUpgradeButton()
    {
        if (_isUpgrading)
        {
            Debug.LogWarning("Upgrade is already in progress!");
            return;
        }

        _ = UpgradeCharacter();
    }

    public async Task UpgradeCharacter()
    {
        _isUpgrading = true;

        try
        {
            await _upgradeTree.UpgradeCharacter(_playerCharacterEntry);
            UpdateUI();

            if (_playerInventoryManager != null)
            {
                // _playerInventoryManager.UpdateOwnedCharacters(LoginController.Instance.PlayerProfile.ownedCharacters);
                _playerInventoryManager.SaveToJson();
                _notificationManager.ShowNotification(_playerCharacterEntry.characterData.characterName + " Level up!");
                UpdateBalanceUI(LoginController.Instance.PlayerProfile);

            }
            else
            {
                Debug.LogError("PlayerInventoryManager is null!");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred during character upgrade: {ex.Message}");
        }
        finally
        {
            _isUpgrading = false;
        }
    }

    private void UpdateBalanceUI(PlayerProfile profile)
    {
        _gemText.text = profile.Gems.ToString();
        _featherText.text = profile.Feathers.ToString();
    }
}

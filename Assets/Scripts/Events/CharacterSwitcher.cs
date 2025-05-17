using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using TMPro;

public class CharacterSwitcher : MonoBehaviour
{
    [SerializeField] private TeamManager _teamManager;
    [SerializeField] private CharController _playerController;
    [SerializeField] private UIController _uiController;

    [Header("UI")]
    [SerializeField] private Button[] _buttons;
    [SerializeField] private Image[] _characterImages;
    [SerializeField] private Image[] _classIcons;
    [SerializeField] private CharacterClassIcons _characterClassIcons;
    [SerializeField] private ElementIcons _elementIcons;
    private int _currentCharacterIndex = 0;
    private int firstButtonIndex = 1; // 0 is first character

    [Header("Character")]
    private Image _CharacterImage;
    private Animator _characterAnimator;
    [SerializeField] private Image _characterUltimateImage;
    [SerializeField] private Transform _characterPosition;
    [SerializeField] private Image _classIconImage;
    [SerializeField] private Image _elementIconImage;
    [SerializeField] private TextMeshProUGUI _damageStatsText;
    [SerializeField] private TextMeshProUGUI _defenseStatsText;

    private void Start()
    {
        for (int i = firstButtonIndex; i < _buttons.Length; i++)
        {
            var characterData = _teamManager.GetCharacterData(i);

            if (characterData == null || characterData.characterImage == null)
            {
                _buttons[i].gameObject.SetActive(false);
                if (_characterImages[i] != null)
                    _characterImages[i].gameObject.SetActive(false);
                if (_classIcons[i] != null)
                    _classIcons[i].gameObject.SetActive(false);
                continue;
            }
            SetImageUI(i, characterData);
        }

        //reset effects
        _teamManager.DestroyAllBuffs();

        // Hiển thị nhân vật đầu tiên (index 0)
        UpdateCharacter();
    }

    public TeamManager GetTeamManager()
    {
        return _teamManager;
    }

    private void SetImageUI(int i, CharacterData characterData)
    {
        if (_characterImages[i] != null)
            _characterImages[i].sprite = characterData.characterImage;

        if (_classIcons[i] != null)
            _classIcons[i].sprite = _characterClassIcons.GetIcon(characterData.characterType);

        if (_teamManager.GetCurrentHealth(i) <= 0)
        {
            _characterImages[i].color = Color.red;
        }
        else
        {
            _characterImages[i].color = Color.white;
        }

        if (_buttons[i] != null)
            _buttons[i].onClick.RemoveAllListeners(); // Xóa listener cũ

        int index = i;
        _buttons[i].onClick.AddListener(() => SwitchCharacter(index));
    }

    private void SwitchCharacter(int index)
    {
        int previousIndex = _currentCharacterIndex;
        _teamManager.UpdateCurrentHealth(_currentCharacterIndex, _playerController.GetCurrentHealth());

        int nextHealth = _teamManager.GetCurrentHealth(index);
        if (nextHealth <= 0)
        {
            Debug.Log("Character is dead, can't switch to this character");
            return;
        }
        //hide all effects of previous character
        _teamManager.CancelBuff(0);
        _teamManager.SwapCharacters(previousIndex, index);

        UpdateHeathBar();
        UpdateCharacter();

        UpdateAfterSwitch();
    }

    public void AutoSwapCharacter()
    {
        // Tìm chỉ số nhân vật đầu tiên còn sống
        for (int i = 0; i < _buttons.Length; i++)
        {
            if (_teamManager.GetCurrentHealth(i) > 0 && i != _currentCharacterIndex)
            {
                SwitchCharacter(i);
                return;
            }
        }
    }

    //check if all characters are dead
    public bool CheckAllCharactersDead()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            if (_teamManager.GetCurrentHealth(i) > 0)
            {
                Debug.Log($"Character {i} is alive, HP: {_teamManager.GetCurrentHealth(i)}");
                return false;
            }
        }
        return true;
    }

    private void UpdateAfterSwitch()
    {
        // Cập nhật lại hình ảnh nút UI sau khi swap
        for (int i = firstButtonIndex; i < _buttons.Length; i++)
        {
            var characterData = _teamManager.GetCharacterData(i);
            if (characterData != null)
            {
                SetImageUI(i, characterData);
            }
        }

    }

    private void UpdateHeathBar()
    {
        _playerController.HealthUIUpdate(_teamManager.GetCurrentHealth(_currentCharacterIndex), _teamManager.GetMaxHealth(_currentCharacterIndex));
    }

    private void UpdateCharacterImage()
    {
        //update character image on UI button
        if (_CharacterImage != null)
        {
            _CharacterImage.sprite = _teamManager.GetCharacterData(_currentCharacterIndex).characterImage;
        }

    }

    void UpdateCharacter()
    {
        //character away is index 0
        SetOriginalPosition();
        var state = _teamManager.GetCharacterData(_currentCharacterIndex);
        int currentHealth = _teamManager.GetCurrentHealth(_currentCharacterIndex);
        int level = _teamManager.GetLevel(_currentCharacterIndex);

        _teamManager.GetAllBuffs(0, _characterPosition);

        if (state == null) return;

        if (_CharacterImage != null)
        {
            _CharacterImage.sprite = state.characterImage;
        }

        if (_characterUltimateImage != null)
        {
            _characterUltimateImage.sprite = state.CharacterUltimateImage;
        }

        if (_classIconImage != null)
        {
            _classIconImage.sprite = _characterClassIcons.GetIcon(state.characterType);
        }

        if (_elementIconImage != null)
        {
            _elementIconImage.sprite = _elementIcons.GetIcon(state.elementType);
        }

        if (_damageStatsText != null)
        {
            _damageStatsText.text = (state.attackDamage + level * state.incrementalAttack).ToString();
        }

        if (_defenseStatsText != null)
        {
            _defenseStatsText.text = (state.defense + level * state.incrementalDefense).ToString();
        }

        if (_characterAnimator != null)
        {
            _characterAnimator.runtimeAnimatorController = state.animatorController;
        }
        Debug.Log($"Switch character: {state.characterName}, Health: {currentHealth}, Level: {level}");

        _playerController.UpdateCharacterData(state, currentHealth, level);
        _uiController.ChangeSkillImage();
    }

    private void SetOriginalPosition()
    {
        _playerController.transform.position = _characterPosition.position;
    }
}

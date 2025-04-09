using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterSwitcher : MonoBehaviour
{
    [SerializeField] private TeamManager _teamManager;
    [SerializeField] private CharController _playerController;
    [SerializeField] private UIController _uiController;

    [Header("UI")]
    [SerializeField] private Button[] _buttons;
    [SerializeField] private Image[] _characterImages;
    private int _currentCharacterIndex = 0;
    private int firstButtonIndex = 1; // 0 is first character

    [Header("Character")]
    private Image _CharacterImage;
    private Animator _characterAnimator;
    [SerializeField] private Transform _characterPosition;

    void Start()
    {
        for (int i = firstButtonIndex; i < _buttons.Length; i++)
        {
            var characterData = _teamManager.GetCharacterData(i);

            if (characterData == null || characterData.characterImage == null)
            {
                _buttons[i].gameObject.SetActive(false);
                if (_characterImages[i] != null)
                    _characterImages[i].gameObject.SetActive(false);
                continue;
            }
            SetImageUI(i, characterData);
        }

        // Hiển thị nhân vật đầu tiên (index 0)
        UpdateCharacter();
    }

    private void SetImageUI(int i, CharacterData characterData)
    {
        if (_characterImages[i] != null)
            _characterImages[i].sprite = characterData.characterImage;

        if (_buttons[i] != null)
            _buttons[i].onClick.RemoveAllListeners(); // Xóa listener cũ

        int index = i;
        _buttons[i].onClick.AddListener(() => SwitchCharacter(index));
    }

    void SwitchCharacter(int index)
    {
        int previousIndex = _currentCharacterIndex;
        Debug.Log("Button " + index + " clicked." + " previous index: " + previousIndex);
        _teamManager.UpdateCurrentHealth(_currentCharacterIndex, _playerController.GetCurrentHealth());
        _teamManager.SwapCharacters(previousIndex, index);


        UpdateHeathBar();
        UpdateCharacter();

        UpdateAfterSwitch();
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

        if (state == null) return;

        if (_CharacterImage != null)
        {
            _CharacterImage.sprite = state.characterImage;
        }

        if (_characterAnimator != null)
        {
            _characterAnimator.runtimeAnimatorController = state.animatorController;
        }

        _playerController.UpdateCharacterData(state, currentHealth);
        _uiController.ChangeSkillImage();
    }

    private void SetOriginalPosition()
    {
        _playerController.transform.position = _characterPosition.position;
    }
}

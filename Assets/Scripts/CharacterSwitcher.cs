using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterSwitcher : MonoBehaviour
{
    [SerializeField] private TeamManager teamManager;
    [SerializeField] private Button[] buttons;
    [SerializeField] private CharController playerController;

    private int currentCharacterIndex = 0;

    [SerializeField] private Image CharacterImage;
    [SerializeField] private Animator characterAnimator;
    private int lengthButton = 3;
    void Start()
    {
        if (buttons.Length != lengthButton || teamManager == null || playerController == null)
        {
            Debug.LogError("Please assign all buttons and team manager in the inspector.");
            return;
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => SwitchCharacter(index));
        }
    }

    void SwitchCharacter(int index)
    {
        teamManager.UpdateCurrentHealth(currentCharacterIndex, playerController.GetCurrentHealth());
        currentCharacterIndex = index;
        UpdateCharacter();
    }

    void UpdateCharacter()
    {
        var state = teamManager.GetCharacterData(currentCharacterIndex);
        int currentHealth = teamManager.GetCurrentHealth(currentCharacterIndex);
        if (state == null) return;

        if (CharacterImage != null)
        {
            CharacterImage.sprite = state.characterImage;
        }

        if (characterAnimator != null)
        {
            characterAnimator.runtimeAnimatorController = state.animatorController;
        }

        playerController.UpdateCharacterData(state, currentHealth);
    }
}

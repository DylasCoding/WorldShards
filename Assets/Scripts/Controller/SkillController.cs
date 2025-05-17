using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    CharController charController;
    BattleManager battleManager;

    [SerializeField] private ElementIcons elementIcons;

    [Header("UI")]
    [SerializeField] private Image[] skillImages;
    [SerializeField] private Image[] elementImages;
    void Awake()
    {
        charController = FindObjectOfType<CharController>();
        if (charController == null)
        {
            Debug.LogError("can't find CharController");
        }
        battleManager = FindObjectOfType<BattleManager>();
        if (battleManager == null)
        {
            Debug.LogError("can't find BattleManager");
        }
    }

    public void Skill1()
    {
        Debug.Log("Skill 1");
        battleManager.PlayerAttack(1);
    }
    public void Skill2()
    {
        Debug.Log("Skill 2");
        battleManager.PlayerAttack(2);
    }
    public void Skill3()
    {
        Debug.Log("Skill 3");
        battleManager.PlayerAttack(3);
    }

    public void ChangeSkillImage()
    {
        if (charController.role == CharController.CharacterRole.Player)
        {
            CharacterData characterData = charController.characterData;
            if (characterData.basicAttack != null)
            {
                if (skillImages.Length > 0)
                    skillImages[0].sprite = characterData.basicAttack.skillImage;
                if (elementImages.Length > 0)
                    elementImages[0].sprite = elementIcons.GetIcon(characterData.basicAttack.elementType);
            }

            if (characterData.specialSkill1 != null)
            {
                if (skillImages.Length > 1)
                    skillImages[1].sprite = characterData.specialSkill1.skillImage;
                if (elementImages.Length > 1)
                    elementImages[1].sprite = elementIcons.GetIcon(characterData.specialSkill1.elementType);
            }

            if (characterData.specialSkill2 != null)
            {
                if (skillImages.Length > 2)
                    skillImages[2].sprite = characterData.specialSkill2.skillImage;
                if (elementImages.Length > 2)
                    elementImages[2].sprite = elementIcons.GetIcon(characterData.specialSkill2.elementType);
            }

        }

    }
}

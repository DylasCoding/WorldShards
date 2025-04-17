using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    CharController charController;
    BattleManager battleManager;

    [Header("UI")]
    [SerializeField] private UnityEngine.UI.Image[] skillImages;
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
                skillImages[0].sprite = characterData.basicAttack.skillImage;

            if (characterData.specialSkill1 != null)
                skillImages[1].sprite = characterData.specialSkill1.skillImage;

            if (characterData.specialSkill2 != null)
                skillImages[2].sprite = characterData.specialSkill2.skillImage;
        }

    }
}

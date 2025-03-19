using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    CharController charController;
    BattleManager battleManager;
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

    public void ChangeCharacter(int index)
    {
        // charController.characterData = charController.characterData.characterList[index];
        // charController.animator.runtimeAnimatorController = charController.characterData.animatorController;
    }
}

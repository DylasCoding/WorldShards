using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public CharController player;
    public CharController enemy;
    private bool isPlayerTurn = true;
    private bool isActionDone = false;

    [Header("Team Manager")]
    [SerializeField] private CharacterSwitcher playerSwitcher;
    [SerializeField] private CharacterSwitcher enemySwitcher;
    private TeamManager playerTeam;
    private TeamManager enemyTeam;

    [Header("Audio")]
    [SerializeField] private AudioManager audioManager;

    [Header("Battle events")]
    [SerializeField] private ActionPanelManager _panelManager;


    private List<Buff> activeBuffs = new List<Buff>(); // List to store active buffs

    private void Start()
    {
        playerTeam = playerSwitcher.GetTeamManager();
        enemyTeam = enemySwitcher.GetTeamManager();
        // audioManager.PlayBattleMusic();
    }

    public void PlayerAttack(int skillIndex)
    {
        if (!isPlayerTurn || isActionDone) { return; }

        SkillData skillData = player.GetSkill(skillIndex);

        BattleUpdateBuffs();
        player.useSkill(skillIndex, enemy, _panelManager, audioManager);
        isActionDone = true; // Đánh dấu hành động đã hoàn thành

        if (skillData.isBuffSkill)
        {
            BattleApplyBuff(0, skillData.buffTurns, skillData, player.transform);
            Debug.Log("Buff applied to player ");
        }
    }

    private void EndPlayerTurn()
    {
        Debug.Log("End Player Turn");

        isPlayerTurn = false;
        isActionDone = false; // Reset trạng thái hành động

        if (enemy.GetCurrentHealth() <= 0)
        {
            CheckTeam();
            // return;
        }

        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        try
        {
            Debug.Log("Enemy Turn");

            BattleUpdateBuffs();

            int randomSkill = Random.Range(1, 3);
            SkillData skill = enemy.GetSkill(randomSkill);
            enemy.useSkill(randomSkill, player, _panelManager, audioManager);

            // Chờ class khác gọi EndTurn để kết thúc lượt của enemy
            isActionDone = true;

            if (skill.isBuffSkill)
            {
                BattleApplyBuff(0, skill.buffTurns, skill, enemy.transform);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error during enemy turn: " + ex.Message);
        }
    }

    private void EndEnemyTurn()
    {
        Debug.Log("End Enemy Turn");

        isPlayerTurn = true;
        isActionDone = false;

        if (player.GetCurrentHealth() <= 0)
        {
            CheckTeam();
            return;
        }
    }

    public void EndTurn()
    {
        if (isPlayerTurn && isActionDone)
        {
            Debug.Log("End Player Turn called from outside");
            EndPlayerTurn();
        }
        else if (!isPlayerTurn && isActionDone)
        {
            Debug.Log("End Enemy Turn called from outside");
            EndEnemyTurn();
        }
        else
        {
            Debug.LogWarning("EndTurn called but no action has been performed or it's not the correct turn.");
        }
    }

    public void BattleApplyBuff(int index, int turns, SkillData skillData, Transform targetTransform)
    {
        if (isPlayerTurn)
        {
            playerTeam.ApplyBuff(index, turns, skillData, targetTransform);
            // playerTeam.SaveTeamToJson();
        }
        else
        {
            enemyTeam.ApplyBuff(index, turns, skillData, targetTransform);
        }
    }

    public void BattleUpdateBuffs()
    {
        if (isPlayerTurn)
        {
            playerTeam.UpdateBuffs(0);
        }
        else
        {
            enemyTeam.UpdateBuffs(0);
        }
    }

    //auto swap character when dead
    public void AutoSwapCharacter()
    {
        if (isPlayerTurn)
        {
            playerSwitcher.AutoSwapCharacter();
        }
        else
        {
            Debug.Log("Auto swap enemy character");
            enemySwitcher.AutoSwapCharacter();
        }
    }
    private void CheckTeam()
    {
        Debug.Log("Check Team");
        AutoSwapCharacter();
    }

    private void EndGame()
    {
        Debug.Log("Game Over");
        // Show game over UI or perform any other actions needed
    }
}

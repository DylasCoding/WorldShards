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

    private List<Buff> activeBuffs = new List<Buff>(); // List to store active buffs

    private void Start()
    {
        playerTeam = playerSwitcher.GetTeamManager();
        enemyTeam = enemySwitcher.GetTeamManager();
    }

    public void PlayerAttack(int skillIndex)
    {
        if (!isPlayerTurn || isActionDone) { return; }

        SkillData skillData = player.GetSkill(skillIndex);

        BattleUpdateBuffs();
        player.useSkill(skillIndex, enemy);
        isActionDone = true; // Đánh dấu hành động đã hoàn thành

        if (skillData.isBuffSkill)
        {
            BattleApplyBuff(0, skillData.buffTurns, skillData, player.transform);
            EndPlayerTurn();
        }
    }

    private void EndPlayerTurn()
    {
        Debug.Log("End Player Turn");

        if (enemy.GetCurrentHealth() <= 0)
        {
            EndGame();
            return;
        }

        isPlayerTurn = false;
        isActionDone = false; // Reset trạng thái hành động
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy Turn");
        yield return new WaitForSeconds(1f);

        BattleUpdateBuffs();

        int randomSkill = Random.Range(1, 3);
        SkillData skill = enemy.GetSkill(randomSkill);
        enemy.useSkill(randomSkill, player);

        // Chờ class khác gọi EndTurn để kết thúc lượt của enemy
        isActionDone = true;

        if (skill.isBuffSkill)
        {
            BattleApplyBuff(0, skill.buffTurns, skill, enemy.transform);
            EndEnemyTurn();
        }
    }

    private void EndEnemyTurn()
    {
        Debug.Log("End Enemy Turn");

        if (player.GetCurrentHealth() <= 0)
        {
            EndGame();
            return;
        }

        isPlayerTurn = true;
        isActionDone = false; // Reset trạng thái hành động
    }

    public void EndTurn()
    {
        if (isPlayerTurn && isActionDone)
        {
            EndPlayerTurn();
        }
        else if (!isPlayerTurn && isActionDone)
        {
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
    private void EndGame()
    {
        if (player.GetCurrentHealth() <= 0)
        {
            Debug.Log("Player is dead");
        }
        if (enemy.GetCurrentHealth() <= 0)
        {
            Debug.Log("Enemy is dead");
        }
    }
}

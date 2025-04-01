using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public CharController player;
    public CharController enemy;
    private bool isPlayerTurn = true;
    private bool isActionDone = false;

    void Start()
    {
        //print name of the player and enemy
        Debug.Log("Player: " + player.characterData.characterName);
        Debug.Log("Enemy: " + enemy.characterData.characterName);
    }

    public void PlayerAttack(int skillIndex)
    {
        if (!isPlayerTurn || isActionDone) { return; }
        if (isPlayerTurn && !isActionDone)
        {
            player.useSkill(skillIndex, enemy);
            StartCoroutine(EndPlayerTurn());
        }
    }

    public IEnumerator EndPlayerTurn()
    {
        yield return new WaitUntil(() => !player.isAttacking);

        if (enemy.GetCurrentHealth() <= 0)
        {
            EndGame();
            yield break;
        }

        isPlayerTurn = false;
        StartCoroutine(EnemyTurn());
    }

    public IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        int randomSkill = Random.Range(1, 3);
        enemy.useSkill(randomSkill, player);

        yield return new WaitUntil(() => !enemy.isAttacking);

        isPlayerTurn = true;
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

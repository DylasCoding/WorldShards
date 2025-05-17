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

    [Header("Battle events")]
    [SerializeField] private ActionPanelManager _panelManager;
    [SerializeField] private SummaryPanel _summaryPanel;


    private List<Buff> activeBuffs = new List<Buff>(); // List to store active buffs

    private void Start()
    {
        playerTeam = playerSwitcher.GetTeamManager();
        enemyTeam = enemySwitcher.GetTeamManager();
        AudioManager.Instance.PlayBattleMusic();
    }

    public void PlayerAttack(int skillIndex)
    {
        if (!isPlayerTurn || isActionDone) { return; }

        SkillData skillData = player.GetSkill(skillIndex);
        BattleUpdateBuffs();

        int level = playerTeam.GetLevel(0);
        player.useSkill(skillIndex, level, enemy, _panelManager);
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
        isActionDone = false;

        enemyTeam.UpdateCurrentHealth(0, enemy.GetCurrentHealth());

        if (enemy.GetCurrentHealth() <= 0)
        {
            CheckTeam();
            if (enemySwitcher.CheckAllCharactersDead())
            {
                return;
            }
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

            int randomSkill = UnityEngine.Random.Range(1, 3);

            SkillData skill = enemy.GetSkill(randomSkill);

            int level = enemyTeam.GetLevel(0);
            enemy.useSkill(randomSkill, level, player, _panelManager);

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
        playerTeam.UpdateCurrentHealth(0, player.GetCurrentHealth());

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
            bool allDead = playerSwitcher.CheckAllCharactersDead();
            if (allDead)
            {
                EndGame(2);
                return;
            }
            else
                playerSwitcher.AutoSwapCharacter();
        }
        else
        {
            bool allDead = enemySwitcher.CheckAllCharactersDead();
            if (allDead)
            {
                EndGame(1);
                return;
            }
            else
                enemySwitcher.AutoSwapCharacter();
        }
    }
    private void CheckTeam()
    {
        Debug.Log("Check Team");
        AutoSwapCharacter();
    }

    private void EndGame(int index)
    {
        int stage = PlayerPrefs.GetInt("Stage", 1);
        Debug.Log("In Stage: " + stage);
        int level = LoginController.Instance.PlayerProfile.Level;
        RewardSystem rewardSystem = new RewardSystem(); // Khởi tạo RewardSystem
        Reward reward;

        switch (index)
        {
            case 1: // Player Win
                Debug.Log("Player Win");

                reward = rewardSystem.RewardCalculation(stage, level, true);
                _summaryPanel.ShowWinPanel("You Win", reward.level, reward.gem, reward.feather);

                UpdateAndSaveReward(reward);
                break;
            case 2: // Enemy Win
                Debug.Log("Enemy Win");

                reward = rewardSystem.RewardCalculation(stage, level, false);
                _summaryPanel.ShowLosePanel("You Lose", reward.level, reward.gem, reward.feather);

                UpdateAndSaveReward(reward);
                break;
            default:
                Debug.LogError("Invalid index for EndGame");
                break;
        }
    }

    private async void UpdateAndSaveReward(Reward reward)
    {
        // Cập nhật profile cục bộ
        var profile = LoginController.Instance.PlayerProfile;
        profile.Gems = reward.gem;
        profile.Feathers = reward.feather;
        profile.Level = reward.level;

        LoginController.Instance.UpdatePlayerProfileWithoutSave(profile);

        // Lưu dữ liệu lên cloud
        await DataSyncManager.SaveReward(reward.gem, reward.feather, reward.level);
    }
}

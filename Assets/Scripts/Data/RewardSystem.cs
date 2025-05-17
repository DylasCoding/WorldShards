using System.Collections;
using UnityEngine;

[System.Serializable]
public class RewardSystem
{
    private int gemReward = 123;
    private int featherReward = 20;
    private float winIncreaseStats = 1.5f;
    private float loseIncreaseStats = 0.1f;
    private int levelIncreaseReward = 1;

    public Reward RewardCalculation(int stage, int level, bool isWin)
    {
        if (isWin)
        {
            return WinRewards(stage, level);
        }
        else
        {
            return LoseRewards(stage, level);
        }
    }

    private Reward WinRewards(int stage, int level)
    {
        int gem = LoginController.Instance.PlayerProfile.Gems + (int)(gemReward * (stage * winIncreaseStats));
        int feather = LoginController.Instance.PlayerProfile.Feathers + (int)(featherReward * (stage * winIncreaseStats));

        Debug.Log("Stage: " + stage);

        if (level <= stage)
        {
            level += levelIncreaseReward;
        }

        Debug.Log("Gem Reward: " + gem);
        Debug.Log("Feather Reward: " + feather);
        Debug.Log("Level Increase Reward: " + level);

        return new Reward
        {
            gem = gem,
            feather = feather,
            level = level
        };
    }

    private Reward LoseRewards(int stage, int level)
    {
        int gem = LoginController.Instance.PlayerProfile.Gems + (int)(gemReward * (stage + loseIncreaseStats));
        int feather = LoginController.Instance.PlayerProfile.Feathers + (int)(featherReward * (stage + loseIncreaseStats));

        Debug.Log("Gem Reward: " + gem);
        Debug.Log("Feather Reward: " + feather);
        Debug.Log("Level Increase Reward: " + level);

        return new Reward
        {
            gem = gem,
            feather = feather,
            level = level
        };
    }
}

[System.Serializable]
public class Reward
{
    public int gem;
    public int feather;
    public int level;
}
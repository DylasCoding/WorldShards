using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

[System.Serializable]
public class UpgradeTree
{
    public int level = 1;
    public int maxLevel = 10;
    public int upgradeGemCost = 100;
    public int upgradeFeatherCost = 10;

    public int GetGemCost(int level)
    {
        if (level < 1 || level > maxLevel)
        {
            Debug.LogError("Level out of range!");
            return 0;
        }

        int cost = upgradeGemCost + upgradeGemCost * level;
        return cost;
    }

    public int GetFeatherCost(int level)
    {
        if (level < 1 || level > maxLevel)
        {
            Debug.LogError("Level out of range!");
            return 0;
        }

        int cost = upgradeFeatherCost + upgradeFeatherCost * level;
        return cost;
    }

    public async Task UpgradeCharacter(PlayerCharacterEntry playerCharacterEntry)
    {
        if (playerCharacterEntry.level < maxLevel)
        {
            level = playerCharacterEntry.level;
            var profile = LoginController.Instance.PlayerProfile;
            int gem = profile.Gems;
            int feather = profile.Feathers;

            if (!CanUpgrade(gem, feather, level))
                return;

            playerCharacterEntry.level++;
            profile.Gems -= GetGemCost(level);
            profile.Feathers -= GetFeatherCost(level);

            Debug.Log("Up 1");

            int charId = playerCharacterEntry.characterData.characterID;
            int ownedCharIndex = profile.ownedCharacters.FindIndex(c => c.characterID == charId);
            if (ownedCharIndex != -1)
            {
                var ownedChar = profile.ownedCharacters[ownedCharIndex];
                ownedChar.level = playerCharacterEntry.level;
                profile.ownedCharacters[ownedCharIndex] = ownedChar;
            }
            else
            {
                Debug.LogError($"OwnedCharacter with characterID {charId} not found!");
            }

            Debug.Log("Up 2");


            await DataSyncManager.SaveGems(profile.Gems);
            await DataSyncManager.SaveFeathers(profile.Feathers);

            LoginController.Instance.UpdatePlayerProfileWithoutSave(profile);

            Debug.Log("Upgraded to level: " + playerCharacterEntry.level);
        }
        else
        {
            Debug.Log("Max level reached!");
        }
    }

    public bool CanUpgrade(int gem, int feather, int level)
    {
        int upgradeGemCost = GetGemCost(level);
        int upgradeFeatherCost = GetFeatherCost(level);

        if (gem >= upgradeGemCost && feather >= upgradeFeatherCost)
            return true;
        else
        {
            Debug.Log("Not enough resources to upgrade!");
            return false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [Header("Chapter 1")]
    [SerializeField] private Button stage1Button;
    [SerializeField] private Sprite stage1Image;

    [SerializeField] private Button stage2Button;
    [SerializeField] private Sprite stage2Image;

    [SerializeField] private Button stage3Button;
    [SerializeField] private Sprite stage3Image;

    [SerializeField] private Button bossStageButton;
    [SerializeField] private Sprite bossStageImage;

    [Header("Notification")]
    [SerializeField] private NotificationManager _notificationPanel;

    [Header("Scene fade")]
    [SerializeField] private SceneDirection _sceneDirection;

    [Header("Test value")]
    [SerializeField] private int level = 1; // Test value, replace with actual player level


    private void Start()
    {
        ShowCompletedStage();
    }

    public void OnStageButtonClicked(int stageIndex)
    {
        switch (stageIndex)
        {
            case 1:
                if (!CheckCondition(stageIndex))
                    break;
                OpenLineUPScene(stageIndex, _sceneDirection);
                break;
            case 2:
                if (!CheckCondition(stageIndex))
                    break;
                OpenLineUPScene(stageIndex, _sceneDirection);
                break;
            case 3:
                if (!CheckCondition(stageIndex))
                    break;
                OpenLineUPScene(stageIndex, _sceneDirection);
                break;
            case 4:
                if (!CheckCondition(stageIndex))
                    break;
                OpenLineUPScene(stageIndex, _sceneDirection);
                break;
            default:
                Debug.LogWarning("Invalid stage index: " + stageIndex);
                break;
        }
    }

    private static void OpenLineUPScene(int stageIndex, SceneDirection _sceneDirection)
    {
        PlayerPrefs.SetInt("Stage", stageIndex);
        Debug.Log("Stage index: " + stageIndex);
        _sceneDirection.GoToLineUpScene();
    }

    private void ShowCompletedStage()
    {
        // var profile = LoginController.Instance.PlayerProfile;
        // int level = profile.Level;

        if (level >= 1)
        {
            stage1Button.image.sprite = stage1Image;

            if (level >= 2)
            {
                stage2Button.image.sprite = stage2Image;

                if (level >= 3)
                {
                    stage3Button.image.sprite = stage3Image;

                    if (level >= 4)
                    {
                        bossStageButton.image.sprite = bossStageImage;
                    }
                }
            }
        }
    }

    private bool CheckCondition(int stageIndex)
    {
        // var profile = LoginController.Instance.PlayerProfile;
        // int level = profile.Level;

        if (level >= stageIndex)
        {
            return true;
        }
        else
        {
            _notificationPanel.ShowNotification("You need to complete the previous stage first.");
            return false;
        }
    }
}

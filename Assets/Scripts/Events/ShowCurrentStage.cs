using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowCurrentStage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _stageText;

    private int _stageIndex;

    void Start()
    {
        _stageIndex = PlayerPrefs.GetInt("Stage", 1);
        ShowStage();

    }

    private void ShowStage()
    {
        switch (_stageIndex)
        {
            case 1:
                _stageText.text = "Stage 1";
                break;
            case 2:
                _stageText.text = "Stage 2";
                break;
            case 3:
                _stageText.text = "Stage 3";
                break;
            case 4:
                _stageText.text = "Boss Stage";
                break;
            default:
                Debug.LogWarning("Invalid stage index: " + _stageIndex);
                break;
        }
    }
}

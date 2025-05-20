using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SummaryPanel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _summaryPanel;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _gems;
    [SerializeField] private TextMeshProUGUI _feather;

    public void ShowWinPanel(string title, int level, int gems, int feather)
    {
        Debug.Log("ShowWinPanel");
        _summaryPanel.SetActive(true);
        _title.text = title;
        _level.text = "Level: " + level.ToString();
        _gems.text = "Gems: " + gems.ToString();
        _feather.text = "Feather: " + feather.ToString();
    }
    public void ShowLosePanel(string title, int level, int gems, int feather)
    {
        Debug.Log("ShowLosePanel");
        _summaryPanel.SetActive(true);
        _title.text = title;
        _title.color = Color.red;
        _level.text = "Level: " + level.ToString();
        _gems.text = "Gems: " + gems.ToString();
        _feather.text = "Feather: " + feather.ToString();
    }

    public void HidePanel()
    {
        _summaryPanel.SetActive(false);
    }


}

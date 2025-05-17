using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefPanelEvent : MonoBehaviour
{
    [SerializeField] private GameObject _refPanel;
    [SerializeField] private GameObject _refElement;

    private void Start()
    {
        _refPanel.SetActive(false);
        _refElement.SetActive(false);
    }

    public void ShowRefPanel()
    {
        _refPanel.SetActive(true);
    }

    public void HideRefPanel()
    {
        _refPanel.SetActive(false);
    }

    public void ShowRefElement()
    {
        _refElement.SetActive(true);
    }

    public void HideRefElement()
    {
        _refElement.SetActive(false);
    }
}

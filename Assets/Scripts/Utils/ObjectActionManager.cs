using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectActionManager : MonoBehaviour
{
    public static ObjectActionManager Instance { get; private set; }

    private Dictionary<string, Action<GameObject>> actionMap = new Dictionary<string, Action<GameObject>>();

    [SerializeField] private List<GameObject> panels;

    private int currentPanelIndex = -1;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            RegisterActions();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void RegisterActions()
    {
        actionMap["Accessory"] = OpenPanel;
        actionMap["Weapon"] = OpenPanel;
        actionMap["Owl"] = OpenPanel;

        actionMap["EditProfile"] = OpenPanel;
        actionMap["MyHeroes"] = OpenPanel;
        actionMap["Inventory"] = OpenPanel;
        actionMap["Setting"] = OpenPanel;

        actionMap["Battle"] = ChangeScene;

        actionMap["EditTeam"] = OpenPanel;
    }

    // Hàm gọi xử lý
    public void ProcessObjectByName(GameObject obj)
    {
        Debug.Log("Processing object: " + obj.name);
        if (actionMap.TryGetValue(obj.name, out var action))
        {
            action.Invoke(obj);
        }
        else
        {
            Debug.LogWarning("No handler for object: " + obj.name);
        }
    }

    void OpenPanel(GameObject obj)
    {
        int panelIndex = -1;

        if (obj.name == "Setting")
        {
            panelIndex = 0;

        }
        else if (obj.name == "EditProfile")
        {
            panelIndex = 1;
        }
        else if (obj.name == "MyHeroes")
        {
            panelIndex = 2;
        }
        else if (obj.name == "EditTeam")
        {
            panelIndex = 0; //another scene so must be 0 first [Scene "LineUp Scene"]
        }

        if (panelIndex >= 0 && panelIndex < panels.Count)
        {
            panels[panelIndex].SetActive(true);
            currentPanelIndex = panelIndex;
        }
        else
        {
            Debug.LogWarning("can't find panel: " + obj.name);
        }
    }

    public void ClosePanel()
    {
        if (currentPanelIndex >= 0 && currentPanelIndex < panels.Count)
        {
            panels[currentPanelIndex].SetActive(false);
            currentPanelIndex = -1; // Reset index
        }
        else
        {
            Debug.LogWarning("Can't find panel to close.");
        }
    }

    //change scene
    void ChangeScene(GameObject obj)
    {
        if (obj.name == "Battle")
            SceneManager.LoadScene("LineUp Scene");
    }
}

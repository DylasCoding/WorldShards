using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using Unity.Services.Authentication;

public class CloudSave : MonoBehaviour
{
    public Text status;
    public InputField inpf;

    public async void Start()
    {
        await UnityServices.InitializeAsync();
        Debug.Log("Player ID: " + AuthenticationService.Instance.PlayerId);
    }

    public async void SaveData()
    {
        var data = new Dictionary<string, object> { { "firstData", inpf.text } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        Debug.Log("Data saved successfully.");
    }

    public async void LoadData()
    {
        var keys = new HashSet<string> { "firstData" };
        Dictionary<string, Item> serverData = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

        if (serverData.ContainsKey("firstData"))
        {
            inpf.text = serverData["firstData"].Value.GetAsString();
            Debug.Log("Data loaded: " + inpf.text);
        }
        else
        {
            Debug.Log("Key not found!");
        }
    }

    public async void DeleteKey()
    {
#pragma warning disable CS0618
        await CloudSaveService.Instance.Data.Player.DeleteAsync("firstData");
#pragma warning restore CS0618
        Debug.Log("Key 'firstData' deleted.");
    }

    public async void RetrieveAllKeys()
    {
        List<ItemKey> allKeys = await CloudSaveService.Instance.Data.Player.ListAllKeysAsync();

        for (int i = 0; i < allKeys.Count; i++)
        {
            Debug.Log("Key: " + allKeys[i].Key);
        }
    }
}
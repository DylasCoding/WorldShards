using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;

public class GachaSystem : MonoBehaviour
{
    [Header("Database & Prefabs")]
    public CharacterDatabase characterDatabase;
    public GameObject gemPrefab;
    public GameObject featherPrefab;
    public GameObject characterPrefab;
    public GameObject vfxPrefab;
    public Transform[] gachaSlots;
    public Transform gachaContainer;

    [Header("UI")]
    public Button gachaButton;
    public GameObject gachaPanel;
    public GameObject notificationPanel;

    [Header("player balance")]
    [SerializeField] private TextMeshProUGUI _gemsText;
    [SerializeField] private TextMeshProUGUI _feathersText;

    [Header("Gacha Settings")]
    [SerializeField] private PlayerInventoryManager _playerInventoryManager;
    [SerializeField] private NotificationManager _notificationManager;

    private bool isRolling = false;

    [Header("roll percentage")]
    private float characterPercentage = 0.1f;
    private float gemPercentage = 0.5f; // 50% chance to get a gem

    private void Awake()
    {
        UpdatePlayerBalance();
    }

    private void UpdatePlayerBalance()
    {
        var profile = LoginController.Instance.PlayerProfile;
        if (_gemsText != null)
            _gemsText.text = profile.Gems.ToString();

        if (_feathersText != null)
            _feathersText.text = profile.Feathers.ToString();
    }

    public void OnGachaButtonClick()
    {
        if (!isRolling)
        {
            AudioManager.Instance.PlayClickSound();
            StartCoroutine(RollGachaCoroutine());
        }
    }

    private IEnumerator RollGachaCoroutine()
    {
        isRolling = true;

        var profile = LoginController.Instance.PlayerProfile;

        if (!CheckBallance(profile))
        {
            _notificationManager.ShowNotification("Not enough gems or feathers to roll.");
            yield break;
        }

        //cost 500 gems to roll
        profile.Gems -= 500;

        ShowGachaPanel();
        gachaButton.interactable = false;
        ClearGachaSlots();

        for (int i = 0; i < gachaSlots.Length; i++)
        {
            Transform slot = gachaSlots[i];

            GameObject vfx = Instantiate(vfxPrefab, slot.position, Quaternion.identity, gachaContainer);
            ParticleSystem ps = vfx.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play();

            GameObject item;
            float rand = Random.value;

            if (rand <= characterPercentage)
            {
                CharacterData character = GetRandomCharacter();
                item = Instantiate(characterPrefab, slot.position, Quaternion.identity, gachaContainer);
                //find component by tag
                Image iconImage = item.transform.Find("CharImage").GetComponent<Image>();
                iconImage.sprite = character.characterImage;

                AudioManager.Instance.PlaySFXOverlay(AudioManager.Instance.audioData.summonSound);

                bool isOwned = profile.ownedCharacters.Exists(c => c.characterID == character.characterID);

                if (!isOwned)
                {
                    profile.ownedCharacters.Add(new OwnedCharacter
                    {
                        characterID = character.characterID,
                        level = 1,
                        isUnlocked = true
                    });

                    _playerInventoryManager.UpdateOwnedCharacters(profile.ownedCharacters);
                    _playerInventoryManager.SaveToJson();

                    StartCoroutine(OnNotification());
                }
                else
                {
                    _notificationManager.ShowNotification("Character already owned!");
                }

            }
            else
            {
                float currencyRand = Random.value;
                if (currencyRand < gemPercentage)
                {
                    item = Instantiate(gemPrefab, slot.position, Quaternion.identity, gachaContainer);

                    AudioManager.Instance.PlaySFXOverlay(AudioManager.Instance.audioData.summonSound);

                    //random gem amount
                    int gemAmount = Random.Range(20, 50);
                    item.GetComponentInChildren<TextMeshProUGUI>().text = gemAmount.ToString();
                    profile.Gems += gemAmount;
                }
                else
                {
                    item = Instantiate(featherPrefab, slot.position, Quaternion.identity, gachaContainer);

                    AudioManager.Instance.PlaySFXOverlay(AudioManager.Instance.audioData.summonSound);


                    //random feather amount
                    int featherAmount = Random.Range(20, 100);
                    item.GetComponentInChildren<TextMeshProUGUI>().text = featherAmount.ToString();
                    profile.Feathers += featherAmount;
                }
            }

            StartCoroutine(DeactivateVFX(vfx, 1f));
            yield return new WaitForSeconds(0.1f);
        }
#pragma warning disable CS8602
        UpdateBalance(profile);
#pragma warning restore CS8602

        LoginController.Instance.UpdatePlayerProfileWithoutSave(profile);

        yield return new WaitForSeconds(0.5f);
        gachaButton.interactable = true;
        isRolling = false;
        ShowGachaPanel();
        UpdatePlayerBalance();
    }

    private CharacterData GetRandomCharacter()
    {
        int index = Random.Range(0, characterDatabase.characters.Count);
        Debug.Log("Character ID: " + characterDatabase.characters[index].characterID);
        return characterDatabase.characters[index];
    }

    private void ClearGachaSlots()
    {
        foreach (Transform child in gachaContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerator DeactivateVFX(GameObject vfx, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (vfx != null) vfx.SetActive(false);
    }

    private IEnumerator OnNotification()
    {
        notificationPanel.SetActive(true);

        RectTransform rectTransform = notificationPanel.GetComponent<RectTransform>();
        Vector3 startPos = new Vector3(-300, 130, 0);
        Vector3 midPos = new Vector3(0, 130, 0);
        Vector3 endPos = new Vector3(300, 130, 0);

        float slideInDuration = 0.2f;
        float slideOutDuration = 0.2f;
        float elapsedTime = 0f;

        // Slide in
        rectTransform.localPosition = startPos;
        while (elapsedTime < slideInDuration)
        {
            rectTransform.localPosition = Vector3.Lerp(startPos, midPos, elapsedTime / slideInDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rectTransform.localPosition = midPos;

        // Wait at center
        yield return new WaitForSeconds(0.5f);

        // Slide out
        elapsedTime = 0f;
        while (elapsedTime < slideOutDuration)
        {
            rectTransform.localPosition = Vector3.Lerp(midPos, endPos, elapsedTime / slideOutDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rectTransform.localPosition = endPos;

        // Hide panel
        notificationPanel.SetActive(false);
    }

    //check if player has enough gems and feathers to roll
    private bool CheckBallance(PlayerProfile profile)
    {
        if (profile.Gems >= 500)
        {
            return true;
        }
        else
        {
            Debug.Log("Not enough gems or feathers to roll.");
            return false;
        }
    }

    private async Task UpdateBalance(PlayerProfile profile)
    {
        await Task.Delay(10);

        await DataSyncManager.SaveGems(profile.Gems);
        await DataSyncManager.SaveFeathers(profile.Feathers);
    }

    private void ShowGachaPanel()
    {
        if (isRolling)
        {
            gachaPanel.SetActive(true);
        }
        else
        {
            gachaPanel.SetActive(false);
        }
    }

}

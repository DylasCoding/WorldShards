using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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

    private bool isRolling = false;
    private bool isShowNotification = false;

    [Header("roll percentage")]
    private float characterPercentage = 0.1f;
    private float gemPercentage = 0.45f; // 50% chance to get a gem

    public void OnGachaButtonClick()
    {
        if (!isRolling)
        {
            StartCoroutine(RollGachaCoroutine());
        }
    }

    private IEnumerator RollGachaCoroutine()
    {
        isRolling = true;
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
                if (!isShowNotification)
                    StartCoroutine(OnNotification());
            }
            else
            {
                float currencyRand = Random.value;
                if (currencyRand < gemPercentage)
                {
                    item = Instantiate(gemPrefab, slot.position, Quaternion.identity, gachaContainer);
                }
                else
                {
                    item = Instantiate(featherPrefab, slot.position, Quaternion.identity, gachaContainer);
                }
            }

            StartCoroutine(DeactivateVFX(vfx, 1f));
            yield return new WaitForSeconds(0.1f); //wait for the VFX to finish before showing the item
        }

        yield return new WaitForSeconds(0.5f);
        gachaButton.interactable = true;
        isRolling = false;
        ShowGachaPanel();
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
        isShowNotification = true;
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
        isShowNotification = false;
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

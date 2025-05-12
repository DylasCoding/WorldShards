using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamDisplayManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TeamManager teamManager;

    [Header("Character Display Objects")]
    [SerializeField] private SpriteRenderer characterSprite1;
    [SerializeField] private SpriteRenderer characterSprite2;
    [SerializeField] private SpriteRenderer characterSprite3;
    [SerializeField] private SpriteRenderer characterSprite4;

    [Header("Team Ultimate Images")]
    [SerializeField] private Image teamUltimateImage1;
    [SerializeField] private Image teamUltimateImage2;
    [SerializeField] private Image teamUltimateImage3;
    [SerializeField] private Image teamUltimateImage4;

    [Header("Team Select Buttons")]
    [SerializeField] private Button teamSelectButton1;
    [SerializeField] private Button teamSelectButton2;
    [SerializeField] private Button teamSelectButton3;
    [SerializeField] private Button teamSelectButton4;

    // List
    private List<SpriteRenderer> characterSprites = new List<SpriteRenderer>();
    private List<Image> teamUltimateImages = new List<Image>();
    private List<Button> teamSelectButtons = new List<Button>();

    private int selectedButtonIndex = -1;

    private void Awake()
    {
        teamManager.LoadTeamFromJson();
        // Thêm tất cả SpriteRenderer vào list để dễ quản lý
        characterSprites.Add(characterSprite1);
        characterSprites.Add(characterSprite2);
        characterSprites.Add(characterSprite3);
        characterSprites.Add(characterSprite4);

        // Thêm tất cả Image vào list để dễ quản lý
        teamUltimateImages.Add(teamUltimateImage1);
        teamUltimateImages.Add(teamUltimateImage2);
        teamUltimateImages.Add(teamUltimateImage3);
        teamUltimateImages.Add(teamUltimateImage4);

        // Thêm tất cả Button vào list để dễ quản lý
        teamSelectButtons.Add(teamSelectButton1);
        teamSelectButtons.Add(teamSelectButton2);
        teamSelectButtons.Add(teamSelectButton3);
        teamSelectButtons.Add(teamSelectButton4);
    }

    private void Start()
    {
        UpdateCharacterSprites();

        if (teamSelectButtons.Count == 0)
        {
            for (int i = 0; i < teamSelectButtons.Count; i++)
            {
                int index = i; // Lưu chỉ số của nút
                teamSelectButtons[i].onClick.AddListener(() => OnTeamButtonClicked(index));
                Debug.Log($"Team button {index} set up.");
            }
        }
    }

    public void UpdateCharacterSprites()
    {
        foreach (var spriteRenderer in characterSprites)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.gameObject.SetActive(false);
            }
        }

        foreach (var image in teamUltimateImages)
        {
            if (image != null)
            {
                image.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            CharacterData characterData = teamManager.GetCharacterData(i);

            if (characterData != null && i < characterSprites.Count && characterSprites[i] != null)
            {
                characterSprites[i].gameObject.SetActive(true);
                characterSprites[i].sprite = characterData.characterSprite;

                if (teamUltimateImages[i] != null)
                {
                    teamUltimateImages[i].gameObject.SetActive(true);
                    teamUltimateImages[i].sprite = characterData.CharacterUltimateImage;
                }

            }
        }
    }

    public void OnTeamButtonClicked(int index)
    {
        selectedButtonIndex = index; // Lưu chỉ số của nút được chọn
        Debug.Log($"Selected Team Button: {index}");
    }

    public void ReplaceCharacterWithSelectedSlot(PlayerCharacterEntry selectedCharacter)
    {
        if (selectedButtonIndex < 0 || selectedButtonIndex >= teamManager.GetTeamSize())
        {
            Debug.LogError("Invalid team button index");
            return;
        }

        // Sử dụng hàm SetCharacterSate để thay đổi dữ liệu
        teamManager.SetCharacterSate(selectedButtonIndex, selectedCharacter);
        UpdateCharacterSprites();
        teamManager.SaveTeamToJson();
        Debug.Log($"Replaced character at index {selectedButtonIndex} with {selectedCharacter.characterData.characterName}");
    }

    public void SaveTeam()
    {
        teamManager.SaveTeamToJson();
    }

    public void ReloadTeam()
    {
        teamManager.LoadTeamFromJson();
        teamManager.InitializeTeam();
        UpdateCharacterSprites();
    }
}
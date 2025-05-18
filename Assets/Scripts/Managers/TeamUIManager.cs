using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TeamDisplayManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TeamManager teamManager;
    [SerializeField] private List<TeamManager> _enemyTeamManager = new List<TeamManager>();
    [SerializeField] private bool isEnemyTeam;
    [SerializeField] private CharacterClassIcons _characterClassIcons;
    [SerializeField] private ElementIcons _elementIcons;

    [Header("Character Display Objects")]
    [SerializeField] private SpriteRenderer characterSprite1;
    [SerializeField] private SpriteRenderer characterSprite2;
    [SerializeField] private SpriteRenderer characterSprite3;
    [SerializeField] private SpriteRenderer characterSprite4;

    [Header("Character Type Images")]
    [SerializeField] private List<SpriteRenderer> characterTypeImages = new List<SpriteRenderer>();

    [Header("Character Element Images")]
    [SerializeField] private List<SpriteRenderer> characterElementImages = new List<SpriteRenderer>();

    [Header("Level Text")]
    [SerializeField] private List<TextMeshProUGUI> levelTexts = new List<TextMeshProUGUI>();

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
        int stageIndex = PlayerPrefs.GetInt("Stage", 1);
        if (isEnemyTeam)
        {
            teamManager = _enemyTeamManager[stageIndex - 1];
            teamManager.LoadTeamFromJson();
        }
        else
        {
            teamManager.LoadTeamFromJson();
        }
        Debug.Log($"Stage Index: {stageIndex}");
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

        foreach (var levelText in levelTexts)
        {
            if (levelText != null)
            {
                levelText.gameObject.SetActive(false);
            }
        }

        foreach (var characterTypeImage in characterTypeImages)
        {
            if (characterTypeImage != null)
            {
                characterTypeImage.gameObject.SetActive(false);
            }
        }

        foreach (var characterElementImage in characterElementImages)
        {
            if (characterElementImage != null)
            {
                characterElementImage.gameObject.SetActive(false);
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

                if (levelTexts[i] != null)
                {
                    levelTexts[i].gameObject.SetActive(true);

                    int level = teamManager.GetLevel(i);
                    levelTexts[i].text = "Lv " + level.ToString();
                }

                if (characterTypeImages[i] != null)
                {
                    characterTypeImages[i].gameObject.SetActive(true);
                    characterTypeImages[i].sprite = _characterClassIcons.GetIcon(characterData.characterType);
                }

                if (characterElementImages[i] != null)
                {
                    characterElementImages[i].gameObject.SetActive(true);
                    characterElementImages[i].sprite = _elementIcons.GetIcon(characterData.elementType);
                }

            }
        }
    }

    public void OnTeamButtonClicked(int index)
    {
        selectedButtonIndex = index;
        Debug.Log($"Selected Team Button: {index}");
    }

    public async void ReplaceCharacterWithSelectedSlot(PlayerCharacterEntry selectedCharacter)
    {
        if (selectedButtonIndex < 0)
        {
            Debug.LogError("Invalid team button index");
            return;
        }

        teamManager.SetCharacterSate(selectedButtonIndex, selectedCharacter);
        UpdateCharacterSprites();

#pragma warning disable CS0618
        await teamManager.SaveTeamToJson();
#pragma warning restore CS0618

        Debug.Log($"Replaced character at index {selectedButtonIndex} with {selectedCharacter.characterData.characterName}");
    }

    public async void SaveTeam()
    {
#pragma warning disable CS0618
        await teamManager.SaveTeamToJson();
#pragma warning restore CS0618
    }

    public void ReloadTeam()
    {
        teamManager.LoadTeamFromJson();
        UpdateCharacterSprites();
    }

}
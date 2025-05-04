using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamDisplayManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TeamManager teamManager;

    [Header("Character Display Objects")]
    [SerializeField] private SpriteRenderer characterSprite1;
    [SerializeField] private SpriteRenderer characterSprite2;
    [SerializeField] private SpriteRenderer characterSprite3;
    [SerializeField] private SpriteRenderer characterSprite4;

    // List để dễ truy cập
    private List<SpriteRenderer> characterSprites = new List<SpriteRenderer>();

    private void Awake()
    {
        // Thêm tất cả SpriteRenderer vào list để dễ quản lý
        characterSprites.Add(characterSprite1);
        characterSprites.Add(characterSprite2);
        characterSprites.Add(characterSprite3);
        characterSprites.Add(characterSprite4);
    }

    private void Start()
    {
        UpdateCharacterSprites();
    }

    // Cập nhật tất cả sprite nhân vật dựa vào TeamManager
    public void UpdateCharacterSprites()
    {
        // Ẩn tất cả sprite trước
        foreach (var spriteRenderer in characterSprites)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.gameObject.SetActive(false);
            }
        }

        // Hiển thị và cập nhật sprite cho các nhân vật trong team
        for (int i = 0; i < 4; i++)
        {
            CharacterData characterData = teamManager.GetCharacterData(i);

            if (characterData != null && i < characterSprites.Count && characterSprites[i] != null)
            {
                characterSprites[i].gameObject.SetActive(true);
                characterSprites[i].sprite = characterData.characterSprite;

                // Thêm các điều chỉnh khác nếu cần
                // Ví dụ: characterSprites[i].sortingOrder = i;
            }
        }
    }

    // Phương thức để thay đổi nhân vật tại một vị trí cụ thể
    public void ChangeCharacterAt(int index, CharacterData newCharacterData)
    {
        if (index < 0 || index >= 4)
        {
            Debug.LogError("Invalid character index");
            return;
        }

        // Tạo mảng mới từ team hiện tại
        CharacterData[] currentTeam = new CharacterData[4];
        for (int i = 0; i < 4; i++)
        {
            currentTeam[i] = teamManager.GetCharacterData(i);
        }

        // Thay đổi nhân vật tại vị trí được chỉ định
        currentTeam[index] = newCharacterData;

        // Cập nhật team mới
        teamManager.SetUpCharacter(currentTeam);

        // Cập nhật sprites
        UpdateCharacterSprites();
    }

    // Phương thức để hoán đổi vị trí hai nhân vật
    public void SwapCharacters(int indexA, int indexB)
    {
        teamManager.SwapCharacters(indexA, indexB);
        UpdateCharacterSprites();
    }

    // Phương thức để lấy thông tin sức khỏe của nhân vật
    public int GetCharacterHealth(int index)
    {
        return teamManager.GetCurrentHealth(index);
    }

    public int GetCharacterMaxHealth(int index)
    {
        return teamManager.GetMaxHealth(index);
    }

    // Cập nhật sức khỏe của nhân vật
    public void UpdateCharacterHealth(int index, int health)
    {
        teamManager.UpdateCurrentHealth(index, health);
    }

    // Lưu team sau khi thay đổi
    public void SaveTeam()
    {
        teamManager.SaveTeamToJson();
    }

    // Tải lại team từ file
    public void ReloadTeam()
    {
        teamManager.LoadTeam();
        teamManager.InitializeTeam();
        UpdateCharacterSprites();
    }

    // Để hiển thị các buff của nhân vật (nếu cần)
    public void ShowCharacterBuffs(int index, Transform targetPosition)
    {
        teamManager.GetAllBuffs(index, targetPosition);
    }

    // Để ẩn các buff của nhân vật
    public void HideCharacterBuffs(int index)
    {
        teamManager.CancelBuff(index);
    }
}
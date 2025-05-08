using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterRosterManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject characterSlotPrefab;
    [SerializeField] private PlayerInventoryManager playerInventoryManager;
    [SerializeField] private ShowInformation showInformation;

    [Header("Configuration")]
    [SerializeField] private bool instantiateOnAwake = true;
    [SerializeField] private bool instantiateOnStart = false;

    // Event để thông báo khi một nhân vật được chọn
    public delegate void CharacterSelectedHandler(CharacterData selectedCharacter);
    public event CharacterSelectedHandler OnCharacterSelected;

    // Danh sách các slot đã tạo
    private List<CharacterSlotUI> characterSlots = new List<CharacterSlotUI>();

    private void Awake()
    {
        if (instantiateOnAwake)
        {
            PopulateCharacterRoster();
        }
    }

    private void Start()
    {
        if (instantiateOnStart && !instantiateOnAwake)
        {
            PopulateCharacterRoster();
        }
    }

    // Tạo và hiển thị tất cả nhân vật từ database
    public void PopulateCharacterRoster()
    {
        ClearAllSlots();
        playerInventoryManager.LoadFromJson(); // Tải dữ liệu từ JSON nếu cần
        if (playerInventoryManager == null || playerInventoryManager.ownedCharacters == null)
        {
            Debug.LogError("Player inventory is null or empty!");
            return;
        }

        foreach (PlayerCharacterEntry entry in playerInventoryManager.ownedCharacters)
        {
            if (entry.isUnlocked)
            {
                CreateCharacterSlot(entry);
            }
        }
    }

    // Tạo một slot mới cho nhân vật
    private void CreateCharacterSlot(PlayerCharacterEntry playerCharacterEntry)
    {
        if (characterSlotPrefab == null)
        {
            Debug.LogError("Character slot prefab is missing!");
            return;
        }

        // Tạo một instance mới từ prefab và thêm vào chính container này
        GameObject slotInstance = Instantiate(characterSlotPrefab, transform);
        CharacterSlotUI slotUI = slotInstance.GetComponent<CharacterSlotUI>();

        if (slotUI != null)
        {
            // Thiết lập dữ liệu cho slot
            slotUI.SetCharacterData(playerCharacterEntry);
            slotUI.SetShowInformation(showInformation);

            // Đăng ký sự kiện click
            slotUI.OnSlotClicked += HandleCharacterSlotClicked;

            // Thêm vào danh sách để quản lý
            characterSlots.Add(slotUI);
        }
        else
        {
            Debug.LogError("CharacterSlotUI component not found on prefab!");
        }
    }

    // Xử lý khi một slot được click
    private void HandleCharacterSlotClicked(CharacterSlotUI slot)
    {
        // Thông báo cho các listener khác về nhân vật được chọn
        OnCharacterSelected?.Invoke(slot.CharacterData);

        // Tuỳ chọn: Đặt trạng thái được chọn cho slot
        foreach (CharacterSlotUI charSlot in characterSlots)
        {
            charSlot.SetSelected(charSlot == slot);
        }
    }

    // Xóa tất cả các slot
    public void ClearAllSlots()
    {
        foreach (CharacterSlotUI slot in characterSlots)
        {
            // Hủy đăng ký sự kiện
            if (slot != null)
            {
                slot.OnSlotClicked -= HandleCharacterSlotClicked;
                Destroy(slot.gameObject);
            }
        }

        characterSlots.Clear();

        // Đảm bảo xóa hết các child object trong container này
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    // Phương thức để lọc nhân vật theo điều kiện// BUG
    public void FilterCharacters(System.Predicate<PlayerCharacterEntry> filterCondition)
    {
        if (playerInventoryManager == null || playerInventoryManager.ownedCharacters == null)
            return;

        ClearAllSlots();

        foreach (PlayerCharacterEntry entry in playerInventoryManager.ownedCharacters)
        {
            if (entry.isUnlocked && filterCondition(entry))
            {
                CreateCharacterSlot(entry);
            }
        }
    }

    // Phương thức để hiển thị tất cả nhân vật
    public void ShowAllCharacters()
    {
        PopulateCharacterRoster();
    }

    // Phương thức để lấy nhân vật được chọn và gửi event
    public void SelectCharacter(int index)
    {
        if (index >= 0 && index < characterSlots.Count)
        {
            OnCharacterSelected?.Invoke(characterSlots[index].CharacterData);

            // Cập nhật trạng thái được chọn
            for (int i = 0; i < characterSlots.Count; i++)
            {
                characterSlots[i].SetSelected(i == index);
            }
        }
    }

    // Phương thức để xử lý khi component bị hủy
    private void OnDestroy()
    {
        // Hủy đăng ký tất cả các sự kiện để tránh memory leak
        foreach (CharacterSlotUI slot in characterSlots)
        {
            if (slot != null)
            {
                slot.OnSlotClicked -= HandleCharacterSlotClicked;
            }
        }
    }
}
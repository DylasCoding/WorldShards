using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro; // Thêm thư viện TMP nếu bạn sử dụng TextMeshPro
public class CharacterSlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI References")]
    [SerializeField] private Image backgroundImage;  // Image bên ngoài (không thay đổi)
    [SerializeField] private Image characterImage;   // Image bên trong (sẽ thay đổi theo character)

    [SerializeField] private SwitchImageButton switchImageButton; // set true/false cho image

    [Header("Optional UI Elements")]
    [SerializeField] private TextMeshProUGUI characterNameText;     // Hiển thị tên nhân vật (nếu cần)
    [SerializeField] private GameObject selectionIndicator; // Hiển thị trạng thái chọn

    private CharacterData _characterData;

    public CharacterData CharacterData => _characterData;

    // Event khi slot được click
    public delegate void SlotClickedHandler(CharacterSlotUI slot);
    public event SlotClickedHandler OnSlotClicked;

    public void SetCharacterData(CharacterData characterData)
    {
        _characterData = characterData;
        UpdateUI();
    }

    // Cập nhật các phần tử UI dựa trên dữ liệu nhân vật
    private void UpdateUI()
    {
        if (_characterData == null)
        {
            // Ẩn hoặc hiển thị placeholder nếu không có dữ liệu
            if (characterImage != null)
                characterImage.gameObject.SetActive(false);

            if (characterNameText != null)
                characterNameText.gameObject.SetActive(false);
            return;
        }

        if (characterImage != null)
        {
            characterImage.gameObject.SetActive(true);
            characterImage.sprite = _characterData.characterImage;

            // Điều chỉnh màu nếu cần
            characterImage.color = Color.white;

            characterNameText.gameObject.SetActive(true);
            characterNameText.text = _characterData.characterName;
        }
    }

    // Xử lý sự kiện click
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Slot clicked: " + _characterData.characterName);
        // Kích hoạt sự kiện click
        OnSlotClicked?.Invoke(this);

        // Hiển thị chỉ báo đã chọn nếu có
        if (selectionIndicator != null)
        {
            selectionIndicator.SetActive(true);
        }
    }

    public void CheckClick()
    {
        Debug.Log("Slot clicked:");
    }

    // Đặt trạng thái đã chọn
    public void SetSelected(bool selected)
    {

        if (selectionIndicator != null)
        {
            selectionIndicator.SetActive(selected);
        }

        Debug.Log("Slot selected: " + _characterData.characterName);

        // Bạn cũng có thể thay đổi hiệu ứng khác như màu sắc
        if (backgroundImage != null)
        {
            backgroundImage.color = selected ? new Color(0.8f, 0.8f, 1f) : Color.white;
        }
    }
}
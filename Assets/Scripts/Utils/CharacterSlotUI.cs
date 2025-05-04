using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System; // Thêm thư viện TMP nếu bạn sử dụng TextMeshPro
public class CharacterSlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI References")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image characterImage;

    [Header("Character Information")]
    [SerializeField] private TextMeshProUGUI _charName;
    [SerializeField] private TextMeshProUGUI _charLevel;
    [SerializeField] private TextMeshProUGUI _charClass;
    [SerializeField] private TextMeshProUGUI _charHealth;
    [SerializeField] private TextMeshProUGUI _charAttack;
    [SerializeField] private TextMeshProUGUI _charDefense;

    [SerializeField] private SwitchImageButton switchImageButton;

    [Header("Optional UI Elements")]
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private GameObject selectionIndicator;

    private CharacterData _characterData;

    public CharacterData CharacterData => _characterData;

    // Event khi slot được click
    public delegate void SlotClickedHandler(CharacterSlotUI slot);
    public event SlotClickedHandler OnSlotClicked;

    private void Start()
    {
        if (switchImageButton != null)
        {
            SwitchImageGroupManager.Instance.Register(switchImageButton);
            GetComponent<Button>().onClick.AddListener(switchImageButton.OnClick);
        }
    }

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
        // Debug.Log("Slot clicked: " + _characterData.characterName);
        // Kích hoạt sự kiện click
        OnSlotClicked?.Invoke(this);

        // Hiển thị chỉ báo đã chọn nếu có
        if (selectionIndicator != null)
        {
            selectionIndicator.SetActive(true);
        }
    }

    // Đặt trạng thái đã chọn
    public void SetSelected(bool selected)
    {
        if (selectionIndicator != null)
        {
            selectionIndicator.SetActive(selected);
        }
        if (_charName != null)
        {
            _charName.text = _characterData.characterName;
        }
        if (_charHealth != null)
        {
            _charHealth.text = _characterData.maxHealth.ToString();
        }
        if (_charAttack != null)
        {
            _charAttack.text = _characterData.attackDamage.ToString();
        }
        if (_charDefense != null)
        {
            _charDefense.text = _characterData.defense.ToString();
        }
    }
}
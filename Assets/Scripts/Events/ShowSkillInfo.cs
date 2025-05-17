using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShowSkillInfo : MonoBehaviour
{
    [SerializeField] private ElementIcons _elementIcons;

    [SerializeField] private List<Image> _elementImages;
    [SerializeField] private List<Image> _skillImages;
    [SerializeField] private List<TextMeshProUGUI> _skillNames;
    [SerializeField] private List<TextMeshProUGUI> _skillDescriptions;
    [SerializeField] private List<TextMeshProUGUI> _skillDamage;

    public void ShowSkillInfoPanel(CharacterData characterData)
    {
        if (characterData == null)
        {
            Debug.LogError("CharacterData is null!");
            return;
        }
        if (characterData.basicAttack != null)
        {
            _skillImages[0].sprite = characterData.basicAttack.skillImage;
            _elementImages[0].sprite = _elementIcons.GetIcon(characterData.basicAttack.elementType);
            _skillNames[0].text = characterData.basicAttack.skillName;
            _skillDescriptions[0].text = characterData.basicAttack.description;
            _skillDamage[0].text = "Damage: +" + characterData.basicAttack.damageIncrease.ToString();
        }
        if (characterData.specialSkill1 != null)
        {
            _skillImages[1].sprite = characterData.specialSkill1.skillImage;
            _elementImages[1].sprite = _elementIcons.GetIcon(characterData.specialSkill1.elementType);
            _skillNames[1].text = characterData.specialSkill1.skillName;
            _skillDescriptions[1].text = characterData.specialSkill1.description;
            _skillDamage[1].text = "Damage: +" + characterData.specialSkill1.damageIncrease.ToString();
        }
        if (characterData.specialSkill2 != null)
        {
            _skillImages[2].sprite = characterData.specialSkill2.skillImage;
            _elementImages[2].sprite = _elementIcons.GetIcon(characterData.specialSkill2.elementType);
            _skillNames[2].text = characterData.specialSkill2.skillName;
            _skillDescriptions[2].text = characterData.specialSkill2.description;
            _skillDamage[2].text = "Damage: +" + characterData.specialSkill2.damageIncrease.ToString();
        }
    }
}

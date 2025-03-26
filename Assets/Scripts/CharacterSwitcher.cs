using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterSwitcher : MonoBehaviour
{
    [SerializeField] private CharacterData[] _characters;
    [SerializeField] private CharacterData _defaultCharacter;
    [SerializeField] private CharController _playerController;

    [SerializeField] private Button[] _switchButton;

    private CharacterData currentCharacter;


    void Start()
    {

    }

}

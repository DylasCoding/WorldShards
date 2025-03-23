using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    public CharacterController currentCharacter;
    public List<CharacterController> characters;

    void Start()
    {
        Debug.Log("Current character: " + currentCharacter.name);
    }

}

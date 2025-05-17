using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEvents : MonoBehaviour
{
    [SerializeField] private SwitchImageButton soundButton;

    private void Start()
    {
        // Set the initial state of the sound button based on the mute state
        CheckSoundImageSprite();
    }

    private void CheckSoundImageSprite()
    {
        if (AudioManager.Instance.GetMuteState())
        {
            soundButton.SetImage(1);
        }
        else
        {
            soundButton.SetImage(0);
        }
    }

    public void OnToggleSound()
    {
        AudioManager.Instance.ToggleMute();
        CheckSoundImageSprite();
    }
}

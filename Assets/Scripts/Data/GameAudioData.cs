using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameAudioConfig", menuName = "GameData/GameAudio")]
public class GameAudioData : ScriptableObject
{
    [Header("Audio Clips")]
    public AudioClip buttonClickSound;
    public AudioClip skillEffectSound;
    public AudioClip characterAttackSound;
    public AudioClip characterHitSound;
    public AudioClip characterDefeatSound;

    [Header("Background Music")]
    public AudioClip backgroundMusic;
    public AudioClip backgroundBattleMusic;

    [Header("Game Events")]
    public AudioClip characterSwitchSound;
    public AudioClip gameStartSound;
    public AudioClip turnSound;
    public AudioClip winSound;
    public AudioClip loseSound;

    [Header("Audio Settings")]
    [Range(0f, 1f)] public float backgroundMusicVolume = 0.5f;
    [Range(0f, 1f)] public float soundEffectsVolume = 0.5f;
}

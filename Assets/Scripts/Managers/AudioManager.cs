using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public GameAudioData audioData;
    private AudioSource musicSource;
    private AudioSource sfxSource;

    private Queue<AudioClip> sfxQueue = new Queue<AudioClip>();
    private bool isPlayingSFX = false;

    private bool isMuted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        // Đọc trạng thái mute từ PlayerPrefs
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        ApplyMuteState();
    }

    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        if (musicClip == null || isMuted) return;

        if (musicSource.clip == musicClip && musicSource.isPlaying)
            return;

        musicSource.clip = musicClip;
        musicSource.volume = audioData.backgroundMusicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void EnqueueSFX(AudioClip clip)
    {
        if (clip == null || isMuted) return;

        sfxQueue.Enqueue(clip);

        if (!isPlayingSFX)
            StartCoroutine(PlaySFXQueue());
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || isMuted) return;

        sfxSource.PlayOneShot(clip, audioData.soundEffectsVolume);
    }

    public void FadeToNewMusic(AudioClip newClip, float fadeDuration = 1f)
    {
        if (newClip == null || isMuted) return;

        if (musicSource.clip == newClip && musicSource.isPlaying)
            return;

        StartCoroutine(FadeMusicRoutine(newClip, fadeDuration));
    }

    public void PlaySFXOverlay(AudioClip clip, float volume = -1f)
    {
        if (clip == null || isMuted) return;

        GameObject tempGO = new GameObject("TempSFX");
        AudioSource tempSource = tempGO.AddComponent<AudioSource>();
        tempSource.clip = clip;
        tempSource.volume = volume > 0 ? volume : audioData.soundEffectsVolume;
        tempSource.Play();

        Destroy(tempGO, clip.length); // Xoá sau khi phát xong
    }


    public void PlayBattleMusic()
    {
        PlayBackgroundMusic(audioData.backgroundBattleMusic);
    }

    public void PlayMainMenuMusic()
    {
        PlayBackgroundMusic(audioData.backgroundMusic);
    }

    public void PlayClickSound()
    {
        PlaySFX(audioData.buttonClickSound);
    }

    public bool GetMuteState()
    {
        return isMuted;
    }

    public bool IsSFXPlaying()
    {
        return isPlayingSFX;
    }

    private IEnumerator PlaySFXQueue()
    {
        isPlayingSFX = true;

        while (sfxQueue.Count > 0)
        {
            AudioClip clipToPlay = sfxQueue.Dequeue();
            sfxSource.PlayOneShot(clipToPlay, audioData.soundEffectsVolume);
            yield return new WaitForSeconds(clipToPlay.length);
        }

        isPlayingSFX = false;
    }

    private IEnumerator FadeMusicRoutine(AudioClip newClip, float duration)
    {
        float startVolume = musicSource.volume;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, audioData.backgroundMusicVolume, t / duration);
            yield return null;
        }

        musicSource.volume = audioData.backgroundMusicVolume;
    }

    // ==== Mute Logic ====
    public void ToggleMute()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
        ApplyMuteState();
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    private void ApplyMuteState()
    {
        musicSource.mute = isMuted;
        sfxSource.mute = isMuted;

        if (isMuted)
        {
            musicSource.Pause();
        }
        else
        {
            musicSource.UnPause();
        }
    }
}

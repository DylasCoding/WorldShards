using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] private GameAudioData audioData;
    private AudioSource musicSource;
    private AudioSource sfxSource;

    private Queue<AudioClip> sfxQueue = new Queue<AudioClip>();
    private bool isPlayingSFX = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Khởi tạo musicSource và sfxSource
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        // PlayBackgroundMusic(audioData.backgroundMusic);
    }

    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        if (musicClip == null) return;

        // Nếu đã phát đúng nhạc này rồi thì thôi
        if (musicSource.clip == musicClip && musicSource.isPlaying)
            return;

        musicSource.clip = musicClip;
        musicSource.volume = audioData.backgroundMusicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }

    // Hàm gọi phát hiệu ứng âm thanh theo thứ tự
    public void EnqueueSFX(AudioClip clip)
    {
        if (clip == null) return;

        sfxQueue.Enqueue(clip);

        if (!isPlayingSFX)
            StartCoroutine(PlaySFXQueue());
    }

    // Hàm  phát ngay lập tức, không chờ hàng đợi
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, audioData.soundEffectsVolume);
    }

    public void FadeToNewMusic(AudioClip newClip, float fadeDuration = 1f)
    {
        if (newClip == null) return;

        // Nếu đang phát đúng bài thì thôi
        if (musicSource.clip == newClip && musicSource.isPlaying)
            return;

        StartCoroutine(FadeMusicRoutine(newClip, fadeDuration));
    }

    public void PlayBattleMusic()
    {
        PlayBackgroundMusic(audioData.backgroundBattleMusic);
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

        // Fade out
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

        // Fade in
        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, audioData.backgroundMusicVolume, t / duration);
            yield return null;
        }

        musicSource.volume = audioData.backgroundMusicVolume;
    }
}

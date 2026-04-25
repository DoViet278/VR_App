using UnityEngine;

public class SoundManage : MonoBehaviour
{
    public static SoundManage Instance { get; private set; }

    [Header("Audio Sources")]
    [Tooltip("Nguồn phát nhạc nền")]
    [SerializeField] private AudioSource bgmSource;
    [Tooltip("Nguồn phát hiệu ứng âm thanh (SFX)")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip clickButtonSound;
    public AudioClip completeBuildPartSound;
    public AudioClip completeBuildFinishSound;
    public AudioClip wrongAnswerSound;
    public AudioClip correctAnswerSound;
    public AudioClip fractureSound;

    private void Awake()
    {
        // Setting up the Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Automatically create AudioSources if they are not assigned in the Inspector
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }
    }

    private void Start()
    {
        // Play the background music when the game starts
        if (backgroundMusic != null)
        {
            PlayBGM(backgroundMusic);
        }
    }

    /// <summary>
    /// Play specific Background Music (BGM).
    /// </summary>
    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;
        
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    /// <summary>
    /// Stop the Background Music.
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    /// <summary>
    /// Play a general Sound Effect (SFX).
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // --- Specific methods to play the required sounds ---

    public void PlayClickButton()
    {
        PlaySFX(clickButtonSound);
    }

    public void PlayCompleteBuildPart()
    {
        PlaySFX(completeBuildPartSound);
    }

    public void PlayCompleteBuildFinish()
    {
        PlaySFX(completeBuildFinishSound);
    }

    public void PlayWrongAnswer()
    {
        PlaySFX(wrongAnswerSound);
    }

    public void PlayCorrectAnswer()
    {
        PlaySFX(correctAnswerSound);
    }

    public void PlayFractureSound()
    {
        PlaySFX(fractureSound);
    }
}

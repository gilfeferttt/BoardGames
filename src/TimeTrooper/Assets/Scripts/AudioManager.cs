using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip countdownClip;
    public AudioClip countdownLastSecondsClip;
    public AudioClip failClip;
    public AudioClip successClip;
    public AudioClip backgroundClip;
    public AudioClip[] rounds;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayMusic(backgroundClip);
    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.volume = 0.1f;
            musicSource.Play();
        }
    }
    public void playCountdown()
    {
        PlaySFX(countdownClip, 0.8f);
    }
    public void playLastSecondsCountdown()
    {
        PlaySFX(countdownLastSecondsClip, 0.3f);
    }
    public void playFail()
    {
        PlaySFX(failClip, 0.5f);
    }
    public void playSuccess()
    {
        PlaySFX(successClip, 0.5f);
    }
    public void playRound(int roundnumber)
    {
        if (roundnumber < rounds.Length)
        {
            PlaySFX(rounds[roundnumber], 0.5f);
        }
    }
    public void stopCountdown()
    {
        StopSFX();
    }
    public void PlaySFX(AudioClip sfxClip, float voliume)
    {
        if (sfxSource != null && sfxClip != null)
        {
            sfxSource.clip = sfxClip;
            sfxSource.volume = voliume;
            sfxSource.Play();
        }
    }
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
    public void PauseMusic()
    {
        if (musicSource != null)
        {
            musicSource.Pause();
        }
    }
    public void UnPauseMusic()
    {
        if (musicSource != null)
        {
            musicSource.UnPause();
        }
    }
    public void StopSFX()
    {
        if (sfxSource != null)
        {
            sfxSource.Stop();
        }
    }
    public void PauseSFX()
    {
        if (sfxSource != null)
        {
            sfxSource.Pause();
        }
    }
    public void UnpauseSFX()
    {
        if (sfxSource != null)
        {
            sfxSource.UnPause();
        }
    }
}

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    

    [Header("SFX Clips")]
    public AudioClip pickupClip;
    public AudioClip doorClip;
    public AudioClip footStepClip;
    public AudioClip waterClip;
    public AudioClip sprayClip;
    public AudioClip wearTowelClip;
    public AudioClip coughClip;
    public AudioClip hurtClip;
    public AudioClip explosionClip;
    public AudioClip fireAlarm;
    public AudioClip scream;
    public AudioClip win;
    public AudioClip lose;
    public AudioClip clickButton;

    private const string SFX_KEY = "SFXVolume";
    private const string BGM_KEY = "BGMVolume";

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
            return;
        }

        LoadVolume();
    }

    private void LoadVolume()
    {
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        float bgmVolume = PlayerPrefs.GetFloat(BGM_KEY, 1f);

        sfxSource.volume = sfxVolume;

      
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;

        PlayerPrefs.SetFloat(SFX_KEY, value);
        PlayerPrefs.Save();
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFX_KEY, 1f);
    }

    public void SetBGMVolume(float value)
    {
    

        PlayerPrefs.SetFloat(BGM_KEY, value);
        PlayerPrefs.Save();
    }

    public float GetBGMVolume()
    {
        return PlayerPrefs.GetFloat(BGM_KEY, 1f);
    }
}
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioLevel SFXLevel;
    public AudioLevel BGMLevel;
    [SerializeField] AudioSource _SFXSource;
    [SerializeField] AudioSource _BGMSource;
    [SerializeField] AudioClip _BGM;
    [SerializeField] AudioClip _selectSFX;
    [SerializeField] AudioClip _submitSFX;

    void OnEnable()
    {
        SettingsEvents.OnSettingsAltered += UpdateAudioSettings;
        SettingsEvents.OnSettingsReverted += UpdateAudioSettings;
    }

    void OnDisable()
    {
        SettingsEvents.OnSettingsAltered -= UpdateAudioSettings;
        SettingsEvents.OnSettingsReverted -= UpdateAudioSettings;
    }


    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);

        else
            instance = this;

    }

    void Start()
    {
        UpdateAudioSettings();
        if (_BGM != null && _BGMSource != null)
            PlayBGM();
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySelectSound()
    {
        if (_selectSFX == null)
        {
            Debug.Log("Select SFX is missing!");
            return;
        }

        _SFXSource.PlayOneShot(_selectSFX);
    }

    public void PlaySubmitSound()
    {
        if (_submitSFX == null)
        {
            Debug.Log("Submit SFX is missing!");
            return;
        }

        _SFXSource.PlayOneShot(_submitSFX);
    }

    public void PlayBGM()
    {
        _BGMSource.clip = _BGM;
        _BGMSource.loop = true;
        _BGMSource.Play();
    }

    public void StopMusic()
    {
        _BGMSource.Stop();
    }

    public void PauseMusic()
    {
        _BGMSource.Pause();
    }

    public void ResumeMusic()
    {
        _BGMSource.UnPause();
    }

    public void UpdateAudioSettings()
    {
        
        _BGMSource.volume = SETTINGS.CurrentBGMVolume;
        _SFXSource.volume = SETTINGS.CurrentSFXVolume;

        BGMLevel = ToAudioLevel(SETTINGS.CurrentBGMVolume);
        SFXLevel = ToAudioLevel(SETTINGS.CurrentSFXVolume);

    }


    float Test(float value)
    {
        value = Mathf.Round(value * 10f) / 10f;
        if (Mathf.Abs(value) < 0.001f)
        {
            value = 0f;
        }

        return value;
    }

    AudioLevel ToAudioLevel(float volume)
    {
        return (AudioLevel)Mathf.FloorToInt(Test(volume) * 10);
    }
}

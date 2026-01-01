using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioLevel SFXLevel;
    public AudioLevel BGMLevel;
    [SerializeField] AudioSource _SFXSource;
    [SerializeField] AudioSource _BGMSource;
    [SerializeField] AudioClip _BGM;
    [SerializeField] AudioClip _selectSFX;
    [SerializeField] AudioClip _submitSFX;
    [SerializeField] AudioCueEventChannelSO _sfxEventChannel;

    void OnEnable()
    {
        _sfxEventChannel.OnAudioCueRequested += PlayAudioCue;
        SettingsEvents.OnSettingsAltered += UpdateAudioSettings;
        SettingsEvents.OnSettingsReverted += UpdateAudioSettings;
    }

    void OnDisable()
    {
        _sfxEventChannel.OnAudioCueRequested -= PlayAudioCue;
        SettingsEvents.OnSettingsAltered -= UpdateAudioSettings;
        SettingsEvents.OnSettingsReverted -= UpdateAudioSettings;
    }


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
    }

    void Start()
    {
        UpdateAudioSettings();
        if (_BGM != null && _BGMSource != null)
            PlayBGM();
        
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

    void PlayAudioCue(SoundData soundData, Vector3 location)
    {
        AudioClip clipToBePlayed = GetClip(soundData);

        if (clipToBePlayed == null)
        {
            Debug.Log("There isn't a valid clip to play.");
            return;
        }

        if (soundData.withVaryingPitch)
        {
            float pitchVariance = soundData.pitchRange;
            float pitch = 1f + Random.Range(-pitchVariance, pitchVariance);
            PlaySFXClip(clipToBePlayed, pitch);
        }

        else
            PlaySFXClip(clipToBePlayed);

    }

    public void PlaySFXClip(AudioClip clip, float pitch = 1f, float volume = 1f)
    {
        if (clip == null) return;

        _SFXSource.clip = clip;
        _SFXSource.pitch = pitch;
        _SFXSource.volume = volume;
        _SFXSource.Play();

    }
    
    public AudioClip GetClip(SoundData soundData)
    {
        if (soundData.clips == null)
        {
            Debug.Log("Passed sound data has no clips.");
            return null;
        }

        if (soundData.playRandomAmongGroup && soundData.clips.Length > 1)
        {
            return soundData.clips[Random.Range(0, soundData.clips.Length)];
        }

        else if (soundData.clips.Length > 0)
        {
            return soundData.clips[0];
        }

        return null;
    }
    

    public void PlayBGM()
    {
        _BGMSource.clip = _BGM;
        _BGMSource.loop = true;
        _BGMSource.Play();
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


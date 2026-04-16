using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioLevel SFXLevel;
    public AudioLevel WeaponSFXLevel;
    public AudioLevel BGMLevel;

    [Header("Managers")]
    [SerializeField] SFXManager _SFXManager;
    [SerializeField] BGMManager _BGMManager;

    [Header("Menu SFX Sound Data")]
    [SerializeField] SoundDataSO _selectSFXSoundData;
    [SerializeField] SoundDataSO _submitSFXSoundData;

    [Header("Event Channel")]
    [SerializeField] AudioCueEventChannelSO _SFXEventChannel;

    void OnEnable()
    {
        _SFXEventChannel.OnAudioCueRequested += PlayAudioCue;

        SettingsManager.OnChangesReverted += UpdateAudioSettings;
        SettingsManager.OnWeaponSFXLevelAltered += UpdateAudioSettings;
        SettingsManager.OnSFXLevelAltered += UpdateAudioSettings;
        SettingsManager.OnBGMLevelAltered += UpdateAudioSettings;
    }

    void OnDisable()
    {
        _SFXEventChannel.OnAudioCueRequested -= PlayAudioCue;

        SettingsManager.OnChangesReverted -= UpdateAudioSettings;
        SettingsManager.OnWeaponSFXLevelAltered += UpdateAudioSettings;
        SettingsManager.OnSFXLevelAltered -= UpdateAudioSettings;
        SettingsManager.OnBGMLevelAltered -= UpdateAudioSettings;
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
        //if (_BGMManager)
            //_BGMManager.PlayBGM();
    }

    public void PlayMenuSelectSound()
    {
        if (_selectSFXSoundData == null)
        {
            //Debug.Log("Select SFX Sound Data is missing!");
            return;
        }

        //Debug.Log("Select sound played.");

        _SFXManager.Play(_selectSFXSoundData);
    }

    public void PlayMenuSubmitSound()
    {
        if (_submitSFXSoundData == null)
        {
            //Debug.Log("Submit SFX Sound Data is missing!");
            return;
        }

        _SFXManager.Play(_submitSFXSoundData);
    }


    void PlayAudioCue(SoundDataSO soundData, Vector3 location)
    {
        AudioClip clipToBePlayed = GetClip(soundData);

        if (clipToBePlayed == null)
        {
            Debug.Log("There isn't a valid clip to play.");
            return;
        }

        _SFXManager.Play(soundData, location);
    }

    /*
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
    */

    /*
    public void PlaySFXClip(AudioClip clip, float pitch = 1f, float volume = 1f)
    {
        if (clip == null) return;

        _SFXSource.clip = clip;
        _SFXSource.pitch = pitch;
        _SFXSource.volume = SETTINGS.CurrentSFXVolume;
        _SFXSource.Play();

    }
    */
    
    public void UpdateAudioSettings()
    {
        
        //_BGMSource.volume = SETTINGS.CurrentBGMVolume;
        _BGMManager.SetVolume(SETTINGS.CurrentBGMVolume);
        //_SFXSource.volume = SETTINGS.CurrentSFXVolume;
        _SFXManager.SetVolume(SETTINGS.CurrentSFXVolume);
        
        //Debug.Log("Current SFX volume : " + _SFXSource.volume);

        BGMLevel = ToAudioLevel(SETTINGS.CurrentBGMVolume);
        SFXLevel = ToAudioLevel(SETTINGS.CurrentSFXVolume);
        WeaponSFXLevel = ToAudioLevel(SETTINGS.CurrentWeaponSFXVolume);

    }

    public AudioClip GetClip(SoundDataSO soundData)
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

    AudioLevel ToAudioLevel(float volume)
    {
        return (AudioLevel)Mathf.FloorToInt(ConversionFunction(volume) * 10);
    }

    float ConversionFunction(float value)
    {
        value = Mathf.Round(value * 10f) / 10f;
        if (Mathf.Abs(value) < 0.001f)
        {
            value = 0f;
        }

        return value;
    }

}


using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Pool;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class GameOverUI : MonoBehaviour
{
    // Note to self :
    // Unity can not serialize InputActions. 
    [SerializeField] AudioCueEventChannelSO _sfxEventChannel;
    [SerializeField] Canvas _canvas;
    [SerializeField] GameObject _titleDisplay;
    [SerializeField] GameObject _killCountGroup;
    [SerializeField] GameObject _timeTakenGroup;
    [SerializeField] GameObject _defaultSelected;
    [SerializeField] TMP_Text _timeTakenText;
    [SerializeField] TMP_Text _killsText;
    Tween _currentActiveTween;
    Tween _timeTween;
    Tween _killsTween;
    Sequence _sequence;

    [SerializeField] SoundData _popSFX;

    void Awake()
    {
        GameManager.OnGameOverShowGameOverMenu += ShowGameOverMenu;
    }

    void OnDisable()
    {
        GameManager.OnGameOverShowGameOverMenu -= ShowGameOverMenu;
    }


    void Update()
    {
        if(PlayerInputManager.Instance.SubmitPressed && _sequence.IsActive())
        {
            Debug.Log("Trying to skip current tween.");
            SkipCurrentTween();
        }

    }

    void ShowGameOverMenu()
    {
        if (_canvas != null)
        {
            _canvas.enabled = true;
            if (_defaultSelected != null)
            PlaySequence();
        }
    }

    
    void PlaySequence()
    {
        int finalKillCount = StatTracker.Instance.KillCount;
        int finalTimeTaken = Mathf.RoundToInt(StatTracker.Instance.ElapsedTime);

        _sequence = DOTween.Sequence();

        // ----- Create main tweens -----

        Tween killsTween = DOTween.To(() => 0, value => _killsText.text = value.ToString(), finalKillCount, 5f)
                           .SetEase(Ease.OutCubic);

        Tween timeTween = DOTween.To(() => 0, value => UpdateFormattedTime(value), finalTimeTaken, 1.5f)
                          .SetEase(Ease.OutSine);


        // ----- Sequence Steps -----

        // Interval + title
        {
            Tween t = CreateInterval(0.5f);
            _sequence.Append(t.OnStart(() => _currentActiveTween = t));

            _sequence.AppendCallback(() => _titleDisplay.SetActive(true));
        }

        // Interval + kill count title
        {
            Tween t = CreateInterval(0.5f);
            _sequence.Append(t.OnStart(() => _currentActiveTween = t));

            _sequence.AppendCallback(() => _killCountGroup.SetActive(true));
        }

        // Kill counter tween
        _sequence.Append(
        killsTween.OnStart(() =>
            {
                _currentActiveTween = killsTween;
                _killsText.enabled = true;
                _sfxEventChannel.RaiseEvent(_popSFX, transform.position);
            })
        );

        // Interval + time taken title
        {
            Tween t = CreateInterval(0.5f);
            _sequence.Append(t.OnStart(() => _currentActiveTween = t));

            _sequence.AppendCallback(() => _timeTakenGroup.SetActive(true));
        }

        // Time tween
        _sequence.Append(
            timeTween.OnStart(() =>
            {
                _currentActiveTween = timeTween;
                _timeTakenText.enabled = true;
                _sfxEventChannel.RaiseEvent(_popSFX, transform.position);
            })
        );

        _sequence.OnComplete(OnSequenceComplete);
        _sequence.Play();
    }
  
    // Apparently, appended intervals are not tweens so I wrapped them to make them "fake" tweens.
    Tween CreateInterval(float duration)
    {
        return DOTween.To(() => 0f, _ => { }, 0f, duration);
    }


    void SkipCurrentTween()
    {
        if (_currentActiveTween != null && _currentActiveTween.IsActive() && _currentActiveTween.IsPlaying())
            _currentActiveTween.Complete();
    }





    void OnSequenceComplete()
    {
        UISelector.instance.SetSelected(_defaultSelected);
    }

    void UpdateFormattedTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        _timeTakenText.text = $"{minutes:00}:{seconds:00}";
    }



    /*
    void InitializePool()
    {
        _emitterPool = new ObjectPool<SoundEmitter>(
        createFunc: CreatePooledObject,
        actionOnGet: OnGetFromPool,
        actionOnRelease: OnReleaseToPool,
        actionOnDestroy: OnDestroyPooledObject,
        collectionCheck: false,
        defaultCapacity: _defaultPoolCapacity,
        maxSize: _maxPoolSize
        );

        Debug.Log("Initialized pool :3");
    }

    SoundEmitter CreatePooledObject()
    {
        GameObject go = Instantiate(_soundEmitterPrefab, _soundEmitterParentTransform);
        SoundEmitter soundEmitter = go.GetComponent<SoundEmitter>();
        soundEmitter.Initialize();
        soundEmitter.SetReturnAction(OnReleaseToPool);
        return soundEmitter;
    }

    void OnGetFromPool(SoundEmitter soundEmitter)
    {
        if (soundEmitter != null)
            soundEmitter.gameObject.SetActive(true);
    }

    void OnReturnToPool(SoundEmitter soundEmitter)
    {
        _emitterPool.Release(soundEmitter);
    }

    void OnReleaseToPool(SoundEmitter soundEmitter)
    {
        if (soundEmitter != null)
            soundEmitter.gameObject.SetActive(false);
    }

    void OnDestroyPooledObject(SoundEmitter soundEmitter)
    {
        Destroy(soundEmitter.gameObject);
    }

    
    public void TryPlaySFX(SoundData soundData)
    {
        AudioClip clipToBePlayed = GetClip(soundData);
        if (clipToBePlayed == null)
        {
            Debug.Log("There isn't a valid clip to play.");
            return;
        }

        SoundEmitter soundEmitter = _emitterPool.Get();
        soundEmitter.SetReturnAction(OnReturnToPool);

        if (soundData.withVaryingPitch)
        {
            float pitchVariance = soundData.pitchRange;
            float pitch = 1f + Random.Range(-pitchVariance, pitchVariance);
            soundEmitter.Play(clipToBePlayed, pitch);
        }

        else
            soundEmitter.Play(clipToBePlayed);
        
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
    */

}

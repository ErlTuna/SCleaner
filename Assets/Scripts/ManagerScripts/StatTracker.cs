using UnityEngine;

public class StatTracker : MonoBehaviour
{
    [SerializeField] VoidEventChannelSO _enemyDeathEventChannel;
    [SerializeField] float _elapsedTime;
    public float ElapsedTime => _elapsedTime;
    [SerializeField] int _killCount;
    public int KillCount => _killCount;
    bool _trackTime = false;

    public static StatTracker Instance {get; private set;}

    void OnEnable()
    {
        GameManager.OnGameStart += ResetKillCounter;
        GameManager.OnGameStart += StartTrackingTime;
        GameManager.OnGameOver += StopTrackingTime;
        _enemyDeathEventChannel.OnEventRaised += IncrementKillCount;
    }

    void OnDisable()
    {
        GameManager.OnGameStart -= ResetKillCounter;
        GameManager.OnGameStart -= StartTrackingTime;
        GameManager.OnGameOver -= StopTrackingTime;
        _enemyDeathEventChannel.OnEventRaised -= IncrementKillCount;
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

    void Update()
    {
        if (!_trackTime) return;
        if (Time.timeScale == 0f) return;

        IncrementTime();
    }

    // -------------------------
    // HELPERS
    // -------------------------

    void StartTrackingTime()
    {
        _elapsedTime = 0f;
        _trackTime = true;
    }

    void StopTrackingTime()
    {
        _trackTime = false;
    }
    void IncrementTime() => _elapsedTime += Time.deltaTime;
    void ResetKillCounter() => _killCount = 0;
    void IncrementKillCount()
    {
        _killCount += 1;
    } 


}

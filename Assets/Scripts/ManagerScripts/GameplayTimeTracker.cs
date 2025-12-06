using UnityEngine;

public class GameplayTimeTracker : MonoBehaviour
{
    [SerializeField] float _elapsedTime;
    public float ElapsedTime => _elapsedTime;
    bool _trackTime = false;
    public static GameplayTimeTracker Instance {get; private set;}

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void OnEnable()
    {
        GameManager.OnGameStart += StartTracking;
        GameManager.OnGameOver += StopTracking;
    }

    void OnDisable()
    {
        GameManager.OnGameStart -= StartTracking;
        GameManager.OnGameOver -= StopTracking;
    }

    void Update()
    {
        if (!_trackTime) return;
        if (Time.timeScale == 0f) return;

        _elapsedTime += Time.deltaTime;
    }

    void StartTracking()
    {
        _elapsedTime = 0f;
        _trackTime = true;
    }

    void StopTracking()
    {
        _trackTime = false;
    }
}


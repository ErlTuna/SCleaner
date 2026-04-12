using UnityEditor;
using UnityEngine;

public class CogwheelMagnetism : MonoBehaviour
{
    Transform _player;
    [SerializeField] float magnetSpeed = 5;
    [SerializeField] float _speedIncreaseAmount;
    [SerializeField] float _targetSpeed;
    [SerializeField] float _speedIncreaseTimeThreshold;
    float _lastSpeedIncreaseTime = 0;
    float _timeSinceLastSpeedIncrease;
    [SerializeField] float _currentSpeed;

    void Start()
    {
        _currentSpeed = magnetSpeed;
        _targetSpeed = magnetSpeed;
    }

    void Awake()
    {
        _timeSinceLastSpeedIncrease = Time.time;
        //StartCoroutine(MoveTowardsPlayer());
    }
    public void AssignPlayerTransform(Transform player)
    {
        _player = player;
    }

    void Update()
    {
        if (_player == null) return;
        // periodically increase target speed
    if (Time.time - _lastSpeedIncreaseTime > _speedIncreaseTimeThreshold)
    {
        _targetSpeed += _speedIncreaseAmount;
        _lastSpeedIncreaseTime = Time.time;
    }

        // smooth current speed toward target speed
        _currentSpeed = Mathf.Lerp(_currentSpeed, _targetSpeed, Time.deltaTime * 3f); // tweak 3f for smoothness
        

        Vector3 dir = _player.position - transform.position;        
        transform.position += _currentSpeed * Time.deltaTime * dir.normalized;
    }
}

    


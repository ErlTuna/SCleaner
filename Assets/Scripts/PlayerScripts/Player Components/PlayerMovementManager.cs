using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementManager : MonoBehaviour, IPickupHandler
{
    [SerializeField] UnitMovementConfigSO _playerMovementConfig;
    UnitStateData _playerStateData;
    [SerializeField] UnitMovementData _playerMovementData;
    [SerializeField] Rigidbody2D _rb2D;
    [SerializeField] GameObject _dustParticlePrefab;
    Vector2 _moveDirection;
    
    [SerializeField] float _dustParticleSpawnInterval = 0f;
    [SerializeField] float _timeSinceLastDustParticle = 0f;


    public void InitializeManager(UnitMovementData playerMovementData, UnitMovementConfigSO playerMovementConfig)
    {
        _playerMovementData = playerMovementData;
        _playerMovementConfig = playerMovementConfig;

    }
    public void InitializeStateData(UnitStateData playerStateData)
    {
        _playerStateData = playerStateData;
    }

    void Update()
    {
        _moveDirection = PlayerInputManager.Instance.MovementInput;
        if (_moveDirection.magnitude > .75f && Time.time - _timeSinceLastDustParticle > _dustParticleSpawnInterval)
        {
            Vector3 spawnPos = transform.position - (Vector3)( .1f * _moveDirection.normalized);
            Instantiate(_dustParticlePrefab, spawnPos, Quaternion.identity);
            _timeSinceLastDustParticle = Time.time;
        }

    }

    void FixedUpdate()
    {
        
        if (_playerStateData.CanMove == false) return;
        if (_rb2D == null) return;
            
        _rb2D.velocity = _moveDirection * _playerMovementData.CurrentMovementSped.Value;
    }

    public void StopMovement()
    {
        _rb2D.velocity = Vector2.zero;
    }

    public void AddMovementSpeedModifier(FloatModifier mod)
    {
        Debug.Log("Previous speed : " + _playerMovementData.CurrentMovementSped.Value);
        _playerMovementData.CurrentMovementSped.AddModifier(mod);
        Debug.Log("Current speed : " + _playerMovementData.CurrentMovementSped.Value);
    }

}


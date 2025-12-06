using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementManager : MonoBehaviour
{
    [SerializeField] UnitMovementConfigSO _playerMovementConfig;
    UnitStateData _playerStateData;
    [SerializeField] UnitMovementData _playerMovementData;
    [SerializeField] Rigidbody2D _rb2D;
    Vector2 _moveDirection;


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

    }

    void FixedUpdate()
    {
        
        if (_playerStateData.CanMove == false) return;
        if (_rb2D == null) return;
            
        _rb2D.velocity = _moveDirection * _playerMovementData.CurrentMovementSpeed;
    }

    public void StopMovement()
    {
        _rb2D.velocity = Vector2.zero;
    }


}


using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMain : Unit
{
    [Header("Input Action Map")]
    [SerializeField] InputActionMap _inputActionAsset;

    [Header("Managers")]
    [SerializeField] PlayerInputManager _playerInputManager;
    [SerializeField] PlayerHealthManager _playerHealthManager;
    [SerializeField] PlayerMovementManager _playerMovementManager;
    [SerializeField] PlayerInventoryManager _playerInventoryManager;
    [SerializeField] PlayerEnergyManager _playerEnergyManager;
    [SerializeField] PlayerAbilityHandler _playerAbilityHandler;
    [SerializeField] PlayerAimManager _playerAimManager;
    [SerializeField] PlayerHitbox _playerHitboxManager;

    [Header("Configs")]
    [SerializeField] UnitHealthConfigSO _healthConfig;
    [SerializeField] UnitMovementConfigSO _movementConfig;
    [SerializeField] UnitEnergyConfigSO _energyConfig;
    [SerializeField] UnitInventoryConfigSO _inventoryConfig;
    [SerializeField] UnitStateConfigSO _stateConfig;

    [Header("Event Channels")]
    [SerializeField] TransformEventChannelSO _transformEventChannel;



    [Header("Datas")]
    UnitHealthData _healthData;
    UnitMovementData _movementData;
    UnitEnergyData _energyData;
    UnitInventoryData _inventoryData;
    [SerializeField] UnitStateData _stateData;

    void OnEnable()
    {
        GameManager.OnGameOver += OnPlayerDefeat;
    }

    void OnDisable()
    {
        GameManager.OnGameOver -= OnPlayerDefeat;
    }




    void Awake()
    {
        PrepareRuntimeData();
        InitializeManagers();
    }

    void Start()
    {
        if (_transformEventChannel != null)
            _transformEventChannel.RaiseEvent(gameObject.transform);
    }

    void PrepareRuntimeData()
    {
        _healthData = new UnitHealthData(_healthConfig);
        _movementData = new UnitMovementData(_movementConfig);
        _energyData = new UnitEnergyData(_energyConfig);
        _inventoryData = new UnitInventoryData(_inventoryConfig);
        _stateData = new UnitStateData(_stateConfig);
    }
    
    void InitializeManagers()
    {
        _playerHealthManager.InitializeManager(_healthData, _healthConfig);
        _playerHealthManager.InitializeStateData(_stateData);


        _playerMovementManager.InitializeManager(_movementData, _movementConfig);
        _playerMovementManager.InitializeStateData(_stateData);

        _playerEnergyManager.InitializeManager(_energyData, _energyConfig);
        _playerInventoryManager.InitializeManager(_inventoryData, _inventoryConfig);

        _playerAbilityHandler.InitializeManager(_energyData);
        _playerAbilityHandler.InitializeStateData(_stateData);

        _playerHitboxManager.InitializeStateData(_stateData);   
    }

    public override UnitStateData GetStateData()
    {
        return _stateData;
    }

    public void OnPlayerDefeat()
    {
        _stateData.IsAlive = false;
        _stateData.CanMove = false;
        _playerMovementManager.StopMovement();
    }
    
}

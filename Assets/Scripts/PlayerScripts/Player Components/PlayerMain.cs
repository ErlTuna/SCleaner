using System;
using System.Collections.Generic;
using SerializeReferenceEditor;
using Unity.VisualScripting;
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
    [SerializeField] PlayerWeaponManager _playerWeaponManager;
    [SerializeField] PlayerEnergyManager _playerEnergyManager;
    [SerializeField] PlayerAbilityHandler _playerAbilityHandler;
    [SerializeField] PlayerAimManager _playerAimManager;
    [SerializeField] PlayerHitbox _playerHitboxManager;

    [Header("Configs")]
    [SerializeField] UnitHealthConfigSO _healthConfig;
    [SerializeField] UnitMovementConfigSO _movementConfig;
    [SerializeField] PlayerEnergyConfigSO _energyConfig;
    [SerializeField] PlayerInventoryConfigSO _inventoryConfig;
    [SerializeField] UnitStateConfigSO _stateConfig;
    [SerializeField] WeaponDatabaseSO _weaponDatabaseSO;

    [Header("Event Channels")]
    [SerializeField] TransformEventChannelSO _transformEventChannel;

    [Header("Datas")]
    UnitHealthData _healthData;
    UnitMovementData _movementData;
    PlayerEnergyData _energyData;
    UnitInventoryData _inventoryData;

    UnitInventoryData_V2 _inventoryData_v2;
    [SerializeField] UnitStateData _stateData;

    [Header("Runtime")]
    PlayerWeaponInventoryRuntime _weaponInventoryRuntime;


    public EffectContainer EffectContainer {get; } = new();
    Dictionary<Type, IManagerComponent> _managerLookUp = new();
    Dictionary<Type, IPickupHandler> _pickupHandlerLookup = new();

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


        PrepareData();
        PrepareRuntime();

        InitializeManagers();
        RegisterManager(_playerHealthManager);
        
        
        RegisterHandler(_playerHealthManager);
        RegisterHandler(_playerInventoryManager);
        RegisterHandler(_playerWeaponManager);
        RegisterHandler(_playerMovementManager);
    }

    void RegisterManager<T>(T manager) where T : IManagerComponent
    {
        Type type = manager.GetType();

        // Register by concrete type
        _managerLookUp[type] = manager;

        // Regsiter by all interfaces
        foreach (var iface in type.GetInterfaces())
        {
            _managerLookUp[iface] = manager;
        }
    }

    void RegisterHandler(IPickupHandler handler)
    {
        var type = handler.GetType();

        _pickupHandlerLookup[type] = handler;

        foreach (var iface in type.GetInterfaces())
        {
            if (typeof(IPickupHandler).IsAssignableFrom(iface))
            {
                _pickupHandlerLookup[iface] = handler;
            }
        }
    }

    public bool TryGetManager<T>(out T manager) where T : class, IPickupHandler
    {
        if (_pickupHandlerLookup.TryGetValue(typeof(T), out IPickupHandler foundManager))
        {
            manager = foundManager as T;
            if (manager != null) return true;
        }

        manager = null;
        Debug.LogWarning($"Manager of type {typeof(T)} could not be found or cast failed!");
        return false;
    }

    public IReadOnlyDictionary<Type, IPickupHandler> GetPickupHandlers()
    {   
        return _pickupHandlerLookup;
    }

    public IReadOnlyDictionary<Type, IManagerComponent> GetManagers()
    {
       return _managerLookUp;
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
        _energyData = new PlayerEnergyData(_energyConfig);
        _inventoryData = new UnitInventoryData(_inventoryConfig);

        


        _stateData = new UnitStateData(_stateConfig);
    }

    void PrepareData()
    {
        _inventoryData_v2 = UnitInventoryData_V2.CreateNew(_inventoryConfig);
    }

    void PrepareRuntime()
    {
        WeaponInventoryDependencies weaponInventoryDependencies = new(_weaponDatabaseSO, _inventoryConfig.WeaponAddedEventChannel, _inventoryConfig.WeaponDropEventChannel);
        _weaponInventoryRuntime = new(_inventoryData_v2.WeaponInventory, weaponInventoryDependencies);
    }
    
    void InitializeManagers()
    {
        _playerHealthManager.InitializeManager(_healthData, _healthConfig);
        _playerHealthManager.InitializeStateData(_stateData);


        _playerMovementManager.InitializeManager(_movementData, _movementConfig);
        _playerMovementManager.InitializeStateData(_stateData);

        _playerEnergyManager.InitializeManager(_energyData, _energyConfig);
        //_playerInventoryManager.InitializeManager(_inventoryData, _inventoryConfig);

        _playerInventoryManager.InitializeManager_V2(_weaponInventoryRuntime, _inventoryConfig);

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
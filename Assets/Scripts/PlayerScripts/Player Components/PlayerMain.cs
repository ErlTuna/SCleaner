using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMain : Unit
{
    [SerializeField] InputActionMap _playerActionMap;
    [SerializeField] PlayerHealthManager _playerHealthManager;
    [SerializeField] PlayerMovementManager _playerMovementManager;
    [SerializeField] PlayerInventoryManager _playerInventoryManager;
    [SerializeField] PlayerEnergyManager _playerEnergyManager;
    [SerializeField] PlayerAbilityHandler _playerAbilityHandler;
    [SerializeField] PlayerAimManager _playerAimManager;

    void OnEnable()
    {
        //PlayerHealth.onPlayerDeath += DisableInput;
    }

    void OnDisable()
    {
        //PlayerHealth.onPlayerDeath -= DisableInput;
    }


    void Awake()
    {
        if (UnitConfigWrapper == null)
        {
            Debug.Log("Player has no config wrapper.");
            return;
        }

        RuntimeDataHolder = new UnitRuntimeDataHolder();

        // Storing uninitialized data
        RuntimeDataHolder.AddRuntimeData(new UnitHealthData());
        RuntimeDataHolder.AddRuntimeData(new UnitMovementData());
        RuntimeDataHolder.AddRuntimeData(new UnitInventoryData());
        RuntimeDataHolder.AddRuntimeData(new UnitEnergyData());
        RuntimeDataHolder.AddRuntimeData(new UnitStateData());

        // Initializing stored data
        RuntimeDataHolder?.InitializeWithWrapper(this, UnitConfigWrapper);

        // Initialized datas to be distributed to managers
        UnitHealthData playerHealthData = RuntimeDataHolder.GetRuntimeData<UnitHealthData>();
        UnitMovementData playerMovementData = RuntimeDataHolder.GetRuntimeData<UnitMovementData>();
        UnitEnergyData playerEnergyData = RuntimeDataHolder.GetRuntimeData<UnitEnergyData>();
        UnitInventoryData playerInventoryData = RuntimeDataHolder.GetRuntimeData<UnitInventoryData>();
        UnitStateData playerStateData = RuntimeDataHolder.GetRuntimeData<UnitStateData>();

        // Distribute runtime datas to managers
        _playerHealthManager.InitializeWithData(this, playerHealthData);
        _playerMovementManager.InitializeWithData(this, playerMovementData);
        _playerEnergyManager.InitializeWithData(this, playerEnergyData);
        _playerAbilityHandler.InitializeWithData(this, playerEnergyData);
        _playerInventoryManager.InitializeWithData(this, playerInventoryData);

    }

    
    /*void DisableInput(){
        playerAim.enabled = false;
        playerActionMap.Disable();
    }

    void EnableInput(){
        playerAim.enabled = true;
        playerActionMap.Enable();
    }*/
    
}

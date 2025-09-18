using SerializeReferenceEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMain : Unit
{
    [SerializeField] InputActionMap _playerActionMap;
    [SerializeField] PlayerHealthManager _playerHealthManager;
    [SerializeField] PlayerMovementManager _playerMovementManager;
    [SerializeField] PlayerInventoryManager_v2 _playerInventoryManager;
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
        if (unitConfigWrapper == null)
        {
            Debug.Log("Player info is null, creating");
            //playerConfig = Resources.Load<UnitConfigSO>("Scriptable Objects/PlayerData");
        }

        RuntimeDataHolder = new PlayerRuntimeDataHolder();
        RuntimeDataHolder?.InitializeWithWrapper(this, unitConfigWrapper);

        PlayerHealthData playerHealthData = RuntimeDataHolder.GetRuntimeData<PlayerHealthData>();
        PlayerMovementData playerMovementData = RuntimeDataHolder.GetRuntimeData<PlayerMovementData>();
        PlayerInventoryData playerInventoryData = RuntimeDataHolder.GetRuntimeData<PlayerInventoryData>();
        PlayerEnergyData playerEnergyData = RuntimeDataHolder.GetRuntimeData<PlayerEnergyData>();

        _playerHealthManager.InitializeWithData(this, playerHealthData);
        _playerMovementManager.InitializeWithData(this, playerMovementData);
        _playerInventoryManager.InitializeWithData(this, playerInventoryData);
        _playerAbilityHandler.InitializeWithData(this, playerEnergyData);
        _playerEnergyManager.InitializeWithData(this, playerEnergyData);



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

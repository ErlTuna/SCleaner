using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMain : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] InputActionAsset playerActionAsset;
    [SerializeField] InputActionMap playerActionMap;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerDash playerDash;
    [SerializeField] PlayerAim playerAim;


    public UnitInfoSO playerInfo;
    public WeaponManager weaponManager;


    void OnEnable(){
        PlayerHealth.onPlayerDeath += DisableInput;
    }

    void OnDisable(){
        PlayerHealth.onPlayerDeath -= DisableInput;
    }


    void Awake(){
        if (playerInfo == null){
            Debug.Log("Player info is null, creating");
            playerInfo = Resources.Load<UnitInfoSO>("Scriptable Objects/PlayerInfo");
        }
        playerInfo.Init();
        
        playerHealth.Init(playerInfo);

        playerMovement.Init(playerInfo);

        playerDash.Init(playerInfo);


    }
    
    void DisableInput(){
        playerAim.enabled = false;
        playerActionMap.Disable();
    }

    void EnableInput(){
        playerAim.enabled = true;
        playerActionMap.Enable();
    }
       
}

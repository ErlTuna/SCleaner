using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMain : MonoBehaviour
{
    [SerializeField] InputActionMap playerActionMap;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerAim playerAim;
    public PlayerCoreSO playerInfo;


    void OnEnable(){
        PlayerHealth.onPlayerDeath += DisableInput;
    }

    void OnDisable(){
        PlayerHealth.onPlayerDeath -= DisableInput;
    }


    void Awake(){
        if (playerInfo == null){
            Debug.Log("Player info is null, creating");
            playerInfo = Resources.Load<PlayerCoreSO>("Scriptable Objects/PlayerData");
        }
        
        playerHealth.Init(playerInfo.healthData);

        playerMovement.Init(playerInfo.movementData);


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

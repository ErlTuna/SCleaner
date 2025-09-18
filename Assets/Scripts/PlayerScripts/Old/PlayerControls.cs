using UnityEngine;
using UnityEngine.InputSystem;

/*
public class PlayerControls : MonoBehaviour
{
    private Rigidbody2D _rb2D;
    private Vector2 _moveDirection;
    public UnitMovementConfigSO movementData;

    void OnEnable()
    {
        PlayerHealth.onPlayerDeath += DisableScript;
    }

    void OnDisable()
    {
        PlayerHealth.onPlayerDeath -= DisableScript;
    }


    void Start()
    {

        _rb2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _rb2D.velocity = _moveDirection * movementData.movementSpeed;
    }

    public void DisableScript()
    {
        Debug.Log("Disabled player movement input");
        enabled = false;
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        //if(!player.isAlive){
        //Debug.Log("Player is dead, can't read input");
        //return;
        //}
        _moveDirection = ctx.ReadValue<Vector2>();
    }

    public void StopMovement()
    {
        Debug.Log("called");
        _rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
    }

}
*/
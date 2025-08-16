using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour, ISetup, IDisable
{


    Rigidbody2D _rb2D;
    Vector2 _moveDirection;
    UnitInfoSO _playerInfo;
    public delegate void OnDashInput(Vector2 moveDirection);
    public static OnDashInput onDash;
    public InputActionAsset inputActions;
    InputAction _movementInputAction;
    public UnitInfoSO playerInfo;



    void OnEnable()
    {
        //PlayerHealth.onPlayerDeath += DisableScript;
    }

    void OnDisable()
    {
        //PlayerHealth.onPlayerDeath -= DisableScript;
    }


    void Start()
    {
        playerInfo.movementSpeed = playerInfo.maxMovementSpeed;
        _rb2D = GetComponent<Rigidbody2D>();
    }

    public void Init(UnitInfoSO info)
    {
        playerInfo = info;
    }

    void Update()
    {
        _moveDirection = PlayerInputManager.instance.MovementInput;

        if (PlayerInputManager.instance.DashInput)
        {
            onDash?.Invoke(_moveDirection);
        }
    }

    void FixedUpdate()
    {
        if (playerInfo.isDashing) return;

        _rb2D.velocity = _moveDirection * playerInfo.movementSpeed;
    }

    public void DisableScript()
    {
        Debug.Log("Disabled player movement input action");
        _rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
        _movementInputAction.Disable();
        enabled = false;
    }
}

/*
public void OnMovement(InputAction.CallbackContext ctx){
        if (ctx.performed)
        _moveDirection = ctx.ReadValue<Vector2>();
        if (ctx.canceled)
        _moveDirection = Vector2.zero;
    }

    /*public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && _moveDirection.magnitude > 0)
        {
            onDash?.Invoke(_moveDirection);
        }
    }

    /*public void ReceiveInputAction(InputAction action){
        _movementInputAction = action;
    }
*/

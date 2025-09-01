using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour, IDisable
{
    Rigidbody2D _rb2D;
    Vector2 _moveDirection;
    public delegate void OnDashInput(Vector2 moveDirection);
    public static OnDashInput OnDash;
    InputAction _movementInputAction;
    public UnitStateSO playerState;
    public MovementSO movementData;

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
        movementData.movementSpeed = movementData.maxMovementSpeed;
        _rb2D = GetComponent<Rigidbody2D>();
    }

    public void Init(MovementSO movementData)
    {
        //playerInfo = info;
    }

    void Update()
    {
        _moveDirection = PlayerInputManager.instance.MovementInput;

    }

    void FixedUpdate()
    {
        if (playerState.canMove == false) return;

        _rb2D.velocity = _moveDirection * movementData.movementSpeed;
    }

    public void DisableScript()
    {
        Debug.Log("Disabled player movement input action");
        _rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
        _movementInputAction.Disable();
        enabled = false;
    }
}


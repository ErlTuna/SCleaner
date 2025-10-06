using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementManager : MonoBehaviour
{
    Unit _owner;
    UnitMovementData _movementData;
    Rigidbody2D _rb2D;
    Vector2 _moveDirection;



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
        _rb2D = GetComponent<Rigidbody2D>();
    }

    public void InitializeWithData(Unit owner, UnitMovementData movementData)
    {
        _owner = owner;
        _movementData = movementData;
    }

    void Update()
    {
        _moveDirection = PlayerInputManager.instance.MovementInput;

    }

    void FixedUpdate()
    {
        if (_owner.RuntimeDataHolder.TryGetRuntimeData(out UnitStateData stateData))
            if (stateData.CanMove == false)
            {
                //Debug.Log("Can't move!");
                return;
            }

        _rb2D.velocity = _moveDirection * _movementData.CurrentMovementSpeed;
    }

    public void DisableScript()
    {
        Debug.Log("Disabled player movement input action");
        _rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
        //_movementInputAction.Disable();
        enabled = false;
    }

}


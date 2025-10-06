using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementManager : MonoBehaviour
{
    Unit _owner;
    UnitMovementData _movementData;
    Rigidbody2D _rb2D;
    Vector2 _moveDirection;


    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }

    public void InitializeWithData(Unit owner, UnitMovementData movementData)
    {
        _owner = owner;
        _movementData = movementData;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMover : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb2D;
    Vector2 _direction;
    float _speed;

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        _rb2D.velocity = _direction * _speed;
    }
    
    public void InitializeMover(Vector2 direction, float speed)
    {
        _speed = speed;
        _direction = direction;
    }
}

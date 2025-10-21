using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandsManager : MonoBehaviour
{
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform weapon;
    Transform _target;
    Transform _currentHand;

    void Start()
    {
        //_currentHand = weapon.parent;
    }

    void Update()
    {
        if (_target == null || weapon == null) return;

        HandleHandSwitch();
        RotateTowardsTarget();
    }
    
    public void SetWeapon(Transform weapon)
    {
        this.weapon = weapon;
        _currentHand = rightHand;
        weapon.SetParent(_currentHand);
        weapon.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    void HandleHandSwitch()
    {
        bool targetIsToTheRight = _target.position.x > transform.position.x;
        Transform desiredHand = targetIsToTheRight ? rightHand : leftHand;

        if (_currentHand != desiredHand)
        {
            _currentHand = desiredHand;
            weapon.SetParent(_currentHand);
            weapon.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            
        }
    }

    void RotateTowardsTarget()
    {
        Vector2 direction = (_target.position - weapon.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weapon.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }   
}

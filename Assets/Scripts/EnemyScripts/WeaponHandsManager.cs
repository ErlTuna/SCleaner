using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WeaponHandsManager : MonoBehaviour
{
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform weaponRoot;
    Transform stockGripPoint;
    Transform secondGripPoint;
    Transform _target;
    Transform _currentHand;
    bool _isWeaponTwoHanded;

    void Update()
    {
        if (_target == null || weaponRoot == null) return;

        if (_isWeaponTwoHanded == false)
            HandleHandSwitchForSingleGrip();

        else
            UpdateHandPositions();

        RotateTowardsTarget();
    }

    public void SetWeapon(Transform weapon)
    {
        weaponRoot = weapon;
        weapon.SetParent(transform);
        weapon.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

    }

    public void SetGripPoints(Transform stockGripPoint, Transform secondGripPoint = null)
    {
        if (stockGripPoint == null)
        {
            Debug.Log("There needs to be at least a stock grip point.");
            return;
        }

        this.stockGripPoint = stockGripPoint;
        this.secondGripPoint = secondGripPoint;

        // assuming right hand is holding the stock by default
        rightHand.gameObject.SetActive(stockGripPoint != null);
        leftHand.gameObject.SetActive(secondGripPoint != null);

        // weapon is held by one hand
        if (stockGripPoint != null && secondGripPoint == null && weaponRoot != null)
        {
            _isWeaponTwoHanded = false;
            _currentHand = rightHand;
            weaponRoot.SetParent(_currentHand);
            weaponRoot.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
        else
        {
            _isWeaponTwoHanded = true;
            UpdateHandPositions();
        }

        Debug.Log("Set grip points. Is the weapon two handed? " + _isWeaponTwoHanded);
    }

    void HandleHandSwitchForSingleGrip()
    {
        bool targetIsToTheRight = _target.position.x > transform.position.x;
        Transform desiredHand = targetIsToTheRight ? rightHand : leftHand;

        if (desiredHand != null && _currentHand != null && desiredHand != _currentHand)
        {
            if (desiredHand.gameObject.activeSelf != true)
                desiredHand.gameObject.SetActive(true);

            weaponRoot.SetParent(desiredHand);
            weaponRoot.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            _currentHand.gameObject.SetActive(false);
            _currentHand = desiredHand;
        }
    }

    void UpdateHandPositions()
    {
        if (stockGripPoint != null)
        {
            leftHand.SetPositionAndRotation(stockGripPoint.position, stockGripPoint.rotation);
        }

        if (secondGripPoint != null)
        {
            rightHand.SetPositionAndRotation(secondGripPoint.position, secondGripPoint.rotation);
        }
    }



    void RotateTowardsTarget()
    {
        Vector2 direction = (_target.position - weaponRoot.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weaponRoot.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }   
}

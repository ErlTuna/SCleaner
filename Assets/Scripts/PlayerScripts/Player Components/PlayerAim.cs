using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimManager : MonoBehaviour
{
    Vector3 _mousePos;
    Camera _cam;

    void Start()
    {
        _cam = Camera.main;
    }

    void Update()
    {
        Aiming();
    }

    void Aiming()
    {
        _mousePos = _cam.ScreenToWorldPoint(PlayerInputManager.instance.PointerInput);
        _mousePos.z = 0f;
        Vector3 aimDirection = (_mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    Vector3 _mousePos;
    Camera _cam;

    void OnEnable(){
        //PlayerHealth.onPlayerDeath += Disable;
    }
    void OnDisable(){
        //PlayerHealth.onPlayerDeath -= Disable;
    }

    void Start()
    {
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Aiming();
    }

    /*void Aim(){
        _mousePos = _cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 aimDirection = (_mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }*/

    void Aiming()
    {
        _mousePos = _cam.ScreenToWorldPoint(PlayerInputManager.instance.PointerInput);
        Vector3 aimDirection = (_mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }


    void Disable(){
        gameObject.SetActive(false);
    }
}

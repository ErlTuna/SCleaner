using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetFollow : MonoBehaviour
{
    public Transform player;
    public float deadZoneRadius = 1f;
    public float maxOffsetDistance = 5f;
    public float followSpeed = 10f;
    Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        if (player == null) return;

        
        Vector3 mousePos = mainCam.ScreenToWorldPoint(PlayerInputManager.instance.PointerInput);
        mousePos.z = 0f;

        Vector3 offset = mousePos - player.position;
        float distance = offset.magnitude;
        if (distance < deadZoneRadius)
        {
            offset = Vector3.zero;
        }
        else
        {
            
            offset -= offset.normalized * deadZoneRadius;

            
            offset = Vector3.ClampMagnitude(offset, maxOffsetDistance);
        }

        Vector3 targetPos = player.position + offset;
        targetPos.z = transform.position.z;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
    }
}

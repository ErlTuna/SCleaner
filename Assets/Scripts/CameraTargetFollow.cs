using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetFollow : MonoBehaviour
{
    public static Action OnCameraMovedToPlayer;
    [SerializeField] TransformEventChannelSO _transformEventChannelSO;
    [SerializeField] Transform _playerTransform;
    [SerializeField] Camera _mainCam;

    public float deadZoneRadius = 1f;
    public float maxOffsetDistance = 5f;
    public float followSpeed = 10f;
    Vector3 _currentOffset;
    Transform _overrideTarget = null;
    bool _isFollowingOverride = false;
    [SerializeField] bool _followMouse = true;
    public float smoothTime = 0.1f;   

    private Vector3 _offsetVelocity = Vector3.zero;

    void OnEnable()
    {
        _transformEventChannelSO.OnEventRaised += SetPlayerTransform;
        GameManager.OnGameOverCameraMovement += StartDefeatCameraMove;
        GameManager.OnLevelLoaded += OnLevelLoaded;
    }

    void OnDisable()
    {
        _transformEventChannelSO.OnEventRaised -= SetPlayerTransform;
        GameManager.OnGameOverCameraMovement -= StartDefeatCameraMove;
        GameManager.OnLevelLoaded -= OnLevelLoaded;
    }


    void Start()
    {
        if (_playerTransform != null)
        {
            Debug.Log("camera moved to player");
            
        }
        _followMouse = true;
    }

    
    void LateUpdate()
    {
        if (_playerTransform == null) return;
        if (_isFollowingOverride) return;

        FollowPlayerMouseOffset();
    }
    

    // Optional: pixels per unit for pixel-perfect rounding
    public float pixelsPerUnit = 32f; 

    /*
    void LateUpdate()
    {
        if (_playerTransform == null || !_followMouse) return;

        // 1. Calculate mouse-based offset
        Vector3 mouseWorldPos = _mainCam.ScreenToWorldPoint(PlayerInputManager.Instance.PointerInput);
        mouseWorldPos.z = 0f;

        Vector3 rawOffset = mouseWorldPos - _playerTransform.position;
        float distance = rawOffset.magnitude;

        Vector3 desiredOffset;
        if (distance < deadZoneRadius)
            desiredOffset = Vector3.zero;
        else
            desiredOffset = Vector3.ClampMagnitude(rawOffset - rawOffset.normalized * deadZoneRadius, maxOffsetDistance);

        // 2. Smooth the offset
        _currentOffset = Vector3.SmoothDamp(_currentOffset, desiredOffset, ref _offsetVelocity, smoothTime);

        // 3. Calculate target camera position
        Vector3 targetPos = _playerTransform.position + _currentOffset;

        // 4. Optional: pixel-perfect rounding
        //float unitsPerPixel = 1f / pixelsPerUnit;
        //targetPos.x = Mathf.Round(targetPos.x / unitsPerPixel) * unitsPerPixel;
        //targetPos.y = Mathf.Round(targetPos.y / unitsPerPixel) * unitsPerPixel;
        //targetPos.z = transform.position.z;

        // 5. Apply camera position
        targetPos.z = transform.position.z;
        transform.position = targetPos;
    }
    */


    void SetPlayerTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
        _mainCam.transform.position = _playerTransform.position;
    }


    void FollowPlayerMouseOffset()
    {
        if (!_followMouse) return;

        // Getting mouse world position
        Vector3 mousePos = _mainCam.ScreenToWorldPoint(PlayerInputManager.Instance.PointerInput);
        mousePos.z = 0f;

        // Calculating raw offset from player to mouse
        Vector3 rawOffset = mousePos - _playerTransform.position;
        float distance = rawOffset.magnitude;

        // Applying deadzone + clamp
        Vector3 desiredOffset;

        // No offset if the pointer is within deadzone. (a circle around the player)
        if (distance < deadZoneRadius)
        {
            desiredOffset = Vector3.zero;
        }
        else
        {
            Vector3 adjusted = rawOffset - rawOffset.normalized * deadZoneRadius;
            desiredOffset = Vector3.ClampMagnitude(adjusted, maxOffsetDistance);
        }

        // moothing only the offset

        // Simple lerp
        _currentOffset = Vector3.Lerp(
            _currentOffset,
            desiredOffset,
            Time.deltaTime * followSpeed
        );

        // Apply to player position 
        Vector3 targetPos = _playerTransform.position + _currentOffset;
        targetPos.z = transform.position.z;

        transform.position = targetPos;
    }  
    void FollowOverrideTarget()
    {
        if (_overrideTarget == null)
            return;

        Vector3 pos = _overrideTarget.position;
        pos.z = transform.position.z;

        transform.position = Vector3.Lerp(
            transform.position,
            pos,
            Time.deltaTime * followSpeed
        );
    }

    void OnLevelLoaded()
    {
        _followMouse = true;
        if (_playerTransform != null)
            transform.position = new Vector3(
            _playerTransform.position.x,
            _playerTransform.position.y,
            transform.position.z
            );

    }

    void OverrideTarget(Transform newTarget)
    {
        _overrideTarget = newTarget;
        _isFollowingOverride = true;       
    }

    void ClearOverride()
    {
        _overrideTarget = null;
        _isFollowingOverride = false;
    }

    void StartDefeatCameraMove()
    {
        StartCoroutine(LerpToPlayer(1f));
    }

    IEnumerator LerpToPlayer(float duration)
    {
        _isFollowingOverride = true;
        _followMouse = false;
        Vector3 start = transform.position;
        Vector3 end = new(_playerTransform.position.x, _playerTransform.position.y, transform.position.z);

        float t = 0f;
        while (t < duration)
        {
            transform.position = Vector3.Lerp(start, end, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        OnCameraMovedToPlayer?.Invoke();
        _isFollowingOverride = false;
    }

}

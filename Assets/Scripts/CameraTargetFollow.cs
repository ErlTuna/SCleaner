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
    Transform _overrideTarget = null;
    bool _isFollowingOverride = false;
    [SerializeField] bool _followMouse = true;

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

    void Awake()
    {
        _mainCam = Camera.main;
    }

    void Start()
    {
        if (_playerTransform != null)
        {
            Debug.Log("camera moved to player");
            _mainCam.transform.position = _playerTransform.position;
        }
        _followMouse = true;
    }

    void LateUpdate()
    {
        if (_playerTransform == null) return;
        if (_isFollowingOverride) return;

        FollowPlayerMouseOffset();
        
        
        /*
        Vector3 mousePos = _mainCam.ScreenToWorldPoint(PlayerInputManager.Instance.PointerInput);
        mousePos.z = 0f;

        Vector3 offset = mousePos - _playerTransform.position;
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

        Vector3 targetPos = _playerTransform.position + offset;
        targetPos.z = transform.position.z;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
        */
        //transform.position = targetPos;
    }

    void SetPlayerTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
        
    }

    void FollowPlayerMouseOffset()
    {
        if (_followMouse == false) return;


        Vector3 mousePos = _mainCam.ScreenToWorldPoint(PlayerInputManager.Instance.PointerInput);
        mousePos.z = 0f;
        Vector3 offset = mousePos - _playerTransform.position;

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

        Vector3 targetPos = _playerTransform.position + offset;
        targetPos.z = transform.position.z;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            Time.deltaTime * followSpeed
        );
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

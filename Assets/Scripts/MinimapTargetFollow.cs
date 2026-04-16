using UnityEngine;

public class MinimapTargetFollow : MonoBehaviour
{
    public Transform _playerTransform;       // Assign your player here
    public Vector3 offset = new(0, 0, -10f); // How high above the player
    [SerializeField] TransformEventChannelSO _transformEventChannelSO;
    void OnEnable()
    {
        _transformEventChannelSO.OnEventRaised += SetPlayerTransform;
    }

    void OnDisable()
    {
        _transformEventChannelSO.OnEventRaised -= SetPlayerTransform;
    }

    void LateUpdate()
    {
        if (_playerTransform)
        {
            Vector3 newPosition = _playerTransform.position + offset;
            transform.SetPositionAndRotation(newPosition, Quaternion.Euler(0f, 0f, 0f));
        }        
    }

    void SetPlayerTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
        
    }
}

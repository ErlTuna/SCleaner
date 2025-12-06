using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Flipper : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Transform _firingPoint;
    float _rotationZ;
    Transform _parentTransform;
    [SerializeField] Transform _visuals;
    [SerializeField] Transform _wrapper;
    [SerializeField] Transform _gripPointsWrapper;
    [SerializeField] Transform _secondGripPoint;
    Vector3 _originalParentLocalPos;
    Vector3 _originalWrapperLocalPos;
    Vector3 _originalFiringPointLocalPos;
    Vector3 _originalGripWrapperLocalPos;
    Vector3 _originalSecondGripPointLocalPos;
    

    void Start()
    {
        _parentTransform = transform.parent;
        if (_firingPoint)
            _originalFiringPointLocalPos = _firingPoint.localPosition;

        if (_wrapper)
            _originalWrapperLocalPos = _wrapper.localPosition;

        if (_parentTransform)
            _originalParentLocalPos = _parentTransform.localPosition;

        /*
        if (_gripPointsWrapper)
            _originalGripWrapperLocalPos = _gripPointsWrapper.localPosition;
        */
        if (_secondGripPoint)
            _originalSecondGripPointLocalPos = _secondGripPoint.localPosition;
    }

    void Update()
    {
        if (_parentTransform == null)
        {
            GetParent();
            return;
        }

        if (_firingPoint == null)
            return;

        _rotationZ = _parentTransform.rotation.eulerAngles.z % 360f;
        if (_rotationZ < 0) _rotationZ += 360f;

        //spriteRenderer.flipY = _rotationZ > 90f && _rotationZ < 270f;
        bool isFlipped = _rotationZ > 90f && _rotationZ < 270f;
        spriteRenderer.flipY = isFlipped;

        if (_parentTransform != null)
            _parentTransform.localPosition = isFlipped
                ? new Vector3(_originalParentLocalPos.x, -_originalParentLocalPos.y, _originalParentLocalPos.z)
                : _originalParentLocalPos;

        if (_wrapper != null)
            _wrapper.localPosition = isFlipped
                ? new Vector3(_originalWrapperLocalPos.x, -_originalWrapperLocalPos.y, _originalWrapperLocalPos.z)
                : _originalWrapperLocalPos;


        if (_firingPoint != null)
            _firingPoint.localPosition = isFlipped
                ? new Vector3(_originalFiringPointLocalPos.x, -_originalFiringPointLocalPos.y, _originalFiringPointLocalPos.z)
                : _originalFiringPointLocalPos;
        
        /*
        if (_gripPointsWrapper != null)
        {
            _gripPointsWrapper.localPosition = isFlipped
            ? new Vector3(_originalGripWrapperLocalPos.x, -_originalGripWrapperLocalPos.y, _originalGripWrapperLocalPos.z)
            : _originalGripWrapperLocalPos;
        }
        */

        if (_secondGripPoint != null)
        {
            _secondGripPoint.localPosition = isFlipped
            ? new Vector3(_originalSecondGripPointLocalPos.x, -_originalSecondGripPointLocalPos.y, _originalSecondGripPointLocalPos.z)
            : _originalSecondGripPointLocalPos;
        }


    }
    
    void GetParent()
    {
        _parentTransform = transform.parent;
        if (_parentTransform)
            _originalParentLocalPos = _parentTransform.position;
    }
}


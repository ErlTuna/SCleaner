using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Flipper : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    float rotationZ;
    Transform _parentTransform;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _parentTransform = transform.parent;
    }

    void Update()
    {
        if (transform.parent == null) return;
        
        float rotationZ = transform.parent.rotation.eulerAngles.z % 360f;
        if (rotationZ < 0) rotationZ += 360f;
        spriteRenderer.flipY = rotationZ > 90f && rotationZ < 270f;
    }
}


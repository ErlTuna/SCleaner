using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Flipper : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    float rotationZ;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (transform.parent == null) return;
        
        rotationZ = transform.parent.rotation.eulerAngles.z;
        spriteRenderer.flipY = rotationZ > 90f && rotationZ < 270f;
    }
}


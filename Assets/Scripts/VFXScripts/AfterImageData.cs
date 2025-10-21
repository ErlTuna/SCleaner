using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/VFX/AfterImageData")]
public class AfterImageData : ScriptableObject
{
    public Sprite AfterImageSprite;
    public Gradient TintGradient;
    public Color Color = Color.white;
    public Material Material;
    [Range(0,1)] public float StartingAlpha = 1f;
    public float SpawnRate = 0.05f;
    public float Lifetime = 0.5f;
    
}

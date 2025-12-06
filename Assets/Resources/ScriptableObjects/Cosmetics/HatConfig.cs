using UnityEngine;

[CreateAssetMenu(fileName = "Hat Config", menuName = "ScriptableObjects/Component Configs/Hat Config")]
public class HatConfig : ScriptableObject
{
    public Sprite DefaultVisual;
    public LayerMask DetachedLayer;
    public float MaxHealth;
    public HatKnockDownBehaviour KnockDownBehaviour;

}


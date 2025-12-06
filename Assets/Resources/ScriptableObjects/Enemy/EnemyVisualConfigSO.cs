using UnityEngine;

[CreateAssetMenu(fileName = "Visual Config", menuName = "ScriptableObjects/Component Configs/Enemy Visual Config")]
public class EnemyVisualConfigSO : ScriptableObject
{
    [Header("Body Visuals")]
    public Sprite DefaultBodyAppearance;
    public Sprite DefeatedBodyAppearance;    

}

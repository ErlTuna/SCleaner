using UnityEngine;

[CreateAssetMenu(fileName = "Movement Config", menuName = "ScriptableObjects/Component Configs/Movement Config")]
public class UnitMovementConfigSO : ScriptableObject
{
    public float StartingMovementSpeed;
    public float MaxMovementSpeed = 5f;
}

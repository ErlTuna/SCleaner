using UnityEngine;

[CreateAssetMenu(fileName = "Player X", menuName = "ScriptableObjects/Unit Info")]
public class UnitInfo : ScriptableObject
{
    public bool isAlive = true;
    public bool isInvuln = false;
    public bool isDashing = false;

    void OnEnable()
    {
        isAlive = true;
        isInvuln = false;
        isDashing = false;
    }
}

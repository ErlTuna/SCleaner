using UnityEngine;

[CreateAssetMenu(fileName = "PickupSO", menuName = "ScriptableObjects/Pickup Info")]
public class PickupSO : ScriptableObject
{
    public int value;
    public AudioClip PickUpAudio;
    public AudioClip FailToPickUpAudio;

    public enum PickupType{
        HEALTH,
        AMMO
    }
}

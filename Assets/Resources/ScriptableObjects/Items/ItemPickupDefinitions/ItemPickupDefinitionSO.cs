using UnityEngine;

[CreateAssetMenu(fileName = "Pickup Definition", menuName = "ScriptableObjects/Pickups/Pickup Definition")]
public class ItemPickupDefinitionSO : ScriptableObject
{
    public PickupType PickupType;
    public string PickUpName;
    public string PickUpDescription;
    public Sprite DefaultPickupSprite;
    public SoundDataSO PickUpSuccessSoundData;
    public SoundDataSO PickUpFailureSoundData;

}

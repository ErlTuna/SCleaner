using UnityEngine;

[CreateAssetMenu(fileName = "Pickup Definition", menuName = "ScriptableObjects/Pickup Definition")]
public class PickupDefinitionSO : ScriptableObject
{
    public AudioClip PickUpSuccessAudio;
    public AudioClip PickUpFailureAudio;
    public Sprite PickupSprite;
    public GameObject PickupPrefab;
}

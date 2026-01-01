using UnityEngine;
public interface IPickup : IHighlightable
{
    //public void PickupEffect();
    //public void PlayPickupSuccess();
    //public void PlayPickUpFailSound();
    public Vector3 Location { get; }
    void OnPickupAttempt(GameObject collector);
    bool CanBePickedUp(GameObject collector);
    void OnCollected(GameObject collector);
}
using UnityEngine;
public interface IPickup
{
    //public void OnPickupAttempt();
    //public void OnCollected();
    //public void PickupEffect();
    //public void PlayPickupSuccess();
    //public void PlayPickUpFailSound();
    public Vector3 Location { get; }
    void OnPickupAttempt(GameObject collector);
    bool CanBePickedUp(GameObject collector);
    void HighlightPickup(bool higlight);
    void OnCollected(GameObject collector);

    /*
    void ShowPrompt();
    void HidePrompt();
    */


}
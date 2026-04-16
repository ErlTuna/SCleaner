using UnityEngine;

public interface IInteractable 
{   
    bool IsInteractable {get;}
    Vector3 Location { get; }
    void OnInteractionAttempt(GameObject interactor);
    void Highlight(bool highlight);
}
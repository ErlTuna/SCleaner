using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetector : MonoBehaviour
{
    [SerializeField] GameObject _owner;
    [SerializeField] CircleCollider2D _detectionArea;
    //readonly HashSet<IPickup> _pickupsInRange = new();
    //IPickup _closestPickup;

    readonly HashSet<IInteractable> _interactablesInRange = new();
    IInteractable _closestInteractable;

    void Update()
    {
        UpdateClosestInteractable();
        if (PlayerInputManager.Instance.ItemPickupInput)
            TryInteracting();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            _interactablesInRange.Add(interactable);
            //UpdateClosestPickup();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            _interactablesInRange.Remove(interactable);
            //UpdateClosestPickup();
        }
    }


    public void TryInteracting()
    {
        if (_closestInteractable == null) return;

        _closestInteractable.OnInteractionAttempt(_owner);
    }

    void UpdateClosestInteractable()
    {
        if (_interactablesInRange.Count == 0)
        {
            if(_closestInteractable != null)
                _closestInteractable.Highlight(false);
                
            _closestInteractable = null;
            return;
        }
        
        float closestDistanceSq = float.MaxValue;
    
        IInteractable newClosest = null;
        foreach (IInteractable interactable in _interactablesInRange)
        {
            if (interactable == null) continue;
            if (interactable.IsInteractable == false) continue;

            float distanceSq = (interactable.Location - transform.position).sqrMagnitude;

            if (distanceSq < closestDistanceSq)
            {
                closestDistanceSq = distanceSq;
                newClosest = interactable;
            }
        }

    
        //If the closest pickup has changed, update highlight
        if (newClosest != null)
        {
            _closestInteractable?.Highlight(false);
            _closestInteractable = newClosest;
            _closestInteractable?.Highlight(true);
        }
    
    }

       
}

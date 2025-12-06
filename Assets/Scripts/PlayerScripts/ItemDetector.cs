using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetector : MonoBehaviour
{
    [SerializeField] GameObject _owner;
    [SerializeField] CircleCollider2D _detectionArea;
    HashSet<IPickup> _pickupsInRange = new();
    IPickup _closestPickup;

    void Update()
    {
        UpdateClosestPickup();
        if (PlayerInputManager.Instance.ItemPickupInput)
            TryCollectingPickup();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IPickup pickup))
        {
            Debug.Log("Caught an item : " + pickup);
            _pickupsInRange.Add(pickup);
            //UpdateClosestPickup();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IPickup>(out var pickup))
        {
            _pickupsInRange.Remove(pickup);
            //UpdateClosestPickup();
        }
    }


    public void TryCollectingPickup()
    {
        if (_closestPickup == null) return;

        _closestPickup.OnPickupAttempt(_owner);
    }

    void UpdateClosestPickup()
    {
        if (_pickupsInRange.Count == 0)
        {
            if(_closestPickup != null)
                _closestPickup?.HighlightPickup(false);
                
            _closestPickup = null;
            return;
        }


    
        float closestDistanceSq = float.MaxValue;
    
        IPickup newClosest = null;
        foreach (IPickup pickup in _pickupsInRange)
        {
            if (pickup == null) continue;

            float distanceSq = (pickup.Location - transform.position).sqrMagnitude;

            if (distanceSq < closestDistanceSq)
            {
                closestDistanceSq = distanceSq;
                newClosest = pickup;
            }
        }

    
        //If the closest pickup has changed, update highlight
        if (newClosest != null)
        {
            _closestPickup?.HighlightPickup(false);
            _closestPickup = newClosest;
            _closestPickup?.HighlightPickup(true);
        }
    
    }

       
}

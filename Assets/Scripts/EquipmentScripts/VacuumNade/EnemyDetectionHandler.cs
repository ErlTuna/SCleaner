using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionHandler : MonoBehaviour
{
    [SerializeField] CircleCollider2D _detectionArea;
    HashSet<GameObject> _caughtEnemies = new();
    public IReadOnlyCollection<GameObject> EnemyGOs => _caughtEnemies;


    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Enemy"))
        {
            _caughtEnemies.Add(other.gameObject);
            Debug.Log("Caught : " + other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            _caughtEnemies.Remove(other.gameObject);
            Debug.Log("Removed : " + other.gameObject);
        }
    }

    public void EnableCollider(bool enabled)
    {
        _detectionArea.enabled = enabled;
    }

}

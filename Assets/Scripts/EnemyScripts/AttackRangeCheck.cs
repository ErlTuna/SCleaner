using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeCheck : MonoBehaviour
{
    [SerializeField] Unit _ownerScript;
    [SerializeField] CircleCollider2D _attackRange;
    EnemyStateData _stateData;
    void Start()
    {
        if (_attackRange == null)
            Debug.Log("Missing attack range circle collider");

        if (_ownerScript.RuntimeDataHolder.TryGetRuntimeData(out EnemyStateData stateData))
        {
            _stateData = stateData;
        }
        else
        {
            Debug.LogError("EnemyStateData not found in runtime data");
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(_stateData == null)
                Debug.Log("The state data doe..." + _stateData);
            if (_stateData != null)
                _stateData.PlayerWithinAttackRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if (_stateData != null)
                _stateData.PlayerWithinAttackRange = false;
        }
    }
}

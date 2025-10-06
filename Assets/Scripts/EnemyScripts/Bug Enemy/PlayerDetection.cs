using Unity.VisualScripting;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{    
    [SerializeField] Unit _owner;
    EnemyStateData _stateData;

    void Start()
    {
        //owner = GetComponentInParent<IEnemy>();
    }

    public void Initialize(Unit owner)
    {
        _owner = owner;
        if (_owner.RuntimeDataHolder.TryGetRuntimeData(out EnemyStateData data))
            _stateData = data;
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player") && _stateData != null)
        {
            _stateData.HasDetectedPlayer = true;
            Debug.Log("Detected player!");
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player") && _stateData != null){
            _stateData.HasDetectedPlayer = false;
        }
    }

}

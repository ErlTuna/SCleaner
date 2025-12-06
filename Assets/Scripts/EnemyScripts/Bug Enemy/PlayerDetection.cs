using Unity.VisualScripting;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{    
    [SerializeField] Unit _owner;
    EnemyStateData _stateData;

    public void InitializeStateData(EnemyStateData stateData)
    {
        _stateData = stateData;
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
            //_stateData.HasDetectedPlayer = false;
        }
    }

}

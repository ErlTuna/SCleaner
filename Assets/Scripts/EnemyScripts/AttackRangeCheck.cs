using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeCheck : MonoBehaviour
{
    IEnemy script;
    [SerializeField] CircleCollider2D _attackRange;
    void Start(){
        if(_attackRange == null)
            _attackRange = GetComponent<CircleCollider2D>();
        script = GetComponentInParent<IEnemy>();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            if(_attackRange.IsTouching(other))
                script.PlayerWithinAttackRange();
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
            if(!_attackRange.IsTouching(other)){
                script.PlayerOutOfAttackRange();
            }
            
        }
    }
}

using UnityEngine;

public class CollisionAttack : MonoBehaviour
{
    [SerializeField] Unit _owner;
    [SerializeField] UnitAttackConfigSO _attackConfigSO;
    void OnTriggerStay2D(Collider2D col){
        if (col.CompareTag("PlayerHitbox"))
        {
            IDamageable damageable = col.GetComponent<IDamageable>();
            damageable?.TakeDamage(_attackConfigSO.Damage);
        }
    }

       
}

        
    


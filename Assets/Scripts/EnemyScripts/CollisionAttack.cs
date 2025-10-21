using UnityEngine;

public class CollisionAttack : MonoBehaviour
{
    [SerializeField] Unit _owner;
    [SerializeField] UnitAttackConfigSO _attackConfigSO;
    void OnTriggerStay2D(Collider2D col){
        if (col.CompareTag("PlayerHitbox"))
        {
            if (col.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage(_attackConfigSO.ContactDamage);
        }
    }

       
}

        
    


using UnityEngine;

public class EnemyCollisionHandler : MonoBehaviour
{
    [SerializeField] Unit _owner;
    [SerializeField] int _contactDamage;

    public void Initialize(int damage)
    {
        _contactDamage = damage;
    }
    
    void OnTriggerStay2D(Collider2D col){
        if (col.CompareTag("PlayerHitbox"))
        {
            if (col.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage(_contactDamage);
        }
    }

       
}

        
    


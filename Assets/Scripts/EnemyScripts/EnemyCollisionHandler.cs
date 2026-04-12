using UnityEngine;

public class EnemyCollisionHandler : MonoBehaviour
{
    [SerializeField] Unit _owner;
    int _contactDamage;

    [Header("Debugging")]
    [SerializeField] bool _overrideContactDamage = false;
    [SerializeField] int _overrideValue = 0;

    public void Initialize(int damage)
    {
        //Debug.Log("PASSED VALUE : " + damage);
        _contactDamage = damage;
        //Debug.Log("Initialized contact damage. Contact damage is : " + _contactDamage + " The onwer is... : " + _owner.gameObject.name);
    }
    
    void OnTriggerStay2D(Collider2D col){
        if (col.CompareTag("PlayerHitbox"))
        {
            if (col.TryGetComponent<IDamageable>(out var damageable))
            {
                if (_overrideContactDamage)
                {   
                    //Debug.Log("Contact damage before call : " + _overrideValue);
                    damageable.TakeDamage(_overrideValue);
                }

                else
                {
                    //Debug.Log("Contact damage before call : " + _contactDamage);
                    damageable.TakeDamage(_contactDamage);
                }
                
            }
                
        }
    }

       
}

        
    


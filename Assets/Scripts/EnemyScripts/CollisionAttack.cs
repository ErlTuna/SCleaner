using UnityEngine;

public class CollisionAttack : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D col){
        IDamageable damageable = null;
        if(col.CompareTag("PlayerHitbox"))
            damageable = col.GetComponent<IDamageable>();
            
        damageable?.TakeDamage(5); 
        }

       
}

        
    


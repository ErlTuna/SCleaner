using System;
using UnityEngine;

public class BugHealth : MonoBehaviour, IDamageable, IHealth
{
    public static event Action OnDeathEvent;
    public UnitInfoSO UnitInfo{get;set;}
    [SerializeField] DamageFlash _damageFlash;
    [SerializeField] GameObject parent;
    public DropRandomItem itemDropper;

    void Awake(){
        if (UnitInfo == null){
            UnitInfo = ScriptableObject.CreateInstance<UnitInfoSO>();
        }
        if (_damageFlash == null){
            _damageFlash = GetComponent<DamageFlash>();
        }
    }
    void Start()
    {
        UnitInfo.health = UnitInfo.maxHealth;
    }

    public void TakeDamage(int amount){
        if(!UnitInfo.isAlive) return;
        
        _damageFlash.TriggerDamageFlash();
        UnitInfo.health -= amount;
        if(UnitInfo.health <= 0){     
            OnDeathEvent?.Invoke();   
            itemDropper.DropItem();    
            Destroy(parent);  
        }
    }

    public void TakeDamage(IEnemy attacker){
        
    }

}

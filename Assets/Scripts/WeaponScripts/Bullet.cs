using System;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public static event Action<Bullet> OnBulletDestroyedEvent;
    public static event Action<GameObject> OnBulletDestroyedEvent_v2;
    [SerializeField] CircleCollider2D _circleCollider2D;
    [SerializeField] Rigidbody2D _rb2D;
    [SerializeField] BulletConfigSO bulletConfig;
    [SerializeField] GameObject _owner;
    public LayerMask layerMask;
    Vector3 _trajectory;
    float _basePushForce;
    float _lifeTime;
    int _damage;
    bool _hasHitAnEnemy = false;
    void Awake(){
        //_trajectory = transform.right;
    }

    void Start()
    {
        _trajectory = transform.right;
    }

    void Update(){
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0){
            Destroy(gameObject);
        }
    }

    void FixedUpdate(){
        Move();
    }

    public void Setup(GameObject owner, WeaponRuntimeData weaponData, WeaponConfigSO weaponConfig)
    {
        _owner = owner;
        bulletConfig = weaponConfig.BulletData;
        _damage = weaponData.Damage;
        _basePushForce = weaponConfig.PushForce;
        _lifeTime = bulletConfig.LifeTime;
    }

    void Move(){
        _rb2D.velocity = _trajectory * bulletConfig.ProjectileSpeed;
    }


    void OnCollisionEnter2D(Collision2D other){
        if((layerMask.value & (1 << other.transform.gameObject.layer)) > 0){
            _circleCollider2D.enabled = false;
            Destroy(gameObject);
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if(_hasHitAnEnemy) return;

        
        if (col.TryGetComponent<IDamageable>(out var damageable))
        {
            _circleCollider2D.enabled = false;
            _hasHitAnEnemy = true;

            float distance = Vector2.Distance(_owner.transform.position, col.transform.position);
            float safeDistance = Mathf.Max(distance, 0.01f); // in case distance is ever zero
            float finalPushForce = _basePushForce / safeDistance;

            if (finalPushForce > _basePushForce)
                finalPushForce = _basePushForce;

            DamageContext context = new(gameObject, transform.position, _damage, finalPushForce);

            damageable?.TakeDamage(context);

            Destroy(gameObject);
        }        
    }



    void OnDestroy()
    {
        OnBulletDestroyedEvent?.Invoke(this);
        OnBulletDestroyedEvent_v2?.Invoke(gameObject);
    }
}


public struct DamageContext
{
    // what hit me?
    public GameObject Hitter;
    // from where?
    public Vector2 HitPosition;
    // for what damage
    public int Damage;
    // with how much force?
    public float PushForce;

    public DamageContext(GameObject Hitter, Vector2 HitPosition, int Damage, float PushForce)
    {
        this.Hitter = Hitter;
        this.HitPosition = HitPosition;
        this.Damage = Damage;
        this.PushForce = PushForce;
    }
}
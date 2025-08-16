using System;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public static event Action<Bullet> OnBulletDestroyedEvent;
    public static event Action<GameObject> OnBulletDestroyedEvent_v2;
    [SerializeField] CircleCollider2D _circleCollider2D;
    public LayerMask layerMask;
    Rigidbody2D _rb2D;
    Vector3 _trajectory;
    float _speed;
    int _damage;
    float _size;
    float _lifeTime;
    bool _hasHitAnEnemy = false;
    void Awake(){
        _trajectory = transform.right;
    }

    void Start()
    {
        _trajectory = transform.right;
        _rb2D = GetComponent<Rigidbody2D>();
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

    void Move(){
        _rb2D.velocity = _trajectory * _speed;
    }

    public void SetupBulletParameters(float speed, float size, int damage, float lifeTime){
        _speed = speed;
        _size = size;
        _damage = damage;
        _lifeTime = lifeTime;
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
        IDamageable damageable = col.GetComponent<IDamageable>();;
        if(damageable != null){
            _circleCollider2D.enabled = false;
            _hasHitAnEnemy = true;
            damageable.TakeDamage(_damage);
            Destroy(gameObject);
        }        
    }

    void OnDestroy()
    {
        OnBulletDestroyedEvent?.Invoke(this);
        OnBulletDestroyedEvent_v2?.Invoke(gameObject);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SuctionAbility : MonoBehaviour
{
    public static event Action<List<IDamageable>, List<IEnemy>> OnAbilityEndExplosion;
    public float _activationTime;
    float _activationLifeTime = 3f;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _vacuumAudio;
    [SerializeField] CircleCollider2D _circleCollider2D;
    public List<IEnemy> _enemyScripts = new List<IEnemy>();
    public List<Rigidbody2D> _enemyRigidbodies = new List<Rigidbody2D>();
    public List<IDamageable> _enemyDamageables = new List<IDamageable>();
    public float _pullForce = .1f;
    public bool _isPulling = false;
    bool _hasActivated = false;
    bool _abilityEnded = false;
   

    void Update()
    {
        CheckActivatedLifeTime();
        if(!_isPulling)
            StartCoroutine(DragEnemiesTowardsCenter());
    }

    void Awake(){
        if(!_circleCollider2D)
            _circleCollider2D = GetComponent<CircleCollider2D>();
    }

    void OnEnable(){
        _enemyRigidbodies.Clear();

        _circleCollider2D.enabled = true;
        _activationTime = Time.time;
        _hasActivated = true;
        enabled = true;
    }

    void OnDisable(){
        _circleCollider2D.enabled = false;
        enabled = false;
        _isPulling = false;
        if(_audioSource)
        _audioSource.Stop();
    }

    //OPTIMIZE THIS LATER!!!
    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Enemy")){
            Rigidbody2D enemyRB = other.gameObject.GetComponent<Rigidbody2D>();
            IEnemy enemyMain = other.gameObject.GetComponent<IEnemy>();
            IDamageable damageable = other.gameObject.GetComponentInChildren<IDamageable>();
            enemyRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            _enemyScripts.Add(enemyMain);
            _enemyRigidbodies.Add(enemyRB);
            _enemyDamageables.Add(damageable);
        }
        
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Enemy")){
        Rigidbody2D enemyRB = other.gameObject.GetComponent<Rigidbody2D>();
        IEnemy enemyMain = other.gameObject.GetComponent<IEnemy>();
        IDamageable damageable = other.gameObject.GetComponentInChildren<IDamageable>();
        enemyRB.constraints = RigidbodyConstraints2D.None;
        _enemyScripts.Remove(enemyMain);
        _enemyRigidbodies.Remove(enemyRB);
        _enemyDamageables.Remove(damageable);
        }
        
    }

    IEnumerator DragEnemiesV2(){
        if(!_enemyRigidbodies.Any()) yield break;

        _isPulling = true;
        for(int i = 0; i < _enemyScripts.Count; ++i){
            if(_enemyScripts[i] != null){
                _enemyScripts[i].EnemyInfo.isImmobilized = true;
                print("set pulled");
            }
        }



        //yield return null;
        //yield return null;

        Vector2 individualPullForce;
        Vector2 direction;
        Vector2 suctionCenter = transform.position;
        

        for(int i = 0; i < _enemyRigidbodies.Count(); ++i){
            Rigidbody2D rb2d = _enemyRigidbodies.ElementAt(i);
            if(rb2d != null){
                    direction = (suctionCenter - rb2d.position).normalized;
                    individualPullForce = direction * 1f;
                    print(direction);
                    print(individualPullForce);
                    rb2d.AddForce(individualPullForce, ForceMode2D.Impulse);
            }
            
        }
        PlayWeaponSounds.ReceiveAudioSource(_audioSource);
        PlayWeaponSounds.PlayGunSound(_vacuumAudio);
        _isPulling = false;
    }

    IEnumerator DragEnemiesTowardsCenter(){
        

        if(!_enemyRigidbodies.Any()) yield break;

        for(int i = 0; i < _enemyScripts.Count; ++i){
            if(_enemyScripts[i] != null){
                _enemyScripts[i].EnemyInfo.isImmobilized = true;
            }
        }
        _isPulling = true;
        float distance;
        Vector2 individualPullForce;
        Vector2 direction;
        Vector2 suctionCenter = transform.position;


        for(int i = 0; i < _enemyRigidbodies.Count(); ++i){

            Rigidbody2D rb2d = _enemyRigidbodies.ElementAt(i);
            if(rb2d != null){
                direction = suctionCenter - rb2d.position;
                distance = direction.magnitude;
                direction.Normalize();

                distance = Mathf.Clamp(distance, 0.1f, _circleCollider2D.radius);

                if (distance < 1f){
                    rb2d.velocity = Vector2.zero;
                    //rb2d.position = Vector2.Lerp(rb2d.position, suctionCenter, 0.25f);
                    rb2d.position = suctionCenter;
                } else {
                    individualPullForce = direction * (_pullForce * distance);
                    rb2d.AddForce(individualPullForce, ForceMode2D.Impulse);
                }
            }
            
        }
        PlayWeaponSounds.ReceiveAudioSource(_audioSource);
        PlayWeaponSounds.PlayGunSound(_vacuumAudio);

        _isPulling = false;
    }

    void CheckActivatedLifeTime(){
        if(!_hasActivated) return;

        if(_activationLifeTime < Time.time - _activationTime){
            OnAbilityEndExplosion?.Invoke(_enemyDamageables, _enemyScripts);
            enabled = false;
        }
    }

    public void ReceiveAudioSource(AudioSource source){
        if(!_audioSource)
        _audioSource = source;
    }

    public void ReceiveVacuumAudio(AudioClip audioClip){
        if(!_vacuumAudio)
        _vacuumAudio = audioClip;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using UnityEngine;

public class Grenade : MonoBehaviour, IEquipment
{
    
    public event Action OnAbilityEnd;
    EquipmentSO _equipmentSO;
    AudioSource _audioSource;
    Transform _grenadePosition;
    Transform _parentTransform;
    Vector2 _trajectory;
    Rigidbody2D _rb2D;
    BoxCollider2D _boxCollider2D;
    public SuctionAbility _suctionAbility;
    public Explode _explosionAbility;
    
    public float _speed = 3f;
    //public bool _hasJustBeenTossed;
    public bool _hasActivated = false;

    void Awake(){
        _rb2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();

        _boxCollider2D = GetComponent<BoxCollider2D>();
        _equipmentSO = Resources.Load<EquipmentSO>("ScriptableObjects/VacuumGrenadeSO");
        
    }

    void OnEnable(){
        SuctionAbility.OnAbilityEndExplosion += TriggerExplosion;
        _explosionAbility.OnExplosionEndResetEvent += Reset;
        _boxCollider2D.enabled = true;
        enabled = false;
    }
    void Start(){
        _rb2D.isKinematic = true;
        _parentTransform = transform.parent;
        _suctionAbility.ReceiveAudioSource(_audioSource);
        _suctionAbility.ReceiveVacuumAudio(_equipmentSO.activatedSFX);
        _explosionAbility.ReceiveAudioSource(_audioSource);
        _explosionAbility.ReceiveExplosionAudio(_equipmentSO.activatedSFX);
    }

    void OnDestroy(){
        SuctionAbility.OnAbilityEndExplosion -= TriggerExplosion;
        _explosionAbility.OnExplosionEndResetEvent -= Reset;
    }

    public void SetupEquipmentParameters(Quaternion rotation, Vector2 direction, Transform parentTransform, Transform grenadePosition){
        transform.rotation = rotation;
        _trajectory = direction;
        _parentTransform = parentTransform;
        _grenadePosition = grenadePosition;
    }

    public void UseEquipment(){
        if(!_boxCollider2D.enabled)
            _boxCollider2D.enabled = true;

        //_hasJustBeenTossed = true;
        //StartCoroutine(SetTossedToFalse());
        _rb2D.AddTorque(25f);
        _rb2D.AddForce(_trajectory * _speed, ForceMode2D.Impulse);
    }
    public void TriggerAbility(){
        _rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
        _boxCollider2D.enabled = false;
        _hasActivated = true;
        _suctionAbility.enabled = true;
    }

    public void Toss(){
        //_hasJustBeenTossed = true;
        _rb2D.AddTorque(2.5f, ForceMode2D.Impulse);
        _rb2D.AddForce(_trajectory * _speed, ForceMode2D.Impulse);
        //StartCoroutine(SetTossedToFalse());
    }

    public IEnumerator TriggerAbilityAfterTime(){
        enabled = true;
        _rb2D.isKinematic = false;
        Toss();
        yield return new WaitForSeconds(2f);
        TriggerAbility();
    }

    public IEnumerator SetTossedToFalse(){
        yield return null;
        //_hasJustBeenTossed = false;
    }

    void Reset(){
        _hasActivated = false;
        //_hasJustBeenTossed = false;
        transform.rotation = Quaternion.identity;
        transform.parent = _parentTransform;
        transform.position = _grenadePosition.position;

        _rb2D.constraints = RigidbodyConstraints2D.None;
        _rb2D.angularVelocity = 0f;
        _rb2D.velocity = Vector2.zero;
        _rb2D.isKinematic = true;

        _boxCollider2D.enabled = true;

        OnAbilityEnd?.Invoke();
        enabled = false;
        gameObject.SetActive(false);
    }

    void TriggerExplosion(List<IDamageable> enemies, List<IEnemy> enemyScripts){
        _explosionAbility.StartExplosion(enemies, enemyScripts);
    }

    public void ActivateScript()
    {
        enabled = true;
    }

    public void DisableScript()
    {
        enabled = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{   
    [SerializeField] AudioSource _audioSource;
    public AudioClip explosionAudio;
    public event Action OnExplosionEndResetEvent;
    int _damage = 10;
    public bool _explosionTriggered = false;
    [SerializeField] ParticleSystem _explosionParticles = default;
    void OnEnable(){
        _explosionTriggered = false;
    }

    void Update(){
        if (_explosionTriggered && !_explosionParticles.IsAlive()){
            OnExplosionEndResetEvent?.Invoke();
            _explosionTriggered = false;
        }
    }
    
    public void StartExplosion(List<IDamageable> enemies, List<IEnemy> enemyScripts){
        if(_explosionTriggered) return;

        _explosionTriggered = true;
        _explosionParticles.Play();

        PlayWeaponSounds.ReceiveAudioSource(_audioSource);
        PlayWeaponSounds.PlayGunSound(explosionAudio);
        
        print("Enemy count : " + enemies.Count);
        for(int i = 0; i < enemies.Count; ++i){
            print("Enemy count : " + enemies.Count + " loop count.");
            enemies[i].TakeDamage(_damage);
            enemyScripts[i].EnemyInfo.isImmobilized = false;
        }
        enemyScripts.Clear();
        enemies.Clear();
    }

    public void StopExplosion(){
        if(_explosionParticles.isPlaying)
        _explosionParticles.Stop();
    }

    public IEnumerator Wait(){
        yield return new WaitForSeconds(.5f);
    }

    public void ReceiveAudioSource(AudioSource source){
        _audioSource = source;
    }
    public void ReceiveExplosionAudio(AudioClip clip){
        explosionAudio = clip;
    }
}

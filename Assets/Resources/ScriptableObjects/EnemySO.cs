using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInfoSO", menuName = "ScriptableObjects/Enemy Info")]
public class EnemySO : ScriptableObject
{
    public AudioClip OnHitSFX;
    public AudioClip OnDefeatSFX;
    public Sprite DefeatSprite;
    public int health;
    public int maxHealth = 20;
    public float movementSpeed;
    public float maxMovementSpeed = 5f;
    public float damage = 5f;
    public bool isAlive = true;
    public bool isInvuln = false;
    public bool hasDetectedPlayer = false;
    public bool playerWithinAttackRange = false;
    public bool hasBeenAttacked = false;
    public bool isImmobilized = false;
    public bool isCharging = false;
    public bool isAttacking = false;
    public bool hasAttacked = false;
    public bool isRecovering = false;
    


    public void Init(){
        health = maxHealth;
        movementSpeed = maxMovementSpeed;
        isAlive = true;
        isInvuln = false;
    }
}

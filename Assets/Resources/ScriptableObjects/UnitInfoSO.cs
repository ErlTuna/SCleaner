using UnityEngine;

[CreateAssetMenu(fileName = "Player Bools", menuName = "ScriptableObjects/Unit Info")]
public class UnitInfoSO : ScriptableObject
{
    public AudioClip HealthFullSFX;
    public AudioClip OnHitSFX;
    public AudioClip OnDefeatSFX;
    public AudioClip DashSFX;
    public AudioClip DashRecoverSFX;
    public Sprite DefaultSprite;
    public Sprite DefeatSprite;
    public Sprite HurtSprite;
    public Sprite AttackSprite;
    public int currency = 0;
    public int maxCurrency = 9999;
    public int health;
    public int maxHealth = 20;
    public float movementSpeed;
    public float maxMovementSpeed = 5f;
    public bool isAlive = true;
    public bool isInvuln = false;
    public bool isDashing = false;
    public float damage = 5f;
    

    public void Init(){
        currency = 0;
        health = maxHealth;
        movementSpeed = maxMovementSpeed;
        isAlive = true;
        isInvuln = false;
        isDashing = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player SOs", menuName = "ScriptableObjects/Component SOs/Player Sounds")]
public class PlayerSoundsSO : ScriptableObject
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
}

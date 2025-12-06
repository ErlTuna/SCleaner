using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageContext
{
    // what hit me?
    public GameObject Hitter;
    public Vector2 HitterMovementVector;
    // from where?
    public Vector2 HitLocation;
    // for what damage
    public int Damage;
    // with how much force?
    public float PushForce;

    public DamageContext(GameObject Hitter, Vector2 HitterMovementVector, Vector2 HitLocation, int Damage, float PushForce)
    {
        this.Hitter = Hitter;
        this.HitterMovementVector = HitterMovementVector;
        this.HitLocation = HitLocation;
        this.Damage = Damage;
        this.PushForce = PushForce;
    }
}

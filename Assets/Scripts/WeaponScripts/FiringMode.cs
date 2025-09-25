using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class FiringMode : MonoBehaviour
{
    [SerializeField] protected BaseWeapon owner;
    public abstract void TryFire();
    public abstract void StopFire();
    public abstract void ResetState();
}

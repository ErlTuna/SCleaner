using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponModifier
{
    void RequestApply(WeaponRuntime weapon);
    void RequestRemove(WeaponRuntime weapon);
}


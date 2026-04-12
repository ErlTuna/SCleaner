using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponModifier
{
    void RequestApply(WeaponRuntimeData weapon);
    void RequestRemove(WeaponRuntimeData weapon);
}


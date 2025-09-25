using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitAttackData : IUnitAttackData
{
    public int Damage { get; set; }

    public void AutoConfigureWithWrapper(UnitConfigsWrapperSO configWrapper)
    {
        ConfigureWith(configWrapper.attackConfigSO);
    }

    public void ConfigureWith(UnitAttackConfigSO config)
    {
        Damage = config.Damage;
    }
}

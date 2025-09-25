using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitAttackData : IConfigurable<UnitAttackConfigSO>, IAutoConfigurable
{
    public int Damage { get; set;}
}

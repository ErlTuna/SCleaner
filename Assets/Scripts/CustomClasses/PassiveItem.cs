using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem
{
    [SerializeField] PassiveItemSO _itemConfig;
    public PassiveItemSO ItemConfig => _itemConfig;
}

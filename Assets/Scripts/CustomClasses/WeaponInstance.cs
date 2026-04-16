using UnityEngine;

public class WeaponInstance
{
    public readonly string WeaponID;
    public readonly GameObject WeaponGO;
    public readonly PlayerWeapon WeaponScript;

    public WeaponInstance(string weaponID, GameObject weaponGO, PlayerWeapon weaponScript)
    {
        WeaponID = weaponID;
        WeaponGO = weaponGO;
        WeaponScript = weaponScript;
    }
}

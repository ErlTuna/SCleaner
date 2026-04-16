using UnityEngine;

[CreateAssetMenu(fileName = "MagazineReloadStrategy", menuName = "ScriptableObjects/Weapon/Reload Strategies/Magazine Reload")]
public class MagazineReloadStrategy : ReloadStrategySO
{
    public override void PerformReload(ReloadContext context)
    {
        if (context.AmmoConfig.HasInfiniteReserveAmmo)
        {
            context.AmmoRuntime.CurrentAmmo = context.AmmoConfig.RoundCapacity;
            Debug.Log("Weapon has infinite ammo. CurrentAmmo now : " + context.AmmoRuntime.CurrentAmmo);
            return;
        }

        int needed = context.AmmoConfig.RoundCapacity - context.AmmoRuntime.CurrentAmmo;
        int taken = Mathf.Min(needed, context.AmmoRuntime.CurrentReserveAmmo);

        context.AmmoRuntime.CurrentAmmo += taken;
        context.AmmoRuntime.CurrentReserveAmmo -= taken;

    }

}


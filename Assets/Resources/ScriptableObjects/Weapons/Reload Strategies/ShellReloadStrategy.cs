using UnityEngine;

[CreateAssetMenu(fileName = "ShellReloadStrategy", menuName = "ScriptableObjects/Weapon/Reload Strategies/Shell Reload")]
public class ShellReloadStrategy : ReloadStrategySO
{
    const int TakenPerShell = 1;
    public override void PerformReload(ReloadContext context)
    {
        // Reload one shell
        context.AmmoRuntime.CurrentAmmo += TakenPerShell;
        
        if (context.AmmoRuntime.HasInfiniteReserveAmmo == false)
            context.AmmoRuntime.CurrentReserveAmmo -= TakenPerShell;

    }
}

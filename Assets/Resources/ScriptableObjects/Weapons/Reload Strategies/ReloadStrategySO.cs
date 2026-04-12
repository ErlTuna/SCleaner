using UnityEngine;
public abstract class ReloadStrategySO : ScriptableObject
{
    public bool CanBeReloadCanceled = false;
    public abstract void PerformReload(ReloadContext context);
}





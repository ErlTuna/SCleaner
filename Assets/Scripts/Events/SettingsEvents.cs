using System;
public static class SettingsEvents
{
    public static event Action OnSettingsAltered;
    public static event Action OnSettingsApplied;
    public static event Action OnSettingsReverted;
    public static event Action OnSettingsChangesUnsaved;
    

    public static void RaiseSettingsAltered()
    {
        OnSettingsAltered?.Invoke();
    }

    public static void RaiseSettingsApplied()
    {
        OnSettingsApplied?.Invoke();
    }

    public static void RaiseSettingsReverted()
    {
        OnSettingsReverted?.Invoke();
    }

    public static void RaiseSettingsChangesUnsaved()
    {
        OnSettingsChangesUnsaved?.Invoke();
    }

    
    
}


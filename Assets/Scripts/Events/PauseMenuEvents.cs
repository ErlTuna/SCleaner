using System;

public static class PauseMenuEvents
{
    public static event Action OnPauseMenuOpened;
    public static event Action OnPauseMenuClosed;
    public static event Action OnSettingsMenuOpened;
    public static event Action OnSettingsMenuClosed;
    public static event Action OnSettingsChangesUnsaved;


    public static void RaisePauseMenuOpened()
    {
        OnPauseMenuOpened?.Invoke();
    }

    public static void RaisePauseMenuClosed()
    {
        OnPauseMenuClosed?.Invoke();
    }

    public static void RaiseOptionsMenuOpened()
    {
        OnSettingsMenuOpened?.Invoke();
    }

    public static void RaiseOptionsMenuClosed()
    {
        OnSettingsMenuClosed?.Invoke();
    }

    public static void RaiseSettingsChangesUnsaved()
    {
        OnSettingsChangesUnsaved?.Invoke();
    }
    
}

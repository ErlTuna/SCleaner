using System;

public static class MenuOpenCloseEvents
{
    public static event Action OnPauseMenuOpened;
    public static event Action OnPauseMenuClosed;
    public static event Action OnSettingsOpened;
    public static event Action OnSettingsClosed;

    public static void RaisePauseMenuOpened()
    {
        OnPauseMenuOpened?.Invoke();
    }

    public static void RaisePauseMenuClosed()
    {
        OnPauseMenuClosed?.Invoke();
    }

    public static void RaiseSettingsOpened()
    {
        OnSettingsOpened?.Invoke();
    }

    public static void RaiseSettingsClosed()
    {
        OnSettingsClosed?.Invoke();
    }

}

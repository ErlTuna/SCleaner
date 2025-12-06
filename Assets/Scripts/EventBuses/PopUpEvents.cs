using System;

public static class PopUpEvents
{
    public static event Action OnPopUpTriggered;
    public static event Action OnWarningYes;
    public static event Action OnWarningNo;

    public static void RaisePopUpTriggered()
    {
        OnPopUpTriggered?.Invoke();
    }

    public static void RaiseWarningYes()
    {
        OnWarningYes?.Invoke();
    }

    public static void RaiseWarningNo()
    {
        OnWarningNo?.Invoke();
    }
}

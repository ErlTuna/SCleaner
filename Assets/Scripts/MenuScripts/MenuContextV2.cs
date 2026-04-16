using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuContextV2<TSection> : MonoBehaviour where TSection : Enum
{
    static MenuContextV2<TSection> _currentContext;
    public static MenuContextV2<TSection> Current => _currentContext;

    public Stack<TSection> sectionHistory = new();

    public abstract void Execute(MenuAction action);

    protected void SetCurrentContext(MenuContextV2<TSection> context)
    {
        _currentContext = context;
    }

    protected void PushSection(TSection section)
    {
        Debug.Log("Pushed section to history : " + section);
        sectionHistory.Push(section);
    }

    protected TSection PopSection()
    {
        if (sectionHistory.Count != 0)
        {
            TSection poppedSection = sectionHistory.Pop();
            Debug.Log("Popped section from history : " + poppedSection);
            return poppedSection;
        }
        return default;
    }

    protected TSection PeekSection()
    {
        if (sectionHistory.Count != 0)
            return sectionHistory.Peek();

        return default;
    }
}

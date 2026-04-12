using System.Collections.Generic;
using UnityEngine;

public abstract class MenuContext : MonoBehaviour
{
    static MenuContext _currentContext;
    public static MenuContext Current => _currentContext;

    //public Stack<IMenuSection> sectionHistory = new();

    public Stack<IMenuSection> navigationHistory = new();
    public abstract void Execute(MenuAction action);

    protected void SetCurrentContext(MenuContext context)
    {
        _currentContext = context;
    }

    /*
    protected void PushSection(IMenuSection menuSection)
    {
        Debug.Log("Pushed section to history : " + menuSection);
        sectionHistory.Push(menuSection);
    }

    protected IMenuSection PopSection()
    {
        if (sectionHistory.Count != 0)
        {
            IMenuSection poppedSection = sectionHistory.Pop();
            Debug.Log("Popped section from history : " + poppedSection);
            return poppedSection;
        }
           

        else
            return null;
    }

    protected IMenuSection PeekSection()
    {
        if (sectionHistory.Count != 0)
            return sectionHistory.Peek();

        return null;
    }
    */

    protected void PushSection(IMenuSection menuSection)
    {
        Debug.Log("Pushed section to history : " + menuSection);
        navigationHistory.Push(menuSection);
    }

    protected IMenuSection PopSection()
    {
        if (navigationHistory.Count != 0)
        {
            IMenuSection poppedSection = navigationHistory.Pop();
            Debug.Log("Popped section from history : " + poppedSection);
            return poppedSection;
        }
           

        else
            return null;
    }

    protected IMenuSection PeekSection()
    {
        if (navigationHistory.Count != 0)
            return navigationHistory.Peek();

        return null;
    }

}

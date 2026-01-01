using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuContext : MonoBehaviour
{
    public static Stack<MenuContext> MenuStack = new Stack<MenuContext>();
    public static MenuContext Current => MenuStack.Count > 0 ? MenuStack.Peek() : null;

    protected virtual void OnEnable()
    {
        MenuStack.Push(this);
    }

    protected virtual void OnDisable()
    {
        if (MenuStack.Peek() == this)
            MenuStack.Pop();
    }

    
    public abstract void Execute(MenuAction action);

}

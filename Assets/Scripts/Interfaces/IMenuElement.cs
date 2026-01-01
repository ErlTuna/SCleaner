using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuElement
{
    public bool IsVisible {get;set;}
    void Show();
    void Hide();
}

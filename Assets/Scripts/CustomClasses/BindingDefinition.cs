using UnityEngine;


// DATA ONLY representation of a key bind.
// This describes WHAT a binding is.
[System.Serializable]
public class BindingDefinition
{
    public BindingId id;
    public Sprite icon;
    public string label;
}

public enum BindingId
{
    OK,
    BACK,
    CANCEL,
    SUBMIT,
    CONFIRM,
    DECREASE,
    INCREASE,
    SAVE,
    DISCARD
}


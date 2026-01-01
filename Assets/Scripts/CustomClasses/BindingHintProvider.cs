using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach this on a game object that exposes binding hints.
// Example : A selectable UI element, such as a button.
public class BindingHintProvider : MonoBehaviour
{
    public BindingDefinition[] bindings;
}

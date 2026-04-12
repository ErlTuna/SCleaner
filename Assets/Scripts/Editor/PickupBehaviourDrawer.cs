/*
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PickupBehaviorSO), true)]
public class PickupBehaviourDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Draw a box around the element
        EditorGUI.BeginProperty(position, label, property);

        // Find the Order property
        SerializedProperty orderProp = property.FindPropertyRelative("Order");

        // Split rect into two: Order (50px) | rest of label
        Rect orderRect = new Rect(position.x, position.y, 50, position.height);
        Rect labelRect = new Rect(position.x + 55, position.y, position.width - 55, position.height);

        // Draw order field
        if (orderProp != null)
        {
            EditorGUI.PropertyField(orderRect, orderProp, GUIContent.none);
        }

        // Draw the rest of the ScriptableObject reference
        EditorGUI.PropertyField(labelRect, property, new GUIContent(property.objectReferenceValue?.name ?? label.text));

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property);
    }
}
#endif
*/
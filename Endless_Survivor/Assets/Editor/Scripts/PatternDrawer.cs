using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

[CustomPropertyDrawer(typeof(IPattern), true)]
public class PatternDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var buttonRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        string typeName = property.managedReferenceValue?.GetType().Name ?? "None";

        if (EditorGUI.DropdownButton(buttonRect, new GUIContent($"{label.text} ({typeName})"), FocusType.Keyboard))
        {
            GenericMenu menu = new GenericMenu();

            foreach (var type in Utility.GetImplementationsOf<IPattern>())
            {
                menu.AddItem(
                    new GUIContent(type.Name),
                    property.managedReferenceValue?.GetType() == type,
                    () =>
                    {
                        property.managedReferenceValue = Activator.CreateInstance(type);
                        property.serializedObject.ApplyModifiedProperties();
                    });
            }

            menu.ShowAsContext();
        }

        if (property.managedReferenceValue != null)
        {
            Rect fieldRect = position;
            fieldRect.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(fieldRect, property, GUIContent.none, true);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.managedReferenceValue == null)
            return EditorGUIUtility.singleLineHeight;

        return EditorGUIUtility.singleLineHeight +
               EditorGUI.GetPropertyHeight(property, true);
    }
}
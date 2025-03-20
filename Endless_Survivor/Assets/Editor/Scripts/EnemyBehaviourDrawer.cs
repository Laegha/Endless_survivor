using Codice.CM.Client.Differences.Graphic;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnemyBehaviour), true)]
public class EnemyBehaviourDrawer : PropertyDrawer
{
    List<Type> _overrideTypes = new List<Type>();
    readonly float _labelShowSize = 20;
    int foldouts;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        EnemyBehaviour target = property.managedReferenceValue as EnemyBehaviour;
        float baseHeight = EditorGUI.GetPropertyHeight(property, true);
        foldouts = Utility.CountOccurrences(property.propertyPath, "Array.data[");

        if (target == null || foldouts > 1)
            return _labelShowSize;

        if (property.isExpanded)
        {
            return baseHeight + 22;
        }

        return baseHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.managedReferenceValue == null)
        {
            EditorGUI.LabelField(position, "No/Invalid behaviour");
            return;
        }

        GUIContent customLabel = new GUIContent(property.managedReferenceValue.GetType().Name);

        float propertyHeight = EditorGUI.GetPropertyHeight(property, true);

        EnemyBehaviour targetBehaviour = property.managedReferenceValue as EnemyBehaviour;
        if (foldouts > 1)
        {
            EditorGUI.LabelField(position, customLabel);
            return;
        }
        // Draw the property
        Rect propertyRect = new Rect(position.x, position.y, position.width, propertyHeight);
        EditorGUI.PropertyField(propertyRect, property, customLabel, true);

        if (!property.isExpanded)
            return;
        Rect buttonRect = new Rect(position.x, position.y + propertyHeight + 2, position.width, 18);
        List<EnemyBehaviour> totalBehaviours = targetBehaviour.EnemyDataBehaviours();

        SyncOverrideBehaviours(targetBehaviour);
        if (EditorGUI.DropdownButton(buttonRect, new GUIContent("Add behaviour to override"), FocusType.Passive))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var behaviour in totalBehaviours)
            {
                if (_overrideTypes.Contains(behaviour.GetType()) || behaviour == targetBehaviour)
                    continue;

                menu.AddItem(new GUIContent(behaviour.GetType().Name), false, () =>
                {
                    targetBehaviour.OverrideBehaviours.Add(behaviour);
                });
            }
            menu.ShowAsContext();
        }
    }

    void SyncOverrideBehaviours(EnemyBehaviour target)
    {
        _overrideTypes.Clear();
        
        foreach (var behaviour in target.OverrideBehaviours)
        {
            if(behaviour == null)
            {
                continue;
            }
            _overrideTypes.Add(behaviour.GetType());

        }
    }
}

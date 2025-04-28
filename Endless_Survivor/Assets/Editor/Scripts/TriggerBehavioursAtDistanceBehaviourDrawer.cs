using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TriggerBehavioursAtDistance), true)]
public class TriggerBehavioursAtDistanceBehaviourDrawer : EnemyBehaviourDrawer
{
    List<Type> __behavioursToTrigger = new List<Type>();
    readonly float _labelShowSize = 20;
    int foldouts;
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        base.GetPropertyHeight(property, label);
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
        EditorGUI.BeginProperty(position, label, property);
        TriggerBehavioursAtDistance triggerBehavioursAtDistance = property.managedReferenceValue as TriggerBehavioursAtDistance;

        GUIContent customLabel = new GUIContent(property.managedReferenceValue.GetType().Name);

        float propertyHeight = EditorGUI.GetPropertyHeight(property, true);


        Rect propertyRect = new Rect(position.x, position.y, position.width, propertyHeight);
        EditorGUI.PropertyField(propertyRect, property, customLabel, true);

        if (!property.isExpanded)
            return;
        Rect buttonRect = new Rect(position.x, position.y + propertyHeight + 2, position.width, 18);
        List<EnemyBehaviour> totalBehaviours = triggerBehavioursAtDistance.EnemyDataBehaviours();

        SyncTriggerBehaviours(triggerBehavioursAtDistance);
        if (EditorGUI.DropdownButton(buttonRect, new GUIContent("Add behaviour to trigger"), FocusType.Passive))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var behaviour in totalBehaviours)
            {
                if (__behavioursToTrigger.Contains(behaviour.GetType()) || behaviour == triggerBehavioursAtDistance)
                    continue;

                menu.AddItem(new GUIContent(behaviour.GetType().Name), false, () =>
                {
                    triggerBehavioursAtDistance.BehavioursToTrigger.Add(behaviour);
                });
            }
            menu.ShowAsContext();
        }
        EditorGUI.EndProperty();

    }
    void SyncTriggerBehaviours(TriggerBehavioursAtDistance target)
    {
        __behavioursToTrigger.Clear();

        foreach (var behaviour in target.BehavioursToTrigger)
        {
            if (behaviour == null)
            {
                continue;
            }
            __behavioursToTrigger.Add(behaviour.GetType());

        }
    }
}

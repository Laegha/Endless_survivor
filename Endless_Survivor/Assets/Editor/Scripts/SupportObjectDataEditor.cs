using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SupportObjectData))]
public class SupportObjectDataEditor : Editor
{
    SerializedProperty _idleAnimation;
    SerializedProperty _supportObjColliders;
    Dictionary<Type, int> _behaviourTypes = new();

    private void OnEnable()
    {
        _idleAnimation = serializedObject.FindProperty("_idleAnimation");
        _supportObjColliders = serializedObject.FindProperty("_supportObjColliders");
        List<Type> behaviourTypes = Utility.GetSubclassesOf(typeof(SupportObjectBehaviour));
        SupportObjectData supportObjectData = (SupportObjectData)target;
        behaviourTypes.ForEach(type => _behaviourTypes.Add(type, supportObjectData.SupportObjBehaviours.Where(behaviour => behaviour.GetType() == type).Count()));
        supportObjectData.SupportObjBehaviours.RemoveAll(behaviour => behaviour == null);
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(_idleAnimation);
        EditorGUILayout.PropertyField(_supportObjColliders);
        serializedObject.ApplyModifiedProperties();

        SupportObjectData supportObjData = (SupportObjectData)target;
        SerializedProperty supportObjBehaviours = serializedObject.FindProperty("_supportObjBehaviours");


        EditorGUILayout.LabelField("Support Object Behaviours", EditorStyles.boldLabel);

        serializedObject.Update();
        //Display behaviours
        for (int i = 0; i < supportObjData.SupportObjBehaviours.Count; i++)
        {
            if (supportObjBehaviours.arraySize <= i)
            {
                break;
            }
            SerializedProperty behaviourProp = supportObjBehaviours.GetArrayElementAtIndex(i);
            if (behaviourProp != null)
                EditorGUILayout.PropertyField(behaviourProp, new GUIContent(supportObjData.SupportObjBehaviours[i].GetType().Name), true);
        }

        //Buttons to add or remove behaviours
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Behaviour"))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var type in _behaviourTypes)
            {
                var typeMaxStacks = type.Key.GetProperty("maxStacks", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (typeMaxStacks == null || type.Value >= (int)typeMaxStacks.GetValue(null) && (int)typeMaxStacks.GetValue(null) >= 0)
                    continue;

                menu.AddItem(new GUIContent(type.Key.Name), false, () =>
                {
                    SupportObjectBehaviour newBehaviour = (SupportObjectBehaviour)Activator.CreateInstance(type.Key);
                    supportObjData.SupportObjBehaviours.Add(newBehaviour);
                    _behaviourTypes[type.Key]++;
                    EditorUtility.SetDirty(supportObjData);
                });
            }
            menu.ShowAsContext();
        }
        if (GUILayout.Button("Remove Behaviour"))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var behaviour in supportObjData.SupportObjBehaviours)
            {
                menu.AddItem(new GUIContent(behaviour.GetType().Name), false, () =>
                {
                    supportObjData.SupportObjBehaviours.Remove(behaviour);
                    _behaviourTypes[behaviour.GetType()]--;
                    EditorUtility.SetDirty(supportObjData);
                });
            }
            menu.ShowAsContext();
        }
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}

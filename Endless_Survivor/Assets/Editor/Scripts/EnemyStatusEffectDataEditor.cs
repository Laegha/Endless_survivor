using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyStatusEffectData))]
public class EnemyStatusEffectDataEditor : Editor
{
    SerializedProperty _maxStacks;
    Dictionary<Type, bool> _addedStatusEffects = new();

    private void OnEnable()
    {
        List<Type> effectTypes = Utility.GetSubclassesOf(typeof(EnemyStatusEffect));
        _maxStacks = serializedObject.FindProperty("_effectMaxStacks");
        EnemyStatusEffectData statusEffectData = (EnemyStatusEffectData)target;
        effectTypes.ForEach(type => _addedStatusEffects.Add(type, statusEffectData.StatusEffects.Exists(effect => effect.GetType() == type)));
        statusEffectData.StatusEffects.RemoveAll(effect => effect == null);
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(_maxStacks);
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
        EnemyStatusEffectData statusEffectData = (EnemyStatusEffectData)target;
        SerializedProperty statusEffects = serializedObject.FindProperty("_statusEffects");

        EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);


        serializedObject.Update();
        statusEffectData.StatusEffects.RemoveAll(x => x == null);
        for (int i = 0; i < statusEffectData.StatusEffects.Count; i++)
        {
            if (statusEffects.arraySize <= i)
            {
                break;
            }
            SerializedProperty effectProp = statusEffects.GetArrayElementAtIndex(i);
            if (effectProp == null)
                continue;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(effectProp, new GUIContent(statusEffectData.StatusEffects[i].GetType().Name), true);

            if (GUILayout.Button("Remove " + statusEffectData.StatusEffects[i].GetType().Name))
            {
                _addedStatusEffects[statusEffectData.StatusEffects[i].GetType()] = false;
                statusEffectData.StatusEffects.RemoveAt(i);
                EditorUtility.SetDirty(statusEffectData);
            }
            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button("Add Status Effect"))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var type in _addedStatusEffects)
            {
                var isTypeUsable = type.Key.GetProperty("isUsable", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (type.Value || isTypeUsable == null || !(bool)isTypeUsable.GetValue(null))
                    continue;

                menu.AddItem(new GUIContent(type.Key.Name), false, () =>
                {
                    EnemyStatusEffect newEffect = (EnemyStatusEffect)Activator.CreateInstance(type.Key);

                    statusEffectData.StatusEffects.Add(newEffect);
                    _addedStatusEffects[type.Key] = true;
                    EditorUtility.SetDirty(statusEffectData);
                });
            }
            menu.ShowAsContext();
        }

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}

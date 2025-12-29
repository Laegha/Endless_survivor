using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CustomEditor(typeof(PlayerStatusEffectData))]
public class PlayerStatusEffectDataEditor : Editor
{
    SerializedProperty _maxStacks;
    Dictionary<Type, int> _addedStatusEffects = new();

    private void OnEnable()
    {
        List<Type> effectTypes = Utility.GetSubclassesOf(typeof(PlayerStatusEffect));
        _maxStacks = serializedObject.FindProperty("_effectMaxStacks");
        PlayerStatusEffectData statusEffectData = (PlayerStatusEffectData)target;
        effectTypes.ForEach(type => _addedStatusEffects.Add(type, statusEffectData.StatusEffects.Where(effect => effect.GetType() == type).Count()));
        statusEffectData.StatusEffects.RemoveAll(effect => effect == null);
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(_maxStacks);
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
        PlayerStatusEffectData statusEffectData = (PlayerStatusEffectData)target;
        SerializedProperty statusEffects = serializedObject.FindProperty("_statusEffects");

        EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);


        serializedObject.Update();
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
                _addedStatusEffects[statusEffectData.StatusEffects[i].GetType()]--;
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
                int effectCurrStacks = type.Value;

                var effectMaxStacksProperty = type.Key.GetProperty("maxStacks", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                int effectMaxStacks = (int)effectMaxStacksProperty.GetValue(null);
                if (effectMaxStacksProperty == null || effectMaxStacks >= 0 && effectMaxStacks <= effectCurrStacks)
                    continue;

                menu.AddItem(new GUIContent(type.Key.Name), false, () =>
                {
                    PlayerStatusEffect newEffect = (PlayerStatusEffect)Activator.CreateInstance(type.Key);

                    statusEffectData.StatusEffects.Add(newEffect);
                    _addedStatusEffects[type.Key] ++;
                    EditorUtility.SetDirty(statusEffectData);
                });
            }
            menu.ShowAsContext();
        }

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}

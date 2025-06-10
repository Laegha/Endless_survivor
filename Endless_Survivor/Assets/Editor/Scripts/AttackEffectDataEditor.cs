using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AttackEffectData))]
public class AttackEffectDataEditor : Editor
{
    Dictionary<Type, bool> _attackEffects = new();
    SerializedProperty _attackEffectChance;

    private void OnEnable()
    {
        _attackEffectChance = serializedObject.FindProperty("_effectChance");
        List<Type> behaviourTypes = Utility.GetSubclassesOf(typeof(AttackEffect));
        AttackEffectData attackEffectData = (AttackEffectData)target;
        behaviourTypes.ForEach(type => _attackEffects.Add(type, attackEffectData.AttackEffects.Exists(behaviour => behaviour.GetType() == type)));
        attackEffectData.AttackEffects.RemoveAll(behaviour => behaviour == null);
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(_attackEffectChance);
        serializedObject.ApplyModifiedProperties();
        //base.OnInspectorGUI();

        AttackEffectData attackEffectData = (AttackEffectData)target;
        SerializedProperty attackEffects = serializedObject.FindProperty("_attackEffects");

        EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);


        serializedObject.Update();
        for (int i = 0; i < attackEffectData.AttackEffects.Count; i++)
        {
            if (attackEffects.arraySize <= i)
            {
                break;
            }
            SerializedProperty effectProp = attackEffects.GetArrayElementAtIndex(i);
            if (effectProp == null)
                continue;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(effectProp, new GUIContent(attackEffectData.AttackEffects[i].GetType().Name), true);

            if (GUILayout.Button("Remove " + attackEffectData.AttackEffects[i].GetType().Name))
            {
                _attackEffects[attackEffectData.AttackEffects[i].GetType()] = false;
                attackEffectData.AttackEffects.RemoveAt(i);
                EditorUtility.SetDirty(attackEffectData);
            }
            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button("Add Effect"))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var type in _attackEffects)
            {
                var isTypeUsable = type.Key.GetProperty("isUsable", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (type.Value || isTypeUsable == null || !(bool)isTypeUsable.GetValue(null))
                    continue;

                menu.AddItem(new GUIContent(type.Key.Name), false, () =>
                {
                    AttackEffect newEffect = (AttackEffect)Activator.CreateInstance(type.Key, (AttackEffect)null, (Attack)null);
                    
                    attackEffectData.AttackEffects.Add(newEffect);
                    _attackEffects[type.Key] = true;
                    EditorUtility.SetDirty(attackEffectData);
                });
            }
            menu.ShowAsContext();
        }

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}

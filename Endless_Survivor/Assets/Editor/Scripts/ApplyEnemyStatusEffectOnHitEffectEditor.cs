using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ApplyEnemyStatusOnHitAttackEffect))]
public class ApplyEnemyStatusEffectOnHitEffectEditor : Editor
{
    SerializedProperty _statusEffect;
    List<Type> _statusEffectTypes = new();
    private void OnEnable()
    {
        _statusEffect = serializedObject.FindProperty("_appliedStatusEffect");
        _statusEffectTypes = Utility.GetSubclassesOf(typeof(EnemyStatusEffect));
    }
    public override void OnInspectorGUI()
    {
        ApplyEnemyStatusOnHitAttackEffect attackEffect = (ApplyEnemyStatusOnHitAttackEffect)target;
        EditorGUILayout.LabelField(attackEffect.AppliedStatusEffect != null ? attackEffect.AppliedStatusEffect.ToString() : "Unassigned status effect", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_statusEffect, true);

        if (GUILayout.Button("Change status effect"))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var type in _statusEffectTypes)
            {
                var isTypeUsable = type.GetProperty("isUsable", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (isTypeUsable == null || !(bool)isTypeUsable.GetValue(null))
                    continue;

                menu.AddItem(new GUIContent(type.Name), false, () =>
                {
                    if (attackEffect.AppliedStatusEffect != null && type == attackEffect.AppliedStatusEffect.GetType())
                        return;
                    attackEffect.AppliedStatusEffect = Activator.CreateInstance(type) as EnemyStatusEffect;
                    EditorUtility.SetDirty(attackEffect);
                });
            }
            menu.ShowAsContext();
        }
    }
}

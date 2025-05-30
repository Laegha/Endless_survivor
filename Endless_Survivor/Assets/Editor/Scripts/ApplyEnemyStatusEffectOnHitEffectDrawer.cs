using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ApplyEnemyStatusOnHitAttackEffect))]
public class ApplyEnemyStatusEffectOnHitEffectDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
        ApplyEnemyStatusOnHitAttackEffect attackEffect = (ApplyEnemyStatusOnHitAttackEffect)property.managedReferenceValue;
        EditorGUILayout.LabelField(attackEffect.AppliedStatusEffect != null ? attackEffect.AppliedStatusEffect.ToString() : "Unassigned status effect", EditorStyles.boldLabel);
        var statusEffect = property.FindPropertyRelative("_appliedStatusEffect");
        EditorGUILayout.PropertyField(statusEffect, true);

        var statusEffectTypes = Utility.GetSubclassesOf(typeof(EnemyStatusEffect));


        if (GUILayout.Button("Change status effect"))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var type in statusEffectTypes)
            {
                var isTypeUsable = type.GetProperty("isUsable", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (isTypeUsable == null || !(bool)isTypeUsable.GetValue(null))
                    continue;

                menu.AddItem(new GUIContent(type.Name), false, () =>
                {
                    if (attackEffect.AppliedStatusEffect != null && type == attackEffect.AppliedStatusEffect.GetType())
                        return;
                    attackEffect.AppliedStatusEffect = Activator.CreateInstance(type) as EnemyStatusEffect;
                });
            }
            menu.ShowAsContext();
        }
    }
}

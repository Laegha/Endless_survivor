using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponData))]
public class WeaponDataEditor : Editor
{
    SerializedProperty _weaponName;

    SerializedProperty _defaultAttack;
    SerializedProperty _additionalAttacks;
    SerializedProperty _attackChangeConditions;

    SerializedProperty _weaponStats;
    SerializedProperty _rangeIncreaseScale;
    SerializedProperty _attackSpeedIncreaseScale;
    SerializedProperty _damageIncreaseScale;

    SerializedProperty _weaponTags;
    SerializedProperty _weaponPools;

    SerializedProperty _weaponDisplaySprite;
    SerializedProperty _rendererOrderOffset;
    SerializedProperty _idleAnimation;
    SerializedProperty _randomIdleAnimations;
    SerializedProperty _randomIdleAnimChance;
    SerializedProperty _randomIdleAnimTime;

    List<Type> _attackControllerTypes = new List<Type>();
    List<Type> _changeConditionTypes = new List<Type>();
    private void OnEnable()
    {
        _weaponName = serializedObject.FindProperty("_weaponName");

        _defaultAttack = serializedObject.FindProperty("_defaultAttack");
        _additionalAttacks = serializedObject.FindProperty("_weaponAttacks");
        _attackChangeConditions = serializedObject.FindProperty("_attackConditions");

        _weaponStats = serializedObject.FindProperty("_weaponStats");

        _rangeIncreaseScale = serializedObject.FindProperty("_rangeIncrease");
        _attackSpeedIncreaseScale = serializedObject.FindProperty("_attackSpeedIncrease");
        _damageIncreaseScale = serializedObject.FindProperty("_damageIncrease");

        _weaponTags = serializedObject.FindProperty("_weaponTags");
        _weaponPools = serializedObject.FindProperty("_weaponPools");

        _weaponDisplaySprite = serializedObject.FindProperty("_weaponDisplaySprite");
        _rendererOrderOffset = serializedObject.FindProperty("_spriteRenderOrderOffset");
        _idleAnimation = serializedObject.FindProperty("_idleAnimation");
        _randomIdleAnimations = serializedObject.FindProperty("_randomIdleAnimations");
        _randomIdleAnimChance = serializedObject.FindProperty("_randomIdleAnimChance");
        _randomIdleAnimTime = serializedObject.FindProperty("_randomIdleAnimTime");
        _attackControllerTypes = Utility.GetSubclassesOf(typeof(WeaponAttackController));
        _changeConditionTypes = Utility.GetSubclassesOf(typeof(WeaponAttackChangeCondition));

    }

    public override void OnInspectorGUI()
    {
        WeaponData weaponData = (WeaponData)target;
        serializedObject.Update();
        EditorGUILayout.PropertyField(_weaponName);
        string defaultAttackTypeDisplay = weaponData.DefaultAttack == null ? "Undefined type" : weaponData.DefaultAttack.GetType().Name;
        EditorGUILayout.LabelField("Default attack: " + defaultAttackTypeDisplay, EditorStyles.whiteLargeLabel);
        
        EditorGUILayout.PropertyField(_defaultAttack, true);

        if (GUILayout.Button("Change default attack type"))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var type in _attackControllerTypes)
            {
                var isTypeUsable = type.GetProperty("isUsable", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (isTypeUsable == null || !(bool)isTypeUsable.GetValue(null))
                    continue;

                menu.AddItem(new GUIContent(type.Name), false, () =>
                {
                    if (weaponData.DefaultAttack != null && type == weaponData.DefaultAttack.GetType())
                        return;
                    weaponData.DefaultAttack = Activator.CreateInstance(type) as WeaponAttackController;
                    EditorUtility.SetDirty(weaponData);
                    serializedObject.Update();
                });
            }
            menu.ShowAsContext();
        }
        //ATTACKS
        EditorGUILayout.LabelField("Other attacks, triggered by the change conditions", EditorStyles.whiteLargeLabel);
        EditorGUI.indentLevel++;
        for (int i = 0; i < weaponData.WeaponAttacks.Count; i++)
        {
            if (_additionalAttacks.arraySize <= i)
            {
                break;
            }
            SerializedProperty attackProp = _additionalAttacks.GetArrayElementAtIndex(i);
            if (attackProp == null)
                continue;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(attackProp, new GUIContent(weaponData.WeaponAttacks[i].GetType().Name), true);

            if (GUILayout.Button("Remove " + weaponData.WeaponAttacks[i].AttackId))
            {
                weaponData.WeaponAttacks.RemoveAt(i);
                EditorUtility.SetDirty(weaponData);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUI.indentLevel--;


        if (GUILayout.Button("Add attack"))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var type in _attackControllerTypes)
            {
                var isTypeUsable = type.GetProperty("isUsable", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (isTypeUsable == null || !(bool)isTypeUsable.GetValue(null))
                    continue;

                menu.AddItem(new GUIContent(type.Name), false, () =>
                {
                    var addedAttack = Activator.CreateInstance(type) as WeaponAttackController;
                    weaponData.WeaponAttacks.Add(addedAttack);
                    EditorUtility.SetDirty(weaponData);
                });
            }
            menu.ShowAsContext();
        }
        //CHANGE CONDITIONS
        EditorGUILayout.LabelField("Conditions under which attacks change", EditorStyles.whiteLargeLabel);
        EditorGUI.indentLevel++;
        for (int i = 0; i < weaponData.AttackConditions.Count; i++)
        {
            if (_attackChangeConditions.arraySize <= i)
            {
                break;
            }
            SerializedProperty conditionProp = _attackChangeConditions.GetArrayElementAtIndex(i);
            if (conditionProp == null)
                continue;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(conditionProp, new GUIContent(weaponData.AttackConditions[i].GetType().Name), true);

            if (GUILayout.Button("Remove " + weaponData.AttackConditions[i].GetType()))
            {
                weaponData.AttackConditions.RemoveAt(i);
                EditorUtility.SetDirty(weaponData);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUI.indentLevel--;
        if (GUILayout.Button("Add change condition"))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var type in _changeConditionTypes)
            {
                var isTypeUsable = type.GetProperty("isUsable", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (isTypeUsable == null || !(bool)isTypeUsable.GetValue(null))
                    continue;

                menu.AddItem(new GUIContent(type.Name), false, () =>
                {
                    var addedCondition = Activator.CreateInstance(type) as WeaponAttackChangeCondition;
                    weaponData.AttackConditions.Add(addedCondition);
                    EditorUtility.SetDirty(weaponData);
                });
            }
            menu.ShowAsContext();
        }

        EditorGUILayout.Space(50);

        EditorGUILayout.LabelField("Base stats for the weapon", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_weaponStats);
        EditorGUILayout.LabelField("Multipliers to the function that increases stats by weapon level.", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_rangeIncreaseScale);
        EditorGUILayout.PropertyField(_attackSpeedIncreaseScale);
        EditorGUILayout.PropertyField(_damageIncreaseScale);

        EditorGUILayout.LabelField("Weapons with similar tags are more likely to spawn during runs", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_weaponTags);

        EditorGUILayout.LabelField("Some enemies will drop only weapons of certain pools", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_weaponPools);

        EditorGUILayout.PropertyField(_weaponDisplaySprite, true);
        EditorGUILayout.PropertyField(_rendererOrderOffset, true);
        EditorGUILayout.LabelField("Animations");
        EditorGUILayout.PropertyField(_idleAnimation, true);

        EditorGUILayout.LabelField("This are animations that may trigger while idle. Leave blank if none should");
        EditorGUILayout.PropertyField(_randomIdleAnimations, true);
        EditorGUILayout.PropertyField(_randomIdleAnimChance, true);
        EditorGUILayout.PropertyField(_randomIdleAnimTime, true);

        serializedObject.ApplyModifiedProperties();
    }
}

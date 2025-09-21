using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponData))]
public class WeaponDataEditor : Editor
{
    SerializedProperty _weaponName;
    SerializedProperty _weaponType;
    SerializedProperty _weaponTransfer;
    SerializedProperty _weaponStats;
    SerializedProperty _statsIncreaseScale;
    SerializedProperty _weaponAttackEffects;
    SerializedProperty _weaponTags;
    SerializedProperty _weaponPools;
    SerializedProperty _weaponDisplaySprite;
    SerializedProperty _idleAnimation;
    SerializedProperty _attackAnimation;
    SerializedProperty _randomIdleAnimations;
    SerializedProperty _randomIdleAnimChance;
    SerializedProperty _randomIdleAnimTime;
    WeaponDataTransferInterface tempInstance;

    List<Type> _interfaceTypes = new List<Type>();
    private void OnEnable()
    {
        _weaponName = serializedObject.FindProperty("_weaponName");
        _weaponType = serializedObject.FindProperty("_weaponType");
        _weaponTransfer = serializedObject.FindProperty("_weaponDataTransferInterface");
        _weaponStats = serializedObject.FindProperty("_weaponStats");
        _statsIncreaseScale = serializedObject.FindProperty("_statsIncreaseScale");
        _weaponAttackEffects = serializedObject.FindProperty("_attackEffects");
        _weaponTags = serializedObject.FindProperty("_weaponTags");
        _weaponPools = serializedObject.FindProperty("_weaponPools");
        _weaponDisplaySprite = serializedObject.FindProperty("_weaponDisplaySprite");
        _idleAnimation = serializedObject.FindProperty("_idleAnimation");
        _attackAnimation = serializedObject.FindProperty("_attackAnimation");
        _randomIdleAnimations = serializedObject.FindProperty("_randomIdleAnimations");
        _randomIdleAnimChance = serializedObject.FindProperty("_randomIdleAnimChance");
        _randomIdleAnimTime = serializedObject.FindProperty("_randomIdleAnimTime");

        WeaponData weaponData = (WeaponData)target;
        tempInstance = weaponData.WeaponDataTransferInterface;
        _interfaceTypes = Utility.GetSubclassesOf(typeof(WeaponDataTransferInterface));
    }

    public override void OnInspectorGUI()
    {
        WeaponData weaponData = (WeaponData)target;
        serializedObject.Update();
        EditorGUILayout.PropertyField(_weaponName);

        EditorGUILayout.LabelField(weaponData.WeaponDataTransferInterface != null ? weaponData.WeaponDataTransferInterface.ToString() : "Unassigned weapon type", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_weaponTransfer, true);

        if(GUILayout.Button("Change weapon type"))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var type in _interfaceTypes)
            {
                var isTypeUsable = type.GetProperty("isUsable", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (isTypeUsable == null || !(bool)isTypeUsable.GetValue(null))
                    continue;

                menu.AddItem(new GUIContent(type.Name), false, () =>
                {
                    if (weaponData.WeaponDataTransferInterface != null && type == weaponData.WeaponDataTransferInterface.GetType())
                        return;
                    weaponData.WeaponDataTransferInterface = Activator.CreateInstance(type) as WeaponDataTransferInterface;
                    EditorUtility.SetDirty(weaponData);
                });
            }
            menu.ShowAsContext();
        }
        
        EditorGUILayout.LabelField("Base stats for the weapon", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_weaponStats);
        EditorGUILayout.LabelField("Multipliers to the function that increases stats by weapon level. KNOCKBACK DOESN'T INCREASE so changing its multiplier is useless", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_statsIncreaseScale);
        EditorGUILayout.PropertyField(_weaponAttackEffects);

        EditorGUILayout.LabelField("Weapons with similar tags are more likely to spawn during runs", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_weaponTags);

        EditorGUILayout.LabelField("Some enemies will drop only weapons of certain pools", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_weaponPools);

        EditorGUILayout.LabelField("Animations");
        EditorGUILayout.PropertyField(_weaponDisplaySprite, true);
        EditorGUILayout.PropertyField(_idleAnimation, true);
        EditorGUILayout.PropertyField(_attackAnimation, true);

        EditorGUILayout.LabelField("This are animations that may trigger while idle. Leave blank if none should");
        EditorGUILayout.PropertyField(_randomIdleAnimations,true);
        EditorGUILayout.PropertyField(_randomIdleAnimChance, true);
        EditorGUILayout.PropertyField(_randomIdleAnimTime, true);

        serializedObject.ApplyModifiedProperties();
    }
}

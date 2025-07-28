using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponData))]
public class WeaponDataEditor : Editor
{
    SerializedProperty _weaponType;
    SerializedProperty _weaponTransfer;
    SerializedProperty _weaponStats;
    SerializedProperty _statsIncreaseScale;
    SerializedProperty _weaponTags;
    SerializedProperty _weaponPools;
    SerializedProperty _weaponDisplaySprite;
    SerializedProperty _idleAnimation;
    SerializedProperty _attackAnimation;
    WeaponDataTransferInterface tempInstance;

    List<Type> _interfaceTypes = new List<Type>();
    private void OnEnable()
    {
        _weaponType = serializedObject.FindProperty("_weaponType");
        _weaponTransfer = serializedObject.FindProperty("_weaponDataTransferInterface");
        _weaponStats = serializedObject.FindProperty("_weaponStats");
        _statsIncreaseScale = serializedObject.FindProperty("_statsIncreaseScale");
        _weaponTags = serializedObject.FindProperty("_weaponTags");
        _weaponPools = serializedObject.FindProperty("_weaponPools");
        _weaponDisplaySprite = serializedObject.FindProperty("_weaponDisplaySprite");
        _idleAnimation = serializedObject.FindProperty("_idleAnimation");
        _attackAnimation = serializedObject.FindProperty("_attackAnimation");
        WeaponData weaponData = (WeaponData)target;
        tempInstance = weaponData.WeaponDataTransferInterface;
        _interfaceTypes = Utility.GetSubclassesOf(typeof(WeaponDataTransferInterface));
    }

    public override void OnInspectorGUI()
    {
        WeaponData weaponData = (WeaponData)target;
        serializedObject.Update();

        EditorGUILayout.LabelField(weaponData.WeaponDataTransferInterface != null ? weaponData.WeaponDataTransferInterface.ToString() : "Unassigned weapon type", EditorStyles.boldLabel);

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
                    if (type == weaponData.WeaponDataTransferInterface.GetType())
                        return;
                    weaponData.WeaponDataTransferInterface = Activator.CreateInstance(type) as WeaponDataTransferInterface;
                    EditorUtility.SetDirty(weaponData);
                });
            }
            menu.ShowAsContext();
        }
        EditorGUILayout.PropertyField(_weaponTransfer, true);

        EditorGUILayout.PropertyField(_weaponStats);
        EditorGUILayout.PropertyField(_statsIncreaseScale);
        EditorGUILayout.PropertyField(_weaponTags);
        EditorGUILayout.PropertyField(_weaponPools);

        EditorGUILayout.LabelField("Animations");
        EditorGUILayout.PropertyField(_weaponDisplaySprite, true);
        EditorGUILayout.PropertyField(_idleAnimation, true);
        EditorGUILayout.PropertyField(_attackAnimation, true);

        serializedObject.ApplyModifiedProperties();
    }
}

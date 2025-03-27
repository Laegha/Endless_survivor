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
    SerializedProperty _idleAnimation;
    SerializedProperty _attackAnimation;
    WeaponDataTransferInterface tempInstance;
    private void OnEnable()
    {
        _weaponType = serializedObject.FindProperty("_weaponType");
        _weaponTransfer = serializedObject.FindProperty("_weaponDataTransferInterface");
        _weaponStats = serializedObject.FindProperty("_weaponStats");
        _statsIncreaseScale = serializedObject.FindProperty("_statsIncreaseScale");
        _weaponTags = serializedObject.FindProperty("_weaponTags");
        _idleAnimation = serializedObject.FindProperty("_idleAnimation");
        _attackAnimation = serializedObject.FindProperty("_attackAnimation");
        WeaponData weaponData = (WeaponData)target;
        tempInstance = weaponData.WeaponDataTransferInterface;
    }

    public override void OnInspectorGUI()
    {
        WeaponData weaponData = (WeaponData)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(_weaponType);

        if(_weaponType.enumValueIndex != (int)weaponData.WeaponType)
        { 
            serializedObject.ApplyModifiedProperties();
            WeaponDataTransferInterface newInterface = null;
            if(weaponData.WeaponType == WeaponData.IWeaponType.Ray)
            {
                newInterface = new RayWeaponDataTransferInterface();
                
            }
            else if(weaponData.WeaponType == WeaponData.IWeaponType.Proyectile)
            {
                newInterface = new ProyectileWeaponDataTransferInterface();
            }
            else
            {
                newInterface = new WeaponDataTransferInterface();
            }
            //add proyectile and custom
            weaponData.WeaponDataTransferInterface = newInterface;
            serializedObject.ApplyModifiedProperties();
        }
        EditorGUILayout.PropertyField(_weaponTransfer, true);

        EditorGUILayout.PropertyField(_weaponStats);
        EditorGUILayout.PropertyField(_statsIncreaseScale);
        EditorGUILayout.PropertyField(_weaponTags);

        EditorGUILayout.LabelField("Animations");
        EditorGUILayout.PropertyField(_idleAnimation, true);
        EditorGUILayout.PropertyField(_attackAnimation, true);

        serializedObject.ApplyModifiedProperties();
    }
}

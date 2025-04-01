using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyData))]
public class EnemyDataEditor : Editor
{
    Dictionary<Type, bool> _behaviourTypes = new Dictionary<Type, bool>();

    SerializedProperty _initialHP;
    SerializedProperty _colliderSize;
    SerializedProperty _colliderOffset;
    SerializedProperty _colliderDirection;
    SerializedProperty _behaviours;
    SerializedProperty _weaponDropChance;
    SerializedProperty _passiveDropChance;
    SerializedProperty _statBoostDropChance;
    private void OnEnable()
    {
        _initialHP = serializedObject.FindProperty("_initialHP");
        _colliderSize = serializedObject.FindProperty("_colliderSize");
        _colliderOffset = serializedObject.FindProperty("_colliderOffset");
        _colliderDirection = serializedObject.FindProperty("_colliderDirection");
        _behaviours = serializedObject.FindProperty("_enemyBehaviours");
        _weaponDropChance = serializedObject.FindProperty("_weaponDropChance");
        _passiveDropChance = serializedObject.FindProperty("_passiveDropChance");
        _statBoostDropChance = serializedObject.FindProperty("_statBoostDropChance");
        List<Type> behaviourTypes = Utility.GetSubclassesOf(typeof(EnemyBehaviour));
        EnemyData enemyData = (EnemyData)target;
        behaviourTypes.ForEach(type => _behaviourTypes.Add(type, enemyData.EnemyBehaviours.Exists(behaviour => behaviour.GetType() == type)));
        enemyData.EnemyBehaviours.ForEach(behaviour => behaviour.EnemyData = enemyData);
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(_initialHP);
        EditorGUILayout.PropertyField(_behaviours);
        serializedObject.Update();

        EnemyData enemyData = (EnemyData)target;

        SyncBehaviourTypes(enemyData);

        if (GUILayout.Button("Add Behaviour"))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var type in _behaviourTypes)
            {
                if (type.Value)
                    continue;

                menu.AddItem(new GUIContent(type.Key.Name), false, () =>
                {
                    EnemyBehaviour newBehaviour = (EnemyBehaviour)Activator.CreateInstance(type.Key);
                    enemyData.EnemyBehaviours.Add(newBehaviour);
                    newBehaviour.EnemyData = enemyData;
                    EditorUtility.SetDirty(enemyData);
                });
            }
            menu.ShowAsContext();
        }

        EditorGUILayout.PropertyField(_colliderSize);
        EditorGUILayout.PropertyField(_colliderOffset);
        EditorGUILayout.PropertyField(_colliderDirection);

        GUILayout.Label("Weapon drop chance");
        _weaponDropChance.floatValue = Mathf.Clamp(EditorGUILayout.Slider(enemyData.WeaponDropChance, 0, 100), 0, 100 - enemyData.PassiveDropChance - enemyData.StatBoostDropChance);
        GUILayout.Label("Passive item drop chance");
        _passiveDropChance.floatValue = Mathf.Clamp(EditorGUILayout.Slider(enemyData.PassiveDropChance, 0, 100), 0, 100 - enemyData.WeaponDropChance - enemyData.StatBoostDropChance);
        GUILayout.Label("Stat boost drop chance");
        _statBoostDropChance.floatValue = Mathf.Clamp(EditorGUILayout.Slider(enemyData.StatBoostDropChance, 0, 100), 0, 100 - enemyData.WeaponDropChance - enemyData.PassiveDropChance);
        
        serializedObject.ApplyModifiedProperties();

    }

    private void SyncBehaviourTypes(EnemyData enemyData)
    {
        // Reiniciar el diccionario antes de actualizar
        foreach (var key in new List<Type>(_behaviourTypes.Keys))
        {
            _behaviourTypes[key] = false;
        }

        // Marcar los tipos presentes en la lista actual
        foreach (var behaviour in enemyData.EnemyBehaviours)
        {
            if(behaviour == null) continue;
            _behaviourTypes[behaviour.GetType()] = true;
        }
    }
}

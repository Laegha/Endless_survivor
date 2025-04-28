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
    SerializedProperty _dropablePickupChances;
    SerializedProperty _onHitSFX;
    SerializedProperty _onDeathSFX;
    private void OnEnable()
    {
        _initialHP = serializedObject.FindProperty("_initialHP");
        _colliderSize = serializedObject.FindProperty("_colliderSize");
        _colliderOffset = serializedObject.FindProperty("_colliderOffset");
        _colliderDirection = serializedObject.FindProperty("_colliderDirection");
        _behaviours = serializedObject.FindProperty("_enemyBehaviours");
        _dropablePickupChances = serializedObject.FindProperty("_dropablePickupChances");
        _onHitSFX = serializedObject.FindProperty("_onHitSFX");
        _onDeathSFX = serializedObject.FindProperty("_onDeathSFX");
        List<Type> behaviourTypes = Utility.GetSubclassesOf(typeof(EnemyBehaviour));
        EnemyData enemyData = (EnemyData)target;
        behaviourTypes.ForEach(type => _behaviourTypes.Add(type, enemyData.EnemyBehaviours.Exists(behaviour => behaviour.GetType() == type)));
        if (enemyData.EnemyBehaviours == null)
            enemyData.EnemyBehaviours = new List<EnemyBehaviour>();
        enemyData.EnemyBehaviours.ForEach(behaviour => behaviour.EnemyData = enemyData);
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(_initialHP);
        EditorGUILayout.PropertyField(_colliderSize);
        EditorGUILayout.PropertyField(_colliderOffset);
        EditorGUILayout.PropertyField(_colliderDirection);
        EditorGUILayout.PropertyField(_onHitSFX);
        EditorGUILayout.PropertyField(_onDeathSFX);

        EditorGUILayout.PropertyField(_behaviours);
        serializedObject.ApplyModifiedProperties();
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


        EditorGUILayout.PropertyField(_dropablePickupChances);
        foreach (var pickupData in enemyData.DropablePickupChances)
        {
            if (pickupData.PickupData == null) 
                continue;
            GUILayout.Label(pickupData.PickupData.name, EditorStyles.boldLabel);
            int maxPercentage = 100;
            foreach (var pickupDataChance in enemyData.DropablePickupChances)
            {
                if (pickupDataChance == pickupData)
                    continue;
                maxPercentage -= pickupDataChance.Chance;
            }
            pickupData.Chance = (int)Mathf.Clamp(EditorGUILayout.Slider(pickupData.Chance, 0, 100), 0, maxPercentage);
        }

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

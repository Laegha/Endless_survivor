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
    SerializedProperty _dropablePickups;
    private void OnEnable()
    {
        _initialHP = serializedObject.FindProperty("_initialHP");
        _colliderSize = serializedObject.FindProperty("_colliderSize");
        _colliderOffset = serializedObject.FindProperty("_colliderOffset");
        _colliderDirection = serializedObject.FindProperty("_colliderDirection");
        _behaviours = serializedObject.FindProperty("_enemyBehaviours");
        _dropablePickups = serializedObject.FindProperty("_dropablePickups");
        List<Type> behaviourTypes = Utility.GetSubclassesOf(typeof(EnemyBehaviour));
        EnemyData enemyData = (EnemyData)target;
        behaviourTypes.ForEach(type => _behaviourTypes.Add(type, enemyData.EnemyBehaviours.Exists(behaviour => behaviour.GetType() == type)));
        enemyData.EnemyBehaviours.ForEach(behaviour => behaviour.EnemyData = enemyData);
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(_initialHP);
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

        EditorGUILayout.PropertyField(_colliderSize);
        EditorGUILayout.PropertyField(_colliderOffset);
        EditorGUILayout.PropertyField(_colliderDirection);

        SyncPickupDatas(enemyData);
        EditorGUILayout.PropertyField(_dropablePickups);
        foreach(var pickupData in enemyData.DropablePickups)
        {
            if(pickupData == null) continue;
            GUILayout.Label(pickupData.name, EditorStyles.boldLabel);
            int maxPercentage = 100;
            foreach(var pickupDataChance in enemyData.DropablePickupsChances)
            {
                if(pickupDataChance.Key == pickupData)
                    continue;
                maxPercentage -= pickupDataChance.Value;
            }
            enemyData.DropablePickupsChances[pickupData] = (int)Mathf.Clamp(EditorGUILayout.Slider(enemyData.DropablePickupsChances[pickupData], 0, 100), 0, maxPercentage);
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
    void SyncPickupDatas(EnemyData enemyData)
    {
        //add missing items to dict
        foreach(var pickupData in enemyData.DropablePickups)
        {
            if (enemyData.DropablePickupsChances.ContainsKey(pickupData))
                continue;
            enemyData.DropablePickupsChances.Add(pickupData, 0);
        }
        //get all the leftover items from dict
        List<PickupData> removablePickupDatas = new List<PickupData>();
        foreach(var pickupDataChance in enemyData.DropablePickupsChances)
        {
            if(enemyData.DropablePickups.Contains(pickupDataChance.Key)) 
                continue;
            removablePickupDatas.Add(pickupDataChance.Key);
        }
        //remove the leftover items from dict
        foreach(var removedPickupData in removablePickupDatas)
        {
            enemyData.DropablePickupsChances.Remove(removedPickupData);
        }
    }
}

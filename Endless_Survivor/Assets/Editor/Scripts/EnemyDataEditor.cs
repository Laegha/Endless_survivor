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
    SerializedProperty _referenceSizeSprite;
    SerializedProperty _knockbackResistance;
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
        _referenceSizeSprite = serializedObject.FindProperty("_referenceSizeSprite");
        _knockbackResistance = serializedObject.FindProperty("_knockbackResistance");
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
        EditorGUILayout.PropertyField(_referenceSizeSprite);
        EditorGUILayout.PropertyField(_knockbackResistance);
        EditorGUILayout.PropertyField(_colliderSize);
        EditorGUILayout.PropertyField(_colliderOffset);
        EditorGUILayout.PropertyField(_colliderDirection);
        EditorGUILayout.PropertyField(_onHitSFX);
        EditorGUILayout.PropertyField(_onDeathSFX);

        EditorGUILayout.LabelField("Enemy Behaviours");
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

        serializedObject.ApplyModifiedProperties();

        //EditorGUILayout.PropertyField(_dropablePickupChances);
        EditorGUILayout.LabelField("Pickups dropped on death");
        RouletteElementChance<PickupData> removedDataChance = null;
        for (int i = 0; i < enemyData.DropablePickupChances.Count; i++)
        {
            var pickupChance = enemyData.DropablePickupChances[i];

            string pickupLabel = pickupChance.Element == null ? "Undefined pickup" : pickupChance.Element.name;
            GUILayout.Label(pickupLabel, EditorStyles.boldLabel);

            EditorGUI.indentLevel++;
            SerializedProperty pickupProperty = _dropablePickupChances.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(pickupProperty.FindPropertyRelative("_element"));
            SerializedProperty chanceProperty = pickupProperty.FindPropertyRelative("_chance");
            EditorGUILayout.BeginHorizontal();
            int maxPercentage = 100;
            foreach (var otherPickupChance in enemyData.DropablePickupChances)
            {
                if (otherPickupChance == pickupChance)
                    continue;
                maxPercentage -= otherPickupChance.Chance;
            }
            chanceProperty.intValue = Mathf.Clamp(EditorGUILayout.IntSlider("Chance", chanceProperty.intValue, 0, 100), 0, maxPercentage);
            //pickupChance.Chance = (int)Mathf.Clamp(EditorGUILayout.Slider(pickupChance.Chance, 0, 100), 0, maxPercentage);
            if (GUILayout.Button("Remove dropable pickup"))
            {
                removedDataChance = pickupChance;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
            serializedObject.ApplyModifiedProperties();
        }
        if (removedDataChance != null)
            enemyData.DropablePickupChances.Remove(removedDataChance);
        if (GUILayout.Button("Add dropable pickup"))
        {
            enemyData.DropablePickupChances.Add(new RouletteElementChance<PickupData>(null, 0));
        }

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
            if (behaviour == null) continue;
            _behaviourTypes[behaviour.GetType()] = true;
        }
    }
}

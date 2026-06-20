using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyData))]
public class EnemyDataEditor : Editor
{
    Dictionary<Type, int> _behaviourTypes = new Dictionary<Type, int>();
    bool _showBehaviourTypes;

    SerializedProperty _initialHP;
    SerializedProperty _referenceSizeSprite;
    SerializedProperty _renderSortOrder;
    SerializedProperty _knockbackResistance;
    SerializedProperty _colliderSize;
    SerializedProperty _colliderOffset;
    SerializedProperty _colliderDirection;
    SerializedProperty _rigidbodyType;
    SerializedProperty _behaviours;
    SerializedProperty _dropablePickupChances;
    SerializedProperty _onHitSFX;
    SerializedProperty _onDeathSFX;
    private void OnEnable()
    {
        _initialHP = serializedObject.FindProperty("_initialHP");
        _referenceSizeSprite = serializedObject.FindProperty("_referenceSizeSprite");
        _renderSortOrder = serializedObject.FindProperty("_renderSortingOffset");
        _knockbackResistance = serializedObject.FindProperty("_knockbackResistance");
        _colliderSize = serializedObject.FindProperty("_colliderSize");
        _colliderOffset = serializedObject.FindProperty("_colliderOffset");
        _colliderDirection = serializedObject.FindProperty("_colliderDirection");
        _rigidbodyType = serializedObject.FindProperty("_rigidbodyType");
        _behaviours = serializedObject.FindProperty("_enemyBehaviours");
        _dropablePickupChances = serializedObject.FindProperty("_dropablePickupChances");
        _onHitSFX = serializedObject.FindProperty("_onHitSFX");
        _onDeathSFX = serializedObject.FindProperty("_onDeathSFX");
        List<Type> behaviourTypes = Utility.GetSubclassesOf(typeof(EnemyBehaviour));
        EnemyData enemyData = (EnemyData)target;
        behaviourTypes.ForEach(type => _behaviourTypes.Add(type, enemyData.EnemyBehaviours.Where(behaviour => behaviour.GetType() == type).Count()));
        if (enemyData.EnemyBehaviours == null)
            enemyData.EnemyBehaviours = new List<EnemyBehaviour>();
        enemyData.EnemyBehaviours.ForEach(behaviour => behaviour.EnemyData = enemyData);
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(_initialHP);
        EditorGUILayout.PropertyField(_referenceSizeSprite);
        EditorGUILayout.PropertyField(_renderSortOrder);
        EditorGUILayout.PropertyField(_knockbackResistance);
        EditorGUILayout.PropertyField(_colliderSize);
        EditorGUILayout.PropertyField(_colliderOffset);
        EditorGUILayout.PropertyField(_colliderDirection);
        EditorGUILayout.PropertyField(_rigidbodyType);
        EditorGUILayout.PropertyField(_onHitSFX);
        EditorGUILayout.PropertyField(_onDeathSFX);

        EnemyData enemyData = (EnemyData)target;

        _showBehaviourTypes = EditorGUILayout.ToggleLeft("Show behaviour types", _showBehaviourTypes);

        EditorGUILayout.LabelField("Enemy Behaviours");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(_behaviours);
        if (_showBehaviourTypes)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space(25);
            foreach (var behaviour in enemyData.EnemyBehaviours)
            {
                EditorGUILayout.LabelField(behaviour.GetType().Name);
            }
            EditorGUILayout.EndVertical();

        }
        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();


        SyncBehaviourTypes(enemyData);

        if (GUILayout.Button("Add Behaviour"))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var type in _behaviourTypes)
            {
                int behaviourCurrStacks = type.Value;

                var behaviourMaxStacksProperty = type.Key.GetProperty("maxStacks", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                int behaviourMaxStacks = behaviourMaxStacksProperty == null ? 0 : (int)behaviourMaxStacksProperty.GetValue(null);
                if (behaviourMaxStacksProperty == null || behaviourMaxStacks >= 0 && behaviourMaxStacks <= behaviourCurrStacks)
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


        //EditorGUILayout.PropertyField(_dropablePickupChances);
        EditorGUILayout.LabelField("Pickups dropped on death");
        EditorGUILayout.PropertyField(_dropablePickupChances);
        serializedObject.ApplyModifiedProperties();
        //RouletteElementChance<PickupData> removedDataChance = null;
        //for (int i = 0; i < enemyData.DropablePickupChances.Count; i++)
        //{
        //    var pickupChance = enemyData.DropablePickupChances[i];

        //    string pickupLabel = pickupChance.RouletteElement == null ? "Undefined pickup" : pickupChance.RouletteElement.name;
        //    GUILayout.Label(pickupLabel, EditorStyles.boldLabel);

        //    EditorGUI.indentLevel++;
        //    SerializedProperty pickupProperty = _dropablePickupChances.GetArrayElementAtIndex(i);
        //    EditorGUILayout.PropertyField(pickupProperty.FindPropertyRelative("_rouletteElement"));
        //    SerializedProperty chanceProperty = pickupProperty.FindPropertyRelative("_chance");
        //    EditorGUILayout.BeginHorizontal();
        //    int maxPercentage = 100;
        //    foreach (var otherPickupChance in enemyData.DropablePickupChances)
        //    {
        //        if (otherPickupChance == pickupChance)
        //            continue;
        //        maxPercentage -= otherPickupChance.Chance;
        //    }
        //    chanceProperty.intValue = Mathf.Clamp(EditorGUILayout.IntSlider("Chance", chanceProperty.intValue, 0, 100), 0, maxPercentage);
        //    //pickupChance.Chance = (int)Mathf.Clamp(EditorGUILayout.Slider(pickupChance.Chance, 0, 100), 0, maxPercentage);
        //    if (GUILayout.Button("Remove dropable pickup"))
        //    {
        //        removedDataChance = pickupChance;
        //    }
        //    EditorGUILayout.EndHorizontal();
        //    EditorGUI.indentLevel--;
        //    serializedObject.ApplyModifiedProperties();
        //}
        //if (removedDataChance != null)
        //    enemyData.DropablePickupChances.Remove(removedDataChance);
        //if (GUILayout.Button("Add dropable pickup"))
        //{
        //    enemyData.DropablePickupChances.Add(new RouletteElementChance<PickupData>(null, 0));
        //}

    }

    private void SyncBehaviourTypes(EnemyData enemyData)
    {
        // Reiniciar el diccionario antes de actualizar
        foreach (var key in new List<Type>(_behaviourTypes.Keys))
        {
            _behaviourTypes[key]--;
        }

        // Marcar los tipos presentes en la lista actual
        foreach (var behaviour in enemyData.EnemyBehaviours)
        {
            if (behaviour == null) continue;
            _behaviourTypes[behaviour.GetType()]++;
        }
    }
}

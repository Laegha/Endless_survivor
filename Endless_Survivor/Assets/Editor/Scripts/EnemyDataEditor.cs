using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyData))]
public class EnemyDataEditor : Editor
{
    Dictionary<Type, bool> _behaviourTypes = new Dictionary<Type, bool>();
    
    SerializedProperty _behaviours;
    private void OnEnable()
    {
        _behaviours = serializedObject.FindProperty("_enemyBehaviours");
        List<Type> behaviourTypes = Utility.GetSubclassesOf(typeof(EnemyBehaviour));
        EnemyData enemyData = (EnemyData)target;
        behaviourTypes.ForEach(type => _behaviourTypes.Add(type, enemyData.EnemyBehaviours.Exists(behaviour => behaviour.GetType() == type)));
        enemyData.EnemyBehaviours.ForEach(behaviour => behaviour.EnemyData = enemyData);
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
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

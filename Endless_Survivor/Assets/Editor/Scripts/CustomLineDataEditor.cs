using Codice.CM.Client.Differences.Graphic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LineXData))]
public class CustomLineDataEditor : Editor
{
    Dictionary<Type, int> _modifierTypes = new Dictionary<Type, int>();
    bool _showModifierTypes;

    SerializedProperty _tileInfo;
    SerializedProperty _lineMaterial;
    //SerializedProperty _lineWidth;
    //SerializedProperty _lineWidthMultiplier;
    SerializedProperty _lineRenderOffset;
    SerializedProperty _lineStartPointAnimation;
    SerializedProperty _lineEndPointAnimation;
    SerializedProperty _lineModifiers;

    private void OnEnable()
    {
        _tileInfo = serializedObject.FindProperty("_tileInfo");
        _lineMaterial = serializedObject.FindProperty("_lineMaterial");
        _lineRenderOffset = serializedObject.FindProperty("_lineRenderOffset");
        _lineStartPointAnimation = serializedObject.FindProperty("_lineStartPointAnimation");
        _lineEndPointAnimation = serializedObject.FindProperty("_lineEndPointAnimation");
        _lineModifiers = serializedObject.FindProperty("_lineModifiers");

        List<Type> modifierTypes = Utility.GetSubclassesOf(typeof(EnemyBehaviour));
        LineXData lineData = (LineXData)target;
        modifierTypes.ForEach(type => _modifierTypes.Add(type, lineData.LineModifiers.Where(behaviour => behaviour.GetType() == type).Count()));
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(_tileInfo);
        EditorGUILayout.PropertyField(_lineMaterial);
        EditorGUILayout.PropertyField(_lineRenderOffset);
        EditorGUILayout.PropertyField(_lineStartPointAnimation);
        EditorGUILayout.PropertyField(_lineEndPointAnimation);

        LineXData lineData= (LineXData)target;

        _showModifierTypes = EditorGUILayout.ToggleLeft("Show modifier types", _showModifierTypes);

        EditorGUILayout.LabelField("Line Modifiers");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(_lineModifiers);
        if (_showModifierTypes)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space(25);
            foreach (var modifier in lineData.LineModifiers)
            {
                EditorGUILayout.LabelField(modifier.GetType().Name);
            }
            EditorGUILayout.EndVertical();

        }
        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();


        SyncBehaviourTypes(lineData);

        if (GUILayout.Button("Add Modifier"))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var type in _modifierTypes)
            {
                int modifierCurrStacks = type.Value;

                var modifierMaxStacksProperty = type.Key.GetProperty("maxStacks", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                int modifierMaxStacks = modifierMaxStacksProperty == null ? 0 : (int)modifierMaxStacksProperty.GetValue(null);
                if (modifierMaxStacksProperty == null || modifierMaxStacks >= 0 && modifierMaxStacks <= modifierCurrStacks)
                    continue;

                menu.AddItem(new GUIContent(type.Key.Name), false, () =>
                {
                    CustomLineModifierBase newModifier = (CustomLineModifierBase)Activator.CreateInstance(type.Key);
                    lineData.LineModifiers.Add(newModifier);
                    EditorUtility.SetDirty(lineData);
                });
            }
            menu.ShowAsContext();
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void SyncBehaviourTypes(LineXData lineData)
    {
        foreach (var key in new List<Type>(_modifierTypes.Keys))
        {
            _modifierTypes[key]--;
        }

        foreach (var modifier in lineData.LineModifiers)
        {
            if (modifier == null) continue;
            _modifierTypes[modifier.GetType()]++;
        }
    }
}
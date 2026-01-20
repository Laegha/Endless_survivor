using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponInteractionData))]
public class WeaponInteractionDataEditor : Editor
{
    SerializedProperty _weaponsNeededForInteraction;
    Dictionary<Type, int> _interactionTypes = new();
    private void OnEnable()
    {
        _weaponsNeededForInteraction = serializedObject.FindProperty("_weaponsNeededForInteraction");
        List<Type> behaviourTypes = Utility.GetSubclassesOf(typeof(WeaponInteraction));
        WeaponInteractionData weaponInteractionData = (WeaponInteractionData)target;
        behaviourTypes.ForEach(type => _interactionTypes.Add(type, weaponInteractionData.InteractionBeahviours.Where(interaction => interaction.GetType() == type).Count()));
        weaponInteractionData.InteractionBeahviours.RemoveAll(behaviour => behaviour == null);
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(_weaponsNeededForInteraction);
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();

        WeaponInteractionData weaponInteractionData = (WeaponInteractionData)target;
        SerializedProperty interactions = serializedObject.FindProperty("_interactionsBehaviours");

        EditorGUILayout.LabelField("Interactions", EditorStyles.boldLabel);

        serializedObject.Update();
        weaponInteractionData.InteractionBeahviours.RemoveAll(x => x == null);
        for (int i = 0; i < weaponInteractionData.InteractionBeahviours.Count; i++)
        {
            if (interactions.arraySize <= i)
            {
                break;
            }
            SerializedProperty behaviourProp = interactions.GetArrayElementAtIndex(i);
            if (behaviourProp != null)
                EditorGUILayout.PropertyField(behaviourProp, new GUIContent(weaponInteractionData.InteractionBeahviours[i].GetType().Name), true);
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Interaction"))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var type in _interactionTypes)
            {
                int interactionCurrStacks = type.Value;

                var interactionMaxStacksProperty = type.Key.GetProperty("maxStacks", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                int interactionMaxStacks = interactionMaxStacksProperty == null ? 0 : (int)interactionMaxStacksProperty.GetValue(null);
                if (interactionMaxStacksProperty == null || interactionMaxStacks >= 0 && interactionMaxStacks <= interactionCurrStacks)
                    continue;

                menu.AddItem(new GUIContent(type.Key.Name), false, () =>
                {
                    WeaponInteraction newInteraction = (WeaponInteraction)Activator.CreateInstance(type.Key);
                    weaponInteractionData.InteractionBeahviours.Add(newInteraction);
                    _interactionTypes[type.Key]++;
                    EditorUtility.SetDirty(weaponInteractionData);
                });
            }
            menu.ShowAsContext();
        }
        if (GUILayout.Button("Remove Behaviour"))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var interaction in weaponInteractionData.InteractionBeahviours)
            {
                menu.AddItem(new GUIContent(interaction.GetType().Name), false, () =>
                {
                    weaponInteractionData.InteractionBeahviours.Remove(interaction);
                    _interactionTypes[interaction.GetType()]--;
                    EditorUtility.SetDirty(weaponInteractionData);
                });
            }
            menu.ShowAsContext();
        }
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}

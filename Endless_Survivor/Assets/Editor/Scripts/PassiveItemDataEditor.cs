using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PassiveItemData))]
public class PassiveItemDataEditor : Editor
{
    SerializedProperty _itemName;
    SerializedProperty _itemDescript;
    SerializedProperty _itemSprite;
    SerializedProperty _itemPools;
    Dictionary<Type, int> _behaviourTypes = new();
    
    private void OnEnable()
    {
        _itemName = serializedObject.FindProperty("_itemName");
        _itemDescript = serializedObject.FindProperty("_itemDescript");
        _itemSprite = serializedObject.FindProperty("_itemSprite");
        _itemPools = serializedObject.FindProperty("_itemPools");
        List<Type> behaviourTypes = Utility.GetSubclassesOf(typeof(PassiveItemBehaviour));
        PassiveItemData passiveItemData = (PassiveItemData)target;
        behaviourTypes.ForEach(type => _behaviourTypes.Add(type, passiveItemData.ItemBehaviours.Where(behaviour => behaviour.GetType() == type).Count()));
        passiveItemData.ItemBehaviours.RemoveAll(behaviour => behaviour == null);
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(_itemName);
        EditorGUILayout.PropertyField(_itemDescript);
        EditorGUILayout.PropertyField(_itemSprite);
        EditorGUILayout.PropertyField(_itemPools);
        serializedObject.ApplyModifiedProperties();
        PassiveItemData passiveItemData = (PassiveItemData)target;
        SerializedProperty itemBehaviours = serializedObject.FindProperty("_itemBehaviours");

        EditorGUILayout.LabelField("Item Behaviours", EditorStyles.boldLabel);

        serializedObject.Update();
        for (int i = 0; i < passiveItemData.ItemBehaviours.Count; i++)
        {
            if (itemBehaviours.arraySize <= i)
            {
                break;
            }
            SerializedProperty behaviourProp = itemBehaviours.GetArrayElementAtIndex(i);
            if (behaviourProp != null)
                EditorGUILayout.PropertyField(behaviourProp, new GUIContent(passiveItemData.ItemBehaviours[i].GetType().Name), true);
        }

        EditorGUILayout.BeginHorizontal();
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
                    PassiveItemBehaviour newBehaviour = (PassiveItemBehaviour)Activator.CreateInstance(type.Key);
                    passiveItemData.ItemBehaviours.Add(newBehaviour);
                    _behaviourTypes[type.Key] ++;
                    EditorUtility.SetDirty(passiveItemData);
                });
            }
            menu.ShowAsContext();
        }
        if (GUILayout.Button("Remove Behaviour"))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var behaviour in passiveItemData.ItemBehaviours)
            {
                menu.AddItem(new GUIContent(behaviour.GetType().Name), false, () =>
                {
                    passiveItemData.ItemBehaviours.Remove(behaviour);
                    _behaviourTypes[behaviour.GetType()] --;
                    EditorUtility.SetDirty(passiveItemData);
                });
            }
            menu.ShowAsContext();
        }
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }

}

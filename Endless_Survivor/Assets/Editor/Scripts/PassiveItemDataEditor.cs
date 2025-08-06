using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PassiveItemData))]
public class PassiveItemDataEditor : Editor
{
    SerializedProperty _itemName;
    SerializedProperty _itemDescript;
    SerializedProperty _itemSprite;
    SerializedProperty _itemPools;
    Dictionary<Type, bool> _behaviourTypes = new();
    
    private void OnEnable()
    {
        _itemName = serializedObject.FindProperty("_itemName");
        _itemDescript = serializedObject.FindProperty("_itemDescript");
        _itemSprite = serializedObject.FindProperty("_itemSprite");
        _itemPools = serializedObject.FindProperty("_itemPools");
        List<Type> behaviourTypes = Utility.GetSubclassesOf(typeof(PassiveItemBehaviour));
        PassiveItemData passiveItemData = (PassiveItemData)target;
        behaviourTypes.ForEach(type => _behaviourTypes.Add(type, passiveItemData.ItemBehaviours.Exists(behaviour => behaviour.GetType() == type)));
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
                var isTypeUsable = type.Key.GetProperty("isUsable", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (type.Value || isTypeUsable == null || !(bool)isTypeUsable.GetValue(null))
                    continue;

                menu.AddItem(new GUIContent(type.Key.Name), false, () =>
                {
                    PassiveItemBehaviour newBehaviour = (PassiveItemBehaviour)Activator.CreateInstance(type.Key);
                    passiveItemData.ItemBehaviours.Add(newBehaviour);
                    _behaviourTypes[type.Key] = true;
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
                    _behaviourTypes[behaviour.GetType()] = false;
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

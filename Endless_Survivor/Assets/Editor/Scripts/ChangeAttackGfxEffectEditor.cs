using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChangeAttackGfxAttackEffect))]
public class ChangeAttackGfxEffectEditor : Editor
{
    SerializedProperty _gfxInterfaces;
    private void OnEnable()
    {
        _gfxInterfaces = serializedObject.FindProperty("_gfxInterfaces");
        ChangeAttackGfxAttackEffect changeAttackGfxAttackEffect = (ChangeAttackGfxAttackEffect)target;
        var interfacesTypes = Utility.GetSubclassesOf(typeof(AttackGfxInterface));
        foreach (var i in interfacesTypes)
        {
            if (i == typeof(AttackGfxInterface) || changeAttackGfxAttackEffect.GfxInterfaces.Find(x => x.GetType() == i) != null)
                continue;
            changeAttackGfxAttackEffect.GfxInterfaces.Add((AttackGfxInterface)Activator.CreateInstance(i));
        }
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ChangeAttackGfxAttackEffect changeAttackGfxAttackEffect = (ChangeAttackGfxAttackEffect)target;
        for (int i = 0; i < _gfxInterfaces.arraySize; i++)
        {
            var elem = _gfxInterfaces.GetArrayElementAtIndex(i);
            EditorGUILayout.LabelField(changeAttackGfxAttackEffect.GfxInterfaces[i].ToString());
            EditorGUILayout.PropertyField(elem, true);
        }
        serializedObject.ApplyModifiedProperties();
    }
}

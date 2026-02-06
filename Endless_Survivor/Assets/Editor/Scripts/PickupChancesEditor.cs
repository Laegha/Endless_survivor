using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PickupChances))]
public class PickupChancesEditor : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property); 
        if (property.managedReferenceValue == null)
        {
            property.managedReferenceValue = new PickupChances();
            property.serializedObject.ApplyModifiedProperties();
        }
        PickupChances pickupChances = property.managedReferenceValue as PickupChances;
        SerializedProperty _dropablePickupChances = property.FindPropertyRelative("_dropablePickupChances");
        //var pickupChances = new PickupChances();
        //EditorGUILayout.PropertyField(_dropablePickupChances);
        RouletteElementChance<PickupData> removedDataChance = null;
        for (int i = 0; i < pickupChances.DropablePickupChances.Count; i++)
        {
            var pickupChance = pickupChances.DropablePickupChances[i];

            string pickupLabel = pickupChance.RouletteElement == null ? "Undefined pickup" : pickupChance.RouletteElement.name;
            GUILayout.Label(pickupLabel, EditorStyles.boldLabel);

            EditorGUI.indentLevel++;
            SerializedProperty pickupProperty = _dropablePickupChances.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(pickupProperty.FindPropertyRelative("_element"));
            SerializedProperty chanceProperty = pickupProperty.FindPropertyRelative("_chance");
            EditorGUILayout.BeginHorizontal();
            int maxPercentage = 100;
            foreach (var otherPickupChance in pickupChances.DropablePickupChances)
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
        }
        if (removedDataChance != null)
            pickupChances.DropablePickupChances.Remove(removedDataChance);
        if (GUILayout.Button("Add dropable pickup"))
        {
            pickupChances.DropablePickupChances.Add(new RouletteElementChance<PickupData>(null, 0));
        }
        property.serializedObject.ApplyModifiedProperties();

        EditorGUI.EndProperty();

    }
}

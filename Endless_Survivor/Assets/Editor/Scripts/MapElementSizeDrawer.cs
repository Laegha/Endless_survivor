using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MapElementSize), true)]
public class MapElementSizeDrawer : PropertyDrawer
{
    const float Spacing = 2f;
    const float ButtonSize = 20f;
    const float ButtonSpacing = 5f;
    const int FontSize = 10;


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Foldout
        property.isExpanded = EditorGUI.Foldout(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            property.isExpanded,
            label,
            true
        );

        if (!property.isExpanded)
        {
            EditorGUI.EndProperty();
            return;
        }

        EditorGUI.indentLevel++;

        var squareSizeProp = property.FindPropertyRelative("_elementSquareSize");
        var occupyingPosProp = property.FindPropertyRelative("_elementOccupyingPositions");

        float squareSizePropY = position.y + EditorGUIUtility.singleLineHeight + Spacing;

        // Element Square Size
        var squareSizeRect = new Rect(
            position.x,
            squareSizePropY,
            position.width,
            EditorGUIUtility.singleLineHeight
        );

        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField(squareSizeRect, squareSizeProp);

        if( EditorGUI.EndChangeCheck() )
        {
            occupyingPosProp.ClearArray();
        }
        float gridLabelY = squareSizePropY + EditorGUIUtility.singleLineHeight + Spacing;

        var gridLabelRect = new Rect(
            position.x,
            gridLabelY,
            position.width,
            EditorGUIUtility.singleLineHeight
        );
        GUI.Label(gridLabelRect, "Tiles occupied");

        //Drawing occupying positions grid        
        float gridStartY = gridLabelY + EditorGUIUtility.singleLineHeight + Spacing;
        Vector2 squareSize = squareSizeProp.vector2Value;
        List<Vector2> occupyingPositions = new();
        for( int i = 0; i < occupyingPosProp.arraySize; i ++)
        {
            occupyingPositions.Add(occupyingPosProp.GetArrayElementAtIndex(i).vector2Value);
        }
        for (int buttonX = 0; buttonX <= squareSize.x * 2; buttonX++)
        {
            for (int buttonY = 0; buttonY <= squareSize.y * 2; buttonY++)
            {
                float buttonXAbs = position.x + buttonX * (ButtonSize + ButtonSpacing);
                float buttonYAbs = gridStartY + buttonY * (ButtonSize + ButtonSpacing);
                Rect buttonRect = new Rect(
                    buttonXAbs,
                    buttonYAbs,
                    ButtonSize,
                    ButtonSize
                    );
                Vector2 relatedObjPos = new Vector2(buttonX, buttonY) - squareSize;

                //GUI.Label(buttonRect, "" + relatedObjPos);
                if ( relatedObjPos == Vector2.zero)
                {
                    var fontStyle = new GUIStyle();
                    fontStyle.fontSize = FontSize;
                    fontStyle.normal.textColor = Color.white;
                    fontStyle.alignment = TextAnchor.MiddleCenter;
                    GUI.Label(buttonRect, "Initial \n tile", fontStyle);
                    continue;
                }
                bool isRelatedPosChecked = occupyingPositions.Any(x => x == relatedObjPos);
                bool toggle = GUI.Toggle(buttonRect, isRelatedPosChecked, "");
                if (isRelatedPosChecked != toggle)
                {
                    if(isRelatedPosChecked)
                        occupyingPosProp.DeleteArrayElementAtIndex(occupyingPositions.IndexOf(relatedObjPos));
                    if(!isRelatedPosChecked)
                    {
                        occupyingPosProp.arraySize++;
                        occupyingPosProp.InsertArrayElementAtIndex(occupyingPosProp.arraySize -1);
                        SerializedProperty newElem = occupyingPosProp.GetArrayElementAtIndex(occupyingPosProp.arraySize -1 );
                        newElem.vector2Value = relatedObjPos;
                    }
                }
            }
        }


        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight;

        if (property.isExpanded)
        {
            var squareSizeProp = property.FindPropertyRelative("_elementSquareSize");
            var occupyingPosProp = property.FindPropertyRelative("_elementOccupyingPositions");

            height += Spacing;
            height += EditorGUIUtility.singleLineHeight; // Vector2
            height += Spacing;
            height += EditorGUIUtility.singleLineHeight; // Grid Label
            height += Spacing;
            height += (squareSizeProp.vector2Value.y + 1) * 2 * (ButtonSize + ButtonSpacing)  ; // Grid
        }

        return height;
    }
}

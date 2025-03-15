using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnemyBehaviour), true)]
public class EnemyBehaviourDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true); // Mantiene la altura correcta
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.managedReferenceValue == null)
        {
            EditorGUI.LabelField(position, "No/Invalid behaviour");
            return;
        }

        // Cambiar el label para que muestre el nombre de la subclase
        label.text = property.managedReferenceValue.GetType().Name;

        // Dibujar el campo con el nuevo label y mantener la UI predeterminada
        EditorGUI.PropertyField(position, property, label, true);
    }
}

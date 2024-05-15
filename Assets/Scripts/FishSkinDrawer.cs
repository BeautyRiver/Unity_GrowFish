using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FishSkin))]
public class FishSkinDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Indent level
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        Rect nameRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect listRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, position.height - EditorGUIUtility.singleLineHeight - 2);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), new GUIContent("테마 이름"));
        EditorGUI.PropertyField(listRect, property.FindPropertyRelative("fishSprites"), true);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("fishSprites")) + EditorGUIUtility.singleLineHeight + 4;
    }
}

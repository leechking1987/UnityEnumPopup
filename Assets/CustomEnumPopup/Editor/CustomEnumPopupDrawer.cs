using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(CustomEnumPopup))]
public class CustomEnumPopupDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        CustomEnumPopup p = attribute as CustomEnumPopup;
        if (!string.IsNullOrEmpty(p.Name))
        {
            label.text = p.Name;
        }
        string[] names = System.Enum.GetNames(fieldInfo.FieldType);
        EditorGUI.BeginProperty(position, label, property);
        Rect prefixPosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        string name;
        if (property.enumValueIndex >= names.Length || property.enumValueIndex < 0)
        {
            name = "-";
        }
        else
        {
            name = names[property.enumValueIndex];
        }
        GUIStyle buttonStyle = new GUIStyle(EditorStyles.popup);
        buttonStyle.padding.top = 2;
        buttonStyle.padding.bottom = 2;
        float baseHeight = GUI.skin.textField.CalcSize(new GUIContent()).y;
        Rect popRect = new Rect(prefixPosition.x, position.y, position.x + position.width - prefixPosition.x, baseHeight);
        if (GUI.Button(popRect, name, buttonStyle))
        {
            CustomEnumPopupWindow window = CustomEnumPopupWindow.CreateInstance<CustomEnumPopupWindow>();
            var windowRect = prefixPosition;
            window.Init(property, fieldInfo.FieldType, p.Type);
            windowRect.position = GUIUtility.GUIToScreenPoint(windowRect.position);
            windowRect.height = popRect.height + 1;
            window.ShowAsDropDown(windowRect, new Vector2(windowRect.width, 400));
        }
        EditorGUI.EndProperty();
    }
}
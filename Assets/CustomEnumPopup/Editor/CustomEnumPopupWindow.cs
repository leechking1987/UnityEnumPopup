using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

public class CustomEnumPopupWindow : EditorWindow
{
    public enum CustomEnumPopType
    {
        Alphabet = 0,
        Enum = 1,
    }

    Vector2 position;

    SerializedProperty sp;

    List<SingleEnumName> enumNames = new List<SingleEnumName>();

    System.Type enumType;

    GUIStyle textStyle;

    GUIStyle selectedBackgroundStyle;

    GUIStyle normalBackgroundStyle;

    bool isInitedStype = false;

    bool isSelected = false;

    GUIStyle searchToobar;

    string searchText = string.Empty;

    CustomEnumPopType type;

    bool isShowValue = false;

    SingleEnumName selectSingle = null;

    public void Init(SerializedProperty sp, System.Type enumType, int type)
    {
        enumNames.Clear();
        this.sp = sp;
        this.enumType = enumType;
        this.type = (CustomEnumPopType)type;
        isShowValue = false;
        string[] names = System.Enum.GetNames(enumType);
        selectSingle = null;
        for (int i = 0; i < names.Length; i++)
        {
            string name = names[i];
            SingleEnumName single = new SingleEnumName();
            single.name = name;
            single.index = i;
            single.value = (int)System.Enum.Parse(enumType, name);
            if (i == sp.enumValueIndex)
            {
                single.isSelect = true;
                selectSingle = single;
            }
            else
            {
                single.isSelect = false;
            }
            enumNames.Add(single);
        }
        if (this.type == CustomEnumPopType.Alphabet)
        {
            enumNames.Sort((e1, e2) => { return e1.name.CompareTo(e2.name); });
        }
        else
        {
            enumNames.Sort((e1, e2) => { return e1.value.CompareTo(e2.value); });
        }

        int realIndex;
        if (null == selectSingle)
        {
            realIndex = 0;
        }
        else
        {
            realIndex = enumNames.IndexOf(selectSingle);
        }
        position.y = 16 * realIndex;

        isSelected = false;
        var t = typeof(EditorStyles);
        var property = t.GetProperty("toolbarSearchField", BindingFlags.NonPublic | BindingFlags.Static);
        searchToobar = property.GetValue(null, null) as GUIStyle;

        searchText = string.Empty;
    }

    void OnGUI()
    {
        InitTextStyle();

        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUI.backgroundColor = new Color(1f, 1f, 1f, 0.5f);
        GUI.SetNextControlName("Search");
        searchText = EditorGUILayout.TextField("", searchText, searchToobar, GUILayout.MinWidth(95));
        EditorGUI.FocusTextInControl("Search");
        if (this.type == CustomEnumPopType.Alphabet)
        {
            if (GUILayout.Button("E", EditorStyles.toolbarButton, GUILayout.Width(16)))
            {
                enumNames.Sort((e1, e2) => { return e1.value.CompareTo(e2.value); });

                int realIndex;
                if (null == selectSingle)
                {
                    realIndex = 0;
                }
                else
                {
                    realIndex = enumNames.IndexOf(selectSingle);
                }
                position.y = 16 * realIndex;

                this.type = CustomEnumPopType.Enum;
            }
        }
        else if (this.type == CustomEnumPopType.Enum)
        {
            if (GUILayout.Button("A", EditorStyles.toolbarButton, GUILayout.Width(16)))
            {
                enumNames.Sort((e1, e2) => { return e1.name.CompareTo(e2.name); });

                int realIndex;
                if (null == selectSingle)
                {
                    realIndex = 0;
                }
                else
                {
                    realIndex = enumNames.IndexOf(selectSingle);
                }
                position.y = 16 * realIndex;

                this.type = CustomEnumPopType.Alphabet;
            }
        }
        isShowValue = GUILayout.Toggle(isShowValue, "V", EditorStyles.toolbarButton, GUILayout.Width(16));
        GUILayout.EndHorizontal();
        GUI.backgroundColor = Color.white;

        position = EditorGUILayout.BeginScrollView(position);

        for (int i = 0; i < enumNames.Count; i++)
        {
            SingleEnumName single = enumNames[i];
            if (!string.IsNullOrEmpty(searchText) && !single.name.ToLower().Contains(searchText))
            {
                continue;
            }
            Rect rect;
            if (single.isSelect)
            {
                rect = EditorGUILayout.BeginHorizontal(selectedBackgroundStyle);
            }
            else
            {
                rect = EditorGUILayout.BeginHorizontal(normalBackgroundStyle);
            }
            GUILayout.Label(single.name, textStyle);
            GUILayout.FlexibleSpace();
            if (isShowValue)
            {
                GUILayout.Label(single.value.ToString(), textStyle);
            }

            if (rect.Contains(Event.current.mousePosition) && Event.current.type == EventType.mouseDown)
            {
                sp.enumValueIndex = single.index;
                sp.serializedObject.ApplyModifiedProperties();
                isSelected = true;
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        if (isSelected)
        {
            isSelected = false;
            Close();
        }
    }

    void InitTextStyle()
    {
        if (!isInitedStype)
        {
            textStyle = new GUIStyle(EditorStyles.label);
            textStyle.fixedHeight = 16;
            textStyle.alignment = TextAnchor.MiddleLeft;

            Texture2D selectedBg = new Texture2D(32, 32, TextureFormat.RGB24, false);
            Texture2D hightLightBg = new Texture2D(32, 32, TextureFormat.RGB24, false);
            if (EditorGUIUtility.isProSkin)
            {
                selectedBg.LoadImage(System.Convert.FromBase64String(s_SelectedBg_Pro));
                hightLightBg.LoadImage(System.Convert.FromBase64String(s_HightLightBg_Pro));
            }
            else
            {
                selectedBg.LoadImage(System.Convert.FromBase64String(s_SelectedBg_Light));
                hightLightBg.LoadImage(System.Convert.FromBase64String(s_HightLightBg_Light));
            }
            selectedBackgroundStyle = new GUIStyle();
            selectedBackgroundStyle.normal.background = selectedBg;
            normalBackgroundStyle = new GUIStyle();
            normalBackgroundStyle.hover.background = hightLightBg;

            isInitedStype = true;
        }
    }

    void Update()
    {
        Repaint();
    }

    const string s_SelectedBg_Pro = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAIAAAD8GO2jAAAAQklEQVRIDe3SsQkAAAgDQXWN7L+nOMFXdm8dIhzpJPV581l+3T5AYYkkQgEMuCKJUAADrkgiFMCAK5IIBTDgipBoAWXpAJEoZnl3AAAAAElFTkSuQmCC";

    const string s_HightLightBg_Pro = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAIAAAD8GO2jAAAAQklEQVRIDe3SsQkAAAgDQXXD7L+MOMFXdm8dIhzpJPV581l+3T5AYYkkQgEMuCKJUAADrkgiFMCAK5IIBTDgipBoARFdATMHrayuAAAAAElFTkSuQmCC";

    const string s_SelectedBg_Light = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAIAAAD8GO2jAAAAQUlEQVRIDe3SsQkAAAgDQXV/yMriBF/ZvXWIcKST1OfNZ/l1+wCFJZIIBTDgiiRCAQy4IolQAAOuSCIUwIArQqIF36EB7diYDg8AAAAASUVORK5CYII=";

    const string s_HightLightBg_Light = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAIAAAD8GO2jAAAAQklEQVRIDe3SsQkAAAgDQXX/ETOMOMFXdm8dIhzpJPV581l+3T5AYYkkQgEMuCKJUAADrkgiFMCAK5IIBTDgipBoAc9YAtQLJ3kPAAAAAElFTkSuQmCC";
}
public class SingleEnumName
{
    public string name;

    public int index;

    public int value;

    public bool isSelect;
}
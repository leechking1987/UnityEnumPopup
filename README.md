# UnityEnumPopup
A flexible unity enum popup support display sorted/default enum names
Support both Professional/Personal skins

![image](/UntiyEnumPopupDemo_Professional.png)
![image](/UntiyEnumPopupDemo_Personal.png)

# Useage
```C#
[CustomEnumPopup]
public YourEnum m_enum;

[CustomEnumPopup(sortType)]
public YourEnum m_enum;

[CustomEnumPopup("YourEnumName")]
public YourEnum m_enum;

[CustomEnumPopup("YourEnumName", sortType)]
public YourEnum m_enum;
```

sortType: 0->Alphabet, 1->Default

"YourEnumName": property name displaied in inspector

# Notes
If you have a custom editor script and try to do something like this:
```C#
int index = yourSerializedProperty.enumValueIndex
EditorGUILayout.PropertyField(yourSerializedProperty);
if(index != yourSerializedProperty.enumValueIndex)
{
	//Do Some Thing
}
```
this script will never work because the original UnityEnumPopup will block the OnGUI main thread but ours' will not

you can just make a bit change to the script to make it work again like this:
```C#
private int index;

void OnEnable()
{
	index = yourSerializedProperty.enumValueIndex
}

EditorGUILayout.PropertyField(yourSerializedProperty);
if(index != yourSerializedProperty.enumValueIndex)
{
	//Do Some Thing
	index = yourSerializedProperty.enumValueIndex
}
```
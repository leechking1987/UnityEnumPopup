using UnityEngine;
using System.Collections;

public class CustomEnumPopup : PropertyAttribute
{
    public string Name { get; private set; }

    public int Type { get; private set; }

    /// <summary>
    /// Draw Custom Enum Popup
    /// </summary>
    public CustomEnumPopup()
    {
        Type = 0;
    }

    /// <summary>
    /// Draw Custom Enum Popup
    /// </summary>
    /// <param name="type">Sort Type 0:Alphabet, 1:Enum</param>
    public CustomEnumPopup(int type)
    {
        this.Type = type;
    }

    /// <summary>
    /// Draw Custom Enum Popup
    /// </summary>
    /// <param name="name">Property Name</param>
    public CustomEnumPopup(string name)
    {
        this.Name = name;
        Type = 0;
    }

    /// <summary>
    /// Draw Custom Enum Popup
    /// </summary>
    /// <param name="name">Property Name</param>
    /// <param name="type">Sort Type 0:Alphabet, 1:Enum</param>
    public CustomEnumPopup(string name, int type)
    {
        this.Name = name;
        this.Type = type;
    }
}
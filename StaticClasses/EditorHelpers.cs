using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public static class EditorHelpers
{
    #region Visuals

    public static VisualElement HorizontalContainer()
    {
        var horizontalContainer = new VisualElement()
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                alignItems = Align.Center,
                alignContent = Align.Center,
            }
        };
        return horizontalContainer;
    }
    public static VisualElement VerticalContainer()
    {
        var verticalContainer = new VisualElement()
        {
            style =
            {
                flexDirection = FlexDirection.Column,
                alignItems = Align.Stretch,
                alignContent = Align.Stretch,
            }
        };
        return verticalContainer;
    }

    public static Button DebugButton(string message)
    {
        return new Button(() => Debug.Log(message))
        {
            text = "🐞",
            style =
            {
                marginLeft = StyleKeyword.Auto,
                fontSize = 10,
                height = 20,
                width = 20,
                alignSelf = Align.FlexEnd
            }
        };
    }

    public static Button ToggleExpandedButton(Action onClick, bool isExpanded)
    {
        return new Button(onClick)
        {
            text = isExpanded ? "▼" : "▶",
            style =
            {
                fontSize = 10,
                height = 20,
                width = 20,
                alignSelf = Align.FlexEnd
            }
        };
    }
    
    public static VisualElement CenterBar(Action onClick)
    {
        var bar = new VisualElement
        {
            style =
            {
                height = 20,
                width = 4,
                alignSelf = Align.Center,
                flexGrow = 1,
                marginTop = 2,
                marginBottom = 2
        }
        };
        bar.RegisterCallback(new EventCallback<MouseDownEvent>(_ => onClick()));
        return bar;
    }

    public static VisualElement TitleLabel(string text)
    {
        return new Label(text)
        {
            style =
            {
                fontSize = 14,
                unityFontStyleAndWeight = FontStyle.Bold
            }
        };
    }
    #endregion

    public static Dictionary<string, Type> BuildTypeMap(Type baseType)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(asm =>
            {
                try
                {
                    return asm.GetTypes();
                }
                catch
                {
                    return Type.EmptyTypes;
                }
            })
            .Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t))
            .ToDictionary(t => ObjectNames.NicifyVariableName(t.Name), t => t);
    }

    public static string GetShortTypeName(string fullTypeName)
    {
        if (string.IsNullOrEmpty(fullTypeName)) return null;
        var parts = fullTypeName.Split(' ');
        return parts.Length > 1 ? parts[1].Split('.').Last() : fullTypeName;
    }

    public static float GetDoublePropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight;
    }

    public static string ToReadableString(string camelCase)
    {
        if (string.IsNullOrEmpty(camelCase)) return camelCase;
        var newText = Regex.Replace(camelCase, "([a-z])([A-Z])", "$1 $2");
        newText = Regex.Replace(newText, "([A-Z])([A-Z][a-z])", "$1 $2");
        newText = newText.Replace("<", "").Replace(">", "").Replace("k__Backing Field", "");

        return char.ToUpper(newText[0]) + newText.Substring(1);
    }

    public static void DrawInterfaceMenu(Rect position, SerializedProperty property, GUIContent label, Rect typeRect, Rect contentRect, Dictionary<string, Type> typeMap)
    {
        EditorGUI.BeginProperty(position, label, property);
        var typeName = property.managedReferenceFullTypename;
        var displayName = GetShortTypeName(typeName);
        var nameProperty = property.FindPropertyRelative("name");
        if (nameProperty == null || string.IsNullOrEmpty(nameProperty.stringValue))
        {
            nameProperty = property.FindPropertyRelative("Name");
        }

        if (nameProperty != null && !string.IsNullOrEmpty(nameProperty.stringValue))
        {
            displayName = nameProperty.stringValue;
        }

        if (EditorGUI.DropdownButton(typeRect, new GUIContent(displayName ?? "Select"), FocusType.Keyboard))
        {
            var menu = new GenericMenu();
            if (typeMap == null || typeMap.Count == 0)
            {
                menu.AddDisabledItem(new GUIContent($"No {typeName}s available"));
                menu.ShowAsContext();
                return;
            }

            foreach (var kvp in typeMap)
            {
                var name = kvp.Key;
                var type = kvp.Value;
                menu.AddItem(new GUIContent(name), type.FullName == displayName, () =>
                {
                    property.managedReferenceValue = Activator.CreateInstance(type);
                    property.serializedObject.ApplyModifiedProperties();
                });
            }

            menu.ShowAsContext();
        }

        if (property.managedReferenceValue != null)
        {
            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(contentRect, property, GUIContent.none, true);
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }
}
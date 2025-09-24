using UnityEditor;
using UnityEngine;

public static class Vector3ContextProperties
{
    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        // Register the context menu item for Vector3 fields
        EditorApplication.contextualPropertyMenu += OnContextualPropertyMenu;
    }
    
    static void OnContextualPropertyMenu(GenericMenu menu, SerializedProperty property)
    {
        if (property.propertyType != SerializedPropertyType.Vector3) return;
        
        menu.AddItem(new GUIContent("Zero"), false, () =>
        {
            property.vector3Value = Vector3.zero;
            property.serializedObject.ApplyModifiedProperties();
        });
    }
}
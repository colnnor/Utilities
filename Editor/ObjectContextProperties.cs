using UnityEditor;
using UnityEngine;

public static class ObjectContextProperties
{
    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        // Register the context menu item for RenderTexture fields
        EditorApplication.contextualPropertyMenu += OnContextualPropertyMenu;
    }
    
    static void OnContextualPropertyMenu(GenericMenu menu, SerializedProperty property)
    {
        if (property.propertyType == SerializedPropertyType.ObjectReference)
        {
            SetToNull(menu, property);
        }
    }

    private static void SetToNull(GenericMenu menu, SerializedProperty property)
    {
        menu.AddItem(new GUIContent("Set To Null"), false, () =>
        {
            property.objectReferenceValue = null;
            property.serializedObject.ApplyModifiedProperties();
        });
    }
    
    static void PingObject(string path)
    {
        Object obj = AssetDatabase.LoadAssetAtPath<Object>(path);
        EditorGUIUtility.PingObject(obj);
    }
}
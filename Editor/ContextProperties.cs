using UnityEditor;
using UnityEngine;

public static class ContextProperties
{
    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        // Ensure we don't accumulate duplicate handlers across domain reloads
        EditorApplication.contextualPropertyMenu -= OnContextualPropertyMenu;
        EditorApplication.contextualPropertyMenu += OnContextualPropertyMenu;

        // Unregister when assemblies reload or editor quits
        AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;
        AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
        EditorApplication.quitting -= OnEditorQuitting;
        EditorApplication.quitting += OnEditorQuitting;
    }

    static void OnContextualPropertyMenu(GenericMenu menu, SerializedProperty property)
    {
        if (property.propertyType == SerializedPropertyType.Vector3)
        {
            menu.AddItem(new GUIContent("Zero"), false, () =>
            {
                property.vector3Value = Vector3.zero;
                property.serializedObject.ApplyModifiedProperties();
            });
        }

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

    private static void OnBeforeAssemblyReload()
    {
        EditorApplication.contextualPropertyMenu -= OnContextualPropertyMenu;
    }

    private static void OnEditorQuitting()
    {
        EditorApplication.contextualPropertyMenu -= OnContextualPropertyMenu;
    }
}
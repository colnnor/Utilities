using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class InspectorHeaderToolbarDrawer
{
    private static readonly Action<Editor> onDrawToolbar;

    static InspectorHeaderToolbarDrawer()
    {
        var cachedMethods = TypeCache.GetMethodsWithAttribute<HeaderToolbarButtonAttribute>();
        foreach (var method in cachedMethods.Where(m => m.IsStatic).OrderBy(m => m.GetCustomAttribute<HeaderToolbarButtonAttribute>().Priority))
        {
            var action = (Action<Editor>)Delegate.CreateDelegate(typeof(Action<Editor>), method);
            onDrawToolbar += action;
        }

        if (cachedMethods.Count > 0)
        {
            Debug.Log($"[InspectorHeaderToolbarDrawer] Registered {cachedMethods.Count} toolbar button methods.");
            Editor.finishedDefaultHeaderGUI += DrawToolbar;
        }
    }

    private static void DrawToolbar(Editor editor)
    {
        Rect lineRect = EditorGUILayout.GetControlRect(false, 1);
        lineRect.height = 1;
        lineRect.y += 1;
        lineRect.x = 0;
        EditorGUI.DrawRect(lineRect, Color.black);

        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.FlexibleSpace();

        onDrawToolbar?.Invoke(editor);

        GUILayout.Space(5);
        GUILayout.EndHorizontal();
    }


}

[AttributeUsage(AttributeTargets.Method)]
public class HeaderToolbarButtonAttribute : Attribute
{
    public int Priority { get; set; } = 0;
    public HeaderToolbarButtonAttribute(int priority = 0)
    {
        Priority = priority;
    }
}
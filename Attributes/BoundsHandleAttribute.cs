using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class BoundsHandleAttribute : PropertyAttribute
{
    // Marker attribute - no parameters needed
}
// csharp

[CustomPropertyDrawer(typeof(BoundsHandleAttribute))]
public class BoundsHandleDrawer : PropertyDrawer
{
    // One handle per property-instance key
    static readonly Dictionary<string, BoxBoundsHandle> s_handles = new Dictionary<string, BoxBoundsHandle>();
    static readonly HashSet<string> s_registered = new HashSet<string>();

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Ensure default property (with foldout for children) is drawn
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Draw the default inspector field so it remains visible and editable
        EditorGUI.PropertyField(position, property, label, true);

        var target = property.serializedObject.targetObject;
        if (target == null) return;

        string key = $"{target.GetInstanceID()}:{property.propertyPath}";

        // Register a SceneView callback once per property-instance key
        if (s_registered.Add(key))
        {
            int instanceId = target.GetInstanceID();
            string propPath = property.propertyPath;
            SceneView.duringSceneGui += sceneView => OnSceneGUI(instanceId, propPath);
        }
    }

    static BoxBoundsHandle GetHandle(string key)
    {
        if (!s_handles.TryGetValue(key, out var handle))
        {
            handle = new BoxBoundsHandle();
            s_handles[key] = handle;
        }
        return handle;
    }

    static void OnSceneGUI(int instanceId, string propertyPath)
    {
        var targetObj = EditorUtility.EntityIdToObject(instanceId);
        if (!targetObj) return;

        var so = new SerializedObject(targetObj);
        var prop = so.FindProperty(propertyPath);
        
        if (prop == null) return;

        var centerProp = prop.FindPropertyRelative("m_Center");
        var extentsProp = prop.FindPropertyRelative("m_Extents");
        if (centerProp == null || extentsProp == null) return;

        Vector3 center = centerProp.vector3Value;
        Vector3 extents = extentsProp.vector3Value;
        Vector3 size = extents * 2f;

        UnityEditor.IMGUI.Controls.BoxBoundsHandle handle = GetHandle($"{instanceId}:{propertyPath}");
        string key = $"{instanceId}:{propertyPath}";
        handle.wireframeColor = Handles.yAxisColor;
        handle.center = center;
        handle.size = size;

        // Use component's transform if available (draw in local space like BoxCollider)
        Matrix4x4 matrix = Matrix4x4.identity;
        if (targetObj is Component comp)
            matrix = comp.transform.localToWorldMatrix;

        using (new Handles.DrawingScope(matrix))
        {
            EditorGUI.BeginChangeCheck();
            handle.DrawHandle();
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(targetObj, "Edit Bounds via Handle");
                centerProp.vector3Value = handle.center;
                extentsProp.vector3Value = handle.size * 0.5f;
                so.ApplyModifiedProperties();
                SceneView.RepaintAll();
            }
        }
    }
}

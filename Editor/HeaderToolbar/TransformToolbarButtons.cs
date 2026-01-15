using UnityEditor;
using UnityEngine;

public class TransformToolbarButtons : Editor
{
    public static bool TransformToolsEnabled
    {
        get => EditorPrefs.GetBool("TransformToolsEnabled", true);
        set => EditorPrefs.SetBool("TransformToolsEnabled", value);
    }

    [HeaderToolbarButton(10000)]
    public static void ShowToolsButton(Editor editor)
    {
        if (editor.target is not GameObject go) return;

        var showToolsLabel = TransformToolsEnabled ? "animationvisibilitytoggleon" : "animationvisibilitytoggleoff";
        GUILayout.Label("Transform Tools:", EditorStyles.boldLabel, GUILayout.Width(100));
        if (GUILayout.Button(EditorGUIUtility.IconContent(showToolsLabel, "Toggle Transform Tools"), EditorStyles.toolbarButton, GUILayout.Width(24)))
        {
            TransformToolsEnabled = !TransformToolsEnabled;
        }
    }

    [HeaderToolbarButton]
    public static void RecenterParentHeaderButton(Editor editor)
    {
        if (editor.target is not GameObject go || !TransformToolsEnabled) return;

        Transform transform = go.transform;
        Transform parent = transform.parent;
        if (!parent) return;

        var iconString = "icons/avatarcompass.png";
        var tooltip = "Recenters the parent to the child's position and rotation";
        var icon = EditorGUIUtility.IconContent(iconString, $"Icon | {tooltip}");

        if (GUILayout.Button(icon, EditorStyles.toolbarButton, GUILayout.Width(24)))
        {
            RecenterParent(transform, parent);
        }
    }

    [HeaderToolbarButton]
    public static void ResetParentScaleHeaderButton(Editor editor)
    {
        if (editor.target is not GameObject go || !TransformToolsEnabled) return;

        Transform transform = go.transform;
        Transform parent = transform.parent;
        if (!parent) return;

        var iconString = "d_ScaleTool";
        var tooltip = "Resets the parent's scale to (1,1,1) while preserving the child's world scale";
        var icon = EditorGUIUtility.IconContent(iconString, $"Icon | {tooltip}");

        if (GUILayout.Button(icon, EditorStyles.toolbarButton, GUILayout.Width(24)))
        {
            ResetParentScale(transform, parent);
        }
    }

    private static void RecenterParent(Transform transform, Transform parent)
    {
        int undoGroupIndex = Undo.GetCurrentGroup();

        Undo.SetCurrentGroupName("Recenter Parent");
        Undo.RecordObject(transform, "Recenter Child");
        Undo.RecordObject(parent, "Recenter Parent");

        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;

        parent.position = originalPosition;
        parent.rotation = originalRotation;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        Undo.CollapseUndoOperations(undoGroupIndex);
    }

    private static void ResetParentScale(Transform transform, Transform parent)
    {
        int undoGroupIndex = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Reset Parent Size");
        Undo.RecordObject(transform, "Reset Child Size");
        Undo.RecordObject(parent, "Reset Parent Size");

        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;
        Vector3 originalScale = parent.localScale;


        parent.localScale = Vector3.one;

        transform.position = originalPosition;
        transform.rotation = originalRotation;
        transform.localScale = originalScale;

        Undo.CollapseUndoOperations(undoGroupIndex);
    }
}
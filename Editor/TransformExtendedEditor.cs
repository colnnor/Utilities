using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Codice.Client.BaseCommands.Import;
using UnityEditor;
using UnityEngine;
using VInspector.Libs;

[CustomEditor(typeof(Transform))]
public class TransformExtendedEditor : Editor
{
    private Editor internalEditor;

    public bool TransformToolsEnabled
    {
        get => transformToolsEnabled;
        private set
        {
            EditorPrefs.SetBool("TransformToolsEnabled", value);
            transformToolsEnabled = value;
        }
    }

    private void OnEnable()
    {
        Type editorType = Type.GetType("UnityEditor.TransformInspector, UnityEditor");
        if (editorType != null) internalEditor = CreateEditor(targets, editorType);
    }

    private void OnDisable()
    {
        if (internalEditor) DestroyImmediate(internalEditor);
    }
    
    Color darkSkinHeaderColor = (Color)new Color32(62, 62, 62, 255);
    Color lightSkinHeaderColor = (Color)new Color32(194, 194, 194, 255);
    [SerializeField] private static bool transformToolsEnabled = true;

    protected override void OnHeaderGUI()
    {
        var rect = EditorGUILayout.GetControlRect(false, 0f);
        rect.height = EditorGUIUtility.singleLineHeight * 1.4f;
        rect.y -= rect.height;
        rect.x = 60;
        rect.xMax -= rect.x * 2f;

        EditorGUI.DrawRect(rect, EditorGUIUtility.isProSkin ? darkSkinHeaderColor : lightSkinHeaderColor);

        Transform t = (Transform)target;
        if (!t) return;
        string header = t.name + " (AI Behaviour)";
        if (string.IsNullOrEmpty(header))
            header = target.ToString();

        EditorGUI.LabelField(rect, header, EditorStyles.boldLabel);
    }
    public override void OnInspectorGUI()
    {
        if (internalEditor) internalEditor.OnInspectorGUI();

        Transform transform = (Transform)target;
        if (!transform) return;

        Transform parent = transform.parent;
        if (!parent) return;
        
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        string showToolsLabel = TransformToolsEnabled ? "Hide Tools" : "Show Tools";
        TransformToolsEnabled = GUILayout.Toggle(TransformToolsEnabled, new GUIContent(showToolsLabel, "Toggle Transform Tools"), "Button", GUILayout.Width(90f));

        if (!TransformToolsEnabled) return;
        EditorGUILayout.BeginHorizontal();

        RecenterParentButton(transform, parent);
        ResetParentScaleButton(transform, parent);

        EditorGUILayout.EndHorizontal();
    }

    private static void ResetParentScaleButton(Transform transform, Transform parent)
    {
        if (GUILayout.Button(new GUIContent("Reset Parent Size", "Resets the parent's scale to (1,1,1) while maintaining the child's world size"), GUILayout.Width(115f)))
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

    private static void RecenterParentButton(Transform transform, Transform parent)
    {
        if (GUILayout.Button(new GUIContent("Recenter Parent", "Moves the parent to the child's position and resets the child's local position and rotation"), GUILayout.Width(110f)))
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
    }
}
using System;
using Codice.Client.BaseCommands.Import;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Transform))]
public class TransformExtendedEditor : Editor
{
    private Editor internalEditor;

    private void OnEnable()
    {
        Type editorType = Type.GetType("UnityEditor.TransformInspector, UnityEditor");
        if(editorType != null)
        {
            internalEditor = CreateEditor(targets, editorType);
        }
    }

    private void OnDisable()
    {
        if(internalEditor != null)
        {
            DestroyImmediate(internalEditor);
        }
    }

    public override void OnInspectorGUI()
    {
        if(internalEditor != null)
        {
            internalEditor.OnInspectorGUI();
        }
        Transform transform = (Transform)target;
        if (!transform) return;
        
        Transform parent = transform.parent;
        if (!parent) return;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Tools", EditorStyles.boldLabel);
        GUILayout.Space(10f);
        
        EditorGUILayout.BeginHorizontal();
        //draw icon
        GUIContent icon = EditorGUIUtility.IconContent("SocialNetworks.UDNLogo");
        GUILayout.Label(icon, GUILayout.Width(20f), GUILayout.Height(20f));
        if (icon != null)
        {
            if (GUILayout.Button(new GUIContent("Recenter Parent", "Moves the parent to the child's position and resets the child's local position and rotation"), GUILayout.Width(150f)))
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

        GUIContent resizeIcon = EditorGUIUtility.IconContent("TreeEditor.Refresh");
        GUILayout.Label(resizeIcon, GUILayout.Width(20f), GUILayout.Height(20f));
        if (resizeIcon != null)
        {
            //tooptip
            
            if (GUILayout.Button(new GUIContent("Reset Parent Size", "Resets the parent's scale to (1,1,1) while maintaining the child's world size"), GUILayout.Width(150f)))
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
        EditorGUILayout.EndHorizontal();
    }
}

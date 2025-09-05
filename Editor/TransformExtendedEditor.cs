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
            if (GUILayout.Button("Recenter Parent", GUILayout.Width(150f)))
            {
                RecenterParent(transform, parent);
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    private static void RecenterParent(Transform transform, Transform parent)
    {
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;

        parent.position = originalPosition;
        parent.rotation = originalRotation;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}

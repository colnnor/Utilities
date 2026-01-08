using System;
using UnityEditor;
using UnityEngine;

public static class CreateEmptyParentAtPosition
{
    [MenuItem("GameObject/Create Empty Parent At Position %g", priority = 0, secondaryPriority = 3f)]
    static void CreateEmptyParentAtSamePosition()
    {
        Transform selected = Selection.activeTransform;
        if (!selected) return;
        
        GameObject newParent = new GameObject("EmptyParent") {
            transform =
            {
                position = selected.position,
                rotation = selected.rotation,
                localScale = selected.localScale
            }
        };

        Undo.RegisterCreatedObjectUndo(newParent, "Create Empty Parent");
        Undo.SetTransformParent(selected, newParent.transform, "Set Parent");

        Selection.activeGameObject = newParent.gameObject;
        EditorGUIUtility.PingObject(selected.gameObject);
        
        Type sceneHierarchyType = Type.GetType("UnityEditor.SceneHierarchyWindow,UnityEditor");
        EditorWindow hierarchyWindow = EditorWindow.GetWindow(sceneHierarchyType);
        hierarchyWindow.SendEvent(EditorGUIUtility.CommandEvent("Rename"));
        Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
    }
    
}
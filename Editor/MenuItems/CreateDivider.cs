using UnityEditor;
using UnityEngine;

public static class CreateDivider
{
    [MenuItem("GameObject/Create Divider", false, -10)]
    public static void CreateDividerMenuItem()
    {
        var go = new GameObject("________________________")
        {
            transform =
            {
                localPosition = Vector3.zero,
                localRotation = Quaternion.identity,
                localScale = Vector3.one
            }
        };

        if (Selection.activeGameObject != null)
        {
            go.transform.SetParent(Selection.activeGameObject.transform.parent);
            var index = Mathf.Max(0, Selection.activeGameObject.transform.GetSiblingIndex());
            go.transform.SetSiblingIndex(index);
        }
        else
        {
            go.transform.SetParent(null);
        }
        
        var transform = go.transform;
        Selection.activeObject = go;
        Undo.RegisterCreatedObjectUndo(go, "Create Divider");
    }
}
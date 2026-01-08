using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public static class ExtractComponentTool
{
    [MenuItem("CONTEXT/Component/Extract As Sibling", priority = 504)]
    public static void ExtractAsSiblingMenuOption(MenuCommand cmd)
    {
        var sourceComponent = cmd.context as Component;
        if (!sourceComponent) return;
        
        ExtractComponent(sourceComponent, out GameObject newGameObject, out GameObject sourceGameObject, out int groupIndex);
        
        newGameObject.transform.SetParent(sourceGameObject.transform.parent);
        newGameObject.transform.SetSiblingIndex(sourceGameObject.transform.GetSiblingIndex() + 1);
        newGameObject.transform.localPosition = sourceGameObject.transform.localPosition;
        newGameObject.transform.localRotation = sourceGameObject.transform.localRotation;
        newGameObject.transform.localScale = sourceGameObject.transform.localScale;
        
        Undo.CollapseUndoOperations(groupIndex);

    }

    [MenuItem("CONTEXT/Component/Extract As Child", priority = 505)]
    public static void ExtractAsChildMenuOption(MenuCommand cmd)
    {
        var sourceComponent = cmd.context as Component;
        if (!sourceComponent) return;
        
        ExtractComponent(sourceComponent, out GameObject newGameObject, out GameObject sourceGameObject, out int groupIndex);
        
        newGameObject.transform.SetParent(sourceGameObject.transform);
        newGameObject.transform.localPosition = Vector3.zero;
        newGameObject.transform.localRotation = Quaternion.identity;
        newGameObject.transform.localScale = Vector3.one;
        
        Undo.CollapseUndoOperations(groupIndex);
    }

    static void ExtractComponent(Component sourceComponent, out GameObject newGameObject, out GameObject sourceGameObject, out int groupIndex)
    {
        groupIndex = Undo.GetCurrentGroup();
        Undo.IncrementCurrentGroup();
        
        sourceGameObject = sourceComponent.gameObject;
        newGameObject = new GameObject(sourceComponent.GetType().Name);
        
        
        Undo.RegisterCreatedObjectUndo(newGameObject, "Extract Component As Sibling");
        
        if(!ComponentUtility.CopyComponent(sourceComponent))Debug.LogError($"Failed to copy component", sourceComponent.gameObject);
        if(!ComponentUtility.PasteComponentAsNew(newGameObject))
        {
            Debug.LogError($"Failed to paste component from {sourceComponent.gameObject.name} to {newGameObject.name}", sourceComponent.gameObject);
            return;
        }
        
        Undo.DestroyObjectImmediate(sourceComponent);
    }
}
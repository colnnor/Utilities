using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class OpenFolderTool 
{
    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID)
    {
        Event e = Event.current;
        if (e is { shift: false }) return false;
        
        var obj = EditorUtility.EntityIdToObject(instanceID);
        string path = AssetDatabase.GetAssetPath(obj);
        
        if(AssetDatabase.IsValidFolder(path))
        {
            EditorUtility.RevealInFinder(path);
        }
        return true;
    }
}

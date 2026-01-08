using System.IO;
using UnityEditor;
using UnityEngine;

public static class SceneListManager
{
    private static readonly string assetPath = "Assets/Resources/SceneList/SceneList.asset";

    public static SceneList GetOrCreateSceneList()
    {
        string directoryPath = Path.GetDirectoryName(assetPath);
        if (!Directory.Exists(directoryPath) && directoryPath != null)
        {
            Directory.CreateDirectory(directoryPath);
        }

        SceneList sceneList = AssetDatabase.LoadAssetAtPath<SceneList>(assetPath);
        if (sceneList == null)
        {
            sceneList = ScriptableObject.CreateInstance<SceneList>();
            AssetDatabase.CreateAsset(sceneList, assetPath);
            AssetDatabase.SaveAssets();
        }
        
        return sceneList;
    }
}
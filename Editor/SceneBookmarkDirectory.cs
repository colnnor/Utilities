using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneBookmarkDirectory", menuName = "Scene View Bookmarks/Directory", order = 1)]
public class SceneBookmarkDirectory : ScriptableObject
{
    [SerializeField, HideInInspector] private string sceneGuid = null;

    [SerializeField] private SceneViewBookmark[] bookmarks = null;
    
    public int Count => bookmarks?.Length ?? 0;
    
    public static SceneBookmarkDirectory Create(Scene scene)
    {
        SceneBookmarkDirectory directory = CreateInstance<SceneBookmarkDirectory>();

        string sceneGuid = AssetDatabase.AssetPathToGUID(scene.path);
        directory.sceneGuid = sceneGuid;
        
        string sceneName = Path.GetFileNameWithoutExtension(scene.path);
        var path = scene.path.Substring(0, scene.path.Length - Path.GetFileName(scene.path).Length);
        path = Path.Combine(path, sceneName + "Bookmarks.asset");
        Debug.Log($"Creating bookmark directory at {path}");
        AssetDatabase.CreateAsset(directory, path);

        return directory;
    }
    
    public static SceneBookmarkDirectory GetOrAdd(Scene scene)
    {
        SceneBookmarkDirectory directory = Find(scene);
        if(directory == null)
        {
            directory = Create(scene);
        }

        return directory;
    }
    public static SceneBookmarkDirectory Find(Scene scene)
    {
        string sceneGuid = AssetDatabase.AssetPathToGUID(scene.path);
        foreach (var directoryGuid in AssetDatabase.FindAssets("t:SceneBookmarkDirectory"))
        {
            string path = AssetDatabase.GUIDToAssetPath(directoryGuid);
            var directory = AssetDatabase.LoadAssetAtPath<SceneBookmarkDirectory>(path);
            Debug.Log($"Checking bookmark directory at {path} with guid {directory.sceneGuid}. directory == scene {directory.sceneGuid.Equals(sceneGuid)}");
            if (directory.sceneGuid.Equals(sceneGuid))
                return directory;
        }
        return null;
    }
    
    public SceneViewBookmark? Get(int index)
    {
        if(bookmarks == null || index < 0 || index >= bookmarks.Length)
            return null;
        
        return bookmarks[index];
    }
    
    public void AddBookmark(SceneViewBookmark bookmark)
    {
        if(bookmarks == null)
        {
            bookmarks = new SceneViewBookmark[1];
            bookmarks[0] = bookmark;
        }
        else
        {
            ArrayUtility.Add(ref bookmarks, bookmark);
        }
        
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
}
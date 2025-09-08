using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneViewBookmarkTool : MonoBehaviour
{
    const string k_menuPath = "Tools/Scene Bookmarks/";

    public static SceneViewBookmark cameraData;
    private static int index = 0;
    [MenuItem(k_menuPath + "Add")]
    public static void AddBookmark()
    {
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log($"Scenename {scene.name} {scene.isLoaded} No active scene loaded");
        SceneBookmarkDirectory directory = SceneBookmarkDirectory.GetOrAdd(scene);
        
        directory.AddBookmark(SceneViewBookmark.CreateFromSceneView(SceneView.lastActiveSceneView));
    }
    [MenuItem(k_menuPath + "Switch _b")]
    public static void SwitchBookmark()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneBookmarkDirectory directory = SceneBookmarkDirectory.Find(scene);
        if(directory == null || directory.Count == 0)
            return;

        SceneViewBookmark? bookmark = directory.Get(index);
        if (bookmark.HasValue)
        {
            bookmark.Value.SetSceneViewOrientation(SceneView.lastActiveSceneView);
            index = (index + 1) % directory.Count;
        }
        
    }

    [InitializeOnLoadMethod]
    static void Initialize()
    {
        EditorSceneManager.sceneOpened += OnSceneOpened;
    }
    
    static void OnSceneOpened(Scene scene, OpenSceneMode mode)
    {
        index = 0;
    }
}
using UnityEngine;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[Overlay(typeof(SceneView), "Scene Favorites Overlay")]
public class SceneFavoritesOverlay : Overlay
{
    private List<string> allScenes = new();
    private List<string> favoriteScenes = new();
    private ScrollView scrollView;

    public override VisualElement CreatePanelContent()
    {
        Initialize();

        var container = new VisualElement();
        container.style.flexDirection = FlexDirection.Column;
        container.style.paddingTop = 10;
        container.style.paddingLeft = 10;
        container.style.paddingRight = 10;
        container.style.paddingBottom = 10;
        container.style.minWidth = 100;
        container.style.minHeight = 50;

        var buttonContainer = new VisualElement();
        buttonContainer.style.flexDirection = FlexDirection.Row;
        buttonContainer.style.justifyContent = Justify.SpaceBetween;

        var addButton = new Button(AddCurrentScene) { text = "+ Current Scene" };
        var clearButton = new Button(ClearAllScenes) { text = "Clear All" };
        var showAllButton = new Button(ShowAllScenesInNewPanel) { text = "+ Scene" };

        buttonContainer.Add(addButton);
        buttonContainer.Add(showAllButton);
        buttonContainer.Add(clearButton);
        container.Add(buttonContainer);

        scrollView = new ScrollView();
        container.Add(scrollView);

        UpdateSceneList();

        return container;
    }

    private void Initialize()
    {
        GetAllScenes();
        LoadFavorites();
    }
    
    private void ShowAllScenesInNewPanel()
    {
        var allScenesPanel = new VisualElement();
        allScenesPanel.style.flexDirection = FlexDirection.Column;
        allScenesPanel.style.paddingTop = 10;
        allScenesPanel.style.paddingLeft = 10;
        allScenesPanel.style.paddingRight = 10;
        allScenesPanel.style.paddingBottom = 10;
        allScenesPanel.style.minWidth = 100;
        allScenesPanel.style.minHeight = 50;

        var allScenesScrollView = new ScrollView();
        allScenesPanel.Add(allScenesScrollView);

        foreach (var scene in allScenes)
        {
            var sceneElement = new VisualElement();
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene);
            var openButton = new Button(() => AddFavoriteScene(scene)) { text = $"+ {sceneName}" };
            sceneElement.Add(openButton);

            allScenesScrollView.Add(sceneElement);
        }

        var window = EditorWindow.GetWindow<EditorWindow>();
        window.rootVisualElement.Add(allScenesPanel);
        
        
    }

    private void AddCurrentScene()
    {
        string currentScenePath = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().path;
        AddFavoriteScene(currentScenePath);
    }

    private void AddFavoriteScene(string currentScenePath)
    {
        if (!string.IsNullOrEmpty(currentScenePath) && !favoriteScenes.Contains(currentScenePath))
        {
            favoriteScenes.Add(currentScenePath);
            SaveFavorites();
            UpdateSceneList();
        }
    }

    private void ClearAllScenes()
    {
        favoriteScenes.Clear();
        SaveFavorites();
        UpdateSceneList();
    }

    private void OpenScene(string scenePath)
    {
        if(UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath);
        }
    }

    private void RemoveScene(string scenePath)
    {
        favoriteScenes.Remove(scenePath);
        SaveFavorites();
        UpdateSceneList();
    }

    private void UpdateSceneList()
    {
        scrollView.Clear();
        foreach (var scene in favoriteScenes)
        {
            var sceneElement = new VisualElement();
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene);
            var openButton = new Button(() => OpenScene(scene)) { text = $"Open {sceneName}" };
            sceneElement.Add(openButton);

            openButton.RegisterCallback<ContextClickEvent>(_ =>
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Remove"), false, () => RemoveScene(scene));
                menu.ShowAsContext();
            });

            scrollView.Add(sceneElement);
        }

        // Adjust the height of the scrollView based on the number of scenes
        float sceneHeight = 30f; // Approximate height of each scene element
        float maxHeight = 500f; // Maximum height of the scrollView
        float newHeight = Mathf.Min(favoriteScenes.Count * sceneHeight, maxHeight);
        scrollView.style.height = newHeight;
    }

    private void GetAllScenes()
    {
        allScenes.Clear();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            allScenes.Add(SceneManager.GetSceneAt(i).path);
        }
    }

    private void SaveFavorites()
    {
        string json = JsonUtility.ToJson(new SceneFavorites { scenes = favoriteScenes });
        EditorPrefs.SetString("SceneFavorites", json);
    }

    private void LoadFavorites()
    {
        string json = EditorPrefs.GetString("SceneFavorites", JsonUtility.ToJson(new SceneFavorites()));
        SceneFavorites loadedFavorites = JsonUtility.FromJson<SceneFavorites>(json);
        favoriteScenes = loadedFavorites.scenes;
    }

    private class SceneFavorites
    {
        public List<string> scenes = new List<string>();
    }
}
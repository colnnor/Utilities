using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[Overlay(typeof(SceneView), "Scene Favorites Overlay")]
public class SceneFavoritesOverlay : Overlay
{
    private List<string> favoriteScenes = new();
    private ScrollView scrollView;
    

    public override VisualElement CreatePanelContent()
    {
        Initialize();

        var container = new VisualElement
        {
            style =
            {
                flexDirection = FlexDirection.Column,
                paddingTop = 10,
                paddingLeft = 10,
                paddingRight = 10,
                paddingBottom = 10,
                minWidth = 100,
                minHeight = 50
            }
        };

        var buttonContainer = new VisualElement
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                justifyContent = Justify.SpaceBetween
            }
        };

        var addButton = new Button(AddCurrentScene) { text = "+ Current Scene" };
        var clearButton = new Button(ClearAllScenes) { text = "Clear All" };

        buttonContainer.Add(addButton);
        buttonContainer.Add(clearButton);
        container.Add(buttonContainer);

        scrollView = new ScrollView();
        container.Add(scrollView);

        UpdateSceneList();

        return container;
    }

    private void Initialize()
    {
        LoadFavorites();
    }
    

    private void AddCurrentScene()
    {
        string currentScenePath = SceneManager.GetActiveScene().path;
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
        if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenePath);
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
            string sceneName = Path.GetFileNameWithoutExtension(scene);
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
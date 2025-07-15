using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[Overlay(typeof(SceneView), "Scene Favorites")]
[Icon("d_BuildSettings.SelectedIcon")] // Set the overlay icon here
public class SceneFavoritesOverlay : Overlay
{
    private SceneList sceneList;
    private ScrollView scrollView;
    private VisualElement favoritesContainer;
    private VisualElement container;
public override VisualElement CreatePanelContent()
{
    Initialize();

    container = new VisualElement
    {
        style =
        {
            flexDirection = FlexDirection.Column,
            paddingTop = 10,
            paddingLeft = 10,
            paddingRight = 10,
            paddingBottom = 10,
            minWidth = 100,
            minHeight = 200,
        }
    };

    var searchField = new TextField("Search Scenes")
    {
        style =
        {
            marginBottom = 10
        }
    };
    searchField.RegisterValueChangedCallback(evt =>
    {
        UpdateSceneList(evt.newValue);
    });

    favoritesContainer = new VisualElement
    {
        style =
        {
            flexDirection = FlexDirection.Column,
            alignItems = Align.FlexStart
        }
    };

    var buttonContainer = new VisualElement
    {
        style =
        {
            paddingTop = 10,
            flexDirection = FlexDirection.Row,
            justifyContent = Justify.SpaceBetween
        }
    };

    var addButton = new Button(AddCurrentScene) { text = "Favorite Current Scene" };
    var refreshButton = new Button(() => UpdateSceneList("")) { text = "Refresh" };
    var clearButton = new Button(ClearAllScenes) { text = "Clear All Favorites" };

    buttonContainer.Add(addButton);
    buttonContainer.Add(refreshButton);
    buttonContainer.Add(clearButton);

    scrollView = new ScrollView();

    container.Add(searchField);
    container.Add(favoritesContainer);
    container.Add(scrollView);
    container.Add(buttonContainer);

    UpdateSceneList("");
    return container;
}

private void UpdateSceneList(string searchTerm)
{
    scrollView.Clear();
    favoritesContainer.Clear();

    List<string> allScenes = new List<string>(sceneList.favoriteScenes);

    string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", null);
    allScenes.AddRange(sceneGuids.Select(AssetDatabase.GUIDToAssetPath).Where(path => !sceneList.favoriteScenes.Contains(path)));

    foreach (string scene in allScenes)
    {
        if (!sceneList.lastOpenedScenes.ContainsKey(scene))
        {
            sceneList.lastOpenedScenes[scene] = DateTime.MinValue;
        }
    }

    allScenes = allScenes.Where(scene =>
        string.IsNullOrEmpty(searchTerm) || Path.GetFileNameWithoutExtension(scene).Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

    allScenes.Sort((a, b) =>
    {
        if (sceneList.favoriteScenes.Contains(a) && !sceneList.favoriteScenes.Contains(b)) return -1;
        if (!sceneList.favoriteScenes.Contains(a) && sceneList.favoriteScenes.Contains(b)) return 1;

        DateTime aLastOpened = sceneList.lastOpenedScenes[a];
        DateTime bLastOpened = sceneList.lastOpenedScenes[b];

        if (aLastOpened != bLastOpened)
        {
            return bLastOpened.CompareTo(aLastOpened);
        }

        return string.Compare(Path.GetFileNameWithoutExtension(a), Path.GetFileNameWithoutExtension(b), StringComparison.Ordinal);
    });

    foreach (var scene in allScenes)
    {
        var sceneElement = new VisualElement
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                alignItems = Align.Center,
                justifyContent = Justify.FlexStart
            }
        };

        string sceneName = Path.GetFileNameWithoutExtension(scene);
        var openLabel = new Label(sceneName)
        {
            style =
            {
                unityTextAlign = TextAnchor.MiddleLeft,
                marginLeft = 5,
                marginRight = 5
            }
        };
        openLabel.RegisterCallback<ClickEvent>(evt => OpenScene(scene));

        var starButton = new Button(() => ToggleFavorite(scene))
        {
            text = sceneList.favoriteScenes.Contains(scene) ? "★" : "☆",
            style = { marginLeft = 5 }
        };

        sceneElement.Add(starButton);
        sceneElement.Add(openLabel);

        if (sceneList.favoriteScenes.Contains(scene))
        {
            favoritesContainer.Add(sceneElement);
        }
        else
        {
            scrollView.Add(sceneElement);
        }
    }

    if (sceneList.favoriteScenes.Count > 0)
    {
        var separator = new VisualElement
        {
            style =
            {
                height = 1,
                backgroundColor = new StyleColor(Color.gray),
                marginTop = 5,
                marginBottom = 5
            }
        };
        favoritesContainer.Add(separator);
    }

    // Adjust scroll view height dynamically based on the number of scenes
    float sceneHeight = 30f;
    float maxHeight = 500f;
    float scenesHeight = allScenes.Count * sceneHeight;
    float newHeight = Mathf.Min(scenesHeight, maxHeight);
    
    scrollView.style.height = newHeight;
    
    container.style.height = newHeight + favoritesContainer.resolvedStyle.height + 100; // Adjust container height to fit scroll view and favorites

}

    private void Initialize()
    {
        sceneList = SceneListManager.GetOrCreateSceneList();
    }

    private void AddCurrentScene()
    {
        string currentScenePath = SceneManager.GetActiveScene().path;
        AddFavoriteScene(currentScenePath);
    }

    private void AddFavoriteScene(string currentScenePath)
    {
        if (!string.IsNullOrEmpty(currentScenePath) && !sceneList.favoriteScenes.Contains(currentScenePath))
        {
            sceneList.favoriteScenes.Add(currentScenePath);
            SaveSceneList();
            UpdateSceneList();
        }
    }

    private void ClearAllScenes()
    {
        sceneList.favoriteScenes.Clear();
        SaveSceneList();
        UpdateSceneList();
    }

    private void OpenScene(string scenePath)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenePath);
            if (!sceneList.favoriteScenes.Contains(scenePath))
            {
                sceneList.lastOpenedScenes[scenePath] = DateTime.Now;
                SaveSceneList();
                UpdateSceneList(); // Ensure the list is updated after opening a scene
            }
        }
    }

    private void RemoveScene(string scenePath)
    {
        sceneList.favoriteScenes.Remove(scenePath);
        SaveSceneList();
        UpdateSceneList();
    }
private void UpdateSceneList()
{
    scrollView.Clear();
    favoritesContainer.Clear();

    List<string> allScenes = new List<string>(sceneList.favoriteScenes);

    // Get all scene GUIDs and add scenes that aren't in favorites
    string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", null);
    allScenes.AddRange(sceneGuids.Select(AssetDatabase.GUIDToAssetPath).Where(path => !sceneList.favoriteScenes.Contains(path)));

    // Ensure all scenes have an entry in lastOpenedScenes
    foreach (string scene in allScenes)
    {
        if (!sceneList.lastOpenedScenes.ContainsKey(scene))
        {
            sceneList.lastOpenedScenes[scene] = DateTime.MinValue; // If not opened, set as minimum value
        }
    }

    // Sort the scenes: first by whether they're a favorite, then by last opened date, and finally by name if not recently opened
    allScenes.Sort((a, b) =>
    {
        if (sceneList.favoriteScenes.Contains(a) && !sceneList.favoriteScenes.Contains(b)) return -1;
        if (!sceneList.favoriteScenes.Contains(a) && sceneList.favoriteScenes.Contains(b)) return 1;

        DateTime aLastOpened = sceneList.lastOpenedScenes[a];
        DateTime bLastOpened = sceneList.lastOpenedScenes[b];

        // If both scenes were opened recently, sort by last opened date
        if (aLastOpened != bLastOpened)
        {
            return bLastOpened.CompareTo(aLastOpened); // Most recently opened first
        }

        // If both scenes have not been opened recently, sort by name alphabetically
        return string.Compare(Path.GetFileNameWithoutExtension(a), Path.GetFileNameWithoutExtension(b), StringComparison.Ordinal);
    });

    // Add scenes to the UI (favorites and other scenes)
    foreach (var scene in allScenes)
    {
        var sceneElement = new VisualElement
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                alignItems = Align.Center,
                justifyContent = Justify.FlexStart
            }
        };

        string sceneName = Path.GetFileNameWithoutExtension(scene);
        var openLabel = new Label(sceneName)
        {
            style =
            {
                unityTextAlign = TextAnchor.MiddleLeft,
                marginLeft = 5,
                marginRight = 5
            }
        };
        openLabel.RegisterCallback<ClickEvent>(evt => OpenScene(scene));

        var starButton = new Button(() => ToggleFavorite(scene))
        {
            text = sceneList.favoriteScenes.Contains(scene) ? "★" : "☆",
            style = { marginLeft = 5 }
        };

        sceneElement.Add(starButton);
        sceneElement.Add(openLabel);

        if (sceneList.favoriteScenes.Contains(scene))
        {
            favoritesContainer.Add(sceneElement);
        }
        else
        {
            scrollView.Add(sceneElement);
        }
    }

    // Add a separator if there are any favorite scenes
    if (sceneList.favoriteScenes.Count > 0)
    {
        var separator = new VisualElement
        {
            style =
            {
                height = 1,
                backgroundColor = new StyleColor(Color.gray),
                marginTop = 5,
                marginBottom = 5
            }
        };
        favoritesContainer.Add(separator);
    }

    // Adjust scroll view height based on the number of scenes
    float sceneHeight = 30f;
    float maxHeight = 500f;
    float newHeight = Mathf.Min(allScenes.Count * sceneHeight, maxHeight);
    scrollView.style.height = newHeight;
}

    private void ToggleFavorite(string scenePath)
    {
        if (sceneList.favoriteScenes.Contains(scenePath))
        {
            sceneList.favoriteScenes.Remove(scenePath);
        }
        else
        {
            sceneList.favoriteScenes.Add(scenePath);
        }
        SaveSceneList();
        UpdateSceneList();
    }

    private void SaveSceneList()
    {
        EditorUtility.SetDirty(sceneList);
        AssetDatabase.SaveAssets();
    }

    private void OnDisable()
    {
        SaveSceneList();
    }
}

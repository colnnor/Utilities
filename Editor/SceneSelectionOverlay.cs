using System.Collections.Generic;
using System.IO;
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
    private List<string> favoriteScenes = new();
    private Dictionary<string, System.DateTime> lastOpenedScenes = new();
    private ScrollView scrollView;
    private VisualElement favoritesContainer;

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

        favoritesContainer = new VisualElement
        {
            style =
            {
                flexDirection = FlexDirection.Column,
                alignItems = Align.FlexStart // Align items to the start (left)
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
        var clearButton = new Button(ClearAllScenes) { text = "Clear All Favorites" };

        buttonContainer.Add(addButton);
        buttonContainer.Add(clearButton);

        scrollView = new ScrollView();

        container.Add(favoritesContainer);
        container.Add(scrollView);
        container.Add(buttonContainer);

        UpdateSceneList();

        return container;
    }

    private void Initialize()
    {
        LoadFavorites();
        LoadLastOpenedScenes();
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
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenePath);
            if (!favoriteScenes.Contains(scenePath))
            {
                lastOpenedScenes[scenePath] = System.DateTime.Now;
                SaveLastOpenedScenes();
                UpdateSceneList(); // Ensure the list is updated after opening a scene
            }
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
        favoritesContainer.Clear();

        List<string> allScenes = new List<string>(favoriteScenes);

        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", null);
        allScenes.AddRange(sceneGuids.Select(AssetDatabase.GUIDToAssetPath).Where(path => !favoriteScenes.Contains(path)));

        // Ensure all scenes in allScenes have an entry in lastOpenedScenes
        foreach (string scene in allScenes)
        {
            if (!lastOpenedScenes.ContainsKey(scene))
            {
                lastOpenedScenes[scene] = System.DateTime.MinValue;
            }
        }

        // Sort scenes by favorite status and last opened time
        allScenes.Sort((a, b) =>
        {
            if (favoriteScenes.Contains(a) && !favoriteScenes.Contains(b)) return -1;
            if (!favoriteScenes.Contains(a) && favoriteScenes.Contains(b)) return 1;
            return lastOpenedScenes[b].CompareTo(lastOpenedScenes[a]);
        });

        foreach (var scene in allScenes)
        {
            var sceneElement = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    justifyContent = Justify.FlexStart // Ensure elements are left-aligned
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
                text = favoriteScenes.Contains(scene) ? "★" : "☆",
                style = { marginLeft = 5 }
            };
            sceneElement.Add(starButton);
            sceneElement.Add(openLabel);

            if (favoriteScenes.Contains(scene))
            {
                favoritesContainer.Add(sceneElement);
            }
            else
            {
                scrollView.Add(sceneElement);
            }
        }

        if (favoriteScenes.Count > 0)
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

        float sceneHeight = 30f;
        float maxHeight = 500f;
        float newHeight = Mathf.Min(allScenes.Count * sceneHeight, maxHeight);
        scrollView.style.height = newHeight;
    }

    private void ToggleFavorite(string scenePath)
    {
        if (favoriteScenes.Contains(scenePath))
        {
            favoriteScenes.Remove(scenePath);
        }
        else
        {
            favoriteScenes.Add(scenePath);
        }
        SaveFavorites();
        UpdateSceneList();
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

    private void SaveLastOpenedScenes()
    {
        string json = JsonUtility.ToJson(new LastOpenedScenes { scenes = lastOpenedScenes });
        EditorPrefs.SetString("LastOpenedScenes", json);
    }

    private void LoadLastOpenedScenes()
    {
        string json = EditorPrefs.GetString("LastOpenedScenes", JsonUtility.ToJson(new LastOpenedScenes()));
        LastOpenedScenes loadedLastOpenedScenes = JsonUtility.FromJson<LastOpenedScenes>(json);
        lastOpenedScenes = loadedLastOpenedScenes.scenes;
    }

    private class SceneFavorites
    {
        public List<string> scenes = new List<string>();
    }

    private class LastOpenedScenes
    {
        public Dictionary<string, System.DateTime> scenes = new Dictionary<string, System.DateTime>();
    }
}
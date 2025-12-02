using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MissingScriptsHunter
{
    [MenuItem("Tools/Find Missing Scripts in Scene %#m")]
    static void FindMissingScriptsInScene()
    {
        bool isPrefabMode = PrefabStageUtility.GetCurrentPrefabStage();
        if (isPrefabMode)
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            
            var prefabRoot = prefabStage.prefabContentsRoot;
            var prefabGameObjects = new List<GameObject> { prefabRoot };
            
            prefabGameObjects.AddRange(prefabRoot.GetComponentsInChildren<Transform>().Select(t => t.gameObject));
            
            CheckGameObjects(prefabGameObjects);
            return;
        }

        var sceneGameObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None).ToList();
        CheckGameObjects(sceneGameObjects);
    }

    static void CheckGameObjects(List<GameObject> gameObjects)
    {
        foreach (var gameObject in gameObjects)
        {
            Component[] components = gameObject.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    Debug.Log($"GameObject: {gameObject.name} has a missing script at index {i}.", gameObject);
                }
            }
        }
    }
}
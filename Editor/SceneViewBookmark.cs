using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[Serializable]
public struct SceneViewBookmark
{
    public Vector3 pivot;
    public Quaternion rotation;
    public float size;
    public bool orthographic;
    
    
    public static SceneViewBookmark CreateFromSceneView(SceneView sceneView)
    {
        SceneViewBookmark state = new()
        {
            pivot = sceneView.pivot,
            rotation = sceneView.rotation,
            size = sceneView.size,
            orthographic = sceneView.orthographic
        };
        return state;
    }
    
    public void SetSceneViewOrientation(SceneView sceneView)
    {
        sceneView.pivot = pivot;
        sceneView.rotation = rotation;
        sceneView.size = size;
        sceneView.orthographic = orthographic;
        sceneView.Repaint();
    }
}
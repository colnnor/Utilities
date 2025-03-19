using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class SelectRendererWindow : OdinEditorWindow
{
    [MenuItem("Tools/Colnnor/Select Renderer")]
    private static void OpenWindow()
    {
        GetWindow<SelectRendererWindow>().Show();
    }

    [Button]
    public static void SelectPipelineRenderer()
    {
        var currentPipeline = GraphicsSettings.defaultRenderPipeline;
        Selection.activeObject = currentPipeline;
    }
}

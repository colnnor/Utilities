using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;

[InitializeOnLoad]
public class RenderPipelineDefinesProcessor
{
    static RenderPipelineDefinesProcessor()
    {
        UpdateDefines();
    }

    private static void UpdateDefines()
    {
        // 1. Check which packages are present/active via GraphicsSettings
        bool hasURP = UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline != null && 
                      UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline.GetType().Name.Contains("Universal");
        
        bool hasHDRP = UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline != null && 
                       UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline.GetType().Name.Contains("HD");

        // 2. Target the current active build platform
        BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup);
        
        string currentDefines = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);
        List<string> definesList = new List<string>(currentDefines.Split(';'));

        // 3. Clean up and re-evaluate symbols
        definesList.Remove("UNITY_URP");
        definesList.Remove("UNITY_HDRP");

        if (hasURP) definesList.Add("UNITY_URP");
        if (hasHDRP) definesList.Add("UNITY_HDRP");

        // 4. Save defines back to player settings if changed
        string newDefines = string.Join(";", definesList.ToArray());
        if (newDefines != currentDefines)
        {
            PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, newDefines);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class SetupEditorWindow : EditorWindow
{
    Dictionary<MethodInfo, bool> selectedAssets = new Dictionary<MethodInfo, bool>();
    Dictionary<MethodInfo, bool> selectedPackages = new Dictionary<MethodInfo, bool>();

    [MenuItem("Tools/Setup/Open Setup Window", priority = -250)]
    public static void OpenWindow()
    {
        var window = GetWindow<SetupEditorWindow>("Setup");
        window.minSize = new Vector2(300, 200);
        window.Show();
    }

    private void OnGUI()
    {
        CreateFoldersButton();
        //horizontal, two panels scrollable
        EditorGUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(position.width / 2 - 10), GUILayout.ExpandHeight(true));
        DrawAssetsTab();
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(position.width / 2 - 10), GUILayout.ExpandHeight(true));
        DrawPackagesTab();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    private void CreateFoldersButton()
    {
        
        if (GUILayout.Button("Create Standard Project Folders"))
        {
            Setup.CreateFolders();
        }
    }

    private void DrawAssetsTab()
    {
        EditorGUILayout.BeginHorizontal();
        var assetsLabelStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 14
        };
        GUILayout.Label("Assets", assetsLabelStyle, GUILayout.Height(20));
        //horizontal line black
        if (GUILayout.Button("Import Selected Assets"))
        {
            foreach (var kvp in selectedAssets)
            {
                if (kvp.Value)
                {
                    kvp.Key.Invoke(null, null);
                }
            }
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.Height(10));
        var methods = typeof(Setup).GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var method in methods.Where(m => m.GetCustomAttributes(typeof(MenuItem), false).Length > 0 && m.Name.StartsWith("Import")))
        {
            //add a toggle for each method and one button to import selected
            var label = method.Name.Replace("Import", "");
            selectedAssets.TryAdd(method, false);
            selectedAssets[method] = EditorGUILayout.ToggleLeft(label, selectedAssets[method]);
        }
    }

    private void DrawPackagesTab()
    {
        EditorGUILayout.BeginHorizontal();
        var packagesLabelStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 14
        };
        GUILayout.Label("Packages", packagesLabelStyle, GUILayout.Height(20));
        var methods = typeof(Setup).GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (GUILayout.Button("Add Selected Packages"))
        {
            foreach (var kvp in selectedPackages)
            {
                if (kvp.Value)
                {
                    kvp.Key.Invoke(null, null);
                }
            }
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.Height(10));
        foreach (var method in methods.Where(m => m.GetCustomAttributes(typeof(MenuItem), false).Length > 0 && m.Name.StartsWith("Install")))
        {
            //add a toggle for each method and one button to import selected
            var label = method.Name.Replace("Install", "");
            selectedPackages.TryAdd(method, false);
            selectedPackages[method] = EditorGUILayout.ToggleLeft(label, selectedPackages[method]);
        }
    }
}
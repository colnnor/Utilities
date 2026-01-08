using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;
public class MainToolbarButtons
{
    [MainToolbarElement("Project/Open Project Settings", defaultDockPosition = MainToolbarDockPosition.Middle)]
    public static MainToolbarElement ProjectSettingsButton()
    {
        var icon = EditorGUIUtility.IconContent("SettingsIcon").image as Texture2D;
        var content = new MainToolbarContent(icon);
        var button = new MainToolbarButton(content, () => { SettingsService.OpenProjectSettings(); });

        MainToolbarElementStyler.StyleElement<EditorToolbarButton>("Project/Open Project Settings", element =>
        {
            element.style.paddingLeft = 0f;
            element.style.paddingRight = 0f;
            element.style.marginLeft = 0f;
            element.style.marginRight = 0f;
            element.style.minWidth = 20f;
            element.style.maxWidth = 20f;

            var image = element.Q<Image>();
            if (image != null)
            {
                image.style.width = 12f;
                image.style.height = 12f;
            }
        });
        return button;
    }

    private static void OpenMyAssets()
    {
        
        var editorAssembly = Assembly.GetAssembly(typeof(Editor));
        var assetStoreWindowType = editorAssembly.GetType("UnityEditor.AssetStoreWindow");
        if (assetStoreWindowType != null)
        {
            var openMethod = assetStoreWindowType.GetMethod("OpenMyAssetsInPackageManager", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            openMethod?.Invoke(null, null);
        }
        else
        {
            UnityEditor.PackageManager.UI.Window.Open("MyAssets");
            Debug.Log($"Editor not found, opened Package Manager instead.");
        }
    }

    [MainToolbarElement("Project/Open My Assets", defaultDockPosition = MainToolbarDockPosition.Middle)]
    public static MainToolbarElement MyAssetsButton()
    {
        var icon = EditorGUIUtility.IconContent("icons/asset store.png").image as Texture2D;
        var content = new MainToolbarContent(icon);
        var button = new MainToolbarButton(content, OpenMyAssets);
        MainToolbarElementStyler.StyleElement<EditorToolbarButton>("Project/Open My Assets", element =>
        {
            element.style.paddingLeft = 0f;
            element.style.paddingRight = 0f;
            element.style.marginLeft = 0f;
            element.style.marginRight = 0f;
            element.style.minWidth = 20f;
            element.style.maxWidth = 20f;

            var image = element.Q<Image>();
            if (image != null)
            {
                image.style.width = 12f;
                image.style.height = 12f;
            }
        });
        return button;
    }

    [MainToolbarElement("Timescale/Reset", defaultDockPosition = MainToolbarDockPosition.Middle)]
    public static MainToolbarElement ResetTimeScaleButton()
    {
        var icon = EditorGUIUtility.IconContent("Refresh").image as Texture2D;
        var content = new MainToolbarContent(icon, "Reset");
        var button = new MainToolbarButton(content, () =>
        {
            Time.timeScale = 1f;
            MainToolbar.Refresh("Timescale/Slider");
        });

        MainToolbarElementStyler.StyleElement<EditorToolbarButton>("Timescale/Reset", element =>
        {
            element.style.paddingLeft = 0f;
            element.style.paddingRight = 0f;
            element.style.marginLeft = 0f;
            element.style.marginRight = 0f;
            element.style.minWidth = 20f;
            element.style.maxWidth = 20f;

            var image = element.Q<Image>();
            if (image != null)
            {
                image.style.width = 12f;
                image.style.height = 12f;
            }
        });

        return button;
    }
}
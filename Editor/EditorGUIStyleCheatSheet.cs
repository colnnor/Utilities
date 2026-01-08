using System.Linq;
using UnityEditor;
using UnityEngine;

public class EditorGUIStyleCheatSheet : EditorWindow
{
    private string searchTerm;
    private Vector2 scrollPos;
    private GUIContent copyIcon;
    private GUIStyle copyButtonStyle;

    private int activeTab = 0;
    
    [MenuItem("Window/UI/Editor GUIStyle Cheat Sheet", priority = 3000)]
    public static void ShowWindow()
    {
        GetWindow<EditorGUIStyleCheatSheet>("Editor GUI Styles");
    }

    private void OnGUI()
    {

        // Tabs
        string[] tabs = { "Icons", "Skin Styles" };
        activeTab = GUILayout.Toolbar(activeTab, tabs);
        
        if (activeTab == 0)
            IconsTab();
        else if (activeTab == 1)
            SkinStylesTab();
        else
        {
            GUILayout.Label("Invalid Tab Selected");
        }
    }

    private string SearchBar()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.Width(Screen.width));
        string search = GUILayout.TextField(searchTerm, EditorStyles.toolbarSearchField, GUILayout.Width(200f));
        
        if (searchTerm != search)
            searchTerm = search;
        
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        return search;
    }

    private void IconsTab()
    {
        var searchString = SearchBar();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        var icons = IconMiner.EnumerateIcons().ToList();
        foreach (string icon in icons)
        {
            if (string.IsNullOrEmpty(searchString) || icon.ToLower().Contains(searchString.ToLower()))
            {
                //remove "icons/" and file extension for display
                var iconContent = EditorGUIUtility.IconContent(icon);
                if (iconContent == null || iconContent.image == null) continue;

                EditorGUILayout.BeginHorizontal("box");

                if (GUILayout.Button(iconContent, copyButtonStyle ??= GUI.skin.FindStyle("ToolbarSearchTextFieldJumpButton")))
                {
                    //add quotation marks around the icon name for easier copy-pasting
                    var copyString = $"\"{icon}\"";
                    EditorGUIUtility.systemCopyBuffer = $"{copyString}";
                    ShowNotification(new GUIContent($"Style {copyString} copied to clipboard"));
                }

                EditorGUILayout.LabelField(icon);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndScrollView();

    }

    private void SkinStylesTab()
    {
        SearchBar();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        foreach (var style in GUI.skin)
        {
            if (style is GUIStyle guiStyle)
            {
                if (!string.IsNullOrEmpty(searchTerm) && !guiStyle.name.ToLower().Contains(searchTerm.ToLower()))
                {
                    continue;
                }

                EditorGUILayout.BeginHorizontal("box");

                var guiName = guiStyle.name;
                
                if (GUILayout.Button(copyIcon ??= EditorGUIUtility.IconContent("Grid.PickingTool"), copyButtonStyle ??= GUI.skin.FindStyle("ToolbarSearchTextFieldJumpButton")))
                {
                    EditorGUIUtility.systemCopyBuffer = guiName;
                    ShowNotification(new GUIContent($"Style \"{guiName}\" copied to clipboard"));
                }

                EditorGUILayout.LabelField(guiStyle.name);

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.LabelField(guiStyle.normal.background == null ? guiStyle.name : string.Empty, guiStyle);

                EditorGUILayout.Space(guiStyle.CalcHeight(GUIContent.none, position.width));
            }
        }

        EditorGUILayout.EndScrollView();
    }

}
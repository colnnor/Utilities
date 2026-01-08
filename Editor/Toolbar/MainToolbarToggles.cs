using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

public class MainToolbarToggles
{
    [MainToolbarElement("EditorSettings/Toggle Domain Reload", defaultDockPosition = MainToolbarDockPosition.Middle)]
    public static MainToolbarElement ToggleDomainReload()
    {
        var enabled = EditorSettings.enterPlayModeOptionsEnabled &&
                      (EditorSettings.enterPlayModeOptions &
                       (EnterPlayModeOptions.DisableDomainReload | EnterPlayModeOptions.DisableSceneReload)) == 0;
        
        var icon = enabled
            ? EditorGUIUtility.IconContent("icons/d_preaudioautoplayoff.png").image as Texture2D
            : EditorGUIUtility.IconContent("d_PlayButton").image as Texture2D;
        
        var content = new MainToolbarContent("Domain Reload", icon,  enabled ? "Domain Reload Enabled" : "Domain Reload Disabled");
        var button = new MainToolbarToggle(content, !enabled, ToggleDomainReload);
        return button;
    }

    private static void ToggleDomainReload(bool e)
    {
        var options = EditorSettings.enterPlayModeOptions;
        //If toggling on, remove the DisableDomainReload and DisableSceneReload flags 
        //1 = DisableDomainReload
        //2 = DisableSceneReload
        if (e)
        {
            Debug.Log("Enabling Domain Reload and Scene Reload.");
            options = EnterPlayModeOptions.None;
        }
        else
        {
            Debug.Log("Disabling Domain Reload and Scene Reload for faster Play Mode entry.");
            options |= EnterPlayModeOptions.DisableDomainReload | EnterPlayModeOptions.DisableSceneReload;
        }
        EditorSettings.enterPlayModeOptions = options;
        MainToolbar.Refresh("EditorSettings/Toggle Domain Reload");
    }
}
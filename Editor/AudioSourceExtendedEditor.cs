using UnityEditor;
using UnityEngine;

public static class AudioSourceExtendedEditor
{
    [HeaderToolbarButton]
    static void OnAudioButton(Editor editor)
    {
        if (editor.target is not GameObject go) return;
        if (!go.TryGetComponent(out AudioSource audioSource)) return;

        var buttonContent = EditorGUIUtility.IconContent("preAudioAutoPlayOff");
        buttonContent.tooltip = "Play Audio";
        if (GUILayout.Button(buttonContent, EditorStyles.toolbarButton))
        {
            audioSource.Play();
        }
    }
}
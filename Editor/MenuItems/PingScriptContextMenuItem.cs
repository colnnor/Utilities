using UnityEditor;
using UnityEngine;

public static class PingScriptContextMenuItem
{
    [MenuItem("CONTEXT/MonoBehaviour/Ping Script")]
    private static void PingScript(MenuCommand command)
    {
        MonoBehaviour monoBehaviour = command.context as MonoBehaviour;
        if (monoBehaviour == null) return;

        MonoScript script = MonoScript.FromMonoBehaviour(monoBehaviour);
        if (script != null)
        {
            EditorGUIUtility.PingObject(script);
        }
    }
    
}
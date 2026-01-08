using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class LockInspector {
    static readonly MethodInfo flipLocked;
    static readonly PropertyInfo constrainProportions;
    const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

    static LockInspector() {
        var editorLockTrackerType = typeof(EditorGUIUtility).Assembly.GetType("UnityEditor.EditorGUIUtility+EditorLockTracker");
        flipLocked = editorLockTrackerType.GetMethod("FlipLocked", bindingFlags);
        constrainProportions = typeof(Transform).GetProperty("constrainProportionsScale", bindingFlags);
    }

    [MenuItem("Edit/Toggle Inspector Lock %l")]
    public static void Lock() {
        // New approach for Unity 2023.2 and above, including Unity 6
        var inspectorWindowType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.InspectorWindow");

        foreach (var inspectorWindow in Resources.FindObjectsOfTypeAll(inspectorWindowType)) {
            var lockTracker = inspectorWindowType.GetField("m_LockTracker", bindingFlags)
                ?.GetValue(inspectorWindow);
            flipLocked?.Invoke(lockTracker, new object[] { });
        }

        // Constrain Proportions lock for all versions including Unity 6
        foreach (var activeEditor in ActiveEditorTracker.sharedTracker.activeEditors) {
            if (activeEditor.target is not Transform target) continue;

            var currentValue = (bool) constrainProportions.GetValue(target, null);
            constrainProportions.SetValue(target, !currentValue, null);
        }

        ActiveEditorTracker.sharedTracker.ForceRebuild();
    }

    [MenuItem("Edit/Toggle Inspector Lock %l", true)]
    public static bool Valid() {
        return ActiveEditorTracker.sharedTracker.activeEditors.Length != 0;
    }
}

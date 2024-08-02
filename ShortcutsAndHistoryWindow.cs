using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;

public class ShortcutsAndHistoryWindow : EditorWindow
{
    private static List<GameObject> shortcutObjects = new List<GameObject>();
    private static Stack<GameObject> selectionHistoryBack = new Stack<GameObject>();
    private static Stack<GameObject> selectionHistoryForward = new Stack<GameObject>();
    private static Vector2 scrollPosition;
    private static GameObject lastSelectedObject;
    private static bool isNavigatingBack;
    private static bool isNavigatingForward;

    [MenuItem("Window/Shortcut Window")]
    public static void ShowWindow()
    {
        GetWindow<ShortcutsAndHistoryWindow>("Shortcut Window");
    }

    private void OnEnable()
    {
        Selection.selectionChanged += OnSelectionChanged;
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= OnSelectionChanged;
    }

    private void OnGUI()
    {
        // Insert custom GUI elements
        EditorGUILayout.Space();

        // Fixed horizontal layout for arrow buttons, clear button, and shortcut objects
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("←", GUILayout.Width(25), GUILayout.Height(30)))
        {
            NavigateBack();
        }

        if (GUILayout.Button("→", GUILayout.Width(25), GUILayout.Height(30)))
        {
            NavigateForward();
        }

        if (GUILayout.Button("Clear", GUILayout.Width(50), GUILayout.Height(30)))
        {
            RemoveAllShortcuts();
        }

        // Horizontal scroll view for shortcut objects
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(50), GUILayout.ExpandWidth(true));
        EditorGUILayout.BeginHorizontal();

        foreach (var obj in shortcutObjects)
        {
            if (obj != null)
            {
                GUIContent content = new GUIContent(AssetPreview.GetMiniThumbnail(obj), obj.name);
                if (GUILayout.Button(content, GUILayout.Width(30), GUILayout.Height(30)))
                {
                    if (Event.current.button == 0) // Left click
                    {
                        Selection.activeObject = obj;
                    }
                    else if (Event.current.button == 1) // Right click
                    {
                        GenericMenu menu = new GenericMenu();
                        GameObject target = obj; // Capture variable for the closure
                        menu.AddItem(new GUIContent("Remove"), false, () => RemoveShortcut(target));
                        menu.ShowAsContext();
                    }
                }
            }
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndHorizontal();

        // Handle mouse scroll wheel for horizontal scrolling
        HandleMouseScroll();

        // Handle drag and drop
        Rect dropArea = GUILayoutUtility.GetLastRect();
        HandleDragAndDrop(dropArea);

        GUILayout.Space(10); // Add spacing to separate custom GUI from the default inspector
    }

    private void HandleMouseScroll()
    {
        Event evt = Event.current;
        if (evt.type == EventType.ScrollWheel && GUILayoutUtility.GetLastRect().Contains(evt.mousePosition))
        {
            scrollPosition.x += evt.delta.y * 10; // Adjust the scroll speed as needed
            evt.Use();
        }
    }

    private static void HandleDragAndDrop(Rect dropArea)
    {
        Event evt = Event.current;

        if (evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform)
        {
            if (!dropArea.Contains(evt.mousePosition))
                return;

            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (evt.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();

                foreach (Object draggedObject in DragAndDrop.objectReferences)
                {
                    if (draggedObject is GameObject gameObject && !shortcutObjects.Contains(gameObject))
                    {
                        shortcutObjects.Add(gameObject);
                    }
                }
            }

            evt.Use();
        }
    }

    private static void OnSelectionChanged()
    {
        GameObject selectedObject = Selection.activeObject as GameObject;

        if (selectedObject != null && selectedObject != lastSelectedObject)
        {
            if (!isNavigatingBack && !isNavigatingForward && lastSelectedObject != null)
            {
                selectionHistoryBack.Push(lastSelectedObject);
                selectionHistoryForward.Clear();
            }

            isNavigatingBack = false;
            isNavigatingForward = false;

            lastSelectedObject = selectedObject;
        }
    }

    [Shortcut("Tools/Shortcut Window/Selection History Back", KeyCode.Z, ShortcutModifiers.Shift)]
    private static void NavigateBack()
    {
        if (selectionHistoryBack.Count <= 0) return;

        isNavigatingBack = true;

        GameObject obj = selectionHistoryBack.Pop();
        if (Selection.activeObject != null)
        {
            selectionHistoryForward.Push(Selection.activeObject as GameObject);
        }

        Selection.activeObject = obj;
    }

    [Shortcut("Tools/Shortcut Window/Selection History Forward", KeyCode.X, ShortcutModifiers.Shift)]
    private static void NavigateForward()
    {
        if (selectionHistoryForward.Count > 0)
        {
            isNavigatingForward = true;

            GameObject obj = selectionHistoryForward.Pop();
            if (Selection.activeObject != null)
            {
                selectionHistoryBack.Push(Selection.activeObject as GameObject);
            }

            Selection.activeObject = obj;
        }
    }

    private static void RemoveAllShortcuts()
    {
        shortcutObjects.Clear();
    }

    private static void RemoveShortcut(GameObject obj)
    {
        shortcutObjects.Remove(obj);
    }
}
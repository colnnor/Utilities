using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

public static class Helpers
{
    private static Camera camera;

    /// <summary>
    /// Gets the main camera in the scene.
    /// </summary>
    /// <returns>The main camera.</returns>
    public static Camera Camera
    {
        get
        {
            if (!camera) camera = Camera.main;
            if (!camera) camera = Object.FindFirstObjectByType<Camera>();
            return camera;
        }
    }

    private static PointerEventData eventDataCurrentPosition;
    private static List<RaycastResult> results;

    /// <summary>
    /// Checks if the mouse is over any UI element.
    /// </summary>
    /// <returns>True if the mouse is over a UI element, false otherwise.</returns>
    public static bool IsOverUI()
    {
        eventDataCurrentPosition = new(EventSystem.current) { position = Input.mousePosition };
        results = new();

        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    /// <summary>
    /// Gets the world real position of a canvas UI element.
    /// </summary>
    /// <param name="element">The RectTransform of the UI element.</param>
    /// <returns>The world position of the UI element.</returns>
    public static Vector2 GetWorldPositionOffCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera, out var result);
        return result;
    }


    /// <summary>
    /// Creates a world text object at the given location
    /// </summary>
    public static TextMeshPro CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default, Quaternion rotation = default, int fontSize = 11, Color? color = null,
        TextAlignmentOptions textAlignment = TextAlignmentOptions.Midline, int sortingOrder = 5000)
    {
        if (color == null) color = Color.white;
        return CreateWorldTextObject(parent, text, localPosition, rotation, fontSize, (Color)color, textAlignment, sortingOrder);
    }

    /// <summary>
    /// Creates a world text object at the given location
    /// </summary>
    public static TextMeshPro CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default, Quaternion rotation = default, Color? color = null, int fontSize = 11)
    {
        TextAlignmentOptions textAlignment = TextAlignmentOptions.Midline;
        int sortingOrder = 5000;
        if (color == null) color = Color.white;
        return CreateWorldTextObject(parent, text, localPosition, rotation, fontSize, (Color)color, textAlignment, sortingOrder);
    }

    private static TextMeshPro CreateWorldTextObject(Transform parent, string text, Vector3 localPosition, Quaternion rotation, int fontSize, Color color, TextAlignmentOptions textAlignment, int sortingOrder)
    {
        GameObject textObject = new("World_Text", typeof(TextMeshPro));
        Transform transform = textObject.transform;
        MeshRenderer renderer = textObject.GetOrAddComponent<MeshRenderer>();
        renderer.sortingOrder = sortingOrder;
        transform.SetParent(parent);
        transform.localPosition = localPosition;
        transform.localRotation = rotation;
        TextMeshPro textMesh = textObject.GetOrAddComponent<TextMeshPro>();
        textMesh.text = text;
        textMesh.color = color;
        textMesh.alignment = textAlignment;
        textMesh.fontSize = fontSize;
        return textMesh;
    }

    /// <summary>
    /// Returns the ceiling of a random range between min and max
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int GetRandomCeil(float min, float max)
    {
        return Mathf.RoundToInt(Random.Range(min, max));
    }
    
    public static Vector3 GetMouseWorldPosition(LayerMask? layerMask = null)
    {
        LayerMask mask = layerMask ?? Physics.DefaultRaycastLayers;
        
        Ray ray = Camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 10000f, mask))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
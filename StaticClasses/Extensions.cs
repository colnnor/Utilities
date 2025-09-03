using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

public static class Extensions
{
    #region Vector3 Extensions
    /// <summary>
    /// Returns a new Vector3 with the specified values, or the original values if not specified
    /// </summary>
    public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null) => new (x ?? vector.x, y ?? vector.y, z ?? vector.z);
    public static Vector3Int With(this Vector3Int vector, float? x = null, float? y = null, float? z = null) => new ((int)(x ?? vector.x), (int)(y ?? vector.y), (int)(z ?? vector.z));
    public static Vector3 WithOffset(this Vector3 vector, Vector3 offset) => new (vector.x + offset.x, vector.y + offset.y, vector.z + offset.z);
    public static Vector3 RandomOffset(this Vector3 vector, float range) => vector + UnityEngine.Random.insideUnitSphere * range;
    public static Vector3 RightFromUpDirection(this Vector3 up, Vector3 forward)
    {
        return Vector3.Cross(up, forward).normalized;
    }
    public static bool IsHorizontal(this Vector3 vector)
    {
        return Mathf.Abs(Vector3.Dot(vector, Vector3.right)) > 0.5f;
    }
    public static Vector3 Remap(this Vector3 vector, float min, float max)
    {
        vector.x = Mathf.Lerp(min, max, vector.x);
        vector.y = Mathf.Lerp(min, max, vector.y);
        vector.z = Mathf.Lerp(min, max, vector.z);
        return vector;
    }
    public static float RandomBetween(this Vector3 vector) => UnityEngine.Random.Range(vector.x, vector.y);

    public static Vector3Int ToInt(this Vector3 vector) => new Vector3Int(Mathf.CeilToInt(vector.x), Mathf.CeilToInt(vector.y), Mathf.CeilToInt(vector.z));
    /// <summary>
    /// Returns a new Vector3 with the specified values added to the original values
    /// </summary>
    public static Vector3 Add(this Vector3 vector, float? x = null, float? y = null, float? z = null) => new Vector3(vector.x + (x ?? 0), vector.y + (y ?? 0), vector.z + (z ?? 0));

    /// <summary>
    /// Randomizes a each value of the Vector3 between the specified min and max values
    /// </summary>
    public static Vector3 Random(this Vector3 vector, float min, float max) => new Vector3(UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max));
    public static Vector3 Random(this Vector3 vector, Vector3 minPosition, Vector3 maxPosition)
    {
        float x = UnityEngine.Random.Range(minPosition.x, maxPosition.x);
        float y = UnityEngine.Random.Range(minPosition.y, maxPosition.y);
        float z = UnityEngine.Random.Range(minPosition.z, maxPosition.z);
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Rounds each value of the Vector3 to the nearest whole number
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Vector3Int Round(this Vector3 vector) => new(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));

    public static Vector3 RoundToFloor(this Vector3 vector) => new(Mathf.Floor(vector.x), Mathf.Floor(vector.y), Mathf.Floor(vector.z));

    public static Vector3 RotationTowards(this Vector3 vector, Vector3 target) => vector - target;
    
    public static Vector3 Clamp(this Vector3 vector, Vector3 min, Vector3 max) => new Vector3(vector.x.Clamp(min.x, max.x), vector.y.Clamp(min.y, max.y), vector.z.Clamp(min.z, max.z));
    public static Vector3 ClampFloat(this Vector3 vector, float min, float max) => new Vector3(vector.x.Clamp(min, max), vector.y.Clamp(min, max), vector.z.Clamp(min, max));
    
    #endregion

    #region Vector2 Extensions
    /// <summary>
    /// Randomizes a each value of the Vector2 between the specified min and max values
    /// </summary>
    public static Vector2 Random(this Vector2 vector, float min, float max) => new Vector2(UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max));
    public static float RandomBetween(this Vector2 vector) => UnityEngine.Random.Range(vector.x, vector.y);

    public static Vector2 With(this Vector2 vector, float? x = null, float? y = null) => new (x ?? vector.x, y ?? vector.y);
    #endregion

    #region GameObject Extensions   

    /// <summary>
    /// Gets or adds a component to a GameObject.
    /// </summary>
    /// <returns>The component of type T.</returns>
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (!component)
            component = gameObject.AddComponent<T>();
        return component;
    }
    
    public static void SetPosition(this GameObject gameObject, Vector3 position)
    {
        gameObject.transform.position = position;
    }
    public static void SetRotation(this GameObject gameObject, Quaternion rotation)
    {
        gameObject.transform.rotation = rotation;
    }
    public static void SetScale(this GameObject gameObject, Vector3 scale)
    {
        gameObject.transform.localScale = scale;
    }
    public static void SetParent(this GameObject gameObject, Transform parent)
    {
        gameObject.transform.SetParent(parent);
    }

    public static void SetPositionAndRotation(this GameObject gameObject, Vector3 position, Quaternion rotation)
    {
        gameObject.transform.SetPositionAndRotation(position, rotation);
    }
    public static GameObject Instantiate(this GameObject obj, Vector3? position = null, Quaternion? rotation = null, Transform parent = null)
    {
        return Object.Instantiate(obj, position ?? Vector3.zero, rotation ?? Quaternion.identity, parent);
    }


    public static T GetComponentOrInChildren<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (!component)
            component = gameObject.GetComponentInChildren<T>();
        return component;
    }
    public static bool TryAddComponent<T>(this GameObject gameObject) where T : Component
    {
        if (!gameObject.GetComponent<T>()) return false;
        
        gameObject.AddComponent<T>();
        return true;
    }

    /// <summary>
    /// Returns null if the object is null, otherwise returns the object itself.
    /// </summary>
    /// <returns>The object itself if not null, otherwise null.</returns>
    public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;

    /// <summary>
    /// Destroys all child objects of the GameObject.
    /// </summary>
    public static void DestroyChildren(this GameObject gameObject)
    {
        gameObject.transform.DestroyChildren();
    }

    /// <summary>
    /// Disables all child objects of the GameObject.
    /// </summary>
    public static void DisableChildren(this GameObject gameObject)
    {
        gameObject.transform.DisableChildren();
    }
    
    public static void Destroy(this GameObject gameObject)
    {
        if(Application.isPlaying)
            Object.Destroy(gameObject);
        else
            Object.DestroyImmediate(gameObject);
    }

    /// <summary>
    /// Enables all child objects of the GameObject.
    /// </summary>
    public static void EnableChildren(this GameObject gameObject)
    {
        gameObject.transform.EnableChildren();
    }
    #endregion

    #region Transform Extensions
    /// <summary>
    /// Returns an enumerable collection of the children of the Transform.
    /// </summary>
    public static IEnumerable<Transform> Children(this Transform parent)
    {
        foreach (Transform child in parent)
            yield return child;
    }
    public static bool TryGetChild(this Transform transform, int index, out Transform child)
    {
        if(index >= 0 && index < transform.childCount)
        {
            child = transform.GetChild(index);
            return true;
        }
        child = null;
        return false;
    }
    public static void ResetLocal(this Transform transform, bool position = true, bool rotation = true, bool scale = true)
    {
        if (position) transform.localPosition = Vector3.zero;
        if (rotation) transform.localRotation = Quaternion.identity;
        if (scale) transform.localScale = Vector3.one;
    }
    public static T GetOrAddComponent<T>(this Transform transform) where T : Component
    {
        return transform.gameObject.GetOrAddComponent<T>();

    }
    /// <summary>
    /// Destroys all child objects of the Transform.
    /// </summary>
    public static void DestroyChildren(this Transform parent)
    {
        if (!Application.isPlaying)
        {
            while (parent.childCount > 0)
                Object.DestroyImmediate(parent.GetChild(0).gameObject);

            return;
        }

        parent.PerformActionOnChildren(child => Object.Destroy(child.gameObject));
    }
    
    public static T GetComponentOrInChildren<T>(this Transform transform) where T : Component
    {
        return transform.gameObject.GetComponentOrInChildren<T>();
    }
    public static void Destroy(this Transform transform)
    {
        if(transform == null) return;
        if(Application.isPlaying)
            Object.Destroy(transform.gameObject);
        else
            Object.DestroyImmediate(transform.gameObject);
    }
    public static List<Transform> GetChildren(this Transform parent)
    {
        List<Transform> children = new List<Transform>();
        for (int i = 0; i < parent.childCount; i++)
        {
            children.Add(parent.GetChild(i));
        }

        return children;
    }
    
    public static List<Transform> GetChildrenRecursive(this Transform parent)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            children.Add(child);
            children.AddRange(child.GetChildrenRecursive());
        }
        return children;
    }
    
    public static List<GameObject> GetChildrenRecursive(this GameObject parent)
    {
        return parent.transform.GetChildrenRecursive().Select(t => t.gameObject).ToList();
    }

    /// <summary>
    /// Disables all child objects of the Transform.
    /// </summary>
    public static void DisableChildren(this Transform parent)
    {
        parent.PerformActionOnChildren(child => child.gameObject.SetActive(false));
    }

    /// <summary>
    /// Enables all child objects of the Transform.
    /// </summary>
    public static void EnableChildren(this Transform parent)
    {
        parent.PerformActionOnChildren(child => child.gameObject.SetActive(true));
    }

    private static void PerformActionOnChildren(this Transform parent, System.Action<Transform> action)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
            action(parent.GetChild(i));
    }
    #endregion

    #region Float Extensions

    public static float Quadratic(this float value, float spacing, int index, float vertex)
    {
        return spacing * (index - vertex).Squared() + vertex;
    }
    
    public static float Abs(this float value)
    {
        return Mathf.Abs(value);
    }
    public static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        if (toMax > toMin)
            return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
            
        return toMax + (fromMax - value) * (toMin - toMax) / (fromMax - fromMin);
    }

    public static float Squared(this float value) => value * value;

    public static float Random(this float value, float min, float max) => UnityEngine.Random.Range(min, max);
    public static float AddRandom(this float value, float range) => value + UnityEngine.Random.Range(-range, range);

    public static bool Proximity(this float value, float target, float range)
    {
        return Mathf.Abs(value - target) <= range;
    }


    public static float Clamp(this float value, float min, float max)
    {
        return Mathf.Clamp(value, min, max);
    }

    public static float RoundToNearest(this float value, float nearest)
    {
        return Mathf.Round(value / nearest) * nearest;
    }

    public static int ToInt(this float value)
    {
        return Mathf.RoundToInt(value);
    }

    public static float ToPercent(this float value)
    {
        return value * 100;
    }
    #endregion

    #region Bool Extensions

    public static bool Invert(this bool value)
    {
        return !value;
    }

    #endregion

    #region int Extensions

    public static int Clamp(this int value, int min, int max)
    {
        return Mathf.Clamp(value, min, max);
    }
    public static int Squared(this int value) => value * value;
    public static int Random(this int value, int min, int max) => UnityEngine.Random.Range(min, max);
    public static string ToOrdinalString(this int number)
    {
        if (number <= 0)
        {
            Debug.LogError("ToOrdinalString() requires a positive integer.");
            return number.ToString();
        }
        switch (number % 100)
        {
            case 11:
            case 12:
            case 13:
                return number + "th";
        }

        switch (number % 10)
        {
            case 1:
                return number + "st";
            case 2:
                return number + "nd";
            case 3:
                return number + "rd";
            default:
                return number + "th";
        }
    }
    public static bool IsPositive(this int value) => value > 0;
    public static bool IsEven(this int value) => value % 2 == 0;

    #endregion

    #region List Extensions
    
    public static bool TryGet<T>(this List<T> list, T input, out T result)
    {
        if(list.Contains(input))
        {
            result = input;
            return true;
        }
        result = default;
        return false;
    }

    public static bool TryGetRandom<T>(this List<T> list, out T result)
    {
        if (list.Count == 0)
        {
            result = default;
            return false;
        }

        result = list[UnityEngine.Random.Range(0, list.Count)];
        return true;
    }
    public static T GetRandom<T>(this List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
    public static List<T> Shuffle<T>(this List<T> list)
    {
        Random rng = new();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }
    /// <summary>
    /// Clears the list and destroys all MonoBehaviour GameObjects in it.
    /// </summary>
    public static List<T> ClearAndDestroy<T>(this List<T> list) where T : MonoBehaviour
    {
        foreach (var item in list.Where(item => item).Select(i => i.gameObject))
        {
            Object.Destroy(item);
        }
        list.Clear();
        return list;
    }
    public static void AddMany<T>(this List<T> list, params T[] items)
    {
        list.AddRange(items);
    }

    #endregion
    
    #region Color Extensions
    
    public static Color Random(this Color color, float min, float max)
    {
        return new Color(UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max));
    }
    
    public static Color WithAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
    
    #endregion
    
    #region Material Extensions
    
    public static Material CreateInstance(this Material material)
    {
        return new Material(material);
    }
    
    #endregion
    
    #region RenderTexture Extensions
    
    public static void SetWidthAndHeight(this RenderTexture renderTexture, int width, int height)
    {
        renderTexture.width = width;
        renderTexture.height = height;
    }
    public static RenderTexture CreateInstance(this RenderTexture renderTexture)
    {
        return new RenderTexture(renderTexture);
    }
    
    #endregion
    
    #region VisualElementExtensions
    public static VisualElement CreateChild(this VisualElement parent, params string[] classes)
    {
        var child = new VisualElement();
        child.AddClass(classes).AddTo(parent);
        return child;
    }
    
    public static T CreateChild<T>(this VisualElement parent, params string[] classes) where T : VisualElement, new()
    {
        var child = new T();
        child.AddClass(classes).AddTo(parent);
        return child;
    }
    public static VisualElement AddTo(this VisualElement child, VisualElement parent)
    {
        parent.Add(child);
        return child;
    }
    
    public static T AddClass<T>(this T element, params string[] classes) where T : VisualElement
    {
        foreach (var className in classes)
        {
            if(!string.IsNullOrEmpty(className))
                element.AddToClassList(className);
        }
        return element;
    }
    
    public static T WithManipulator<T>(this T element, IManipulator manipulator) where T : VisualElement
    {
        element.AddManipulator(manipulator);
        return element;
    }
    #endregion
    
    #region Queue Extensions
    
    public static void Remove<T>(this Queue<T> queue, T itemToRemove)
    {
        var tempQueue = new Queue<T>();
        while(queue.Count > 0)
        {
            T item = queue.Dequeue();
            if (!item.Equals(itemToRemove))
            {
                tempQueue.Enqueue(item);
            }
        }
        while(tempQueue.Count > 0)
        {
            queue.Enqueue(tempQueue.Dequeue());
        }
    }
    #endregion
}
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    public bool AutoUnparentOnAwake = true;

    protected static T instance;

    public static bool HasInstance => instance;
    public static T TryGetInstance() => HasInstance ? instance : null;

    public static T Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindAnyObjectByType<T>();
                if(!instance)
                {
                    var go = new GameObject(typeof(T).Name + " Auto-Generated");
                    Debug.LogWarning($"No instance of {typeof(T).Name} found in the scene. An instance has been auto-generated.", go);
                    instance = go.AddComponent<T>();
                }
            }

            return instance;
        }
    }
    protected virtual void Awake()
    {
        InitializeSingleton();
    }

    protected virtual void InitializeSingleton()
    {
        if (!Application.isPlaying) return;

        if (AutoUnparentOnAwake)
        {
            transform.SetParent(null);
        }

        if (!instance)
        {
            instance = this as T;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
    
}
using Unity.Netcode;
using UnityEngine;

public class NetworkSingleton<T> : NetworkBehaviour where T : Component
{
    public bool AutoUnparentOnAwake = true;
    protected static T instance;

    public static bool HasInstance => instance;
    public static T TryGetInstance() => HasInstance ? instance : null;

    public static T Instance
    {
        get
        {
            if (instance) return instance;
            instance = FindFirstObjectByType<T>();
            if (instance) return instance;
            
            var go = new GameObject(typeof(T).Name + " Auto-Generated");
            go.AddComponent<NetworkObject>();
            instance = go.AddComponent<T>();

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
            DontDestroyOnLoad(gameObject);
        }
        
        if (!instance)
        {
            instance = this as T;

        }
        else
        {
            if (instance == this) return;
            
            if(gameObject.TryGetComponent(out NetworkObject no) && no.IsSpawned)
            {
                no.Despawn();
            }
        }
    }
}
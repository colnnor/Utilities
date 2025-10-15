using System;
using System.Collections.Generic;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Observable<T>
{
    [SerializeField] private T value;
    public event Action<T> OnValueChanged = delegate { };
    
    public T Value
    {
        get => value;
        set => Set(value);
    }
    
    public static implicit operator T(Observable<T> observable) => observable.value;

    public Observable(T value, Action<T> callback = null)
    {
        this.value = value;
        if(callback != null) OnValueChanged += callback;
    }

    public void Set(T newValue)
    {
        if(EqualityComparer<T>.Default.Equals(value, newValue)) return;
        
        value = newValue;
        Invoke();
    }
    
    public void Invoke()
    {
        OnValueChanged?.Invoke(value);
    }
    
    public void AddListener(Action<T> callback) => OnValueChanged += callback;

    public void RemoveListener(Action<T> callback) => OnValueChanged -= callback;

    public void Dispose()
    {
        OnValueChanged = null;
        value = default;
    }
}

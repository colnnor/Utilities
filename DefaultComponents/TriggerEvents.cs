using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TriggerEvents : MonoBehaviour
{
    public event Action<Collider> onTriggerEnter;
    public event Action<Collider> onTriggerExit;

    [Serializable] public class TriggerEnterEvent : UnityEvent
    {
    }

    [Serializable] public class TriggerExitEvent : UnityEvent
    {
    }

    public TriggerEnterEvent triggerEnterEvent = new();
    public TriggerExitEvent triggerExitEvent = new();

    private bool isEnabled;

    private List<Collider> colliders = new List<Collider>();

    public void Enable(bool enabled = true)
    {
        if (!enabled)
        {
            foreach (var col in colliders)
            {
                onTriggerExit?.Invoke(col);
            }
            
            colliders.Clear();
            triggerExitEvent.Invoke();
        }
        else
        {
            colliders = new List<Collider>();
        }

        isEnabled = enabled;
    }

    private void Reset()
    {
        if (!TryGetComponent(out Collider _))
        {
            SphereCollider sphereCol = gameObject.AddComponent<SphereCollider>();
            sphereCol.radius = 1f;
            sphereCol.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        if (colliders.Contains(other)) return; // Prevent duplicate entries
        colliders.Add(other);

        onTriggerEnter?.Invoke(other);
        triggerEnterEvent.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!enabled) return;

        if (!colliders.Contains(other)) return; // Prevent duplicate exits
        colliders.Remove(other);

        onTriggerExit?.Invoke(other);
        triggerExitEvent.Invoke();
    }
}
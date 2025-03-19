using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{
    public event Action<Collider> TriggerEnter;
    public event Action<Collider> TriggerExit;
    
    [Serializable] public class TriggerEnterEvent : UnityEvent {}
    [Serializable] public class TriggerExitEvent : UnityEvent {}
    
    public TriggerEnterEvent triggerEnterEvent = new ();
    public TriggerExitEvent triggerExitEvent = new ();
    
    private void OnTriggerEnter(Collider other)
    {
        TriggerEnter?.Invoke(other);
        triggerEnterEvent.Invoke();
    }
    
    private void OnTriggerExit(Collider other)
    {
        TriggerExit?.Invoke(other);
        triggerExitEvent.Invoke();
    }
}

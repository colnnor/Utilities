using UnityEngine;
using UnityEngine.Events;

public class OnEnableEvents : MonoBehaviour
{
    public UnityEvent onEnableEvent;
    public UnityEvent onDisableEvent;
    private void OnEnable()
    {
        onEnableEvent?.Invoke();
    }
    private void OnDisable()
    {
        onDisableEvent?.Invoke();
    }
    
    public void AddListener(UnityAction action)
    {
        onEnableEvent.AddListener(action);
    }
}

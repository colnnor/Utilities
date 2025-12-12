using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class KeyDownEvents : MonoBehaviour
{
    [SerializeField] private Key eventKey;
    
    public UnityEvent onKeyDownEvent = new();
    private void Update()
    {
        if(Inputs.GetKeyDown(eventKey))
        {
            onKeyDownEvent.Invoke();
        }
    }
}
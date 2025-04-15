using UnityEngine;

public interface IRaycastable
{
    public void OnRaycastEnter()
    {
        
    }

    public void OnRaycastExit()
    {
        
    }

    public void OnRaycastStay()
    {
        
    }
    
    public Transform transform { get; }
    public GameObject gameObject { get; }
}
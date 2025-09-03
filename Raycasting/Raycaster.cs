using System;
using UnityEngine;

[Serializable]
public class Raycaster
{
    public Transform raycastOrigin;
    public float raycastDistance;
    public LayerMask raycastLayerMask;
    
    private Ray ray;
    public IRaycastable CurrentRaycastable { get; private set; }

    public event Action<IRaycastable> OnRaycastEnter;
    public event Action<IRaycastable> OnRaycastExit;

    public bool HasRaycastable => CurrentRaycastable != null;
    public Vector3 HitPoint { get; private set; }
    public Raycaster(Transform raycastOrigin, float raycastDistance, LayerMask raycastLayerMask)
    {
        this.raycastOrigin = raycastOrigin;
        this.raycastDistance = raycastDistance;
        this.raycastLayerMask = raycastLayerMask;
        
        Debug.Log("Raycaster created");
    }

    public void UpdateRaycaster()
    {
        ray = new Ray(raycastOrigin.position, raycastOrigin.forward );

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, raycastLayerMask))
        {
            if (hit.collider.TryGetComponent(out IRaycastable raycastable))
            {
                if (CurrentRaycastable != raycastable)
                {
                    NewRaycastHit(raycastable);
                }
                else
                {
                    RaycastStay(hit);
                }
            }
            else
            {
                RaycastExit();
            }
        }
        else
        {
            RaycastExit();
        }
    }

    private void NewRaycastHit(IRaycastable raycastable)
    {
        CurrentRaycastable?.OnRaycastExit();
        CurrentRaycastable = raycastable;
        CurrentRaycastable.OnRaycastEnter();

        OnRaycastEnter?.Invoke(CurrentRaycastable);
    }

    private void RaycastStay(RaycastHit hit)
    {
        CurrentRaycastable.OnRaycastStay();
        HitPoint = hit.point;
    }

    private void RaycastExit()
    {
        OnRaycastExit?.Invoke(CurrentRaycastable);

        CurrentRaycastable?.OnRaycastExit();
        CurrentRaycastable = null;
    }
}
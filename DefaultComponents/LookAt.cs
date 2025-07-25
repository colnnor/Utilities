using System;
//using Sirenix.OdinInspector;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 upAxis = Vector3.up;
    
    [Header("Lock Axis")]
    [SerializeField] private bool lockX;
    [SerializeField] private bool lockY;
    [SerializeField] private bool lockZ;
    
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    private void Update()
    {
        if (target == null) return;
        Vector3 lookAtPosition = target.position;
        Vector3 direction = lookAtPosition - transform.position;
        if (lockX) direction.x = 0;
        if (lockY) direction.y = 0;
        if (lockZ) direction.z = 0;
        transform.rotation = Quaternion.LookRotation(direction, upAxis);
    }
}

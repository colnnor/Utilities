using System;
//using Sirenix.OdinInspector;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 upAxis = Vector3.up;
    [SerializeField] private bool localUp;
    [SerializeField] private bool invertDirection;
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
        if (invertDirection) direction = -direction;
        Quaternion targetRotation = Quaternion.LookRotation(direction, localUp ? transform.up : upAxis);
        Vector3 euler = targetRotation.eulerAngles;
        if (lockX) euler.x = transform.eulerAngles.x;
        if (lockY) euler.y = transform.eulerAngles.y;
        if (lockZ) euler.z = transform.eulerAngles.z;
        transform.rotation = Quaternion.Euler(euler);
    }
}

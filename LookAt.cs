using Sirenix.OdinInspector;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 upAxis = Vector3.up;
    
    [Title("Lock Axis")]
    [SerializeField, LabelWidth(15), LabelText("X")] private bool lockX;
    [SerializeField, LabelWidth(15), LabelText("Y")] private bool lockY;
    [SerializeField, LabelWidth(15), LabelText("Z")] private bool lockZ;
    
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

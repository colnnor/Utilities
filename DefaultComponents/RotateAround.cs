using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private Transform rotationCenter;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private int circleResolution = 36;

    private Vector3 startOffset;

    private void Start()
    {
        CacheStartOffset();
    }

    private void OnValidate()
    {
        if (!rotationCenter)
            return;

        CacheStartOffset();
    }

    private void Update()
    {
        if (!rotationCenter)
            return;

        var axis = GetAxis();
        transform.RotateAround(rotationCenter.position, axis, rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        if (!rotationCenter)
            return;

        var axis = GetAxis();
        var radius = Vector3.Distance(rotationCenter.position, transform.position);
        var offset = transform.position - rotationCenter.position;
        var orbitDirection = Vector3.ProjectOnPlane(offset, axis);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(rotationCenter.position, transform.position);

        if (orbitDirection.sqrMagnitude > 0.0001f)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(rotationCenter.position, orbitDirection.normalized * radius);
        }

        Gizmos.color = Color.green;
        MyGizmos.DrawCircle(rotationCenter.position, radius, axis, circleResolution);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(rotationCenter.position, axis.normalized * Mathf.Max(0.5f, radius * 0.15f));
    }

    private Vector3 GetAxis()
    {
        var axis = rotationAxis.normalized;
        if (axis.sqrMagnitude < 0.0001f)
            axis = Vector3.up;

        return axis;
    }

    private void CacheStartOffset()
    {
        if (!rotationCenter)
            return;

        var axis = GetAxis();
        var offset = transform.position - rotationCenter.position;
        var planeOffset = Vector3.ProjectOnPlane(offset, axis);

        if (planeOffset.sqrMagnitude > 0.0001f)
            startOffset = planeOffset.normalized * offset.magnitude;
        else
            startOffset = Quaternion.AngleAxis(90f, axis) * Vector3.right * offset.magnitude;
    }

    [ContextMenu("Use Current Position As Start Orbit")]
    public void UseCurrentPositionAsStartOrbit()
    {
        CacheStartOffset();
    }

    public Vector3 GetStartOffset()
    {
        return startOffset;
    }
}
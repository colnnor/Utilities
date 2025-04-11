using System;
using UnityEngine;
using static Axis;

public class RotateByLocation : MonoBehaviour
{
    [SerializeField] private AxisType locationAxisToTrack;
    [SerializeField] private AxisType axisToRotate;
    [SerializeField] private bool invertRotation;
    [Tooltip("Distance to move for a full rotation")]
    [SerializeField] private float fullRotationDistance = 1;
    [Tooltip("Degrees to rotate per one unit moved")]
    [FloatIncrement]
    [SerializeField] private float rotationAmount;
    private float delta;
    private Vector3 offset;
    private float currentRotation;

    private void Start()
    {
        offset = transform.eulerAngles;
    }

    private void Update()
    {
        delta = GetPositionDelta();
        if (Mathf.Abs(delta) > 0)
        {
            currentRotation = delta * rotationAmount / fullRotationDistance;
            currentRotation = Mathf.Repeat(currentRotation, 360);
        }
        Vector3 axis = GetAxis(axisToRotate) * (invertRotation ? -1 : 1);
            
        transform.eulerAngles = new Vector3(
            axis.x * currentRotation,
            axis.y * currentRotation,
            axis.z * currentRotation
        ) + offset;
    }
    float GetPositionDelta()
    {
        return locationAxisToTrack switch
        {
            AxisType.None => 0,
            AxisType.X => transform.position.x,
            AxisType.Y => transform.position.y,
            AxisType.Z => transform.position.z,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
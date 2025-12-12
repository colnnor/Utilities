using System;
using UnityEngine;
using UnityEngine.InputSystem;
using AxisType = Axis.AxisType;

public class SimpleMovement : MonoBehaviour
{
    public AxisType horizontalAxis = AxisType.X;
    public AxisType verticalAxis = AxisType.Y;
    
    public float speed = 1f;
    public float speedRamp = 10f;

    private float currentSpeed;
    private Vector3 movementDelta;

    private void Start()
    {
        currentSpeed = speed;
    }

    void Update()
    {
        float horizontal = Inputs.GetAxis("Horizontal");
        float vertical = Inputs.GetAxis("Vertical");
        
        Vector3 targetDelta = Axis.GetAxis(horizontalAxis) * horizontal + Axis.GetAxis(verticalAxis) * vertical;
        movementDelta = Vector3.Lerp(movementDelta, targetDelta, Time.deltaTime * speedRamp);
        currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * speedRamp);
        transform.position += movementDelta * (currentSpeed * Time.deltaTime);
    
        //rotate towards movement direction
        if (movementDelta.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDelta.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speedRamp);
        }
    }
    
    
}
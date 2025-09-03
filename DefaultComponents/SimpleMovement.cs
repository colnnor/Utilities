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
        float horizontalInput = Keyboard.current.aKey.isPressed ? -1f : Keyboard.current.dKey.isPressed ? 1f : 0f;
        float verticalInput = Keyboard.current.wKey.isPressed ? 1f : Keyboard.current.sKey.isPressed ? -1f : 0f;
        
        Vector3 horiz = Axis.GetAxis(horizontalAxis) * horizontalInput;
        Vector3 vert = Axis.GetAxis(verticalAxis) * verticalInput;

        movementDelta = (horiz + vert).normalized;
        
        if (movementDelta.magnitude > 0)
        {
            transform.position += transform.forward * (currentSpeed * Time.deltaTime);
        }
        
        float rotationY = Keyboard.current.leftArrowKey.isPressed ? -1f : Keyboard.current.rightArrowKey.isPressed ? 1f : 0f;
        if (rotationY != 0)
        {
            transform.Rotate(Vector3.up, rotationY * speed * 50 * Time.deltaTime);
        }
    }
    
}
using UnityEngine;
using AxisType = Axis.AxisType;

public class SimpleMovement : MonoBehaviour
{
    public AxisType horizontalAxis = AxisType.X;
    public AxisType verticalAxis = AxisType.Y;
    
    public float speed = 1f;
    public float speedRamp = 10f;
    
    private Vector3 movementDelta;
    
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Horizontal");
        
        Vector3 horiz = Axis.GetAxis(horizontalAxis) * horizontalInput;
        Vector3 vert = Axis.GetAxis(verticalAxis) * verticalInput;

        movementDelta = (horiz + vert).normalized;
        
        if (movementDelta.magnitude > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + movementDelta, speed * Time.deltaTime);
        }
        else
        {
            speed = Mathf.Max(1f, speed - speedRamp * Time.deltaTime); // Ramp down speed but keep it above 1
        }
    }
    
}
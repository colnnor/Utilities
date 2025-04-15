using UnityEngine;
using static Axis;

public class SimpleMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    
    [SerializeField] private AxisType horizontalAxis = AxisType.X;
    [SerializeField] private AxisType verticalAxis = AxisType.Y;
    
    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 horizontalAxisVector = GetAxis(horizontalAxis);
        
        float vertical = Input.GetAxis("Vertical");
        Vector3 verticalAxisVector = GetAxis(verticalAxis);
        
        Vector3 moveDirection = horizontalAxisVector * horizontal + verticalAxisVector * vertical;
        transform.position += moveDirection * (moveSpeed * Time.deltaTime);
    }
}
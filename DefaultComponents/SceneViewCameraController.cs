using UnityEngine;

public class SceneViewCameraController : MonoBehaviour
{
    public float panSpeed = 0.1f;
    public float rotateSpeed = 5f;
    public float zoomSpeed = 10f;
    public float minZoomDistance = 1f;
    public float maxZoomDistance = 100f;

    private Vector3 lastMousePosition;
    private float distanceToTarget;
    private Transform target;

    void Start()
    {
        // Set an initial distance to the target for zooming purposes
        if (target == null)
        {
            GameObject targetGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
            target = targetGO.transform;
            target.GetComponent<BoxCollider>().enabled = false;
            target.localScale = Vector3.one * 0.1f;
            target.position = transform.position + transform.forward * 10f;
        }

        distanceToTarget = Vector3.Distance(transform.position, target.position);
    }

    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButton(2))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 panDirection = -transform.right * delta.x + -transform.up * delta.y;
            transform.position += panDirection * (panSpeed * Time.deltaTime);
        }
        
        if(Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftAlt))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                target.position = hit.point;
                distanceToTarget = Vector3.Distance(transform.position, target.position);
                Debug.Log($"Target position: {target.position}");
            }
        }

        if (Input.GetMouseButton(1))
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float forward = 0;
            
            if (Input.GetKey(KeyCode.E))
            {
                forward = 1;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                forward = -1;
            }
            if (horizontal != 0 || vertical != 0 || forward != 0)
            {
                Vector3 panDirection = transform.right * horizontal + transform.forward * vertical + transform.up * forward;
                transform.position += panDirection * (panSpeed * 10 * Time.deltaTime);
            }

            Vector3 delta = Input.mousePosition - lastMousePosition;
            float rotationX = -delta.y * rotateSpeed * Time.deltaTime;
            float rotationY = delta.x * rotateSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up, rotationY);
            transform.Rotate(Vector3.right, rotationX);
        }

        // Rotate with Alt + Left Mouse Button
        if(Input.GetKeyDown(KeyCode.LeftAlt) && Input.GetMouseButton(0))
        {
            target.position = transform.position + transform.forward * distanceToTarget;
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float rotationX = -delta.y * rotateSpeed * Time.deltaTime;
            float rotationY = delta.x * rotateSpeed * Time.deltaTime;
            
            transform.RotateAround(target.position, transform.right, rotationX);
            transform.RotateAround(target.position, Vector3.up, rotationY);
        }

        // Zoom with scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            distanceToTarget -= scroll * zoomSpeed;
            distanceToTarget = Mathf.Clamp(distanceToTarget, minZoomDistance, maxZoomDistance);

            transform.position = target.position - transform.forward * distanceToTarget;
        }

        // Update last mouse position
        transform.eulerAngles = transform.eulerAngles.With(z: 0);

        lastMousePosition = Input.mousePosition;
    }
}
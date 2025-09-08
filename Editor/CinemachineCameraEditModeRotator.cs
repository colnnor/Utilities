using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class CinemachineCameraEditModeRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Key lefKey = Key.A;
    [SerializeField] private Key rightKey = Key.D;
    private void Update()
    {
        Debug.Log("Update");
        if(Application.isPlaying || !CinemachineCameraSwitcher.activeCamera || !CinemachineCameraSwitcher.rotationActive) return;
        Debug.Log("Rotating");
        float horizontalInput = 0f;
        if (Keyboard.current[lefKey].isPressed) horizontalInput -= 1f;
        if (Keyboard.current[rightKey].isPressed) horizontalInput += 1f;
        if (Mathf.Approximately(horizontalInput, 0f)) return;
        
        CinemachineCamera activeCamera = CinemachineCameraSwitcher.activeCamera;
        if (activeCamera == null) return;
        
        activeCamera.transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime, Space.World);
    }
}
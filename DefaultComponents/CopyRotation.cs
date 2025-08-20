//using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteAlways]
public class CopyRotation : MonoBehaviour
{
    [SerializeField] private bool local;
    //[HideIf("uniform")]
    [SerializeField] private bool copyX = true;
    //[HideIf("uniform")]
    [SerializeField] private bool copyY = true;
    //[HideIf("uniform")]
    [SerializeField] private bool copyZ = true;

    [SerializeField] private Transform targetTransform;

    private void Update()
    {
        if (!targetTransform) return;

        var targetRotation = targetTransform.rotation;
        var currentRotation = local ? transform.localRotation : transform.rotation;

        var newRotation = new Quaternion(
            copyX ? targetRotation.x : currentRotation.x,
            copyY ? targetRotation.y : currentRotation.y,
            copyZ ? targetRotation.z : currentRotation.z,
            currentRotation.w);

        if (local)
        {
            transform.localRotation = newRotation;
        }
        else
        {
            transform.rotation = newRotation;
        }
    }
}
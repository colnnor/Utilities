using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteAlways]
public class CopyRotation : MonoBehaviour
{
    [SerializeField] private bool local;
    [SerializeField] private bool uniform = true;
    [HideIf("uniform")]
    [SerializeField] private bool copyX = true;
    [HideIf("uniform")]
    [SerializeField] private bool copyY = true;
    [HideIf("uniform")]
    [SerializeField] private bool copyZ = true;

    [SerializeField] private Transform targetTransform;

    private void Update()
    {
        if (!targetTransform) return;

        Vector3 targetRotation = local ? targetTransform.localEulerAngles : targetTransform.eulerAngles;

        if (!uniform)
        {
            if (copyX) targetRotation.x = targetRotation.x;
            if (copyY) targetRotation.y = targetRotation.y;
            if (copyZ) targetRotation.z = targetRotation.z;
        }

        if (local)
        {
            transform.localRotation = Quaternion.Euler(targetRotation);
            return;
        }

        transform.rotation = Quaternion.Euler(targetRotation);
    }
}
using UnityEngine;

[ExecuteInEditMode]
public class LockZPosition : MonoBehaviour
{
    private void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
            SetPosition();
    }

    private void SetPosition()
    {
        transform.position = transform.position.With(z: 0);
    }
}
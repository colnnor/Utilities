using System;
using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteInEditMode]
public class LockZPosition : MonoBehaviour
{
    [ContextMenu("Set Editable")]
    private void SetEditable()
    {
        this.hideFlags = HideFlags.None;
    }
    [ContextMenu("Set Not Editable")]
    private void SetNotEditable()
    {
        this.hideFlags = HideFlags.NotEditable;
    }

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
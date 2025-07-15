using System;
using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
public class LockZPosition : MonoBehaviour
{
    private bool locking;
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
            SetPosition();
    }

    private void SetPosition()
    {
        locking = true;
        transform.position = transform.position.With(z: 0);
    }
}
using System;
using UnityEditor;
using UnityEngine;

public class VisualizeGizmo : MonoBehaviour
{
    public  bool drawGizmos = true;
    private void OnDrawGizmos()
    {
        if(drawGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, Vector3.one  * .1f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.up);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        }
    }
}
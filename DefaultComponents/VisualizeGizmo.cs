using System;
using UnityEditor;
using UnityEngine;

public class VisualizeGizmo : MonoBehaviour
{
    public  bool drawGizmos = true;
    public bool drawCollider = true;
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
        
        if(drawCollider)
        {
            Collider col = GetComponent<Collider>();
            if (col == null) return;
            Gizmos.color = Color.cyan;
           
            if(col is BoxCollider box)
            {
                Gizmos.matrix = Matrix4x4.TRS(box.transform.position, box.transform.rotation, box.transform.lossyScale);
                Gizmos.DrawWireCube(box.center, box.size);
            }
            else if(col is SphereCollider sphere)
            {
                Gizmos.matrix = Matrix4x4.TRS(sphere.transform.position, sphere.transform.rotation, sphere.transform.lossyScale);
                Gizmos.DrawWireSphere(sphere.center, sphere.radius);
            }
            else if(col is CapsuleCollider capsule)
            {
                Gizmos.matrix = Matrix4x4.TRS(capsule.transform.position, capsule.transform.rotation, capsule.transform.lossyScale);
                Vector3 point1 = capsule.center + Vector3.up * (capsule.height / 2 - capsule.radius);
                Vector3 point2 = capsule.center - Vector3.up * (capsule.height / 2 - capsule.radius);
                Gizmos.DrawWireSphere(point1, capsule.radius);
                Gizmos.DrawWireSphere(point2, capsule.radius);
            }
            // Add more collider types as needed
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteAlways]
public class Mirror : MonoBehaviour
{
    [SerializeField] private bool mirrorX;
    [SerializeField] private bool mirrorY;
    [SerializeField] private bool mirrorZ;
    
    [SerializeField] private Transform mirrorObject;

    private void Update()
    {
        if(!mirrorObject) return;
        float x = transform.localPosition.x;
        float y = transform.localPosition.y;
        float z = transform.localPosition.z;
        if (mirrorX) x = -x;
        if (mirrorY) y = -y;
        if (mirrorZ) z = -z;
        
        mirrorObject.localPosition = new Vector3(x, y, z);
    }
}

using System;
using UnityEngine;

[ExecuteAlways]
public class ZeroChildren : MonoBehaviour
{
    private int childCount;
    private void Update()
    {
        if(transform.childCount != childCount)
        {
            childCount = transform.childCount;
            foreach (var child in transform.Children())
            {
                child.localPosition = Vector3.zero;
            }
        }
    }
}

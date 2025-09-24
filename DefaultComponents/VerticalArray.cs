using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

using UnityEngine;

[ExecuteAlways]
public class VerticalArray : MonoBehaviour
{
    [SerializeField] private Axis.AxisType axis = Axis.AxisType.Y;
    [SerializeField] private float spacing = 0.1f;
    
    public float Spacing => spacing;
    
#if ODIN_INSPECTOR
    [Button]
#endif
    void AddChild()
    {
        Transform childOne = transform.GetChild(0);
        Instantiate(childOne, transform);
    }
    
#if ODIN_INSPECTOR
    [Button]
#endif
    void RemoveLastChild()
    {
        Transform lastChild = transform.GetChild(transform.childCount - 1);
        lastChild.Destroy();

    }
    
    private int childCount;
    private void Start()
    {
        ArrangeChildren();
    }

    private void Update()
    {
        if (childCount != transform.childCount)
        {
            ArrangeChildren();
            childCount = transform.childCount;
        }
    }

    private void OnValidate()
    {
        ArrangeChildren();
    }

    public void SetSpacing(float spacing)
    {
        this.spacing = spacing;
        ArrangeChildren();
    }
    private void ArrangeChildren()
    {
        Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }
        
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i] == null) continue;

            Vector3 position = children[i].localPosition;
            Vector3 axisPosition = Axis.GetAxis(axis);
            position = axisPosition * (i * spacing);
            children[i].localPosition = position;
        }
    }

}
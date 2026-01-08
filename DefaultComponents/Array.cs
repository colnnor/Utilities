using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

[ExecuteAlways]
public class Array : MonoBehaviour
{
    public enum ArrayType
    {
        Grid,
        Axis,
        Radial
    }

    #region Radial

    [SerializeField] private int count = 10;
    [SerializeField] private bool faceCenter = false;
    [SerializeField] private Vector3 individualRotationOffset;
    [SerializeField] private Vector3 constantRotationOffset;
    [SerializeField] private float radius = 5f;

    List<Transform> children = new List<Transform>();

    private void OnValidate()
    {
        if (count < 1) count = 1;
        if (radius < 0.1f) radius = 0.1f;

        UpdateArray();
    }

    private void Start()
    {
        UpdateArray();
    }

    private void UpdateArray()
    {
        children = transform.GetChildren();
        VerifyChildCount();

        float angleStep = 360f / count;
        for (int i = 0; i < children.Count; i++)
        {
            if (!children[i]) continue;

            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            children[i].localPosition = newPos;
            if (faceCenter)
            {
                Vector3 directionToCenter = (transform.position - children[i].position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(directionToCenter, Vector3.up);
                children[i].rotation = lookRotation * Quaternion.Euler(constantRotationOffset);
            }
            else
            {
                Vector3 eulerAngles = children[i].eulerAngles.With(y: individualRotationOffset.y * i);
                children[i].localRotation = Quaternion.Euler(eulerAngles);
                
            }
        }
    }

    
#if ODIN_INSPECTOR
    [Button]
#endif
    void RepopulateChildren()
    {
        int tempCount = count;
        count = 1;
        VerifyChildCount();
        count = tempCount;
        UpdateArray();
    }

    private void VerifyChildCount()
    {
        if (children.Count > count)
        {
            for (int i = children.Count - 1; i >= count; i--)
            {
                children[i].Destroy();
                children.RemoveAt(i);
            }
        }
        else if (children.Count < count)
        {
            for (int i = children.Count; i < count; i++)
            {
                if (transform.TryGetChild(0, out Transform child0))
                {
                    Transform child = Instantiate(child0, transform);
                    child.name = $"{children[0].name} {i}";
                    children.Add(child);
                }
                else Debug.LogWarning("No child found to duplicate.", this);
            }
        }
    }

    #endregion
}
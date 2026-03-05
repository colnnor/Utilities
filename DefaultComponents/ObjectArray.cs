using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

[ExecuteAlways]
public class ObjectArray : MonoBehaviour
{
    public enum ArrayType { Grid, Radial }
    public enum Plane { XY, XZ }

    [Header("Array Settings")]
    [SerializeField] private ArrayType arrayType = ArrayType.Grid;
    [SerializeField] private Plane plane = Plane.XZ;
    [SerializeField] private float spacing = 2f;

    [Header("Grid Settings")]
    [SerializeField, Min(1)] private int gridSizeX = 5;
    [SerializeField, Min(1)] private int gridSizeY = 5;
    [SerializeField] private Vector2 gridSpacing = Vector2.one * 2f;
    
    //[SerializeField, Min(1)] private int gridSizeZ = 5; // For future extension

    [Header("Radial Settings")]
    [SerializeField, Min(1)] private int radialCount = 10;
    [SerializeField, Min(0.1f)] private float radius = 5f;

    [Header("Rotation Settings")]
    [SerializeField] private bool faceCenter;
    [SerializeField] private Vector3 individualRotationOffset;
    [SerializeField] private Vector3 constantRotationOffset;

    private List<Transform> children = new List<Transform>();

    private void OnValidate()
    {
        gridSizeX = Mathf.Max(1, gridSizeX);
        gridSizeY = Mathf.Max(1, gridSizeY);
        //gridSizeZ = Mathf.Max(1, gridSizeZ);
        radialCount = Mathf.Max(1, radialCount);
        radius = Mathf.Max(0.1f, radius);
        UpdateArray();
    }

    private void Start() => UpdateArray();

    private void UpdateArray()
    {
        children = transform.GetChildren();
        VerifyChildCount();
        switch (arrayType)
        {
            case ArrayType.Grid:
                UpdateGridArray();
                break;
            case ArrayType.Radial:
                UpdateRadialArray();
                break;
        }
    }

    private void UpdateGridArray()
    {
        int count = gridSizeX * gridSizeY;
        for (int i = 0; i < children.Count; i++)
        {
            int x = i % gridSizeX;
            int y = i / gridSizeX;
            Vector3 pos = plane == Plane.XY 
                ? new Vector3(x * gridSpacing.x, y * gridSpacing.y, 0) 
                : new Vector3(x * gridSpacing.x, 0, y * gridSpacing.y);
            children[i].localPosition = pos;
            ApplyRotation(i);
        }
    }

    private void UpdateRadialArray()
    {
        float angleStep = 360f / radialCount;
        for (int i = 0; i < children.Count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 pos = plane == Plane.XY ? new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0) : new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            children[i].localPosition = pos;
            ApplyRotation(i);
        }
    }

    private void ApplyRotation(int i)
    {
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

#if ODIN_INSPECTOR
    [Button]
#endif
    void RepopulateChildren()
    {
        int tempCount = GetTargetCount();
        SetTargetCount(1);
        VerifyChildCount();
        SetTargetCount(tempCount);
        UpdateArray();
    }

    private void VerifyChildCount()
    {
        int targetCount = GetTargetCount();
        if (children.Count > targetCount)
        {
            for (int i = children.Count - 1; i >= targetCount; i--)
            {
                children[i].Destroy();
                children.RemoveAt(i);
            }
            Debug.Log($"Removed {children.Count - targetCount} extra children.", this);
        }
        else if (children.Count < targetCount)
        {
            for (int i = children.Count; i < targetCount; i++)
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

    private int GetTargetCount()
    {
        return arrayType == ArrayType.Grid ? gridSizeX * gridSizeY : radialCount;
    }

    private void SetTargetCount(int value)
    {
        if (arrayType == ArrayType.Grid)
        {
            gridSizeX = Mathf.Max(1, value);
            gridSizeY = 1;
        }
        else
        {
            radialCount = Mathf.Max(1, value);
        }
    }
}
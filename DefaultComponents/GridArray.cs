using System.Collections.Generic;
// Sirenix.OdinInspector;
using UnityEngine;

public enum GridOrientation
{
    XZ,
    XY
}

public class GridArray : MonoBehaviour
{
    [SerializeField] private GridOrientation gridOrientation = GridOrientation.XZ;
    [SerializeField] private int columns = 1;
    [SerializeField] private bool autoSetSpacing = false;
    //[HideIf("autoSetSpacing")]
    [SerializeField] private float horizontalSpacing = 0.1f;
    //[HideIf("autoSetSpacing")]
    [SerializeField] private float verticalSpacing = 0.1f;

    private int rows;
    private List<Transform> children = new List<Transform>();

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            children.Add(child);
        }
    }

    private void Start()
    {
        ArrangeChildren();
    }

    private void OnValidate()
    {
        if (columns < 1) columns = 1;
        ArrangeChildren();
    }

    private void ArrangeChildren()
    {
        children = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child != transform)
            {
                children.Add(child);
            }
        }

        if (children.Count == 0) return;

        rows = Mathf.CeilToInt((float)children.Count / columns);

        if (autoSetSpacing)
        {
            horizontalSpacing = 1 / (float)children.Count;
            verticalSpacing = 1 / (float)children.Count;
        }

        Vector3 centerOfGrid = Vector3.zero;
        centerOfGrid.x = -(columns - 1) * (children[0].localScale.x + horizontalSpacing) / 2;
        centerOfGrid.y = 0;
        centerOfGrid.z = -(rows - 1) * (children[0].localScale.y + verticalSpacing) / 2;

        Vector3 offset = Vector3.zero;

        for (int i = 0; i < children.Count; i++)
        {
            int column = i % columns;
            int row = i / columns;

            float x = column * (children[i].localScale.x + horizontalSpacing);
            float y = row * (children[i].localScale.y + verticalSpacing);

            offset = gridOrientation switch
            {
                GridOrientation.XZ => new Vector3(x, 0, y),
                GridOrientation.XY => new Vector3(x, y, 0),
                _ => offset
            };

            Vector3 position = transform.position + centerOfGrid + offset;
            children[i].position = position;
        }
    }
}
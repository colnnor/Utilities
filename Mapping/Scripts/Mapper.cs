using UnityEngine;

/// <summary>
/// There are two components to the mapping system. A
/// </summary>
public class Mapper : MonoBehaviour
{
    [SerializeField] private BoxCollider mappingBounds;
    [SerializeField] private TrackingArea trackingArea;

    public Vector3 GetPosition(Vector3 input)
    {
        Bounds mapping = mappingBounds.bounds;

        var input01 = trackingArea.GetInputPosition01(input);
        
        float trackedX = input01.x;
        float trackedY = input01.y;
        float trackedZ = input01.z;
        
        float mappedX = Mathf.Lerp(mapping.min.x, mapping.max.x, trackedX);
        float mappedY = Mathf.Lerp(mapping.min.y, mapping.max.y, trackedY);
        float mappedZ = Mathf.Lerp(mapping.min.z, mapping.max.z, trackedZ);
        
        return new Vector3(mappedX, mappedY, mappedZ);
    }

    private void OnDrawGizmos()
    {
        if(!trackingArea || !mappingBounds) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(trackingArea.Bounds.center, trackingArea.Bounds.size);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(mappingBounds.bounds.center, mappingBounds.bounds.size);
    }
}
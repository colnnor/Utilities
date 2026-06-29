using UnityEngine;

public class TrackingArea : MonoBehaviour
{
    [SerializeField] private BoxCollider trackingArea;
    
    public Bounds Bounds => trackingArea.bounds;

    
    public Vector3 GetInputPosition01(Vector3 input)
    {
        Bounds tracking = trackingArea.bounds;

        var x = input.x;
        var y = input.y;
        var z = input.z;
        
        float trackedX = Mathf.InverseLerp(tracking.min.x, tracking.max.x, x);
        float trackedY = Mathf.InverseLerp(tracking.min.y, tracking.max.y, y);
        float trackedZ = Mathf.InverseLerp(tracking.min.z, tracking.max.z, z);
        
        return new Vector3(trackedX, trackedY, trackedZ);
    }
}
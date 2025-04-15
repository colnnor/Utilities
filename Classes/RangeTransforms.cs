using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class RangeTransforms
{
    public Transform minTransform;
    public Transform maxTransform;
    
    public float RandomX() => Random.Range(minTransform.position.x, maxTransform.position.x);
    public float RandomY() => Random.Range(minTransform.position.y, maxTransform.position.y);
    public float RandomZ() => Random.Range(minTransform.position.z, maxTransform.position.z);
    public Vector3 RandomPosition() => new(RandomX(), RandomY(), RandomZ());

    public void DrawGizmos(Color color)
    {
        if(!minTransform || !maxTransform) return;
        Gizmos.color = color;
        Vector3 center = Vector3.Lerp(minTransform.position, maxTransform.position, 0.5f);
        Vector3 size = maxTransform.position - minTransform.position;
        
        Gizmos.DrawCube(center, size);
    }
}
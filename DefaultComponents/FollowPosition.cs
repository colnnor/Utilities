using Sirenix.OdinInspector;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    [SerializeField] private bool isFollowing = true;
    
    [Button]
    void SetOffsetFromCurrentPosition()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {
        if (!isFollowing || !target) return;

        transform.position = target.position.WithOffset(offset);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetFollowing(bool isFollowing)
    {
        this.isFollowing = isFollowing;
    }
}
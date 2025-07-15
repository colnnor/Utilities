using Sirenix.OdinInspector;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    [OnValueChanged("SetPosition")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    [SerializeField] private bool useLocal = false;
    [SerializeField] private bool isFollowing = true;
    
    [Button]
    void SetOffsetFromCurrentPosition()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {

        SetPosition();
    }

    private void SetPosition()
    {
        if (!isFollowing || !target) return;
        if (useLocal)
            transform.localPosition = target.position.WithOffset(offset);
        else
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
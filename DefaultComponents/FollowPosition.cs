#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

using UnityEngine;

public class FollowPosition : MonoBehaviour
{
#if ODIN_INSPECTOR
    [OnValueChanged("SetPosition")]
#endif
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    [SerializeField] private bool isFollowing = true;
#if ODIN_INSPECTOR
    [Button]
#endif
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
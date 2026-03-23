#if ODIN_INSPECTOR

using Sirenix.OdinInspector;
#endif

using UnityEngine;

public class FollowRotation : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    [SerializeField] private bool isFollowing = true;

    void Update()
    {
        if (!isFollowing || !target) return;

        transform.rotation = target.rotation * Quaternion.Euler(offset);
    }
#if ODIN_INSPECTOR

    
    [Button]
#endif
    public void SetOffsetFromCurrentRotation()
    {
        offset = transform.rotation.eulerAngles - target.rotation.eulerAngles;
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
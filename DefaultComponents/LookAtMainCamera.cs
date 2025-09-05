using System;
using UnityEngine;
using UnityEngine.Assertions;
using AxisType = Axis.AxisType;
public class LookAtMainCamera : MonoBehaviour
{
    private Transform target;
    private float startingYRotation;
    [SerializeField] private Vector3 rotationOffset;
    private void Start()
    {
        Camera mainCam = Camera.main;
        Assert.IsNotNull(mainCam, "Main Camera not found");
        SetTarget(mainCam.transform);
        startingYRotation = transform.localEulerAngles.y;
    }

    private void Update()
    {
        Vector3 targetPosition = target.position;
        targetPosition.x = transform.position.x;
        transform.LookAt(targetPosition);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, startingYRotation, transform.localEulerAngles.z) + rotationOffset;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
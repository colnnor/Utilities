using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DistanceTracker : MonoBehaviour
{
    [SerializeField] private Transform targetOne;
    [SerializeField] private Transform targetTwo;
    [SerializeField] private TextMeshPro distanceText;
    [SerializeField] private LineRenderer lineRenderer;

    private void Awake()
    {
        if(!lineRenderer)
            lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        
        if(!distanceText)
        {
            distanceText = GetComponentInChildren<TextMeshPro>();
        }
    }

    private void Update()
    {
        if(!targetOne || !targetTwo || !distanceText || !lineRenderer)
            return;
        
        float distance = Vector3.Distance(targetOne.position, targetTwo.position);
        distanceText.text = distance.ToString("F2") + "m";
        lineRenderer.SetPosition(0, targetOne.position);
        lineRenderer.SetPosition(1, targetTwo.position);
        
        Vector3 centerPoint = (targetOne.position + targetTwo.position) / 2;
        distanceText.transform.position = centerPoint;
    }
}

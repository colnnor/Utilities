﻿using UnityEngine;

public class MyGizmos
{
    public static void DrawArrow(Vector3 position, Vector3 direction, float bodyLength = 5f, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        var endPoint = position + direction * bodyLength;
        Gizmos.DrawLine(position, endPoint);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(endPoint, right * arrowHeadLength);
        Gizmos.DrawRay(endPoint, left * arrowHeadLength);
    }
    public static void DrawCircle(Vector3 position, float radius, Vector3 normal, int resolution = 36)
    {
        var axis = normal.normalized;
        if (axis.sqrMagnitude < 0.0001f)
            axis = Vector3.up;

        var tangent1 = Vector3.Cross(axis, Vector3.up);
        if (tangent1.sqrMagnitude < 0.0001f)
            tangent1 = Vector3.Cross(axis, Vector3.right);
        tangent1.Normalize();

        var tangent2 = Vector3.Cross(axis, tangent1).normalized;
        var points = new Vector3[resolution];

        for (int i = 0; i < resolution; i++)
        {
            float angle = i * Mathf.PI * 2f / resolution;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            points[i] = position + tangent1 * x + tangent2 * y;
        }

        Gizmos.DrawLineStrip(points, true);
    }
}
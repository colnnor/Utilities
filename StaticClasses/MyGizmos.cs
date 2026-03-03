using UnityEngine;

public class MyGizmos
{
    public static void DrawArrow(Vector3 position, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.DrawRay(position, direction);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(position + direction, right * arrowHeadLength);
        Gizmos.DrawRay(position + direction, left * arrowHeadLength);
    }
    public static void DrawCircle(Vector3 position, float radius, int resolution = 36)
    {
        var points = new Vector3[resolution];
        for (int i = 0; i < resolution; i++)
        {
            float angle = i * Mathf.PI * 2f / resolution;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            points[i] = position + new Vector3(x, 0, z);
        }
        Gizmos.DrawLineStrip(points, true);
    }
}
using UnityEngine;

public static class Axis
{
    public enum AxisType
    {
        None,
        X,
        Y,
        Z
    }
    
    public static Vector3 GetAxis(AxisType type)
    {
        return type switch
        {
            AxisType.None => Vector3.zero,
            AxisType.X => Vector3.right,
            AxisType.Y => Vector3.up,
            AxisType.Z => Vector3.forward
        };
    }
    
}

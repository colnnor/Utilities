using UnityEngine;


[ExecuteInEditMode]
[SelectionBase]
public class PositionConstraints : MonoBehaviour
{
    public bool lockX;
    public bool lockY;
    public bool lockZ;
    private void Update()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        var x = lockX ? 0 : transform.position.x;
        var y = lockY ? 0 : transform.position.y;
        var z = lockZ ? 0 : transform.position.z;
        transform.position = new Vector3(x, y, z);
    }
}
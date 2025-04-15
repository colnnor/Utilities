using UnityEngine;

public readonly struct Direction
{
    public static readonly Direction Up = new(Vector3.up);
    public static readonly Direction Down = new(Vector3.down);
    public static readonly Direction Left = new(Vector3.left);
    public static readonly Direction Right = new(Vector3.right);
    public static readonly Direction Forward = new(Vector3.forward);
    public static readonly Direction Bac = new(Vector3.back);

    public readonly Vector3 vector;
    public Direction(Vector3 vector)
    {
        this.vector = vector;
    }
    
    public static Direction operator +(Direction a, Direction b) => new(a.vector + b.vector);
    public static Direction operator -(Direction a, Direction b) => new(a.vector - b.vector);
    public static Direction operator *(Direction a, float b) => new(a.vector * b);
    public static Direction operator /(Direction a, float b) => new(a.vector / b);
    
    public static implicit operator Vector3(Direction direction) => direction.vector;
    
    public Vector2Int ToVector2Int()
    {
        return new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
    }
}
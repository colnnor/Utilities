using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DirectionClass
{
    public static readonly DirectionClass Up = new(Vector3Int.up);
    public static readonly DirectionClass Down = new(Vector3Int.down);
    public static readonly DirectionClass Left = new(Vector3Int.left);
    public static readonly DirectionClass Right = new(Vector3Int.right);
    public static readonly DirectionClass Forward = new(Vector3Int.forward);
    public static readonly DirectionClass Back = new(Vector3Int.back);

    /// <summary>
    /// Up, Down, Left, Right, Forward, Back
    /// </summary>
    public static List<DirectionClass> directions => new List<DirectionClass>
    {
        Up,
        Down,
        Left,
        Right,
        Forward,
        Back
    };
    public static DirectionClass Opposite(DirectionClass direction)
    {
        if (direction == Up) return Down;
        if (direction == Down) return Up;
        if (direction == Left) return Right;
        if (direction == Right) return Left;
        if (direction == Forward) return Back;
        if (direction == Back) return Forward;
        Debug.LogError("Invalid direction");
        return Up;
    }
    /// <summary>
    /// Up, Down, Left, Right, Forward, Back
    /// </summary>
    public DirectionClass this[int index] => directions[index];
    public static DirectionClass IndexOf(int index)
    {
        return directions[index];
    }
    
    public Vector3Int vector;
    public DirectionClass(Vector3Int vector)
    {
        this.vector = vector;
    }
    
    public static DirectionClass operator +(DirectionClass a, DirectionClass b) => new(a.vector + b.vector);
    public static DirectionClass operator -(DirectionClass a, DirectionClass b) => new(a.vector - b.vector);
    public static DirectionClass operator *(DirectionClass a, int b) => new(a.vector * b);
    public static DirectionClass operator /(DirectionClass a, int b) => new(a.vector / b);
    
    public static implicit operator Vector3(DirectionClass direction) => direction.vector;
    
    public Vector2Int ToVector2Int()
    {
        return new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
    }
}
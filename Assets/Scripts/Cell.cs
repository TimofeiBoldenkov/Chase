using UnityEngine;
using System;

public class Cell
{
    public bool Walkable;
    public Vector2Int Position;

    public Cell(bool walkable, Vector2Int position) { Walkable = walkable; Position = position; }
}
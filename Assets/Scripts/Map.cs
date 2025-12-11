using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class Map : MonoBehaviour
{
    private Cell[,] cells;
    public int Rows => cells.GetLength(0);
    public int Columns => cells.GetLength(1);

    private const string MAP_FILE_PATH = "Assets/Data/map.txt";

    void Awake()
    {
        ReadMapFromTXT();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    void ReadMapFromTXT()
    {
        string[] mapRows = File.ReadAllText(MAP_FILE_PATH).Split("\n");
        if (mapRows.Length == 0 || mapRows[0].Length == 0)
        {
            throw new FormatException("Map should not be empty");
        }

        cells = new Cell[mapRows.Length, mapRows[0].Length];

        int rowLength = mapRows[0].Length;
        for (int y = 0; y < mapRows.Length; y++)
        {
            if (mapRows[mapRows.Length - y - 1].Length != rowLength)
            {
                throw new FormatException("Map rows should be of the same length");
            }

            for (int x = 0; x < rowLength; x++)
            {
                cells[y, x] = mapRows[mapRows.Length - y - 1][x] switch
                {
                    'R' => new Cell(true, new Vector2Int(x, y)),
                    'C' => new Cell(false, new Vector2Int(x, y)),
                    _ => throw new FormatException($"Unexpected symbol '{mapRows[mapRows.Length - y - 1][x]}' at ({x},{y})"),
                };
            }
        }
    }

    public Cell[,] GetCells()
    {
        return cells;
    }

    public List<Vector2Int> GetWalkableNeighbourCells(Vector2Int position)
    {
        var NeighbourCells = new List<Vector2Int>();

        for (int y = position.y - 1; y <= position.y + 1; y++)
        {
            for (int x = position.x - 1; x <= position.x + 1; x++)
            {
                if (y >= 0 && y < cells.GetLength(0) && 
                    x >= 0 && x < cells.GetLength(1) && 
                    cells[y, x].Walkable)
                {
                    NeighbourCells.Add(cells[y, x].Position);
                }
            }
        }

        return NeighbourCells;
    }

    public int? DistanceBetweenNeighbourCells(Vector2Int position1, Vector2Int position2)
    {
        if (Math.Abs(position1.x - position2.x) == 1 &&
            Math.Abs(position1.y - position2.y) == 1)
        {
            return 14;
        } 
        else if ((Math.Abs(position1.x - position2.x) == 1 && Math.Abs(position1.y - position2.y) == 0) ||
            (Math.Abs(position1.x - position2.x) == 0 && Math.Abs(position1.y - position2.y) == 1))
        {
            return 10;
        } else
        {
            return null;
        }
    }

    public Cell GetCellFromPos(Vector2Int pos)
    {
        return cells[pos.y, pos.x];
    }
}

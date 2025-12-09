using System;
using UnityEngine;
using System.IO;

public class Map : MonoBehaviour
{
    private Cell[,] cells;

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
        for (int i = 0; i < mapRows.Length; i++)
        {
            if (mapRows[i].Length != rowLength)
            {
                throw new FormatException("Map rows should be of the same length");
            }

            for (int j = 0; j < mapRows[i].Length; j++)
            {
                switch (mapRows[i][j])
                {
                    case 'R':
                        cells[i, j] = new Cell(true);
                        break;
                    case 'C':
                        cells[i, j] = new Cell(false);
                        break;
                    default:
                        throw new FormatException($"Unexpected symbol '{mapRows[i][j]}' at ({i},{j})");
                }
            }
        }
    }

    public Cell[,] GetCells()
    {
        return cells;
    }
}

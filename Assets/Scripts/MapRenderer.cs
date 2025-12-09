using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class MapRenderer : MonoBehaviour
{
    public Map Map;
    public TileBase CragTile;
    public TileBase RoadTile;
    public TileBase CityTile;
    private Tilemap tilemap;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    void Start()
    {
        var cells = Map.GetCells();
        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                if (cells[x, y].Walkable)
                {
                    tilemap.SetTile(new Vector3Int(x, cells.GetLength(1) - 1 - y, 0), RoadTile);
                } 
                else
                {
                    tilemap.SetTile(new Vector3Int(x, cells.GetLength(1) - 1 - y, 0), CragTile);
                }
            }
        }
    }

    void Update()
    {
        
    }
}

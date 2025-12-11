using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class MapRenderer : MonoBehaviour
{
    public Map Map;
    public TileBase MountainTile;
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
        for (int y = 0; y < cells.GetLength(0); y++)
        {
            for (int x = 0; x < cells.GetLength(1); x++)
            {
                if (cells[y, x].Terrain == Cell.TerrainType.Road)
                {
                    tilemap.SetTile(new Vector3Int(x, y), RoadTile);
                } 
                else if (cells[y, x].Terrain == Cell.TerrainType.Mountain)
                {
                    tilemap.SetTile(new Vector3Int(x, y), MountainTile);
                }
            }
        }
    }

    void Update()
    {
        
    }
}

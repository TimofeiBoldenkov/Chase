using UnityEngine;

public static class GridUtils
{
    public static Vector3Int GridPosFromWorldPos(Grid grid, Vector3 worldPos)
    {   
        var cellPos = grid.WorldToCell(worldPos);
        return cellPos;
    }
    public static Vector3Int GridPosFromScreenPos(Grid grid, Vector3 screenPos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane)
        );
        var cellPos = grid.WorldToCell(worldPos);
        return cellPos;
    }
    public static Vector3 CellCenterWorldPosFromCellPos(Grid grid, Vector3Int cellPos)
    {
        var cellWorldPos = grid.CellToWorld(cellPos);
        var offset = grid.cellSize / 2;
        return cellWorldPos + offset;
    }
}
using UnityEngine;

public static class PosUtils
{
    public static Vector3Int WorldPosToGridPos(Grid grid, Vector3 worldPos)
    {   
        var cellPos = grid.WorldToCell(worldPos);
        return cellPos;
    }
    public static Vector3Int ScreenPosToGridPos(Grid grid, Vector3 screenPos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane)
        );
        var cellPos = grid.WorldToCell(worldPos);
        return cellPos;
    }
    public static Vector3 GridPosToCellCenterWorldPos(Grid grid, Vector3Int gridPos)
    {
        var cellWorldPos = grid.CellToWorld(gridPos);
        var offset = grid.cellSize / 2;
        return cellWorldPos + offset;
    }
}
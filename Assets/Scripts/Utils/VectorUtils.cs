using UnityEngine;

public static class VectorUtils
{
    public static Vector2Int Vector3IntToVector2Int(Vector3Int v)
    {
        return new Vector2Int(v.x, v.y);
    }

    public static Vector3Int Vector2IntToVector3Int(Vector2Int v)
    {
        return new Vector3Int(v.x, v.y);
    }
}
using System;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public GameObject GridObject;
    private Grid _grid;
    public GameObject MapObject;
    private Map _map;
    public int MaxMovePoints;
    public int MovePoints { get => _movePoints; }
    private int _movePoints;
    public Vector2Int MapPos => VectorUtils.Vector3IntToVector2Int(PosUtils.WorldPosToGridPos(_grid, transform.position));

    void Awake()
    {
        _grid = GridObject.GetComponent<Grid>();
        _map = MapObject.GetComponent<Map>();
        _movePoints = MaxMovePoints;
    }

    void Start()
    {
        _map.AddHero(MapPos);
    }

    void Update()
    {
        
    }

    public void TakeMovePoints(int movePoints)
    {
        if (_movePoints < movePoints)
        {
            throw new FormatException($"Unable to take {movePoints} movePoints: hero has only {_movePoints}");
        }

        _movePoints -= movePoints;
    }

    public void RestoreMovePoints()
    {
        _movePoints = MaxMovePoints;
    }

    public void MoveTo(Vector2Int mapPos)
    {
        _map.MoveHero(MapPos, mapPos);
        transform.position = PosUtils.GridPosToCellCenterWorldPos(_grid, VectorUtils.Vector2IntToVector3Int(mapPos));
    }

    public void MakeVisible()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void MakeInvisible()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}

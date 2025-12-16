using System;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public GameObject GridObject;
    private Grid _grid;
    public GameObject MapObject;
    private Map _map;
    public GameObject TeamControllerObject;
    private TeamController _teamController;
    public int MaxMovePoints;
    public int MovePoints { get => _movePoints; }
    private int _movePoints;
    public Vector2Int MapPos => VectorUtils.Vector3IntToVector2Int(PosUtils.WorldPosToGridPos(_grid, transform.position));
    public bool Alive { get => _alive; }
    private bool _alive = true;

    void Awake()
    {
        _grid = GridObject.GetComponent<Grid>();
        _map = MapObject.GetComponent<Map>();
        _teamController = TeamControllerObject.GetComponent<TeamController>();
        _movePoints = MaxMovePoints;
    }

    void Start()
    {
        _map.AddHero(this);
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
        var targetHero = _map.GetCellFromPos(mapPos).Hero;
        if (targetHero != null)
        {
            var winner = _teamController.GetBattleWinner(this, targetHero);
            if (winner == this)
            {
                Debug.Log("Kill");
                targetHero.Kill();
                _map.MoveHero(MapPos, mapPos);
                transform.position = PosUtils.GridPosToCellCenterWorldPos(_grid, VectorUtils.Vector2IntToVector3Int(mapPos));
            }
            else
            {
                Kill();
            }
        }
        else
        {
            _map.MoveHero(MapPos, mapPos);
            transform.position = PosUtils.GridPosToCellCenterWorldPos(_grid, VectorUtils.Vector2IntToVector3Int(mapPos));
        }
    }

    public void MakeVisible()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void MakeInvisible()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Kill()
    {
        _alive = false;
        _map.RemoveHero(this);
        MakeInvisible();
    }
}

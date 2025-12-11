using System;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public GameObject map;
    public int MaxMovePoints;
    public int MovePoints { get => _movePoints; }
    private int _movePoints;

    void Awake()
    {
        _movePoints = MaxMovePoints;
    }

    void Start()
    {
        
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
}

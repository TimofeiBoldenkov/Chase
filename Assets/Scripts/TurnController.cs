using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

public class TurnController : MonoBehaviour
{
    public GameObject TeamControllerObject;
    private TeamController _teamController;
    public GameObject HeroesControllerObject;
    public GameObject MovementControllerObject;
    private int _currentTeamIndex;
    public TeamController.Team CurrentTeam { get => _turnOrder[_currentTeamIndex]; }
    private readonly List<TeamController.Team> _turnOrder =
        new() { TeamController.Team.ChasedTeam, TeamController.Team.ChasingTeam };
    private InputAction _finishTurnAction;
    public event Action OnFinishTurn;

    void Awake()
    {
        _teamController = TeamControllerObject.GetComponent<TeamController>();
        _finishTurnAction = InputSystem.actions.FindAction("Finish Turn");
    }

    void OnEnable()
    {
        _finishTurnAction.Enable();
        _finishTurnAction.performed += OnFinishTurnInput;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    void OnDisable()
    {
        _finishTurnAction.Disable();
        _finishTurnAction.performed -= OnFinishTurnInput;
    }

    private void OnFinishTurnInput(InputAction.CallbackContext context)
    {
        foreach (var hero in _teamController.GetTeamMembers(CurrentTeam))
        {
            hero.RestoreMovePoints();
        }
        _currentTeamIndex = (_currentTeamIndex + 1) % _turnOrder.Count;
        OnFinishTurn?.Invoke();
    }

    public List<TeamController.Team> GetEnemyTeams()
    {
        var enemyTeams = new List<TeamController.Team>();

        foreach (TeamController.Team team in Enum.GetValues(typeof(TeamController.Team)))
        {
            if (team != CurrentTeam)
            {
                enemyTeams.Add(team);
            }
        }

        return enemyTeams;
    }
}

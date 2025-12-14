using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class TurnController : MonoBehaviour
{
    public GameObject TeamControllerObject;
    private TeamController _teamController;
    public GameObject HeroesControllerObject;
    private HeroesController _heroesController;
    public GameObject MovementControllerObject;
    private MovementController _movementController;
    private int _currentTeamIndex;
    public TeamController.Team CurrentTeam { get => _turnOrder[_currentTeamIndex]; }
    private readonly List<TeamController.Team> _turnOrder =
        new() { TeamController.Team.ChasedTeam, TeamController.Team.ChasingTeam };
    private InputAction _finishTurnAction;

    void Awake()
    {
        _teamController = TeamControllerObject.GetComponent<TeamController>();
        _heroesController = HeroesControllerObject.GetComponent<HeroesController>();
        _movementController = MovementControllerObject.GetComponent<MovementController>();
        _finishTurnAction = InputSystem.actions.FindAction("Finish Turn");
    }

    void OnEnable()
    {
        _finishTurnAction.Enable();
        _finishTurnAction.performed += OnFinishTurn;
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
        _finishTurnAction.performed -= OnFinishTurn;
    }

    private void OnFinishTurn(InputAction.CallbackContext context)
    {
        foreach (var hero in _teamController.GetTeamMembers(CurrentTeam))
        {
            hero.RestoreMovePoints();
        }
        _heroesController.ResetSelection();
        _movementController.ErasePath();
        _currentTeamIndex = (_currentTeamIndex + 1) % _turnOrder.Count;
    }
}

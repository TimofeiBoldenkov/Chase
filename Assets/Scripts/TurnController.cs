using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class TurnController : MonoBehaviour
{
    public GameObject TeamControllerObject;
    private TeamController teamController;
    private int currentTeamIndex;
    public TeamController.Team CurrentTeam { get => turnOrder[currentTeamIndex]; }
    private readonly List<TeamController.Team> turnOrder =
        new() { TeamController.Team.ChasedTeam, TeamController.Team.ChasingTeam };
    private InputAction finishTurnAction;

    void Awake()
    {
        teamController = TeamControllerObject.GetComponent<TeamController>();
        finishTurnAction = InputSystem.actions.FindAction("Finish Turn");
    }

    void OnEnable()
    {
        finishTurnAction.Enable();
        finishTurnAction.performed += OnFinishTurn;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    void OnDisable()
    {
        finishTurnAction.Disable();
        finishTurnAction.performed -= OnFinishTurn;
    }

    private void OnFinishTurn(InputAction.CallbackContext context)
    {
        foreach (var hero in teamController.GetTeamMembers(CurrentTeam))
        {
            hero.RestoreMovePoints();
        }
        currentTeamIndex = (currentTeamIndex + 1) % turnOrder.Count;
        Debug.Log($"Current team index: {currentTeamIndex}");
    }
}

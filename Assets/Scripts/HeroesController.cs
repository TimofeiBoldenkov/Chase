using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class HeroesController : MonoBehaviour
{
    public GameObject MapObject;
    public GameObject GridObject;
    public GameObject TeamControllerObject;
    private TeamController _teamController;
    public GameObject TurnControllerObject;
    private TurnController _turnController; public Hero SelectedHero { get => _selectedHero; }
    private Hero _selectedHero;
    private InputAction _selectHeroAction;
    public event Action OnSelect;

    void Awake()
    {
        _teamController = TeamControllerObject.GetComponent<TeamController>();
        _turnController = TurnControllerObject.GetComponent<TurnController>();
        _selectHeroAction = InputSystem.actions.FindAction("Select Hero");
    }

    void OnEnable()
    {
        _selectHeroAction.Enable();
        _selectHeroAction.performed += OnSelectInput;
        _turnController.OnFinishTurn += OnFinishTurn;
    }

    void Start()
    {

    }

    void OnDisable()
    {
        _selectHeroAction.performed -= OnSelectInput;
        _selectHeroAction.Disable();
        _turnController.OnFinishTurn -= OnFinishTurn;
    }

    private void OnSelectInput(InputAction.CallbackContext context)
    {
        var currentTeamHeroes = _teamController.GetTeamMembers(_turnController.CurrentTeam);

        var keyControl = context.control as KeyControl;
        if (keyControl.keyCode == Key.Digit1)
        {
            _selectedHero = (currentTeamHeroes.Count >= 1 && currentTeamHeroes[0].Alive) ?
                currentTeamHeroes[0] : null;
        }
        else if (keyControl.keyCode == Key.Digit2)
        {
            _selectedHero = (currentTeamHeroes.Count >= 2 && currentTeamHeroes[1].Alive) ?
                currentTeamHeroes[1] : null;
        }
        else if (keyControl.keyCode == Key.Digit3)
        {
            _selectedHero = (currentTeamHeroes.Count >= 3 && currentTeamHeroes[2].Alive) ?
                currentTeamHeroes[2] : null;
        }
        else if (keyControl.keyCode == Key.Digit4)
        {
            _selectedHero = (currentTeamHeroes.Count >= 4 && currentTeamHeroes[3].Alive) ?
                currentTeamHeroes[3] : null;
        }

        OnSelect?.Invoke();
    }

    public void OnFinishTurn()
    {
        _selectedHero = null;
    }
}

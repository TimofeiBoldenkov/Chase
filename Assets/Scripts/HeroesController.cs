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
    private TurnController _turnController;
    public Hero SelectedHero { get => _selectedHero; }
    private Hero _selectedHero;
    private InputAction _selectHeroAction;

    void Awake()
    {
        _selectHeroAction = InputSystem.actions.FindAction("Select Hero");
        _teamController = TeamControllerObject.GetComponent<TeamController>();
        _turnController = TurnControllerObject.GetComponent<TurnController>();
    }

    void OnEnable()
    {
        _selectHeroAction.Enable();
        _selectHeroAction.performed += OnSelect;
    }

    void Start()
    {
        
    }

    void OnDisable()
    {
        _selectHeroAction.performed -= OnSelect;
        _selectHeroAction.Disable();
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        var currentTeamHeroes = _teamController.GetTeamMembers(_turnController.CurrentTeam);

        var keyControl = context.control as KeyControl;
        if (keyControl.keyCode == Key.Digit1)
        {
            _selectedHero = currentTeamHeroes.Count >= 1 ? currentTeamHeroes[0] : null;
        }
        else if (keyControl.keyCode == Key.Digit2)
        {
            _selectedHero = currentTeamHeroes.Count >= 2 ? currentTeamHeroes[1] : null;
        }
        else if (keyControl.keyCode == Key.Digit3)
        {
            _selectedHero = currentTeamHeroes.Count >= 3 ? currentTeamHeroes[2] : null;
        }
        else if (keyControl.keyCode == Key.Digit4)
        {
            _selectedHero = currentTeamHeroes.Count >= 4 ? currentTeamHeroes[3] : null;
        }
    }
}

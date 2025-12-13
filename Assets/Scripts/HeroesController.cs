using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class HeroesController : MonoBehaviour
{
    public GameObject MapObject;
    private Map map;
    public GameObject GridObject;
    private Grid grid;
    public GameObject TeamControllerObject;
    private TeamController teamController;
    public GameObject TurnControllerObject;
    private TurnController turnController;
    public Hero SelectedHero { get => _selectedHero; }
    private Hero _selectedHero;
    private InputAction selectHeroAction;

    void Awake()
    {
        selectHeroAction = InputSystem.actions.FindAction("Select Hero");
        map = MapObject.GetComponent<Map>();
        grid = GridObject.GetComponent<Grid>();
        teamController = TeamControllerObject.GetComponent<TeamController>();
        turnController = TurnControllerObject.GetComponent<TurnController>();
    }

    void OnEnable()
    {
        selectHeroAction.Enable();
        selectHeroAction.performed += OnSelect;
    }

    void Start()
    {
        
    }

    void OnDisable()
    {
        selectHeroAction.performed -= OnSelect;
        selectHeroAction.Disable();
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        var currentTeamHeroes = teamController.GetTeamMembers(turnController.CurrentTeam);

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

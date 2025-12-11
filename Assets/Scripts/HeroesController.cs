using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class HeroesController : MonoBehaviour
{
    public GameObject ChasedHeroObject;
    public GameObject ChasingHero1Object;
    public GameObject ChasingHero2Object;
    public GameObject ChasingHero3Object;
    public GameObject MovementControllerObject;
    private MovementController movementController;
    public GameObject MapObject;
    private Map map;
    public GameObject GridObject;
    private Grid grid;
    public Hero SelectedHero { get => _selectedHero.GetComponent<Hero>(); }
    private GameObject _selectedHero;
    private InputAction selectHeroAction;

    void Awake()
    {
        selectHeroAction = InputSystem.actions.FindAction("Select Hero");
        movementController = MovementControllerObject.GetComponent<MovementController>();
        map = MapObject.GetComponent<Map>();
        grid = GridObject.GetComponent<Grid>();
    }

    void OnEnable()
    {
        selectHeroAction.Enable();
        selectHeroAction.performed += OnSelect;
    }

    void Start()
    {
        map.AddHero(VectorUtils.Vector3IntToVector2Int(
            GridUtils.GridPosFromWorldPos(grid, ChasedHeroObject.transform.position)));
        map.AddHero(VectorUtils.Vector3IntToVector2Int(
            GridUtils.GridPosFromWorldPos(grid, ChasingHero1Object.transform.position)));
        map.AddHero(VectorUtils.Vector3IntToVector2Int(
            GridUtils.GridPosFromWorldPos(grid, ChasingHero2Object.transform.position)));
        map.AddHero(VectorUtils.Vector3IntToVector2Int(
            GridUtils.GridPosFromWorldPos(grid, ChasingHero3Object.transform.position)));
    }

    void OnDisable()
    {
        selectHeroAction.performed -= OnSelect;
        selectHeroAction.Disable();
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        var keyControl = context.control as KeyControl;
        if (keyControl.keyCode == Key.Digit1)
        {
            _selectedHero = ChasedHeroObject;
        }
        else if (keyControl.keyCode == Key.Digit2)
        {
            _selectedHero = ChasingHero1Object;
        }
        else if (keyControl.keyCode == Key.Digit3)
        {
            _selectedHero = ChasingHero2Object;
        }
        else if (keyControl.keyCode == Key.Digit4)
        {
            _selectedHero = ChasingHero3Object;
        }
    }

}

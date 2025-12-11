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
    private InputAction selectHeroAction;

    void Awake()
    {
        selectHeroAction = InputSystem.actions.FindAction("Select Hero");
        movementController = MovementControllerObject.GetComponent<MovementController>();
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
        var keyControl = context.control as KeyControl;
        if (keyControl.keyCode == Key.Digit1)
        {
            movementController.SetHero(ChasedHeroObject);
        }
        else if (keyControl.keyCode == Key.Digit2)
        {
            movementController.SetHero(ChasingHero1Object);
        }
        else if (keyControl.keyCode == Key.Digit3)
        {
            movementController.SetHero(ChasingHero2Object);
        }
        else if (keyControl.keyCode == Key.Digit4)
        {
            movementController.SetHero(ChasingHero3Object);
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class MovementControllerScript : MonoBehaviour
{
    public GameObject gridObject;
    private Grid grid;
    public GameObject heroObject;
    private Hero hero;
    public GameObject moveTargetPrefab;
    private GameObject moveTargetObject;
    private InputAction moveAction;

    void Awake()
    {
        hero = heroObject.GetComponent<Hero>();
        grid = gridObject.GetComponent<Grid>();
        moveAction = InputSystem.actions.FindAction("Move");
        moveTargetObject = Instantiate(moveTargetPrefab);
        moveTargetObject.SetActive(false);
    }

    void OnEnable()
    {
        moveAction.Enable();
        moveAction.performed += OnMove;
    }

    void Start()
    {

    }

    void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.Disable();
    }

    void Update()
    {

    }

    private void OnMove(InputAction.CallbackContext context)
    {
        var cellPos = GridUtils.CellPosFromScreenPos(grid, Mouse.current.position.ReadValue());
        var cellCenterWorldPos = GridUtils.CellCenterWorldPosFromCellPos(grid, cellPos);
        if (!moveTargetObject.activeSelf)
        {
            moveTargetObject.SetActive(true);
            moveTargetObject.transform.position = cellCenterWorldPos;
        }
        else if (cellPos != GridUtils.CellPosFromWorldPos(grid, moveTargetObject.transform.position))
        {
            moveTargetObject.transform.position = cellCenterWorldPos;
        }
        else
        {
            moveTargetObject.SetActive(false);
            hero.transform.position = cellCenterWorldPos;
        }
    }
}

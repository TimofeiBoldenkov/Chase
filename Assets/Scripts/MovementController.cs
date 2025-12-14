using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MovementController : MonoBehaviour
{
    public GameObject GridObject;
    private Grid grid;
    public GameObject MapObject;
    private Map map;
    public GameObject MoveTargetPrefab;
    private GameObject moveTargetObject;
    public GameObject PathPointPrefab;
    public GameObject NextTurnsPathPointPrefab;
    public GameObject HeroesControllerObject;
    private HeroesController heroesController;
    private List<GameObject> pathPointObjects;
    private InputAction moveAction;
    private int moveCost;

    void Awake()
    {
        grid = GridObject.GetComponent<Grid>();
        map = MapObject.GetComponent<Map>();
        heroesController = HeroesControllerObject.GetComponent<HeroesController>();
        moveAction = InputSystem.actions.FindAction("Move");
        moveTargetObject = Instantiate(MoveTargetPrefab);
        moveTargetObject.SetActive(false);
        pathPointObjects = new List<GameObject>();
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

    private void ErasePath()
    {
        while (!(pathPointObjects.Count == 0))
        {
            pathPointObjects[^1].SetActive(false);
            Destroy(pathPointObjects[^1]);
            pathPointObjects.RemoveAt(pathPointObjects.Count - 1);
        }

        moveTargetObject.SetActive(false);
    }

    private void PaintPath(List<Vector2Int> path, int availableSteps)
    {
        if (path == null || path.Count == 0)
        {
            return;
        }

        ErasePath();

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 worldPos = PosUtils.CellPosToCellCenterWorldPos(grid,
                    VectorUtils.Vector2IntToVector3Int(path[i]));

            if (i == availableSteps - 1)
            {
                moveTargetObject.transform.position = worldPos;
                moveTargetObject.SetActive(true);
            }
            else if (i < availableSteps)
            {
                var pathPoint = Instantiate(PathPointPrefab);
                pathPoint.transform.position = worldPos;
                pathPoint.SetActive(true);
                pathPointObjects.Add(pathPoint);
            }
            else
            {
                var pathPoint = Instantiate(NextTurnsPathPointPrefab);
                pathPoint.transform.position = worldPos;
                pathPoint.SetActive(true);
                pathPointObjects.Add(pathPoint);
            }
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        var hero = heroesController.SelectedHero;
        if (hero == null)
        {
            return;
        }

        var mouseGridPos = PosUtils.ScreenPosToGridPos(grid, Mouse.current.position.ReadValue());
        var heroGridPos = PosUtils.WorldPosToGridPos(grid, hero.transform.position);
        var targetGridPos = PosUtils.WorldPosToGridPos(grid, moveTargetObject.transform.position);

        if (!moveTargetObject.activeSelf ||
            mouseGridPos != targetGridPos)
        {
            var (path, availableSteps, cost) = AStar.FindPath(map, VectorUtils.Vector3IntToVector2Int(heroGridPos),
                VectorUtils.Vector3IntToVector2Int(mouseGridPos), hero.MovePoints);
            Debug.Log(cost);
            PaintPath(path, availableSteps);
            moveCost = cost;
        }
        else
        {
            ErasePath();
            hero.transform.position = PosUtils.CellPosToCellCenterWorldPos(grid, targetGridPos);
            map.MoveHero(VectorUtils.Vector3IntToVector2Int(heroGridPos), VectorUtils.Vector3IntToVector2Int(targetGridPos));
            hero.TakeMovePoints(moveCost);
        }
    }
}

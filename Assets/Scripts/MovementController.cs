using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MovementController : MonoBehaviour
{
    public GameObject GridObject;
    private Grid _grid;
    public GameObject MapObject;
    private Map _map;
    public GameObject MoveTargetPrefab;
    private GameObject _moveTargetObject;
    public GameObject PathPointPrefab;
    public GameObject NextTurnsPathPointPrefab;
    public GameObject HeroesControllerObject;
    private HeroesController _heroesController;
    private List<GameObject> _pathPointObjects;
    private InputAction _moveAction;
    private int _moveCost;

    void Awake()
    {
        _grid = GridObject.GetComponent<Grid>();
        _map = MapObject.GetComponent<Map>();
        _heroesController = HeroesControllerObject.GetComponent<HeroesController>();
        _moveAction = InputSystem.actions.FindAction("Move");
        _moveTargetObject = Instantiate(MoveTargetPrefab);
        _moveTargetObject.SetActive(false);
        _pathPointObjects = new List<GameObject>();
    }

    void OnEnable()
    {
        _moveAction.Enable();
        _moveAction.performed += OnMove;
    }

    void Start()
    {

    }

    void OnDisable()
    {
        _moveAction.performed -= OnMove;
        _moveAction.Disable();
    }

    void Update()
    {

    }

    public void ErasePath()
    {
        while (!(_pathPointObjects.Count == 0))
        {
            _pathPointObjects[^1].SetActive(false);
            Destroy(_pathPointObjects[^1]);
            _pathPointObjects.RemoveAt(_pathPointObjects.Count - 1);
        }

        _moveTargetObject.SetActive(false);
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
            Vector3 worldPos = PosUtils.CellPosToCellCenterWorldPos(_grid,
                    VectorUtils.Vector2IntToVector3Int(path[i]));

            if (i == availableSteps - 1)
            {
                _moveTargetObject.transform.position = worldPos;
                _moveTargetObject.SetActive(true);
            }
            else if (i < availableSteps)
            {
                var pathPoint = Instantiate(PathPointPrefab);
                pathPoint.transform.position = worldPos;
                pathPoint.SetActive(true);
                _pathPointObjects.Add(pathPoint);
            }
            else
            {
                var pathPoint = Instantiate(NextTurnsPathPointPrefab);
                pathPoint.transform.position = worldPos;
                pathPoint.SetActive(true);
                _pathPointObjects.Add(pathPoint);
            }
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        var hero = _heroesController.SelectedHero;
        if (hero == null)
        {
            return;
        }

        var mouseGridPos = PosUtils.ScreenPosToGridPos(_grid, Mouse.current.position.ReadValue());
        var heroGridPos = PosUtils.WorldPosToGridPos(_grid, hero.transform.position);
        var targetGridPos = PosUtils.WorldPosToGridPos(_grid, _moveTargetObject.transform.position);

        if (!_moveTargetObject.activeSelf ||
            mouseGridPos != targetGridPos)
        {
            var (path, availableSteps, cost) = AStar.FindPath(_map, VectorUtils.Vector3IntToVector2Int(heroGridPos),
                VectorUtils.Vector3IntToVector2Int(mouseGridPos), hero.MovePoints);
            PaintPath(path, availableSteps);
            _moveCost = cost;
        }
        else
        {
            ErasePath();
            hero.transform.position = PosUtils.CellPosToCellCenterWorldPos(_grid, targetGridPos);
            _map.MoveHero(VectorUtils.Vector3IntToVector2Int(heroGridPos), VectorUtils.Vector3IntToVector2Int(targetGridPos));
            hero.TakeMovePoints(_moveCost);
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MovementController : MonoBehaviour
{
    public GameObject GridObject;
    private Grid grid;
    private Hero hero;
    public GameObject MapObject;
    private Map map;
    public GameObject MoveTargetPrefab;
    private GameObject moveTargetObject;
    public GameObject PathPointPrefab;
    private List<GameObject> pathPointObjects;
    private InputAction moveAction;

    void Awake()
    {
        grid = GridObject.GetComponent<Grid>();
        map = MapObject.GetComponent<Map>();
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

    public void SetHero(GameObject newHeroObject)
    {
        hero = newHeroObject.GetComponent<Hero>();
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

    private void PaintPath(Vector3Int from, Vector3Int to)
    {
        var path = AStar.FindPath(map, VectorUtils.Vector3IntToVector2Int(from),
            VectorUtils.Vector3IntToVector2Int(to));


        if (path == null || path.Count == 0)
        {
            return;
        }

        ErasePath();

        foreach (var step in path)
        {
            Vector3 worldPos = GridUtils.CellCenterWorldPosFromCellPos(grid,
                    VectorUtils.Vector2IntToVector3Int(step));

            if (step == path.Last.Value)
            {
                moveTargetObject.transform.position = worldPos;
                moveTargetObject.SetActive(true);
            }
            else
            {
                var pathPoint = Instantiate(PathPointPrefab);
                pathPoint.transform.position = worldPos;
                pathPoint.SetActive(true);
                pathPointObjects.Add(pathPoint);
            }
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (hero == null)
        {
            return;
        }

        var mouseGridPos = GridUtils.GridPosFromScreenPos(grid, Mouse.current.position.ReadValue());
        var heroGridPos = GridUtils.GridPosFromWorldPos(grid, hero.transform.position);
        var targetGridPos = GridUtils.GridPosFromWorldPos(grid, moveTargetObject.transform.position);

        if (!moveTargetObject.activeSelf ||
            mouseGridPos != targetGridPos)
        {
            PaintPath(heroGridPos, mouseGridPos);
        }
        else
        {
            ErasePath();
            hero.transform.position = GridUtils.CellCenterWorldPosFromCellPos(grid, mouseGridPos);
        }
    }
}

using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InformationController : MonoBehaviour
{
    private struct Information
    {
        public Vector2Int SupposedPosition;
        public float MaxDeviation;
    }

    public GameObject TeamControllerObject;
    private TeamController _teamController;
    public GameObject TurnControllerObject;
    private TurnController _turnController;
    public GameObject MovementControllerObject;
    private MovementController _movementController;
    public GameObject GridObject;
    private Grid _grid;
    public GameObject MapObject;
    private Map _map;
    public GameObject SupposedPositionMarkerPrefab;
    private Dictionary<Hero, GameObject> _markers = new Dictionary<Hero, GameObject>();
    private const float DEVIATION_MULTIPLIER = 0.13f;
    private const float DEVIATION_ABRUPTNESS = 1.3f;
    private const float LINE_OF_SIGHT_DISTANCE = 8f;
    private bool _informationPainted;
    private Dictionary<Hero, Information> _informationDict;
    private InputAction _showInformationAction;

    void Awake()
    {
        _teamController = TeamControllerObject.GetComponent<TeamController>();
        _turnController = TurnControllerObject.GetComponent<TurnController>();
        _map = MapObject.GetComponent<Map>();
        _movementController = MovementControllerObject.GetComponent<MovementController>();
        _grid = GridObject.GetComponent<Grid>();
        _showInformationAction = InputSystem.actions.FindAction("Show Information");
    }

    void Start()
    {
        PaintCurrentTeam();
        PaintHeroesWithinLineOfSight();
    }

    void OnEnable()
    {
        _turnController.OnFinishTurn += OnFinishTurn;
        _movementController.OnMove += OnMoveHero;
        _showInformationAction.performed += OnShowInformation;
    }

    void OnDisable()
    {
        _turnController.OnFinishTurn -= OnFinishTurn;
        _movementController.OnMove -= OnMoveHero;
        _showInformationAction.performed -= OnShowInformation;
    }

    private Dictionary<Hero, Information> CalculateInformationForCurrentTeam()
    {
        var informationDict = new Dictionary<Hero, Information>();
        var playerHeroes = _teamController.GetTeamMembers(_turnController.CurrentTeam);

        foreach (var team in _turnController.GetEnemyTeams())
        {
            foreach (var enemyHero in _teamController.GetTeamMembers(team))
            {
                informationDict[enemyHero] = GetHeroInformation(enemyHero, playerHeroes,
                    _teamController.InformationAccuracy[_turnController.CurrentTeam]);
            }
        }

        return informationDict;
    }

    private Information GetHeroInformation(Hero enemyHero, List<Hero> playerHeroes, float informationAccuracy)
    {
        float minimumDistance = GetMinimumDistanceToEnemyHero(enemyHero, playerHeroes);

        var information = new Information();
        information.MaxDeviation = Mathf.Pow(minimumDistance, DEVIATION_ABRUPTNESS) *
            (1 / informationAccuracy);
        information.SupposedPosition = enemyHero.MapPos + GetRandomVector2Int(information.MaxDeviation);

        return information;
    }

    private float GetMinimumDistanceToEnemyHero(Hero enemyHero, List<Hero> playerHeroes)
    {
        if (playerHeroes.Count == 0) throw new FormatException("playerHeroes list must not be empty");

        float minimumDistance = GetDistanceBetweenHeroes(playerHeroes[0], enemyHero);
        for (int i = 1; i < playerHeroes.Count; i++)
        {
            float distance = GetDistanceBetweenHeroes(playerHeroes[i], enemyHero);
            if (distance < minimumDistance)
            {
                minimumDistance = distance;
            }
        }

        return minimumDistance;
    }

    private static float GetDistanceBetweenHeroes(Hero hero1, Hero hero2)
    {
        var hero1MapPos = hero1.MapPos;
        var hero2MapPos = hero2.MapPos;

        return Mathf.Sqrt(Mathf.Pow(hero1MapPos.x - hero2MapPos.x, 2) + Mathf.Pow(hero1MapPos.y - hero2MapPos.y, 2));
    }

    private static Vector2Int GetRandomVector2Int(float maxMagnitude)
    {
        var angle = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
        var magnitude = UnityEngine.Random.Range(0f, maxMagnitude);
        var randomVector = new Vector2Int();
        randomVector.x = Mathf.RoundToInt(magnitude * Mathf.Cos(angle));
        randomVector.y = Mathf.RoundToInt(magnitude * Mathf.Sin(angle));

        return randomVector;
    }

    private void PaintCurrentTeam()
    {
        var playerHeroes = _teamController.GetTeamMembers(_turnController.CurrentTeam);
        foreach (var hero in playerHeroes)
        {
            hero.MakeVisible();
        }
        foreach (var enemyTeam in _turnController.GetEnemyTeams())
        {
            foreach (var hero in _teamController.GetTeamMembers(enemyTeam))
            {
                hero.MakeInvisible();
            }
        }
    }

    private void EraseInformationMarkers()
    {
        if (_markers == null) return;

        foreach (var kv in _markers)
        {
            var marker = kv.Value;
            marker.SetActive(false);
            Destroy(marker);
        }

        _markers.Clear();
    }

    private void PaintInformation(Dictionary<Hero, Information> informationDict)
    {
        EraseInformationMarkers();
        foreach (var kv in informationDict)
        {
            var information = kv.Value;
            if (information.SupposedPosition.x >= 0 && information.SupposedPosition.x < _map.Columns &&
                information.SupposedPosition.y >= 0 && information.SupposedPosition.y < _map.Rows)
            {
                var marker = Instantiate(SupposedPositionMarkerPrefab);
                marker.transform.position = PosUtils.GridPosToCellCenterWorldPos(_grid,
                    VectorUtils.Vector2IntToVector3Int(information.SupposedPosition));
                marker.transform.localScale = Vector3.one * 2 * information.MaxDeviation;
                marker.SetActive(true);
                _markers[kv.Key] = marker;
            }
        }
        PaintHeroesWithinLineOfSight();
    }

    private void OnFinishTurn()
    {
        EraseInformationMarkers();
        _informationDict = null;
        _informationPainted = false;
        PaintCurrentTeam();
        PaintHeroesWithinLineOfSight();
    }

    private void PaintHeroesWithinLineOfSight()
    {
        var playerHeroes = _teamController.GetTeamMembers(_turnController.CurrentTeam);
        foreach (var team in _turnController.GetEnemyTeams())
        {
            foreach (var enemyHero in _teamController.GetTeamMembers(team))
            {
                if (enemyHero.gameObject.GetComponent<SpriteRenderer>().enabled == false &&
                    GetMinimumDistanceToEnemyHero(enemyHero, playerHeroes) < LINE_OF_SIGHT_DISTANCE)
                {
                    if (_markers.ContainsKey(enemyHero))
                    {
                        _markers[enemyHero].SetActive(false);
                        Destroy(_markers[enemyHero]);
                        _markers.Remove(enemyHero);
                    }
                    enemyHero.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }
    }

    private void OnMoveHero()
    {
        PaintHeroesWithinLineOfSight();
    }

    private void OnShowInformation(InputAction.CallbackContext context)
    {
        if (!_informationPainted)
        {
            _informationDict ??= CalculateInformationForCurrentTeam();
            PaintInformation(_informationDict);
            _informationPainted = true;
        }
        else
        {
            EraseInformationMarkers();
            _informationPainted = false;
        }
    }
}

using UnityEngine;

public class VictoryController : MonoBehaviour
{
    public GameObject MapObject;
    private Map _map;
    public GameObject TeamControllerObject;
    private TeamController _teamController;
    public GameObject FullScreenMessageObject;
    private FullScreenMessage _fullScreenMessage;
    public GameObject MovementControllerObject;
    private MovementController _movementController;

    void Awake()
    {
        _map = MapObject.GetComponent<Map>();
        _teamController = TeamControllerObject.GetComponent<TeamController>();
        _fullScreenMessage = FullScreenMessageObject.GetComponent<FullScreenMessage>();
        _movementController = MovementControllerObject.GetComponent<MovementController>();
    }

    void OnEnable()
    {
        _movementController.OnMove += ShowMessageIfWon;
    }

    void OnDisable()
    {
        _movementController.OnMove -= ShowMessageIfWon;
    }

    private void ShowMessageIfWon()
    {
        var winner = CheckIfWon();
        switch (winner)
        {
            case TeamController.Team.ChasedTeam:
                _fullScreenMessage.Show("Chased team won!", false);
                break;
            case TeamController.Team.ChasingTeam:
                _fullScreenMessage.Show("Chasing team won!", false);
                break;
            default:
                break;
        }
    }

    private TeamController.Team? CheckIfWon()
    {
        var chasedTeamMembers = _teamController.GetTeamMembers(TeamController.Team.ChasedTeam);
        var chasingTeamMembers = _teamController.GetTeamMembers(TeamController.Team.ChasingTeam);

        if (chasingTeamMembers.Count < 2)
        {
            return TeamController.Team.ChasedTeam;
        }

        foreach (var hero in chasedTeamMembers)
        {
            if (_map.GetCellFromPos(hero.MapPos).Terrain == Cell.TerrainType.Destination)
            {
                return TeamController.Team.ChasedTeam;
            }
        }

        foreach (var chasedHero in chasedTeamMembers)
        {
            int surroundedBy = 0;
            foreach (var chasingHero in chasingTeamMembers)
            {
                if (_map.DistanceBetweenNeighbourCells(chasedHero.MapPos, chasingHero.MapPos) != null)
                {
                    surroundedBy++;
                }
            }
            if (surroundedBy >= 2)
            {
                return TeamController.Team.ChasingTeam;
            }
        }

        return null;
    }
}

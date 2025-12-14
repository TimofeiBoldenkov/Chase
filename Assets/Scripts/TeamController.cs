using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    public List<GameObject> ChasedTeamMembers;
    private List<Hero> ChasedTeamMembersComponents { get => GetHeroComponentsFromObjects(ChasedTeamMembers); }
    public List<GameObject> ChasingTeamMembers;
    private List<Hero> ChasingTeamMembersComponents { get => GetHeroComponentsFromObjects(ChasingTeamMembers); }
    public GameObject MapObject;
    private Map _map;
    public GameObject GridObject;
    private Grid _grid;

    public enum Team
    {
        ChasedTeam,
        ChasingTeam,
    }

    void Awake()
    {
        _map = MapObject.GetComponent<Map>();
        _grid = GridObject.GetComponent<Grid>();
    }

    void Start()
    {
        foreach (var hero in ChasedTeamMembers)
        {
            _map.AddHero(VectorUtils.Vector3IntToVector2Int(
                PosUtils.WorldPosToGridPos(_grid, hero.transform.position)));
        }
        foreach (var hero in ChasingTeamMembers)
        {
            _map.AddHero(VectorUtils.Vector3IntToVector2Int(
                PosUtils.WorldPosToGridPos(_grid, hero.transform.position)));
        }
    }

    void Update()
    {

    }

    private static List<Hero> GetHeroComponentsFromObjects(List<GameObject> heroObjects)
    {
        var heroComponents = new List<Hero>();
        foreach (var heroObject in heroObjects)
        {
            heroComponents.Add(heroObject.GetComponent<Hero>());
        }

        return heroComponents;
    }

    public List<Hero> GetTeamMembers(Team team)
    {
        return team switch
        {
            Team.ChasedTeam => ChasedTeamMembersComponents,
            Team.ChasingTeam => ChasingTeamMembersComponents,
            _ => null
        };
    }
}

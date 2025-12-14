using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    public List<GameObject> ChasedTeamMembers;
    private List<Hero> chasedTeamMembersComponents { get => GetHeroComponentsFromObjects(ChasedTeamMembers); }
    public List<GameObject> ChasingTeamMembers;
    private List<Hero> chasingTeamMembersComponents { get => GetHeroComponentsFromObjects(ChasingTeamMembers); }
    public GameObject MapObject;
    private Map map;
    public GameObject GridObject;
    private Grid grid;

    public enum Team
    {
        ChasedTeam,
        ChasingTeam,
    }

    void Awake()
    {
        map = MapObject.GetComponent<Map>();
        grid = GridObject.GetComponent<Grid>();
    }

    void Start()
    {
        foreach (var hero in ChasedTeamMembers)
        {
            map.AddHero(VectorUtils.Vector3IntToVector2Int(
                PosUtils.WorldPosToGridPos(grid, hero.transform.position)));
        }
        foreach (var hero in ChasingTeamMembers)
        {
            map.AddHero(VectorUtils.Vector3IntToVector2Int(
                PosUtils.WorldPosToGridPos(grid, hero.transform.position)));
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
            Team.ChasedTeam => chasedTeamMembersComponents,
            Team.ChasingTeam => chasingTeamMembersComponents,
            _ => null
        };
    }
}

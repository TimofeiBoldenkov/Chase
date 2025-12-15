using System;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    public List<GameObject> ChasedTeamMembers;
    private List<Hero> ChasedTeamMembersComponents { get => GetHeroComponentsFromObjects(ChasedTeamMembers); }
    public List<GameObject> ChasingTeamMembers;
    private List<Hero> ChasingTeamMembersComponents { get => GetHeroComponentsFromObjects(ChasingTeamMembers); }
    public GameObject GridObject;

    public Dictionary<Team, float> InformationAccuracy = new Dictionary<Team, float>
    {
        {Team.ChasedTeam, 10},
        {Team.ChasingTeam, 0.5f}
    };

    public enum Team
    {
        ChasedTeam,
        ChasingTeam,
    }

    void Awake()
    {
        
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

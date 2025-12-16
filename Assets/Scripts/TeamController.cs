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
        {Team.ChasingTeam, 2}
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

    private static List<Hero> GetAliveHeroes(List<Hero> heroes)
    {
        var aliveHeroes = new List<Hero>();
        foreach (var hero in heroes)
        {
            if (hero.Alive)
            {
                aliveHeroes.Add(hero);
            }
        }

        return aliveHeroes;
    }

    public List<Hero> GetTeamMembers(Team team)
    {
        return team switch
        {
            Team.ChasedTeam => GetAliveHeroes(ChasedTeamMembersComponents),
            Team.ChasingTeam => GetAliveHeroes(ChasingTeamMembersComponents),
            _ => null
        };
    }

    public Hero GetBattleWinner(Hero hero1, Hero hero2)
    {
        var hero1Team = GetTeamMembers(Team.ChasedTeam).Contains(hero1) ?
            Team.ChasedTeam : Team.ChasingTeam;
        var hero2Team = GetTeamMembers(Team.ChasedTeam).Contains(hero2) ?
            Team.ChasedTeam : Team.ChasingTeam;

        if (hero1Team == Team.ChasedTeam && hero2Team == Team.ChasingTeam)
        {
            return hero1;
        }
        else if (hero1Team == Team.ChasingTeam && hero2Team == Team.ChasedTeam)
        {
            return hero2;
        }
        else
        {
            return null;
        }
    }
}

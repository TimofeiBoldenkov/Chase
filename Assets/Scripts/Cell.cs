using UnityEngine;
using System;
using System.Collections.Generic;

public class Cell
{
    public enum TerrainType
    {
        Road,
        Mountain,
        Destination,
    }

    [Flags]
    public enum FlagsType
    {
        None = 0,
        Hero = 1 << 0,
        City = 1 << 1,
        // ...
    }

    public TerrainType Terrain { get; set; }
    public FlagsType Flags { get; set; }
    public Vector2Int Position { get; set; }
    public Hero Hero
    {
        get
        {
            if (FlagsAreSet(FlagsType.Hero))
            {
                return _hero;
            }
            else
            {
                return null;
            }
        }
    }
    private Hero _hero;

    public Cell(TerrainType terrain, Vector2Int position) { Terrain = terrain; Position = position; }

    public bool Walkable
    {
        get
        {
            if (Terrain == TerrainType.Road || Terrain == TerrainType.Destination)
            {
                if (FlagsAreSet(FlagsType.Hero))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }

    public void AddFlags(FlagsType flags)
    {
        Flags |= flags;
    }

    public void RemoveFlags(FlagsType flags)
    {
        Flags &= ~flags;
    }

    public void AddHero(Hero hero)
    {
        AddFlags(FlagsType.Hero);
        _hero = hero;
    }

    public void RemoveHero()
    {
        if (Hero != null)
        {
            _hero = null;
            RemoveFlags(FlagsType.Hero);
        }
    }

    public bool FlagsAreSet(FlagsType flags)
    {
        return (Flags & flags) == flags;
    }
}
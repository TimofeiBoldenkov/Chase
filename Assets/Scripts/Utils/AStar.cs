using System;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    private class OpenNode
    {
        public Vector2Int Position;
        public int F;

        public OpenNode(Vector2Int position, int f)
        {
            Position = position;
            F = f;
        }
    }

    static private int H(Vector2Int pos1, Vector2Int pos2)
    {
        return Math.Abs(pos2.x - pos1.x) * 10 + Math.Abs(pos2.y - pos1.y) * 10;
    }

    static public (List<Vector2Int> path, int availableSteps, int cost) 
        FindPath(Map map, Vector2Int from, Vector2Int to, int movePoints)
    {
        if (from.x < 0 || from.x >= map.Columns || from.y < 0 || from.y >= map.Rows ||
            to.x < 0 || to.x >= map.Columns || to.y < 0 || to.y >= map.Rows ||
            to == from)
        {
            return (null, 0, 0);
        }

        var isOpen = new bool[map.Rows, map.Columns];
        var isClosed = new bool[map.Rows, map.Columns];
        var parent = new Vector2Int[map.Rows, map.Columns];
        var g = new int[map.Rows, map.Columns];

        var current = from;
        var openList = new MinHeap<OpenNode>();
        bool destinationReached = false;
        while (true)
        {
            isClosed[current.y, current.x] = true;

            foreach (var neighbour in map.GetWalkableNeighbourCells(current))
            {
                if (!isClosed[neighbour.y, neighbour.x])
                {
                    int? distance = map.DistanceBetweenNeighbourCells(current, neighbour);
                    if (!distance.HasValue)
                    {
                        continue;
                    }
                    int newG = g[current.y, current.x] + (int) distance;
                    if (!isOpen[neighbour.y, neighbour.x] || newG < g[neighbour.y, neighbour.x])
                    {
                        g[neighbour.y, neighbour.x] = newG;
                        parent[neighbour.y, neighbour.x] = current;
                        var openNode = new OpenNode(neighbour, g[neighbour.y, neighbour.x] + H(neighbour, to));
                        openList.Push(openNode, openNode.F);
                        isOpen[neighbour.y, neighbour.x] = true;
                    }
                }

                if (neighbour == to)
                {
                    destinationReached = true;
                    break;
                }
            }
            if (destinationReached)
            {
                break;
            }

            do
            {
                if (openList.Empty)
                {
                    return (null, 0, 0);
                }
                current = openList.Pop().Position;
            } while (isClosed[current.y, current.x]);
        }

        var linkedListPath = new LinkedList<Vector2Int>();
        linkedListPath.AddLast(to);
        var step = to;
        while (true)
        {
            step = parent[step.y, step.x];
            if (step == from)
            {
                break;
            }
            else
            {
                linkedListPath.AddFirst(step);
            }
        }

        var path = new List<Vector2Int>(linkedListPath);

        int availableSteps;
        int cost = 0;
        for (availableSteps = 0; availableSteps < path.Count; availableSteps++)
        {
            var pos = path[availableSteps];
            if (g[pos.y, pos.x] > movePoints)
            {
                break;
            }
            cost = g[pos.y, pos.x];
        }

        return (path, availableSteps, cost);
    }
}
using Assets.GridMap.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    private const int MOVE_STRAIGH_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    public Grid<PathNode> grid {  get; private set; }
    private List<PathNode> openNodes;
    private List<PathNode> closeNodes;

    public PathFinding(int with, int height, int cellSize, Vector2 startPosition)
    {
        grid = new(with, height, cellSize, startPosition, (Grid<PathNode> g, int x, int y) => new(g,x,y));
        grid.TextPoint(grid);
    }
    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);
        openNodes = new List<PathNode>() { startNode };
        closeNodes = new List<PathNode>();
        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                PathNode pathNode = grid.GetGridObject(i, j);
                pathNode.gCost = 9999;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();
            PathfindingDebugStepVisual.Instance.ClearSnapshots();
            PathfindingDebugStepVisual.Instance.TakeSnapshot(grid, startNode, openNodes, closeNodes);
            while(openNodes.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(openNodes);
                if(currentNode == endNode)
                {
                    PathfindingDebugStepVisual.Instance.TakeSnapshot(grid, currentNode, openNodes, closeNodes);
                    PathfindingDebugStepVisual.Instance.TakeSnapshotFinalPath(grid, CalculatePath(endNode));
                    return CalculatePath(endNode);
                }
                openNodes.Remove(currentNode);
                closeNodes.Add(currentNode);
                foreach (PathNode neighbour in GetNeighbourList(currentNode))
                {
                    if (closeNodes.Contains(neighbour)) continue;
                    if (!neighbour.isWalkable)
                    {
                        closeNodes.Add(neighbour);
                        continue;
                    }
                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbour);
                    if(tentativeGCost < neighbour.gCost)
                    {
                        neighbour.gCost = tentativeGCost;
                        neighbour.hCost = CalculateDistanceCost(neighbour, endNode);
                        neighbour.CalculateFCost();
                        neighbour.cameFromNode = currentNode;
                        if(!openNodes.Contains(neighbour))
                        {
                            openNodes.Add(neighbour);
                        }
                    }
                    PathfindingDebugStepVisual.Instance.TakeSnapshot(grid, currentNode, openNodes, closeNodes);
                }
            }
        }
        //out of nodes on the openList
        return null;
    }
    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbours = new(); 
        //left
        if(currentNode.x - 1 >= 0)
        {
            neighbours.Add(grid.GetGridObject(currentNode.x - 1, currentNode.y));
            //down left
            if (currentNode.y - 1 >= 0)
            {
                neighbours.Add(grid.GetGridObject(currentNode.x - 1, currentNode.y - 1));
            }
            //up left
            if (currentNode.y + 1 < grid.height)
            {
                neighbours.Add(grid.GetGridObject(currentNode.x - 1, currentNode.y + 1));
            }
        }
        //right
        if (currentNode.x + 1 < grid.width)
        {
            neighbours.Add(grid.GetGridObject(currentNode.x + 1, currentNode.y));
            //down right
            if (currentNode.y - 1 >= 0)
            {
                neighbours.Add(grid.GetGridObject(currentNode.x + 1, currentNode.y - 1));
            }
            //up right
            if (currentNode.y + 1 < grid.height)
            {
                neighbours.Add(grid.GetGridObject(currentNode.x + 1, currentNode.y + 1));
            }
        }
        //up
        if (currentNode.y + 1 < grid.height) neighbours.Add(grid.GetGridObject(currentNode.x, currentNode.y + 1));
        //down
        if (currentNode.y - 1 >= 0) neighbours.Add(grid.GetGridObject(currentNode.x, currentNode.y - 1));
        return neighbours;
    }
    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }
    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGH_COST * remaining;
    }
    private PathNode GetLowestFCostNode(List<PathNode> nodeList)
    {
        PathNode lowestFCostNode = nodeList[0];
        foreach (PathNode node in nodeList)
        {
            if(node.fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = node;
            }
        }
        return lowestFCostNode;
    }
}

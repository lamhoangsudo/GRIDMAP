using Assets.GridMap.Scripts;
using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingTest : MonoBehaviour
{
    private PathFinding pathFinding;
    [SerializeField]
    private PathfindingVisual pathfindingVisual;
    [SerializeField]
    private PathfindingDebugStepVisual stepVisual;
    private void Start()
    {
        pathFinding = new(10, 10, 5, Vector2.zero);
        pathfindingVisual.SetGrid(pathFinding.grid);
        stepVisual.Setup(pathFinding.grid);
    }
    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            pathFinding.grid.GetXY((Vector2)mouseWorldPosition, out int x, out int y);
            List<PathNode> path = pathFinding.FindPath(0, 0, x, y);
            if(path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Vector3 startPoint = new(pathFinding.grid.GetLocalPosition(path[i].x, path[i].y).x + pathFinding.grid.cellSize * 0.5f, 
                        pathFinding.grid.GetLocalPosition(path[i].x, path[i].y).y + pathFinding.grid.cellSize * 0.5f);
                    Vector3 endPoint = new(pathFinding.grid.GetLocalPosition(path[i + 1].x, path[i +1].y).x + pathFinding.grid.cellSize * 0.5f,
                        pathFinding.grid.GetLocalPosition(path[i + 1].x, path[i + 1].y).y + pathFinding.grid.cellSize * 0.5f);
                    Debug.DrawLine(startPoint, endPoint, Color.red, 100f);
                }
            }
        }
        if(Input.GetMouseButtonUp(1))
        {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            pathFinding.grid.GetXY((Vector2)mouseWorldPosition, out int x, out int y);
            pathFinding.grid.GetGridObject(x, y).SetIsWalkable(!pathFinding.grid.GetGridObject(x, y).isWalkable);
        }
    }
}

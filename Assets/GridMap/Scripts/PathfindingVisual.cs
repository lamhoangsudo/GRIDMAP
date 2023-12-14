/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingVisual : MonoBehaviour {

    private Grid<PathNode> grid;
    private Mesh mesh;
    private bool updateMesh;

    private void Awake() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetGrid(Grid<PathNode> grid) {
        this.grid = grid;
        UpdateVisual();

        grid.OnGridChangeValue += Grid_OnGridChangeValue; ;
    }

    private void Grid_OnGridChangeValue(object sender, Grid<PathNode>.OnGridValueChangeEvent e)
    {
        updateMesh = true;
    }

    private void LateUpdate() {
        if (updateMesh) {
            updateMesh = false;
            UpdateVisual();
        }
    }

    private void UpdateVisual() {
        MeshUtils.CreateEmptyMeshArrays(grid.width * grid.height, out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.width; x++) {
            for (int y = 0; y < grid.height; y++) {
                int index = x * grid.height + y;
                Vector3 quadSize = new Vector3(1, 1) * grid.cellSize;

                PathNode pathNode = grid.GetGridObject(x, y);

                if (pathNode.isWalkable) {
                    quadSize = Vector3.zero;
                }

                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, (Vector3)grid.GetLocalPosition(x, y) + quadSize * .5f, 0f, quadSize, Vector2.zero, Vector2.zero);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

}


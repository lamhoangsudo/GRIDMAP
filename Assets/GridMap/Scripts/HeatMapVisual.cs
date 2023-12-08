using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMapVisual : MonoBehaviour
{
    private Grid grid;
    private Mesh mesh;
    private bool meshUpdate;

    public void SetGrid(Grid grid)
    {
        this.grid = grid;
        UpdateHeatMapVisual();
        grid.OnGridChangeValue += Grid_OnGridChangeValue;    
    }

    public void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Grid_OnGridChangeValue(object sender, Grid.OnGridValueChangeEvent e)
    {
        meshUpdate = true;
    }
    public void LateUpdate()
    {
        if (meshUpdate)
        {
            meshUpdate = false;
            UpdateHeatMapVisual();
        }
    }
    public void UpdateHeatMapVisual()
    {
        MeshUtils.CreateEmptyMeshArrays(grid.width * grid.height, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles);
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                int index = x * grid.height + y;
                Vector3 baseSize = new Vector3(1, 1) * grid.cellSize;
                int gridValue = grid.GetValue(grid.GetLocalPosition(x,y));
                float gridVauleNomalize = (float)gridValue / Grid.HEAT_MAP_MAX_VALUE;
                Vector2 gridVauleUV = new Vector2(gridVauleNomalize, 0f);
                MeshUtils.AddToMeshArrays(vertices, uvs, triangles, index, (Vector3)grid.GetLocalPosition(x, y) + baseSize * 0.5f, 0f, baseSize, gridVauleUV, gridVauleUV);
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
    }
}

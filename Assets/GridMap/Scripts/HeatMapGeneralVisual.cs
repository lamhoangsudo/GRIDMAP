using Assets.GridMap.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMapGeneralVisual : MonoBehaviour
{
    private Grid<HeatMapGridObject> grid;
    private Mesh mesh;
    private bool meshUpdate;


    public void SetGrid(Grid<HeatMapGridObject> grid)
    {
        this.grid = grid;
        UpdateHeatMapVisual();
        grid.OnGridChangeValue += Grid_OnGridChangeValue;
    }

    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Grid_OnGridChangeValue(object sender, Grid<HeatMapGridObject>.OnGridValueChangeEvent e)
    {
        meshUpdate = true;
    }
    void LateUpdate()
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
                HeatMapGridObject gridValue = grid.GetGridObject(x,y);
                float gridVauleNomalize = gridValue.GetValueNormalize();
                Vector2 gridVauleUV = new(gridVauleNomalize, 0f);
                MeshUtils.AddToMeshArrays(vertices, uvs, triangles, index, (Vector3)grid.GetLocalPosition(x, y) + baseSize * 0.5f, 0f, baseSize, gridVauleUV, gridVauleUV);
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
    }
}

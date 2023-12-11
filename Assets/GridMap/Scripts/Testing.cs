using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System.ComponentModel;
using Assets.GridMap.Scripts;

public class Testing : MonoBehaviour
{
    private Grid<int> grid1;
    private Grid<bool> grid2;
    private Grid<HeatMapGridObject> grid3;
    private Grid<StringGridObject> grid4;
    [SerializeField]
    private Transform statPosition1, statPosition2, statPosition3, statPosition4;
    [SerializeField]
    [DefaultValue(0)]
    private int width, height, cellSize, value, fullValueRange, totalRange;
    [SerializeField]
    private HeatMapBoolVisual heatMapBoolVisual;
    [SerializeField]
    private HeatMapVisual heatMapVisual;
    [SerializeField]
    private HeatMapGeneralVisual heatMapGeneralVisual;
    // Start is called before the first frame update
    void Start()
    {
        grid1 = new Grid<int>(width, height, cellSize, (Vector2)statPosition1.localPosition);
        grid2 = new Grid<bool>(width, height, cellSize, (Vector2)statPosition2.localPosition);
        grid3 = new Grid<HeatMapGridObject>(width, height, cellSize, (Vector2)statPosition3.localPosition, (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g,x,y));
        grid4 = new Grid<StringGridObject>(width, height, cellSize, (Vector2)statPosition4.localPosition, (Grid<StringGridObject> s, int x, int y) => new StringGridObject(s,x,y));
        grid1.TextPoint(grid1);
        grid2.TextPoint(grid2);
        grid3.TextPoint(grid3);
        grid4.TextPoint(grid4);
        heatMapVisual.SetGrid(grid1);
        heatMapBoolVisual.SetGrid(grid2);
        heatMapGeneralVisual.SetGrid(grid3);
    }

    // Update is called once per frame
    void Update()
    {
        //releases right mouse button
        if(Input.GetMouseButtonUp(0))
        {
            //grid.AddValue(UtilsClass.GetMouseWorldPosition(), fullValueRange, totalRange, value);
            grid1.SetGridObject(UtilsClass.GetMouseWorldPosition(), grid1.GetGridObject(UtilsClass.GetMouseWorldPosition()) + 5);           
            grid2.SetGridObject(UtilsClass.GetMouseWorldPosition(), true);
            HeatMapGridObject heatMapGridObject = grid3.GetGridObject(UtilsClass.GetMouseWorldPosition());
            heatMapGridObject?.AddValue(5);
        }
        if(Input.GetKeyUp(KeyCode.Z))
        {
            StringGridObject stringGridObject = grid4.GetGridObject(UtilsClass.GetMouseWorldPosition());
            stringGridObject?.AddLetter("Z");
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            StringGridObject stringGridObject = grid4.GetGridObject(UtilsClass.GetMouseWorldPosition());
            stringGridObject?.AddLetter("X");
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            StringGridObject stringGridObject = grid4.GetGridObject(UtilsClass.GetMouseWorldPosition());
            stringGridObject?.AddLetter("C");
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            StringGridObject stringGridObject = grid4.GetGridObject(UtilsClass.GetMouseWorldPosition());
            stringGridObject?.AddLetter("V");
        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            StringGridObject stringGridObject = grid4.GetGridObject(UtilsClass.GetMouseWorldPosition());
            stringGridObject?.AddNumber("1");
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            StringGridObject stringGridObject = grid4.GetGridObject(UtilsClass.GetMouseWorldPosition());
            stringGridObject?.AddNumber("2");
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            StringGridObject stringGridObject = grid4.GetGridObject(UtilsClass.GetMouseWorldPosition());
            stringGridObject?.AddNumber("3");
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            StringGridObject stringGridObject = grid4.GetGridObject(UtilsClass.GetMouseWorldPosition());
            stringGridObject?.AddNumber("4");
        }
        /*
        //hold press right mouse button
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("last value: " + grid.GetGridObject(UtilsClass.GetMouseWorldPosition()));
        }
        //releases left mouse button
        if (Input.GetMouseButtonUp(1))
        {
            grid.SetGridObject(UtilsClass.GetMouseWorldPosition(), grid.GetGridObject(UtilsClass.GetMouseWorldPosition()) - 5);
        }
        //hold press left mouse button
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("last value: " + grid.GetGridObject(UtilsClass.GetMouseWorldPosition()));
        }
        if (Input.GetMouseButton(2))
        {
            grid.SetGridObject(UtilsClass.GetMouseWorldPosition(), 50);
        }
        //grid.SetGridObject(UtilsClass.GetMouseWorldPosition(), 50);
        */
    }
}

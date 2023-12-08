using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System.ComponentModel;

public class Testing : MonoBehaviour
{
    private Grid grid;
    [SerializeField]
    private Transform statPosition;
    [SerializeField]
    [DefaultValue(0)]
    private int width, height, cellSize, value, fullValueRange, totalRange;
    [SerializeField]
    private HeatMapVisual heatMapVisual;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid(width, height, cellSize, new Vector2(statPosition.localPosition.x, statPosition.localPosition.y));
        grid.TextPoint(grid);
        heatMapVisual.SetGrid(grid);
    }

    // Update is called once per frame
    void Update()
    {
        //releases right mouse button
        if(Input.GetMouseButtonUp(0))
        {
            //grid.SetValue(UtilsClass.GetMouseWorldPosition(), grid.GetValue(UtilsClass.GetMouseWorldPosition()) + 5);
            grid.AddValue(UtilsClass.GetMouseWorldPosition(), fullValueRange, totalRange, value);
        }
        //hold press right mouse button
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("last value: " + grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }
        //releases left mouse button
        if (Input.GetMouseButtonUp(1))
        {
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), grid.GetValue(UtilsClass.GetMouseWorldPosition()) - 5);
        }
        //hold press left mouse button
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("last value: " + grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }
        if (Input.GetMouseButton(2))
        {
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), 50);
        }
        //grid.SetValue(UtilsClass.GetMouseWorldPosition(), 50);
    }
}

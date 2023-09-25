using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    private Grid grid;
    [SerializeField]
    private Transform statPosition;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid(10, 10, 5f, new Vector2(statPosition.localPosition.x, statPosition.localPosition.y));
        grid.TextPoint(grid);
    }

    // Update is called once per frame
    void Update()
    {
        //releases right mouse button
        if(Input.GetMouseButtonUp(0))
        {
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), 20);
        }
        //hold press right mouse button
        if(Input.GetMouseButtonDown(0))
        {
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), 30);
        }
        //releases left mouse button
        if (Input.GetMouseButtonUp(1))
        {
            Debug.Log("last value: " + grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }
        //hold press left mouse button
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("last value: " + grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }
        if (Input.GetMouseButton(2))
        {
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), 40);
        }
        grid.SetValue(UtilsClass.GetMouseWorldPosition(), 50);
    }
}

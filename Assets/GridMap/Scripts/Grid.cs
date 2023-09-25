using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Grid
{
    private int width;
    private int height;
    private int[,] gridArray;
    private TextMesh[,] textArray;
    private float cellSize;
    private Vector2 statPosition;

    public Grid(int width, int height, float cellSize, Vector2 statPosition)
    {
        this.width = width;
        this.height = height;
        this.gridArray = new int[width, height];
        textArray = new TextMesh[width, height];
        this.cellSize = cellSize;
        this.statPosition = statPosition;
    }

    public void TextPoint(Grid grid)
    {
        int value = 0;
        //chieu rong
        for (int i = 0; i < grid.width; i++)
        {
            //chieu dai
            for (int j = 0; j < grid.height; j++)
            {
                textArray[i, j] = UtilsClass.CreateWorldText(grid.gridArray[i, j].ToString(), null, GetLocalPosition(i, j) + new Vector2(cellSize, cellSize) * 0.5f, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Left);
                Debug.DrawLine(GetLocalPosition(i, j + 1), GetLocalPosition(i, j), Color.white, 100f);
                Debug.DrawLine(GetLocalPosition(i + 1, j), GetLocalPosition(i, j), Color.white, 100f);
                value++;
                SetValue(i, j, value);
            }
        }
        Debug.DrawLine(GetLocalPosition(0, height), GetLocalPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetLocalPosition(width, 0), GetLocalPosition(width, height), Color.white, 100f);
    }
    public Vector2 GetLocalPosition(int i, int j)
    {
        return new Vector2(i, j) * cellSize + statPosition;
    }
    public void SetValue(int i, int j, int value)
    {
        if (i >= 0 && j >= 0 && i < width && j < height)
        {
            gridArray[i, j] = value;
            textArray[i, j].text = value.ToString();
        }
    }
    public void SetValue(Vector2 localPosition, int value)
    {
        localPosition = (localPosition - statPosition) / cellSize;
        SetValue(Mathf.FloorToInt(localPosition.x), Mathf.FloorToInt(localPosition.y), value);
        Debug.Log(localPosition.x.ToString());
        Debug.Log(localPosition.y.ToString());
    }
    public int GetValue(Vector2 localPosition)
    {
        localPosition = (localPosition - statPosition) / cellSize;
        if (((int)localPosition.x) >= 0 && ((int)localPosition.y) >= 0 && ((int)localPosition.x) < width && ((int)localPosition.y) < height)
        {
            return gridArray[((int)localPosition.x), ((int)localPosition.y)];
        }
        return 0;
    }
}

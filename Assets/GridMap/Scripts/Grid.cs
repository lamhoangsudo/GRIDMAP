using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.EventSystems;
using System;

public class Grid
{
    public const int HEAT_MAP_MAX_VALUE = 100;
    public const int HEAT_MAP_MIN_VAULE = 0;
    public int width { get; private set; }
    public int height { get; private set; }
    private int[,] gridArray;
    private TextMesh[,] textArray;
    public float cellSize { get; private set; }
    private Vector2 statPosition;
    public class OnGridValueChangeEvent : EventArgs
    {
        public int x;
        public int y;
    }
    public event EventHandler<OnGridValueChangeEvent> OnGridChangeValue;

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
    public void GetXY(Vector2 localPosition, out int x, out int y)
    {
        localPosition = (localPosition - statPosition) / cellSize;
        x = Mathf.FloorToInt(localPosition.x);
        y = Mathf.FloorToInt(localPosition.y);
    }
    public void SetValue(int i, int j, int value)
    {
        if (i >= 0 && j >= 0 && i < width && j < height)
        {
            value = Mathf.Clamp(value, HEAT_MAP_MIN_VAULE, HEAT_MAP_MAX_VALUE);
            gridArray[i, j] = value;
            textArray[i, j].text = value.ToString();
            OnGridChangeValue?.Invoke(this, new OnGridValueChangeEvent { x = i, y = j });
        }
    }
    public void SetValue(Vector2 localPosition, int value)
    {
        GetXY(localPosition, out int x, out int y);
        SetValue(x, y, value);
    }
    public int GetValue(Vector2 localPosition)
    {
        GetXY(localPosition, out int x, out int y);
        return GetValue(x, y);
    }
    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        return 0;
    }
    public void AddValue(int x, int y, int value)
    {
        SetValue(x, y, GetValue(x, y) + value);
    }
    public void AddValue(Vector2 localPosition, int fullValueRange, int totalRange, int value)
    {
        int lowerValueAmont = Mathf.RoundToInt((float)value / (totalRange - fullValueRange));
        GetXY(localPosition, out int xCenter, out int yCenter);
        //if (xCenter >= 0 && yCenter >= 0 && xCenter < width && yCenter < height)
        //{
            for (int x = 0; x < totalRange; x++)
            {
                for (int y = 0; y < totalRange - x; y++)
                {
                    int radius = x + y;
                    int addValueAmount = value;
                    if (radius > fullValueRange)
                    {
                        addValueAmount -= lowerValueAmont * (radius - fullValueRange);
                    }
                    AddValue(xCenter + x, yCenter + y, addValueAmount);
                    if (x != 0)
                    {
                        AddValue(xCenter - x, yCenter + y, addValueAmount);
                    }
                    if (y != 0)
                    {
                        AddValue(xCenter + x, yCenter - y, addValueAmount);
                        if (x != 0)
                        {
                            AddValue(xCenter - x, yCenter - y, addValueAmount);
                        }
                    }
                }
            }
        //}
    }
}

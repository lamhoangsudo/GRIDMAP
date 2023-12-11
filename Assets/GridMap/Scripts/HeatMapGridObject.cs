using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GridMap.Scripts
{
    public class HeatMapGridObject
    {
        private const int MAX_HEAT_MAP_VALUE = 100;
        private const int MIN_HEAT_MAP_VALUE = 0;
        private int value;
        private Grid<HeatMapGridObject> grid;
        private int x;
        private int y;

        public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void AddValue(int addValue)
        {
            value += addValue;
            value = Mathf.Clamp(value, MIN_HEAT_MAP_VALUE, MAX_HEAT_MAP_VALUE);
            grid.SetGridObject(x, y, this);
        }
        public float GetValueNormalize()
        {
            return (float)value / MAX_HEAT_MAP_VALUE;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}

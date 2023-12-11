using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GridMap.Scripts
{
    public class StringGridObject
    {
        private string letter, number;
        private Grid<StringGridObject> grid;
        private int x;
        private int y;

        public StringGridObject(Grid<StringGridObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
            letter = "";
            number = "";
        }
        public void AddLetter(string letter)
        {
            this.letter += letter;
            grid.SetGridObject(x,y,this);
        }
        public void AddNumber(string number) 
        {  
            this.number += number;
            grid.SetGridObject(x, y, this);
        }

        public override string ToString()
        {
            return letter + "\n" + number;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Calculations
    {

        public int CalculateDeltaX(Node begin, Node end)
        {
            int deltaX = begin.x - end.x;
            return deltaX;
        }

        public int CalculateDeltaY(Node begin, Node end)
        {
            int deltaY = begin.y - end.y;
            return deltaY;
        }
    }
}

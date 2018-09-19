using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Node
    {
        private char name { get; set; }
        private int x { get; set; }
        private int y { get; set; }
        private int z { get; set; }

        public Node(char name, int x, int y, int z)
        {
            this.name = name;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public char GetName()
        {
            return this.name;
        }
    }
}

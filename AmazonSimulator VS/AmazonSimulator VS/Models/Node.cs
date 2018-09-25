using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Node
    {
        public char name { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public List<Node> nodes { get; set; }

        public Node(char name, int x, int y, int z)
        {
            this.name = name;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void SetNodeConnection(List<Node> nodes)
        {
            this.nodes = nodes;
        }
    }
}

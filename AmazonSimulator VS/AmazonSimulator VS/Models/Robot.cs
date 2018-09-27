using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models
{
    public class Robot : Model3D, IUpdatable
    {
        public List<Node> destinations = new List<Node>();
        public double speed = 1;

        public Robot(double x, double y, double z, double rotationX, double rotationY, double rotationZ) :
            base("robot", x, y, z, rotationX, rotationY, rotationZ){}

        public override void Move(double x, double y, double z)
        {
            base.Move(x, y, z);
        }

        public override bool Update(int tick)
        {
            return base.Update(tick);
        }

        public void AddDestination(Node n)
        {
            destinations.Add(n);
        }

    }
}
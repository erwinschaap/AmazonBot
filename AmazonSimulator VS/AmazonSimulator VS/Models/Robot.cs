using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models
{
    public class Robot : Model3D, IUpdatable
    {
        private double speed = 50;
        private Node destination;
        private Scaffholding hasScaffholding;

        private List<IRobotTask> tasks = new List<IRobotTask>();

        public Robot(double x, double y, double z, double rotationX, double rotationY, double rotationZ) :
            base("robot", x, y, z, rotationX, rotationY, rotationZ)
        { }

        public override void Move(double x, double y, double z)
        {
            if(hasScaffholding != null)
            {
                hasScaffholding.Move(x, y, z);
            }
            base.Move(x, y, z);
        }

        public void MoveOverPath(List<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                MoveTo(node);
            }
        }

        public void MoveTo(Node node)
        {
            this.destination = node;
        }

        public override bool Update(int tick)
        {
            //if(tasks.Count > 0)
            //{
            //   if(tasks.First().TaskComplete(this))
            //    {
            //        tasks.RemoveAt(0);

            //        if(tasks.Count > 0)
            //        {
            //            tasks.First().StartTask(this);
            //        }
            //    }
            //}

            //double currentX = this.x;

            //this.Move(currentX)

            return base.Update(tick);
        }
    }
}
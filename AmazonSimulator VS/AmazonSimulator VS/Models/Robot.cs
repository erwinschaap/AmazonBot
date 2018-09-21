using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models
{
    public class Robot : Model3D, IUpdatable
    {
        public Robot(double x, double y, double z, double rotationX, double rotationY, double rotationZ) :
            base("robot", x, y, z, rotationX, rotationY, rotationZ)
        {
        }

        public override bool update(int tick)
        {
            //

            return base.update(tick);
        }

    }
}